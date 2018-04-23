Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Windows
Imports System.Windows.Input
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Grid
Imports DevExpress.XtraScheduler

Namespace SchedulerGridDragDrop
	Partial Public Class MainWindow
		Inherits Window
		Private startPoint As Point
		Private startDrag As Boolean = False
		Private currentRowHandle As Integer
		Private dragFromGrid As Boolean = False

		Private dragHandler As AppointmentDragHelper
		Private resizeHandler As AppointmentResizeHelper


		Public Sub New()
			InitializeComponent()

			gridControl1.ItemsSource = DemoUtils.GenerateScheduleTasks()

			schedulerControl1.ApplyTemplate()
			schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Day


			schedulerControl1.DayView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never
			schedulerControl1.WorkWeekView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never
			schedulerControl1.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never


			dragHandler = New AppointmentDragHelper(schedulerControl1)
			resizeHandler = New AppointmentResizeHelper(schedulerControl1)

			dragHandler.AttachToControl()
			resizeHandler.AttachToControl()

		End Sub

		Private Sub tableView1_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			If e.LeftButton <> MouseButtonState.Pressed Then
				Return
			End If

			Me.startPoint = e.GetPosition(Nothing)
			Me.startDrag = IsGridRowAvailable(e)
			Me.currentRowHandle = gridControl1.View.GetRowHandleByMouseEventArgs(e)
		End Sub

		Private Sub tableView1_PreviewMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim position As Point = e.GetPosition(Nothing)

			If Me.startDrag AndAlso e.LeftButton = MouseButtonState.Pressed AndAlso IsGridRowAvailable(e) AndAlso (Math.Abs(position.X - Me.startPoint.X) > 1 OrElse Math.Abs(position.Y - Me.startPoint.Y) > 1) Then
				Me.startDrag = False
				Me.dragFromGrid = True

				DragDrop.DoDragDrop(gridControl1, ObtainSchedulerDataFromRow(currentRowHandle), DragDropEffects.Move)
			End If

			Me.startPoint = e.GetPosition(Nothing)
		End Sub

		Private Sub tableView1_PreviewDrop(ByVal sender As Object, ByVal e As DragEventArgs)
			If e.Data.GetDataPresent(GetType(SchedulerDragData)) Then
				Dim schedulerData As SchedulerDragData = (CType(e.Data.GetData(GetType(SchedulerDragData)), SchedulerDragData))
				Dim dataRow As DataRow = ObtainDataRowFromSchedulerData(schedulerData)
				Dim dataTable As DataTable = CType(gridControl1.ItemsSource, DataTable)
				Dim hitInfo As TableViewHitInfo = tableView1.CalcHitInfo(TryCast(e.OriginalSource, DependencyObject))
				Dim rowIndex As Integer = dataTable.Rows.Count

				If hitInfo.RowHandle <> GridControl.InvalidRowHandle Then
					rowIndex = gridControl1.GetListIndexByRowHandle(hitInfo.RowHandle)
				End If

				If Me.dragFromGrid Then
					tableView1.DeleteRow(currentRowHandle)
				End If

				dataTable.Rows.InsertAt(dataRow, rowIndex)
				schedulerData.PrimaryAppointment.Delete()
			End If
		End Sub

		Private Sub schedulerControl1_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			Me.dragFromGrid = False
		End Sub

		Private Sub schedulerControl1_AppointmentDrop(ByVal sender As Object, ByVal e As AppointmentDragEventArgs)
			Dim createEventMsg As String = "Creating an event at {0} on {1}."
			Dim moveEventMsg As String = "Moving the event from {0} on {1} to {2} on {3}."

			Dim srcStart As DateTime = e.SourceAppointment.Start
			If dragHandler.IsExternalDrag Then
				e.EditedAppointment.Start = dragHandler.newAppointmentDate
			End If
			Dim newStart As DateTime = e.EditedAppointment.Start

			Dim msg As String = If((srcStart = DateTime.MinValue), String.Format(createEventMsg, newStart.ToShortTimeString(), newStart.ToShortDateString()), String.Format(moveEventMsg, srcStart.ToShortTimeString(), srcStart.ToShortDateString(), newStart.ToShortTimeString(), newStart.ToShortDateString()))

			Dim mbResult As MessageBoxResult = DXMessageBox.Show(msg & Constants.vbCrLf & "Proceed?", "Demo", MessageBoxButton.YesNo, MessageBoxImage.Question)

			schedulerControl1.Focus()

			If mbResult = MessageBoxResult.Yes Then
				If (Not Me.dragFromGrid) Then
					Return
				End If

				schedulerControl1.Storage.AppointmentStorage.Add(e.EditedAppointment.Copy())
				tableView1.DeleteRow(currentRowHandle)
			End If

			e.Allow = False

		End Sub

		Private Function ObtainSchedulerDataFromRow(ByVal rowHandle As Integer) As Object
			Dim appointments As New AppointmentBaseCollection()
			Dim apt As Appointment = schedulerControl1.Storage.CreateAppointment(AppointmentType.Normal)
			apt.Subject = CStr(ObtainDataFromRow(rowHandle, "Subject"))
			apt.LabelKey = CInt(Fix(ObtainDataFromRow(rowHandle, "Severity")))
			apt.StatusKey = CInt(Fix(ObtainDataFromRow(rowHandle, "Priority")))
			apt.Duration = TimeSpan.FromMinutes(CInt(Fix(ObtainDataFromRow(rowHandle, "Duration"))))
			apt.Description = CStr(ObtainDataFromRow(rowHandle, "Description"))
			appointments.Add(apt)
			Return New SchedulerDragData(appointments, 0)
		End Function

		Private Function ObtainDataRowFromSchedulerData(ByVal schedulerData As SchedulerDragData) As DataRow
			Dim apt As Appointment = schedulerData.PrimaryAppointment
			Dim dataTable As DataTable = CType(gridControl1.ItemsSource, DataTable)
			Dim dataRow As DataRow = dataTable.NewRow()

			dataRow("Subject") = apt.Subject
			dataRow("Severity") = apt.LabelKey
			dataRow("Priority") = apt.StatusKey
			dataRow("Duration") = CInt(Fix(apt.Duration.TotalMinutes))
			dataRow("Description") = apt.Description

			Return dataRow
		End Function

		Private Function ObtainDataFromRow(ByVal rowHandle As Integer, ByVal columnName As String) As Object
			Return gridControl1.GetCellValue(rowHandle, gridControl1.Columns(columnName))
		End Function

		Private Function IsGridRowAvailable(ByVal e As MouseEventArgs) As Boolean
			Dim rowHandle As Integer = gridControl1.View.GetRowHandleByMouseEventArgs(e)
			Dim hitInfo As TableViewHitInfo = tableView1.CalcHitInfo(TryCast(e.OriginalSource, DependencyObject))

			Return gridControl1.GetRow(rowHandle) IsNot Nothing AndAlso hitInfo.HitTest = TableViewHitTest.RowIndicator
		End Function
	End Class
End Namespace