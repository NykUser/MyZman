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
Imports Zmanim.Utilities

Namespace Zmanim.Calculator
    ''' <summary>
    '''   Implementation of sunrise and sunset methods to calculate astronomical times.
    '''   This calculator uses the Java algorithm written by <ahref="http://www.kevinboone.com/suntimes.html">Kevin Boone</a> that is based
    '''   on the <ahref="http://aa.usno.navy.mil/">US Naval Observatory's</a><ahref="http://aa.usno.navy.mil/publications/docs/asa.php">Almanac</a> for
    '''   Computer algorithm ( <ahref="http://www.amazon.com/exec/obidos/tg/detail/-/0160515106/">Amazon</a>,
    '''   <ahref="http://search.barnesandnoble.com/booksearch/isbnInquiry.asp?isbn=0160515106">Barnes
    '''     &amp; Noble</a>) and is used with his permission. Added to Kevin's code is
    '''   adjustment of the zenith to account for elevation.
    ''' </summary>
    ''' <author>Kevin Boone</author>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class SunTimesCalculator
        Inherits AstronomicalCalculator
        ' DEG_PER_HOUR is the number of degrees of longitude
        ' that corresponds to one hour time difference.
        Private Const DEG_PER_HOUR As Double = 360.0 / 24.0

        ''' <summary>
        ''' </summary>
        ''' <value>the descriptive name of the algorithm.</value>
        Public Overrides ReadOnly Property CalculatorName As String
            Get
                Return "US Naval Almanac Algorithm"
            End Get
        End Property

        ''' <summary>
        ''' A method that calculates UTC sunrise as well as any time based on an
        ''' angle above or below sunrise. This abstract method is implemented by the
        ''' classes that extend this class.
        ''' </summary>
        ''' <paramname="dateWithLocation">Used to calculate day of year.</param>
        ''' <paramname="zenith">the azimuth below the vertical zenith of 90 degrees. for
        ''' sunrise typically the <seecref="AstronomicalCalculator.AdjustZenith">zenith</see> used for
        ''' the calculation uses geometric zenith of 90°; and
        ''' <seecref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        ''' solar refraction and the sun's radius. Another example would
        ''' be <seecref="AstronomicalCalendar.GetBeginNauticalTwilight"/>
        ''' that passes <seecref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to
        ''' this method.</param>
        ''' <paramname="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        ''' <returns>
        ''' The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        ''' 5.75.0. If an error was encountered in the calculation (expected
        ''' behavior for some locations such as near the poles,
        ''' <seecref="Double.NaN"/> will be returned.
        ''' </returns>
        ''' <seealsocref="AstronomicalCalculator.GetUtcSunrise"/>
        Public Overrides Function GetUtcSunrise(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean) As Double
            Dim elevation = If(adjustForElevation, dateWithLocation.Location.Elevation, 0)
            Return GetTimeUtc(dateWithLocation.Date, dateWithLocation.Location, AdjustZenith(zenith, elevation), True)
        End Function

        ''' <summary>
        ''' A method that calculates UTC sunset as well as any time based on an angle
        ''' above or below sunset. This abstract method is implemented by the classes
        ''' that extend this class.
        ''' </summary>
        ''' <paramname="dateWithLocation">Used to calculate day of year.</param>
        ''' <paramname="zenith">the azimuth below the vertical zenith of 90°;. For sunset
        ''' typically the <seecref="AstronomicalCalculator.AdjustZenith">zenith</see> used for the
        ''' calculation uses geometric zenith of 90°; and
        ''' <seecref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        ''' solar refraction and the sun's radius. Another example would
        ''' be <seecref="AstronomicalCalendar.GetEndNauticalTwilight"/> that
        ''' passes <seecref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to this
        ''' method.</param>
        ''' <paramname="adjustForElevation"></param>
        ''' <returns>
        ''' The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        ''' 5.75.0. If an error was encountered in the calculation (expected
        ''' behavior for some locations such as near the poles,
        ''' <seealsocref="Double.NaN"/> will be returned.
        ''' </returns>
        ''' <seealsocref="AstronomicalCalculator.GetUtcSunset"/>
        Public Overrides Function GetUtcSunset(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean) As Double
            Dim elevation = If(adjustForElevation, dateWithLocation.Location.Elevation, 0)
            Return GetTimeUtc(dateWithLocation.Date, dateWithLocation.Location, AdjustZenith(zenith, elevation), False)
        End Function

        ''' <summary>
        '''  sin of an angle in degrees
        ''' </summary>
        Private Shared Function SinDeg(ByVal deg As Double) As Double
            Return Math.Sin(deg * 2.0 * Math.PI / 360.0)
        End Function

        ''' <summary>
        '''  acos of an angle, result in degrees
        ''' </summary>
        Private Shared Function AcosDeg(ByVal x As Double) As Double
            Return Math.Acos(x) * 360.0 / (2 * Math.PI)
        End Function

        ''' <summary>
        '''  * asin of an angle, result in degrees
        ''' </summary>
        Private Shared Function AsinDeg(ByVal x As Double) As Double
            Return Math.Asin(x) * 360.0 / (2 * Math.PI)
        End Function

        ''' <summary>
        '''  tan of an angle in degrees
        ''' </summary>
        Private Shared Function TanDeg(ByVal deg As Double) As Double
            Return Math.Tan(deg * 2.0 * Math.PI / 360.0)
        End Function

        ''' <summary>
        '''  cos of an angle in degrees
        ''' </summary>
        Private Shared Function CosDeg(ByVal deg As Double) As Double
            Return Math.Cos(deg * 2.0 * Math.PI / 360.0)
        End Function

        ''' <summary>
        '''  Get time difference between location's longitude and the Meridian, in
        '''  hours. West of Meridian has a negative time difference
        ''' </summary>
        Private Shared Function GetHoursFromMeridian(ByVal longitude As Double) As Double
            Return longitude / DEG_PER_HOUR
        End Function

        ''' <summary>
        '''  Gets the approximate time of sunset or sunrise In _days_ since midnight
        '''  Jan 1st, assuming 6am and 6pm events. We need this figure to derive the
        '''  Sun's mean anomaly
        ''' </summary>
        Private Shared Function GetApproxTimeDays(ByVal dayOfYear As Integer, ByVal hoursFromMeridian As Double, ByVal isSunrise As Boolean) As Double
            Dim sunriseSunsetVar = If(isSunrise, 6.0, 18.0)
            Return dayOfYear + (sunriseSunsetVar - hoursFromMeridian) / 24
        End Function

        ''' <summary>
        '''  Calculate the Sun's mean anomaly in degrees, at sunrise or sunset, given
        '''  the longitude in degrees
        ''' </summary>
        Private Shared Function GetMeanAnomaly(ByVal dayOfYear As Integer, ByVal longitude As Double, ByVal isSunrise As Boolean) As Double
            Return 0.9856 * GetApproxTimeDays(dayOfYear, GetHoursFromMeridian(longitude), isSunrise) - 3.289
        End Function

        ''' <summary>
        '''  Calculates the Sun's true longitude in degrees. The result is an angle
        '''  gte 0 and lt 360. Requires the Sun's mean anomaly, also in degrees
        ''' </summary>
        Private Shared Function GetSunTrueLongitude(ByVal sunMeanAnomaly As Double) As Double
            Dim l = sunMeanAnomaly + 1.916 * SinDeg(sunMeanAnomaly) + 0.02 * SinDeg(2 * sunMeanAnomaly) + 282.634

            ' get longitude into 0-360 degree range
            If l >= 360.0 Then
                l = l - 360.0
            End If

            If l < 0 Then
                l = l + 360.0
            End If

            Return l
        End Function

        ''' <summary>
        '''  Calculates the Sun's right ascension in hours, given the Sun's true
        '''  longitude in degrees. Input and output are angles gte 0 and lt 360.
        ''' </summary>
        Private Shared Function GetSunRightAscensionHours(ByVal sunTrueLongitude As Double) As Double
            Dim a = 0.91764 * TanDeg(sunTrueLongitude)
            Dim ra = 360.0 / (2.0 * Math.PI) * Math.Atan(a)
            Dim lQuadrant = Math.Floor(sunTrueLongitude / 90.0) * 90.0
            Dim raQuadrant = Math.Floor(ra / 90.0) * 90.0
            ra = ra + (lQuadrant - raQuadrant)
            Return ra / DEG_PER_HOUR ' convert to hours
        End Function

        ''' <summary>
        '''  Gets the cosine of the Sun's local hour angle
        ''' </summary>
        Private Shared Function GetCosLocalHourAngle(ByVal sunTrueLongitude As Double, ByVal latitude As Double, ByVal zenith As Double) As Double
            Dim sinDec = 0.39782 * SinDeg(sunTrueLongitude)
            Dim cosDec = CosDeg(AsinDeg(sinDec))
            Return (CosDeg(zenith) - sinDec * SinDeg(latitude)) / (cosDec * CosDeg(latitude))
        End Function

        ''' <summary>
        '''  Calculate local mean time of rising or setting. By `local' is meant the
        '''  exact time at the location, assuming that there were no time zone. That
        '''  is, the time difference between the location and the Meridian depended
        '''  entirely on the longitude. We can't do anything with this time directly;
        '''  we must convert it to UTC and then to a local time. The result is
        '''  expressed as a fractional number of hours since midnight
        ''' </summary>
        Private Shared Function GetLocalMeanTime(ByVal localHour As Double, ByVal sunRightAscensionHours As Double, ByVal approxTimeDays As Double) As Double
            Return localHour + sunRightAscensionHours - 0.06571 * approxTimeDays - 6.622
        End Function

        ''' <summary>
        '''   Get sunrise or sunset time in UTC, according to flag.
        ''' </summary>
        ''' <paramname="date">The date</param>
        ''' <paramname="location">The location</param>
        ''' <paramname="zenith">Sun's zenith, in degrees</param>
        ''' <paramname="isSunrise">type of calculation to carry out sunrise or sunset.
        ''' </param>
        ''' <returns> the time as a double. If an error was encountered in the
        '''   calculation (expected behavior for some locations such as near
        '''   the poles, <seecref="Double.NaN"/> will be returned. </returns>
        Private Shared Function GetTimeUtc(ByVal [date] As Date, ByVal location As IGeoLocation, ByVal zenith As Double, ByVal isSunrise As Boolean) As Double
            Dim dayOfYear = [date].DayOfYear
            Dim sunMeanAnomaly = GetMeanAnomaly(dayOfYear, location.Longitude, isSunrise)
            Dim sunTrueLong = GetSunTrueLongitude(sunMeanAnomaly)
            Dim sunRightAscensionHours = GetSunRightAscensionHours(sunTrueLong)
            Dim cosLocalHourAngle = GetCosLocalHourAngle(sunTrueLong, location.Latitude, zenith)
            Dim localHourAngle As Double = 0

            If isSunrise Then
                If cosLocalHourAngle > 1 Then ' no rise. No need for an Exception
                    ' since the calculation
                    ' will return Double.NaN
                End If

                localHourAngle = 360.0 - AcosDeg(cosLocalHourAngle)
            Else

                If cosLocalHourAngle < -1 Then ' no SET. No need for an Exception
                    ' since the calculation
                    ' will return Double.NaN
                End If

                localHourAngle = AcosDeg(cosLocalHourAngle)
            End If

            Dim localHour = localHourAngle / DEG_PER_HOUR
            Dim localMeanTime = GetLocalMeanTime(localHour, sunRightAscensionHours, GetApproxTimeDays(dayOfYear, GetHoursFromMeridian(location.Longitude), isSunrise))
            Dim pocessedTime = localMeanTime - GetHoursFromMeridian(location.Longitude)

            While pocessedTime < 0.0
                pocessedTime += 24.0
            End While

            While pocessedTime >= 24.0
                pocessedTime -= 24.0
            End While

            Return pocessedTime
        End Function
    End Class
End Namespace
