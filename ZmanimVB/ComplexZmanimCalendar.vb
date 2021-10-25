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
    ''' This class extends ZmanimCalendar and provides many more zmanim than
    ''' available in the ZmanimCalendar. The basis for most zmanim in this class are
    ''' from the <em>sefer</em> <b>Yisroel Vehazmanim</b> by <b>Rabbi Yisroel Dovid  Harfenes</b>. <br/>
    ''' For an example of the number of different <em>zmanim</em> made available by
    ''' this class, there are methods to return 12 different calculations for
    ''' <em>alos</em> (dawn) available in this class. The real power of this API is
    ''' the ease in calculating <em>zmanim</em> that are not part of the API. The
    ''' methods for doing <em>zmanim</em> calculations not present in this or it's
    ''' superclass the <seecref="ZmanimCalendar"/> are contained in the
    ''' <seecref="AstronomicalCalendar"/>, the base class of the calendars in our API
    ''' since they are generic methods for calculating time based on degrees or time
    ''' before or after <seecref="AstronomicalCalendar.GetSunrise">sunrise"</see> and <seecref="AstronomicalCalendar.GetSunset">sunset</see> and
    ''' are of interest for calculation beyond <em>zmanim</em> calculations. Here are
    ''' some examples: <br/>
    ''' First create the Calendar for the location you would like to calculate:
    ''' <example>
    ''' 		<code>
    ''' string locationName = "Lakewood, NJ"
    ''' double latitude = 40.0828; //Lakewood, NJ
    ''' double longitude = -74.2094; //Lakewood, NJ
    ''' double elevation = 0;
    ''' ITimeZone timeZone = new JavaTimeZone("America/New_York");
    ''' GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    ''' elevation, timeZone);
    ''' ComplexZmanimCalendar czc = new ComplexZmanimCalendar(DateTime.Now, location);
    ''' </code>
    ''' 	</example>
    ''' Note: For locations such as Israel where the beginning and end of daylight
    ''' savings time can fluctuate from year to year create a
    ''' <seecref="Zmanim.TimeZone.ITimeZone"/> with the known start and end of DST. <br/>
    ''' To get alos calculated as 14° below the horizon (as calculated in the
    ''' calendars published in Montreal) use:
    ''' <code>
    ''' DateTime alos14 = czc.getSunriseOffsetByDegrees(14);
    ''' </code>
    ''' To get <em>mincha gedola</em> calculated based on the MGA using a <em>shaah zmanis</em> based on the day starting 16.1° below the horizon (and ending
    ''' 16.1° after sunset the following calculation can be used:
    ''' <code>
    ''' DateTime minchaGedola = czc.getTimeOffset(czc.getAlos16point1Degrees(),
    ''' czc.getShaahZmanis16Point1Degrees() * 6.5);
    ''' </code>
    ''' A little more complex example would be calculating <em>plag hamincha</em>
    ''' based on a shaah zmanis that was not present in this class. While a drop more
    ''' complex it is still rather easy. For example if you wanted to calculate
    ''' <em>plag</em> based on the day starting 12° before sunrise and ending
    ''' 12° after sunset as calculated in the calendars in Manchester, England
    ''' (there is nothing that would prevent your calculating the day using sunrise
    ''' and sunset offsets that are not identical degrees, but this would lead to
    ''' chatzos being a time other than the <seecref="AstronomicalCalendar.GetSunTransit">solar transit</see>
    ''' (solar midday)). The steps involved would be to first calculate the
    ''' <em>shaah zmanis</em> and than use that time in milliseconds to calculate
    ''' 10.75 hours after sunrise starting at 12° before sunset
    ''' <code>
    ''' long shaahZmanis = czc.getTemporalHour(czc.getSunriseOffsetByDegrees(12),
    ''' czc.getSunsetOffsetByDegrees(12));
    ''' DateTime plag = getTimeOffset(czc.getSunriseOffsetByDegrees(12),
    ''' shaahZmanis * 10.75);
    ''' </code>
    ''' 	<h2>Disclaimer:</h2> While I did my best to get accurate results please do
    ''' not rely on these zmanim for <em>halacha lemaaseh</em>
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class ComplexZmanimCalendar
        Inherits ZmanimCalendar
        ''' <summary>
        '''  The zenith of 3.7° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>tzais</em>
        '''  (nightfall) according to some opinions. This calculation is based on the
        '''  opinion of the Geonim that <em>tzais</em> is the time it takes to walk
        '''  3/4 of a Mil at 18 minutes a Mil, or 13.5 minutes after sunset. The sun
        '''  is 3.7° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see> at this time
        '''  in Jerusalem on March 16, about 4 days before the equinox, the day that a
        '''  solar hour is one hour.
        ''' </summary>
        Protected Friend Const ZENITH_3_POINT_7 As Double = GEOMETRIC_ZENITH + 3.7

        ''' <summary>
        '''  The zenith of 5.95° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>tzais</em>
        '''  (nightfall) according to some opinions. This calculation is based on the
        '''  position of the sun 24 minutes after sunset in Jerusalem on March 16,
        '''  about 4 days before the equinox, the day that a solar hour is one hour,
        '''  which calculates to 5.95° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetTzaisGeonim5Point95Degrees()"/>
        Protected Friend Const ZENITH_5_POINT_95 As Double = GEOMETRIC_ZENITH + 5.95

        ''' <summary>
        '''  The zenith of 7.083° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This is often referred to as 7°5' or 7° and 5 minutes.
        '''  This calculation is used for calculating <em>alos</em> (dawn) and
        '''  <em>tzais</em> (nightfall) according to some opinions. This calculation
        '''  is based on the position of the sun 30 minutes after sunset in Jerusalem
        '''  on March 16, about 4 days before the equinox, the day that a solar hour
        '''  is one hour, which calculates to 7.0833333° below
        '''  <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>. This is time some opinions
        '''  consider dark enough for 3 stars to be visible. This is the opinion of
        '''  the Shu"t Melamed Leho'il, Shu"t Binyan Tziyon, Tenuvas Sadeh and very
        '''  close to the time of the Mekor Chesed on the Sefer chasidim.
        ''' </summary>
        ''' <seealsocref="GetTzaisGeonim7Point083Degrees()"/>
        ''' <seealsocref="GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees()"/>
        Protected Friend Const ZENITH_7_POINT_083 As Double = GEOMETRIC_ZENITH + 7 + 5.0 / 60

        ''' <summary>
        '''  The zenith of 10.2° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>misheyakir</em>
        '''  according to some opinions. This calculation is based on the position of
        '''  the sun 45 minutes before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on
        '''  March 16, about 4 days before the equinox, the day that a solar hour is
        '''  one hour which calculates to 10.2° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetMisheyakir10Point2Degrees()"/>
        Protected Friend Const ZENITH_10_POINT_2 As Double = GEOMETRIC_ZENITH + 10.2

        ''' <summary>
        '''  The zenith of 11° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>misheyakir</em>
        '''  according to some opinions. This calculation is based on the position of
        '''  the sun 48 minutes before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on
        '''  March 16, about 4 days before the equinox, the day that a solar hour is
        '''  one hour which calculates to 11° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetMisheyakir11Degrees()"/>
        Protected Friend Const ZENITH_11_DEGREES As Double = GEOMETRIC_ZENITH + 11

        ''' <summary>
        '''  The zenith of 11.5° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>misheyakir</em>
        '''  according to some opinions. This calculation is based on the position of
        '''  the sun 52 minutes before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on
        '''  March 16, about 4 days before the equinox, the day that a solar hour is
        '''  one hour which calculates to 11.5° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetMisheyakir11Point5Degrees()"/>
        Protected Friend Const ZENITH_11_POINT_5 As Double = GEOMETRIC_ZENITH + 11.5

        ''' <summary>
        '''  The zenith of 13° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating
        '''  <em>Rabainu Tam's bain hashmashos</em> according to some opinions. <br/>
        '''  <br/>
        ''' NOTE: See comments on <seecref="GetBainHasmashosRT13Point24Degrees"/>for additional details about the degrees
        ''' </summary>
        Protected Friend Const ZENITH_13_POINT_24 As Double = GEOMETRIC_ZENITH + 13.24

        ''' <summary>
        '''  The zenith of 19.8° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>alos</em> (dawn)
        '''  and <em>tzais</em> (nightfall) according to some opinions. This
        '''  calculation is based on the position of the sun 90 minutes after sunset
        '''  in Jerusalem on March 16, about 4 days before the equinox, the day that a
        '''  solar hour is one hour which calculates to 19.8° below
        '''  <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetTzais19Point8Degrees()"/>
        ''' <seealsocref="GetAlos19Point8Degrees()"/>
        ''' <seealsocref="GetAlos90()"/>
        ''' <seealsocref="GetTzais90()"/>
        Protected Friend Const ZENITH_19_POINT_8 As Double = GEOMETRIC_ZENITH + 19.8

        ''' <summary>
        '''  The zenith of 26° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>alos</em> (dawn)
        '''  and <em>tzais</em> (nightfall) according to some opinions. This
        '''  calculation is based on the position of the sun <seecref="GetAlos120()">120 minutes</see>
        '''  after sunset in Jerusalem on March 16, about 4 days before the
        '''  equinox, the day that a solar hour is one hour which calculates to
        '''  26° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetAlos26Degrees()"/>
        ''' <seealsocref="GetTzais26Degrees()"/>
        ''' <seealsocref="GetAlos120()"/>
        ''' <seealsocref="GetTzais120()"/>
        Protected Friend Const ZENITH_26_DEGREES As Double = GEOMETRIC_ZENITH + 26.0

        ''' NOTE: Experimental and may not make the final 1.3 cut
        ''' <summary>
        '''  The zenith of 4.37° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>tzais</em>
        '''  (nightfall) according to some opinions. This calculation is based on the
        '''  position of the sun <seecref="GetTzaisGeonim4Point37Degrees()">16 7/8 minutes</see>
        '''  after sunset (3/4 of a 22.5 minute Mil) in Jerusalem on March
        '''  16, about 4 days before the equinox, the day that a solar hour is one
        '''  hour which calculates to 4.37° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetTzaisGeonim4Point37Degrees()"/>
        Protected Friend Const ZENITH_4_POINT_37 As Double = GEOMETRIC_ZENITH + 4.37

        ''' <summary>
        '''  The zenith of 4.61° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>tzais</em>
        '''  (nightfall) according to some opinions. This calculation is based on the
        '''  position of the sun <seecref="GetTzaisGeonim4Point37Degrees">18 minutes</see>
        '''  after sunset (3/4 of a 24 minute Mil) in Jerusalem on March 16, about 4
        '''  days before the equinox, the day that a solar hour is one hour which
        '''  calculates to 4.61° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ''' </summary>
        ''' <seealsocref="GetTzaisGeonim4Point61Degrees()"/>
        Protected Friend Const ZENITH_4_POINT_61 As Double = GEOMETRIC_ZENITH + 4.61

        ''' <summary>
        ''' The zenith of 4.8° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>(90°).
        ''' </summary>
        Protected Friend Const ZENITH_4_POINT_8 As Double = GEOMETRIC_ZENITH + 4.8

        ''' <summary>
        '''  The zenith of 3.65° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        '''  (90°). This calculation is used for calculating <em>tzais</em>
        '''  (nightfall) according to some opinions. This calculation is based on the
        '''  position of the sun <seecref="GetTzaisGeonim3Point65Degrees">13.5 minutes</see>
        '''  after sunset (3/4 of an 18 minute Mil) in Jerusalem on March 16, about 4
        '''  days before the equinox, the day that a solar hour is one hour which
        '''  calculates to 3.65° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">see zenith</see>
        ''' </summary>
        ''' <seealsocref="GetTzaisGeonim3Point65Degrees()"/>
        Protected Friend Const ZENITH_3_POINT_65 As Double = GEOMETRIC_ZENITH + 3.65
        Protected Friend Const ZENITH_3_POINT_676 As Double = GEOMETRIC_ZENITH + 3.676

        ''' <summary>
        ''' The zenith of 5.88° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ''' (90°).
        ''' </summary>
        Protected Friend Const ZENITH_5_POINT_88 As Double = GEOMETRIC_ZENITH + 5.88
        Private ateretTorahSunsetOffsetField As Double = 40

        ''' <summary>
        ''' Initializes a new instance of the <seecref="ComplexZmanimCalendar"/> class.
        ''' </summary>
        ''' <paramname="location">The location.</param>
        Public Sub New(ByVal location As IGeoLocation)
            MyBase.New(location)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="ComplexZmanimCalendar"/> class.
        ''' </summary>
        ''' <paramname="date">The date.</param>
        ''' <paramname="location">The location.</param>
        Public Sub New(ByVal [date] As DateTime, ByVal location As IGeoLocation)
            MyBase.New([date], location)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="ComplexZmanimCalendar"/> class.
        ''' </summary>
        ''' <paramname="dateWithLocation">The date with location.</param>
        Public Sub New(ByVal dateWithLocation As IDateWithLocation)
            MyBase.New(dateWithLocation)
        End Sub

        ''' <summary>
        ''' Default constructor will set a default <seecref="GeoLocation"/>,
        ''' a default <seecref="AstronomicalCalculator.GetDefault"> AstronomicalCalculator</see>
        ''' and default the calendar to the current date.
        ''' </summary>
        ''' <seealsocref="AstronomicalCalendar"/>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a 19.8° dip. This calculation divides the day based on the opinion of
        ''' the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        ''' when the sun is 19.8° below the eastern geometric horizon before
        ''' sunrise. Dusk for this is when the sun is 19.8° below the western
        ''' geometric horizon after sunset. This day is split into 12 equal parts
        ''' with each part being a <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as northern and southern locations even south of the Arctic
        ''' Circle and north of the Antarctic Circle where the sun may not
        ''' reach low enough below the horizon for this calculation, a
        ''' <seealsocref="Long.MinValue"/> will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis19Point8Degrees() As Long
            Return MyBase.GetTemporalHour(GetAlos19Point8Degrees(), GetTzais19Point8Degrees())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a 18° dip. This calculation divides the day based on the opinion of
        ''' the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        ''' when the sun is 18° below the eastern geometric horizon before
        ''' sunrise. Dusk for this is when the sun is 18° below the western
        ''' geometric horizon after sunset. This day is split into 12 equal parts
        ''' with each part being a <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as northern and southern locations even south of the Arctic
        ''' Circle and north of the Antarctic Circle where the sun may not
        ''' reach low enough below the horizon for this calculation, a
        ''' <seealsocref="Long.MinValue"/> will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis18Degrees() As Long
            Return MyBase.GetTemporalHour(GetAlos18Degrees(), GetTzais18Degrees())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a dip of 26°. This calculation divides the day based on the opinion
        ''' of the MGA that the day runs from dawn to dusk. Dawn for this calculation
        ''' is when the sun is <seecref="GetAlos26Degrees">26°</see> below the eastern
        ''' geometric horizon before sunrise. Dusk for this is when the sun is
        ''' <seecref="GetTzais26Degrees">26°</see> below the western geometric horizon
        ''' after sunset. This day is split into 12 equal parts with each part being
        ''' a <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a
        ''' <em>shaah zmanis</em>. If the calculation can't be computed such
        ''' as northern and southern locations even south of the Arctic
        ''' Circle and north of the Antarctic Circle where the sun may not
        ''' reach low enough below the horizon for this calculation, a
        ''' <seealsocref="Long.MinValue"/> will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis26Degrees() As Long
            Return MyBase.GetTemporalHour(GetAlos26Degrees(), GetTzais26Degrees())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a dip of 16.1°. This calculation divides the day based on the opinion
        ''' that the day runs from dawn to dusk. Dawn for this calculation is when
        ''' the sun is 16.1° below the eastern geometric horizon before sunrise
        ''' and dusk is when the sun is 16.1° below the western geometric horizon
        ''' after sunset. This day is split into 12 equal parts with each part being
        ''' a <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as northern and southern locations even south of the Arctic
        ''' Circle and north of the Antarctic Circle where the sun may not
        ''' reach low enough below the horizon for this calculation, a
        ''' <seealsocref="Long.MinValue"/> will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        ''' <seealsocref="GetTzais16Point1Degrees()"/>
        ''' <seealsocref="GetSofZmanShmaMGA16Point1Degrees()"/>
        ''' <seealsocref="GetSofZmanTfilaMGA16Point1Degrees()"/>
        ''' <seealsocref="GetMinchaGedola16Point1Degrees()"/>
        ''' <seealsocref="GetMinchaKetana16Point1Degrees()"/>
        ''' <seealsocref="GetPlagHamincha16Point1Degrees()"/>
        Public Overridable Function GetShaahZmanis16Point1Degrees() As Long
            Return MyBase.GetTemporalHour(GetAlos16Point1Degrees(), GetTzais16Point1Degrees())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (solar hour) according to the
        ''' opinion of the MGA. This calculation divides the day based on the opinion
        ''' of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        ''' calculation is 60 minutes before sunrise and dusk is 60 minutes after
        ''' sunset. This day is split into 12 equal parts with each part being a
        ''' <em>shaah zmanis</em>. Alternate mothods of calculating a
        ''' <em>shaah zmanis</em> are available in the subclass
        ''' <seecref="ComplexZmanimCalendar"/>
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis60Minutes() As Long
            Return MyBase.GetTemporalHour(GetAlos60(), GetTzais60())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (solar hour) according to the
        ''' opinion of the MGA. This calculation divides the day based on the opinion
        ''' of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        ''' calculation is 72 minutes before sunrise and dusk is 72 minutes after
        ''' sunset. This day is split into 12 equal parts with each part being a
        ''' <em>shaah zmanis</em>. Alternate mothods of calculating a
        ''' <em>shaah zmanis</em> are available in the subclass
        ''' <seecref="ComplexZmanimCalendar"/>
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis72Minutes() As Long
            Return MyBase.GetShaahZmanisMGA()
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ''' opinion of the MGA based on <em>alos</em> being
        ''' <seecref="GetAlos72Zmanis">72</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        ''' the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ''' for this calculation is 72 minutes <em>zmaniyos</em> before sunrise and
        ''' dusk is 72 minutes <em>zmaniyos</em> after sunset. This day is split into
        ''' 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ''' identical to 1/10th of the day from <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        ''' <seecref="AstronomicalCalendar.GetSunset">sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzais72Zmanis()"/>
        Public Overridable Function GetShaahZmanis72MinutesZmanis() As Long
            Return MyBase.GetTemporalHour(GetAlos72Zmanis(), GetTzais72Zmanis())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a dip of 90 minutes. This calculation divides the day based on the
        ''' opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ''' calculation is 90 minutes before sunrise and dusk is 90 minutes after
        ''' sunset. This day is split into 12 equal parts with each part being a
        ''' <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis90Minutes() As Long
            Return MyBase.GetTemporalHour(GetAlos90(), GetTzais90())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ''' opinion of the MGA based on <em>alos</em> being
        ''' <seecref="GetAlos90Zmanis">90</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        ''' the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ''' for this calculation is 90 minutes <em>zmaniyos</em> before sunrise and
        ''' dusk is 90 minutes <em>zmaniyos</em> after sunset. This day is split into
        ''' 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ''' identical to 1/8th of the day from <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        ''' <seecref="AstronomicalCalendar.GetSunset">sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos90Zmanis()"/>
        ''' <seealsocref="GetTzais90Zmanis()"/>
        Public Overridable Function GetShaahZmanis90MinutesZmanis() As Long
            Return MyBase.GetTemporalHour(GetAlos90Zmanis(), GetTzais90Zmanis())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ''' opinion of the MGA based on <em>alos</em> being
        ''' <seecref="GetAlos96Zmanis">96</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        ''' the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ''' for this calculation is 96 minutes <em>zmaniyos</em> before sunrise and
        ''' dusk is 96 minutes <em>zmaniyos</em> after sunset. This day is split into
        ''' 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ''' identical to 1/7.5th of the day from <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        ''' <seecref="AstronomicalCalendar.GetSunset">sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos96Zmanis()"/>
        ''' <seealsocref="GetTzais96Zmanis()"/>
        Public Overridable Function GetShaahZmanis96MinutesZmanis() As Long
            Return MyBase.GetTemporalHour(GetAlos96Zmanis(), GetTzais96Zmanis())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ''' opinion of the Chacham Yosef Harari-Raful of Yeshivat Ateret Torah
        ''' calculated with <em>alos</em> being 1/10th of sunrise to sunset day, or
        ''' <seecref="GetAlos72Zmanis">72</see> minutes <em>zmaniyos</em> of such a day
        ''' before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>, and tzais is usually calculated as
        ''' <seecref="GetTzaisAteretTorah">40 minutes</see> after <seecref="AstronomicalCalendar.GetSunset"> sunset</see>.
        ''' This day is split into 12 equal parts with each part being a
        ''' <em>shaah zmanis</em>. Note that with this system, chatzos (mid-day) will
        ''' not be the point that the sun is <seecref="AstronomicalCalendar.GetSunTransit">halfway across the sky</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzaisAteretTorah()"/>
        ''' <seealsocref="AteretTorahSunsetOffset"/>
        Public Overridable Function GetShaahZmanisAteretTorah() As Long
            Return MyBase.GetTemporalHour(GetAlos72Zmanis(), GetTzaisAteretTorah())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a dip of 96 minutes. This calculation divides the day based on the
        ''' opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ''' calculation is 96 minutes before sunrise and dusk is 96 minutes after
        ''' sunset. This day is split into 12 equal parts with each part being a
        ''' <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis96Minutes() As Long
            Return MyBase.GetTemporalHour(GetAlos96(), GetTzais96())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ''' a dip of 120 minutes. This calculation divides the day based on the
        ''' opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ''' calculation is 120 minutes before sunrise and dusk is 120 minutes after
        ''' sunset. This day is split into 12 equal parts with each part being a
        ''' <em>shaah zmanis</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanis120Minutes() As Long
            Return MyBase.GetTemporalHour(GetAlos120(), GetTzais120())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ''' opinion of the MGA based on <em>alos</em> being
        ''' <seecref="GetAlos120Zmanis">120</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        ''' the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ''' for this calculation is 120 minutes <em>zmaniyos</em> before sunrise and
        ''' dusk is 120 minutes <em>zmaniyos</em> after sunset. This day is split
        ''' into 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ''' identical to 1/6th of the day from <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        ''' <seecref="AstronomicalCalendar.GetSunset">sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set, a
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation
        ''' on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos120Zmanis()"/>
        ''' <seealsocref="GetTzais120Zmanis()"/>
        Public Overridable Function GetShaahZmanis120MinutesZmanis() As Long
            Return MyBase.GetTemporalHour(GetAlos120Zmanis(), GetTzais120Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seecref="GetAlos120Zmanis">dawn</see>. The
        ''' formula used is:<br/>
        ''' 10.75 * <seecref="GetShaahZmanis120MinutesZmanis()"/> after
        ''' <seecref="GetAlos120Zmanis">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha120MinutesZmanis() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos120Zmanis(), GetTzais120Zmanis())
        End Function


        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seecref="GetAlos120">dawn</see>. The formula
        ''' used is:<br/>
        ''' 10.75 <seecref="GetShaahZmanis120Minutes()"/> after <seecref="GetAlos120()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha120Minutes() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos120(), GetTzais120())
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 60 minutes before
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> on the time to walk the
        ''' distance of 4 <em>Mil</em> at 15 minutes a <em>Mil</em> (the opinion of
        ''' the Chavas Yair. See the Divray Malkiel). This is based on the opinion of
        ''' most <em>Rishonim</em> who stated that the time of the <em>Neshef</em>
        ''' (time between dawn and sunrise) does not vary by the time of year or
        ''' location but purely depends on the time it takes to walk the distance of
        ''' 4 <em>Mil</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetAlos60() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), -60 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 72 minutes
        ''' <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/10th
        ''' of the day before sea level sunrise. This is based on an 18 minute
        ''' <em>Mil</em> so the time for 4 <em>Mil</em> is 72 minutes which is 1/10th
        ''' of a day (12 * 60 = 720) based on the day starting at
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> and ending at
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. The actual alculation is
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunrise"/>- ( <seecref="ZmanimCalendar.GetShaahZmanisGra"/> * 1.2).
        ''' This calculation is used in the calendars published by
        ''' <em>Hisachdus Harabanim D'Artzos Habris Ve'Kanada</em>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetShaahZmanisGra"/>
        Public Overridable Function GetAlos72Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()

            If shaahZmanis = Long.MinValue Then
                Return Nothing
            End If

            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), CLng(shaahZmanis * -1.2))
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 96 minutes before
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> based on the time to walk
        ''' the distance of 4 <em>Mil</em> at 24 minutes a <em>Mil</em>. This is
        ''' based on the opinion of most <em>Rishonim</em> who stated that the time
        ''' of the <em>Neshef</em> (time between dawn and sunrise) does not vary by
        ''' the time of year or location but purely depends on the time it takes to
        ''' walk the distance of 4 <em>Mil</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetAlos96() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), -96 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 90 minutes
        ''' <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        ''' of the day before sea level sunrise. This is based on a 22.5 minute
        ''' <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is 1/8th
        ''' of a day (12 * 60 = 720) /8 =90 based on the day starting at
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> and ending at <seealsocref="AstronomicalCalendar.GetSunset">sunset</seealso>.
        ''' The actual calculation is <seecref="AstronomicalCalendar.GetSunrise"/> - (
        ''' <seecref="ZmanimCalendar.GetShaahZmanisGra"/> * 1.5).
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetShaahZmanisGra"/>
        Public Overridable Function GetAlos90Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()
            If shaahZmanis = Long.MinValue Then Return Nothing
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), CLng(shaahZmanis * -1.5))
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 90 minutes
        ''' <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        ''' of the day before sea level sunrise. This is based on a 24 minute
        ''' <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is
        ''' 1/7.5th of a day (12 * 60 = 720) / 7.5 =96 based on the day starting at
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> and ending at <seecref="AstronomicalCalendar.GetSunset">sunset</see>.
        ''' The actual calculation is <seealsocref="AstronomicalCalendar.GetSunrise"/> - (
        ''' <seecref="ZmanimCalendar.GetShaahZmanisGra"/> * 1.6).
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetShaahZmanisGra"/>
        Public Overridable Function GetAlos96Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()

            If shaahZmanis = Long.MinValue Then
                Return Nothing
            End If

            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), CLng(shaahZmanis * -1.6))
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 90 minutes before
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> on the time to walk the
        ''' distance of 4 <em>Mil</em> at 22.5 minutes a <em>Mil</em>. This is based
        ''' on the opinion of most <em>Rishonim</em> who stated that the time of the
        ''' <em>Neshef</em> (time between dawn and sunrise) does not vary by the time
        ''' of year or location but purely depends on the time it takes to walk the
        ''' distance of 4 <em>Mil</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetAlos90() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), -90 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 120 minutes before
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> (no adjustment for
        ''' elevation is made) based on the time to walk the distance of 5
        ''' <em>Mil</em>( <em>Ula</em>) at 24 minutes a <em>Mil</em>. This is based
        ''' on the opinion of most <em>Rishonim</em> who stated that the time of the
        ''' <em>Neshef</em> (time between dawn and sunrise) does not vary by the time
        ''' of year or location but purely depends on the time it takes to walk the
        ''' distance of 5 <em>Mil</em>(<em>Ula</em>).
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetAlos120() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), -120 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated using 120 minutes
        ''' <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/6th
        ''' of the day before sea level sunrise. This is based on a 24 minute
        ''' <em>Mil</em> so the time for 5 <em>Mil</em> is 120 minutes which is 1/6th
        ''' of a day (12 * 60 = 720) / 6 =120 based on the day starting at
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> and ending at <seecref="AstronomicalCalendar.GetSunset">sunset</see>.
        ''' The actual calculation is <seealsocref="AstronomicalCalendar.GetSunrise"/> - (
        ''' <seecref="ZmanimCalendar.GetShaahZmanisGra"/> * 2).
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetShaahZmanisGra"/>
        Public Overridable Function GetAlos120Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()

            If shaahZmanis = Long.MinValue Then
                Return Nothing
            End If

            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), shaahZmanis * -2)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated when the sun is
        ''' <seecref="ZENITH_26_DEGREES">26°</see> below the eastern geometric horizon
        ''' before sunrise. This calculation is based on the same calculation of
        ''' <seecref="GetAlos120">120 minutes</see> but uses a degree based calculation
        ''' instead of 120 exact minutes. This calculation is based on the position
        ''' of the sun 120 minutes before sunrise in Jerusalem in the equinox which
        ''' calculates to 26° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing <em>alos</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_26_DEGREES"/>
        ''' <seealsocref="GetAlos120()"/>
        ''' <seealsocref="GetTzais120()"/>
        Public Overridable Function GetAlos26Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_26_DEGREES)
        End Function

        ''' <summary>
        ''' to return <em>alos</em> (dawn) calculated when the sun is
        ''' <seecref="AstronomicalCalendar.ASTRONOMICAL_ZENITH">18°</see> below the eastern geometric horizon
        ''' before sunrise.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing <em>alos</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="AstronomicalCalendar.ASTRONOMICAL_ZENITH"/>
        Public Overridable Function GetAlos18Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ASTRONOMICAL_ZENITH)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated when the sun is
        ''' <seealsocref="ZENITH_19_POINT_8">19.8°</seealso> below the eastern geometric horizon
        ''' before sunrise. This calculation is based on the same calculation of
        ''' <seealsocref="GetAlos90">90 minutes</seealso> but uses a degree based calculation
        ''' instead of 90 exact minutes. This calculation is based on the position of
        ''' the sun 90 minutes before sunrise in Jerusalem in the equinox which
        ''' calculates to 19.8° below <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing <em>alos</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_19_POINT_8"/>
        ''' <seealsocref="GetAlos90()"/>
        Public Overridable Function GetAlos19Point8Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_19_POINT_8)
        End Function

        ''' <summary>
        ''' Method to return <em>alos</em> (dawn) calculated when the sun is
        ''' <seealsocref="ZmanimCalendar.ZENITH_16_POINT_1">16.1°</seealso> below the eastern geometric horizon
        ''' before sunrise. This calculation is based on the same calculation of
        ''' <seealsocref="ZmanimCalendar.GetAlos72">72 minutes</seealso> but uses a degree based calculation
        ''' instead of 72 exact minutes. This calculation is based on the position of
        ''' the sun 72 minutes before sunrise in Jerusalem in the equinox which
        ''' calculates to 16.1° below <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing <em>alos</em>.
        ''' If the
        ''' calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.ZENITH_16_POINT_1"/>
        ''' <seealsocref="ZmanimCalendar.GetAlos72"/>
        Public Overridable Function GetAlos16Point1Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_16_POINT_1)
        End Function

        ''' <summary>
        ''' This method returns <em>misheyakir</em> based on the position of the sun
        ''' when it is <seealsocref="ZENITH_11_DEGREES">11.5°</seealso> below
        ''' <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> (90°). This calculation is
        ''' used for calculating <em>misheyakir</em> according to some opinions. This
        ''' calculation is based on the position of the sun 52 minutes before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>in Jerusalem in the equinox which calculates
        ''' to 11.5° below <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of misheyakir. If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' ///
        ''' <seealsocref="ZENITH_11_POINT_5"/>
        Public Overridable Function GetMisheyakir11Point5Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_11_POINT_5)
        End Function

        ''' <summary>
        ''' This method returns <em>misheyakir</em> based on the position of the sun
        ''' when it is <seealsocref="ZENITH_11_DEGREES">11°</seealso> below
        ''' <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> (90°). This calculation is
        ''' used for calculating <em>misheyakir</em> according to some opinions. This
        ''' calculation is based on the position of the sun 48 minutes before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>in Jerusalem in the equinox which calculates
        ''' to 11° below <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        ''' </summary>
        ''' <returns>
        ''' If the calculation can't be computed such as northern and
        ''' southern locations even south of the Arctic Circle and north of
        ''' the Antarctic Circle where the sun may not reach low enough below
        ''' the horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_11_DEGREES"/>
        Public Overridable Function GetMisheyakir11Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_11_DEGREES)
        End Function

        ''' <summary>
        ''' This method returns <em>misheyakir</em> based on the position of the sun
        ''' when it is <seealsocref="ZENITH_10_POINT_2">10.2°</seealso> below
        ''' <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> (90°). This calculation is
        ''' used for calculating <em>misheyakir</em> according to some opinions. This
        ''' calculation is based on the position of the sun 45 minutes before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso> in Jerusalem in the equinox which calculates
        ''' to 10.2° below <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest misheyakir.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_10_POINT_2"/>
        Public Overridable Function GetMisheyakir10Point2Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_10_POINT_2)
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seealsocref="GetAlos19Point8Degrees()">19.8°</seealso> before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 3
        ''' <em><seealsocref="GetShaahZmanis19Point8Degrees">shaos zmaniyos</seealso></em> (solar
        ''' hours) after <seealsocref="GetAlos19Point8Degrees">dawn</seealso> based on the opinion
        ''' of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ''' with both being 19.8° below sunrise or sunset. This returns the time
        ''' of 3 <seealsocref="GetShaahZmanis19Point8Degrees()"/> after
        ''' <seealsocref="GetAlos19Point8Degrees">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis19Point8Degrees()"/>
        ''' <seealsocref="GetAlos19Point8Degrees()"/>
        Public Overridable Function GetSofZmanShmaMGA19Point8Degrees() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos19Point8Degrees(), GetTzais19Point8Degrees())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seealsocref="GetAlos16Point1Degrees()">16.1°</seealso> before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 3
        ''' <em><seealsocref="GetShaahZmanis16Point1Degrees">shaos zmaniyos</seealso></em> (solar
        ''' hours) after <seealsocref="GetAlos16Point1Degrees">dawn</seealso> based on the opinion
        ''' of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ''' with both being 16.1° below sunrise or sunset. This returns the time
        ''' of 3 <seealsocref="GetShaahZmanis16Point1Degrees()"/> after
        ''' <seealsocref="GetAlos16Point1Degrees">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees()"/>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        Public Overridable Function GetSofZmanShmaMGA16Point1Degrees() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos16Point1Degrees(), GetTzais16Point1Degrees())
        End Function


        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to recite Shema in the morning) according to the
        ''' opinion of the <em>MGA</em> based on <em>alos</em> being <seealsocref="GetAlos18Degrees()"/> before
        ''' <seealsocref="GetSunrise()"/>. This time is 3 <em><seealsocref="GetShaahZmanis18Degrees()"/></em> (solar
        ''' hours) after <seealsocref="GetAlos18Degrees()"/> based on the opinion of the <em>MGA</em> that the day is calculated
        ''' from dawn to nightfall with both being 18&deg; below sunrise or sunset. This returns the time of 3 *
        ''' <seealsocref="GetShaahZmanis18Degrees()"/> after <seealsocref="GetAlos18Degrees()"/>.
        ''' </summary>
        ''' <returns> the <code>Date</code> of the latest <em>zman krias shema</em>. If the calculation can't be computed such
        '''         as northern and southern locations even south of the Arctic Circle and north of the Antarctic Circle
        '''         where the sun may not reach low enough below the horizon for this calculation, a null will be returned.
        '''         See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        ''' <seealsocref="GetShaahZmanis18Degrees"/>
        ''' <seealsocref="GetAlos18Degrees"/>
        Public Overridable Function GetSofZmanShmaMGA18Degrees() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos18Degrees(), GetTzais18Degrees())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="ZmanimCalendar.GetAlos72">72</see> minutes before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        ''' <em><seecref="GetShaahZmanis72Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="ZmanimCalendar.GetAlos72">dawn</see> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seecref="ZmanimCalendar.GetAlos72">dawn</see> of 72 minutes
        ''' before sunrise to <seecref="ZmanimCalendar.GetTzais72">nightfall</see> of 72 minutes after
        ''' sunset. This returns the time of 3 * <seealsocref="GetShaahZmanis72Minutes()"/>
        ''' after <seecref="ZmanimCalendar.GetAlos72">dawn</see>. This class returns an identical time to
        ''' <seecref="ZmanimCalendar.GetSofZmanShmaMGA"/> and is repeated here for clarity.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis72Minutes()"/>
        ''' <seealsocref="ZmanimCalendar.GetAlos72"/>
        ''' <seealsocref="ZmanimCalendar.GetSofZmanShmaMGA"/>
        Public Overridable Function GetSofZmanShmaMGA72Minutes() As DateTime?
            Return MyBase.GetSofZmanShmaMGA()
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos72Zmanis">72</see> minutes
        ''' <em>zmaniyos</em>, or 1/10th of the day before <seecref="AstronomicalCalendar.GetSunrise"> sunrise</see>
        ''' . This time is 3
        ''' <em><seecref="GetShaahZmanis90MinutesZmanis">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos72Zmanis">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a
        ''' <seecref="GetAlos72Zmanis">dawn</see> of 72 minutes <em>zmaniyos</em>, or
        ''' 1/10th of the day before <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see>
        ''' to <seecref="GetTzais72Zmanis">nightfall</see> of 72 minutes <em>zmaniyos</em>
        ''' after <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. This returns the
        ''' time of 3 * <seecref="GetShaahZmanis72MinutesZmanis()"/> after
        ''' <seecref="GetAlos72Zmanis">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis72MinutesZmanis()"/>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        Public Overridable Function GetSofZmanShmaMGA72MinutesZmanis() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos72Zmanis(), GetTzais72Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos90">90</see> minutes before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        ''' <em><seecref="GetShaahZmanis90Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="GetAlos90">dawn</see> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seecref="GetAlos90">dawn</see> of 90 minutes
        ''' before sunrise to <seecref="GetTzais90">nightfall</see> of 90 minutes after
        ''' sunset. This returns the time of 3 * <seecref="GetShaahZmanis90Minutes()"/>
        ''' after <seecref="GetAlos90">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis90Minutes()"/>
        ''' <seealsocref="GetAlos90()"/>
        Public Overridable Function GetSofZmanShmaMGA90Minutes() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos90(), GetTzais90())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos90Zmanis">90</see> minutes
        ''' <em>zmaniyos</em> before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        ''' <em><seecref="GetShaahZmanis90MinutesZmanis">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos90Zmanis">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a
        ''' <seecref="GetAlos90Zmanis">dawn</see> of 90 minutes <em>zmaniyos</em> before
        ''' sunrise to <seecref="GetTzais90Zmanis">nightfall</see> of 90 minutes
        ''' <em>zmaniyos</em> after sunset. This returns the time of 3 *
        ''' <seecref="GetShaahZmanis90MinutesZmanis()"/> after <seecref="GetAlos90Zmanis()"> dawn</see>
        ''' .
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis90MinutesZmanis()"/>
        ''' <seealsocref="GetAlos90Zmanis()"/>
        Public Overridable Function GetSofZmanShmaMGA90MinutesZmanis() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos90Zmanis(), GetTzais90Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos96">96</see> minutes before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        ''' <em><seecref="GetShaahZmanis96Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="GetAlos96">dawn</see> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seecref="GetAlos96">dawn</see> of 96 minutes
        ''' before sunrise to <seecref="GetTzais96">nightfall</see> of 96 minutes after
        ''' sunset. This returns the time of 3 * <seecref="GetShaahZmanis96Minutes()"/>
        ''' after <seecref="GetAlos96">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis96Minutes()"/>
        ''' <seealsocref="GetAlos96()"/>
        Public Overridable Function GetSofZmanShmaMGA96Minutes() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos96(), GetTzais96())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos90Zmanis">96</see> minutes
        ''' <em>zmaniyos</em> before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        ''' <em><seecref="GetShaahZmanis96MinutesZmanis">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos96Zmanis">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a
        ''' <seecref="GetAlos96Zmanis">dawn</see> of 96 minutes <em>zmaniyos</em> before
        ''' sunrise to <seecref="GetTzais90Zmanis">nightfall</see> of 96 minutes
        ''' <em>zmaniyos</em> after sunset. This returns the time of 3 *
        ''' <seecref="GetShaahZmanis96MinutesZmanis()"/> after <seecref="GetAlos96Zmanis()"> dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis96MinutesZmanis()"/>
        ''' <seealsocref="GetAlos96Zmanis()"/>
        Public Overridable Function GetSofZmanShmaMGA96MinutesZmanis() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos96Zmanis(), GetTzais96Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) calculated as 3 hours (regular and not zmaniyos)
        ''' before <seecref="ZmanimCalendar.GetChatzos"/>. This is the opinion of the
        ''' <em>Shach</em> in the
        ''' <em>Nekudas Hakesef (Yora Deah 184), Shevus Yaakov, Chasan Sofer</em> and
        ''' others.This returns the time of 3 hours before
        ''' <seecref="ZmanimCalendar.GetChatzos"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetChatzos"/>
        ''' <seealsocref="GetSofZmanTfila2HoursBeforeChatzos()"/>
        Public Overridable Function GetSofZmanShma3HoursBeforeChatzos() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetChatzos(), -180 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos120">120</see> minutes or 1/6th of the day
        ''' before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        ''' <em><seecref="GetShaahZmanis120Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="GetAlos120">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a <seecref="GetAlos120()"> dawn</see>
        ''' of 120 minutes before sunrise to <seecref="GetTzais120">nightfall</see>
        ''' of 120 minutes after sunset. This returns the time of 3 *
        ''' <seecref="GetShaahZmanis120Minutes()"/> after <seecref="GetAlos120">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis120Minutes()"/>
        ''' <seealsocref="GetAlos120()"/>
        Public Overridable Function GetSofZmanShmaMGA120Minutes() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos120(), GetTzais120())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) based on the opinion that the day starts at
        ''' <em><seecref="GetAlos16Point1Degrees">alos 16.1°</see></em> and ends at
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. 3 shaos zmaniyos are
        ''' calculated based on this day and added to
        ''' <seecref="GetAlos16Point1Degrees">alos</see>to reach this time. This time is 3
        ''' <em>shaos zmaniyos</em> (solar hours) after
        ''' <seecref="GetAlos16Point1Degrees">dawn</see> based on the opinion that the day
        ''' is calculated from a <seecref="GetAlos16Point1Degrees">alos 16.1°</see> to
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.<br/>
        ''' 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ''' midday.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema based on this day.
        ''' If the calculation can't be computed such as northern and
        ''' southern locations even south of the Arctic Circle and north of
        ''' the Antarctic Circle where the sun may not reach low enough below
        ''' the horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        ''' <seealsocref="AstronomicalCalendar.GetSeaLevelSunset"/>
        Public Overridable Function GetSofZmanShmaAlos16Point1ToSunset() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos16Point1Degrees(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) based on the opinion that the day starts at
        ''' <em><seecref="GetAlos16Point1Degrees">alos 16.1°</see></em> and ends at
        ''' <seecref="GetTzaisGeonim7Point083Degrees">tzais 7.083°</see>. 3
        ''' <em>shaos zmaniyos</em> are calculated based on this day and added to
        ''' <seecref="GetAlos16Point1Degrees">alos</see> to reach this time. This time is 3
        ''' <em>shaos zmaniyos</em> (temporal hours) after
        ''' <seecref="GetAlos16Point1Degrees">alos 16.1°</see> based on the opinion
        ''' that the day is calculated from a <seecref="GetAlos16Point1Degrees()">alos 16.1°</see>
        ''' to
        ''' <em><seecref="GetTzaisGeonim7Point083Degrees">tzais 7.083°</see></em>.<br/>
        ''' 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ''' midday.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema based on this
        ''' calculation. If the calculation can't be computed such as
        ''' northern and southern locations even south of the Arctic Circle
        ''' and north of the Antarctic Circle where the sun may not reach low
        ''' enough below the horizon for this calculation, a null will be
        ''' returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        ''' <seealsocref="GetTzaisGeonim7Point083Degrees()"/>
        Public Overridable Function GetSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos16Point1Degrees(), GetTzaisGeonim7Point083Degrees())
        End Function

        ''' <summary>
        ''' From the GR"A in Kol Eliyahu on Berachos #173 that states that zman krias
        ''' shema is calculated as half the time from <seecref="AstronomicalCalendar.GetSeaLevelSunrise"> sea level sunrise</see>
        ''' to fixed local chatzos. The GRA himself seems to contradict this when he stated that <em>zman krias shema</em>
        ''' is 1/4 of the day from sunrise to sunset. See <em>Sarah Lamoed</em> #25 in Yisroel Vehazmanim Vol III page 1016
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema based on this calculation.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetFixedLocalChatzos()"/>
        <Obsolete("Pending confirmation from Rabbi Harfenes, this method is deprecated and should not be used.")>
        Public Overridable Function GetSofZmanShmaKolEliyahu() As DateTime?
            Dim chatzos As DateTime? = GetFixedLocalChatzos()

            If chatzos = Date.MinValue OrElse MyBase.GetSunrise() = Date.MinValue Then
                Return Nothing
            End If

            Dim diff As Long = (chatzos.Value.ToUnixEpochMilliseconds() - MyBase.GetSeaLevelSunrise().Value.ToUnixEpochMilliseconds()) / 2
            Return MyBase.GetTimeOffset(chatzos, -diff)
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos19Point8Degrees()">19.8°</see> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis19Point8Degrees">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos19Point8Degrees">dawn</see> based on the opinion
        ''' of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ''' with both being 19.8° below sunrise or sunset. This returns the time
        ''' of 4 <seecref="GetShaahZmanis19Point8Degrees()"/> after
        ''' <seecref="GetAlos19Point8Degrees">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis19Point8Degrees()"/>
        ''' <seealsocref="GetAlos19Point8Degrees()"/>
        Public Overridable Function GetSofZmanTfilaMGA19Point8Degrees() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos19Point8Degrees(), GetTzais19Point8Degrees())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos19Point8Degrees()">16.1°</see> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis16Point1Degrees">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos16Point1Degrees">dawn</see> based on the opinion
        ''' of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ''' with both being 16.1° below sunrise or sunset. This returns the time
        ''' of 4 <seecref="GetShaahZmanis16Point1Degrees()"/> after
        ''' <seecref="GetAlos16Point1Degrees">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees()"/>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        Public Overridable Function GetSofZmanTfilaMGA16Point1Degrees() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos16Point1Degrees(), GetTzais16Point1Degrees())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to recite the morning prayers) according to the opinion
        ''' of the <em>MGA</em> based on <em>alos</em> being <seealsocref="GetAlos18Degrees()"/> before {@link #getSunrise()
        ''' sunrise}. This time is 4 <em><seealsocref="GetShaahZmanis18Degrees()"/></em> (solar hours) after
        ''' <seealsocref="GetAlos18Degrees()"/> based on the opinion of the <em>MGA</em> that the day is calculated from dawn to
        ''' nightfall with both being 18&deg; below sunrise or sunset. This returns the time of 4 *
        ''' <seealsocref="GetShaahZmanis18Degrees()"/> after <seealsocref="GetAlos18Degrees()"/>.
        ''' </summary>
        ''' <returns> the <code>Date</code> of the latest <em>zman krias shema</em>. If the calculation can't be computed such
        '''         as northern and southern locations even south of the Arctic Circle and north of the Antarctic Circle
        '''         where the sun may not reach low enough below the horizon for this calculation, a null will be returned.
        '''         See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis18Degrees"/>
        ''' <seealsocref="GetAlos18Degrees"/>
        Public Overridable Function GetSofZmanTfilaMGA18Degrees() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos18Degrees(), GetTzais18Degrees())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="ZmanimCalendar.GetAlos72">72</see> minutes before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis72Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="ZmanimCalendar.GetAlos72">dawn</see> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seecref="ZmanimCalendar.GetAlos72">dawn</see> of 72 minutes
        ''' before sunrise to <seecref="ZmanimCalendar.GetTzais72">nightfall</see> of 72 minutes after
        ''' sunset. This returns the time of 4 * <seecref="GetShaahZmanis72Minutes()"/>
        ''' after <seecref="ZmanimCalendar.GetAlos72">dawn</see>. This class returns an identical time to
        ''' <seecref="ZmanimCalendar.GetSofZmanTfilaMGA"/> and is repeated here for clarity.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman tfila.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis72Minutes()"/>
        ''' <seealsocref="ZmanimCalendar.GetAlos72"/>
        ''' <seealsocref="ZmanimCalendar.GetSofZmanShmaMGA"/>
        Public Overridable Function GetSofZmanTfilaMGA72Minutes() As DateTime?
            Return MyBase.GetSofZmanTfilaMGA()
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to the morning
        ''' prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        ''' <seecref="GetAlos72Zmanis">72</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis72MinutesZmanis">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos72Zmanis">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a
        ''' <seecref="GetAlos72Zmanis">dawn</see> of 72 minutes <em>zmaniyos</em> before
        ''' sunrise to <seecref="GetTzais72Zmanis">nightfall</see> of 72 minutes
        ''' <em>zmaniyos</em> after sunset. This returns the time of 4 *
        ''' <seecref="GetShaahZmanis72MinutesZmanis()"/> after <seecref="GetAlos72Zmanis()"> dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis72MinutesZmanis()"/>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        Public Overridable Function GetSofZmanTfilaMGA72MinutesZmanis() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos72Zmanis(), GetTzais72Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seealsocref="GetAlos90">90</seealso> minutes before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 4
        ''' <em><seealsocref="GetShaahZmanis90Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ''' after <seealsocref="GetAlos90">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seealsocref="GetAlos90">dawn</seealso> of 90 minutes
        ''' before sunrise to <seealsocref="GetTzais90">nightfall</seealso> of 90 minutes after
        ''' sunset. This returns the time of 4 * <seealsocref="GetShaahZmanis90Minutes()"/>
        ''' after <seealsocref="GetAlos90">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman tfila.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis90Minutes()"/>
        ''' <seealsocref="GetAlos90()"/>
        Public Overridable Function GetSofZmanTfilaMGA90Minutes() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos90(), GetTzais90())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to the morning
        ''' prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        ''' <seecref="GetAlos90Zmanis">90</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis90MinutesZmanis">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos90Zmanis">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a
        ''' <seecref="GetAlos90Zmanis">dawn</see> of 90 minutes <em>zmaniyos</em> before
        ''' sunrise to <seealsocref="GetTzais90Zmanis">nightfall</seealso> of 90 minutes
        ''' <em>zmaniyos</em> after sunset. This returns the time of 4 *
        ''' <seecref="GetShaahZmanis90MinutesZmanis()"/> after <seecref="GetAlos90Zmanis()"> dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis90MinutesZmanis()"/>
        ''' <seealsocref="GetAlos90Zmanis()"/>
        Public Overridable Function GetSofZmanTfilaMGA90MinutesZmanis() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos90Zmanis(), GetTzais90Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos96">96</see> minutes before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis96Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="GetAlos96">dawn</see> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seecref="GetAlos96">dawn</see> of 96 minutes
        ''' before sunrise to <seecref="GetTzais96">nightfall</see> of 96 minutes after
        ''' sunset. This returns the time of 4 * <seecref="GetShaahZmanis96Minutes()"/>
        ''' after <seecref="GetAlos96">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman tfila.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis96Minutes()"/>
        ''' <seealsocref="GetAlos96()"/>
        Public Overridable Function GetSofZmanTfilaMGA96Minutes() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos96(), GetTzais96())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to the morning
        ''' prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        ''' <seecref="GetAlos96Zmanis">96</see> minutes <em>zmaniyos</em> before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis96MinutesZmanis">shaos zmaniyos</see></em> (solar
        ''' hours) after <seecref="GetAlos96Zmanis">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a
        ''' <seecref="GetAlos96Zmanis">dawn</see> of 96 minutes <em>zmaniyos</em> before
        ''' sunrise to <seecref="GetTzais96Zmanis">nightfall</see> of 96 minutes
        ''' <em>zmaniyos</em> after sunset. This returns the time of 4 *
        ''' <seecref="GetShaahZmanis96MinutesZmanis()"/> after <seecref="GetAlos96Zmanis()"> dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis90MinutesZmanis()"/>
        ''' <seealsocref="GetAlos90Zmanis()"/>
        Public Overridable Function GetSofZmanTfilaMGA96MinutesZmanis() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos96Zmanis(), GetTzais96Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seecref="GetAlos120">120</see> minutes before
        ''' <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        ''' <em><seecref="GetShaahZmanis120Minutes">shaos zmaniyos</see></em> (solar hours)
        ''' after <seecref="GetAlos120">dawn</see> based on the opinion of the
        ''' <em>MG"A</em> that the day is calculated from a <seecref="GetAlos120()"> dawn</see>
        ''' of 120 minutes before sunrise to <seecref="GetTzais120">nightfall</see>
        ''' of 120 minutes after sunset. This returns the time of 4 *
        ''' <seecref="GetShaahZmanis120Minutes()"/> after <seecref="GetAlos120">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis120Minutes()"/>
        ''' <seealsocref="GetAlos120()"/>
        Public Overridable Function GetSofZmanTfilaMGA120Minutes() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos120(), GetTzais120())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) calculated as 2 hours befor
        ''' <seecref="ZmanimCalendar.GetChatzos"/>. This is based on the opinions that
        ''' calculate <em>sof zman krias shema</em> as
        ''' <seecref="GetSofZmanShma3HoursBeforeChatzos()"/>. This returns the time of 2
        ''' hours before <seealsocref="ZmanimCalendar.GetChatzos"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetChatzos"/>
        ''' <seealsocref="GetSofZmanShma3HoursBeforeChatzos()"/>
        Public Overridable Function GetSofZmanTfila2HoursBeforeChatzos() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetChatzos(), -120 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns mincha gedola calculated as 30 minutes after
        ''' <em><seecref="ZmanimCalendar.GetChatzos">chatzos</see></em> and not 1/2 of a
        ''' <em><seecref="ZmanimCalendar.GetShaahZmanisGra">shaah zmanis</see></em> after
        ''' <em><seecref="ZmanimCalendar.GetChatzos">chatzos</see></em> as calculated by
        ''' <seecref="ZmanimCalendar.GetMinchaGedola"/>. Some use this time to delay the start of mincha
        ''' in the winter when 1/2 of a
        ''' <em><seecref="ZmanimCalendar.GetShaahZmanisGra">shaah zmanis</see></em> is less than 30
        ''' minutes. See <seealsocref="GetMinchaGedolaGreaterThan30()"/>for a conveniance
        ''' method that returns the later of the 2 calculations. One should not use
        ''' this time to start <em>mincha</em> before the standard
        ''' <em><seecref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. See <em>Shulchan Aruch
        ''' Orach Chayim Siman Raish Lamed Gimel seif alef</em> and the
        ''' <em>Shaar Hatziyon seif katan ches</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of 30 mintes after <em>chatzos</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="GetMinchaGedolaGreaterThan30()"/>
        Public Overridable Function GetMinchaGedola30Minutes() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetChatzos(), MINUTE_MILLIS * 30)
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha gedola</em> according to the
        ''' Magen Avraham with the day starting 72 minutes before sunrise and ending
        ''' 72 minutes after sunset. This is the earliest time to pray
        ''' <em>mincha</em>. For more information on this see the documentation on
        ''' <em><seecref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        ''' 6.5 <seecref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">solar hours</see> after alos. The calculation
        ''' used is 6.5 * <seecref="GetShaahZmanis72Minutes()"/> after
        ''' <seecref="ZmanimCalendar.GetAlos72">alos</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha gedola.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetAlos72"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaKetana"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        Public Overridable Function GetMinchaGedola72Minutes() As DateTime?
            Return MyBase.GetMinchaGedola(MyBase.GetAlos72(), MyBase.GetTzais72())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha gedola</em> according to the
        ''' Magen Avraham with the day starting and ending 16.1° below the
        ''' horizon. This is the earliest time to pray <em>mincha</em>. For more
        ''' information on this see the documentation on
        ''' <em><seecref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        ''' 6.5 <seecref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">solar hours</see> after alos. The calculation
        ''' used is 6.5 * <seecref="GetShaahZmanis16Point1Degrees()"/> after
        ''' <seecref="GetAlos16Point1Degrees">alos</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha gedola.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees()"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaKetana"/>
        Public Overridable Function GetMinchaGedola16Point1Degrees() As DateTime?
            Return MyBase.GetMinchaGedola(GetAlos16Point1Degrees(), GetTzais16Point1Degrees())
        End Function

        ''' <summary>
        ''' This is a conveniance methd that returns the later of
        ''' <seecref="ZmanimCalendar.GetMinchaGedola"/> and <seecref="GetMinchaGedola30Minutes()"/>. In
        ''' the winter when a <em><seecref="ZmanimCalendar.GetShaahZmanisGra">shaah zmanis</see></em> is
        ''' less than 30 minutes <seecref="GetMinchaGedola30Minutes()"/> will be
        ''' returned, otherwise <seecref="ZmanimCalendar.GetMinchaGedola"/> will be returned.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the later of <seecref="ZmanimCalendar.GetMinchaGedola"/>
        ''' and <seecref="GetMinchaGedola30Minutes()"/>
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetMinchaGedolaGreaterThan30() As DateTime?
            Dim minchaGedola = MyBase.GetMinchaGedola()
            Dim minchaGedola30Minutes = GetMinchaGedola30Minutes()
            If minchaGedola30Minutes Is Nothing OrElse minchaGedola Is Nothing Then Return Nothing
            Return If(minchaGedola30Minutes.Value.CompareTo(minchaGedola.Value) > 0, minchaGedola30Minutes, minchaGedola)
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha ketana</em> according to the
        ''' Magen Avraham with the day starting and ending 16.1° below the
        ''' horizon. This is the perfered earliest time to pray <em>mincha</em> in
        ''' the opinion of the Ramba"m and others. For more information on this see
        ''' the documentation on <em><seealsocref="ZmanimCalendar.GetMinchaGedola">mincha gedola</seealso></em>.
        ''' This is calculated as 9.5 <seealsocref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">solar hours</seealso> after
        ''' alos. The calculation used is 9.5 *
        ''' <seealsocref="GetShaahZmanis16Point1Degrees()"/> after
        ''' <seealsocref="GetAlos16Point1Degrees">alos</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha ketana.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees()"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaKetana"/>
        Public Overridable Function GetMinchaKetana16Point1Degrees() As DateTime?
            Return MyBase.GetMinchaKetana(GetAlos16Point1Degrees(), GetTzais16Point1Degrees())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha ketana</em> according to the
        ''' Magen Avraham with the day starting 72 minutes before sunrise and ending
        ''' 72 minutes after sunset. This is the perfered earliest time to pray
        ''' <em>mincha</em> in the opinion of the Ramba"m and others. For more
        ''' information on this see the documentation on
        ''' <em><seecref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        ''' 9.5 <seecref="GetShaahZmanis72Minutes()"/> after alos. The calculation used
        ''' is 9.5 * <seecref="GetShaahZmanis72Minutes()"/> after <seecref="ZmanimCalendar.GetAlos72"> alos</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha ketana.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees()"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaKetana"/>
        Public Overridable Function GetMinchaKetana72Minutes() As DateTime?
            Return MyBase.GetMinchaKetana(MyBase.GetAlos72(), MyBase.GetTzais72())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seecref="GetAlos60">dawn</see>. The formula
        ''' used is:<br/>
        ''' 10.75 <seecref="GetShaahZmanis60Minutes()"/> after <seealsosee="getAlos60()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha60Minutes() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos60(), GetTzais60())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seecref="ZmanimCalendar.GetAlos72">dawn</see>. The formula
        ''' used is:<br/>
        ''' 10.75 <seecref="GetShaahZmanis72Minutes()"/> after <seecref="ZmanimCalendar.GetAlos72"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha72Minutes() As DateTime?
            Return MyBase.GetPlagHamincha(MyBase.GetAlos72(), MyBase.GetTzais72())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seealsocref="GetAlos90">dawn</seealso>. The formula
        ''' used is:<br/>
        ''' 10.75 <seealsocref="GetShaahZmanis90Minutes()"/> after <seealsocref="GetAlos90()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha90Minutes() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos90(), GetTzais90())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seealsocref="GetAlos96">dawn</seealso>. The formula
        ''' used is:<br/>
        ''' 10.75 <seealsocref="GetShaahZmanis96Minutes()"/> after <seealsocref="GetAlos96()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha96Minutes() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos96(), GetTzais96())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seealsocref="GetAlos96Zmanis">dawn</seealso>. The
        ''' formula used is:<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis96MinutesZmanis()"/> after
        ''' <seealsocref="GetAlos96Zmanis">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha96MinutesZmanis() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos96Zmanis(), GetTzais96Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seealsocref="GetAlos90Zmanis">dawn</seealso>. The
        ''' formula used is:<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis90MinutesZmanis()"/> after
        ''' <seealsocref="GetAlos90Zmanis">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha90MinutesZmanis() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos90Zmanis(), GetTzais90Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em>. This is
        ''' calculated as 10.75 hours after <seealsocref="GetAlos72Zmanis">dawn</seealso>. The
        ''' formula used is:<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis72MinutesZmanis()"/> after
        ''' <seealsocref="GetAlos72Zmanis">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha72MinutesZmanis() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos72Zmanis(), GetTzais72Zmanis())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' opinion that the day starts at
        ''' <em><seealsocref="GetAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ''' <em><seealsocref="GetTzais16Point1Degrees">tzais 16.1°</seealso></em>. This is
        ''' calculated as 10.75 hours <em>zmaniyos</em> after
        ''' <seealsocref="GetAlos16Point1Degrees">dawn</seealso>. The formula is<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis16Point1Degrees()"/> after
        ''' <seealsocref="GetAlos16Point1Degrees()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha16Point1Degrees() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos16Point1Degrees(), GetTzais16Point1Degrees())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' opinion that the day starts at
        ''' <em><seealsocref="GetAlos19Point8Degrees">alos 19.8°</seealso></em> and ends at
        ''' <em><seealsocref="GetTzais19Point8Degrees">tzais 19.8°</seealso></em>. This is
        ''' calculated as 10.75 hours <em>zmaniyos</em> after
        ''' <seealsocref="GetAlos19Point8Degrees">dawn</seealso>. The formula is<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis19Point8Degrees()"/> after
        ''' <seealsocref="GetAlos19Point8Degrees()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha19Point8Degrees() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos19Point8Degrees(), GetTzais19Point8Degrees())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' opinion that the day starts at
        ''' <em><seealsocref="GetAlos26Degrees">alos 26°</seealso></em> and ends at
        ''' <em><seealsocref="GetTzais26Degrees">tzais 26°</seealso></em>. This is calculated
        ''' as 10.75 hours <em>zmaniyos</em> after <seealsocref="GetAlos26Degrees">dawn</seealso>.
        ''' The formula is<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis26Degrees()"/> after
        ''' <seealsocref="GetAlos26Degrees()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha26Degrees() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos26Degrees(), GetTzais26Degrees())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' opinion that the day starts at
        ''' <em><seealsocref="GetAlos18Degrees">alos 18°</seealso></em> and ends at
        ''' <em><seealsocref="GetTzais18Degrees">tzais 18°</seealso></em>. This is calculated
        ''' as 10.75 hours <em>zmaniyos</em> after <seealsocref="GetAlos18Degrees">dawn</seealso>.
        ''' The formula is<br/>
        ''' 10.75 * <seealsocref="GetShaahZmanis18Degrees()"/> after
        ''' <seealsocref="GetAlos18Degrees()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha18Degrees() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos18Degrees(), GetTzais18Degrees())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' opinion that the day starts at
        ''' <em><seealsocref="GetAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ''' <seealsocref="AstronomicalCalendar.GetSunset">sunset</seealso>. 10.75 shaos zmaniyos are calculated based on
        ''' this day and added to <seealsocref="GetAlos16Point1Degrees">alos</seealso> to reach
        ''' this time. This time is 10.75 <em>shaos zmaniyos</em> (temporal hours)
        ''' after <seealsocref="GetAlos16Point1Degrees">dawn</seealso> based on the opinion that
        ''' the day is calculated from a <seealsocref="GetAlos16Point1Degrees">dawn</seealso> of
        ''' 16.1 degrees before sunrise to <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>
        ''' . This returns the time of 10.75 * the calculated
        ''' <em>shaah zmanis</em> after <seealsocref="GetAlos16Point1Degrees">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the plag.
        ''' If the calculation can't be computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        ''' <seealsocref="AstronomicalCalendar.GetSeaLevelSunset"/>
        Public Overridable Function GetPlagAlosToSunset() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos16Point1Degrees(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' opinion that the day starts at
        ''' <em><seecref="GetAlos16Point1Degrees">alos 16.1°</see></em> and ends at
        ''' <seecref="GetTzaisGeonim7Point083Degrees">tzais</see>. 10.75 shaos zmaniyos are
        ''' calculated based on this day and added to
        ''' <seecref="GetAlos16Point1Degrees">alos</see> to reach this time. This time is
        ''' 10.75 <em>shaos zmaniyos</em> (temporal hours) after
        ''' <seecref="GetAlos16Point1Degrees">dawn</see> based on the opinion that the day
        ''' is calculated from a <seecref="GetAlos16Point1Degrees">dawn</see> of 16.1
        ''' degrees before sunrise to <seecref="GetTzaisGeonim7Point083Degrees">tzais</see>
        ''' . This returns the time of 10.75 * the calculated <em>shaah zmanis</em>
        ''' after <seecref="GetAlos16Point1Degrees">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the plag.
        ''' If the calculation can't be computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos16Point1Degrees()"/>
        ''' <seealsocref="GetTzaisGeonim7Point083Degrees()"/>
        Public Overridable Function GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos16Point1Degrees(), GetTzaisGeonim7Point083Degrees())
        End Function

        ''' <summary>
        ''' Method to return <em>Bain Hashmasho</em> of <em>Rabainu Tam</em> calculated when the sun is
        ''' <seealsocref="ZENITH_13_POINT_24"/> below the western <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric horizon</seealso> (90°;)
        ''' after sunset. This calculation is based on the same calculation of <seecref="getBainHasmashosRT58Point5Minutes">
        ''' Bain Hasmashos Rabainu Tam 58.5 minutes</see> but uses a degree based calculation instead of 58.5 exact minutes. This
        ''' calculation is based on the position of the sun 58.5 minutes after sunset in Jerusalem during the equinox which
        ''' calculates to 13.24°; below <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH"/>.<br/>
        ''' 	<br/>
        ''' NOTE: As per Yisroel Vehazmanim Vol III page 1028 No 50, a dip of slightly less than 13°; should be used.
        ''' Calculations show that the proper dip to be 13.2456°; (truncated to 13.24 that provides about 1.5 second
        ''' earlier (<em>lechumra</em>) time) below the horizon at that time. This makes a difference of 1 minute and 10
        ''' seconds in Jerusalem during the Equinox, and 1 minute 29 seconds during the solstice as compared to the proper
        ''' 13.24°;. For NY during the solstice, the difference is 1 minute 56 seconds.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the sun being 13.24° below
        ''' <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see> (90°).
        ''' If the calculation can't be computed such as northern and southern
        ''' locations even south of the Arctic Circle and north of the
        ''' Antarctic Circle where the sun may not reach low enough below the
        ''' horizon for this calculation, a null will be returned. See
        ''' detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        ''' documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_13_DEGREES"/>
        Public Overridable Function GetBainHasmashosRT13Point24Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_13_POINT_24)
        End Function

        ''' <summary>
        ''' This method returns Bain Hashmashos of Rabainu Tam calculated as a 58.5
        ''' minute offset after sunset. Bain hashmashos is 3/4 of a mil before tzais
        ''' or 3 1/4 mil after sunset. With a mil calculated as 18 minutes, 3.25 * 18
        ''' = 58.5 minutes.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of 58.5 minutes after sunset.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetBainHasmashosRT58Point5Minutes() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), 58.5 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns the time of <em>bain hashmashos</em> based on the
        ''' calculation of 13.5 minutes (3/4 of an 18 minute mil before shkiah
        ''' calculated as <seealsocref="GetTzaisGeonim7Point083Degrees">7.083°</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the bain hashmashos of Rabainu Tam in this calculation.
        ''' If the calculation can't be computed such as
        ''' northern and southern locations even south of the Arctic Circle
        ''' and north of the Antarctic Circle where the sun may not reach low
        ''' enough below the horizon for this calculation, a null will be
        ''' returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetTzaisGeonim7Point083Degrees()"/>
        Public Overridable Function GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSunsetOffsetByDegrees(ZENITH_7_POINT_083), -13.5 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns <em>bain hashmashos</em> of Rabainu Tam calculated in
        ''' the opinion of the Divray Yosef (see Yisrael Vehazmanim) calculated
        ''' 5/18th (27.77%) of the time between alos (calculated as 19.8° before
        ''' sunrise) and sunrise. This is added to sunset to arrive at the time for
        ''' bain hashmashos of Rabainu Tam).
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of bain hashmashos of Rabainu Tam for this calculation.
        ''' If the calculation can't be computed such as
        ''' northern and southern locations even south of the Arctic Circle
        ''' and north of the Antarctic Circle where the sun may not reach low
        ''' enough below the horizon for this calculation, a null will be
        ''' returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetBainHasmashosRT2Stars() As DateTime?
            Dim alos19Point8 = GetAlos19Point8Degrees()
            Dim sunrise = MyBase.GetSeaLevelSunrise()
            If alos19Point8 Is Nothing OrElse sunrise Is Nothing Then Return Nothing
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), (sunrise.Value.ToUnixEpochMilliseconds() - alos19Point8.Value.ToUnixEpochMilliseconds()) * (5 / 18.0R))
        End Function

        ''' <summary>
        '''  This method returns the <em>tzais</em> (nightfall) based on the opinion
        '''  of the <em>Geonim</em> calculated at the sun's position at
        '''  <seealsocref="ZENITH_5_POINT_95">5.95°</seealso> below the western horizon.
        ''' </summary>
        ''' <returns> the <c>DateTime</c> representing the time when the sun is
        '''  5.95° below sea level. </returns>
        ''' <seealsocref="ZENITH_5_POINT_95"/>
        ' public Date getTzaisGeonim3Point7Degrees(){
        ' return getSunsetOffsetByDegrees(ZENITH_3_POINT_7);
        ' }

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated at the sun's position at
        ''' <seealsocref="ZENITH_5_POINT_95">5.95°</seealso> below the western horizon.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 5.95° below sea level.
        ''' If the calculation can't be computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_5_POINT_95"/>
        Public Overridable Function GetTzaisGeonim5Point95Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_5_POINT_95)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated calculated as 3/4 of a <ahref="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on an 18 minute Mil, or 13.5 minutes. It is the sun's
        ''' position at <seealsocref="ZENITH_3_POINT_65">3.65°</seealso> below the western
        ''' horizon. This is a very early zman and should not be relied on without
        ''' Rabbinical guidance.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 3.65° below sea level.
        ''' If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_3_POINT_65"/>
        Public Overridable Function GetTzaisGeonim3Point65Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_3_POINT_65)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion of the <em>Geonim</em> calculated as 3/4
        ''' of a <ahref="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on an 18
        ''' minute Mil, or 13.5 minutes. It is the sun's position at <seealsocref="ZENITH_3_POINT_676"/> below the western
        ''' horizon based on the calculations of Stanley Fishkind. This is a very early <em>zman</em> and should not be
        ''' relied on without Rabbinical guidance.
        ''' </summary>
        ''' <returns> the <code>DateTime</code> representing the time when the sun is 3.676°; below sea level. If the
        '''         calculation can't be computed such as northern and southern locations even south of the Arctic Circle and
        '''         north of the Antarctic Circle where the sun may not reach low enough below the horizon for this
        '''         calculation, a null will be returned. See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/>
        '''         documentation. </returns>
        ''' <seealsocref="ZENITH_3_POINT_676"/>
        Public Overridable Function GetTzaisGeonim3Point676Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_3_POINT_676)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated as 3/4 of a <ahref="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on a 24 minute Mil, or 18 minutes. It is the sun's
        ''' position at <seealsocref="ZENITH_4_POINT_61">4.61°</seealso> below the western
        ''' horizon. This is a very early zman and should not be relied on without
        ''' Rabbinical guidance.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 4.61° below sea level.
        ''' If the calculation can't be computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_4_POINT_61"/>
        Public Overridable Function GetTzaisGeonim4Point61Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_4_POINT_61)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated as 3/4 of a <ahref="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a>, based on a 22.5 minute Mil, or 16 7/8 minutes. It is the sun's
        ''' position at <seealsocref="ZENITH_4_POINT_37">4.37°</seealso> below the western
        ''' horizon. This is a very early zman and should not be relied on without
        ''' Rabbinical guidance.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 4.37° below sea level.
        ''' If the calculation can't be computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_4_POINT_37"/>
        Public Overridable Function GetTzaisGeonim4Point37Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_4_POINT_37)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated as 3/4 of a <ahref="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a>. It is based on the Baal Hatanya based on a Mil being 24
        ''' minutes, and is calculated as 18 +2 + 4 for a total of 24 minutes (FIXME:
        ''' additional details needed). It is the sun's position at
        ''' <seecref="ZENITH_5_POINT_88">5.88°</see> below the western horizon. This is a
        ''' very early zman and should not be relied on without Rabbinical guidance.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is
        ''' 5.88° below sea level.
        ''' If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_5_POINT_88"/>
        Public Overridable Function GetTzaisGeonim5Point88Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_5_POINT_88)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated as 3/4 of a <ahref="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">
        ''' Mil</a>. It is the sun's position at <seecref="ZENITH_4_POINT_8">4.8°</see>
        ''' below the western horizon based on Rabbi Leo Levi's calculations. (FIXME:
        ''' additional documentation needed) This is the This is a very early zman
        ''' and should not be relied on without Rabbinical guidance.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 4.8° below sea level.
        ''' If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_4_POINT_8"/>
        Public Overridable Function GetTzaisGeonim4Point8Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_4_POINT_8)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated at the sun's position at
        ''' <seecref="ZENITH_7_POINT_083">7.083°</see> below the western horizon.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 7.083° below sea level.
        ''' If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_7_POINT_083"/>
        Public Overridable Function GetTzaisGeonim7Point083Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_7_POINT_083)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the <em>Geonim</em> calculated at the sun's position at
        ''' <seecref="ZmanimCalendar.ZENITH_8_POINT_5">8.5°</see> below the western horizon.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time when the sun is 8.5° below sea level.
        ''' If the calculation can't be computed
        ''' such as northern and southern locations even south of the Arctic
        ''' Circle and north of the Antarctic Circle where the sun may not
        ''' reach low enough below the horizon for this calculation, a
        ''' null will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.ZENITH_8_POINT_5"/>
        Public Overridable Function GetTzaisGeonim8Point5Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_8_POINT_5)
        End Function

        ''' <summary>
        ''' This method returns the <em>tzais</em> (nightfall) based on the opinion
        ''' of the Chavas Yair and Divray Malkiel that the time to walk the distance
        ''' of a Mil is 15 minutes for a total of 60 minutes for 4 mil after
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing 60 minutes after sea level sunset.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos60()"/>
        Public Overridable Function GetTzais60() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), 60 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns tzais usually calculated as 40 minutes after sunset.
        ''' Please note that Chacham Yosef Harari-Raful of Yeshivat Ateret Torah who
        ''' uses this time, does so only for calculating various other zmanai hayom
        ''' such as Sof Zman Krias Shema and Plag Hamincha. His calendars do not
        ''' publish a zman for Tzais. It should also be noted that Chacham
        ''' Harari-Raful provided a 25 minute zman for Israel. This API uses 40
        ''' minutes year round in any place on the globe by default. This offset can
        ''' be changed by calling <seecref="AteretTorahSunsetOffset"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing 40 minutes
        ''' (setable via <seecref="AteretTorahSunsetOffset"/>) after sea level sunset.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="AteretTorahSunsetOffset"/>
        Public Overridable Function GetTzaisAteretTorah() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), AteretTorahSunsetOffset * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' Gets or Sets the offset in minutes after sunset for the Ateret Torah
        ''' zmanim. The default if unset is 40 minutes. Chacham Yosef Harari-Raful of
        ''' Yeshivat Ateret Torah uses 40 minutes globally with the exception of
        ''' Israel where a 25 minute offset is used. This 25 minute (or any other)
        ''' offset can be overridden by this methd. This offset impacts all Ateret
        ''' Torah methods.
        ''' --
        ''' Returns the offset in minutes after sunset used to calculate
        ''' <em>tzais</em> for the Ateret Torah zmanim. The defaullt value is 40
        ''' minutes.
        ''' </summary>
        ''' <value>the number of minutes after sunset to use as an offset for the
        '''   Ateret Torah &lt;em&gt;tzais&lt;/em&gt;</value>
        Public Overridable Property AteretTorahSunsetOffset As Double
            Get
                Return ateretTorahSunsetOffsetField
            End Get
            Set(ByVal value As Double)
                ateretTorahSunsetOffsetField = value
            End Set
        End Property

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) based on the calculation of Chacham Yosef
        ''' Harari-Raful of Yeshivat Ateret Torah, that the day starts
        ''' <seecref="GetAlos72Zmanis">1/10th of the day</see> before sunrise and is
        ''' usually calculated as ending <seecref="GetTzaisAteretTorah()">40 minutes after sunset</see>
        ''' . <em>shaos zmaniyos</em> are calculated based on this day
        ''' and added to <seecref="GetAlos72Zmanis">alos</see> to reach this time. This
        ''' time is 3 <em>
        ''' 		<seecref="GetShaahZmanisAteretTorah">shaos zmaniyos</see></em>
        ''' (temporal hours) after <seecref="GetAlos72Zmanis">alos 72 zmaniyos</see>.<br/>
        ''' 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ''' midday.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema based on this
        ''' calculation.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzaisAteretTorah()"/>
        ''' <seealsocref="AteretTorahSunsetOffset"/>
        ''' <seealsocref="GetShaahZmanisAteretTorah()"/>
        Public Overridable Function GetSofZmanShmaAteretTorah() As DateTime?
            Return MyBase.GetSofZmanShma(GetAlos72Zmanis(), GetTzaisAteretTorah())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) based on the calculation of Chacham Yosef Harari-Raful
        ''' of Yeshivat Ateret Torah, that the day starts <seecref="GetAlos72Zmanis()"> 1/10th of the day</see>
        ''' before sunrise and and is usually calculated as ending
        ''' <seecref="GetTzaisAteretTorah">40 minutes after sunset</see>.
        ''' <em>shaos zmaniyos</em> are calculated based on this day and added to
        ''' <seecref="GetAlos72Zmanis">alos</see> to reach this time. This time is 4
        ''' <em><seecref="GetShaahZmanisAteretTorah">shaos zmaniyos</see></em> (temporal
        ''' hours) after <seecref="GetAlos72Zmanis">alos 72 zmaniyos</see>.<br/>
        ''' 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ''' midday.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema based on this
        ''' calculation.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzaisAteretTorah()"/>
        ''' <seealsocref="GetShaahZmanisAteretTorah()"/>
        ''' <seealsocref="AteretTorahSunsetOffset"/>
        Public Overridable Function GetSofZmanTfilahAteretTorah() As DateTime?
            Return MyBase.GetSofZmanTfila(GetAlos72Zmanis(), GetTzaisAteretTorah())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha gedola</em> based on the
        ''' calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        ''' the day starts <seecref="GetAlos72Zmanis">1/10th of the day</see> before
        ''' sunrise and and is usually calculated as ending
        ''' <seecref="GetTzaisAteretTorah">40 minutes after sunset</see>. This is the
        ''' perfered earliest time to pray <em>mincha</em> in the opinion of the
        ''' Ramba"m and others. For more information on this see the documentation on
        ''' <em><seecref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        ''' 6.5 <seecref="GetShaahZmanisAteretTorah">solar hours</see> after alos. The
        ''' calculation used is 6.5 * <seealsocref="GetShaahZmanisAteretTorah()"/> after
        ''' <seecref="GetAlos72Zmanis">alos</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha gedola.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzaisAteretTorah()"/>
        ''' <seealsocref="GetShaahZmanisAteretTorah()"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="GetMinchaKetanaAteretTorah()"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        Public Overridable Function GetMinchaGedolaAteretTorah() As DateTime?
            Return MyBase.GetMinchaGedola(GetAlos72Zmanis(), GetTzaisAteretTorah())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha ketana</em> based on the
        ''' calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        ''' the day starts <seecref="GetAlos72Zmanis">1/10th of the day</see> before
        ''' sunrise and and is usually calculated as ending
        ''' <seecref="GetTzaisAteretTorah">40 minutes after sunset</see>. This is the
        ''' perfered earliest time to pray <em>mincha</em> in the opinion of the
        ''' Ramba"m and others. For more information on this see the documentation on
        ''' <em><seecref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        ''' 9.5 <seecref="GetShaahZmanisAteretTorah">solar hours</see> after
        ''' <seecref="GetAlos72Zmanis">alos</see>. The calculation used is 9.5 *
        ''' <seecref="GetShaahZmanisAteretTorah()"/> after <seecref="GetAlos72Zmanis()"> alos</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha ketana.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzaisAteretTorah()"/>
        ''' <seealsocref="GetShaahZmanisAteretTorah()"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaGedola"/>
        ''' <seealsocref="ZmanimCalendar.GetMinchaKetana"/>
        Public Overridable Function GetMinchaKetanaAteretTorah() As DateTime?
            Return MyBase.GetMinchaKetana(GetAlos72Zmanis(), GetTzaisAteretTorah())
        End Function

        ''' <summary>
        ''' This method returns the time of <em>plag hamincha</em> based on the
        ''' calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        ''' the day starts <seecref="GetAlos72Zmanis">1/10th of the day</see> before
        ''' sunrise and and is usually calculated as ending
        ''' <seecref="GetTzaisAteretTorah">40 minutes after sunset</see>.
        ''' <em>shaos zmaniyos</em> are calculated based on this day and added to
        ''' <seecref="GetAlos72Zmanis">alos</see> to reach this time. This time is 10.75
        ''' <em><seecref="GetShaahZmanisAteretTorah">shaos zmaniyos</see></em> (temporal
        ''' hours) after <seecref="GetAlos72Zmanis">dawn</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the plag.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        ''' <seealsocref="GetTzaisAteretTorah()"/>
        ''' <seealsocref="GetShaahZmanisAteretTorah()"/>
        Public Overridable Function GetPlagHaminchaAteretTorah() As DateTime?
            Return MyBase.GetPlagHamincha(GetAlos72Zmanis(), GetTzaisAteretTorah())
        End Function

        ' 
        ' ///	 <summary> 
        ' ///	This method returns the time of <em>misheyakir</em> based on the common
        ' ///	calculation of the Syrian community in NY that the alos is a fixed minute
        ' ///	offset from day starting <seealso cref="GetAlos72Zmanis">1/10th of the day</seealso>
        ' ///	before sunrise. The common offsets are 6 minutes (based on th Pri
        ' ///	Megadim, but not linked to the calculation of Alos as 1/10th of the day),
        ' ///	8 and 18 minutes (possibly attributed to Chacham Baruch Ben Haim). Since
        ' ///	there is no universal accepted offset, the user of this API will have to
        ' ///	specify one. Chacham Yosef Harari-Raful of Yeshivat Ateret Torah does not
        ' ///	supply any zman for misheyakir and does not endorse any specific
        ' ///	calculation for misheyakir. For that reason, this method is not enabled.
        ' ///	 </summary>
        ' ///	<param name="minutes">
        ' ///	           the number of minutes after alos calculated as
        ' ///	           <seealso cref="GetAlos72Zmanis">1/10th of the day</seealso> </param>
        ' ///	<returns> the <c>DateTime</c> of misheyakir </returns>
        ' ///	<seealso cref="GetAlos72Zmanis()"/>
        ' ///	 
        ' // public Date getMesheyakirAteretTorah(double minutes) {
        ' // return getTimeOffset(GetAlos72Zmanis(), minutes * MINUTE_MILLIS);
        ' // }
        ' 

        ''' <summary>
        ''' Method to return <em>tzais</em> (dusk) calculated as 72 minutes zmaniyos,
        ''' or 1/10th of the day after <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos72Zmanis()"/>
        Public Overridable Function GetTzais72Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()
            If shaahZmanis = Long.MinValue Then Return Nothing
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), shaahZmanis * 1.2)
        End Function

        ''' <summary>
        ''' Method to return <em>tzais</em> (dusk) calculated using 90 minutes
        ''' zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos90Zmanis()"/>
        Public Overridable Function GetTzais90Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()
            If shaahZmanis = Long.MinValue Then Return Nothing
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), shaahZmanis * 1.5)
        End Function

        ''' <summary>
        ''' Method to return <em>tzais</em> (dusk) calculated using 96 minutes
        ''' zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos96Zmanis()"/>
        Public Overridable Function GetTzais96Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()
            If shaahZmanis = Long.MinValue Then Return Nothing
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), shaahZmanis * 1.6)
        End Function

        ''' <summary>
        ''' Method to return <em>tzais</em> (dusk) calculated as 90 minutes after sea
        ''' level sunset. This method returns <em>tzais</em> (nightfall) based on the
        ''' opinion of the Magen Avraham that the time to walk the distance of a Mil
        ''' in the Ramba"m's opinion is 18 minutes for a total of 90 minutes based on
        ''' the opinion of <em>Ula</em> who calculated <em>tzais</em> as 5 Mil after
        ''' sea level shkiah (sunset). A similar calculation
        ''' <seecref="GetTzais19Point8Degrees()"/>uses solar position calculations based
        ''' on this time.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetTzais19Point8Degrees()"/>
        ''' <seealsocref="GetAlos90()"/>
        Public Overridable Function GetTzais90() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), 90 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns <em>tzais</em> (nightfall) based on the opinion of
        ''' the Magen Avraham that the time to walk the distance of a Mil in the
        ''' Ramba"ms opinion is 2/5 of an hour (24 minutes) for a total of 120
        ''' minutes based on the opinion of <em>Ula</em> who calculated
        ''' <em>tzais</em> as 5 Mil after sea level shkiah (sunset). A similar
        ''' calculation <seecref="GetTzais26Degrees()"/> uses temporal calculations based
        ''' on this time.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetTzais26Degrees()"/>
        ''' <seealsocref="GetAlos120()"/>
        Public Overridable Function GetTzais120() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), 120 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' Method to return <em>tzais</em> (dusk) calculated using 120 minutes
        ''' zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos120Zmanis()"/>
        Public Overridable Function GetTzais120Zmanis() As DateTime?
            Dim shaahZmanis As Long = MyBase.GetShaahZmanisGra()
            If shaahZmanis = Long.MinValue Then Return Nothing
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), shaahZmanis * 2.0)
        End Function

        ''' <summary>
        ''' For information on how this is calculated see the comments on
        ''' <seecref="GetAlos16Point1Degrees()"/>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation
        ''' can't be computed such as northern and southern locations even
        ''' south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this
        ''' calculation, a null will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZmanimCalendar.GetTzais72"/>
        ''' <seealsocref="GetAlos16Point1Degrees">for more information on this calculation.</seealso>
        Public Overridable Function GetTzais16Point1Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_16_POINT_1)
        End Function

        ''' <summary>
        ''' For information on how this is calculated see the comments on
        ''' <seecref="GetAlos26Degrees()"/>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetTzais120()"/>
        ''' <seealsocref="GetAlos26Degrees()"/>
        Public Overridable Function GetTzais26Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_26_DEGREES)
        End Function

        ''' <summary>
        ''' For information on how this is calculated see the comments on
        ''' <seecref="GetAlos18Degrees()"/>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation
        ''' can't be computed such as northern and southern locations even
        ''' south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this
        ''' calculation, a null will be returned. See detailed explanation on
        ''' top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos18Degrees()"/>
        Public Overridable Function GetTzais18Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ASTRONOMICAL_ZENITH)
        End Function

        ''' <summary>
        ''' For information on how this is calculated see the comments on
        ''' <seecref="GetAlos19Point8Degrees()"/>
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be
        ''' computed such as northern and southern locations even south of
        ''' the Arctic Circle and north of the Antarctic Circle where the sun
        ''' may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetTzais90()"/>
        ''' <seealsocref="GetAlos19Point8Degrees()"/>
        Public Overridable Function GetTzais19Point8Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_19_POINT_8)
        End Function

        ''' <summary>
        ''' A method to return <em>tzais</em> (dusk) calculated as 96 minutes after
        ''' sea level sunset. For information on how this is calculated see the
        ''' comments on <seecref="GetAlos96()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as in the Arctic Circle
        ''' where there is at least one day a year where the sun does not
        ''' rise, and one where it does not set, a null will be returned. See
        ''' detailed explanation on top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetAlos96()"/>
        Public Overridable Function GetTzais96() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset(), 96 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' A method that returns the local time for fixed <em>chatzos</em>. This
        ''' time is noon and midnight adjusted from standard time to account for the
        ''' local latitude. The 360° of the globe divided by 24 calculates to
        ''' 15° per hour with 4 minutes per degree, so at a longitude of 0 , 15,
        ''' 30 etc Chatzos in 12:00 noon. Lakewood, NJ whose longitude is -74.2094 is
        ''' 0.7906 away from the closest multiple of 15 at -75°. This is
        ''' multiplied by 4 to yeild 3 minutes and 10 seconds for a chatzos of
        ''' 11:56:50. This method is not tied to the theoretical 15° timezones,
        ''' but will adjust to the actual timezone and <ahref="http://en.wikipedia.org/wiki/Daylight_saving_time">Daylight saving
        ''' time</a>.
        ''' </summary>
        ''' <returns>
        ''' the DateTime representing the local <em>chatzos</em>
        ''' </returns>
        ''' <seealsocref="GeoLocation.GetLocalMeanTimeOffset"/>
        Public Overridable Function GetFixedLocalChatzos() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetDateFromTime(12.0 - MyBase.DateWithLocation.Location.TimeZone.UtcOffset(MyBase.DateWithLocation.Date) / HOUR_MILLIS), -MyBase.DateWithLocation.Location.GetLocalMeanTimeOffset(MyBase.DateWithLocation.Date))
        End Function

        ''' <summary>
        ''' A method that returns the latest <em>zman krias shema</em> (time to say
        ''' Shema in the morning) calculated as 3 hours before
        ''' <seecref="GetFixedLocalChatzos()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman shema.
        ''' </returns>
        ''' <seealsocref="GetFixedLocalChatzos()"/>
        ''' <seealsocref="GetSofZmanTfilaFixedLocal()"/>
        Public Overridable Function GetSofZmanShmaFixedLocal() As DateTime?
            Return MyBase.GetTimeOffset(GetFixedLocalChatzos(), -180 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) calculated as 2 hours before
        ''' <seecref="GetFixedLocalChatzos()"/>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman tfila.
        ''' </returns>
        ''' <seealsocref="GetFixedLocalChatzos()"/>
        ''' <seealsocref="GetSofZmanShmaFixedLocal()"/>
        Public Overridable Function GetSofZmanTfilaFixedLocal() As DateTime?
            Return MyBase.GetTimeOffset(GetFixedLocalChatzos(), -120 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This method returns the latest time one is allowed eating chametz on Erev Pesach according to the opinion of the
        ''' <em>GRA</em> and the </em>Baal Hatanya</em>. This time is identical to the {@link #getSofZmanTfilaGRA() Sof zman
        ''' tefilah GRA}. This time is 4 hours into the day based on the opinion of the <em>GRA</em> and the </em>Baal
        ''' Hatanya</em> that the day is calculated from sunrise to sunset. This returns the time 4 *
        ''' <seealso cref="GetShaahZmanisGra()"/> after <seealso cref="GetSeaLevelSunrise() sea level sunrise"/>.
        ''' </summary>
        ''' <seealso cref="ZmanimCalendar#getShaahZmanisGra"></seealso>
        ''' <seealso cref="ZmanimCalendar#getSofZmanTfilaGRA"></seealso>
        ''' <returns> the <code>Date</code> one is allowed eating chametz on Erev Pesach. If the calculation can't be computed
        '''         such as in the Arctic Circle where there is at least one day a year where the sun does not rise, and one
        '''         where it does not set, a null will be returned. See detailed explanation on top of the
        '''         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function getSofZmanAchilasChametzGRA() As DateTime?
            Return MyBase.GetSofZmanTfilaGRA()
        End Function

        ''' <summary>
        ''' This method returns the latest time one is allowed eating chametz on Erev Pesach according to the opinion of the
        ''' <em>MGA</em> based on <em>alos</em> being <seealsocref="GetAlos72()"/> minutes before <seealsocref="GetSunrise()"/>.
        ''' This time is identical to the <seealsocref="GetSofZmanTfilaMGA72Minutes()"/>. This time
        ''' is 4 <em><seealsocref="GetShaahZmanisMGA()"/></em> (temporal hours) after <seealsocref="GetAlos72()"/> based
        ''' on the opinion of the <em>MGA</em> that the day is calculated from a <seealsocref="GetAlos72()"/> of 72 minutes
        ''' before sunrise to <seealsocref="GetTzais72()"/> of 72 minutes after sunset. This returns the time of 4 *
        ''' <seealsocref="GetShaahZmanisMGA()"/> after <seealsocref="GetAlos72()"/>.
        ''' </summary>
        ''' <returns> the <code>Date</code> of the latest time of eating chametz. If the calculation can't be computed such as
        '''         in the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        '''         does not set), a null will be returned. See detailed explanation on top of the
        '''         <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        ''' <seealsocref="GetShaahZmanisMGA"></seealso>
        ''' <seealsocref="GetAlos72"></seealso>
        ''' <seealsocref="GetSofZmanTfilaMGA72Minutes"></seealso>
        Public Overridable Function GetSofZmanAchilasChametzMGA72Minutes() As DateTime?
            Return GetSofZmanTfilaMGA72Minutes()
        End Function

        ''' <summary>
        ''' This method returns the latest time one is allowed eating chametz on Erev Pesach according to the opinion of the
        ''' <em>MGA</em> based on <em>alos</em> being <seealsocref="GetAlos16Point1Degrees()"/> before
        ''' <seealsocref="GetSunrise()"/>. This time is 4 <em><seealsocref="GetShaahZmanis16Point1Degrees()"/></em>
        ''' (solar hours) after <seealsocref="GetAlos16Point1Degrees()"/> based on the opinion of the <em>MGA</em> that the day
        ''' is calculated from dawn to nightfall with both being 16.1&deg; below sunrise or sunset. This returns the time of
        ''' 4 <seealsocref="GetShaahZmanis16Point1Degrees()"/> after <seealsocref="GetAlos16Point1Degrees()"/>.
        ''' </summary>
        ''' <returns> the <code>Date</code> of the latest time of eating chametz. If the calculation can't be computed such as
        '''         northern and southern locations even south of the Arctic Circle and north of the Antarctic Circle where
        '''         the sun may not reach low enough below the horizon for this calculation, a null will be returned. See
        '''         detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees"></seealso>
        ''' <seealsocref="GetAlos16Point1Degrees"></seealso>
        ''' <seealsocref="GetSofZmanTfilaMGA16Point1Degrees"></seealso>
        Public Overridable Function GetSofZmanAchilasChametzMGA16Point1Degrees() As DateTime?
            Return GetSofZmanTfilaMGA16Point1Degrees()
        End Function

        ''' <summary>
        ''' This method returns the latest time for burning chametz on Erev Pesach according to the opinion of the
        ''' <em>GRA</em> and the </em>Baal Hatanya</em>. This time is 5 hours into the day based on the opinion of the
        ''' <em>GRA</em> and the </em>Baal Hatanya</em> that the day is calculated from sunrise to sunset. This returns the
        ''' time 5 * <seealso cref="GetShaahZmanisGra()"/> after <seealso cref="GetSeaLevelSunrise() sea level sunrise"/>.
        ''' </summary>
        ''' <seealso cref="ZmanimCalendar#getShaahZmanisGra"></seealso>
        ''' <returns> the <code>Date</code> of the latest time for burning chametz on Erev Pesach. If the calculation can't be
        '''         computed such as in the Arctic Circle where there is at least one day a year where the sun does not rise,
        '''         and one where it does not set, a null will be returned. See detailed explanation on top of the
        '''         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetSofZmanBiurChametzGRA() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise(), MyBase.GetShaahZmanisGra() * 5)
        End Function

        ''' <summary>
        ''' This method returns the latest time for burning chametz on Erev Pesach according to the opinion of the
        ''' <em>MGA</em> based on <em>alos</em> being <seealsocref="GetAlos72()"/> minutes before <seealsocref="GetSunrise()"/>.
        ''' This time is 5 <em><seealsocref="GetShaahZmanisMGA()"/></em> (temporal hours) after {@link #getAlos72()
        ''' dawn} based on the opinion of the <em>MGA</em> that the day is calculated from a <seealsocref="GetAlos72()"/> of 72
        ''' minutes before sunrise to <seealsocref="GetTzais72()"/> of 72 minutes after sunset. This returns the time of 5
        ''' * <seealsocref="GetShaahZmanisMGA"/> after <seealsocref="GetAlos72()"/>.
        ''' </summary>
        ''' <returns> the <code>Date</code> of the latest time for burning chametz on Erev Pesach. If the calculation can't be
        '''         computed such as in the Arctic Circle where there is at least one day a year where the sun does not rise,
        '''         and one where it does not set), a null will be returned. See detailed explanation on top of the
        '''         <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        ''' <seealsocref="GetShaahZmanisMGA"></seealso>
        ''' <seealsocref="GetAlos72()"></seealso>
        Public Overridable Function GetSofZmanBiurChametzMGA72Minutes() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetAlos72(), MyBase.GetShaahZmanisMGA() * 5)
        End Function

        ''' <summary>
        ''' This method returns the latest time for burning <em>chametz</em> on <em>Erev Pesach</em> according to the opinion
        ''' of the <em>MGA</em> based on <em>alos</em> being <seealsocref="GetAlos16Point1Degrees()"/> before
        ''' <seealsocref="GetSunrise()"/>. This time is 5 <em><seealsocref="GetShaahZmanis16Point1Degrees()"/></em>
        ''' (solar hours) after <seealsocref="GetAlos16Point1Degrees()"/> based on the opinion of the <em>MGA</em> that the day
        ''' is calculated from dawn to nightfall with both being 16.1&deg; below sunrise or sunset. This returns the time of
        ''' 5 <seealsocref="GetShaahZmanis16Point1Degrees()"/> after <seealsocref="GetAlos16Point1Degrees()"/>.
        ''' </summary>
        ''' <returns> the <code>Date</code> of the latest time for burning chametz on Erev Pesach. If the calculation can't be
        '''         computed such as northern and southern locations even south of the Arctic Circle and north of the
        '''         Antarctic Circle where the sun may not reach low enough below the horizon for this calculation, a null
        '''         will be returned. See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanis16Point1Degrees"></seealso>
        ''' <seealsocref="GetAlos16Point1Degrees"></seealso>
        Public Overridable Function GetSofZmanBiurChametzMGA16Point1Degrees() As DateTime?
            Return MyBase.GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 5)
        End Function

        ''' <summary>
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        ''' </returns>
        Public Overrides Function GetHashCode() As Integer
            Dim num = &H11
            num = &H25 * num + MyBase.GetTemporalHour().GetHashCode()
            num += &H25 * num + MyBase.DateWithLocation.GetHashCode()
            num += &H25 * num + MyBase.DateWithLocation.Location.GetHashCode()
            Return (num + (&H25 * num + MyBase.AstronomicalCalculator.GetHashCode()))
        End Function


        ''' Below are Methods added to kosherjava and not yet ported to NET            
        Protected Friend Const ZENITH_19_DEGREES As Double = GEOMETRIC_ZENITH + 19
        Protected Friend Const ZENITH_16_POINT_9 As Double = GEOMETRIC_ZENITH + 16.9
        Protected Friend Const ZENITH_MINUS_2_POINT_1 As Double = GEOMETRIC_ZENITH - 2.1
        Protected Friend Const ZENITH_MINUS_2_POINT_8 As Double = GEOMETRIC_ZENITH - 2.8
        Protected Friend Const ZENITH_MINUS_3_POINT_05 As Double = GEOMETRIC_ZENITH - 3.05
        Protected Friend Const ZENITH_1_POINT_583 As Double = GEOMETRIC_ZENITH + 1.583
        Protected Friend Const ZENITH_7_POINT_65 As Double = GEOMETRIC_ZENITH + 7.65
        Protected Friend Const ZENITH_9_POINT_5 As Double = GEOMETRIC_ZENITH + 9.5
        Protected Friend Const ZENITH_6_DEGREES As Double = GEOMETRIC_ZENITH + 6
        Protected Friend Const ZENITH_3_POINT_8 As Double = GEOMETRIC_ZENITH + 3.8
        Protected Friend Const ZENITH_6_POINT_45 As Double = GEOMETRIC_ZENITH + 6.45
        Protected Friend Const ZENITH_7_POINT_67 As Double = GEOMETRIC_ZENITH + 7.67
        Protected Friend Const ZENITH_9_POINT_3 As Double = GEOMETRIC_ZENITH + 9.3
        Protected Friend Const ZENITH_9_POINT_75 As Double = GEOMETRIC_ZENITH + 9.75

        Public Overridable Function GetAlos19Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_19_DEGREES)
        End Function

        Public Overridable Function GetBainHasmashosYereim13Point5Minutes() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSunset(), -13.5 * MINUTE_MILLIS)
        End Function

        Public Overridable Function GetBainHasmashosYereim16Point875Minutes() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSunset(), -16.875 * MINUTE_MILLIS)
        End Function

        Public Overridable Function GetBainHasmashosYereim18Minutes() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSunset(), -18 * MINUTE_MILLIS)
        End Function

        Public Overridable Function GetBainHasmashosYereim2Point1Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_MINUS_2_POINT_1)
        End Function

        Public Overridable Function GetBainHasmashosYereim2Point8Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_MINUS_2_POINT_8)
        End Function

        Public Overridable Function GetBainHasmashosYereim3Point05Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_MINUS_3_POINT_05)
        End Function

        Public Overridable Function GetAlosBaalHatanya() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_16_POINT_9)
        End Function

        Public Overridable Function GetSunriseBaalHatanya() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_1_POINT_583)
        End Function

        Public Overridable Function GetSunsetBaalHatanya() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_1_POINT_583)
        End Function

        Public Overridable Function GetMinchaGedolaBaalHatanya() As DateTime?
            Return MyBase.GetMinchaGedola(GetSunriseBaalHatanya(), GetSunsetBaalHatanya())
        End Function

        Public Overridable Function GetMinchaGedolaBaalHatanyaGreaterThan30() As DateTime?
            Dim minchaGedola = GetMinchaGedolaBaalHatanya()
            Dim minchaGedola30Minutes = GetMinchaGedola30Minutes()
            If minchaGedola30Minutes Is Nothing OrElse minchaGedola Is Nothing Then Return Nothing
            Return If(minchaGedola30Minutes.Value.CompareTo(minchaGedola.Value) > 0, minchaGedola30Minutes, minchaGedola)
        End Function

        Public Overridable Function GetMinchaKetanaBaalHatanya() As DateTime?
            Return MyBase.GetMinchaKetana(GetSunriseBaalHatanya(), GetSunsetBaalHatanya())
        End Function

        Public Overridable Function GetMisheyakir7Point65Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_7_POINT_65)
        End Function

        Public Overridable Function GetMisheyakir9Point5Degrees() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_9_POINT_5)
        End Function

        Public Overridable Function GetPlagHaminchaBaalHatanya() As DateTime?
            Return MyBase.GetPlagHamincha(GetSunriseBaalHatanya(), GetSunsetBaalHatanya())
        End Function

        Public Overridable Function GetShaahZmanisBaalHatanya() As Long
            Return MyBase.GetTemporalHour(GetSunriseBaalHatanya(), GetSunsetBaalHatanya())
        End Function

        Public Overridable Function GetSofZmanBiurChametzBaalHatanya() As DateTime?
            Return MyBase.GetTimeOffset(GetSunriseBaalHatanya(), GetShaahZmanisBaalHatanya() * 5)
        End Function

        Public Overridable Function GetSofZmanAchilasChametzBaalHatanya() As DateTime?
            Return GetSofZmanTfilaBaalHatanya()
        End Function

        Public Overridable Function GetSofZmanShmaBaalHatanya() As DateTime?
            Return MyBase.GetSofZmanShma(GetSunriseBaalHatanya(), GetSunsetBaalHatanya())
        End Function

        Public Overridable Function GetSofZmanTfilaBaalHatanya() As DateTime?
            Return MyBase.GetSofZmanTfila(GetSunriseBaalHatanya(), GetSunsetBaalHatanya())
        End Function

        Public Overridable Function GetTzaisBaalHatanya() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_6_DEGREES)
        End Function

        Public Overridable Function GetTzaisGeonim3Point7Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_3_POINT_7)
        End Function

        Public Overridable Function GetTzaisGeonim3Point8Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_3_POINT_8)
        End Function

        Public Overridable Function GetTzaisGeonim6Point45Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_6_POINT_45)
        End Function

        Public Overridable Function GetTzaisGeonim7Point67Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_7_POINT_67)
        End Function

        Public Overridable Function GetTzaisGeonim9Point3Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_9_POINT_3)
        End Function

        Public Overridable Function GetTzaisGeonim9Point75Degrees() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_9_POINT_75)
        End Function


        ''' Methods still not ported
        ''' getMidnightLastNight
        ''' getMidnightTonight
        ''' getMoladBasedTime
        ''' getSofZmanKidushLevana15Days
        ''' getSofZmanKidushLevanaBetweenMoldos
        ''' getTchilasZmanKidushLevana3Days
        ''' getTchilasZmanKidushLevana7Days
        ''' getZmanMolad
        ''' getElevationAdjustedSunrise
        ''' 


    End Class

End Namespace