Public Class SettingsCollection
    Inherits SerializableData

    Public LastSelectedIndex As Integer = 0
    Public PlaceListInHebrew As Boolean = False
    Public IsraeliYomTov As Boolean = False
    Public DefaultType As String = "Last"
    Public DefaultSelectedindex As Integer = 1
    Public DefaultName As String = ""
    Public DefaultPlaceListInHebrew As Boolean = False
    Public ColorZman As Boolean = False
    Public StayOnTop As Boolean = False
    Public UseOlderUsnoAlgorithm As Boolean = False
    Public CalculateElevation As Boolean = False
    Public Clock24Hour As Boolean = False
    Public ChangeKeybordLayout As Boolean = True
    Public AskWhenChanging As Boolean = True
    Public BackUpWhenChanging As Boolean = True
    Public ShowTimesOnStatusBar As Boolean = True
    Public LineBetweenZmanim As Boolean = False
    Public TransparencyValue As Double = 1
    Public LocationY As Integer = -1
    Public LocationX As Integer = -1
    Public SizeW As Integer = -1
    Public SizeH As Integer = -1
    Public DataGridCol1W As Integer = -1
    Public DataGridCol2W As Integer = -1
    Public HebrewMenus As Boolean = False
    Public MenuLanguageWasChanged As Boolean = False
    Public SchedulerLocation As String
    Public SchedulerLatitude As String
    Public SchedulerLongitude As String
    Public SchedulerElevation As String
    Public SchedulerTimeZone As String

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