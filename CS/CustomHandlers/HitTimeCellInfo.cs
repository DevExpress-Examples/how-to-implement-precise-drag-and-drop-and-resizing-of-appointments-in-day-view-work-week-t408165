using System;
using System.Windows;
using System.Windows.Media;
using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler.Drawing;
using DevExpress.XtraScheduler.Native;

public class HitTimeCellInfo { 
    public static HitTimeCellInfo CreateHitTimeCellInfo(SchedulerControl control, Point mousePosition) {

        SchedulerControlHitInfo hitInfo = control.ActiveView.CalcHitInfo(mousePosition);
        SchedulerHitInfo cellHitInfo = hitInfo.FindHitInfo(SchedulerHitTest.Cell) as SchedulerHitInfo;
        if (cellHitInfo == null)
            return null;
        UIElement cell = cellHitInfo.VisualHit;
        if (cell == null)
            return null;

        //--First method
        //GeneralTransform positionTransform = cell.TransformToAncestor(control);
        //Point areaPosition = positionTransform.Transform(new Point());

        //--Second method
        Point areaPosition = control.PointFromScreen(cell.PointToScreen(new Point()));
        Rect timeCellBounds = new Rect(areaPosition, cell.RenderSize);

        double percent = 0d;

        if(control.ActiveViewType == DevExpress.XtraScheduler.SchedulerViewType.Timeline) {
            percent = (timeCellBounds.Width != 0) ? (double)(mousePosition.X - timeCellBounds.X) / (double)timeCellBounds.Width : 0;
        }
        else {
            percent = (timeCellBounds.Height != 0) ? (double)(mousePosition.Y - timeCellBounds.Y) / (double)timeCellBounds.Height : 0;
        }

        TimeSpan timeShift = TimeSpan.FromMinutes(cellHitInfo.ViewInfo.Interval.Duration.TotalMinutes * percent);
        return new HitTimeCellInfo(cellHitInfo.ViewInfo, timeShift, timeCellBounds);
    }

    readonly ISelectableIntervalViewInfo viewInfo;
    readonly TimeSpan timeShift;
    readonly Rect bounds;

    public ISelectableIntervalViewInfo ViewInfo { get { return viewInfo; } }
    public TimeSpan TimeShift { get { return timeShift; } }
    public Rect Bounds { get { return bounds; } }

    HitTimeCellInfo(ISelectableIntervalViewInfo viewInfo, TimeSpan timeShift, Rect bounds) { 
        this.viewInfo = viewInfo;
        this.timeShift = timeShift;
        this.bounds = bounds;
    }
}