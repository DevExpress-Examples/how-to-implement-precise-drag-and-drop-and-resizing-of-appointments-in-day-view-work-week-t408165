using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler.Native;
using DevExpress.Xpf.Scheduler.Drawing;
using System.Windows;
using System.Windows.Media;

public abstract class SchedulerMoveEventHandler {
    protected SchedulerControl control;

    protected System.Windows.Point MousePosition {
        get { 
            System.Drawing.Point currentPosition = Form.MousePosition;
            return control.PointFromScreen(new System.Windows.Point(currentPosition.X, currentPosition.Y)); 
        }
    }
    protected HitTimeCellInfo HitTimeCellInfo {
        get { return HitTimeCellInfo.CreateHitTimeCellInfo(control, MousePosition); }
    }

    protected IAppointmentView  AppointmentViewInfo {
        get { return GetAppointmentViewInfo(); }
    }

    protected ISelectableIntervalViewInfo SelectableIntervalViewInfo {
        get { return GetSelectableIntervalViewInfo(); }
    }

    public virtual void AttachToControl() { }
    public virtual void DetachFromControl() { }

    protected SchedulerMoveEventHandler(SchedulerControl control) {
        this.control = control;
    }
    
    protected IAppointmentView  GetAppointmentViewInfo() {
        SchedulerControlHitInfo hitInfo = control.ActiveView.CalcHitInfo(MousePosition);
        return (hitInfo.HitTest == SchedulerHitTest.AppointmentContent) ? (IAppointmentView )hitInfo.ViewInfo : null;
    }

    protected ISelectableIntervalViewInfo GetSelectableIntervalViewInfo() {
        SchedulerControlHitInfo hitInfo = control.ActiveView.CalcHitInfo(MousePosition);
        return (hitInfo.HitTest == SchedulerHitTest.Cell) ? (ISelectableIntervalViewInfo)hitInfo.ViewInfo : null;
    }
}