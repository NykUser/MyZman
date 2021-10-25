'port from https://github.com/Yitzchok/Zmanim/blob/master/src/Zmanim/JewishCalendar/JewishCalendar.cs
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
'// * Zmanim .NET API Is distributed in the hope that it will be useful,
'// * but WITHOUT ANY WARRANTY; without even the implied warranty of
'// * MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'// * GNU Lesser General Public License for more details.
'// *
'// * You should have received a copy of the GNU Lesser General Public License
'// * along with Zmanim.NET API.  If Not, see <http://www.gnu.org/licenses/lgpl.html>.
Imports System
Imports System.Diagnostics
Namespace Zmanim.JewishCalendar
    Partial Public Class JewishCalendar
        Public Enum JewishMonth
            NONE = -1
            NISSAN = 1
            IYAR
            SIVAN
            TAMMUZ
            AV
            ELUL
            TISHREI
            CHESHVAN
            KISLEV
            TEVES
            SHEVAT
            ADAR
            ADAR_II
        End Enum

        Public Enum JewishHoliday
            NONE = -1
            EREV_PESACH = 0
            PESACH = 1
            CHOL_HAMOED_PESACH = 2
            PESACH_SHENI = 3
            EREV_SHAVUOS = 4
            SHAVUOS = 5
            SEVENTEEN_OF_TAMMUZ = 6
            TISHA_BEAV = 7
            TU_BEAV = 8
            EREV_ROSH_HASHANA = 9
            ROSH_HASHANA = 10
            FAST_OF_GEDALYAH = 11
            EREV_YOM_KIPPUR = 12
            YOM_KIPPUR = 13
            EREV_SUCCOS = 14
            SUCCOS = 15
            CHOL_HAMOED_SUCCOS = 16
            HOSHANA_RABBA = 17
            SHEMINI_ATZERES = 18
            SIMCHAS_TORAH = 19
            CHANUKAH = 21
            TENTH_OF_TEVES = 22
            TU_BESHVAT = 23
            FAST_OF_ESTHER = 24
            PURIM = 25
            SHUSHAN_PURIM = 26
            PURIM_KATAN = 27
            ROSH_CHODESH = 28
            YOM_HASHOAH = 29
            YOM_HAZIKARON = 30
            YOM_HAATZMAUT = 31
            YOM_YERUSHALAYIM = 32
        End Enum

        Public Enum JewishYearType
            CHASERIM = 0
            KESIDRAN
            SHELAIMIM
        End Enum

        Public Enum Parsha
            NONE
            BERESHIS
            NOACH
            LECH_LECHA
            VAYERA
            CHAYEI_SARA
            TOLDOS
            VAYETZEI
            VAYISHLACH
            VAYESHEV
            MIKETZ
            VAYIGASH
            VAYECHI
            SHEMOS
            VAERA
            BO
            BESHALACH
            YISRO
            MISHPATIM
            TERUMAH
            TETZAVEH
            KI_SISA
            VAYAKHEL
            PEKUDEI
            VAYIKRA
            TZAV
            SHMINI
            TAZRIA
            METZORA
            ACHREI_MOS
            KEDOSHIM
            EMOR
            BEHAR
            BECHUKOSAI
            BAMIDBAR
            NASSO
            BEHAALOSCHA
            SHLACH
            KORACH
            CHUKAS
            BALAK
            PINCHAS
            MATOS
            MASEI
            DEVARIM
            VAESCHANAN
            EIKEV
            REEH
            SHOFTIM
            KI_SEITZEI
            KI_SAVO
            NITZAVIM
            VAYEILECH
            HAAZINU
            VZOS_HABERACHA
            VAYAKHEL_PEKUDEI
            TAZRIA_METZORA
            ACHREI_MOS_KEDOSHIM
            BEHAR_BECHUKOSAI
            CHUKAS_BALAK
            MATOS_MASEI
            NITZAVIM_VAYEILECH
            SHKALIM
            ZACHOR
            PARA
            HACHODESH
        End Enum

        Public Function NativeMonthToJewishMonth(ByVal nativeMonth As Integer, ByVal leapYear As Boolean) As JewishMonth
            Select Case nativeMonth
                Case 1
                    Return JewishMonth.TISHREI
                Case 2
                    Return JewishMonth.CHESHVAN
                Case 3
                    Return JewishMonth.KISLEV
                Case 4
                    Return JewishMonth.TEVES
                Case 5
                    Return JewishMonth.SHEVAT
            End Select

            If leapYear Then

                Select Case nativeMonth
                    Case 6
                        Return JewishMonth.ADAR
                    Case 7
                        Return JewishMonth.ADAR_II
                    Case 8
                        Return JewishMonth.NISSAN
                    Case 9
                        Return JewishMonth.IYAR
                    Case 10
                        Return JewishMonth.SIVAN
                    Case 11
                        Return JewishMonth.TAMMUZ
                    Case 12
                        Return JewishMonth.AV
                    Case 13
                        Return JewishMonth.ELUL
                End Select
            Else

                Select Case nativeMonth
                    Case 6
                        Return JewishMonth.ADAR
                    Case 7
                        Return JewishMonth.NISSAN
                    Case 8
                        Return JewishMonth.IYAR
                    Case 9
                        Return JewishMonth.SIVAN
                    Case 10
                        Return JewishMonth.TAMMUZ
                    Case 11
                        Return JewishMonth.AV
                    Case 12
                        Return JewishMonth.ELUL
                End Select
            End If

            Return JewishMonth.NONE
        End Function

        Public Function JewishMonthToNativeMonth(ByVal jewishMonth As JewishMonth, ByVal leapYear As Boolean) As Integer
            Select Case jewishMonth
                Case JewishMonth.TISHREI
                    Return 1
                Case JewishMonth.CHESHVAN
                    Return 2
                Case JewishMonth.KISLEV
                    Return 3
                Case JewishMonth.TEVES
                    Return 4
                Case JewishMonth.SHEVAT
                    Return 5
            End Select

            If leapYear Then

                Select Case jewishMonth
                    Case JewishMonth.ADAR
                        Return 6
                    Case JewishMonth.ADAR_II
                        Return 7
                    Case JewishMonth.NISSAN
                        Return 8
                    Case JewishMonth.IYAR
                        Return 9
                    Case JewishMonth.SIVAN
                        Return 10
                    Case JewishMonth.TAMMUZ
                        Return 11
                    Case JewishMonth.AV
                        Return 12
                    Case JewishMonth.ELUL
                        Return 13
                End Select
            Else

                Select Case jewishMonth
                    Case JewishMonth.ADAR
                        Return 6
                    Case JewishMonth.NISSAN
                        Return 7
                    Case JewishMonth.IYAR
                        Return 8
                    Case JewishMonth.SIVAN
                        Return 9
                    Case JewishMonth.TAMMUZ
                        Return 10
                    Case JewishMonth.AV
                        Return 11
                    Case JewishMonth.ELUL
                        Return 12
                End Select
            End If

            Return -1
        End Function

        'Public Function GetDafYomiBavli(ByVal dt As DateTime) As Daf
        '    Return YomiCalculator.GetDafYomiBavli(dt)
        'End Function

        Public Function GetJewishDayOfWeek(ByVal dt As DateTime) As Integer
            Return CInt(GetDayOfWeek(dt)) + 1
        End Function

        Public Overrides Function IsLeapYear(ByVal year As Integer) As Boolean
            Return MyBase.IsLeapYear(year)
        End Function

        Private Function GetJewishHoliday(ByVal dayOfMonth As Integer, ByVal dayOfWeek As Integer, ByVal year As Integer, ByVal inIsrael As Boolean, ByVal useModernHolidays As Boolean, ByVal hebrewMonth As JewishMonth, ByVal isAdar29Days As Boolean) As JewishHoliday
            Select Case hebrewMonth
                Case JewishMonth.NISSAN

                    If dayOfMonth = 14 Then
                        Return JewishHoliday.EREV_PESACH
                    ElseIf dayOfMonth = 15 OrElse dayOfMonth = 21 OrElse (Not inIsrael AndAlso (dayOfMonth = 16 OrElse dayOfMonth = 22)) Then
                        Return JewishHoliday.PESACH
                    ElseIf dayOfMonth >= 17 AndAlso dayOfMonth <= 20 OrElse (dayOfMonth = 16 AndAlso inIsrael) Then
                        Return JewishHoliday.CHOL_HAMOED_PESACH
                    End If

                    If useModernHolidays AndAlso ((dayOfMonth = 26 AndAlso dayOfWeek = 5) OrElse (dayOfMonth = 28 AndAlso dayOfWeek = 2) OrElse (dayOfMonth = 27 AndAlso dayOfWeek <> 1 AndAlso dayOfWeek <> 6)) Then
                        Return JewishHoliday.YOM_HASHOAH
                    End If

                Case JewishMonth.IYAR

                    If useModernHolidays AndAlso ((dayOfMonth = 4 AndAlso dayOfWeek = 3) OrElse ((dayOfMonth = 3 OrElse dayOfMonth = 2) AndAlso dayOfWeek = 4) OrElse (dayOfMonth = 5 AndAlso dayOfWeek = 2)) Then
                        Return JewishHoliday.YOM_HAZIKARON
                    End If

                    If useModernHolidays AndAlso ((dayOfMonth = 5 AndAlso dayOfWeek = 4) OrElse ((dayOfMonth = 4 OrElse dayOfMonth = 3) AndAlso dayOfWeek = 5) OrElse (dayOfMonth = 6 AndAlso dayOfWeek = 3)) Then
                        Return JewishHoliday.YOM_HAATZMAUT
                    End If

                    If dayOfMonth = 14 Then
                        Return JewishHoliday.PESACH_SHENI
                    End If

                    If useModernHolidays AndAlso dayOfMonth = 28 Then
                        Return JewishHoliday.YOM_YERUSHALAYIM
                    End If

                Case JewishMonth.SIVAN

                    If dayOfMonth = 5 Then
                        Return JewishHoliday.EREV_SHAVUOS
                    ElseIf dayOfMonth = 6 OrElse (dayOfMonth = 7 AndAlso Not inIsrael) Then
                        Return JewishHoliday.SHAVUOS
                    End If

                Case JewishMonth.TAMMUZ

                    If (dayOfMonth = 17 AndAlso dayOfWeek <> 7) OrElse (dayOfMonth = 18 AndAlso dayOfWeek = 1) Then
                        Return JewishHoliday.SEVENTEEN_OF_TAMMUZ
                    End If

                Case JewishMonth.AV

                    If (dayOfWeek = 1 AndAlso dayOfMonth = 10) OrElse (dayOfWeek <> 7 AndAlso dayOfMonth = 9) Then
                        Return JewishHoliday.TISHA_BEAV
                    ElseIf dayOfMonth = 15 Then
                        Return JewishHoliday.TU_BEAV
                    End If

                Case JewishMonth.ELUL

                    If dayOfMonth = 29 Then
                        Return JewishHoliday.EREV_ROSH_HASHANA
                    End If

                Case JewishMonth.TISHREI

                    If dayOfMonth = 1 OrElse dayOfMonth = 2 Then
                        Return JewishHoliday.ROSH_HASHANA
                    ElseIf (dayOfMonth = 3 AndAlso dayOfWeek <> 7) OrElse (dayOfMonth = 4 AndAlso dayOfWeek = 1) Then
                        Return JewishHoliday.FAST_OF_GEDALYAH
                    ElseIf dayOfMonth = 9 Then
                        Return JewishHoliday.EREV_YOM_KIPPUR
                    ElseIf dayOfMonth = 10 Then
                        Return JewishHoliday.YOM_KIPPUR
                    ElseIf dayOfMonth = 14 Then
                        Return JewishHoliday.EREV_SUCCOS
                    End If

                    If dayOfMonth = 15 OrElse (dayOfMonth = 16 AndAlso Not inIsrael) Then
                        Return JewishHoliday.SUCCOS
                    End If

                    If dayOfMonth >= 17 AndAlso dayOfMonth <= 20 OrElse (dayOfMonth = 16 AndAlso inIsrael) Then
                        Return JewishHoliday.CHOL_HAMOED_SUCCOS
                    End If

                    If dayOfMonth = 21 Then
                        Return JewishHoliday.HOSHANA_RABBA
                    End If

                    If dayOfMonth = 22 Then
                        Return JewishHoliday.SHEMINI_ATZERES
                    End If

                    If dayOfMonth = 23 AndAlso Not inIsrael Then
                        Return JewishHoliday.SIMCHAS_TORAH
                    End If

                Case JewishMonth.KISLEV

                    If dayOfMonth >= 25 Then
                        Return JewishHoliday.CHANUKAH
                    End If

                Case JewishMonth.TEVES

                    If dayOfMonth = 1 OrElse dayOfMonth = 2 OrElse (dayOfMonth = 3 AndAlso isAdar29Days) Then
                        Return JewishHoliday.CHANUKAH
                    ElseIf dayOfMonth = 10 Then
                        Return JewishHoliday.TENTH_OF_TEVES
                    End If

                Case JewishMonth.SHEVAT

                    If dayOfMonth = 15 Then
                        Return JewishHoliday.TU_BESHVAT
                    End If

                Case JewishMonth.ADAR

                    If Not IsLeapYear(year) Then

                        If ((dayOfMonth = 11 OrElse dayOfMonth = 12) AndAlso dayOfWeek = 5) OrElse (dayOfMonth = 13 AndAlso Not (dayOfWeek = 6 OrElse dayOfWeek = 7)) Then
                            Return JewishHoliday.FAST_OF_ESTHER
                        End If

                        If dayOfMonth = 14 Then
                            Return JewishHoliday.PURIM
                            'Nykedit change to Return SHUSHAN_PURIM not inIsrael 
                        ElseIf dayOfMonth = 15 Then 'AndAlso inIsrael
                            Return JewishHoliday.SHUSHAN_PURIM
                        End If
                    Else

                        If dayOfMonth = 14 Then
                            Return JewishHoliday.PURIM_KATAN
                        End If
                    End If

                Case JewishMonth.ADAR_II

                    If ((dayOfMonth = 11 OrElse dayOfMonth = 12) AndAlso dayOfWeek = 5) OrElse (dayOfMonth = 13 AndAlso Not (dayOfWeek = 6 OrElse dayOfWeek = 7)) Then
                        Return JewishHoliday.FAST_OF_ESTHER
                    End If

                    If dayOfMonth = 14 Then
                        Return JewishHoliday.PURIM
                        'Nykedit change to Return SHUSHAN_PURIM not inIsrael 
                    ElseIf dayOfMonth = 15 Then 'AndAlso inIsrael
                        Return JewishHoliday.SHUSHAN_PURIM
                    End If
            End Select

            Return JewishHoliday.NONE
        End Function

        Private Function IsYomTovAssurBemelacha(ByVal holidayIndex As JewishHoliday) As Boolean
            Return holidayIndex = JewishHoliday.PESACH OrElse holidayIndex = JewishHoliday.SHAVUOS OrElse holidayIndex = JewishHoliday.SUCCOS OrElse holidayIndex = JewishHoliday.SHEMINI_ATZERES OrElse holidayIndex = JewishHoliday.SIMCHAS_TORAH OrElse holidayIndex = JewishHoliday.ROSH_HASHANA OrElse holidayIndex = JewishHoliday.YOM_KIPPUR
        End Function

        Private Function IsCholHamoed(ByVal holidayIndex As JewishHoliday) As Boolean
            Return holidayIndex = JewishHoliday.CHOL_HAMOED_PESACH OrElse holidayIndex = JewishHoliday.CHOL_HAMOED_SUCCOS
        End Function

        Private Function GetDayOfOmer(ByVal jewishMonth As JewishMonth, ByVal dayOfMonth As Integer) As Integer
            Dim omer As Integer = -1

            If jewishMonth = JewishMonth.NISSAN AndAlso dayOfMonth >= 16 Then
                omer = dayOfMonth - 15
            ElseIf jewishMonth = JewishMonth.IYAR Then
                omer = dayOfMonth + 15
            ElseIf jewishMonth = JewishMonth.SIVAN AndAlso dayOfMonth < 6 Then
                omer = dayOfMonth + 44
            End If

            Return omer
        End Function
    End Class

    Partial Public Class JewishCalendar
        Inherits System.Globalization.HebrewCalendar

        Public Function IsLeapYearFromDateTime(ByVal dt As DateTime) As Boolean
            Return IsLeapYear(GetYear(dt))
        End Function

        Public Function GetJewishMonth(ByVal dt As DateTime) As JewishMonth
            Return NativeMonthToJewishMonth(GetMonth(dt), IsLeapYearFromDateTime(dt))
        End Function

        Public Function GetJewishDateTime(ByVal year As Integer, ByVal month As JewishMonth, ByVal day As Integer) As DateTime
            Return ToDateTime(year, JewishMonthToNativeMonth(month, IsLeapYear(year)), day, 0, 0, 0, 0)
        End Function

        Public Function GetJewishYearType(ByVal dt As DateTime) As JewishYearType
            Dim jType As JewishYearType = JewishYearType.SHELAIMIM

            If MonthIs29Days(dt, JewishMonth.CHESHVAN) Then
                jType = JewishYearType.KESIDRAN

                If MonthIs29Days(dt, JewishMonth.KISLEV) Then
                    jType = JewishYearType.CHASERIM
                End If
            End If

            Return jType
        End Function

        Public Function MonthIs29Days(ByVal dt As DateTime, ByVal month As JewishMonth) As Boolean
            Return GetJewishDaysInMonth(dt, month) = 29
        End Function
        'nykedit added
        Public Function GetJewishDaysInMonth(ByVal dt As DateTime) As Integer
            Return GetJewishDaysInMonth(GetYear(dt), GetJewishMonth(dt))
        End Function
        Public Function GetJewishDaysInMonth(ByVal dt As DateTime, ByVal month As JewishMonth) As Integer
            Return GetJewishDaysInMonth(GetYear(dt), month)
        End Function

        Public Function GetJewishDaysInMonth(ByVal year As Integer, ByVal month As JewishMonth) As Integer
            Dim nativeMonth As Integer = JewishMonthToNativeMonth(month, IsLeapYear(year))
            Return GetDaysInMonth(year, nativeMonth)
        End Function

        Public Function GetJewishHoliday(ByVal dt As DateTime, ByVal inIsrael As Boolean) As JewishHoliday
            Return GetJewishHoliday(dt, inIsrael, True)
        End Function

        Public Function GetJewishHoliday(ByVal dt As DateTime, ByVal inIsrael As Boolean, ByVal useModernHolidays As Boolean) As JewishHoliday
            Dim hebrewMonth As JewishMonth = GetJewishMonth(dt)
            Return GetJewishHoliday(GetDayOfMonth(dt), GetJewishDayOfWeek(dt), GetYear(dt), inIsrael, useModernHolidays, hebrewMonth, MonthIs29Days(dt, JewishMonth.KISLEV))
        End Function

        Public Function IsYomTov(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Dim holidayIndex As JewishHoliday = GetJewishHoliday(dt, inIsrael)

            If (IsErevYomTov(dt, inIsrael) AndAlso (holidayIndex <> JewishHoliday.HOSHANA_RABBA AndAlso (holidayIndex = JewishHoliday.CHOL_HAMOED_PESACH AndAlso GetDayOfMonth(dt) <> 20))) OrElse holidayIndex = JewishHoliday.CHANUKAH OrElse (IsTaanis(dt, inIsrael) AndAlso holidayIndex <> JewishHoliday.YOM_KIPPUR) Then
                Return False
            End If

            Return holidayIndex <> JewishHoliday.NONE
        End Function

        Public Function IsYomTovAssurBemelacha(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Dim holidayIndex As JewishHoliday = GetJewishHoliday(dt, inIsrael)
            Return IsYomTovAssurBemelacha(holidayIndex)
        End Function

        Public Function IsCholHamoed(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Dim holidayIndex As JewishHoliday = GetJewishHoliday(dt, inIsrael)
            Return IsCholHamoed(holidayIndex)
        End Function

        Public Function IsErevYomTov(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Dim holidayIndex As JewishHoliday = GetJewishHoliday(dt, inIsrael)
            Return holidayIndex = JewishHoliday.EREV_PESACH OrElse holidayIndex = JewishHoliday.EREV_SHAVUOS OrElse holidayIndex = JewishHoliday.EREV_ROSH_HASHANA OrElse holidayIndex = JewishHoliday.EREV_YOM_KIPPUR OrElse holidayIndex = JewishHoliday.EREV_SUCCOS OrElse holidayIndex = JewishHoliday.HOSHANA_RABBA OrElse (holidayIndex = JewishHoliday.CHOL_HAMOED_PESACH AndAlso GetDayOfMonth(dt) = 20)
        End Function

        Public Function IsErevRoshChodesh(ByVal dt As DateTime) As Boolean
            Return (GetDayOfMonth(dt) = 29 AndAlso GetJewishMonth(dt) <> JewishMonth.ELUL)
        End Function

        Public Function IsTaanis(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Dim holidayIndex As JewishHoliday = GetJewishHoliday(dt, inIsrael)
            Return holidayIndex = JewishHoliday.SEVENTEEN_OF_TAMMUZ OrElse holidayIndex = JewishHoliday.TISHA_BEAV OrElse holidayIndex = JewishHoliday.YOM_KIPPUR OrElse holidayIndex = JewishHoliday.FAST_OF_GEDALYAH OrElse holidayIndex = JewishHoliday.TENTH_OF_TEVES OrElse holidayIndex = JewishHoliday.FAST_OF_ESTHER
        End Function

        Public Function GetDayOfChanukah(ByVal dt As DateTime) As Integer
            If IsChanukah(dt) Then

                If GetJewishMonth(dt) = JewishMonth.KISLEV Then
                    Return GetDayOfMonth(dt) - 24
                Else
                    Return If(MonthIs29Days(dt, JewishMonth.KISLEV), GetDayOfMonth(dt) + 5, GetDayOfMonth(dt) + 6)
                End If
            Else
                Return -1
            End If
        End Function

        Public Function IsChanukah(ByVal dt As DateTime) As Boolean
            Return GetJewishHoliday(dt, True) = JewishHoliday.CHANUKAH
        End Function

        Public Function IsRoshChodesh(ByVal dt As DateTime) As Boolean
            Return (GetDayOfMonth(dt) = 1 AndAlso GetJewishMonth(dt) <> JewishMonth.TISHREI) OrElse GetDayOfMonth(dt) = 30
        End Function

        Public Function GetDayOfOmer(ByVal dt As DateTime) As Integer
            Return GetDayOfOmer(GetJewishMonth(dt), GetDayOfMonth(dt))
        End Function

        Private Function GetParshaYearType(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Integer
            Dim roshHashanaDayOfWeek As Integer = CInt(MyBase.GetDayOfWeek(MyBase.ToDateTime(MyBase.GetYear(dt), 1, 1, 14, 0, 0, 0)))

            If MyBase.IsLeapYear(MyBase.GetYear(dt)) Then

                Select Case roshHashanaDayOfWeek
                    Case CInt(DayOfWeek.Monday)

                        If IsKislevShort(dt) Then

                            If inIsrael Then
                                Return 14
                            End If

                            Return 6
                        End If

                        If IsCheshvanLong(dt) Then

                            If inIsrael Then
                                Return 15
                            End If

                            Return 7
                        End If

                    Case CInt(DayOfWeek.Tuesday)

                        If inIsrael Then
                            Return 15
                        End If

                        Return 7
                    Case CInt(DayOfWeek.Thursday)

                        If IsKislevShort(dt) Then
                            Return 8
                        End If

                        If IsCheshvanLong(dt) Then
                            Return 9
                        End If

                    Case CInt(DayOfWeek.Saturday)

                        If IsKislevShort(dt) Then
                            Return 10
                        End If

                        If IsCheshvanLong(dt) Then

                            If inIsrael Then
                                Return 16
                            End If

                            Return 11
                        End If

                    Case Else
                End Select
            Else

                Select Case roshHashanaDayOfWeek
                    Case CInt(DayOfWeek.Monday)

                        If IsKislevShort(dt) Then
                            Return 0
                        End If

                        If IsCheshvanLong(dt) Then

                            If inIsrael Then
                                Return 12
                            End If

                            Return 1
                        End If

                    Case CInt(DayOfWeek.Tuesday)

                        If inIsrael Then
                            Return 12
                        End If

                        Return 1
                    Case CInt(DayOfWeek.Thursday)

                        If IsCheshvanLong(dt) Then
                            Return 3
                        End If

                        If Not IsKislevShort(dt) Then

                            If inIsrael Then
                                Return 13
                            End If

                            Return 2
                        End If

                    Case CInt(DayOfWeek.Saturday)

                        If IsKislevShort(dt) Then
                            Return 4
                        End If

                        If IsCheshvanLong(dt) Then
                            Return 5
                        End If

                    Case Else
                End Select
            End If

            Return -1
        End Function

        Public Function GetParshah(ByVal dt As DateTime) As Parsha
            dt = New DateTime(dt.Year, dt.Month, dt.Day, 14, 0, 0)

            If dt.DayOfWeek <> DayOfWeek.Saturday Then
                'NykEdit
                dt = dt.AddDays(Add_till_sab(dt.DayOfWeek))
                'Return Parsha.NONE
            End If

            Dim yearType As Integer = GetParshaYearType(dt, False)
            Dim roshHashanaDayOfWeek As Integer = CInt(MyBase.GetDayOfWeek(MyBase.ToDateTime(MyBase.GetYear(dt), 1, 1, 14, 0, 0, 0)))
            Dim daysSinceRoshHashana As TimeSpan = dt - MyBase.ToDateTime(MyBase.GetYear(dt), 1, 1, 14, 0, 0, 0)
            Dim day As Integer = roshHashanaDayOfWeek + daysSinceRoshHashana.Days + 1

            If yearType >= 0 Then
                Return ParshaList(yearType, day / 7)
            End If

            Return Parsha.NONE
        End Function

        Public Function GetParshah(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Parsha
            dt = New DateTime(dt.Year, dt.Month, dt.Day, 14, 0, 0)

            If dt.DayOfWeek <> DayOfWeek.Saturday Then
                'NykEdit
                dt = dt.AddDays(Add_till_sab(dt.DayOfWeek))
                'Return Parsha.NONE
            End If

            Dim yearType As Integer = GetParshaYearType(dt, inIsrael)
            Dim roshHashanaDayOfWeek As Integer = CInt(MyBase.GetDayOfWeek(MyBase.ToDateTime(MyBase.GetYear(dt), 1, 1, 14, 0, 0, 0)))
            Dim daysSinceRoshHashana As TimeSpan = dt - MyBase.ToDateTime(MyBase.GetYear(dt), 1, 1, 14, 0, 0, 0)
            Dim day As Integer = roshHashanaDayOfWeek + daysSinceRoshHashana.Days + 1

            If yearType >= 0 Then
                Return ParshaList(yearType, day / 7)
            End If

            Return Parsha.NONE
        End Function

        Private Function IsCheshvanLong(ByVal dt As DateTime) As Boolean
            Return MyBase.GetDaysInMonth(MyBase.GetYear(dt), 2) = 30
        End Function

        Private Function IsKislevShort(ByVal dt As DateTime) As Boolean
            Return MyBase.GetDaysInMonth(MyBase.GetYear(dt), 3) = 29
        End Function

        Private ReadOnly ParshaList As Parsha(,) = New Parsha(,) {
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NONE, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS_BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NONE, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS_BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH, Parsha.NONE},
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM},
        {Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.ACHREI_MOS, Parsha.NONE, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS, Parsha.MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM},
        {Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.ACHREI_MOS, Parsha.NONE, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS, Parsha.MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH},
        {Parsha.NONE, Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH},
        {Parsha.NONE, Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NONE, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS_BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH},
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR_BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL_PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.NONE, Parsha.SHMINI, Parsha.TAZRIA_METZORA, Parsha.ACHREI_MOS_KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM, Parsha.NONE, Parsha.NONE, Parsha.NONE, Parsha.NONE},
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH, Parsha.NONE},
        {Parsha.NONE, Parsha.VAYEILECH, Parsha.HAAZINU, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS, Parsha.MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM},
        {Parsha.NONE, Parsha.NONE, Parsha.HAAZINU, Parsha.NONE, Parsha.NONE, Parsha.BERESHIS, Parsha.NOACH, Parsha.LECH_LECHA, Parsha.VAYERA, Parsha.CHAYEI_SARA, Parsha.TOLDOS, Parsha.VAYETZEI, Parsha.VAYISHLACH, Parsha.VAYESHEV, Parsha.MIKETZ, Parsha.VAYIGASH, Parsha.VAYECHI, Parsha.SHEMOS, Parsha.VAERA, Parsha.BO, Parsha.BESHALACH, Parsha.YISRO, Parsha.MISHPATIM, Parsha.TERUMAH, Parsha.TETZAVEH, Parsha.KI_SISA, Parsha.VAYAKHEL, Parsha.PEKUDEI, Parsha.VAYIKRA, Parsha.TZAV, Parsha.SHMINI, Parsha.TAZRIA, Parsha.METZORA, Parsha.NONE, Parsha.ACHREI_MOS, Parsha.KEDOSHIM, Parsha.EMOR, Parsha.BEHAR, Parsha.BECHUKOSAI, Parsha.BAMIDBAR, Parsha.NASSO, Parsha.BEHAALOSCHA, Parsha.SHLACH, Parsha.KORACH, Parsha.CHUKAS, Parsha.BALAK, Parsha.PINCHAS, Parsha.MATOS_MASEI, Parsha.DEVARIM, Parsha.VAESCHANAN, Parsha.EIKEV, Parsha.REEH, Parsha.SHOFTIM, Parsha.KI_SEITZEI, Parsha.KI_SAVO, Parsha.NITZAVIM_VAYEILECH}}

        'NykEdit
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

        'nykedit added Function's not in the C# port from Java
        '* Returns a parsha enum if the Shabbos Is one of the four parshiyos of Parsha.SHKALIM, Parsha.ZACHOR, Parsha.PARA,
        '* Parsha.HACHODESH Or Parsha.NONE for a regular Shabbos (Or any weekday).
        '* @return one of the four parshiyos of Parsha.SHKALIM, Parsha.ZACHOR, Parsha.PARA, Parsha.HACHODESH Or Parsha.NONE.
        Public Function getSpecialShabbos(ByVal dt As DateTime) As Parsha
            If dt.DayOfWeek = DayOfWeek.Saturday Then
                If GetJewishMonth(dt) = JewishMonth.SHEVAT AndAlso Not IsLeapYearFromDateTime(dt) _
                    OrElse GetJewishMonth(dt) = JewishMonth.ADAR AndAlso IsLeapYearFromDateTime(dt) Then
                    If GetDayOfMonth(dt) = 25 OrElse GetDayOfMonth(dt) = 27 OrElse GetDayOfMonth(dt) = 29 Then
                        Return Parsha.SHKALIM
                    End If
                End If


                If GetJewishMonth(dt) = JewishMonth.ADAR AndAlso Not IsLeapYearFromDateTime(dt) _
                OrElse GetJewishMonth(dt) = JewishMonth.ADAR_II Then
                    If GetDayOfMonth(dt) = 1 Then Return Parsha.SHKALIM

                    If GetDayOfMonth(dt) = 8 OrElse GetDayOfMonth(dt) = 9 _
                        OrElse GetDayOfMonth(dt) = 11 OrElse GetDayOfMonth(dt) = 13 Then Return Parsha.ZACHOR

                    If GetDayOfMonth(dt) = 18 OrElse GetDayOfMonth(dt) = 20 _
                        OrElse GetDayOfMonth(dt) = 22 OrElse GetDayOfMonth(dt) = 23 Then Return Parsha.PARA

                    If GetDayOfMonth(dt) = 25 OrElse GetDayOfMonth(dt) = 27 _
                        OrElse GetDayOfMonth(dt) = 29 Then Return Parsha.HACHODESH
                End If

                If GetJewishMonth(dt) = JewishMonth.NISSAN AndAlso GetDayOfMonth(dt) = 1 Then _
                   Return Parsha.HACHODESH
            End If
            Return Parsha.NONE

        End Function

        Public Function hasCandleLighting(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Return isTomorrowShabbosOrYomTov(dt, inIsrael)
        End Function

        Public Function isTomorrowShabbosOrYomTov(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Return (dt.DayOfWeek = DayOfWeek.Friday) OrElse IsErevYomTov(dt, inIsrael) OrElse isErevYomTovSheni(dt, inIsrael)
        End Function

        Public Function isErevYomTovSheni(ByVal dt As DateTime, ByVal inIsrael As Boolean) As Boolean
            Return (((GetJewishMonth(dt) = JewishMonth.ADAR) _
                    AndAlso (GetDayOfMonth(dt) = 1)) _
                    OrElse (Not inIsrael _
                    AndAlso (((GetJewishMonth(dt) = JewishMonth.NISSAN) _
                    AndAlso ((GetDayOfMonth(dt) = 15) _
                    OrElse (GetDayOfMonth(dt) = 21))) _
                    OrElse (((GetJewishMonth(dt) = JewishMonth.TISHREI) _
                    AndAlso ((GetDayOfMonth(dt) = 15) _
                    OrElse (GetDayOfMonth(dt) = 22))) _
                    OrElse ((GetJewishMonth(dt) = JewishMonth.SIVAN) _
                    AndAlso (GetDayOfMonth(dt) = 6))))))
        End Function

    End Class
End Namespace
