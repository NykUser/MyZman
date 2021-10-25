' * Zmanim .NET API
' * Copyright (C) 2004-2011 Eliyahu Hershfeld
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

Namespace Zmanim.JewishCalendar

    ''' <summary>
    ''' This class calculates the Daf Yomi page (daf) for a given date. The class currently only supports Daf Yomi Bavli, but
    ''' may cover Yerushalmi, Mishna Yomis etc in the future.
    ''' 
    ''' @author &copy; Bob Newell (original C code)
    ''' @author &copy; Eliyahu Hershfeld 2011
    ''' @version 0.0.1
    ''' </summary>
    Public Class YomiCalculator
        Private Shared dafYomiStartDate As Date = New DateTime(1923, 9, 11)
        Private Shared dafYomiJulianStartDay As Integer = GetJulianDay(dafYomiStartDate)
        Private Shared shekalimChangeDate As Date = New DateTime(1975, 6, 24)
        Private Shared shekalimJulianChangeDay As Integer = GetJulianDay(shekalimChangeDate)

        ''' <summary>
        ''' Returns the <ahref="http://en.wikipedia.org/wiki/Daf_yomi">Daf Yomi</a> <ahref="http://en.wikipedia.org/wiki/Talmud">Bavli</a> <seealsocref="Daf"/> for a given date. The first Daf Yomi cycle
        ''' started on Rosh Hashana 5684 (September 11, 1923) and calculations prior to this date will result in an
        ''' IllegalArgumentException thrown. For historical calculations (supported by this method), it is important to note
        ''' that a change in length of the cycle was instituted starting in the eighth Daf Yomi cycle beginning on June 24,
        ''' 1975. The Daf Yomi Bavli cycle has a single masechta of the Talmud Yerushalmi - Shekalim as part of the cycle.
        ''' Unlike the Bavli where the number of daf per masechta was standardized since the original <ahref="http://en.wikipedia.org/wiki/Daniel_Bomberg">Bomberg Edition</a> published from 1520 - 1523, there is no
        ''' uniform page length in the Yerushalmi. The early cycles had the Yerushalmi Shekalim length of 13 days following
        ''' the <ahref="http://en.wikipedia.org/wiki/Zhytomyr">Zhytomyr</a> Shas used by <ahref="http://en.wikipedia.org/wiki/Meir_Shapiro">Rabbi Meir Shapiro</a>. With the start of the eighth Daf Yomi
        ''' cycle beginning on June 24, 1975 the length of the Yerushalmi shekalim was changed from 13 to 22 daf to follow
        ''' the Vilna Shas that is in common use today.
        ''' </summary>
        ''' <paramname="calendar">
        '''            the calendar date for calculation </param>
        ''' <returns> the <seealsocref="Daf"/>.
        ''' </returns>
        ''' <exceptioncref="IllegalArgumentException">
        '''             if the date is prior to the September 11, 1923 start date of the first Daf Yomi cycle </exception>
        Public Shared Function GetDafYomiBavli(ByVal [date] As Date) As Daf
            '' 
            ' * The number of daf per masechta. Since the number of blatt in Shekalim changed on the 8th Daf Yomi cycle
            ' * beginning on June 24, 1975 from 13 to 22, the actual calculation for blattPerMasechta[4] will later be
            ' * adjusted based on the cycle.

            Dim blattPerMasechta = {64, 157, 105, 121, 22, 88, 56, 40, 35, 31, 32, 29, 27, 122, 112, 91, 66, 49, 90, 82, 119, 119, 176, 113, 24, 49, 76, 14, 120, 110, 142, 61, 34, 34, 28, 22, 4, 9, 5, 73}
            Dim dafYomi As Daf = Nothing
            Dim julianDay = GetJulianDay([date])
            Dim cycleNo = 0
            Dim dafNo = 0

            If [date] < dafYomiStartDate Then
                ' TODO: should we return a null or throw an IllegalArgumentException?
                Throw New ArgumentException([date] & " is prior to organized Daf Yomi Bavli cycles that started on " & dafYomiStartDate)
            End If

            If [date].Equals(shekalimChangeDate) OrElse [date] > shekalimChangeDate Then
                cycleNo = 8 + (julianDay - shekalimJulianChangeDay) / 2711
                dafNo = (julianDay - shekalimJulianChangeDay) Mod 2711
            Else
                cycleNo = 1 + (julianDay - dafYomiJulianStartDay) / 2702
                dafNo = (julianDay - dafYomiJulianStartDay) Mod 2702
            End If

            Dim total = 0
            Dim masechta = -1
            Dim blatt = 0

            ' Fix Shekalim for old cycles. 
            If cycleNo <= 7 Then
                blattPerMasechta(4) = 13
            Else
                blattPerMasechta(4) = 22 ' correct any change that may have been changed from a prior calculation
            End If
            ' Finally find the daf. 
            For j = 0 To blattPerMasechta.Length - 1
                masechta += 1
                total = total + blattPerMasechta(j) - 1

                If dafNo < total Then
                    blatt = 1 + blattPerMasechta(j) - (total - dafNo)
                    ' Fiddle with the weird ones near the end. 
                    If masechta = 36 Then
                        blatt += 21
                    ElseIf masechta = 37 Then
                        blatt += 24
                    ElseIf masechta = 38 Then
                        blatt += 32
                    End If

                    Dim isWithNextMasechta = IsCurrentBlattWithNextMasechta(masechta, blatt)
                    dafYomi = New Daf(masechta, blatt, isWithNextMasechta)
                    Exit For
                End If
            Next

            Return dafYomi
        End Function

        Private Shared Function IsCurrentBlattWithNextMasechta(ByVal masechta As Integer, ByVal blatt As Integer) As Boolean
            Return masechta = 35 AndAlso blatt = 22 OrElse masechta = 36 AndAlso blatt = 25
        End Function

        ''' <summary>
        ''' Return the <ahref="http://en.wikipedia.org/wiki/Julian_day">Julian day</a> from a Java Date.
        ''' </summary>
        ''' <paramname="date">
        '''            The Java Date </param>
        ''' <returns> the Julian day number corresponding to the date </returns>
        Private Shared Function GetJulianDay(ByVal [date] As Date) As Integer
            Dim calendar As Date = New DateTime()
            calendar = [date]
            Dim year = calendar.Year
            Dim month = calendar.Month
            Dim day = calendar.Day

            If month <= 2 Then
                year -= 1
                month += 12
            End If

            Dim a As Integer = year / 100
            Dim b As Integer = 2 - a + a / 4
            Return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5
        End Function
    End Class
End Namespace
