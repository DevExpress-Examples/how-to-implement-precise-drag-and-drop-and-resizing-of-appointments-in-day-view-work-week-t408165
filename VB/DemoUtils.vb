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
Imports System.Data

Namespace SchedulerGridDragDrop
	Public Class DemoUtils
		Public Shared RandomInstance As New Random()

		Private Shared taskDescriptions() As String = { "Implementing Developer Express MasterView control into Accounting System.", "Web Edition: Data Entry Page. The issue with date validation.", "Payables Due Calculator. It is ready for testing.", "Web Edition: Search Page. It is ready for testing.", "Main Menu: Duplicate Items. Somebody has to review all menu items in the system.", "Receivables Calculator. Where can I found the complete specs", "Ledger: Inconsistency. Please fix it.", "Receivables Printing. It is ready for testing.", "Screen Redraw. Somebody has to look at it.", "Email System. What library we are going to use?", "Adding New Vendors Fails. This module doesn't work completely!", "History. Will we track the sales history in our system?", "Main Menu: Add a File menu. File menu is missed!!!", "Currency Mask. The current currency mask in completely inconvinience.", "Drag & Drop. In the schedule module drag & drop is not available.", "Data Import. What competitors databases will we support?", "Reports. The list of incomplete reports.", "Data Archiving. This features is still missed in our application", "Email Attachments. How to add the multiple attachment? I did not find a way to do it.", "Check Register. We are using different paths for different modules.", "Data Export. Our customers asked for export into Excel"}

		Public Shared Function GenerateScheduleTasks() As DataTable
			Dim table As New DataTable()
			table = New DataTable("ScheduleTasks")
			table.Columns.Add("ID", GetType(Integer))
			table.Columns.Add("Subject", GetType(String))
			table.Columns.Add("Severity", GetType(Integer))
			table.Columns.Add("Priority", GetType(Integer))
			table.Columns.Add("Duration", GetType(Integer))
			table.Columns.Add("Description", GetType(String))
			For i As Integer = 0 To 20
				Dim description As String = taskDescriptions(i)
				Dim index As Integer = description.IndexOf("."c)
				Dim subject As String
				If index <= 0 Then
					subject = "task" & Convert.ToInt32(i + 1)
				Else
					subject = description.Substring(0, index)
				End If
				table.Rows.Add(New Object() { i + 1, subject, RandomInstance.Next(3), RandomInstance.Next(3), Math.Max(1, RandomInstance.Next(8)) * 60, description })
			Next i
			Return table
		End Function
	End Class
End Namespace