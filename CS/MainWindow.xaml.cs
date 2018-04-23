using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.XtraScheduler;

namespace SchedulerGridDragDrop {
    public partial class MainWindow : Window {
        private Point startPoint;
        private bool startDrag = false;
        private int currentRowHandle;
        private bool dragFromGrid = false;

        AppointmentDragHelper dragHandler;
        AppointmentResizeHelper resizeHandler;


        public MainWindow() {
            InitializeComponent();

            gridControl1.ItemsSource = DemoUtils.GenerateScheduleTasks();

            schedulerControl1.ApplyTemplate();
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Day;


            schedulerControl1.DayView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
            schedulerControl1.WorkWeekView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
            schedulerControl1.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;


            dragHandler = new AppointmentDragHelper(schedulerControl1);
            resizeHandler = new AppointmentResizeHelper(schedulerControl1);

            dragHandler.AttachToControl();
            resizeHandler.AttachToControl();

        }

        void tableView1_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            
            this.startPoint = e.GetPosition(null);
            this.startDrag = IsGridRowAvailable(e);
            this.currentRowHandle = gridControl1.View.GetRowHandleByMouseEventArgs(e);
        }

        void tableView1_PreviewMouseMove(object sender, MouseEventArgs e) {
            Point position = e.GetPosition(null);
            
            if (this.startDrag && e.LeftButton == MouseButtonState.Pressed && IsGridRowAvailable(e) && (Math.Abs(position.X - this.startPoint.X) > 1 || Math.Abs(position.Y - this.startPoint.Y) > 1)) {
                this.startDrag = false;
                this.dragFromGrid = true;

                DragDrop.DoDragDrop(gridControl1, ObtainSchedulerDataFromRow(currentRowHandle), DragDropEffects.Move);
            }

            this.startPoint = e.GetPosition(null);
        }

        void tableView1_PreviewDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(SchedulerDragData))) {
                SchedulerDragData schedulerData = ((SchedulerDragData)e.Data.GetData(typeof(SchedulerDragData)));
                DataRow dataRow = ObtainDataRowFromSchedulerData(schedulerData);
                DataTable dataTable = (DataTable)gridControl1.ItemsSource;
                TableViewHitInfo hitInfo = tableView1.CalcHitInfo(e.OriginalSource as DependencyObject);
                int rowIndex = dataTable.Rows.Count;

                if (hitInfo.RowHandle != GridControl.InvalidRowHandle)
                    rowIndex = gridControl1.GetListIndexByRowHandle(hitInfo.RowHandle);
                
                if (this.dragFromGrid)
                    tableView1.DeleteRow(currentRowHandle);

                dataTable.Rows.InsertAt(dataRow, rowIndex);
                schedulerData.PrimaryAppointment.Delete();
            }
        }

        void schedulerControl1_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            this.dragFromGrid = false;
        }

        void schedulerControl1_AppointmentDrop(object sender, AppointmentDragEventArgs e) {
            string createEventMsg = "Creating an event at {0} on {1}.";
            string moveEventMsg = "Moving the event from {0} on {1} to {2} on {3}.";

            DateTime srcStart = e.SourceAppointment.Start;
            if(dragHandler.IsExternalDrag) {
                e.EditedAppointment.Start = dragHandler.newAppointmentDate;
            }
            DateTime newStart = e.EditedAppointment.Start;

            string msg = (srcStart == DateTime.MinValue) ? String.Format(createEventMsg, newStart.ToShortTimeString(), newStart.ToShortDateString()) :
                String.Format(moveEventMsg, srcStart.ToShortTimeString(), srcStart.ToShortDateString(), newStart.ToShortTimeString(), newStart.ToShortDateString());

            MessageBoxResult mbResult = DXMessageBox.Show(msg + "\r\nProceed?", "Demo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            schedulerControl1.Focus();

            if (mbResult == MessageBoxResult.Yes) {
                if (!this.dragFromGrid)
                    return;

                schedulerControl1.Storage.AppointmentStorage.Add(e.EditedAppointment.Copy());
                tableView1.DeleteRow(currentRowHandle);
            }

            e.Allow = false;
            
        }

        private object ObtainSchedulerDataFromRow(int rowHandle) {
            AppointmentBaseCollection appointments = new AppointmentBaseCollection();
            Appointment apt = schedulerControl1.Storage.CreateAppointment(AppointmentType.Normal);
            apt.Subject = (string)ObtainDataFromRow(rowHandle, "Subject");
            apt.LabelKey = (int)ObtainDataFromRow(rowHandle, "Severity");
            apt.StatusKey = (int)ObtainDataFromRow(rowHandle, "Priority");
            apt.Duration = TimeSpan.FromMinutes((int)ObtainDataFromRow(rowHandle, "Duration"));
            apt.Description = (string)ObtainDataFromRow(rowHandle, "Description");
            appointments.Add(apt);
            return new SchedulerDragData(appointments, 0);
        }

        private DataRow ObtainDataRowFromSchedulerData(SchedulerDragData schedulerData) {
            Appointment apt = schedulerData.PrimaryAppointment;
            DataTable dataTable = (DataTable)gridControl1.ItemsSource;
            DataRow dataRow = dataTable.NewRow();

            dataRow["Subject"] = apt.Subject;
            dataRow["Severity"] = apt.LabelKey;
            dataRow["Priority"] = apt.StatusKey;
            dataRow["Duration"] = (int)apt.Duration.TotalMinutes;
            dataRow["Description"] = apt.Description;

            return dataRow;
        }

        private object ObtainDataFromRow(int rowHandle, string columnName) {
            return gridControl1.GetCellValue(rowHandle, gridControl1.Columns[columnName]);
        }

        private bool IsGridRowAvailable(MouseEventArgs e) {
            int rowHandle = gridControl1.View.GetRowHandleByMouseEventArgs(e);
            TableViewHitInfo hitInfo = tableView1.CalcHitInfo(e.OriginalSource as DependencyObject);

            return gridControl1.GetRow(rowHandle) != null && hitInfo.HitTest == TableViewHitTest.RowIndicator;
        }
    }
}