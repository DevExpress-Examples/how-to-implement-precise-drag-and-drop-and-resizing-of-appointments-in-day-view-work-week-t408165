Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.XtraScheduler.Native
Imports DevExpress.Xpf.Scheduler.Drawing
Imports System.Windows
Imports System.Windows.Media

Public MustInherit Class SchedulerMoveEventHandler
	Protected control As SchedulerControl

	Protected ReadOnly Property MousePosition() As System.Windows.Point
		Get
			Dim currentPosition As System.Drawing.Point = Form.MousePosition
			Return control.PointFromScreen(New System.Windows.Point(currentPosition.X, currentPosition.Y))
		End Get
	End Property
	Protected ReadOnly Property HitTimeCellInfo() As HitTimeCellInfo
		Get
			Return HitTimeCellInfo.CreateHitTimeCellInfo(control, MousePosition)
		End Get
	End Property

	Protected ReadOnly Property AppointmentViewInfo() As IAppointmentView
		Get
			Return GetAppointmentViewInfo()
		End Get
	End Property

	Protected ReadOnly Property SelectableIntervalViewInfo() As ISelectableIntervalViewInfo
		Get
			Return GetSelectableIntervalViewInfo()
		End Get
	End Property

	Public Overridable Sub AttachToControl()
	End Sub
	Public Overridable Sub DetachFromControl()
	End Sub

	Protected Sub New(ByVal control As SchedulerControl)
		Me.control = control
	End Sub

	Protected Function GetAppointmentViewInfo() As IAppointmentView
		Dim hitInfo As SchedulerControlHitInfo = control.ActiveView.CalcHitInfo(MousePosition)
		Return If((hitInfo.HitTest = SchedulerHitTest.AppointmentContent), CType(hitInfo.ViewInfo, IAppointmentView), Nothing)
	End Function

	Protected Function GetSelectableIntervalViewInfo() As ISelectableIntervalViewInfo
		Dim hitInfo As SchedulerControlHitInfo = control.ActiveView.CalcHitInfo(MousePosition)
		Return If((hitInfo.HitTest = SchedulerHitTest.Cell), CType(hitInfo.ViewInfo, ISelectableIntervalViewInfo), Nothing)
	End Function
End Class