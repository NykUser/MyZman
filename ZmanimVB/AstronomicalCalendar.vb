' * Zmanim .NET API
' * Copyright (C) 2004-2010 Eliyahu Hershfeld
' *
' * Converted to C# by AdminJew
' *
' * This file is part of Zmanim .NET API.
' *
' * Zmanim .NET API is free software: you can redistribute it and/or modify
' * it under the terms of the GNU Lesser General Public License as published by
' * the Free Software Foundation, either version 3 of the License, or
' * (at your option) any later version.
' *
' * Zmanim .NET API is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU Lesser General Public License for more details.
' *
' * You should have received a copy of the GNU Lesser General Public License
' * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

Imports System
Imports Zmanim.Calculator
Imports Zmanim.Extensions
Imports Zmanim.Utilities
Namespace Zmanim

    ''' <summary>
    ''' A calendar that calculates astronomical time calculations such as
    ''' <seecref="GetSunrise">sunrise</see> and <seecref="GetSunset">sunset</see> times. This
    ''' class contains a <seecref="DateWithLocation">Calendar</see> and can therefore use the
    ''' standard Calendar functionality to change dates etc. The calculation engine
    ''' used to calculate the astronomical times can be changed to a different
    ''' implementation by implementing the <seecref="AstronomicalCalculator"/> and setting
    ''' it with the <seecref="AstronomicalCalculator"/>. A
    ''' number of different implementations are included in the util package <br/>
    ''' 	<b>Note:</b> There are times when the algorithms can't calculate proper
    ''' values for sunrise, sunset and twilight. This is usually caused by trying to calculate
    ''' times for areas either very far North or South, where sunrise / sunset never
    ''' happen on that date. This is common when calculating twilight with a deep dip
    ''' below the horizon for locations as south of the North Pole as London in the
    ''' northern hemisphere. The sun never reaches this dip at certain
    ''' times of the year. When the calculations encounter this condition a null
    ''' will be returned when a <seecref="DateTime"/> is expected and
    ''' <seecref="Long.MinValue"/> when a long is expected. The reason that
    ''' <c>Exception</c>s are not thrown in these cases is because the lack
    ''' of a rise/set or twilight is not an exception, but expected in many parts of the world.
    ''' Here is a simple example of how to use the API to calculate sunrise: <br/>
    ''' First create the Calendar for the location you would like to calculate:
    ''' <example>
    ''' 		<code>
    ''' string locationName = "Lakewood, NJ"
    ''' double latitude = 40.0828; //Lakewood, NJ
    ''' double longitude = -74.2094; //Lakewood, NJ
    ''' double elevation = 20; // optional elevation correction in Meters
    ''' ITimeZone timeZone = new JavaTimeZone("America/New_York");
    ''' GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    ''' elevation, timeZone);
    ''' AstronomicalCalendar ac = new AstronomicalCalendar(location);
    ''' </code>
    ''' You can set the Date and Location on the constructor (or else it will default the the current day).
    ''' <code>
    ''' AstronomicalCalendar ac = new AstronomicalCalendar(new DateTime(2010, 2, 8), location);
    ''' </code>
    ''' Or you can set the DateTime by calling.
    ''' <code>
    ''' ac.DateWithLocation.Date = new DateTime(2010, 2, 8);
    ''' </code>
    ''' To get the time of sunrise
    ''' <code>
    ''' Date sunrise = ac.getSunrise();
    ''' </code>
    ''' 	</example>
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class AstronomicalCalendar
        Implements IAstronomicalCalendar
        ''' <summary>
        '''  90° below the vertical. Used for certain calculations.<br/>
        '''  <b>Note </b>: it is important to note the distinction between this zenith
        '''  and the <seecref="Calculator.AstronomicalCalculator.AdjustZenith">adjusted zenith</see> used
        '''  for some solar calculations. This 90 zenith is only used because some
        '''  calculations in some subclasses are historically calculated as an offset
        '''  in reference to 90.
        ''' </summary>
        Public Const GEOMETRIC_ZENITH As Double = 90

        ' 
        ' ///	 <summary> 
        ' ///	Default value for Sun's zenith and true rise/set Zenith (used in this
        ' ///	class and subclasses) is the angle that the center of the Sun makes to a
        ' ///	line perpendicular to the Earth's surface. If the Sun were a point and
        ' ///	the Earth were without an atmosphere, true sunset and sunrise would
        ' ///	correspond to a 90° zenith. Because the Sun is not a point, and
        ' ///	because the atmosphere refracts light, this 90° zenith does not, in
        ' ///	fact, correspond to true sunset or sunrise, instead the center of the
        ' ///	Sun's disk must lie just below the horizon for the upper edge to be
        ' ///	obscured. This means that a zenith of just above 90° must be used.
        ' ///	The Sun subtends an angle of 16 minutes of arc (this can be changed via
        ' ///	the <see cref="setSunRadius(double)"/> method , and atmospheric refraction
        ' ///	accounts for 34 minutes or so (this can be changed via the
        ' ///	<see cref="AstronomicalCalculator.setRefraction(double)"/> method), giving a total of 50 arcminutes.
        ' ///	The total value for ZENITH is 90+(5/6) or 90.8333333° for true
        ' ///	sunrise/sunset. </summary>
        ' ///	 
        ' public static double ZENITH = GEOMETRIC_ZENITH + 5.0 / 6.0;
        ' 

        ''' <summary>
        '''   Sun's zenith at civil twilight (96°).
        ''' </summary>
        Public Const CIVIL_ZENITH As Double = 96

        ''' <summary>
        '''   Sun's zenith at nautical twilight (102°).
        ''' </summary>
        Public Const NAUTICAL_ZENITH As Double = 102

        ''' <summary>
        '''   Sun's zenith at astronomical twilight (108°).
        ''' </summary>
        Public Const ASTRONOMICAL_ZENITH As Double = 108

        ''' <summary>
        '''   constant for milliseconds in a minute (60,000)
        ''' </summary>
        Friend Const MINUTE_MILLIS As Long = 60 * 1000

        ''' <summary>
        '''   constant for milliseconds in an hour (3,600,000)
        ''' </summary>
        Friend Const HOUR_MILLIS As Long = MINUTE_MILLIS * 60

        ''' <summary>
        '''  Default constructor will set a default <seecref="GeoLocation"/>,
        '''  a default
        '''  <seecref="Calculator.AstronomicalCalculator.GetDefault()">AstronomicalCalculator</see> and
        '''  default the calendar to the current date.
        ''' </summary>
        Public Sub New()
            Me.New(New GeoLocation())
        End Sub

        ''' <summary>
        '''  A constructor that takes in as a parameter geolocation information
        ''' </summary>
        ''' <paramname="geoLocation">
        '''  The location information used for astronomical calculating sun
        '''  times. </param>
        Public Sub New(ByVal geoLocation As IGeoLocation)
            Me.New(Date.Now, geoLocation)
        End Sub

        ''' <summary>
        '''  A constructor that takes in as a parameter geolocation information
        ''' </summary>
        ''' <paramname="dateTime">The DateTime</param>
        ''' <paramname="geoLocation">
        '''  The location information used for astronomical calculating sun
        '''  times. </param>
        Public Sub New(ByVal dateTime As Date, ByVal geoLocation As IGeoLocation)
            Me.New(New DateWithLocation(dateTime, geoLocation))
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="AstronomicalCalendar"/> class.
        ''' </summary>
        ''' <paramname="dateWithLocation">The date with location.</param>
        Public Sub New(ByVal dateWithLocation As IDateWithLocation)
            Me.DateWithLocation = dateWithLocation
            AstronomicalCalculator = Calculator.AstronomicalCalculator.GetDefault()
        End Sub

        ''' <summary>
        ''' The getSunrise method Returns a <c>DateTime</c> representing the
        ''' sunrise time. The zenith used for the calculation uses
        ''' <seealsocref="GEOMETRIC_ZENITH">geometric zenith</seealso> of 90°. This is adjusted
        ''' by the <seealsocref="AstronomicalCalculator"/> that adds approximately 50/60 of a
        ''' degree to account for 34 archminutes of refraction and 16 archminutes for
        ''' the sun's radius for a total of
        ''' <seealsocref="Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</seealso>. See
        ''' documentation for the specific implementation of the
        ''' <seealsocref="AstronomicalCalculator"/> that you are using.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the exact sunrise time.
        ''' If the calculation can't be computed such as in the Arctic
        ''' Circle where there is at least one day a year where the sun does
        ''' not rise, and one where it does not set, a null will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="Calculator.AstronomicalCalculator.AdjustZenith"/>
        Public Overridable Function GetSunrise() As DateTime? Implements IAstronomicalCalendar.GetSunrise
            Dim sunrise = GetUtcSunrise(GEOMETRIC_ZENITH)
            If Double.IsNaN(sunrise) Then Return Nothing
            Return GetDateFromTime(sunrise)
        End Function

        ''' <summary>
        ''' Method that returns the sunrise without correction for elevation.
        ''' Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        ''' amount of visible light, something that is not affected by elevation.
        ''' This method returns sunrise calculated at sea level. This forms the base
        ''' for dawn calculations that are calculated as a dip below the horizon
        ''' before sunrise.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the exact sea-level sunrise
        ''' time.
        ''' If the calculation can't be computed such as in the Arctic
        ''' Circle where there is at least one day a year where the sun does
        ''' not rise, and one where it does not set, a null will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="GetSunrise"/>
        ''' <seealsocref="GetUtcSeaLevelSunrise"/>
        Public Overridable Function GetSeaLevelSunrise() As DateTime?
            Dim sunrise = GetUtcSeaLevelSunrise(GEOMETRIC_ZENITH)
            If Double.IsNaN(sunrise) Then Return Nothing
            Return GetDateFromTime(sunrise)
        End Function

        ''' <summary>
        ''' A method to return the the beginning of civil twilight (dawn) using a
        ''' zenith of <seealsocref="CIVIL_ZENITH">96°</seealso>.
        ''' </summary>
        ''' <returns>
        ''' The <c>DateTime</c> of the beginning of civil twilight using
        ''' a zenith of 96°. If the calculation can't be computed (see explanation on top of the page), null
        ''' will be returned.
        ''' </returns>
        ''' <seealsocref="CIVIL_ZENITH"/>
        Public Overridable Function GetBeginCivilTwilight() As DateTime?
            Return GetSunriseOffsetByDegrees(CIVIL_ZENITH)
        End Function

        ''' <summary>
        '''  A method to return the the beginning of nautical twilight using a zenith
        '''  of <seecref="NAUTICAL_ZENITH">102°</see>.
        ''' </summary>
        ''' <returns> The <c>DateTime</c> of the beginning of nautical twilight
        '''  using a zenith of 102°. If the calculation can't be
        '''  computed (see explanation on top of the page), null will be returned. </returns>
        ''' <seealsocref="NAUTICAL_ZENITH"/>
        Public Overridable Function GetBeginNauticalTwilight() As DateTime?
            Return GetSunriseOffsetByDegrees(NAUTICAL_ZENITH)
        End Function

        ''' <summary>
        '''  A method that returns the the beginning of astronomical twilight using a
        '''  zenith of <seecref="ASTRONOMICAL_ZENITH">108°</see>.
        ''' </summary>
        ''' <returns> The <c>DateTime</c> of the beginning of astronomical twilight
        '''  using a zenith of 108°. If the 
        ''' calculation can't be computed (see explanation on top of thepage),
        ''' null will be returned.
        ''' </returns>
        ''' <seealsocref="ASTRONOMICAL_ZENITH"/>
        Public Overridable Function GetBeginAstronomicalTwilight() As DateTime?
            Return GetSunriseOffsetByDegrees(ASTRONOMICAL_ZENITH)
        End Function

        ''' <summary>
        ''' The getSunset method Returns a <c>DateTime</c> representing the
        ''' sunset time. The zenith used for the calculation uses
        ''' <seecref="GEOMETRIC_ZENITH">geometric zenith</see> of 90°. This is adjusted
        ''' by the <seecref="AstronomicalCalculator"/> that adds approximately 50/60 of a
        ''' degree to account for 34 archminutes of refraction and 16 archminutes for
        ''' the sun's radius for a total of
        ''' <seecref="Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</see>. See
        ''' documentation for the specific implementation of the
        ''' <seecref="AstronomicalCalculator"/> that you are using. Note: In certain cases
        ''' the calculates sunset will occur before sunrise. This will typically
        ''' happen when a timezone other than the local timezone is used (calculating
        ''' Los Angeles sunset using a GMT timezone for example). In this case the
        ''' sunset date will be incremented to the following date.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the exact sunset time. 
        ''' If the calculation can't be computed such as in the Arctic
        ''' Circle where there is at least one day a year where the sun does
        ''' not rise, and one where it does not set, a null will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="Calculator.AstronomicalCalculator.AdjustZenith"/>
        Public Overridable Function GetSunset() As DateTime? Implements IAstronomicalCalendar.GetSunset
            Dim sunset = GetUtcSunset(GEOMETRIC_ZENITH)
            If Double.IsNaN(sunset) Then Return Nothing
            Return GetAdjustedSunsetDate(GetDateFromTime(sunset), GetSunrise())
        End Function

        ''' <summary>
        ''' A method that will roll the sunset time forward a day if sunset occurs
        ''' before sunrise. This will typically happen when a timezone other than the
        ''' local timezone is used (calculating Los Angeles sunset using a GMT
        ''' timezone for example). In this case the sunset date will be incremented
        ''' to the following date.
        ''' </summary>
        ''' <paramname="sunset">the sunset date to adjust if needed</param>
        ''' <paramname="sunrise">the sunrise to compare to the sunset</param>
        ''' <returns>
        ''' the adjusted sunset date.
        ''' If the calculation can't be computed such as in the Arctic
        ''' Circle where there is at least one day a year where the sun does
        ''' not rise, and one where it does not set, a null will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        Private Function GetAdjustedSunsetDate(ByVal sunset As DateTime?, ByVal sunrise As DateTime?) As DateTime?
            If sunset Is Nothing OrElse sunrise Is Nothing OrElse sunrise.Value.CompareTo(sunset.Value) < 0 Then Return sunset
            Return sunset.Value.AddDays(1)
        End Function

        ''' <summary>
        ''' Method that returns the sunset without correction for elevation.
        ''' Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        ''' amount of visible light, something that is not affected by elevation.
        ''' This method returns sunset calculated at sea level. This forms the base
        ''' for dusk calculations that are calculated as a dip below the horizon
        ''' after sunset.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the exact sea-level sunset time.
        ''' If the calculation can't be computed such as in the Arctic
        ''' Circle where there is at least one day a year where the sun does
        ''' not rise, and one where it does not set, a null will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="GetSunset"/>
        ''' <seealsocref="GetUtcSeaLevelSunset"/>
        Public Overridable Function GetSeaLevelSunset() As DateTime?
            Dim sunset = GetUtcSeaLevelSunset(GEOMETRIC_ZENITH)
            If Double.IsNaN(sunset) Then Return Nothing
            Return GetAdjustedSunsetDate(GetDateFromTime(sunset), GetSeaLevelSunrise())
        End Function

        ''' <summary>
        ''' A method to return the the end of civil twilight using a zenith of
        ''' <seecref="CIVIL_ZENITH">96°</see>.
        ''' </summary>
        ''' <returns>
        ''' The <c>DateTime</c> of the end of civil twilight using a
        ''' zenith of <seealsocref="CIVIL_ZENITH">96°</seealso>. If the 
        ''' calculation can't be computed (see explanation on top of thepage),
        ''' null will be returned.
        ''' </returns>
        ''' <seealsocref="CIVIL_ZENITH"/>
        Public Overridable Function GetEndCivilTwilight() As DateTime?
            Return GetSunsetOffsetByDegrees(CIVIL_ZENITH)
        End Function

        ''' <summary>
        ''' A method to return the the end of nautical twilight using a zenith of
        ''' <seecref="NAUTICAL_ZENITH">102°</see>.
        ''' </summary>
        ''' <returns>
        ''' The <c>DateTime</c> of the end of nautical twilight using a
        ''' zenith of <seealsocref="NAUTICAL_ZENITH">102°</seealso>. If the 
        ''' calculation can't be computed (see explanation on top of thepage),
        ''' null will be returned.
        ''' </returns>
        ''' <seealsocref="NAUTICAL_ZENITH"/>
        Public Overridable Function GetEndNauticalTwilight() As DateTime?
            Return GetSunsetOffsetByDegrees(NAUTICAL_ZENITH)
        End Function

        ''' <summary>
        ''' A method to return the the end of astronomical twilight using a zenith of
        ''' <seecref="ASTRONOMICAL_ZENITH">108°</see>.
        ''' </summary>
        ''' <returns>
        ''' The The <c>DateTime</c> of the end of astronomical twilight
        ''' using a zenith of <seecref="ASTRONOMICAL_ZENITH">108°</see>.
        ''' If the calculation can't be computed (see explanation on top of thepage),
        ''' null will be returned.
        ''' </returns>
        ''' <seealsocref="ASTRONOMICAL_ZENITH"/>
        Public Overridable Function GetEndAstronomicalTwilight() As DateTime?
            Return GetSunsetOffsetByDegrees(ASTRONOMICAL_ZENITH)
        End Function

        ''' <summary>
        ''' Utility method that returns a date offset by the offset time passed in.
        ''' This method casts the offset as a <code>long</code> and calls
        ''' <seecref="GetTimeOffset(System.DateTime,Long)"/>.
        ''' </summary>
        ''' <paramname="time">the start time</param>
        ''' <paramname="offset">the offset in milliseconds to add to the time</param>
        ''' <returns>
        ''' the <seecref="DateTime"/>with the offset added to it
        ''' </returns>
        Public Overridable Function GetTimeOffset(ByVal time As Date, ByVal offset As Double) As DateTime?
            Return GetTimeOffset(time, CLng(offset))
        End Function

        Protected Overridable Function GetTimeOffset(ByVal time As DateTime?, ByVal offset As Double) As DateTime?
            Return GetTimeOffset(time, CLng(offset))
        End Function

        ''' <summary>
        ''' A utility method to return a date offset by the offset time passed in.
        ''' </summary>
        ''' <paramname="time">the start time</param>
        ''' <paramname="offset">the offset in milliseconds to add to the time.</param>
        ''' <returns>
        ''' the <seecref="DateTime"/> with the offset in milliseconds added
        ''' to it
        ''' </returns>
        Public Overridable Function GetTimeOffset(ByVal time As Date, ByVal offset As Long) As DateTime?
            If offset = Long.MinValue Then Return Nothing
            Return time.AddMilliseconds(offset)
        End Function

        Protected Overridable Function GetTimeOffset(ByVal time As DateTime?, ByVal offset As Long) As DateTime?
            If time Is Nothing Then Return Nothing
            Return GetTimeOffset(time.Value, offset)
        End Function

        ''' <summary>
        ''' A utility method to return the time of an offset by degrees below or
        ''' above the horizon of <seecref="GetSunrise">sunrise</see>.
        ''' </summary>
        ''' <paramname="offsetZenith">the degrees before <seecref="GetSunrise"/> to use in the
        ''' calculation. For time after sunrise use negative numbers.</param>
        ''' <returns>
        ''' The <seealsocref="DateTime"/> of the offset after (or before)
        ''' <seecref="GetSunrise"/>.
        ''' If the calculation can't be computed such as in the Arctic
        ''' Circle where there is at least one day a year where the sun does
        ''' not rise, and one where it does not set, a null will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        Public Overridable Function GetSunriseOffsetByDegrees(ByVal offsetZenith As Double) As DateTime?
            Dim alos = GetUtcSunrise(offsetZenith)
            If Double.IsNaN(alos) Then Return Nothing
            Return GetDateFromTime(alos)
        End Function

        ''' <summary>
        '''  A utility method to return the time of an offset by degrees below or
        '''  above the horizon of <seecref="GetSunset">sunset</see>.
        ''' </summary>
        ''' <paramname="offsetZenith">
        '''  the degrees after <seecref="GetSunset"/> to use in the
        '''  calculation. For time before sunset use negative numbers. </param>
        ''' <returns> The <seealsocref="DateTime"/>of the offset after (or before)
        '''  <seecref="GetSunset"/>.
        ''' If the calculation can't be computed such as in the Arctic Circle where
        ''' there is at least one day a year where the sun does not rise, and
        ''' one where it does not set, <seecref="Double.NaN"/> will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        Public Overridable Function GetSunsetOffsetByDegrees(ByVal offsetZenith As Double) As DateTime?
            Dim sunset = GetUtcSunset(offsetZenith)
            If Double.IsNaN(sunset) Then Return Nothing
            Return GetAdjustedSunsetDate(GetDateFromTime(sunset), GetSunriseOffsetByDegrees(offsetZenith))
        End Function

        ''' <summary>
        ''' Method that returns the sunrise in UTC time without correction for time
        ''' zone offset from GMT and without using daylight savings time.
        ''' </summary>
        ''' <paramname="zenith">the degrees below the horizon. For time after sunrise use
        ''' negative numbers.</param>
        ''' <returns>
        ''' The time in the format: 18.75 for 18:45:00 UTC/GMT. 
        ''' If the calculation can't be computed such as in the Arctic Circle where
        ''' there is at least one day a year where the sun does not rise, and
        ''' one where it does not set, <seecref="Double.NaN"/> will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        Public Overridable Function GetUtcSunrise(ByVal zenith As Double) As Double
            Return AstronomicalCalculator.GetUtcSunrise(DateWithLocation, zenith, True)
        End Function

        ''' <summary>
        ''' Method that returns the sunrise in UTC time without correction for time
        ''' zone offset from GMT and without using daylight savings time. Non-sunrise
        ''' and sunset calculations such as dawn and dusk, depend on the amount of
        ''' visible light, something that is not affected by elevation. This method
        ''' returns UTC sunrise calculated at sea level. This forms the base for dawn
        ''' calculations that are calculated as a dip below the horizon before
        ''' sunrise.
        ''' </summary>
        ''' <paramname="zenith">the degrees below the horizon. For time after sunrise use
        ''' negative numbers.</param>
        ''' <returns>
        ''' The time in the format: 18.75 for 18:45:00 UTC/GMT.
        ''' If the calculation can't be computed such as in the Arctic Circle where
        ''' there is at least one day a year where the sun does not rise, and
        ''' one where it does not set, <seecref="Double.NaN"/> will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="GetUtcSunrise"/>
        ''' <seealsocref="GetUtcSeaLevelSunset"/>
        Public Overridable Function GetUtcSeaLevelSunrise(ByVal zenith As Double) As Double
            Return AstronomicalCalculator.GetUtcSunrise(DateWithLocation, zenith, False)
        End Function

        ''' <summary>
        ''' Method that returns the sunset in UTC time without correction for time
        ''' zone offset from GMT and without using daylight savings time.
        ''' </summary>
        ''' <paramname="zenith">the degrees below the horizon. For time after before sunset
        ''' use negative numbers.</param>
        ''' <returns>
        ''' The time in the format: 18.75 for 18:45:00 UTC/GMT.
        ''' If the calculation can't be computed such as in the Arctic Circle where
        ''' there is at least one day a year where the sun does not rise, and
        ''' one where it does not set, <seecref="Double.NaN"/> will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="GetUtcSeaLevelSunset"/>
        Public Overridable Function GetUtcSunset(ByVal zenith As Double) As Double
            Return AstronomicalCalculator.GetUtcSunset(DateWithLocation, zenith, True)
        End Function

        ''' <summary>
        ''' Method that returns the sunset in UTC time without correction for
        ''' elevation, time zone offset from GMT and without using daylight savings
        ''' time. Non-sunrise and sunset calculations such as dawn and dusk, depend
        ''' on the amount of visible light, something that is not affected by
        ''' elevation. This method returns UTC sunset calculated at sea level. This
        ''' forms the base for dusk calculations that are calculated as a dip below
        ''' the horizon after sunset.
        ''' </summary>
        ''' <paramname="zenith">the degrees below the horizon. For time before sunset use
        ''' negative numbers.</param>
        ''' <returns>
        ''' The time in the format: 18.75 for 18:45:00 UTC/GMT.
        ''' If the calculation can't be computed such as in the Arctic Circle where
        ''' there is at least one day a year where the sun does not rise, and
        ''' one where it does not set, <seecref="Double.NaN"/> will be returned.
        ''' See detailed explanation on top of the page.
        ''' </returns>
        ''' <seealsocref="GetUtcSunset"/>
        ''' <seealsocref="GetUtcSeaLevelSunrise"/>
        Public Overridable Function GetUtcSeaLevelSunset(ByVal zenith As Double) As Double
            Return AstronomicalCalculator.GetUtcSunset(DateWithLocation, zenith, False)
        End Function

        ''' <summary>
        ''' Method to return a temporal (solar) hour. The day from sunrise to sunset
        ''' is split into 12 equal parts with each one being a temporal hour.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a temporal hour. If
        ''' the calculation can't be computed  (see explanation on top of the page) <seecref="Long.MinValue"/>
        ''' will be returned.
        ''' </returns>
        Public Overridable Function GetTemporalHour() As Long
            Return GetTemporalHour(GetSunrise().Value, GetSunset().Value)
        End Function

        ''' <summary>
        ''' Utility method that will allow the calculation of a temporal (solar) hour
        ''' based on the sunrise and sunset passed to this method.
        ''' </summary>
        ''' <paramname="sunrise">The start of the day.</param>
        ''' <paramname="sunset">The end of the day.</param>
        ''' <returns>
        ''' the <code>long</code> millisecond length of the temporal hour.
        ''' If the calculation can't be computed (see explanation on top of the page)
        ''' <seecref="Long.MinValue"/> will be returned.
        ''' </returns>
        ''' <seealsocref="GetTemporalHour()"/>
        Public Overridable Function GetTemporalHour(ByVal sunrise As Date, ByVal sunset As Date) As Long
            'Note: I don't think we need this.
            If sunrise = Date.MinValue OrElse sunset = Date.MinValue Then Return Long.MinValue
            Return (sunset - sunrise).TotalMilliseconds / 12
        End Function

        Protected Overridable Function GetTemporalHour(ByVal sunrise As DateTime?, ByVal sunset As DateTime?) As Long
            If sunrise Is Nothing OrElse sunset Is Nothing Then
                Return Long.MinValue
            End If

            Return (sunset - sunrise).Value.TotalMilliseconds / 12
        End Function

        ''' <summary>
        ''' A method that returns sundial or solar noon. It occurs when the Sun is
        ''' <ahref="http://en.wikipedia.org/wiki/Transit_%28astronomy%29">transitting</a> the 
        ''' <ahref="http://en.wikipedia.org/wiki/Meridian_%28astronomy%29">celestial meridian</a>.
        ''' In this class it is calculated as halfway between sea level sunrise and sea level sunset,
        ''' which can be slightly off the real transit
        ''' time due to changes in declination (the lengthening or shortening day).
        ''' </summary>
        ''' <returns> the <code>Date</code> representing Sun's transit. If the calculation can't be computed such as in the
        '''         Arctic Circle where there is at least one day a year where the sun does not rise, and one where it does
        '''         not set, null will be returned. See detailed explanation on top of the page. </returns>
        ''' <seealsocref="GetSunTransit"/>
        ''' <seealsocref="GetTemporalHour"/>
        Public Overridable Function GetSunTransit() As DateTime?
            Return GetSunTransit(GetSeaLevelSunrise(), GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' A method that returns sundial or solar noon. It occurs when the Sun is 
        ''' <ahref="http://en.wikipedia.org/wiki/Transit_%28astronomy%29">transitting</a>
        ''' the <ahref="http://en.wikipedia.org/wiki/Meridian_%28astronomy%29">celestial meridian</a>. In this class it is
        ''' calculated as halfway between the sunrise and sunset passed to this method. This time can be slightly off the
        ''' real transit time due to changes in declination (the lengthening or shortening day).
        ''' </summary>
        ''' <paramname="startOfDay">
        '''            the start of day for calculating the sun's transit. This can be sea level sunrise, visual sunrise (or
        '''            any arbitrary start of day) passed to this method. </param>
        ''' <paramname="endOfDay">
        '''            the end of day for calculating the sun's transit. This can be sea level sunset, visual sunset (or any
        '''            arbitrary end of day) passed to this method.
        ''' </param>
        ''' <returns> the <code>Date</code> representing Sun's transit. If the calculation can't be computed such as in the
        '''         Arctic Circle where there is at least one day a year where the sun does not rise, and one where it does
        '''         not set, null will be returned. See detailed explanation on top of the page. </returns>
        Public Overridable Function GetSunTransit(ByVal startOfDay As DateTime?, ByVal endOfDay As DateTime?) As DateTime?
            Dim temporalHour = GetTemporalHour(startOfDay, endOfDay)
            Return GetTimeOffset(startOfDay, temporalHour * 6)
        End Function

        ''' <summary>
        '''  A method that returns a <c>DateTime</c> from the time passed in
        ''' </summary>
        ''' <paramname="time">
        '''  The time to be set as the time for the <c>DateTime</c>.
        '''  The time expected is in the format: 18.75 for 6:45:00 PM </param>
        ''' <returns> The Date. </returns>
        Protected Friend Overridable Function GetDateFromTime(ByVal time As Double) As DateTime?
            If Double.IsNaN(time) Then Return Nothing
            time = (time + 240) Mod 24 ' the calculators sometimes return a double
            ' that is negative or slightly greater than 24

            'Nykedit
            Dim dt As DateTime = (New DateTime()).AddHours(time)
            Dim hours As Integer = dt.Hour
            Dim minutes As Integer = dt.Minute
            Dim seconds As Integer = dt.Second
            Dim Milliseconds As Integer = dt.Millisecond

            'this did not work in vb as nedeed 
            'Dim calculatedTime As Double = time
            'Dim hours As Integer = CType(calculatedTime, Integer)
            '' retain only the hours
            'calculatedTime -= hours
            'Dim minutes As Integer = CType((calculatedTime * 60), Integer)
            '' retain only the minutes
            'calculatedTime -= minutes
            'Dim seconds As Integer = CType((calculatedTime * 60), Integer)
            '' retain only the seconds
            'calculatedTime -= seconds
            '' remaining milliseconds

            Dim [date] = DateWithLocation.Date
            Dim utcDateTime = New DateTime([date].Year, [date].Month, [date].Day, hours, minutes, seconds, Milliseconds, DateTimeKind.Utc)
            Dim localOffset As Long = DateWithLocation.Location.TimeZone.UtcOffset(utcDateTime)
            Return utcDateTime.AddMilliseconds(localOffset)
        End Function
        'Protected Function getDateFromTime2(ByVal time As Double, ByVal isSunrise As Boolean) As Date
        '    If Double.IsNaN(time) Then
        '        Return Nothing
        '    End If

        '    Dim calculatedTime As Double = time
        '    Dim hours As Integer = CType(calculatedTime, Integer)
        '    ' retain only the hours
        '    calculatedTime = (calculatedTime - hours)
        '    Dim minutes As Integer = CType((calculatedTime * 60), Integer)
        '    ' retain only the minutes
        '    calculatedTime = (calculatedTime - minutes)
        '    Dim seconds As Integer = CType((calculatedTime * 60), Integer)
        '    ' retain only the seconds
        '    calculatedTime = (calculatedTime - seconds)
        '    ' remaining milliseconds

        '    '' Check if a date transition has occurred, or is about to occur - this indicates the date of the event is
        '    '' actually not the target date, but the day prior or after
        '    'Dim localTimeHours As Integer = (CType(getGeoLocation.getLongitude, Integer) / 15)
        '    'If (isSunrise _
        '    '        AndAlso (localTimeHours _
        '    '        + (hours > 18))) Then
        '    '    cal.add(Calendar.DAY_OF_MONTH, -1)
        '    'ElseIf (Not isSunrise _
        '    '        AndAlso (localTimeHours _
        '    '        + (hours < 6))) Then
        '    '    cal.add(Calendar.DAY_OF_MONTH, 1)
        '    'End If

        '    cal.set(Calendar.HOUR_OF_DAY, hours)
        '    cal.set(Calendar.MINUTE, minutes)
        '    cal.set(Calendar.SECOND, seconds)
        '    cal.set(Calendar.MILLISECOND, CType((calculatedTime * 1000), Integer))
        '    Return cal.getTime
        'End Function

        ''' <summary>
        ''' Will return the dip below the horizon before sunrise that matches the
        ''' offset minutes on passed in. For example passing in 72 minutes for a
        ''' calendar set to the equinox in Jerusalem returns a value close to
        ''' 16.1°
        ''' Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        ''' <em><b>TODO:</b></em> Improve efficiency.
        ''' </summary>
        ''' <paramname="minutes">offset</param>
        ''' <returns>
        ''' the degrees below the horizon that match the offset on the
        ''' equinox in Jerusalem at sea level.
        ''' </returns>
        ''' <seealsocref="GetSunsetSolarDipFromOffset"/>
        Public Overridable Function GetSunriseSolarDipFromOffset(ByVal minutes As Double) As Double
            Dim offsetByDegrees = GetSeaLevelSunrise()
            Dim offsetByTime = GetTimeOffset(GetSeaLevelSunrise().Value, -(minutes * MINUTE_MILLIS))
            Dim degrees = 0D
            Dim incrementor = 0.0001D

            While offsetByDegrees Is Nothing OrElse offsetByDegrees.Value.ToUnixEpochMilliseconds() > offsetByTime.Value.ToUnixEpochMilliseconds()
                degrees += incrementor
                offsetByDegrees = GetSunriseOffsetByDegrees(GEOMETRIC_ZENITH + degrees)
            End While

            Return degrees
        End Function

        ''' <summary>
        ''' Will return the dip below the horizon after sunset that matches the
        ''' offset minutes on passed in. For example passing in 72 minutes for a
        ''' calendar set to the equinox in Jerusalem returns a value close to
        ''' 16.1°
        ''' Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        ''' <em><b>TODO:</b></em> Improve efficiency.
        ''' </summary>
        ''' <paramname="minutes">offset</param>
        ''' <returns>
        ''' the degrees below the horizon that match the offset on the
        ''' equinox in Jerusalem at sea level.
        ''' </returns>
        ''' <seealsocref="GetSunriseSolarDipFromOffset"/>
        Public Overridable Function GetSunsetSolarDipFromOffset(ByVal minutes As Double) As Double
            Dim offsetByDegrees = GetSeaLevelSunset()
            Dim offsetByTime = GetTimeOffset(GetSeaLevelSunset().Value, minutes * MINUTE_MILLIS)
            Dim degrees = 0D
            Dim incrementor = 0.001D

            While offsetByDegrees Is Nothing OrElse offsetByDegrees.Value.ToUnixEpochMilliseconds() < offsetByTime.Value.ToUnixEpochMilliseconds()
                degrees += incrementor
                offsetByDegrees = GetSunsetOffsetByDegrees(GEOMETRIC_ZENITH + degrees)
            End While

            Return degrees
        End Function

        ''' <returns> an XML formatted representation of the class. It returns the
        '''  default output of the
        '''  <seecref="ZmanimFormatter.ToXml">toXML</see>
        '''  method. </returns>
        ''' <seealsocref="ZmanimFormatter.ToXml"/>
        Public Overrides Function ToString() As String
            Return ZmanimFormatter.ToXml(Me)
        End Function

        ''' <summary>
        ''' Determines whether the specified <seecref="System.Object"/> is equal to this instance.
        ''' </summary>
        ''' <paramname="obj">The <seecref="System.Object"/> to compare with this instance.</param>
        ''' <returns>
        ''' 	<c>true</c> if the specified <seecref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        ''' </returns>
        ''' <exceptioncref="T:System.NullReferenceException">
        ''' The <paramrefname="obj"/> parameter is null.
        ''' </exception>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If Me Is obj Then Return True
            If Not (TypeOf obj Is AstronomicalCalendar) Then Return False
            Dim aCal = CType(obj, AstronomicalCalendar)
            Return DateWithLocation.Equals(aCal.DateWithLocation) AndAlso DateWithLocation.Location.Equals(aCal.DateWithLocation.Location) AndAlso AstronomicalCalculator.Equals(aCal.AstronomicalCalculator)
        End Function

        ''' <summary>
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        ''' </returns>
        Public Overrides Function GetHashCode() As Integer
            Dim result = 17
            result = 37 * result + [GetType]().GetHashCode() ' needed or this and subclasses will return identical hash
            result += 37 * result + DateWithLocation.GetHashCode()
            result += 37 * result + DateWithLocation.Location.GetHashCode()
            result += 37 * result + AstronomicalCalculator.GetHashCode()
            Return result
        End Function

        ''' <summary>
        ''' Gets or Sets the current AstronomicalCalculator set.
        ''' </summary>
        ''' <value>Returns the astronimicalCalculator.</value>
        Public Overridable Property AstronomicalCalculator As IAstronomicalCalculator Implements IAstronomicalCalendar.AstronomicalCalculator

        ''' <summary>
        ''' Gets or Sets the Date and Location to be used in the calculations.
        ''' </summary>
        ''' <value>The calendar to set.</value>
        Public Overridable Property DateWithLocation As IDateWithLocation Implements IAstronomicalCalendar.DateWithLocation
    End Class

End Namespace