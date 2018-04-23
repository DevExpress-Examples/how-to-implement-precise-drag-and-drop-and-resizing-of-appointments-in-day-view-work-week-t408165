Imports Microsoft.VisualBasic
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Windows.Input
Imports System.Windows
Imports DevExpress.Xpf.Core

Public Class AppointmentDragHelper
	Inherits SchedulerMoveEventHandler
	Private dragInfo As AppointmentDragInfo
	Private prevDate As DateTime

	Public Property newAppointmentDate() As DateTime
	Public Property IsExternalDrag() As Boolean

	Protected ReadOnly Property CurrentDateTime() As DateTime
		Get
			Return GetCurrentDateTime()
		End Get
	End Property

	Public Sub New(ByVal control As SchedulerControl)
		MyBase.New(control)
	End Sub

	Public Overrides Sub AttachToControl()
		AddHandler control.MouseDown, AddressOf MouseDownHandler
		AddHandler control.AppointmentDrop, AddressOf AppointmentDropHandler
		AddHandler control.AppointmentDrag, AddressOf AppointmentDragHandler
	End Sub

	Private Sub control_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)

	End Sub

	Public Overrides Sub DetachFromControl()
		RemoveHandler control.MouseDown, AddressOf MouseDownHandler
		RemoveHandler control.AppointmentDrop, AddressOf AppointmentDropHandler
		RemoveHandler control.AppointmentDrag, AddressOf AppointmentDragHandler
	End Sub

	Private Sub MouseDownHandler(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
		CreateAppoointmentDragInfo()
	End Sub

	Private Sub CreateAppoointmentDragInfo()
		If dragInfo Is Nothing Then
			If AppointmentViewInfo IsNot Nothing Then
				IsExternalDrag = False
				dragInfo = New AppointmentDragInfo() With {.Appointment = AppointmentViewInfo.Appointment, .TimeAtCursor = CurrentDateTime.TimeOfDay, .InitialInterval = New TimeInterval(AppointmentViewInfo.Appointment.Start, AppointmentViewInfo.Appointment.End)}
			ElseIf SelectableIntervalViewInfo IsNot Nothing Then
				IsExternalDrag = True
				dragInfo = New AppointmentDragInfo() With {.Appointment = Nothing, .TimeAtCursor = CurrentDateTime.TimeOfDay, .InitialInterval = SelectableIntervalViewInfo.Interval}
			End If
		End If
	End Sub


	Private Sub AppointmentDragHandler(ByVal sender As Object, ByVal e As AppointmentDragEventArgs)
		CreateAppoointmentDragInfo()
		OnDragDrop(e)
	End Sub
	Private Sub AppointmentDropHandler(ByVal sender As Object, ByVal e As AppointmentDragEventArgs)
		OnDragDrop(e)
		dragInfo = Nothing
	End Sub
	Private Function GetCurrentDateTime() As DateTime
        Dim hitTimeCellInfo As HitTimeCellInfo = Me.HitTimeCellInfo
        If hitTimeCellInfo Is Nothing Then
            Return prevDate
        End If
		prevDate = hitTimeCellInfo.ViewInfo.Interval.Start + hitTimeCellInfo.TimeShift
		Return prevDate
	End Function

	Private Sub OnDragDrop(ByVal e As AppointmentDragEventArgs)
		If dragInfo Is Nothing Then
			Return
		End If
		If dragInfo.Appointment Is Nothing Then
			e.EditedAppointment.Start = GetCurrentDateTime() + dragInfo.InitialInterval.Start.TimeOfDay - dragInfo.TimeAtCursor
		Else
			e.EditedAppointment.Start = GetCurrentDateTime() + e.SourceAppointment.Start.TimeOfDay - dragInfo.TimeAtCursor
		End If

		newAppointmentDate = e.EditedAppointment.Start
	End Sub
End Class
