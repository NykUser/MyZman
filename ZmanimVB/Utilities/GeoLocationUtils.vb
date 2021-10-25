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

Namespace Zmanim.Utilities
    ''' <summary>
    '''   A class for various location calculations
    '''   Most of the code in this class is ported from <ahref="http://www.movable-type.co.uk/">Chris Veness'</a>
    '''   <ahref="http://www.fsf.org/licensing/licenses/lgpl.html">LGPL</a> Javascript Implementation
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class GeoLocationUtils
        Private Shared DISTANCE As Integer
        Private Shared INITIAL_BEARING As Integer = 1
        Private Shared FINAL_BEARING As Integer = 2

        ''' <summary>
        ''' Calculate the initial <ahref="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ''' between this Object and a second Object passed to this method using <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ''' inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ''' Solutions of Geodesics on the Ellipsoid with application of nested
        ''' equations</a>", Survey Review, vol XXII no 176, 1975.
        ''' </summary>
        ''' <paramname="location">the destination location</param>
        ''' <paramname="destination">The destination.</param>
        ''' <returns></returns>
        Public Shared Function GetGeodesicInitialBearing(ByVal location As GeoLocation, ByVal destination As GeoLocation) As Double
            Return VincentyFormula(location, destination, INITIAL_BEARING)
        End Function

        ''' <summary>
        ''' Calculate the final <ahref="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ''' between this Object and a second Object passed to this method using <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ''' inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ''' Solutions of Geodesics on the Ellipsoid with application of nested
        ''' equations</a>", Survey Review, vol XXII no 176, 1975.
        ''' </summary>
        ''' <paramname="location">the destination location</param>
        ''' <paramname="destination">The destination.</param>
        ''' <returns></returns>
        Public Shared Function GetGeodesicFinalBearing(ByVal location As GeoLocation, ByVal destination As GeoLocation) As Double
            Return VincentyFormula(location, destination, FINAL_BEARING)
        End Function

        ''' <summary>
        ''' Calculate <ahref="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ''' distance</a> in Meters between this Object and a second Object passed to
        ''' this method using <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ''' inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ''' Solutions of Geodesics on the Ellipsoid with application of nested
        ''' equations</a>", Survey Review, vol XXII no 176, 1975.
        ''' </summary>
        ''' <paramname="location">the destination location</param>
        ''' <paramname="destination">The destination.</param>
        ''' <returns></returns>
        Public Shared Function GetGeodesicDistance(ByVal location As GeoLocation, ByVal destination As GeoLocation) As Double
            Return VincentyFormula(location, destination, DISTANCE)
        End Function

        ''' <summary>
        ''' Calculate <ahref="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ''' distance</a> in Meters between this Object and a second Object passed to
        ''' this method using <ahref="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ''' inverse formula See T Vincenty, "<ahref="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ''' Solutions of Geodesics on the Ellipsoid with application of nested
        ''' equations</a>", Survey Review, vol XXII no 176, 1975.
        ''' </summary>
        ''' <paramname="location">the destination location</param>
        ''' <paramname="destination">The destination.</param>
        ''' <paramname="formula">This formula calculates initial bearing (<seealsocref="INITIAL_BEARING"/>),
        ''' final bearing (<seealsocref="FINAL_BEARING"/>) and distance (<seealsocref="DISTANCE"/>).</param>
        ''' <returns></returns>
        Private Shared Function VincentyFormula(ByVal location As GeoLocation, ByVal destination As GeoLocation, ByVal formula As Integer) As Double
            Dim lA As Double = 6378137
            Dim lB = 6356752.3142
            Dim f = 1 / 298.257223563 ' WGS-84 ellipsiod
            Dim L = destination.Longitude - location.Longitude.ToRadians
            Dim U1 = Math.Atan((1 - f) * Math.Tan(location.Latitude.ToRadians))
            Dim U2 = Math.Atan((1 - f) * Math.Tan(destination.Latitude.ToRadians))
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
            If formula = GeoLocationUtils.DISTANCE Then Return distance
            If formula = INITIAL_BEARING Then Return fwdAz
            If formula = FINAL_BEARING Then Return revAz
            ' should never happpen

            Return Double.NaN
        End Function

        ''' <summary>
        ''' Returns the <ahref="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ''' bearing from the current location to the GeoLocation passed in.
        ''' </summary>
        ''' <paramname="location">destination location</param>
        ''' <paramname="destination">The destination.</param>
        ''' <returns>the bearing in degrees</returns>
        Public Shared Function GetRhumbLineBearing(ByVal location As GeoLocation, ByVal destination As GeoLocation) As Double
            Dim dLon = destination.Longitude - location.Longitude.ToRadians
            Dim dPhi = Math.Log(Math.Tan(destination.Latitude.ToRadians / 2 + Math.PI / 4) / Math.Tan(location.Latitude.ToRadians / 2 + Math.PI / 4))
            If Math.Abs(dLon) > Math.PI Then dLon = If(dLon > 0, -(2 * Math.PI - dLon), 2 * Math.PI + dLon)
            Return Math.Atan2(dLon, dPhi).ToDegree
        End Function

        ''' <summary>
        ''' Returns the <ahref="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ''' distance from the current location to the GeoLocation passed in.
        ''' Ported from <ahref="http://www.movable-type.co.uk/">Chris Veness'</a> Javascript Implementation
        ''' </summary>
        ''' <paramname="location">the destination location</param>
        ''' <paramname="destination">The destination.</param>
        ''' <returns>the distance in Meters</returns>
        Public Shared Function GetRhumbLineDistance(ByVal location As GeoLocation, ByVal destination As GeoLocation) As Double
            Dim R As Double = 6371 ' earth's mean radius in km
            Dim dLat = destination.Latitude - location.Latitude.ToRadians
            Dim dLon = Math.Abs(destination.Longitude - location.Longitude).ToRadians
            Dim dPhi = Math.Log(Math.Tan(destination.Longitude.ToRadians / 2 + Math.PI / 4) / Math.Tan(location.Latitude.ToRadians / 2 + Math.PI / 4))
            Dim q = If(Math.Abs(dLat) > 0.0000000001, dLat / dPhi, Math.Cos(location.Latitude.ToRadians))
            ' if dLon over 180° take shorter rhumb across 180° meridian:
            If dLon > Math.PI Then dLon = 2 * Math.PI - dLon
            Dim d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon)
            Return d * R
        End Function
    End Class
End Namespace
