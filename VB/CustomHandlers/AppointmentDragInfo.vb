Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraScheduler

Public Class AppointmentDragInfo
	Public Sub New()
	End Sub

	Public Property Appointment() As Appointment
	Public Property TimeAtCursor() As TimeSpan

	Public Property InitialInterval() As TimeInterval
End Class