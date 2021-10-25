Imports System
Imports Zmanim.Extensions

Namespace Zmanim.TimeZone
    ''' <summary>
    ''' TimeZone based on the Gmt offset (this is very limited)
    ''' </summary>
    Public Class OffsetTimeZone
        Implements ITimeZone

        Private ReadOnly offsetFromGmt As TimeSpan

        ''' <summary>
        ''' </summary>
        ''' <paramname="hoursOffsetFromGmt">The amount of hours from gmt.</param>
        Public Sub New(ByVal hoursOffsetFromGmt As Integer)
            Me.New(New TimeSpan(hoursOffsetFromGmt, 0, 0))
        End Sub

        ''' <summary>
        ''' </summary>
        ''' <paramname="offsetFromGmt">TimeSpan from Gmt</param>
        Public Sub New(ByVal offsetFromGmt As TimeSpan)
            Me.offsetFromGmt = offsetFromGmt
        End Sub

        Public Function UtcOffset(ByVal dateTime As Date) As Integer Implements ITimeZone.UtcOffset
            Return offsetFromGmt.TotalMilliseconds
        End Function

        Public Function IsDaylightSavingTime(ByVal dateTime As Date) As Boolean Implements ITimeZone.IsDaylightSavingTime
            Return False
        End Function

        Public Function GetId() As String Implements ITimeZone.GetId
            Return "Offset"
        End Function

        Public Function GetDisplayName() As String Implements ITimeZone.GetDisplayName
            Return GetId()
        End Function

        Public Function GetOffset(ByVal timeFromEpoch As Long) As Integer Implements ITimeZone.GetOffset
            Return UtcOffset(timeFromEpoch.ToDateTime())
        End Function
    End Class
End Namespace
