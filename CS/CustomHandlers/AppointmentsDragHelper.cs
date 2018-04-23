using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using DevExpress.Xpf.Core;

public class AppointmentDragHelper : SchedulerMoveEventHandler {
    AppointmentDragInfo dragInfo;
    DateTime prevDate;

    public DateTime newAppointmentDate { get; set; }
    public bool IsExternalDrag { get; set; }

    protected DateTime CurrentDateTime {
        get { return GetCurrentDateTime(); }
    }

    public AppointmentDragHelper(SchedulerControl control)
        : base(control) {
    }

    public override void AttachToControl() {
        control.MouseDown += MouseDownHandler;
        control.AppointmentDrop += AppointmentDropHandler;
        control.AppointmentDrag += AppointmentDragHandler;
    }

    void control_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {

    }

    public override void DetachFromControl() {
        control.MouseDown -= MouseDownHandler;
        control.AppointmentDrop -= AppointmentDropHandler;
        control.AppointmentDrag -= AppointmentDragHandler;
    }

    void MouseDownHandler(object sender, MouseButtonEventArgs e) {
        CreateAppoointmentDragInfo();
    }

    void CreateAppoointmentDragInfo() {
        if(dragInfo == null) {
            if(AppointmentViewInfo != null) {
                IsExternalDrag = false;
                dragInfo = new AppointmentDragInfo() {
                    Appointment = AppointmentViewInfo.Appointment,
                    TimeAtCursor = CurrentDateTime.TimeOfDay,
                    InitialInterval = new TimeInterval(AppointmentViewInfo.Appointment.Start, AppointmentViewInfo.Appointment.End)
                };
            }
            else if(SelectableIntervalViewInfo != null) {
                IsExternalDrag = true;
                dragInfo = new AppointmentDragInfo() {
                    Appointment = null,
                    TimeAtCursor = CurrentDateTime.TimeOfDay,
                    InitialInterval = SelectableIntervalViewInfo.Interval
                };
            }            
        }
    }


    void AppointmentDragHandler(object sender, AppointmentDragEventArgs e) {
        CreateAppoointmentDragInfo();
        OnDragDrop(e);
    }
    void AppointmentDropHandler(object sender, AppointmentDragEventArgs e) {
        OnDragDrop(e);
        dragInfo = null;
    }
    DateTime GetCurrentDateTime() {
        HitTimeCellInfo hitTimeCellInfo = HitTimeCellInfo;
        if(hitTimeCellInfo == null)
            return prevDate;
        prevDate = hitTimeCellInfo.ViewInfo.Interval.Start + hitTimeCellInfo.TimeShift;
        return prevDate;
    }

    void OnDragDrop(AppointmentDragEventArgs e) {
        if(dragInfo == null)
            return;
        if(dragInfo.Appointment == null)
            e.EditedAppointment.Start = GetCurrentDateTime() + dragInfo.InitialInterval.Start.TimeOfDay - dragInfo.TimeAtCursor;
        else
            e.EditedAppointment.Start = GetCurrentDateTime() + e.SourceAppointment.Start.TimeOfDay - dragInfo.TimeAtCursor;

        newAppointmentDate = e.EditedAppointment.Start;
    }
}
