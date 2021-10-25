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
Imports System.Text
Imports Zmanim.Extensions
Imports Zmanim.TimeZone

Namespace Zmanim.Utilities
    ''' <summary>
    '''   A class that contains location information such as latitude and longitude
    '''   required for astronomical calculations. The elevation field is not used by
    '''   most calculation engines and would be ignored if set. Check the documentation
    '''   for specific implementations of the <seealsocref="AstronomicalCalculator"/> to see if
    '''   elevation is calculated as part o the algorithm.
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class GeoLocation
        Implements IGeoLocation
        ''' <summary>
        '''   constant for milliseconds in a minute (60,000)
        ''' </summary>
        Private Const MINUTE_MILLIS As Long = 60 * 1000

        ''' <summary>
        '''   constant for milliseconds in an hour (3,600,000)
        ''' </summary>
        Private Const HOUR_MILLIS As Long = MINUTE_MILLIS * 60
        Private DISTANCE As Integer
        Private FINAL_BEARING As Integer = 2
        Private INITIAL_BEARING As Integer = 1
        Private elevationField As Double
        Private latitudeField As Double
        Private locationNameField As String
        Private longitudeField As Double
        Private timeZoneField As ITimeZone

        ''' <summary>
        '''  GeoLocation constructor with parameters for all required fields.
        ''' </summary>
        ''' <paramname="latitude">
        '''  the latitude in a double format such as 40.095965 for
        '''  Lakewood, NJ <br/> <b>Note: </b> For latitudes south of the
        '''  equator, a negative value should be used. </param>
        ''' <paramname="longitude">
        '''  double the longitude in a double format such as -74.222130 for
        '''  Lakewood, NJ. <br/> <b>Note: </b> For longitudes east of the
        '''  <ahref="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        '''    Meridian </a> (Greenwich), a negative value should be used. </param>
        ''' <paramname="timeZone">
        '''  the <c>TimeZone</c> for the location. </param>
        Public Sub New(ByVal latitude As Double, ByVal longitude As Double, ByVal timeZone As ITimeZone)
            Me.New(String.Empty, latitude, longitude, 0, timeZone)
        End Sub

        ''' <summary>
        '''  GeoLocation constructor with parameters for all required fields.
        ''' </summary>
        ''' <paramname="name">
        '''  The location name for display use such as &quot;Lakewood,
        '''  NJ&quot; </param>
        ''' <paramname="latitude">
        '''  the latitude in a double format such as 40.095965 for
        '''  Lakewood, NJ <br/> <b>Note: </b> For latitudes south of the
        '''  equator, a negative value should be used. </param>
        ''' <paramname="longitude">
        '''  double the longitude in a double format such as -74.222130 for
        '''  Lakewood, NJ. <br/> <b>Note: </b> For longitudes east of the
        '''  <ahref="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        '''    Meridian </a> (Greenwich), a negative value should be used. </param>
        ''' <paramname="timeZone">
        '''  the <c>TimeZone</c> for the location. </param>
        Public Sub New(ByVal name As String, ByVal latitude As Double, ByVal longitude As Double, ByVal timeZone As ITimeZone)
            Me.New(name, latitude, longitude, 0, timeZone)
        End Sub


        ''' <summary>
        '''  GeoLocation constructor with parameters for all required fields.
        ''' </summary>
        ''' <paramname="name">
        '''  The location name for display use such as &quot;Lakewood,
        '''  NJ&quot; </param>
        ''' <paramname="latitude">
        '''  the latitude in a double format such as 40.095965 for
        '''  Lakewood, NJ <br/> <b>Note: </b> For latitudes south of the
        '''  equator, a negative value should be used. </param>
        ''' <paramname="longitude">
        '''  double the longitude in a double format such as -74.222130 for
        '''  Lakewood, NJ. <br/> <b>Note: </b> For longitudes east of the
        '''  <ahref="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        '''    Meridian </a> (Greenwich), a negative value should be used. </param>
        ''' <paramname="elevation">
        '''  the elevation above sea level in Meters. Elevation is not used
        '''  in most algorithms used for calculating sunrise and set. </param>
        ''' <paramname="timeZone">
        '''  the <c>TimeZone</c> for the location. </param>
        Public Sub New(ByVal name As String, ByVal latitude As Double, ByVal longitude As Double, ByVal elevation As Double, ByVal timeZone As ITimeZone)
            LocationName = name
            Me.Latitude = latitude
            Me.Longitude = longitude
            Me.Elevation = elevation
            Me.TimeZone = timeZone
        End Sub


        ''' <summary>
        '''  Default GeoLocation constructor will set location to the Prime Meridian
        '''  at Greenwich, England and a TimeZone of GMT. The longitude will be set to
        '''  0 and the latitude will be 51.4772 to match the location of the
        '''  <ahref="http://www.rog.nmm.ac.uk">Royal Observatory, Greenwich </a>. No
        '''  daylight savings time will be used.
        ''' </summary>
        Public Sub New()
            LocationName = "Greenwich, England"
            Longitude = 0 ' added for clarity
            Latitude = 51.4772
            TimeZone = New OffsetTimeZone(0)
        End Sub

        ''' <summary>
        '''  Method to get the elevation in Meters.
        ''' </summary>
        ''' <value> Returns the elevation in Meters. </value>
        Public Overridable Property Elevation As Double Implements IGeoLocation.Elevation
            Get
                Return elevationField
            End Get
            Set(ByVal value As Double)

                If value < 0 Then
                    Throw New ArgumentException("Elevation cannot be negative")
                End If

                elevationField = value
            End Set
        End Property


        ''' <summary>
        '''  Method to set the latitude.
        ''' </summary>
        ''' <value>
        '''  The degrees of latitude to set. The values should be between
        '''  -90° and 90°. An IllegalArgumentException will be
        '''  thrown if the value exceeds the limit. For example 40.095965
        '''  would be used for Lakewood, NJ. &lt;b&gt;Note: &lt;/b&gt; For latitudes south of the
        '''  equator, a negative value should be used. </value>
        Public Overridable Property Latitude As Double Implements IGeoLocation.Latitude
            Set(ByVal value As Double)

                If value > 90 OrElse value < -90 Then
                    Throw New ArgumentException("Latitude must be between -90 and  90")
                End If

                latitudeField = value
            End Set
            Get
                Return latitudeField
            End Get
        End Property


        ''' <summary>
        '''  Method to set the latitude in degrees, minutes and seconds.
        ''' </summary>
        ''' <paramname="degrees">
        '''  The degrees of latitude to set between -90 and 90. An
        '''  IllegalArgumentException will be thrown if the value exceeds
        '''  the limit. For example 40 would be used for Lakewood, NJ. </param>
        ''' <paramname="minutes"> <ahref="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">minutes of arc</a> </param>
        ''' <paramname="seconds"> <ahref="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">seconds of arc</a> </param>
        ''' <paramname="direction">
        '''  N for north and S for south. An IllegalArgumentException will
        '''  be thrown if the value is not S or N. </param>
        Public Overridable Sub SetLatitude(ByVal degrees As Integer, ByVal minutes As Integer, ByVal seconds As Double, ByVal direction As String)
            Dim tempLat = degrees + (minutes + seconds / 60.0) / 60.0

            If tempLat > 90 OrElse tempLat < 0 Then
                Throw New ArgumentException("Latitude must be between 0 and  90. Use direction of S instead of negative.")
            End If

            If direction.Equals("S") Then
                tempLat *= -1
            ElseIf Not direction.Equals("N") Then
                Throw New ArgumentException("Latitude direction must be N or S")
            End If

            latitudeField = tempLat
        End Sub


        ''' <summary>
        '''  Method to set the longitude in a double format.
        ''' </summary>
        ''' <value>
        '''  The degrees of longitude to set in a double format between
        '''  -180° and 180°. An IllegalArgumentException will be
        '''  thrown if the value exceeds the limit. For example -74.2094
        '''  would be used for Lakewood, NJ. Note: for longitudes east of
        '''  the &lt;a href = &quot;http://en.wikipedia.org/wiki/Prime_Meridian&quot;&gt;Prime
        '''  Meridian&lt;/a&gt; (Greenwich) a negative value should be used. </value>
        Public Overridable Property Longitude As Double Implements IGeoLocation.Longitude
            Set(ByVal value As Double)

                If value > 180 OrElse value < -180 Then
                    Throw New ArgumentException("Longitude must be between -180 and  180")
                End If

                longitudeField = value
            End Set
            Get
                Return longitudeField
            End Get
        End Property


        ''' <summary>
        '''  Method to set the longitude in degrees, minutes and seconds.
        ''' </summary>
        ''' <paramname="degrees">
        '''  The degrees of longitude to set between -180 and 180. An
        '''  IllegalArgumentException will be thrown if the value exceeds
        '''  the limit. For example -74 would be used for Lakewood, NJ.
        '''  Note: for longitudes east of the <ahref="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        '''                                     Meridian </a> (Greenwich) a negative value should be used. </param>
        ''' <paramname="minutes"> <ahref="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">minutes of arc</a> </param>
        ''' <paramname="seconds"> <ahref="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">seconds of arc</a> </param>
        ''' <paramname="direction">
        '''  E for east of the Prime Meridian or W for west of it. An
        '''  IllegalArgumentException will be thrown if the value is not E
        '''  or W. </param>
        Public Overridable Sub SetLongitude(ByVal degrees As Integer, ByVal minutes As Integer, ByVal seconds As Double, ByVal direction As String)
            Dim longTemp = degrees + (minutes + seconds / 60.0) / 60.0

            If longTemp > 180 OrElse longitudeField < 0 Then
                Throw New ArgumentException("Longitude must be between 0 and  180. Use the ")
            End If

            If direction.Equals("W") Then
                longTemp *= -1
            ElseIf Not direction.Equals("E") Then
                Throw New ArgumentException("Longitude direction must be E or W")
            End If

            longitudeField = longTemp
        End Sub


        ''' <value> Returns the location name. </value>
        Public Overridable Property LocationName As String Implements IGeoLocation.LocationName
            Get
                Return locationNameField
            End Get
            Set(ByVal value As String)
                locationNameField = value
            End Set
        End Property


        ''' <value> Returns the timeZone. </value>
        Public Overridable Property TimeZone As ITimeZone Implements IGeoLocation.TimeZone
            Get
                Return timeZoneField
            End Get
            Set(ByVal value As ITimeZone)
                timeZoneField = value
            End Set
        End Property

        ''' <summary>
        ''' A method that will return the location's local mean time offset in
        ''' milliseconds from local standard time. The globe is split into 360°,
        ''' with 15° per hour of the day. For a local that is at a longitude that
        ''' is evenly divisible by 15 (longitude % 15 == 0), at solar
        ''' <seecref="AstronomicalCalendar.GetSunTransit">noon</see>
        ''' (with adjustment for the <ahref="http://en.wikipedia.org/wiki/Equation_of_time">equation of time</a>)
        ''' the sun should be directly overhead, so a user who is 1° west of this
        ''' will have noon at 4 minutes after standard time noon, and conversely, a
        ''' user who is 1° east of the 15° longitude will have noon at 11:56
        ''' AM.
        ''' </summary>
        ''' <paramname="date">The date used to get the UtcOffset.</param>
        ''' <returns>
        ''' the offset in milliseconds not accounting for Daylight saving
        ''' time. A positive value will be returned East of the timezone
        ''' line, and a negative value West of it.
        ''' </returns>
        Public Overridable Function GetLocalMeanTimeOffset(ByVal [date] As Date) As Long Implements IGeoLocation.GetLocalMeanTimeOffset
            Return Longitude * 4 * MINUTE_MILLIS - TimeZone.UtcOffset([date])
        End Function

        ''' <summary>
        '''  Calculate the initial <ahref="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        '''  between this Object and a second Object passed to this method using
        '''  <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        '''  inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        '''                                     Solutions of Geodesics on the Ellipsoid with application of nested
        '''                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ''' </summary>
        ''' <paramname="location">
        '''  the destination location </param>
        Public Overridable Function GetGeodesicInitialBearing(ByVal location As GeoLocation) As Double Implements IGeoLocation.GetGeodesicInitialBearing
            Return VincentyFormula(location, INITIAL_BEARING)
        End Function


        ''' <summary>
        '''  Calculate the final <ahref="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        '''  between this Object and a second Object passed to this method using
        '''  <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        '''  inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        '''                                     Solutions of Geodesics on the Ellipsoid with application of nested
        '''                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ''' </summary>
        ''' <paramname="location">
        '''  the destination location </param>
        Public Overridable Function GetGeodesicFinalBearing(ByVal location As GeoLocation) As Double Implements IGeoLocation.GetGeodesicFinalBearing
            Return VincentyFormula(location, FINAL_BEARING)
        End Function

        ''' <summary>
        '''  Calculate <ahref="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        '''              distance</a> in Meters between this Object and a second Object passed to
        '''  this method using <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        '''  inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        '''                                     Solutions of Geodesics on the Ellipsoid with application of nested
        '''                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ''' </summary>
        ''' <paramname="location">
        '''  the destination location </param>
        Public Overridable Function GetGeodesicDistance(ByVal location As GeoLocation) As Double Implements IGeoLocation.GetGeodesicDistance
            Return VincentyFormula(location, DISTANCE)
        End Function

        ''' <summary>
        '''  Calculate <ahref="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        '''              distance</a> in Meters between this Object and a second Object passed to
        '''  this method using <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        '''  inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        '''                                     Solutions of Geodesics on the Ellipsoid with application of nested
        '''                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ''' </summary>
        ''' <paramname="location">
        '''  the destination location </param>
        ''' <paramname="formula">
        '''  This formula calculates initial bearing (<seealsocref="INITIAL_BEARING"/>),
        '''  final bearing (<seealsocref="FINAL_BEARING"/>) and distance (<seealsocref="DISTANCE"/>). </param>
        Private Function VincentyFormula(ByVal location As GeoLocation, ByVal formula As Integer) As Double
            Dim lA As Double = 6378137
            Dim lB = 6356752.3142
            Dim f = 1 / 298.257223563 ' WGS-84 ellipsiod
            Dim L = location.Longitude - Longitude.ToRadians
            Dim U1 = Math.Atan((1 - f) * Math.Tan(Latitude.ToRadians))
            Dim U2 = Math.Atan((1 - f) * Math.Tan(location.Latitude.ToRadians))
            Dim sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1)
            Dim sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2)
            Dim lambda = L
            Dim lambdaP = 2 * Math.PI
            Dim iterLimit As Double = 20
            Dim sinLambda As Double = 0
            Dim cosLambda As Double = 0
            Dim sinSigma As Double = 0
            Dim cosSigma As Double = 0
            Dim sigma As Double = 0
            Dim sinAlpha As Double = 0
            Dim cosSqAlpha As Double = 0
            Dim cos2SigmaM As Double = 0
            Dim C As Double

            While Math.Abs(lambda - lambdaP) > 0.000000000001 AndAlso Threading.Interlocked.Decrement(CInt(iterLimit)) > 0
                sinLambda = Math.Sin(lambda)
                cosLambda = Math.Cos(lambda)
                sinSigma = Math.Sqrt(cosU2 * sinLambda * (cosU2 * sinLambda) + (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda))
                If sinSigma = 0 Then Return 0 ' co-incident points
                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda
                sigma = Math.Atan2(sinSigma, cosSigma)
                sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma
                cosSqAlpha = 1 - sinAlpha * sinAlpha
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha
                If Double.IsNaN(cos2SigmaM) Then cos2SigmaM = 0 ' equatorial line: cosSqAlpha=0 (§6)
                C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha))
                lambdaP = lambda
                lambda = L + (1 - C) * f * sinAlpha * (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)))
            End While

            If iterLimit = 0 Then Return Double.NaN ' formula failed to converge
            Dim uSq = cosSqAlpha * (lA * lA - lB * lB) / (lB * lB)
            Dim A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)))
            Dim B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)))
            Dim deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) - B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)))
            Dim distance = lB * A * (sigma - deltaSigma)

            ' initial bearing
            Dim fwdAz = Math.Atan2(cosU2 * sinLambda, cosU1 * sinU2 - sinU1 * cosU2 * cosLambda).ToDegree
            ' final bearing
            Dim revAz = Math.Atan2(cosU1 * sinLambda, -sinU1 * cosU2 + cosU1 * sinU2 * cosLambda).ToDegree

            If formula = Me.DISTANCE Then
                Return distance
            ElseIf formula = INITIAL_BEARING Then
                Return fwdAz
            ElseIf formula = FINAL_BEARING Then
                Return revAz ' should never happpen
            Else
                Return Double.NaN
            End If
        End Function


        ''' <summary>
        '''  Returns the <ahref="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        '''  bearing from the current location to the GeoLocation passed in.
        ''' </summary>
        ''' <paramname="location">
        '''  destination location </param>
        ''' <returns> the bearing in degrees </returns>
        Public Overridable Function GetRhumbLineBearing(ByVal location As GeoLocation) As Double Implements IGeoLocation.GetRhumbLineBearing
            Dim dLon = location.Longitude - Longitude.ToRadians
            Dim dPhi = Math.Log(Math.Tan(location.Latitude.ToRadians / 2 + Math.PI / 4) / Math.Tan(Latitude.ToRadians / 2 + Math.PI / 4))
            If Math.Abs(dLon) > Math.PI Then dLon = If(dLon > 0, -(2 * Math.PI - dLon), 2 * Math.PI + dLon)
            Return Math.Atan2(dLon, dPhi).ToDegree
        End Function


        ''' <summary>
        '''  Returns the <ahref="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        '''  distance from the current location to the GeoLocation passed in.
        ''' </summary>
        ''' <paramname="location">
        '''  the destination location </param>
        ''' <returns> the distance in Meters </returns>
        Public Overridable Function GetRhumbLineDistance(ByVal location As GeoLocation) As Double Implements IGeoLocation.GetRhumbLineDistance
            Dim R As Double = 6371 ' earth's mean radius in km
            Dim dLat = location.Latitude - Latitude.ToRadians
            Dim dLon = Math.Abs(location.Longitude - Longitude).ToRadians
            Dim dPhi = Math.Log(Math.Tan(location.Longitude.ToRadians / 2 + Math.PI / 4) / Math.Tan(Latitude.ToRadians / 2 + Math.PI / 4))
            Dim q = If(Math.Abs(dLat) > 0.0000000001, dLat / dPhi, Math.Cos(Latitude.ToRadians))
            ' if dLon over 180° take shorter rhumb across 180° meridian:
            If dLon > Math.PI Then dLon = 2 * Math.PI - dLon
            Dim d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon)
            Return d * R
        End Function


        ''' <summary>
        '''  A method that returns an XML formatted <c>String</c> representing
        '''  the serialized <c>Object</c>. Very similar to the toString
        '''  method but the return value is in an xml format. The format currently
        '''  used (subject to change) is:
        ''' 	
        '''  <code>
        '''    &lt;GeoLocation&gt;
        '''    &lt;LocationName&gt;Lakewood, NJ&lt;/LocationName&gt;
        '''    &lt;Latitude&gt;40.0828&amp;deg&lt;/Latitude&gt;
        '''    &lt;Longitude&gt;-74.2094&amp;deg&lt;/Longitude&gt;
        '''    &lt;Elevation&gt;0 Meters&lt;/Elevation&gt;
        '''    &lt;TimezoneName&gt;America/New_York&lt;/TimezoneName&gt;
        '''    &lt;TimeZoneDisplayName&gt;Eastern Standard Time&lt;/TimeZoneDisplayName&gt;
        '''    &lt;TimezoneGMTOffset&gt;-5&lt;/TimezoneGMTOffset&gt;
        '''    &lt;TimezoneDSTOffset&gt;1&lt;/TimezoneDSTOffset&gt;
        '''    &lt;/GeoLocation&gt;
        '''  </code>
        ''' </summary>
        ''' <returns> The XML formatted <code>String</code>. </returns>
        Public Overridable Function ToXml() As String
            Dim sb = New StringBuilder()
            sb.Append("<GeoLocation>" & Microsoft.VisualBasic.Constants.vbLf)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbTab & "<LocationName>")).Append(CStr(LocationName)).Append("</LocationName>" & Microsoft.VisualBasic.Constants.vbLf)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbTab & "<Latitude>")).Append(Latitude).Append(CStr("°")).Append("</Latitude>" & Microsoft.VisualBasic.Constants.vbLf)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbTab & "<Longitude>")).Append(Longitude).Append(CStr("°")).Append("</Longitude>" & Microsoft.VisualBasic.Constants.vbLf)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbTab & "<Elevation>")).Append(Elevation).Append(CStr(" Meters")).Append("</Elevation>" & Microsoft.VisualBasic.Constants.vbLf)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbTab & "<TimezoneName>")).Append(CStr(TimeZone.GetId())).Append("</TimezoneName>" & Microsoft.VisualBasic.Constants.vbLf)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbTab & "<TimeZoneDisplayName>")).Append(CStr(TimeZone.GetDisplayName())).Append("</TimeZoneDisplayName>" & Microsoft.VisualBasic.Constants.vbLf)
            'sb.Append("\t<TimezoneGMTOffset>").Append(getTimeZone().getRawOffset() / HOUR_MILLIS).Append("</TimezoneGMTOffset>\n")
            'sb.Append("\t<TimezoneDSTOffset>").Append(getTimeZone().getDSTSavings() / HOUR_MILLIS).Append("</TimezoneDSTOffset>\n")
            sb.Append("</GeoLocation>")
            Return sb.ToString()
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
            If Not (TypeOf obj Is GeoLocation) Then Return False
            Dim geo = CType(obj, GeoLocation)
            Return BitConverter.DoubleToInt64Bits(latitudeField) = BitConverter.DoubleToInt64Bits(geo.latitudeField) AndAlso BitConverter.DoubleToInt64Bits(longitudeField) = BitConverter.DoubleToInt64Bits(geo.longitudeField) AndAlso elevationField = geo.elevationField AndAlso If(Equals(locationNameField, Nothing), Equals(geo.locationNameField, Nothing), locationNameField.Equals(geo.locationNameField)) AndAlso If(timeZoneField Is Nothing, geo.timeZoneField Is Nothing, timeZoneField.Equals(geo.timeZoneField))
        End Function

        ''' <summary>
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        ''' </returns>
        Public Overrides Function GetHashCode() As Integer
            Dim result = 17
            Dim latLong = BitConverter.DoubleToInt64Bits(latitudeField)
            Dim lonLong = BitConverter.DoubleToInt64Bits(longitudeField)
            Dim elevLong = BitConverter.DoubleToInt64Bits(elevationField)
            Dim latInt = CInt(latLong Xor latLong >> 32)
            Dim lonInt = CInt(lonLong Xor lonLong >> 32)
            Dim elevInt = CInt(elevLong Xor elevLong >> 32)
            result = 37 * result + [GetType]().GetHashCode()
            result += 37 * result + latInt
            result += 37 * result + lonInt
            result += 37 * result + elevInt
            result += 37 * result + (If(Equals(locationNameField, Nothing), 0, locationNameField.GetHashCode()))
            result += 37 * result + (If(timeZoneField Is Nothing, 0, timeZoneField.GetHashCode()))
            Return result
        End Function

        ''' <summary>
        ''' Returns a <seecref="System.String"/> that represents this instance.
        ''' </summary>
        ''' <returns>
        ''' A <seecref="System.String"/> that represents this instance.
        ''' </returns>
        Public Overrides Function ToString() As String
            Dim sb = New StringBuilder()
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbLf & "Location Name:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab)).Append(LocationName)
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbLf & "Latitude:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab)).Append(Latitude).Append("°")
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbLf & "Longitude:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab)).Append(Longitude).Append("°")
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbLf & "Elevation:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab)).Append(Elevation).Append(" Meters")
            sb.Append(CStr(Microsoft.VisualBasic.Constants.vbLf & "Timezone Name:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab)).Append(TimeZone.GetId())
            '        
            '		 * sb.append("\nTimezone Display Name:\t\t").append(
            '		 * getTimeZone().getDisplayName());
            '		 
            ' 
            'sb.Append("\nTimezone GMT Offset:\t\t").Append(getTimeZone().getRawOffset() / HOUR_MILLIS);
            'sb.Append("\nTimezone DST Offset:\t\t").Append(getTimeZone().getDSTSavings() / HOUR_MILLIS);

            Return sb.ToString()
        End Function
    End Class
End Namespace
