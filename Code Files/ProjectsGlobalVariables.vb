'Needs to be a Module for it to be global
'class dose not work even with imports GlobalVariables on top of each code file, or in the project's Properties window 
'Not as said here https://stackoverflow.com/questions/4529862/declare-global-variables-in-visual-studio-2010-and-vb-net

Module ProjectsGlobalVariables

    Public Property varSC As SettingsCollection = New SettingsCollection
    Public Property varCZC As ComplexZmanimCalendar
    Public Property varEngPlaceList As LocationCollection = New LocationCollection 'As New List(Of aLocation)
    Public Property varHebPlaceList As LocationCollection = New LocationCollection 'As New List(Of aLocation)

    Public Property varZmanimFunc As New List(Of String)
    Public Property varFinishedLoading As Boolean = False
    Public Property varZmanTimeZone As TimeZoneInfo
    Public Property varAddedGets As New AddedGets
    Public Property varSavedInputLanguage As InputLanguage
    Public Property varUserFile As String = Environment.CurrentDirectory & "\MYZman.exe.UserSettings.xml"
    Public Property varLocationsFile As String = Environment.CurrentDirectory & "\LocationsList.xml"
    Public Property varSelectedIndexBeforChange As Integer = -1
    Public Property varSavedStatusLabel As String
    Public Property varMouseEnter As Boolean = False
    Public Property varDataGridCombo As ComboBox
    Public Property varDataGridNumColumn As Integer
    Public Property varDataGridNumRow As Integer = 0
    Public Property varGeoLocation As GeoLocation
    Public Property varLocationSecondTry As Boolean = False

    Public Property varHC As New HebrewCalendar()
    Public Property varHculture As CultureInfo = CultureInfo.CreateSpecificCulture("he-IL")
End Module
