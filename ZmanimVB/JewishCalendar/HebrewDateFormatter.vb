'port from https://github.com/Yitzchok/Zmanim/blob/master/src/Zmanim/JewishCalendar/HebrewDateFormatter.cs
'// * Zmanim .NET API
'// * Copyright (C) 2004-2010 Eliyahu Hershfeld
'// *
'// * Converted to C# by AdminJew
'// *
'// * This file Is part of Zmanim .NET API.
'// *
'// * Zmanim .NET API Is free software: you can redistribute it And/Or modify
'// * it under the terms of the GNU Lesser General Public License as published by
'// * the Free Software Foundation, either version 3 of the License, Or
'// * (at your option) any later version.
'// *
'// * Zmanim .NET API Is distributed in the hope that it will be Useful,
'// * but WITHOUT ANY WARRANTY; without even the implied warranty of
'// * MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'// * GNU Lesser General Public License for more details.
'// *
'// * You should have received a copy of the GNU Lesser General Public License
'// * along with Zmanim.NET API.  If Not, see <http://www.gnu.org/licenses/lgpl.html>.
Imports System.Text
Imports System
Imports System.Diagnostics
Imports System.Collections.Generic
Namespace Zmanim.JewishCalendar
    Public Class HebrewDateFormatter
        Private hebrewFormatField As Boolean = False
        'Nykedit added
        Private UseModernHolidaysField As Boolean = False
        Private UseLonghebrewYearsField As Boolean = False
        Private UseGershGershayimField As Boolean = True
        Private longWeekFormatField As Boolean = True
        Private hebrewDaysOfWeek As String() = {"ראשון", "שני", "שלישי", "רביעי", "חמישי", "ששי", "שבת"}
        'made first empty that will be none add Shvii Shel Pesach and more 
        Private transliteratedHolidays As String() = {"", "Erev Pesach", "Pesach", "Chol Hamoed Pesach", "Shvii Shel Pesach", "Acharon shel Pesach", "Pesach Sheni", "Erev Shavuos", "Shavuos", "Seventeenth of Tammuz", "Tishah B'Av", "Tu B'Av", "Erev Rosh Hashana", "Rosh Hashana", "Fast of Gedalyah", "Erev Yom Kippur", "Yom Kippur", "Erev Succos", "Succos", "Chol Hamoed Succos", "Hoshana Rabbah", "Shemini Atzeres", "Simchas Torah", "Erev Chanukah", "Chanukah", "Tenth of Teves", "Tu B'Shvat", "Fast of Esther", "Purim", "Shushan Purim", "Purim Katan", "Rosh Chodesh", "Yom HaShoah", "Yom Hazikaron", "Yom Ha'atzmaut", "Yom Yerushalayim"}
        Private hebrewHolidays As String() = {"", "ערב פסח", "פסח", "חול המועד פסח", "שביעי של פסח", "אחרון של פסח", "פסח שני", "ערב שבועות", "שבועות", "שבעה עשר בתמוז", "תשעה באב", "ט״ו באב", "ערב ראש השנה", "ראש השנה", "צום גדליה", "ערב יום כיפור", "יום כיפור", "ערב סוכות", "סוכות", "חול המועד סוכות", "הושענא רבה", "שמיני עצרת", "שמחת תורה", "ערב חנוכה", "חנוכה", "עשרה בטבת", "ט״ו בשבט", "תענית אסתר", "פורים", "שושן פורים", "פורים קטן", "ראש חודש", "יום השואה", "יום הזיכרון", "יום העצמאות", "יום ירושלים"}
        Private Const GERESH As String = "׳"
        Private Const GERSHAYIM As String = "״"
        Private transliteratedMonthsField As String() = {"Nissan", "Iyar", "Sivan", "Tammuz", "Av", "Elul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shevat", "Adar", "Adar II", "Adar I"}
        Private hebrewOmerPrefixField As String = "ב"
        Private transliteratedShabbosDayOfweekField As String = "Shabbos"
        Private Shared ReadOnly hebrewMonthsField As String() = {"ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר ב", "אדר א"}


        Private Shared jewishCalendar As JewishCalendar = New JewishCalendar()

        Public Overridable Property LongWeekFormat As Boolean
            Get
                Return longWeekFormatField
            End Get
            Set(ByVal value As Boolean)
                longWeekFormatField = value
            End Set
        End Property

        Public Overridable Property TransliteratedShabbosDayOfWeek As String
            Get
                Return transliteratedShabbosDayOfweekField
            End Get
            Set(ByVal value As String)
                transliteratedShabbosDayOfweekField = value
            End Set
        End Property

        Public Overridable Property TransliteratedHolidayList As String()
            Get
                Return transliteratedHolidays
            End Get
            Set(ByVal value As String())
                Me.transliteratedHolidays = value
            End Set
        End Property
        'nykedit add space to eng day of chanukah and ד
        Public Overridable Function FormatYomTov(ByVal dt As DateTime, ByVal inIsrael As Boolean) As String
            'NYkedit add  UseModernHolidaysField
            Dim holiday As JewishCalendar.JewishHoliday = jewishCalendar.GetJewishHoliday(dt, inIsrael, UseModernHolidays)
            Dim index As Integer = CInt(holiday)


            If holiday = JewishCalendar.JewishHoliday.CHANUKAH Then
                Dim dayOfChanukah As Integer = jewishCalendar.GetDayOfChanukah(dt)
                Return If(HebrewFormat, (FormatHebrewNumber(dayOfChanukah) & " ד" & hebrewHolidays(index)), (transliteratedHolidays(index) & " " & dayOfChanukah))
            End If

            Return If(index = -1, "", If(HebrewFormat, hebrewHolidays(index), transliteratedHolidays(index)))
        End Function

        Public Overridable Function FormatRoshChodesh(ByVal dt As DateTime) As String
            If Not jewishCalendar.IsRoshChodesh(dt) Then
                Return ""
            End If

            Dim formattedRoshChodesh As String = ""
            Dim dayOfMonth As Integer = jewishCalendar.GetDayOfMonth(dt)
            Dim month As JewishCalendar.JewishMonth = jewishCalendar.GetJewishMonth(dt)
            Dim year As Integer = jewishCalendar.GetYear(dt)
            Dim isLeapYear As Boolean = jewishCalendar.IsLeapYearFromDateTime(dt)

            If dayOfMonth = 30 Then

                If month < JewishCalendar.JewishMonth.ADAR OrElse month = JewishCalendar.JewishMonth.ADAR AndAlso isLeapYear Then
                    month += 1
                Else
                    month = JewishCalendar.JewishMonth.NISSAN
                End If
            End If

            Dim updatedDateTime As DateTime = jewishCalendar.GetJewishDateTime(year, month, dayOfMonth)
            formattedRoshChodesh = If(HebrewFormat, hebrewHolidays(CInt(JewishCalendar.JewishHoliday.ROSH_CHODESH)), transliteratedHolidays(CInt(JewishCalendar.JewishHoliday.ROSH_CHODESH)))
            formattedRoshChodesh += " " & FormatMonth(updatedDateTime)
            Return formattedRoshChodesh
        End Function

        Public Overridable Property HebrewFormat As Boolean
            Get
                Return hebrewFormatField
            End Get
            Set(ByVal value As Boolean)
                hebrewFormatField = value
            End Set
        End Property
        'nykedit add
        Public Overridable Property UseModernHolidays As Boolean
            Get
                Return UseModernHolidaysField
            End Get
            Set(ByVal value As Boolean)
                UseModernHolidaysField = value
            End Set
        End Property
        Public Overridable Property HebrewOmerPrefix As String
            Get
                Return hebrewOmerPrefixField
            End Get
            Set(ByVal value As String)
                Me.hebrewOmerPrefixField = value
            End Set
        End Property

        Public Overridable Property TransliteratedMonthList As String()
            Get
                Return transliteratedMonthsField
            End Get
            Set(ByVal value As String())
                transliteratedMonthsField = value
            End Set
        End Property

        Public Overridable Function FormatDayOfWeek(ByVal dt As DateTime) As String
            Dim dayOfWeek As Integer = jewishCalendar.GetJewishDayOfWeek(dt)

            If HebrewFormat Then
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(If(LongWeekFormat, hebrewDaysOfWeek(dayOfWeek - 1), FormatHebrewNumber(dayOfWeek)))
                Return sb.ToString()
            Else
                Return If(dayOfWeek = 7, TransliteratedShabbosDayOfWeek, dt.ToString("dddd"))
            End If
        End Function

        Public Overridable Property UseGershGershayim As Boolean
            Get
                Return UseGershGershayimField
            End Get
            Set(ByVal value As Boolean)
                UseGershGershayimField = value
            End Set
        End Property

        Public Overridable Property UseLongHebrewYears As Boolean
            Get
                Return UseLonghebrewYearsField
            End Get
            Set(ByVal value As Boolean)
                UseLonghebrewYearsField = value
            End Set
        End Property
        'nykedit - changed to Use FormatDayOfMonth and FormatYear
        Public Overridable Function Format(ByVal dt As DateTime) As String
            Return FormatDayOfMonth(dt) & " " & FormatMonth(dt) & " " & FormatYear(dt)
            'If HebrewFormat Then
            '    Return FormatHebrewNumber(jewishCalendar.GetDayOfMonth(dt)) & " " & FormatMonth(dt) & If(IncludeYear, " " & FormatHebrewNumber(jewishCalendar.GetYear(dt)), "")
            'Else
            '    Return jewishCalendar.GetDayOfMonth(dt) & " " & FormatMonth(dt) & If(IncludeYear, ", " + jewishCalendar.GetYear(dt), "")
            'End If
        End Function
        'nykedit
        Public Overridable Function FormatDayOfMonth(ByVal dt As DateTime) As String
            If HebrewFormat Then
                Return FormatHebrewNumber(jewishCalendar.GetDayOfMonth(dt))
            Else
                Return jewishCalendar.GetDayOfMonth(dt)
            End If
        End Function
        Public Overridable Function FormatYear(ByVal dt As DateTime) As String
            If HebrewFormat Then
                Return FormatHebrewNumber(jewishCalendar.GetYear(dt))
            Else
                Return jewishCalendar.GetYear(dt)
            End If
        End Function
        'nykedit added optin for date or month and year
        Public Overridable Function FormatMonth(ByVal dt As DateTime) As String
            Dim month As JewishCalendar.JewishMonth = jewishCalendar.GetJewishMonth(dt)
            Dim Year As JewishCalendar.JewishMonth = jewishCalendar.GetYear(dt)
            Return FormatMonth(Year, month)
        End Function
        Public Overridable Function FormatMonth(ByVal Year As Integer, ByVal month As Integer) As String
            Dim isLeapYear As Boolean = jewishCalendar.IsLeapYear(Year)
            'Dim isLeapYear As Boolean = jewishCalendar.IsLeapYearFromDateTime(dt)

            If HebrewFormat Then

                If isLeapYear AndAlso month = JewishCalendar.JewishMonth.ADAR Then
                    Return hebrewMonthsField(13) & (If(UseGershGershayim, GERESH, ""))
                ElseIf isLeapYear AndAlso month = JewishCalendar.JewishMonth.ADAR_II Then
                    Return hebrewMonthsField(12) & (If(UseGershGershayim, GERESH, ""))
                Else
                    Return hebrewMonthsField(CInt(month) - 1)
                End If
            Else

                If isLeapYear AndAlso month = JewishCalendar.JewishMonth.ADAR Then
                    Return transliteratedMonthsField(13)
                Else
                    Return transliteratedMonthsField(CInt(month) - 1)
                End If
            End If
        End Function

        Public Overridable Function FormatOmer(ByVal dt As DateTime) As String
            Dim omer As Integer = jewishCalendar.GetDayOfOmer(dt)

            If omer = -1 Then
                Return ""
            End If

            If HebrewFormat Then
                Return FormatHebrewNumber(omer) & " " & HebrewOmerPrefix & "עומר"
            Else

                If omer = 33 Then
                    Return "Lag BaOmer"
                Else
                    Return "Omer " & omer
                End If
            End If
        End Function

        Private Function FormatMolad(ByVal moladChalakim As Long) As String
            Dim adjustedChalakim As Long = moladChalakim
            Dim MINUTE_CHALAKIM As Integer = 18
            Dim HOUR_CHALAKIM As Integer = 1080
            Dim DAY_CHALAKIM As Integer = 24 * HOUR_CHALAKIM
            Dim days As Long = adjustedChalakim / DAY_CHALAKIM
            adjustedChalakim = adjustedChalakim - (days * DAY_CHALAKIM)
            Dim hours As Integer = CInt(((adjustedChalakim / HOUR_CHALAKIM)))

            If hours >= 6 Then
                days += 1
            End If

            adjustedChalakim = adjustedChalakim - (hours * HOUR_CHALAKIM)
            Dim minutes As Integer = CInt((adjustedChalakim / MINUTE_CHALAKIM))
            adjustedChalakim = adjustedChalakim - minutes * MINUTE_CHALAKIM
            Return "Day: " & days Mod 7 & " hours: " & hours & ", minutes " & minutes & ", chalakim: " & adjustedChalakim
        End Function

        Public Overridable Function GetFormattedKviah(ByVal jewishYear As Integer) As String
            Dim dt As DateTime = jewishCalendar.GetJewishDateTime(jewishYear, JewishCalendar.JewishMonth.TISHREI, 1)
            Dim yearType As JewishCalendar.JewishYearType = jewishCalendar.GetJewishYearType(dt)
            Dim roshHashanaDayOfweek As Integer = jewishCalendar.GetJewishDayOfWeek(dt)
            Dim returnValue As String = FormatHebrewNumber(roshHashanaDayOfweek)
            returnValue += (If(yearType = JewishCalendar.JewishYearType.CHASERIM, "ח", If(yearType = JewishCalendar.JewishYearType.SHELAIMIM, "ש", "כ")))
            dt = jewishCalendar.GetJewishDateTime(jewishYear, JewishCalendar.JewishMonth.NISSAN, 15)
            Dim pesachDayOfweek As Integer = jewishCalendar.GetJewishDayOfWeek(dt)
            returnValue += FormatHebrewNumber(pesachDayOfweek)
            returnValue = returnValue.Replace(GERESH, "")
            Return returnValue
        End Function

        Public Overridable Function FormatDafYomiBavli(ByVal daf As Daf) As String
            If HebrewFormat Then
                Return daf.Masechta & " " & FormatHebrewNumber(daf.Page)
            Else
                Return daf.MasechtaTransliterated & " " + daf.Page
            End If
        End Function


        Public Overridable Function FormatHebrewNumber(ByVal number As Integer) As String
            If number < 0 Then
                Throw New System.ArgumentException("negative numbers can't be formatted")
            ElseIf number > 9999 Then
                Throw New System.ArgumentException("numbers > 9999 can't be formatted")
            End If

            'the was a issue with the port from C# to vb
            'changed to Use Function GetNumtoHebrewLetter 
            Return GetNumtoHebrewLetter(number, 0, UseGershGershayim, False)
        End Function

        'new func for TalUmatar
        Public Function FormatTalUmatarStarts(ByVal dt As DateTime, ByVal InIsrael As Boolean) As String
            Dim istoday = jewishCalendar.GetTalUmatarStartsToday(dt, InIsrael)
            Dim wasyesterday = jewishCalendar.GetTalUmatarStartsToday(dt.AddDays(-1), InIsrael)

            If istoday = True Then
                'move from Erav Shabbos
                If dt.DayOfWeek = DayOfWeek.Friday Then Return ""
                GoTo Yes
            End If
            'yesterday was Erav Shabbos
            If wasyesterday = True And dt.DayOfWeek = DayOfWeek.Saturday Then GoTo Yes

            Return ""
