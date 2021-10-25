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
Imports Zmanim.Extensions

Namespace Zmanim.Calculator
    ''' <summary>
    '''   Implementation of sunrise and sunset methods to calculate astronomical times.
    '''   This implementation is a port of the C++ algorithm written by Ken Bloom for
    '''   the sourceforge.net <ahref="http://sourceforge.net/projects/zmanim/">Zmanim</a>
    '''   project. Ken's algorithm is based on the US Naval Almanac algorithm. Added to
    '''   Ken's code is adjustment of the zenith to account for elevation.
    ''' </summary>
    ''' <author>Ken Bloom</author>
    ''' <author>Eliyahu Hershfeld</author>
    ''' <remarks>
    '''   Changed to LGPL with permission from the authors.
    ''' </remarks>
    Public Class ZmanimCalculator
        Inherits AstronomicalCalculator

        ''' <summary>
        '''   Gets the name of the calculator/.
        ''' </summary>
        ''' <value></value>
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
        ''' <paramname="adjustForElevation"></param>
        ''' <returns>
        ''' The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        ''' 5.75.0. If an error was encountered in the calculation (expected
        ''' behavior for some locations such as near the poles,
        ''' <seecref="Double.NaN"/> will be returned.
        ''' </returns>
        Public Overrides Function GetUtcSunrise(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean) As Double
            Return GetUtcSunriseSunset(dateWithLocation, zenith, adjustForElevation, True)
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
        Public Overrides Function GetUtcSunset(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean) As Double
            Return GetUtcSunriseSunset(dateWithLocation, zenith, adjustForElevation, False)
        End Function

        Private Function GetUtcSunriseSunset(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean, ByVal isSunrise As Boolean) As Double
            Dim elevation = If(adjustForElevation, dateWithLocation.Location.Elevation, 0)
            Dim adjustedZenith = AdjustZenith(zenith, elevation)

            ' step 1: First calculate the day of the year
            Dim dayOfYear = dateWithLocation.Date.DayOfYear

            ' step 2: convert the longitude to hour value and calculate an
            ' approximate time
            Dim lngHour = dateWithLocation.Location.Longitude / 15
            Dim t = dayOfYear + (If(isSunrise, 6, 18) - lngHour) / 24

            ' step 3: calculate the sun's mean anomaly
            Dim meanAnomaly = 0.9856 * t - 3.289

            ' step 4: calculate the sun's true longitude
            Dim trueLongitude As Double = meanAnomaly + (1.916 * Math.Sin(meanAnomaly.ToRadians())) + (0.02 * Math.Sin((2 * meanAnomaly).ToRadians())) + 282.634

            While trueLongitude < 0
                trueLongitude = trueLongitude + 360
            End While

            While trueLongitude >= 360
                trueLongitude = trueLongitude - 360
            End While

            ' step 5a: calculate the sun's right ascension
            Dim rAscension As Double = Math.Atan(0.91764 * Math.Tan(trueLongitude.ToRadians())).ToDegree()

            While rAscension < 0
                rAscension = rAscension + 360
            End While

            While rAscension >= 360
                rAscension = rAscension - 360
            End While

            ' step 5b: right ascension value needs to be in the same quadrant as L
            Dim lQuadrant = Math.Floor(trueLongitude / 90) * 90
            Dim rQuadrant = Math.Floor(rAscension / 90) * 90
            rAscension = rAscension + (lQuadrant - rQuadrant)

            ' step 5c: right ascension value needs to be converted into hours
            rAscension /= 15

            ' step 6: calculate the sun's declination
            Dim sinDec As Double = 0.39782 * Math.Sin(trueLongitude.ToRadians())
            Dim cosDec = Math.Cos(Math.Asin(sinDec))
            Dim latitudeRadians = dateWithLocation.Location.Latitude.ToRadians()

            ' step 7a: calculate the sun's local hour angle
            Dim cosH As Double = (Math.Cos(adjustedZenith.ToRadians()) - sinDec * Math.Sin(latitudeRadians)) / (cosDec * Math.Cos(latitudeRadians))

            ' step 7b: finish calculating H and convert into hours
            Dim hours As Double = Math.Acos(cosH).ToDegree()
            If isSunrise Then hours = 360 - hours
            hours = hours / 15

            ' step 8: calculate local mean time

            Dim localMeanTime = hours + rAscension - 0.06571 * t - 6.622

            ' step 9: convert to UTC
            Dim utc = localMeanTime - lngHour

            While utc < 0
                utc = utc + 24
            End While

            While utc >= 24
                utc = utc - 24
            End While

            Return utc
        End Function
    End Class
End Namespace
