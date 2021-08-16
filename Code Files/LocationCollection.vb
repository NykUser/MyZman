Public Class LocationCollection
    Inherits SerializableData
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
        'Debug.Print(olocation.EngName)
        Items.Add(New aLocation(olocation))
    End Sub
    Public Function GetEnumerator() As IEnumerator(Of aLocation) Implements IEnumerable(Of aLocation).GetEnumerator
        Return Items.GetEnumerator()
    End Function

    Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function
End Class