Yes:
            If HebrewFormat = True Then
                Return "טל ומטר בערבית"
            Else
                Return "Tal Umatar To Maariv"
            End If
        End Function
        'new func for BirkasHachamah
        Public Function FormatBirkasHachamah(ByVal dt As DateTime) As String
            If jewishCalendar.GetBirkasHachamah(dt) = True Then
                If HebrewFormat = True Then
                    Return "ברכת החמה"
                Else
                    Return "Birkas Hachamah"
                End If
            End If
            Return ""
        End Function

        'nykedit
        ''' <summary>
        ''' Gets a string where each number in <paramrefname="number"/> is replaced with the
        ''' Hebrew Letter representation.
        ''' </summary>
        ''' <paramname="intNum">The number to convert.</param>
        ''' <paramname="intIncludeThousands">Include Thousands. 0 for false 1 for true 2 for apostrophe after Thousand num</param>
        ''' <paramname="IncludeQuotes">Include Quotes.</param>
        ''' <paramname="blnGoodNumbers">Convert to Good Numbers.</param>
        ''' <returns>
        ''' A <seecref="String"/> Hebrew Letter representation of the <paramrefname="number"/>.
        ''' </returns>
        Public Function GetNumtoHebrewLetter(ByVal intNum As Integer, Optional intIncludeThousands As Integer = 0, Optional blnIncludeQuotes As Boolean = True, Optional blnGoodNumbers As Boolean = True) As String
            Dim strTemp As String
            Dim strThousands As String
            Dim Digit As Integer
            strTemp = ""

            If intNum >= 1000 Then
                strThousands = GetNumtoHebrewLetter(intNum \ 1000, 0, False) 'dont get apostrophe will be set belwo
                intNum = intNum Mod 1000
                If intIncludeThousands = 2 Then strThousands &= "'"    'apostrophe after alef
            End If

            If intNum >= 100 Then
                Digit = intNum \ 100
                Select Case Digit
                    Case 1 : strTemp &= "ק"
                    Case 2 : strTemp &= "ר"
                    Case 3 : strTemp &= "ש"
                    Case 4 : strTemp &= "ת"
                    Case 5 : strTemp &= "תק"
                    Case 6 : strTemp &= "תר"
                    Case 7 : strTemp &= "תש"
                    Case 8 : strTemp &= "תת"
                    Case 9 : strTemp &= "תתק"
                End Select
            End If

            If intNum >= 10 Then
                Digit = (intNum Mod 100) \ 10
                Select Case Digit
                    Case 1 : strTemp &= "י"
                    Case 2 : strTemp &= "כ"
                    Case 3 : strTemp &= "ל"
                    Case 4 : strTemp &= "מ"
                    Case 5 : strTemp &= "נ"
                    Case 6 : strTemp &= "ס"
                    Case 7 : strTemp &= "ע"
                    Case 8 : strTemp &= "פ"
                    Case 9 : strTemp &= "צ"
                End Select
            End If

            Digit = (intNum Mod 10)
            Select Case Digit
                Case 1 : strTemp &= "א"
                Case 2 : strTemp &= "ב"
                Case 3 : strTemp &= "ג"
                Case 4 : strTemp &= "ד"
                Case 5 : strTemp &= "ה"
                Case 6 : strTemp &= "ו"
                Case 7 : strTemp &= "ז"
                Case 8 : strTemp &= "ח"
                Case 9 : strTemp &= "ט"
            End Select

            strTemp = strTemp.Replace("יה", "טו")
            strTemp = strTemp.Replace("יו", "טז")

            If blnGoodNumbers Then
                strTemp = strTemp.Replace("רצח", "רחצ")
                strTemp = strTemp.Replace("רע", "ער")
                strTemp = strTemp.Replace("רעה", "ערה")
                strTemp = strTemp.Replace("שד", "דש")
                strTemp = strTemp.Replace("שמד", "דשמ")
            End If

            If blnIncludeQuotes Then
                If strTemp.Length >= 2 Then
                    strTemp = strTemp.Substring(0, strTemp.Length - 1) & """" & strTemp.Substring(strTemp.Length - 1, 1)
                    'strTemp = Mid(strTemp, 1, Len(strTemp) - 1) & """" & Mid(strTemp, Len(strTemp), 1)
                End If
                If strTemp.Length = 1 Then strTemp &= "'"
            End If

            If intIncludeThousands > 0 And intIncludeThousands < 3 Then strTemp = strThousands & strTemp

            Return strTemp
        End Function

        'Nyk new not in java

        'nykedit Parsha Functions were having issues replaced completely   
        Public Function FormatParsha(ByVal DateIn As Date, inIsrael As Boolean) As String
            Return FormatParsha(DateIn, inIsrael, HebrewFormat)
        End Function
        Public Function FormatParsha(ByVal DateIn As Date, inIsrael As Boolean, ByVal HebrewNames As Boolean) As String
            'move to sab - needed for week of R"H 
            If DateIn.DayOfWeek <> DayOfWeek.Saturday Then DateIn = DateIn.AddDays(Add_till_sab(DateIn.DayOfWeek))

            Dim jewishYear = jewishCalendar.GetYear(DateIn)
            Dim roshHashana As DateTime = jewishCalendar.GetJewishDateTime(jewishYear, JewishCalendar.JewishMonth.TISHREI, 1)
            Dim roshHashanaDayOfWeek As Integer = jewishCalendar.GetJewishDayOfWeek(roshHashana)
            Dim kvia As String = jewishCalendar.GetJewishYearType(roshHashana).ToString
            Dim IsLeapYear As Boolean = jewishCalendar.IsLeapYear(jewishYear)

            Dim daysSinceRoshHashana As TimeSpan = DateIn - roshHashana
            Dim day As Integer = roshHashanaDayOfWeek + daysSinceRoshHashana.Days
            Dim week As Integer = Math.Ceiling(day / 7)
            Dim array As Integer() = Nothing
            Dim index As Integer


            Dim Mon_short As Integer() = {51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}
            Dim Mon_long As Integer() = {51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, -1, 34, 35, 36, 37, 57, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}

            Dim Mon_short_leap As Integer() = {51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, -1, 34, 35, 36, 37, 57, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}
            Dim Mon_short_leap_Israel As Integer() = {51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}
            Dim Mon_long_leap As Integer() = {51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50}
            Dim Mon_long_leap_Israel As Integer() = {51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50}

            Dim Thu_normal As Integer() = {52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50}
            Dim Thu_normal_Israel As Integer() = {52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50}
            Dim Thu_long As Integer() = {52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50}

            Dim Thu_short_leap As Integer() = {52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, -1, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50}
            Dim Thu_long_leap As Integer() = {52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, -1, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 59}

            Dim Sat_short As Integer() = {-1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50}
            Dim Sat_long As Integer() = {-1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}

            Dim Sat_short_leap As Integer() = {-1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}
            Dim Sat_long_leap As Integer() = {-1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, -1, 34, 35, 36, 37, 57, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59}

            Dim hebrewParshiyos As String() = {"בראשית", "נח", "לך לך", "וירא", "חיי שרה", "תולדות", "ויצא", "וישלח", "וישב", "מקץ", "ויגש", "ויחי", "שמות", "וארא", "בא", "בשלח", "יתרו", "משפטים", "תרומה", "תצוה", "כי תשא", "ויקהל", "פקודי", "ויקרא", "צו", "שמיני", "תזריע", "מצרע", "אחרי מות", "קדושים", "אמור", "בהר", "בחקתי", "במדבר", "נשא", "בהעלתך", "שלח לך", "קרח", "חוקת", "בלק", "פינחס", "מטות", "מסעי", "דברים", "ואתחנן", "עקב", "ראה", "שופטים", "כי תצא", "כי תבוא", "ניצבים", "וילך", "האזינו", "ויקהל פקודי", "תזריע מצרע", "אחרי מות קדושים", "בהר בחקתי", "חוקת בלק", "מטות מסעי", "ניצבים וילך"}
            Dim EngParshiyos As String() = {"Bereshis", "Noach", "Lech Lecha", "Vayera", "Chayei Sara", "Toldos", "Vayetzei", "Vayishlach", "Vayeshev", "Miketz", "Vayigash", "Vayechi", "Shemos", "Vaera", "Bo", "Beshalach", "Yisro", "Mishpatim", "Terumah", "Tetzaveh", "Ki Sisa", "Vayakhel", "Pekudei", "Vayikra", "Tzav", "Shmini", "Tazria", "Metzora", "Achrei Mos", "Kedoshim", "Emor", "Behar", "Bechukosai", "Bamidbar", "Nasso", "Beha'aloscha", "Sh'lach", "Korach", "Chukas", "Balak", "Pinchas", "Matos", "Masei", "Devarim", "Vaeschanan", "Eikev", "Re'eh", "Shoftim", "Ki Seitzei", "Ki Savo", "Nitzavim", "Vayeilech", "Ha'Azinu", "Vayakhel Pekudei", "Tazria Metzora", "Achrei Mos Kedoshim", "Behar Bechukosai", "Chukas Balak", "Matos Masei", "Nitzavim Vayeilech"}

            If IsLeapYear = False Then

                Select Case roshHashanaDayOfWeek
                    Case 7
                        If kvia = "CHASERIM" Then
                            array = Sat_short
                        ElseIf kvia = "SHELAIMIM" Then
                            array = Sat_long
                        End If

                    Case 2

                        If kvia = "CHASERIM" Then
                            array = Mon_short
                        ElseIf kvia = "SHELAIMIM" Then
                            array = If(inIsrael, Mon_short, Mon_long)
                        End If

                    Case 3

                        If kvia = "KESIDRAN" Then
                            array = If(inIsrael, Mon_short, Mon_long)
                        End If

                    Case 5

                        If kvia = "KESIDRAN" Then
                            array = If(inIsrael, Thu_normal_Israel, Thu_normal)
                        ElseIf kvia = "SHELAIMIM" Then
                            array = Thu_long
                        End If
                End Select

            Else

                Select Case roshHashanaDayOfWeek
                    Case 7

                        If kvia = "CHASERIM" Then
                            array = Sat_short_leap
                        ElseIf kvia = "SHELAIMIM" Then
                            array = If(inIsrael, Sat_short_leap, Sat_long_leap)
                        End If

                    Case 2

                        If kvia = "CHASERIM" Then
                            array = If(inIsrael, Mon_short_leap_Israel, Mon_short_leap)
                        ElseIf kvia = "SHELAIMIM" Then
                            array = If(inIsrael, Mon_long_leap_Israel, Mon_long_leap)
                        End If

                    Case 3

                        If kvia = "KESIDRAN" Then
                            array = If(inIsrael, Mon_long_leap_Israel, Mon_long_leap)
                        End If

                    Case 5

                        If kvia = "CHASERIM" Then
                            array = Thu_short_leap
                        ElseIf kvia = "SHELAIMIM" Then
                            array = Thu_long_leap
                        End If
                End Select
            End If

            If array Is Nothing Then
                Return ""
            Else
                index = array(week - 1)
                If index = -1 Then
                    Return ""
                End If

                Return If(HebrewNames, hebrewParshiyos(index), EngParshiyos(index))
            End If
        End Function
        Private Function Add_till_sab(ByVal day As DayOfWeek) As Integer
            ' monday is 1
            If (day = DayOfWeek.Sunday) Then Return 6
            If (day = DayOfWeek.Monday) Then Return 5
            If (day = DayOfWeek.Tuesday) Then Return 4
            If (day = DayOfWeek.Wednesday) Then Return 3
            If (day = DayOfWeek.Thursday) Then Return 2
            If (day = DayOfWeek.Friday) Then Return 1

            Return 0
        End Function

        'Nykedit not in C# port rework to jave
        Public Overridable Function FormatSpecialParsha(ByVal dt As DateTime) As String
            Return FormatSpecialParsha(dt, HebrewFormat)
        End Function
        Public Overridable Function FormatSpecialParsha(ByVal dt As DateTime, ByVal HebrewNames As Boolean) As String

            If dt.DayOfWeek = DayOfWeek.Saturday Then

                If FormatParsha(dt, False, True) = "בשלח" Then Return If(HebrewNames, "שירה", "Shirah")

                If jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.TISHREI AndAlso jewishCalendar.GetDayOfMonth(dt) > 2 AndAlso jewishCalendar.GetDayOfMonth(dt) < 10 Then _
                       Return If(HebrewNames, "שובה", "Shuva")


                If jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.SHEVAT AndAlso Not jewishCalendar.IsLeapYearFromDateTime(dt) _
                        OrElse jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.ADAR AndAlso jewishCalendar.IsLeapYearFromDateTime(dt) Then
                    If jewishCalendar.GetDayOfMonth(dt) = 25 OrElse jewishCalendar.GetDayOfMonth(dt) = 27 OrElse jewishCalendar.GetDayOfMonth(dt) = 29 Then
                        Return If(HebrewNames, "שקלים", "Shkalim")
                    End If
                End If

                If jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.ADAR AndAlso Not jewishCalendar.IsLeapYearFromDateTime(dt) _
                    OrElse jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.ADAR_II Then
                    If jewishCalendar.GetDayOfMonth(dt) = 1 Then Return If(HebrewNames, "שקלים", "Shkalim")

                    If jewishCalendar.GetDayOfMonth(dt) = 8 OrElse jewishCalendar.GetDayOfMonth(dt) = 9 _
                            OrElse jewishCalendar.GetDayOfMonth(dt) = 11 OrElse jewishCalendar.GetDayOfMonth(dt) = 13 Then Return If(HebrewNames, "זכור", "Zachor")

                    If jewishCalendar.GetDayOfMonth(dt) = 18 OrElse jewishCalendar.GetDayOfMonth(dt) = 20 _
                            OrElse jewishCalendar.GetDayOfMonth(dt) = 22 OrElse jewishCalendar.GetDayOfMonth(dt) = 23 Then Return If(HebrewNames, "פרה", "Para")

                    If jewishCalendar.GetDayOfMonth(dt) = 25 OrElse jewishCalendar.GetDayOfMonth(dt) = 27 _
                            OrElse jewishCalendar.GetDayOfMonth(dt) = 29 Then Return If(HebrewNames, "החודש", "Hachodesh")
                End If

                If jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.NISSAN Then
                    If jewishCalendar.GetDayOfMonth(dt) = 1 Then Return If(HebrewNames, "החודש", "Hachodesh")
                    'this will move it back a week when sab is ErevYomTov
                    'If GetDayOfMonth(dt) >= 7 AndAlso GetDayOfMonth(dt) < 14 Then Return Parsha.HAGADOL
                    If jewishCalendar.GetDayOfMonth(dt) >= 8 AndAlso jewishCalendar.GetDayOfMonth(dt) < 15 Then Return If(HebrewNames, "הגדול", "HaGadol")
                End If

                If jewishCalendar.GetJewishMonth(dt) = JewishCalendar.JewishMonth.AV Then
                    If jewishCalendar.GetDayOfMonth(dt) >= 3 AndAlso jewishCalendar.GetDayOfMonth(dt) < 10 Then Return If(HebrewNames, "חזון", "Chazon")
                    If jewishCalendar.GetDayOfMonth(dt) >= 10 AndAlso jewishCalendar.GetDayOfMonth(dt) < 17 Then Return If(HebrewNames, "נחמו", "Nachamu")
                End If

            End If
            Return ""

        End Function

        Public Class HolidayandParsha
            Public Property HebName As String
            Public Property EngName As String
            Public Property EngDate As DateTime
            Public Sub New(ByVal HebName As String, ByVal EngName As String, ByVal EngDate As DateTime)
                'MyBase.New
                Me.HebName = HebName
                Me.EngName = EngName
                Me.EngDate = EngDate
            End Sub
        End Class
        Public Function GetJewishHolidayList(ByVal Year As Integer) As List(Of HolidayandParsha)
            Dim HolidayList As List(Of HolidayandParsha) = New List(Of HolidayandParsha)
            Dim IsLeap = jewishCalendar.IsLeapYear(Year)

            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.ROSH_HASHANA), transliteratedHolidays(JewishCalendar.JewishHoliday.ROSH_HASHANA), New DateTime(Year, 1, 1, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.FAST_OF_GEDALYAH), transliteratedHolidays(JewishCalendar.JewishHoliday.FAST_OF_GEDALYAH), New DateTime(Year, 1, 3, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_KIPPUR), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_KIPPUR), New DateTime(Year, 1, 10, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SUCCOS), transliteratedHolidays(JewishCalendar.JewishHoliday.SUCCOS), New DateTime(Year, 1, 15, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.HOSHANA_RABBA), transliteratedHolidays(JewishCalendar.JewishHoliday.HOSHANA_RABBA), New DateTime(Year, 1, 21, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SHEMINI_ATZERES), transliteratedHolidays(JewishCalendar.JewishHoliday.SHEMINI_ATZERES), New DateTime(Year, 1, 22, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SIMCHAS_TORAH), transliteratedHolidays(JewishCalendar.JewishHoliday.SIMCHAS_TORAH), New DateTime(Year, 1, 23, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.CHANUKAH), transliteratedHolidays(JewishCalendar.JewishHoliday.CHANUKAH), New DateTime(Year, 3, 25, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.TENTH_OF_TEVES), transliteratedHolidays(JewishCalendar.JewishHoliday.TENTH_OF_TEVES), New DateTime(Year, 4, 10, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.TU_BESHVAT), transliteratedHolidays(JewishCalendar.JewishHoliday.TU_BESHVAT), New DateTime(Year, 5, 15, jewishCalendar)))
            If IsLeap Then _
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.PURIM_KATAN), transliteratedHolidays(JewishCalendar.JewishHoliday.PURIM_KATAN), New DateTime(Year, 6, 14, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.PURIM), transliteratedHolidays(JewishCalendar.JewishHoliday.PURIM), New DateTime(Year, If(IsLeap, 7, 6), 14, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.PESACH), transliteratedHolidays(JewishCalendar.JewishHoliday.PURIM), New DateTime(Year, If(IsLeap, 8, 7), 15, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SHVII_SHEL_PESACH), transliteratedHolidays(JewishCalendar.JewishHoliday.SHVII_SHEL_PESACH), New DateTime(Year, If(IsLeap, 8, 7), 21, jewishCalendar)))

            If UseModernHolidays = True Then
                'YOM_HASHOAH
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HASHOAH), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HASHOAH), New DateTime(Year, If(IsLeap, 8, 7), 27, jewishCalendar)))
                If New DateTime(Year, If(IsLeap, 8, 7), 27, jewishCalendar).DayOfWeek = DayOfWeek.Friday Then _
                    HolidayList(HolidayList.Count - 1).EngDate = New DateTime(Year, If(IsLeap, 8, 7), 26, jewishCalendar)
                If New DateTime(Year, If(IsLeap, 8, 7), 27, jewishCalendar).DayOfWeek = DayOfWeek.Friday Then _
                    HolidayList(HolidayList.Count - 1).EngDate = New DateTime(Year, If(IsLeap, 8, 7), 28, jewishCalendar)
                'YOM_HAZIKARON - YOM_HAATZMAUT
                Dim dt As DateTime = New DateTime(Year, If(IsLeap, 9, 8), 4, jewishCalendar)
                If dt.DayOfWeek = DayOfWeek.Tuesday Then
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), New DateTime(Year, If(IsLeap, 9, 8), 4, jewishCalendar)))
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), New DateTime(Year, If(IsLeap, 9, 8), 5, jewishCalendar)))
                End If
                If dt.DayOfWeek = DayOfWeek.Sunday Then
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), New DateTime(Year, If(IsLeap, 9, 8), 5, jewishCalendar)))
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), New DateTime(Year, If(IsLeap, 9, 8), 6, jewishCalendar)))
                End If
                If dt.DayOfWeek = DayOfWeek.Thursday Then
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), New DateTime(Year, If(IsLeap, 9, 8), 3, jewishCalendar)))
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), New DateTime(Year, If(IsLeap, 9, 8), 4, jewishCalendar)))
                End If
                If dt.DayOfWeek = DayOfWeek.Friday Then
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAZIKARON), New DateTime(Year, If(IsLeap, 9, 8), 2, jewishCalendar)))
                    HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_HAATZMAUT), New DateTime(Year, If(IsLeap, 9, 8), 3, jewishCalendar)))
                End If
            End If

            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.PESACH_SHENI), transliteratedHolidays(JewishCalendar.JewishHoliday.PESACH_SHENI), New DateTime(Year, If(IsLeap, 9, 8), 15, jewishCalendar)))
            HolidayList.Add(New HolidayandParsha("לג בעומר", "Lag BaOmer", New DateTime(Year, If(IsLeap, 9, 8), 18, jewishCalendar)))

            If UseModernHolidays = True Then
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.YOM_YERUSHALAYIM), transliteratedHolidays(JewishCalendar.JewishHoliday.YOM_YERUSHALAYIM), New DateTime(Year, If(IsLeap, 9, 8), 28, jewishCalendar)))
            End If

            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SHAVUOS), transliteratedHolidays(JewishCalendar.JewishHoliday.SHAVUOS), New DateTime(Year, If(IsLeap, 10, 9), 6, jewishCalendar)))

            If New DateTime(Year, If(IsLeap, 11, 10), 17, jewishCalendar).DayOfWeek <> DayOfWeek.Saturday Then
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SEVENTEEN_OF_TAMMUZ), transliteratedHolidays(JewishCalendar.JewishHoliday.SEVENTEEN_OF_TAMMUZ), New DateTime(Year, If(IsLeap, 11, 10), 17, jewishCalendar)))
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.TISHA_BEAV), transliteratedHolidays(JewishCalendar.JewishHoliday.TISHA_BEAV), New DateTime(Year, If(IsLeap, 12, 11), 9, jewishCalendar)))
            Else
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.SEVENTEEN_OF_TAMMUZ), transliteratedHolidays(JewishCalendar.JewishHoliday.SEVENTEEN_OF_TAMMUZ), New DateTime(Year, If(IsLeap, 11, 10), 18, jewishCalendar)))
                HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.TISHA_BEAV), transliteratedHolidays(JewishCalendar.JewishHoliday.TISHA_BEAV), New DateTime(Year, If(IsLeap, 12, 11), 10, jewishCalendar)))
            End If
            HolidayList.Add(New HolidayandParsha(hebrewHolidays(JewishCalendar.JewishHoliday.TU_BEAV), transliteratedHolidays(JewishCalendar.JewishHoliday.TU_BEAV), New DateTime(Year, If(IsLeap, 12, 11), 15, jewishCalendar)))
            Return HolidayList
        End Function
        Public Function GetParshaList(ByVal Year As Integer, inIsrael As Boolean) As List(Of HolidayandParsha)
            Dim ParshaList As List(Of HolidayandParsha) = New List(Of HolidayandParsha)
            Dim IsLeap = jewishCalendar.IsLeapYear(Year)
            Dim dt As DateTime = New DateTime(Year, 1, 1, jewishCalendar)
            Dim DateEnd As DateTime = New DateTime(Year, If(IsLeap, 13, 12), 29, jewishCalendar)

            'For Each Day In jewishCalendar
            Do While (dt <= DateEnd)
                If dt.DayOfWeek = DayOfWeek.Saturday Then
                    Dim HebParshah = FormatParsha(dt, inIsrael, True)
                    Dim EngParshah = FormatParsha(dt, inIsrael, False)
                    Dim HebSpecialParshah = FormatSpecialParsha(dt, True)
                    Dim EngSpecialParshah = FormatSpecialParsha(dt, False)
                    If HebParshah <> "" Then
                        If HebSpecialParshah <> "" Then
                            ParshaList.Add(New HolidayandParsha(HebParshah & " - " & HebSpecialParshah, EngParshah & " - " & EngSpecialParshah, dt))
                        Else
                            ParshaList.Add(New HolidayandParsha(HebParshah, EngParshah, dt))
                        End If
                    End If
                End If

                dt = dt.AddDays(1)
            Loop
            Return ParshaList

        End Function
    End Class


End Namespace