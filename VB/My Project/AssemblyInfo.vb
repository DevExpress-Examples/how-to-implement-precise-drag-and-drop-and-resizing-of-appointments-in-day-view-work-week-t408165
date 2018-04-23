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
Imports System.Reflection
Imports System.Resources
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Windows

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.
<Assembly: AssemblyTitle("SchedulerGridDragDrop")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyConfiguration("")>
<Assembly: AssemblyCompany("DevExpress")>
<Assembly: AssemblyProduct("SchedulerGridDragDrop")>
<Assembly: AssemblyCopyright("Copyright © DevExpress 2011")>
<Assembly: AssemblyTrademark("")>
<Assembly: AssemblyCulture("")>

' Setting ComVisible to false makes the types in this assembly not visible 
' to COM components.  If you need to access a type in this assembly from 
' COM, set the ComVisible attribute to true on that type.
<Assembly: ComVisible(False)>

'In order to begin building localizable applications, set 
'<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
'inside a <PropertyGroup>.  For example, if you are using US english
'in your source files, set the <UICulture> to en-US.  Then uncomment
'the NeutralResourceLanguage attribute below.  Update the "en-US" in
'the line below to match the UICulture setting in the project file.

'[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


	'(used if a resource is not found in the page, 
	' or application resource dictionaries)
	'(used if a resource is not found in the page, 
	' app, or any theme specific resource dictionaries)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>


' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' [assembly: AssemblyVersion("1.0.*")]
<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>
