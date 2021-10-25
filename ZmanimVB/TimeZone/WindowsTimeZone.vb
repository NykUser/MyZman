#If Not NET20
Imports System
Imports Zmanim.Extensions

Namespace Zmanim.TimeZone
    ''' <summary>
    ''' A ITimeZone implementation of the Windows TimeZone
    ''' (uses the default .net <seecref="TimeZone"/> class)
    ''' </summary>
    Public Class WindowsTimeZone
        Implements ITimeZone
        ''' <summary>
        ''' Initializes a new instance of the <seecref="WindowsTimeZone"/> class.
        ''' </summary>
        Public Sub New()
            Me.New(TimeZoneInfo.Local)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="WindowsTimeZone"/> class.
        ''' </summary>
        ''' <paramname="timeZone">The time zone.</param>
        Public Sub New(ByVal timeZone As TimeZoneInfo)
            Me.TimeZone = timeZone
        End Sub

#If Not NO_FIND_SYSTEM_TIMEZONE_BY_ID Then
        ''' <summary>
        ''' Initializes a new instance of the <seecref="WindowsTimeZone"/> class.
        ''' </summary>
        ''' <paramname="timeZoneName">Name of the time zone.</param>
        Public Sub New(ByVal timeZoneName As String)
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName)
        End Sub
#End If

        ''' <summary>
        ''' Gets or sets the time zone.
        ''' </summary>
        ''' <value>The time zone.</value>
        Public Property TimeZone As TimeZoneInfo

        ''' <summary>
        ''' UTCs the offset.
        ''' If Daylight Saving Time is in effect at the specified date,
        ''' the offset value is adjusted with the amount of daylight saving.
        ''' </summary>
        ''' <paramname="dateTime">The date time.</param>
        ''' <returns></returns>
        Public Function UtcOffset(ByVal dateTime As Date) As Integer Implements ITimeZone.UtcOffset
            Return TimeZone.GetUtcOffset(dateTime).TotalMilliseconds
        End Function

        ''' <summary>
        ''' Ins the daylight time.
        ''' </summary>
        ''' <paramname="dateTime">The date time.</param>
        ''' <returns></returns>
        Public Function IsDaylightSavingTime(ByVal dateTime As Date) As Boolean Implements ITimeZone.IsDaylightSavingTime
            Return TimeZone.IsDaylightSavingTime(dateTime)
        End Function

        ''' <summary>
        ''' Gets the ID of this time zone.
        ''' </summary>
        ''' <returns>the ID of this time zone.</returns>
        Public Function GetId() As String Implements ITimeZone.GetId
            Return GetDisplayName()
        End Function

        ''' <summary>
        ''' Returns a name of this time zone suitable for presentation to the user in the default locale.
        ''' This method returns the long name, not including daylight savings.
        ''' If the display name is not available for the locale, then this method returns a string in the normalized custom ID format.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDisplayName() As String Implements ITimeZone.GetDisplayName
            Return TimeZone.StandardName
        End Function

        ''' <summary>
        ''' Returns the offset of this time zone from UTC at the specified date.
        ''' If Daylight Saving Time is in effect at the specified date,
        ''' the offset value is adjusted with the amount of daylight saving.
        ''' </summary>
        ''' <paramname="timeFromEpoch">the date represented in milliseconds since January 1, 1970 00:00:00 GMT</param>
        ''' <returns>
        ''' the amount of time in milliseconds to add to UTC to get local time.
        ''' </returns>
        Public Function GetOffset(ByVal timeFromEpoch As Long) As Integer Implements ITimeZone.GetOffset
            Return UtcOffset(timeFromEpoch.ToDateTime())
        End Function
    End Class
End Namespace
#End If
