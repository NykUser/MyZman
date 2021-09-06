Public Class SettingsCollection
    Inherits SerializableData

    Public Property LastSelectedIndex As Integer = 0
    Public Property PlaceListInHebrew As Boolean = False
    Public Property IsraeliYomTov As Boolean = False
    Public Property DefaultType As String = "Last"
    Public Property DefaultSelectedindex As Integer = 1
    Public Property DefaultName As String = ""
    Public Property DefaultPlaceListInHebrew As Boolean = False
    Public Property ColorZman As Boolean = True
    Public Property StayOnTop As Boolean = False
    Public Property UseOlderUsnoAlgorithm As Boolean = False
    Public Property CalculateElevation As Boolean = False
    Public Property Clock24Hour As Boolean = False
    Public Property ChangeKeybordLayout As Boolean = True
    Public Property AskWhenChanging As Boolean = True
    Public Property BackUpWhenChanging As Boolean = True
    Public Property ShowTimesOnStatusBar As Boolean = True
    Public Property ShowTooltips As Boolean = True
    Public Property DisplayDafYomi As Boolean = True
    Public Property LineBetweenZmanim As Boolean = False
    Public Property TransparencyValue As Double = 1
    Public Property LocationY As Integer = -1
    Public Property LocationX As Integer = -1
    Public Property SizeW As Integer = -1
    Public Property SizeH As Integer = -1
    Public Property DataGridSizeH As Integer = -1
    Public Property DataGridCol1W As Integer = -1
    Public Property DataGridCol2W As Integer = -1
    Public Property HebrewMenus As Boolean = False
    Public Property MenuLanguageWasChanged As Boolean = False
    Public Property SchedulerLocation As String
    Public Property SchedulerLatitude As String
    Public Property SchedulerLongitude As String
    Public Property SchedulerElevation As String
    Public Property SchedulerTimeZone As String
    Public Property HideLocationBox As Boolean = False

    Private oZmanim As ZmanimCollection = New ZmanimCollection
    Private oLocation As LocationCollection = New LocationCollection
    Private oSchedule As ScheduleCollection = New ScheduleCollection
    Public ReadOnly Property Zmanim As ZmanimCollection
        Get
            Return oZmanim
        End Get
    End Property
    Public ReadOnly Property Location As LocationCollection
        Get
            Return oLocation
        End Get
    End Property
    Public ReadOnly Property Schedule As ScheduleCollection
        Get
            Return oSchedule
        End Get
    End Property
End Class


Partial Public Class SettingsCollection

    Public Class ZmanimCollection
        Implements IEnumerable(Of aZman)
        Public Items As New List(Of aZman)

        Public Property Item(ByVal index As Integer) As aZman
            Get
                Return CType(Me(index), aZman)
            End Get
            Set(ByVal value As aZman)
                Items(index) = value
            End Set
        End Property
        Public Sub Add(ByVal ozman As aZman)
            Items.Add(New aZman(ozman))
        End Sub
        Public Function GetEnumerator() As IEnumerator(Of aZman) Implements IEnumerable(Of aZman).GetEnumerator
            Return Items.GetEnumerator()
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function
    End Class

    Public Class LocationCollection
        Implements IEnumerable(Of aLocation)
        Public Items As New List(Of aLocation)

        Public Property Item(ByVal index As Integer) As aLocation
            Get
                Return CType(Me(index), aLocation)
            End Get
            Set(ByVal value As aLocation)
                Items(index) = value
            End Set
        End Property
        Public Sub Add(ByVal olocation As aLocation)
            Items.Add(New aLocation(olocation))
        End Sub
        Public Function GetEnumerator() As IEnumerator(Of aLocation) Implements IEnumerable(Of aLocation).GetEnumerator
            Return Items.GetEnumerator()
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function
    End Class

    Public Class ScheduleCollection
        Implements IEnumerable(Of aSchedule)
        Public Items As New List(Of aSchedule)

        Public Property Item(ByVal index As Integer) As aSchedule
            Get
                Return CType(Me(index), aSchedule)
            End Get
            Set(ByVal value As aSchedule)
                Items(index) = value
            End Set
        End Property
        Public Sub Add(ByVal oSchedule As aSchedule)
            Items.Add(New aSchedule(oSchedule))
        End Sub
        Public Function GetEnumerator() As IEnumerator(Of aSchedule) Implements IEnumerable(Of aSchedule).GetEnumerator
            Return Items.GetEnumerator()
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function
    End Class

End Class









'Inherits CollectionBase
'Public Sub Add(ByVal oZman As aZman)
'    List.Add(oZman)
'End Sub

'Public Sub Remove(ByVal index As Integer)
'    If index > Count - 1 Or index < 0 Then
'        Console.WriteLine("Index not valid!")
'    Else
'        List.RemoveAt(index)
'    End If
'End Sub

'Public Sub Remove(ByVal index As Integer)
'    If index > Count - 1 Or index < 0 Then
'        Console.WriteLine("Index not valid!")
'    Else
'        Items.RemoveAt(index)
'    End If
'End Sub

'Default Public ReadOnly Property Item(ByVal index As Integer) As aZman
'    Get
'        Return CType(List.Item(index), aZman)
'    End Get
'End Property