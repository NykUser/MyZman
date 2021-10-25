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

Imports Zmanim.Utilities
Namespace Zmanim
    ''' <summary>
    ''' 	<p> Description: A .NET library for calculating zmanim. </p>
    ''' The zmanim library is an API is a specialized calendar that can calculate
    ''' sunrise and sunset and Jewish <em>zmanim</em> (religious times) for prayers
    ''' and other Jewish religious duties. For a much more extensive list of zmanim
    ''' use the <seealsocref="ComplexZmanimCalendar"/> that extends this class. This class
    ''' contains the main functionality of the Zmanim library. See documentation for
    ''' the <seealsocref="ComplexZmanimCalendar"/> and <seealsocref="AstronomicalCalendar"/> for simple
    ''' examples on using the API.<br/>
    ''' 	<b>Note:</b> It is important to read the technical notes on top of the
    ''' <seecref="AstronomicalCalculator"/> documentation. <h2>
    ''' Disclaimer:</h2> While I did my best to get accurate results please do not
    ''' rely on these zmanim for <em>halacha lemaaseh</em>.
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class ZmanimCalendar
        Inherits AstronomicalCalendar

        Private candleLightingOffsetField As Double = 18

        ''' <summary>
        '''  The zenith of 16.1° below geometric zenith (90°). This
        '''  calculation is used for calculating <em>alos</em> (dawn) and
        '''  <em>tzais</em> (nightfall) in some opinions. This calculation is based on
        '''  the calculation that the time between dawn and sunrise (and sunset to
        '''  nightfall) is the time that is takes to walk 4 <em>mil</em> at 18 minutes
        '''  a mil (<em>Ramba"m</em> and others). The sun's position at 72 minutes
        '''  before <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on the equinox is
        '''  16.1° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        ''' </summary>
        ''' <seealsocref="GetAlosHashachar"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetAlos16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetTzais16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetSofZmanShmaMGA16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetSofZmanTfilaMGA16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetMinchaGedola16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetMinchaKetana16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetPlagHamincha16Point1Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees()"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetSofZmanShmaAlos16Point1ToSunset()"/>
        Protected Friend Const ZENITH_16_POINT_1 As Double = GEOMETRIC_ZENITH + 16.1

        ''' <summary>
        '''  The zenith of 8.5° below geometric zenith (90°). This calculation
        '''  is used for calculating <em>alos</em> (dawn) and <em>tzais</em>
        '''  (nightfall) in some opinions. This calculation is based on the position
        '''  of the sun 36 minutes after <seecref="AstronomicalCalendar.GetSunset">sunset</see> in Jerusalem on
        '''  March 16, about 4 days before the equinox, the day that a solar hour is
        '''  one hour, which is 8.5° below <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        '''  The Ohr Meir considers this the time that 3 small starts are
        '''  visible, later than the required 3 medium stars.
        ''' </summary>
        ''' <seealsocref="GetTzais"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetTzaisGeonim8Point5Degrees"/>
        Protected Friend Const ZENITH_8_POINT_5 As Double = GEOMETRIC_ZENITH + 8.5

        ''' <summary>
        '''  Default constructor will set a default <seecref="GeoLocation"/>,
        '''  a default <seecref="AstronomicalCalculator.GetDefault"> AstronomicalCalculator</see>
        '''  and default the calendar to the current date.
        ''' </summary>
        ''' <seealsocref="AstronomicalCalendar"/>
        Public Sub New()
        End Sub

        ''' <summary>
        '''  A constructor that takes a <seealsocref="GeoLocation"/> as a parameter.
        ''' </summary>
        ''' <paramname="location">
        '''  the location </param>
        Public Sub New(ByVal location As IGeoLocation)
            MyBase.New(location)
        End Sub

        ''' <summary>
        ''' A constructor that takes a <seealsocref="GeoLocation"/> as a parameter.
        ''' </summary>
        ''' <paramname="date">The date.</param>
        ''' <paramname="location">the location</param>
        Public Sub New(ByVal [date] As Date, ByVal location As IGeoLocation)
            MyBase.New([date], location)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="ZmanimCalendar"/> class.
        ''' </summary>
        ''' <paramname="dateWithLocation">The date with location.</param>
        Public Sub New(ByVal dateWithLocation As IDateWithLocation)
            MyBase.New(dateWithLocation)
        End Sub

        ''' <summary>
        ''' Returns <em>tzais</em> (nightfall) when the sun is 8.5° below the
        ''' western geometric horizon (90°) after <seealsocref="AstronomicalCalendar.GetSunset">sunset</seealso>. For
        ''' information on the source of this calculation see
        ''' <seealsocref="ZENITH_8_POINT_5"/>.
        ''' </summary>
        ''' <returns>
        ''' The <code>DateTime</code> of nightfall.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="ZENITH_8_POINT_5"/>
        Public Overridable Function GetTzais() As DateTime?
            Return MyBase.GetSunsetOffsetByDegrees(ZENITH_8_POINT_5)
        End Function

        ''' <summary>
        '''  Returns <em>alos</em> (dawn) based on the time when the sun is 16.1°
        '''  below the eastern <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric horizon</see> before
        '''  <seecref="AstronomicalCalendar.GetSunrise">sunrise</see>. For more information the source of 16.1°
        '''  see <seecref="ZENITH_16_POINT_1"/>.
        ''' </summary>
        ''' <seealsocref="ZENITH_16_POINT_1"/>
        ''' <returns>
        '''  The <c>DateTime</c> of dawn.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetAlosHashachar() As DateTime?
            Return MyBase.GetSunriseOffsetByDegrees(ZENITH_16_POINT_1)
        End Function

        ''' <summary>
        '''  Method to return <em>alos</em> (dawn) calculated using 72 minutes before
        '''  <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> (no adjustment for
        '''  elevation) based on the time to walk the distance of 4 <em>Mil</em> at 18
        '''  minutes a <em>Mil</em>. This is based on the opinion of most
        '''  <em>Rishonim</em> who stated that the time of the <em>Neshef</em> (time
        '''  between dawn and sunrise) does not vary by the time of year or location
        '''  but purely depends on the time it takes to walk the distance of 4
        '''  <em>Mil</em>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> representing the time.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetAlos72() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunrise().Value, -72 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        '''  This method returns <em>chatzos</em> (midday) following the opinion of
        '''  the GRA that the day for Jewish halachic times start at
        '''  <seecref="AstronomicalCalendar.GetSunrise">sunrise</see> and ends at <seecref="AstronomicalCalendar.GetSunset">sunset</see>. The
        '''  returned value is identical to <seecref="AstronomicalCalendar.GetSunTransit"/>
        ''' </summary>
        ''' <seealsocref="AstronomicalCalendar.GetSunTransit"/>
        ''' <returns> the <c>DateTime</c> of chatzos.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        '''  </returns>
        Public Overridable Function GetChatzos() As DateTime?
            Return MyBase.GetSunTransit()
        End Function

        ''' <summary>
        ''' A method that returns "solar" midnight, or the time when the sun is at its 
        ''' <ahref="http://en.wikipedia.org/wiki/Nadir">nadir</a>. <br/>
        ''' <br/>
        ''' <b>Note:</b> this method is experimental and might be removed.
        ''' </summary>
        ''' <returns> the <code>Date</code> of Solar Midnight (chatzos layla). If the calculation can't be computed such as in
        '''         the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        '''         does not set, a null will be returned. See detailed explanation on top of the
        '''         <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetSolarMidnight() As DateTime?
            Dim sunset As DateTime? = MyBase.GetSeaLevelSunset()
            Dim clonedCal As ZmanimCalendar = CType(MemberwiseClone(), ZmanimCalendar)
            clonedCal.DateWithLocation = New DateWithLocation(MyBase.DateWithLocation.Date.AddDays(1), MyBase.DateWithLocation.Location)
            Dim sunrise As DateTime? = clonedCal.GetSeaLevelSunrise()
            Return MyBase.GetTimeOffset(sunset, MyBase.GetTemporalHour(sunset, sunrise) * 6)
        End Function

        ''' <summary>
        ''' This is a generic method for calculating the latest <em>zman krias shema</em> (time to recite Shema in the
        ''' morning) based on the start and end of day passed to the method. The time from the start of day to the end of day
        ''' are divided into 12 shaos zmaniyos (temporal hours), and <em>zman krias shema</em> is calculated as 3 shaos
        ''' zmaniyos from the beginning of the day. As an example, passing <seealsocref="GetSeaLevelSunrise">sea level sunrise</seealso>
        ''' and <seealsocref="GetSeaLevelSunset">sea level sunset</seealso> to this method will return <em>zman krias shema</em> according to
        ''' the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        ''' </summary>
        ''' <paramname="startOfDay">
        '''            the start of day for calculating <em>zman krias shema</em>. This can be sunrise or any alos passed to
        '''            this method. </param>
        ''' <paramname="endOfDay">
        '''            the start of day for calculating <em>zman krias shema</em>. This can be sunset or any tzais passed to
        '''            this method. </param>
        ''' <returns> the <code>DateTime</code> of the latest zman shema based on the start and end of day times passed to this
        '''         method. If the calculation can't be computed such as in the Arctic Circle where there is at least one day
        '''         a year where the sun does not rise, and one where it does not set, a null will be returned. See detailed
        '''         explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetSofZmanShma(ByVal startOfDay As DateTime?, ByVal endOfDay As DateTime?) As DateTime?
            Dim shaahZmanis = MyBase.GetTemporalHour(startOfDay, endOfDay)
            Return MyBase.GetTimeOffset(startOfDay, shaahZmanis * 3)
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to recite Shema in the morning). This time is 3
        ''' <em><seealsocref="GetShaahZmanisGra">shaos zmaniyos</seealso></em> (solar hours) after <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level
        ''' sunrise</see> based on the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em> that the day is calculated from
        ''' sunrise to sunset. This returns the time 3 * <seealsocref="GetShaahZmanisGra"/> after <seecref="AstronomicalCalendar.GetSeaLevelSunrise"> sea
        ''' level sunrise</see>.
        ''' </summary>
        ''' <seealsocref="GetSofZmanShma"/>
        ''' <seealsocref="GetShaahZmanisGra"/>
        ''' <returns> the <code>DateTime</code> of the latest zman shema according to the GRA and Baal Hatanya. If the calculation
        '''         can't be computed such as in the Arctic Circle where there is at least one day a year where the sun does
        '''         not rise, and one where it does not set, a null will be returned. See detailed explanation on top of the
        '''         <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetSofZmanShmaGRA() As DateTime?
            Return GetSofZmanShma(MyBase.GetSeaLevelSunrise(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman krias shema</em> (time to recite shema in the morning) in the opinion of
        ''' the <em>MGA</em> based on <em>alos</em> being 72 minutes before <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 3
        ''' <em>shaos zmaniyos</em> (solar hours) after dawn based on the opinion of the <em>MGA</em> that the day is
        ''' calculated from a dawn of 72 minutes before sunrise to nightfall of 72 minutes after sunset. This returns the
        ''' time of 3 * <em>shaos zmaniyos</em> after dawn.
        ''' </summary>
        ''' <returns> the <code>DateTime</code> of the latest <em>zman shema</em>. If the calculation can't be computed such as in
        '''         the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        '''         does not set, a null will be returned. See detailed explanation on top of the
        '''         <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        ''' <seealsocref="GetSofZmanShma"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetShaahZmanis72Minutes"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetAlos72"/>
        ''' <seealsocref="ComplexZmanimCalendar.GetSofZmanShmaMGA72Minutes"/>
        Public Overridable Function GetSofZmanShmaMGA() As DateTime?
            Return GetSofZmanShma(GetAlos72(), GetTzais72())
        End Function

        ''' <summary>
        '''  This method returns the <em>tzais</em> (nightfall) based on the opinion
        '''  of the <em>Ramba"m</em> and <em>Rabainu Tam</em> that <em>tzais</em> is
        '''  calculated as the time it takes to walk 4 <em>Mil</em> at 18 minutes a
        '''  <em>Mil</em> for a total of 72 minutes. Even for locations above sea
        '''  level, this is calculated at sea level, since the darkness level is not
        '''  affected by elevation.
        ''' </summary>
        ''' <returns> the <c>DateTime</c> representing 72 minutes after sea level sunset.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        '''  </returns>
        Public Overridable Function GetTzais72() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset().Value, 72 * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' A method to return candle lighting time. This is calculated as
        ''' <seealsocref="CandleLightingOffset"/> minutes before sunset. This will
        ''' return the time for any day of the week, since it can be
        ''' used to calculate candle lighting time for <em>yom tov</em>
        ''' (mid-week holidays) as well. To calculate the offset
        ''' of non-sea level sunset, pass the elevation adjusted sunset to <seealsocref="AstronomicalCalendar.GetTimeOffset(System.Nullable(OfSystem.DateTime),Long)"/>.
        ''' </summary>
        ''' <returns>
        ''' candle lighting time.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="CandleLightingOffset"/>
        Public Overridable Function GetCandleLighting() As DateTime?
            Return MyBase.GetTimeOffset(MyBase.GetSeaLevelSunset().Value, -CandleLightingOffset * MINUTE_MILLIS)
        End Function

        ''' <summary>
        ''' This is a generic method for calculating the latest <em>zman tefilah<em> (time to recite the morning prayers)
        ''' based on the start and end of day passed to the method. The time from the start of day to the end of day
        ''' are divided into 12 shaos zmaniyos (temporal hours), and <em>zman krias shema</em> is calculated as 4 shaos
        ''' zmaniyos from the beginning of the day. As an example, passing <seealsocref="GetSeaLevelSunrise()"/>
        ''' and <seealsocref="GetSeaLevelSunset"/> to this method will return <em>zman tefilah<em> according to
        ''' the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        ''' </summary>
        ''' <paramname="startOfDay">
        '''            the start of day for calculating <em>zman tefilah<em>. This can be sunrise or any alos passed to
        '''            this method. </param>
        ''' <paramname="endOfDay">
        '''            the start of day for calculating <em>zman tefilah<em>. This can be sunset or any tzais passed to this
        '''            method. </param>
        ''' <returns> the <code>Date</code> of the latest <em>zman tefilah<em> based on the start and end of day times passed
        '''         to this method. If the calculation can't be computed such as in the Arctic Circle where there is at least
        '''         one day a year where the sun does not rise, and one where it does not set, a null will be returned. See
        '''         detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation. </returns></em></returns></em></param></em></param></em></em></em></summary>  
        Public Overridable Function GetSofZmanTfila(ByVal startOfDay As DateTime?, ByVal endOfDay As DateTime?) As DateTime?
            Dim shaahZmanis = MyBase.GetTemporalHour(startOfDay, endOfDay)
            Return MyBase.GetTimeOffset(startOfDay, shaahZmanis * 4)
        End Function

        ''' <summary>
        ''' This method returns the latest
        ''' <em>zman tefilah<em> (time to pray morning prayers). This time is 4
        ''' hours into the day based on the opinion of the <em>GR"A</em> and the
        ''' </em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        ''' This returns the time 4 * <seealsocref="GetShaahZmanisGra"/> after
        ''' <seealsocref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman tefilah.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanisGra"/>
        Public Overridable Function GetSofZmanTfilaGRA() As DateTime?
            Return GetSofZmanTfila(MyBase.GetSeaLevelSunrise(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' This method returns the latest <em>zman tfila</em> (time to say the
        ''' morning prayers) in the opinion of the <em>MG"A</em> based on
        ''' <em>alos</em> being <seealsocref="GetAlos72">72</seealso> minutes before
        ''' <seealsocref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 4
        ''' <em><seealsocref="GetShaahZmanisMGA">shaos zmaniyos</seealso></em> (temporal hours)
        ''' after <seealsocref="GetAlos72">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ''' that the day is calculated from a <seealsocref="GetAlos72">dawn</seealso> of 72 minutes
        ''' before sunrise to <seealsocref="GetTzais72">nightfall</seealso> of 72 minutes after
        ''' sunset. This returns the time of 4 * <seealsocref="GetShaahZmanisMGA"/> after
        ''' <seealsocref="GetAlos72">dawn</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the latest zman tfila.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanisMGA"/>
        ''' <seealsocref="GetAlos72"/>
        Public Overridable Function GetSofZmanTfilaMGA() As DateTime?
            Return MyBase.GetTimeOffset(GetAlos72().Value, GetShaahZmanisMGA() * 4)
        End Function

        ''' <summary>
        ''' This is a generic method for calulating <em>mincha gedola</em>. <em>Mincha gedola</em> is the earliest time one
        ''' can pray mincha (6.5 hours from the begining of the day), based on the start and end of day passed to the method.
        ''' The time from the start of day to the end of day are divided into 12 shaos zmaniyos, and <em>Mincha gedola</em>
        ''' is calculated as 6.5 hours from the beginning of the day. As an example, passing {@link #getSeaLevelSunrise() sea
        ''' level sunrise} and <seealsocref=""/> to this method will return <em>Mincha gedola</em>
        ''' according to the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        ''' </summary>
        ''' <paramname="startOfDay">
        '''            the start of day for calculating <em>Mincha gedola</em>. This can be sunrise or any alos passed to
        '''            this method. </param>
        ''' <paramname="endOfDay">
        '''            the start of day for calculating <em>Mincha gedola</em>. This can be sunrise or any alos passed to
        '''            this method. </param>
        ''' <returns> the <code>Date</code> of the time of <em>Mincha gedola</em> based on the start and end of day times
        '''         passed to this method. If the calculation can't be computed such as in the Arctic Circle where there is
        '''         at least one day a year where the sun does not rise, and one where it does not set, a null will be
        '''         returned. See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetMinchaGedola(ByVal startOfDay As DateTime?, ByVal endOfDay As DateTime?) As DateTime?
            Dim shaahZmanis = MyBase.GetTemporalHour(startOfDay, endOfDay)
            Return MyBase.GetTimeOffset(startOfDay, shaahZmanis * 6.5)
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha gedola</em>.
        ''' <em>Mincha gedola</em> is the earliest time one can pray mincha. The
        ''' Ramba"m is of the opinion that it is better to delay <em>mincha</em>
        ''' until <em><seealsocref="GetMinchaKetana">mincha ketana</seealso></em> while the
        ''' <em>Ra"sh,
        ''' Tur, GR"A</em> and others are of the opinion that <em>mincha</em> can be
        ''' prayed <em>lechatchila</em> starting at <em>mincha gedola</em>. This is
        ''' calculated as 6.5 <seealsocref="GetShaahZmanisGra">sea level solar hours</seealso>
        ''' after <seealsocref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>. This calculation
        ''' is calculated based on the opinion of the <em>GR"A</em> and the
        ''' <em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        ''' This returns the time 6.5 <seealsocref="GetShaahZmanisGra"/> after
        ''' <seealsocref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha gedola.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanisGra"/>
        ''' <seealsocref="GetMinchaKetana"/>
        Public Overridable Function GetMinchaGedola() As DateTime?
            Return GetMinchaGedola(MyBase.GetSeaLevelSunrise(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' This is a generic method for calulating <em>mincha ketana</em>. <em>Mincha ketana</em> is the preferred time one
        ''' can pray can pray <em>mincha</em> in the opinion of the Rambam and others (9.5 hours from the begining of the
        ''' day), based on the start and end of day passed to the method. The time from the start of day to the end of day
        ''' are divided into 12 shaos zmaniyos, and <em>mincha ketana</em> is calculated as 9.5 hours from the beginning of
        ''' the day. As an example, passing <seealsocref=""/> and {@link #getSeaLevelSunset sea
        ''' level sunset} to this method will return <em>Mincha ketana</em> according to the opinion of the <em>GRA</em> and
        ''' the <em>Baal Hatanya</em>.
        ''' </summary>
        ''' <paramname="startOfDay">
        '''            the start of day for calculating <em>Mincha ketana</em>. This can be sunrise or any alos passed to
        '''            this method. </param>
        ''' <paramname="endOfDay">
        '''            the start of day for calculating <em>Mincha ketana</em>. This can be sunrise or any alos passed to
        '''            this method. </param>
        ''' <returns> the <code>Date</code> of the time of <em>Mincha ketana</em> based on the start and end of day times
        '''         passed to this method. If the calculation can't be computed such as in the Arctic Circle where there is
        '''         at least one day a year where the sun does not rise, and one where it does not set, a null will be
        '''         returned. See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetMinchaKetana(ByVal startOfDay As DateTime?, ByVal endOfDay As DateTime?) As DateTime?
            Dim shaahZmanis = MyBase.GetTemporalHour(startOfDay, endOfDay)
            Return MyBase.GetTimeOffset(startOfDay, shaahZmanis * 9.5)
        End Function

        ''' <summary>
        ''' This method returns the time of <em>mincha ketana</em>. This is the
        ''' perfered earliest time to pray <em>mincha</em> in the opinion of the
        ''' Ramba"m and others. For more information on this see the documentation on
        ''' <em><seealsocref="GetMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ''' 9.5 <seealsocref="GetShaahZmanisGra">sea level solar hours</seealso> after
        ''' <seealsocref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>. This calculation is
        ''' calculated based on the opinion of the <em>GR"A</em> and the
        ''' <em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        ''' This returns the time 9.5 * <seealsocref="GetShaahZmanisGra"/> after
        ''' <seealsocref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of mincha gedola.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="GetShaahZmanisGra"/>
        ''' <seealsocref="GetMinchaGedola"/>
        Public Overridable Function GetMinchaKetana() As DateTime?
            Return GetMinchaKetana(MyBase.GetSeaLevelSunrise(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' This is a generic method for calulating <em>plag hamincha</em> (1.25 hours before the end of the day) based on
        ''' the start and end of day passed to the method. The time from the start of day to the end of day are divided into
        ''' 12 shaos zmaniyos, and plag is calculated as 10.75 hours from the beginning of the day. As an example, passing
        ''' <seealsocref=""/> and <seealsocref=""/> to this method
        ''' will return Plag Hamincha according to the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        ''' </summary>
        ''' <paramname="startOfDay">
        '''            the start of day for calculating plag. This can be sunrise or any alos passed to this method. </param>
        ''' <paramname="endOfDay">
        '''            the start of day for calculating plag. This can be sunrise or any alos passed to this method. </param>
        ''' <returns> the <code>Date</code> of the time of <em>plag hamincha</em> based on the start and end of day times
        '''         passed to this method. If the calculation can't be computed such as in the Arctic Circle where there is
        '''         at least one day a year where the sun does not rise, and one where it does not set, a null will be
        '''         returned. See detailed explanation on top of the <seealsocref="AstronomicalCalendar"/> documentation. </returns>
        Public Overridable Function GetPlagHamincha(ByVal startOfDay As DateTime?, ByVal endOfDay As DateTime?) As DateTime?
            Dim shaahZmanis = MyBase.GetTemporalHour(startOfDay, endOfDay)
            Return MyBase.GetTimeOffset(startOfDay, shaahZmanis * 10.75)
        End Function

        ''' <summary>
        ''' This method returns he time of <em>plag hamincha</em>. This is calculated
        ''' as 10.75 hours after sunrise. This calculation is calculated based on the
        ''' opinion of the <em>GR"A</em> and the <em>Baal Hatanya</em> that the day
        ''' is calculated from sunrise to sunset. This returns the time 10.75 *
        ''' <seecref="GetShaahZmanisGra"/> after <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see>.
        ''' </summary>
        ''' <returns>
        ''' the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        ''' If the calculation can't be computed such as northern and southern locations
        ''' even south of the Arctic Circle and north of the Antarctic Circle
        ''' where the sun may not reach low enough below the horizon for this calculation,
        ''' a null will be returned. See detailed explanation on top of the
        ''' <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetPlagHamincha() As DateTime?
            Return GetPlagHamincha(MyBase.GetSeaLevelSunrise(), MyBase.GetSeaLevelSunset())
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (
        ''' <seecref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">temporal hour</see>) according to the
        ''' opinion of the <em>GR"A</em> and the <em>Baal Hatanya</em>. This
        ''' calculation divides the day based on the opinion of the <em>GR"A</em> and
        ''' the <em>Baal Hatanya</em> that the day runs from <seecref="AstronomicalCalendar.GetSunrise"> sunrise</see>
        ''' to <seealsocref="AstronomicalCalendar.GetSunset">sunset</seealso>. The calculations are based on a
        ''' day from <seecref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> to
        ''' <seecref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. The day is split into 12
        ''' equal parts each part with each one being a <em>shaah zmanis</em>. This
        ''' method is similar to <seecref="AstronomicalCalendar.GetTemporalHour()"/>, but all calculations are
        ''' based on a sealevel sunrise and sunset. For additional information, see
        ''' Zmanim Kehilchasam, 2nd Edition by Rabbi Dovid Yehuda Burstein,
        ''' Jerusalem, 2007.
        ''' </summary>
        ''' <returns>
        ''' the <code>long</code> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set,
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation on
        ''' top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        ''' <seealsocref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)"/>
        Public Overridable Function GetShaahZmanisGra() As Long
            Return MyBase.GetTemporalHour(MyBase.GetSeaLevelSunrise().Value, MyBase.GetSeaLevelSunset().Value)
        End Function

        ''' <summary>
        ''' Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ''' opinion of the MGA. This calculation divides the day based on the opinion
        ''' of the <em>MGA</em> that the day runs from dawn to dusk (for sof zman
        ''' krias shema and tfila). Dawn for this calculation is 72 minutes before
        ''' sunrise and dusk is 72 minutes after sunset. This day is split into 12
        ''' equal parts with each part being a <em>shaah zmanis</em>. Alternate
        ''' mothods of calculating a <em>shaah zmanis</em> are available in the
        ''' subclass <seecref="ComplexZmanimCalendar"/>.
        ''' </summary>
        ''' <returns>
        ''' the <code>long</code> millisecond length of a <em>shaah zmanis</em>.
        ''' If the calculation can't be computed such
        ''' as in the Arctic Circle where there is at least one day a year
        ''' where the sun does not rise, and one where it does not set,
        ''' <seecref="Long.MinValue"/> will be returned. See detailed explanation on
        ''' top of the <seecref="AstronomicalCalendar"/> documentation.
        ''' </returns>
        Public Overridable Function GetShaahZmanisMGA() As Long
            Return MyBase.GetTemporalHour(GetAlos72().Value, GetTzais72().Value)
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
            If Me Is obj Then
                Return True
            End If

            If Not (TypeOf obj Is ZmanimCalendar) Then
                Return False
            End If

            Dim zCal = CType(obj, ZmanimCalendar)
            ' return getCalendar().ToMillisecondsFromEpoch().equals(zCal.getCalendar().ToMillisecondsFromEpoch())
            Return MyBase.DateWithLocation.Equals(zCal.DateWithLocation) AndAlso MyBase.DateWithLocation.Location.Equals(zCal.DateWithLocation.Location) AndAlso MyBase.AstronomicalCalculator.Equals(zCal.AstronomicalCalculator)
        End Function

        ''' <summary>
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        ''' </returns>
        Public Overrides Function GetHashCode() As Integer
            Dim result = 17
            result = 37 * result + [GetType]().GetHashCode() ' needed or this and
            ' subclasses will
            ' return identical hash
            result += 37 * result + MyBase.DateWithLocation.GetHashCode()
            result += 37 * result + MyBase.DateWithLocation.Location.GetHashCode()
            result += 37 * result + MyBase.AstronomicalCalculator.GetHashCode()
            Return result
        End Function

        ''' <summary>
        '''  A method to get the offset in minutes before
        '''  <seecref="AstronomicalCalendar.GetSunset">sunset</see> that is used in
        '''  calculating candle lighting time. The default time used is 18 minutes
        '''  before sunset. Some calendars use 15 minutes, while the custom in
        '''  Jerusalem is to use a 40 minute offset. Please check the local custom for
        '''  Candle lighting time.
        ''' </summary>
        ''' <value> Returns the candle lighting offset to set in minutes.. </value>
        ''' <seealsocref="GetCandleLighting"/>
        Public Overridable Property CandleLightingOffset As Double
            Get
                Return candleLightingOffsetField
            End Get
            Set(ByVal value As Double)
                candleLightingOffsetField = value
            End Set
        End Property
    End Class
End Namespace