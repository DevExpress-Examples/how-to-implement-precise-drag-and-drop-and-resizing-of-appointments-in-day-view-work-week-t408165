Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Forms
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Native

Public Class AppointmentResizeHelper
	Inherits SchedulerMoveEventHandler
	Public Sub New(ByVal control As SchedulerControl)
		MyBase.New(control)
	End Sub

	Public Overrides Sub AttachToControl()
		AddHandler control.AppointmentResizing, AddressOf AppointmentResizeHandler
		AddHandler control.AppointmentResized, AddressOf AppointmentResizeHandler
	End Sub

	Public Overrides Sub DetachFromControl()
		RemoveHandler control.AppointmentResizing, AddressOf AppointmentResizeHandler
		RemoveHandler control.AppointmentResized, AddressOf AppointmentResizeHandler
	End Sub

	Protected Overridable Sub AppointmentResizeHandler(ByVal sender As Object, ByVal e As AppointmentResizeEventArgs)
        Dim hitTimeCellInfo As HitTimeCellInfo = Me.HitTimeCellInfo
		If hitTimeCellInfo Is Nothing Then
			Return
		End If
		Dim timeCellBounds As Rect = hitTimeCellInfo.Bounds

		Dim borderPos As Double = 0R
		Dim mousePosition As Double = 0R

        If control.ActiveViewType = SchedulerViewType.Timeline Then
            mousePosition = Me.MousePosition.X
            borderPos = If(e.ResizedSide = ResizedSide.AtStartTime, timeCellBounds.X, timeCellBounds.X + timeCellBounds.Width)
        Else
            borderPos = If(e.ResizedSide = ResizedSide.AtStartTime, timeCellBounds.Y, timeCellBounds.Y + timeCellBounds.Height)
            mousePosition = Me.MousePosition.Y
        End If

		If Math.Abs(mousePosition - borderPos) > 1 Then
			Dim cellTimeShift As TimeSpan = hitTimeCellInfo.TimeShift
			If e.ResizedSide = ResizedSide.AtStartTime Then
				If e.SourceAppointment.End > e.HitInterval.Start + cellTimeShift Then
					e.EditedAppointment.Start = e.HitInterval.Start + cellTimeShift
					e.EditedAppointment.End = e.SourceAppointment.End
				End If
			Else
				If e.HitInterval.Start + cellTimeShift > e.SourceAppointment.Start Then
					e.EditedAppointment.Start = e.SourceAppointment.Start
					e.EditedAppointment.End = e.HitInterval.Start + cellTimeShift
				End If
			End If
			e.Handled = True
		End If
	End Sub
End Class
