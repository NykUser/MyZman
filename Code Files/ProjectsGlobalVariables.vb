'needs to be Module it should be global
'class dose not work even with imports GlobalVariables on top of each code file, or in the project's Properties window 
'not as said in https://stackoverflow.com/questions/4529862/declare-global-variables-in-visual-studio-2010-and-vb-net

Module ProjectsGlobalVariables

    Public varSC As SettingsCollection = New SettingsCollection
    Public varCZC As ComplexZmanimCalendar
    Public varEngPlaceList As LocationCollection = New LocationCollection 'As New List(Of aLocation)
    Public varHebPlaceList As LocationCollection = New LocationCollection 'As New List(Of aLocation)

    Public varZmanimFunc As New List(Of String)
    Public varFinishedLoading As Boolean = False
    Public varZmanTimeZone As TimeZoneInfo
    Public varAddedGets As New AddedGets
    Public varSavedInputLanguage As InputLanguage
    Public varUserFile As String = Environment.CurrentDirectory & "\MYZman.exe.UserSettings.xml" 'Environment.CurrentDirectory not good when it runs from task scheduler it will be C:\Windows\System32\Tasks
    Public varLocationsFile As String = Environment.CurrentDirectory & "\LocationsList.xml"
    Public varSelectedIndexBeforChange As Integer = -1
    Public varString As String
    Public varSavedStatusLabel As String
    'Public varTransparencyBox As New ToolStripMenuTextBoxAndLabel("Opacity: ", 40, 0)
    Public varMouseEnter As Boolean = False
    Public varDataGridCombo As ComboBox
    Public varDataGridNumColumn As Integer
    Public varDataGridNumRow As Integer = 0
    Public varGeoLocation As GeoLocation

    Public varHC As New HebrewCalendar()
    Public varHculture As CultureInfo = CultureInfo.CreateSpecificCulture("he-IL")
    Public varEC As New GregorianCalendar()
    Public varEculture As CultureInfo = CultureInfo.CreateSpecificCulture("en-US")
End Module
