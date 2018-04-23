using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Native;

public class AppointmentResizeHelper : SchedulerMoveEventHandler {
    public AppointmentResizeHelper(SchedulerControl control) : base(control) { }

    public override void AttachToControl() {
        control.AppointmentResizing += AppointmentResizeHandler;
        control.AppointmentResized += AppointmentResizeHandler;
    }

    public override void DetachFromControl() {
        control.AppointmentResizing -= AppointmentResizeHandler;
        control.AppointmentResized -= AppointmentResizeHandler;
    }

    protected virtual void AppointmentResizeHandler(object sender, AppointmentResizeEventArgs e) {
        HitTimeCellInfo hitTimeCellInfo = HitTimeCellInfo;
        if (hitTimeCellInfo == null)
            return;
        Rect timeCellBounds = hitTimeCellInfo.Bounds;

        double borderPos = 0d;
        double mousePosition = 0d;

        if(control.ActiveViewType == SchedulerViewType.Timeline) {
            mousePosition = MousePosition.X;
            borderPos = e.ResizedSide == ResizedSide.AtStartTime ? timeCellBounds.X : timeCellBounds.X + timeCellBounds.Width;
        }
        else {
            borderPos = e.ResizedSide == ResizedSide.AtStartTime ? timeCellBounds.Y : timeCellBounds.Y + timeCellBounds.Height;
            mousePosition = MousePosition.Y;
        }

        if(Math.Abs(mousePosition - borderPos) > 1) {
            TimeSpan cellTimeShift = hitTimeCellInfo.TimeShift;
            if(e.ResizedSide == ResizedSide.AtStartTime) {
                if(e.SourceAppointment.End > e.HitInterval.Start + cellTimeShift) {
                    e.EditedAppointment.Start = e.HitInterval.Start + cellTimeShift;
                    e.EditedAppointment.End = e.SourceAppointment.End;
                }
            }
            else
                if(e.HitInterval.Start + cellTimeShift > e.SourceAppointment.Start) {
                    e.EditedAppointment.Start = e.SourceAppointment.Start;
                    e.EditedAppointment.End = e.HitInterval.Start + cellTimeShift;
                }
            e.Handled = true;
        }
    }
}
