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
Namespace Zmanim.JewishCalendar
    Public Class HebrewDateFormatter
        Private hebrewFormatField As Boolean = False
        'Nykedit added
        Private UseModernHolidaysField As Boolean = False
        Private UseLonghebrewYearsField As Boolean = False
        Private UseGershGershayimField As Boolean = True
        Private longWeekFormatField As Boolean = True
        'Nykedit 0 should be empty string
        'Nykedit SpecialShabbos
        Private HebrewParshaList As String() = {"", "בראשית", "נח", "לך לך", "וירא", "חיי שרה", "תולדות", "ויצא", "וישלח", "וישב", "מקץ", "ויגש", "ויחי", "שמות", "וארא", "בא", "בשלח", "יתרו", "משפטים", "תרומה", "תצוה", "כי תשא", "ויקהל", "פקודי", "ויקרא", "צו", "שמיני", "תזריע", "מצרע", "אחרי מות", "קדושים", "אמור", "בהר", "בחקתי", "במדבר", "נשא", "בהעלתך", "שלח לך", "קרח", "חוקת", "בלק", "פינחס", "מטות", "מסעי", "דברים", "ואתחנן", "עקב", "ראה", "שופטים", "כי תצא", "כי תבוא", "ניצבים", "וילך", "האזינו", "ויקהל פקודי", "תזריע מצרע", "אחרי מות קדושים", "בהר בחקתי", "חוקת בלק", "מטות מסעי", "ניצבים וילך", "שקלים", "זכור", "פרה", "החודש"}
        Private TransliteratedParshaList As String() = {"", "Bereshis", "Noach", "Lech Lecha", "Vayera", "Chayei Sara", "Toldos", "Vayetzei", "Vayishlach", "Vayeshev", "Miketz", "Vayigash", "Vayechi", "Shemos", "Vaera", "Bo", "Beshalach", "Yisro", "Mishpatim", "Terumah", "Tetzaveh", "Ki Sisa", "Vayakhel", "Pekudei", "Vayikra", "Tzav", "Shmini", "Tazria", "Metzora", "Achrei Mos", "Kedoshim", "Emor", "Behar", "Bechukosai", "Bamidbar", "Nasso", "Beha'aloscha", "Sh'lach", "Korach", "Chukas", "Balak", "Pinchas", "Matos", "Masei", "Devarim", "Vaeschanan", "Eikev", "Re'eh", "Shoftim", "Ki Seitzei", "Ki Savo", "Nitzavim", "Vayeilech", "Ha'Azinu", "Vayakhel Pekudei", "Tazria Metzora", "Achrei Mos Kedoshim", "Behar Bechukosai", "Chukas Balak", "Matos Masei", "Nitzavim Vayeilech", "Shkalim", "Zachor", "Para", "Hachodesh"}

        Private hebrewDaysOfWeek As String() = {"ראשון", "שני", "שלישי", "רביעי", "חמישי", "ששי", "שבת"}
        Private transliteratedHolidays As String() = {"Erev Pesach", "Pesach", "Chol Hamoed Pesach", "Pesach Sheni", "Erev Shavuos", "Shavuos", "Seventeenth of Tammuz", "Tishah B'Av", "Tu B'Av", "Erev Rosh Hashana", "Rosh Hashana", "Fast of Gedalyah", "Erev Yom Kippur", "Yom Kippur", "Erev Succos", "Succos", "Chol Hamoed Succos", "Hoshana Rabbah", "Shemini Atzeres", "Simchas Torah", "Erev Chanukah", "Chanukah", "Tenth of Teves", "Tu B'Shvat", "Fast of Esther", "Purim", "Shushan Purim", "Purim Katan", "Rosh Chodesh", "Yom HaShoah", "Yom Hazikaron", "Yom Ha'atzmaut", "Yom Yerushalayim"}
        'nykedit שושן פורים
        Private hebrewHolidays As String() = {"ערב פסח", "פסח", "חול המועד פסח", "פסח שני", "ערב שבועות", "שבועות", "שבעה עשר בתמוז", "תשעה באב", "ט״ו באב", "ערב ראש השנה", "ראש השנה", "צום גדליה", "ערב יום כיפור", "יום כיפור", "ערב סוכות", "סוכות", "חול המועד סוכות", "הושענא רבה", "שמיני עצרת", "שמחת תורה", "ערב חנוכה", "חנוכה", "עשרה בטבת", "ט״ו בשבט", "תענית אסתר", "פורים", "שושן פורים", "פורים קטן", "ראש חודש", "יום השואה", "יום הזיכרון", "יום העצמאות", "יום ירושלים"}
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


            If holiday = jewishCalendar.JewishHoliday.CHANUKAH Then
                Dim dayOfChanukah As Integer = jewishCalendar.GetDayOfChanukah(dt)
                Return If(HebrewFormat, (FormatHebrewNumber(dayOfChanukah) & " ד" & hebrewHolidays(index)), (transliteratedHolidays(index) & " " & dayOfChanukah))
            End If

            Return If(index = -1, "", If(HebrewFormat, hebrewHolidays(index), transliteratedHolidays(index)))
        End Function
        'nykedit
        Public Overridable Function FormatParsha(ByVal dt As DateTime, ByVal inIsrael As Boolean) As String
            Dim Parshah = jewishCalendar.GetParshah(dt, inIsrael)
            Try
                Return If(HebrewFormat, HebrewParshaList(Parshah), TransliteratedParshaList(Parshah))
            Catch ex As Exception
                Return ""
            End Try
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

                If month < jewishCalendar.JewishMonth.ADAR OrElse month = jewishCalendar.JewishMonth.ADAR AndAlso isLeapYear Then
                    month += 1
                Else
                    month = jewishCalendar.JewishMonth.NISSAN
                End If
            End If

            Dim updatedDateTime As DateTime = jewishCalendar.GetJewishDateTime(year, month, dayOfMonth)
            formattedRoshChodesh = If(HebrewFormat, hebrewHolidays(CInt(jewishCalendar.JewishHoliday.ROSH_CHODESH)), transliteratedHolidays(CInt(jewishCalendar.JewishHoliday.ROSH_CHODESH)))
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

                If isLeapYear AndAlso month = jewishCalendar.JewishMonth.ADAR Then
                    Return hebrewMonthsField(13) & (If(UseGershGershayim, GERESH, ""))
                ElseIf isLeapYear AndAlso month = jewishCalendar.JewishMonth.ADAR_II Then
                    Return hebrewMonthsField(12) & (If(UseGershGershayim, GERESH, ""))
                Else
                    Return hebrewMonthsField(CInt(month) - 1)
                End If
            Else

                If isLeapYear AndAlso month = jewishCalendar.JewishMonth.ADAR Then
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
            Dim dt As DateTime = jewishCalendar.GetJewishDateTime(jewishYear, jewishCalendar.JewishMonth.TISHREI, 1)
            Dim yearType As JewishCalendar.JewishYearType = jewishCalendar.GetJewishYearType(dt)
            Dim roshHashanaDayOfweek As Integer = jewishCalendar.GetJewishDayOfWeek(dt)
            Dim returnValue As String = FormatHebrewNumber(roshHashanaDayOfweek)
            returnValue += (If(yearType = jewishCalendar.JewishYearType.CHASERIM, "ח", If(yearType = jewishCalendar.JewishYearType.SHELAIMIM, "ש", "כ")))
            dt = jewishCalendar.GetJewishDateTime(jewishYear, jewishCalendar.JewishMonth.NISSAN, 15)
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

        'NykEdit not sure but this did not work looks like an issue with port to vb
        'changed to Use Function GetNumtoHebrewLetter 
        Public Overridable Function FormatHebrewNumber(ByVal number As Integer) As String
            If number < 0 Then
                Throw New System.ArgumentException("negative numbers can't be formatted")
            ElseIf number > 9999 Then
                Throw New System.ArgumentException("numbers > 9999 can't be formatted")
            End If

            Return GetNumtoHebrewLetter(number, 0, UseGershGershayim, False)
            'will not continue for here

            Dim ALAFIM As String = "אלפים"
            Dim EFES As String = "אפס"
            Dim jHundreds As String() = New String() {"", "ק", "ר", "ש", "ת", "תק", "תר", "תש", "תת", "תתק"}
            Dim jTens As String() = New String() {"", "י", "כ", "ל", "מ", "נ", "ס", "ע", "פ", "צ"}
            Dim jTenEnds As String() = New String() {"", "י", "ך", "ל", "ם", "ן", "ס", "ע", "ף", "ץ"}
            Dim tavTaz As String() = New String() {"טו", "טז"}
            Dim jOnes As String() = New String() {"", "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט"}

            If number = 0 Then
                Return EFES
            End If

            Dim shortNumber As Integer = number Mod 1000
            Dim singleDigitNumber As Boolean = (shortNumber < 11 OrElse (shortNumber < 100 AndAlso shortNumber Mod 10 = 0) OrElse (shortNumber <= 400 AndAlso shortNumber Mod 100 = 0))
            Dim thousands As Integer = number / 1000
            Dim sb As StringBuilder = New StringBuilder()

            If number Mod 1000 = 0 Then
                sb.Append(jOnes(thousands))

                If UseGershGershayim Then
                    sb.Append(GERESH)
                End If

                sb.Append(" ")
                sb.Append(ALAFIM)
                Return sb.ToString()
            ElseIf UseLongHebrewYears AndAlso number >= 1000 Then
                sb.Append(jOnes(thousands))

                If UseGershGershayim Then
                    sb.Append(GERESH)
                End If

                sb.Append(" ")
            End If

            number = number Mod 1000
            Dim hundreds As Integer = number / 100
            sb.Append(jHundreds(hundreds))
            number = number Mod 100

            If number = 15 Then
                sb.Append(tavTaz(0))
            ElseIf number = 16 Then
                sb.Append(tavTaz(1))
            Else
                Dim tens As Integer = number / 10

                If number Mod 10 = 0 Then

                    If singleDigitNumber = False Then
                        sb.Append(jTenEnds(tens))
                    Else
                        sb.Append(jTens(tens))
                    End If
                Else
                    sb.Append(jTens(tens))
                    number = number Mod 10
                    sb.Append(jOnes(number))
                End If
            End If

            If UseGershGershayim Then

                If singleDigitNumber = True Then
                    sb.Append(GERESH)
                Else
                    sb.Insert(sb.Length - 1, GERSHAYIM)
                End If
            End If

            Return sb.ToString()
        End Function

        'Nykedit new not in C# port
        Public Overridable Function FormatSpecialParsha(ByVal dt As DateTime) As String
            Dim SpecialParsha = jewishCalendar.getSpecialShabbos(dt.Date)
            If SpecialParsha = jewishCalendar.Parsha.NONE Then Return ""
            Try
                '-1 as the is a empty string at 0
                Return If(HebrewFormat, HebrewParshaList(SpecialParsha - 1), TransliteratedParshaList(SpecialParsha - 1))
            Catch ex As Exception
                Return ""
            End Try
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

    End Class

End Namespace