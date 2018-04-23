' Developer Express Code Central Example:
' How to implement drag and drop between a scheduler and a grid
' 
' This example illustrates how to implement drag and drop for appointments between
' a SchedulerControl and a GridControl. Note that we handle the PreviewMouseDown
' and PreviewMouseMove events of the TableView Class
' (http://documentation.devexpress.com/#WPF/clsDevExpressXpfGridTableViewtopic) to
' initiate drag-and-drop operations for the GridControl (we use
' DragDrop.DoDragDrop Method
' (http://msdn.microsoft.com/en-us/library/system.windows.dragdrop.dodragdrop.aspx)
' for this purpose), whereas this operation is initiated automatically for the
' SchedulerControl (if the OptionsCustomization.AllowAppointmentDrag Property
' (http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerOptionsCustomization_AllowAppointmentDragtopic)
' has its default value). The PreviewDrop event of the TableView and
' SchedulerControl.AppointmentDrop Event
' (http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerSchedulerControl_AppointmentDroptopic)
' are handled to process SchedulerDragData dropped in the GridControl and
' SchedulerControl respectively.
' Note: Rows in the GridControl are dragged via
' the RowIndicatior (see the screenshot in the TableViewHitInfo Class
' (http://documentation.devexpress.com/#WPF/clsDevExpressXpfGridTableViewHitInfotopic)
' help section) in this example.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E3808


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Windows

Namespace SchedulerGridDragDrop
	''' <summary>
	''' Interaction logic for App.xaml
	''' </summary>
	Partial Public Class App
		Inherits Application
	End Class
End Namespace
