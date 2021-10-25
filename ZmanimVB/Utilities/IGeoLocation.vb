Imports TimeZone
Imports Zmanim.TimeZone

Namespace Zmanim.Utilities
    ''' <summary>
    ''' Contains location information such as latitude and longitude ... and TimeZone.
    ''' </summary>
    Public Interface IGeoLocation
        ''' <summary>
        '''  Method to get the elevation in Meters.
        ''' </summary>
        ''' <value> Returns the elevation in Meters. </value>
        Property Elevation As Double

        ''' <summary>
        '''  Method to set the latitude.
        ''' </summary>
        ''' <value>
        '''  The degrees of latitude to set. The values should be between
        '''  -90° and 90°. An IllegalArgumentException will be
        '''  thrown if the value exceeds the limit. For example 40.095965
        '''  would be used for Lakewood, NJ. &lt;b&gt;Note: &lt;/b&gt; For latitudes south of the
        '''  equator, a negative value should be used. </value>
        Property Latitude As Double

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
        Property Longitude As Double

        ''' <value> Returns the location name. </value>
        Property LocationName As String

        ''' <value> Returns the timeZone. </value>
        Property TimeZone As ITimeZone

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
        Function GetLocalMeanTimeOffset(ByVal [date] As Date) As Long

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
        Function GetGeodesicInitialBearing(ByVal location As GeoLocation) As Double

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
        Function GetGeodesicFinalBearing(ByVal location As GeoLocation) As Double

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
        Function GetGeodesicDistance(ByVal location As GeoLocation) As Double

        ''' <summary>
        '''  Returns the <ahref="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        '''  bearing from the current location to the GeoLocation passed in.
        ''' </summary>
        ''' <paramname="location">
        '''  destination location </param>
        ''' <returns> the bearing in degrees </returns>
        Function GetRhumbLineBearing(ByVal location As GeoLocation) As Double

        ''' <summary>
        '''  Returns the <ahref="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        '''  distance from the current location to the GeoLocation passed in.
        ''' </summary>
        ''' <paramname="location">
        '''  the destination location </param>
        ''' <returns> the distance in Meters </returns>
        Function GetRhumbLineDistance(ByVal location As GeoLocation) As Double
    End Interface
End Namespace
