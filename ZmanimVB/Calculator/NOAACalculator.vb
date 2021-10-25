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
    '''   Implementation of sunrise and sunset methods to calculate astronomical times based on the <ahref="http://noaa.gov">NOAA</a> algorithm.
    '''   This calculator uses the Java algorithm based on the implementation by <ahref="http://noaa.gov">NOAA - National Oceanic and Atmospheric
    '''                                                                            Administration</a>'s <ahref="http://www.srrb.noaa.gov/highlights/sunrise/sunrisehtml">Surface Radiation
    '''                                                                                                   Research Branch</a>. NOAA's <ahref="http://www.srrb.noaa.gov/highlights/sunrise/solareqns.PDF">implementation</a>
    '''   is based on equations from <ahref="http://www.willbell.com/math/mc1.htm">Astronomical Algorithms</a> by
    '''   <ahref="http://en.wikipedia.org/wiki/Jean_Meeus">Jean Meeus</a>. Added to
    '''   the algorithm is an adjustment of the zenith to account for elevation.
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class NOAACalculator
        Inherits AstronomicalCalculator

        ''' The <ahref="http://en.wikipedia.org/wiki/Julian_day">Julian day</a> of January 1, 2000
        Private Shared JULIAN_DAY_JAN_1_2000 As Double = 2451545.0

        ''' Julian days per century
        Private Shared JULIAN_DAYS_PER_CENTURY As Double = 36525.0

        ''' <summary>
        ''' Gets the name of the Calculator.
        ''' </summary>
        ''' <value>the descriptive name of the algorithm.</value>
        Public Overrides ReadOnly Property CalculatorName As String
            Get
                Return "US National Oceanic and Atmospheric Administration Algorithm"
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
            Dim adjustedZenith = AdjustZenith(zenith, If(adjustForElevation, dateWithLocation.Location.Elevation, 0))
            Dim sunrise = GetSunriseUTC(GetJulianDay(dateWithLocation.Date), dateWithLocation.Location.Latitude, -dateWithLocation.Location.Longitude, adjustedZenith)
            sunrise = sunrise / 60

            ' ensure that the time is >= 0 and < 24
            While sunrise < 0.0
                sunrise += 24.0
            End While

            While sunrise >= 24.0
                sunrise -= 24.0
            End While

            Return sunrise
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
            Dim sunset = GetSunsetUTC(GetJulianDay(dateWithLocation.Date), dateWithLocation.Location.Latitude, -dateWithLocation.Location.Longitude, AdjustZenith(zenith, elevation))
            sunset = sunset / 60

            ' ensure that the time is >= 0 and < 24
            While sunset < 0.0
                sunset += 24.0
            End While

            While sunset >= 24.0
                sunset -= 24.0
            End While

            Return sunset
        End Function

        ''' <summary>
        '''  Generate a Julian day from a .NET date
        ''' </summary>
        ''' <paramname="date">DateTime</param>
        ''' <returns> the Julian day corresponding to the date Note: Number is returned
        '''  for start of day. Fractional days should be added later. </returns>
        Private Shared Function GetJulianDay(ByVal [date] As Date) As Double
            Dim year = [date].Year
            Dim month = [date].Month
            Dim day = [date].Day

            If month <= 2 Then
                year -= 1
                month += 12
            End If

            Dim a As Integer = year / 100
            Dim b As Integer = 2 - a + a / 4
            Return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5
        End Function

        ''' <summary>
        ''' Convert <ahref="http://en.wikipedia.org/wiki/Julian_day">Julian day</a> to centuries since J2000.0.
        ''' </summary>
        ''' <paramname="julianDay">
        '''  the Julian Day to convert </param>
        ''' <returns> the T value corresponding to the Julian Day </returns>
        Private Shared Function GetJulianCenturiesFromJulianDay(ByVal julianDay As Double) As Double
            Return (julianDay - JULIAN_DAY_JAN_1_2000) / JULIAN_DAYS_PER_CENTURY
        End Function

        ''' <summary>
        ''' Convert centuries since J2000.0 to <ahref="http://en.wikipedia.org/wiki/Julian_day">Julian day</a>.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns>the Julian Day corresponding to the Julian centuries passed in</returns>
        Private Shared Function GetJulianDayFromJulianCenturies(ByVal julianCenturies As Double) As Double
            Return julianCenturies * JULIAN_DAYS_PER_CENTURY + JULIAN_DAY_JAN_1_2000
        End Function

        ''' <summary>
        ''' Returns the Geometric <ahref="http://en.wikipedia.org/wiki/Mean_longitude">Mean Longitude</a> of the Sun.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the Geometric Mean Longitude of the Sun in degrees </returns>
        Private Shared Function GetSunGeometricMeanLongitude(ByVal julianCenturies As Double) As Double
            Dim longitude = 280.46646 + julianCenturies * (36000.76983 + 0.0003032 * julianCenturies)

            While longitude > 360.0
                longitude -= 360.0
            End While

            While longitude < 0.0
                longitude += 360.0
            End While

            Return longitude ' in degrees
        End Function

        ''' <summary>
        '''  Returns the Geometric <ahref="http://en.wikipedia.org/wiki/Mean_anomaly">Mean Anomaly</a> of the Sun.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the Geometric Mean Anomaly of the Sun in degrees </returns>
        Private Shared Function GetSunGeometricMeanAnomaly(ByVal julianCenturies As Double) As Double
            Return 357.52911 + julianCenturies * (35999.05029 - 0.0001537 * julianCenturies) ' in degrees
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Eccentricity_%28orbit%29">eccentricity of earth's orbit</a>.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the unitless eccentricity </returns>
        Private Shared Function GetEarthOrbitEccentricity(ByVal julianCenturies As Double) As Double
            Dim e = 0.016708634 - julianCenturies * (0.000042037 + 0.0000001267 * julianCenturies)
            Return e ' unitless
        End Function

        ''' <summary>
        ''' Returns the <ahref="http://en.wikipedia.org/wiki/Equation_of_the_center">equation of center</a> for the sun.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the equation of center for the sun in degrees </returns>
        Private Shared Function GetSunEquationOfCenter(ByVal julianCenturies As Double) As Double
            Dim m = GetSunGeometricMeanAnomaly(julianCenturies)
            Dim mrad As Double = m.ToRadians()
            Dim sinm = Math.Sin(mrad)
            Dim sin2m = Math.Sin(mrad + mrad)
            Dim sin3m = Math.Sin(mrad + mrad + mrad)
            Return sinm * (1.914602 - julianCenturies * (0.004817 + 0.000014 * julianCenturies)) + sin2m * (0.019993 - 0.000101 * julianCenturies) + sin3m * 0.000289 ' in degrees
        End Function

        ''' <summary>
        '''  Calculate the true longitude of the sun
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the sun's true longitude in degrees </returns>
        Private Shared Function GetSunTrueLongitude(ByVal julianCenturies As Double) As Double
            Dim sunLongitude = GetSunGeometricMeanLongitude(julianCenturies)
            Dim center = GetSunEquationOfCenter(julianCenturies)
            Return sunLongitude + center ' in degrees
        End Function

        ''' <summary>
        '''  calculate the apparent longitude of the sun
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> sun's apparent longitude in degrees </returns>
        Private Shared Function GetSunApparentLongitude(ByVal julianCenturies As Double) As Double
            Dim sunTrueLongitude = GetSunTrueLongitude(julianCenturies)
            Dim omega = 125.04 - 1934.136 * julianCenturies
            Return sunTrueLongitude - 0.00569 - 0.00478 * Math.Sin(omega.ToRadians()) ' in degrees
        End Function

        ''' <summary>
        ''' Returns the mean <ahref="http://en.wikipedia.org/wiki/Axial_tilt">obliquity of the ecliptic</a> (Axial tilt).
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the mean obliquity in degrees </returns>
        Private Shared Function GetMeanObliquityOfEcliptic(ByVal julianCenturies As Double) As Double
            Dim seconds = 21.448 - julianCenturies * (46.815 + julianCenturies * (0.00059 - julianCenturies * 0.001813))
            Return 23.0 + (26.0 + seconds / 60.0) / 60.0 ' in degrees
        End Function

        ''' <summary>
        ''' Returns the corrected <ahref="http://en.wikipedia.org/wiki/Axial_tilt">obliquity of the ecliptic</a> (Axial tilt)
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> the corrected obliquity in degrees </returns>
        Private Shared Function GetObliquityCorrection(ByVal julianCenturies As Double) As Double
            Dim obliquityOfEcliptic = GetMeanObliquityOfEcliptic(julianCenturies)
            Dim omega = 125.04 - 1934.136 * julianCenturies
            Return obliquityOfEcliptic + 0.00256 * Math.Cos(omega.ToRadians()) ' in degrees
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Declination">declination</a> of the sun.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        Private Shared Function GetSunDeclination(ByVal julianCenturies As Double) As Double
            Dim obliquityCorrection = GetObliquityCorrection(julianCenturies)
            Dim lambda = GetSunApparentLongitude(julianCenturies)
            Dim sint As Double = Math.Sin(obliquityCorrection.ToRadians()) * Math.Sin(lambda.ToRadians())
            Return Math.Asin(sint).ToDegree() ' in degrees
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Equation_of_time">Equation of Time</a> - the difference between
        ''' true solar time and mean solar time
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <returns> equation of time in minutes of time </returns>
        Private Shared Function GetEquationOfTime(ByVal julianCenturies As Double) As Double
            Dim epsilon = GetObliquityCorrection(julianCenturies)
            Dim geomMeanLongSun = GetSunGeometricMeanLongitude(julianCenturies)
            Dim eccentricityEarthOrbit = GetEarthOrbitEccentricity(julianCenturies)
            Dim geomMeanAnomalySun = GetSunGeometricMeanAnomaly(julianCenturies)
            Dim y As Double = Math.Tan(epsilon.ToRadians() / 2.0)
            y *= y
            Dim sin2l0 As Double = Math.Sin(2.0 * geomMeanLongSun.ToRadians())
            Dim sinm As Double = Math.Sin(geomMeanAnomalySun.ToRadians())
            Dim cos2l0 As Double = Math.Cos(2.0 * geomMeanLongSun.ToRadians())
            Dim sin4l0 As Double = Math.Sin(4.0 * geomMeanLongSun.ToRadians())
            Dim sin2m As Double = Math.Sin(2.0 * geomMeanAnomalySun.ToRadians())
            Dim equationOfTime = y * sin2l0 - 2.0 * eccentricityEarthOrbit * sinm + 4.0 * eccentricityEarthOrbit * y * sinm * cos2l0 - 0.5 * y * y * sin4l0 - 1.25 * eccentricityEarthOrbit * eccentricityEarthOrbit * sin2m
            Return equationOfTime.ToDegree() * 4.0 ' in minutes of time
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Hour_angle">hour angle</a> of the sun at sunrise for the
        ''' latitude.
        ''' </summary>
        ''' <paramname="lat">,
        ''' the latitude of observer in degrees</param>
        ''' <paramname="solarDec">the declination angle of sun in degrees</param>
        ''' <paramname="zenith">The zenith.</param>
        ''' <returns>hour angle of sunrise in radians</returns>
        Private Shared Function GetSunHourAngleAtSunrise(ByVal lat As Double, ByVal solarDec As Double, ByVal zenith As Double) As Double
            Dim latRad As Double = lat.ToRadians()
            Dim sdRad As Double = solarDec.ToRadians()
            Return Math.Acos(Math.Cos(zenith.ToRadians()) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad)) ' in radians
        End Function

        ''' <summary>
        ''' Returns the <ahref="http://en.wikipedia.org/wiki/Hour_angle">hour angle</a> of the sun at sunset for the
	    ''' latitude.
        ''' </summary>
        ''' <paramname="lat">the latitude of observer in degrees</param>
        ''' <paramname="solarDec">the declination angle of sun in degrees</param>
        ''' <paramname="zenith">The zenith.</param>
        ''' <returns>
        ''' the hour angle of sunset in radians.
        ''' </returns>
        Private Shared Function GetSunHourAngleAtSunset(ByVal lat As Double, ByVal solarDec As Double, ByVal zenith As Double) As Double
            Return -GetSunHourAngleAtSunrise(lat, solarDec, zenith) ' in radians
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Celestial_coordinate_system">Solar Elevation</a> for the
        ''' horizontal coordinate system at the given location at the given time.Can be negative if the sun is below the
        ''' horizon.Not corrected for altitude.
        '''
        ''' <paramname="dateWithLocation">the date with location</param></summary> 
        Public Shared Function GetSolarElevation(ByVal dateWithLocation As IDateWithLocation) As Double
            Dim julianDay = GetJulianDay(dateWithLocation.Date)
            Dim julianCenturies = GetJulianCenturiesFromJulianDay(julianDay)
            Dim eot = GetEquationOfTime(julianCenturies)
            Dim longitude = dateWithLocation.Date.Hour + 12.0 + (dateWithLocation.Date.Minute + eot + dateWithLocation.Date.Second / 60.0) / 60.0
            longitude = -(longitude * 360.0 / 24.0) Mod 360.0
            Dim hourAngle_rad As Double = (dateWithLocation.Location.Longitude - longitude).ToRadians()
            Dim declination = GetSunDeclination(julianCenturies)
            Dim dec_rad As Double = declination.ToRadians()
            Dim lat_rad As Double = dateWithLocation.Location.Latitude.ToRadians()
            Return Math.Asin(Math.Sin(lat_rad) * Math.Sin(dec_rad) + Math.Cos(lat_rad) * Math.Cos(dec_rad) * Math.Cos(hourAngle_rad)).ToDegree()
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Universal_Coordinated_Time">Universal Coordinated Time</a> (UTC)
        ''' of sunrise for the given day at the given location on earth
        ''' </summary>
        ''' <paramname="julianDay">the julian day</param>
        ''' <paramname="latitude">the latitude of observer in degrees</param>
        ''' <paramname="longitude">the longitude of observer in degrees</param>
        ''' <paramname="zenith">The zenith.</param>
        ''' <returns>the time in minutes from zero Z</returns>
        Private Shared Function GetSunriseUTC(ByVal julianDay As Double, ByVal latitude As Double, ByVal longitude As Double, ByVal zenith As Double) As Double
            Dim julianCenturies = GetJulianCenturiesFromJulianDay(julianDay)

            ' Find the time of solar noon at the location, and use that declination.
            ' This is better than start of the Julian day

            Dim noonmin = GetSolarNoonUTC(julianCenturies, longitude)
            Dim tnoon = GetJulianCenturiesFromJulianDay(julianDay + noonmin / 1440.0)

            ' First pass to approximate sunrise (using solar noon)

            Dim eqTime = GetEquationOfTime(tnoon)
            Dim solarDec = GetSunDeclination(tnoon)
            Dim hourAngle = GetSunHourAngleAtSunrise(latitude, solarDec, zenith)
            Dim delta As Double = longitude - hourAngle.ToDegree()
            Dim timeDiff = 4 * delta ' in minutes of time
            Dim timeUTC = 720 + timeDiff - eqTime ' in minutes

            ' Second pass includes fractional jday in gamma calc

            Dim newt = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(julianCenturies) + timeUTC / 1440.0)
            eqTime = GetEquationOfTime(newt)
            solarDec = GetSunDeclination(newt)
            hourAngle = GetSunHourAngleAtSunrise(latitude, solarDec, zenith)
            delta = longitude - hourAngle.ToDegree()
            timeDiff = 4 * delta
            timeUTC = 720 + timeDiff - eqTime ' in minutes
            Return timeUTC
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Universal_Coordinated_Time">Universal Coordinated Time</a> (UTC)
        ''' of<ahref="http://en.wikipedia.org/wiki/Noon#Solar_noon"> solar noon</a> for the given day at the given location
        ''' on earth.
        ''' </summary>
        ''' <paramname="julianCenturies">
        '''  the number of Julian centuries since J2000.0 </param>
        ''' <paramname="longitude">
        '''  the longitude of observer in degrees </param>
        ''' <returns> the time in minutes from zero Z </returns>
        Private Shared Function GetSolarNoonUTC(ByVal julianCenturies As Double, ByVal longitude As Double) As Double
            ' First pass uses approximate solar noon to calculate eqtime
            Dim tnoon = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(julianCenturies) + longitude / 360.0)
            Dim eqTime = GetEquationOfTime(tnoon)
            Dim solNoonUTC = 720 + longitude * 4 - eqTime ' min
            Dim newt = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(julianCenturies) - 0.5 + solNoonUTC / 1440.0)
            eqTime = GetEquationOfTime(newt)
            Return 720 + longitude * 4 - eqTime ' min
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Universal_Coordinated_Time">Universal Coordinated Time</a> (UTC)
        ''' of sunset for the given day at the given location on earth
        ''' </summary>
        ''' <paramname="julianDay">
        '''  the julian day </param>
        ''' <paramname="latitude">
        '''  the latitude of observer in degrees </param>
        ''' <paramname="longitude"> :
        '''  longitude of observer in degrees </param>
        ''' <paramname="zenith"> </param>
        ''' <returns> the time in minutes from zero Z </returns>
        Private Shared Function GetSunsetUTC(ByVal julianDay As Double, ByVal latitude As Double, ByVal longitude As Double, ByVal zenith As Double) As Double
            Dim t = GetJulianCenturiesFromJulianDay(julianDay)

            ' Find the time of solar noon at the location, and use
            ' that declination. This is better than start of the
            ' Julian day

            Dim noonmin = GetSolarNoonUTC(t, longitude)
            Dim tnoon = GetJulianCenturiesFromJulianDay(julianDay + noonmin / 1440.0)

            ' First calculates sunrise and approx length of day

            Dim eqTime = GetEquationOfTime(tnoon)
            Dim solarDec = GetSunDeclination(tnoon)
            Dim hourAngle = GetSunHourAngleAtSunset(latitude, solarDec, zenith)
            Dim delta = longitude - hourAngle.ToDegree
            Dim timeDiff = 4 * delta
            Dim timeUTC = 720 + timeDiff - eqTime

            ' first pass used to include fractional day in gamma calc

            Dim newt = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(t) + timeUTC / 1440.0)
            eqTime = GetEquationOfTime(newt)
            solarDec = GetSunDeclination(newt)
            hourAngle = GetSunHourAngleAtSunset(latitude, solarDec, zenith)
            delta = longitude - hourAngle.ToDegree
            timeDiff = 4 * delta
            Return 720 + timeDiff - eqTime ' in minutes
        End Function
    End Class
End Namespace
