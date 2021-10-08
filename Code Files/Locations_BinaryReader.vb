'not in use now - unblock line 5-8 to use
Imports System.IO

Public Class Locations_List
    'Public Shared AllLocations As List(Of aLocation) = GetAllLocations(My.Resources.LocationsList)
    'Shared Sub New()
    '    SortByName(AllLocations)
    'End Sub
    Shared Function GetAllLocations(ByVal bufer As Byte()) As List(Of aLocation)
        Dim list As New List(Of aLocation)
        Dim BR As New BinaryReader(New MemoryStream(bufer))
        With BR
            Dim count = .ReadInt32 - 1
            For I = 0 To count
                Dim LI As New aLocation
                LI.Num = .ReadString
                LI.EngCountry = .ReadString
                LI.HebCountry = .ReadString
                LI.Elevation = .ReadDouble
                LI.Latitude = .ReadDouble
                LI.Longitude = .ReadDouble
                LI.EngName = .ReadString
                LI.HebName = .ReadString
                LI.TimeOffset = .ReadDouble

                list.Add(LI)

            Next
            .Close()
        End With

        Return list
    End Function
    Shared Sub SortByName(ByVal LIlist As List(Of aLocation))
        LIlist.Sort(Function(A As aLocation, B As aLocation)
                        Return A.EngName.CompareTo(B.EngName)
                    End Function)
    End Sub

End Class
