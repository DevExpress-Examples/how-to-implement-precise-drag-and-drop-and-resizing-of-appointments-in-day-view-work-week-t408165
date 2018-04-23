Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Media
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.XtraScheduler.Drawing
Imports DevExpress.XtraScheduler.Native

Public Class HitTimeCellInfo
	Public Shared Function CreateHitTimeCellInfo(ByVal control As SchedulerControl, ByVal mousePosition As Point) As HitTimeCellInfo
		Dim hitInfo As SchedulerControlHitInfo = control.ActiveView.CalcHitInfo(mousePosition)
		Dim cellHitInfo As SchedulerHitInfo = TryCast(hitInfo.FindHitInfo(SchedulerHitTest.Cell), SchedulerHitInfo)
		If cellHitInfo Is Nothing Then
			Return Nothing
		End If
		Dim cell As UIElement = cellHitInfo.VisualHit
		If cell Is Nothing Then
			Return Nothing
		End If

		'--First method
		'GeneralTransform positionTransform = cell.TransformToAncestor(control);
		'Point areaPosition = positionTransform.Transform(new Point());

		'--Second method
		Dim areaPosition As Point = control.PointFromScreen(cell.PointToScreen(New Point()))
		Dim timeCellBounds As New Rect(areaPosition, cell.RenderSize)

		Dim percent As Double = 0R

        If control.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Timeline Then
            percent = If((timeCellBounds.Width <> 0), CDbl(mousePosition.X - timeCellBounds.X) / CDbl(timeCellBounds.Width), 0)
        Else
            percent = If((timeCellBounds.Height <> 0), CDbl(mousePosition.Y - timeCellBounds.Y) / CDbl(timeCellBounds.Height), 0)
        End If

		Dim timeShift As TimeSpan = TimeSpan.FromMinutes(cellHitInfo.ViewInfo.Interval.Duration.TotalMinutes * percent)
		Return New HitTimeCellInfo(cellHitInfo.ViewInfo, timeShift, timeCellBounds)
	End Function

	Private ReadOnly viewInfo_Renamed As ISelectableIntervalViewInfo
	Private ReadOnly timeShift_Renamed As TimeSpan
	Private ReadOnly bounds_Renamed As Rect

	Public ReadOnly Property ViewInfo() As ISelectableIntervalViewInfo
		Get
			Return viewInfo_Renamed
		End Get
	End Property
	Public ReadOnly Property TimeShift() As TimeSpan
		Get
			Return timeShift_Renamed
		End Get
	End Property
	Public ReadOnly Property Bounds() As Rect
		Get
			Return bounds_Renamed
		End Get
	End Property

	Private Sub New(ByVal viewInfo As ISelectableIntervalViewInfo, ByVal timeShift As TimeSpan, ByVal bounds As Rect)
		Me.viewInfo_Renamed = viewInfo
		Me.timeShift_Renamed = timeShift
		Me.bounds_Renamed = bounds
	End Sub
End Class