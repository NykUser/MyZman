
Namespace Zmanim.TimeZone
    ''' <summary>
    ''' Provides the most basic useage of a TimeZone.
    ''' </summary>
    Public Interface ITimeZone
        ''' <summary>
        ''' UTCs the offset.
        ''' If Daylight Saving Time is in effect at the specified date,
        ''' the offset value is adjusted with the amount of daylight saving.
        ''' </summary>
        ''' <paramname="dateTime">The date time.</param>
        ''' <returns></returns>
        Function UtcOffset(ByVal dateTime As Date) As Integer

        ''' <summary>
        ''' Is the current DateTime in daylight time for this time zone.
        ''' </summary>
        ''' <paramname="dateTime">The date time.</param>
        ''' <returns></returns>
        Function IsDaylightSavingTime(ByVal dateTime As Date) As Boolean

        ''' <summary>
        ''' Gets the ID of this time zone.
        ''' </summary>
        ''' <returns>the ID of this time zone.</returns>
        Function GetId() As String

        ''' <summary>
        ''' Returns a name of this time zone suitable for presentation to the user in the default locale. 
        ''' This method returns the long name, not including daylight savings.
        ''' If the display name is not available for the locale, then this method returns a string in the normalized custom ID format.
        ''' </summary>
        ''' <returns></returns>
        Function GetDisplayName() As String

        ''' <summary>
        ''' Returns the offset of this time zone from UTC at the specified date.
        ''' If Daylight Saving Time is in effect at the specified date,
        ''' the offset value is adjusted with the amount of daylight saving.
        ''' </summary>
        ''' <paramname="timeFromEpoch">the date represented in milliseconds since January 1, 1970 00:00:00 GMT</param>
        ''' <returns>the amount of time in milliseconds to add to UTC to get local time.</returns>
        Function GetOffset(ByVal timeFromEpoch As Long) As Integer
    End Interface
End Namespace
