'isue with Zmanim.TimeZone.WindowsTimeZone using this in its place

Imports Zmanim.TimeZone
Class PZmanimTimeZone
    Implements ITimeZone
    Private _TimeZoneInfo As TimeZoneInfo
    Sub New(ByVal TimeZoneInfo As TimeZoneInfo)
        _TimeZoneInfo = TimeZoneInfo
    End Sub
    'Public Function Clone() As Object Implements System.ICloneable.Clone
    '    '  Throw New NotImplementedException
    'End Function
    Public Function GetDisplayName() As String Implements Zmanim.TimeZone.ITimeZone.GetDisplayName
        'Throw New NotImplementedException
    End Function
    Public Function GetId() As String Implements Zmanim.TimeZone.ITimeZone.GetId
        ' Throw New NotImplementedException
    End Function
    Public Function GetOffset(timeFromEpoch As Long) As Integer Implements Zmanim.TimeZone.ITimeZone.GetOffset
        '   Throw New NotImplementedException
    End Function
    Public Function IsDaylightSavingTime(dateTime As Date) As Boolean Implements Zmanim.TimeZone.ITimeZone.IsDaylightSavingTime
        '  Throw New NotImplementedException
    End Function
    Public Function UtcOffset(dateTime As Date) As Integer Implements Zmanim.TimeZone.ITimeZone.UtcOffset
        'was giving some peple Error
        Return CInt(_TimeZoneInfo.GetUtcOffset(dateTime).TotalMilliseconds)
    End Function
End Class
