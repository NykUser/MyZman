Class AddedGets
    'use a new instacnce of CZC when date is sent in
    Private TempCZC As ComplexZmanimCalendar
    Public Function ZmanGetMisheyakir50(DateIn As Date, LocationIn As GeoLocation)
        TempCZC = New ComplexZmanimCalendar(DateIn, LocationIn)
        If varSC.UseOlderUsnoAlgorithm = False Then
            TempCZC.AstronomicalCalculator = New NOAACalculator()
        Else
            TempCZC.AstronomicalCalculator = New ZmanimCalculator()
        End If

        Try
            Return CDate(TempCZC.GetSunrise.ToString).AddMinutes(-50) ' .ToLongTimeString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ZmanGetPlagHamincha8Point5Degrees(DateIn As Date, LocationIn As GeoLocation)
        TempCZC = New ComplexZmanimCalendar(DateIn, LocationIn)
        If varSC.UseOlderUsnoAlgorithm = False Then
            TempCZC.AstronomicalCalculator = New NOAACalculator()
        Else
            TempCZC.AstronomicalCalculator = New ZmanimCalculator()
        End If
        Try
            Dim mydate As Time
            'looks fine now - the was some kind of reason for adding time formats in the past
            Return CDate(TempCZC.GetPlagHamincha(TempCZC.GetSunriseOffsetByDegrees(90 + 8.5), TempCZC.GetSunsetOffsetByDegrees(90 + 8.5))) '.ToString("H:mm:ss")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ZmanGetBeinHashmases8Point5Degrees13Min(DateIn As Date, LocationIn As GeoLocation)
        TempCZC = New ComplexZmanimCalendar(DateIn, LocationIn)
        If varSC.UseOlderUsnoAlgorithm = False Then
            TempCZC.AstronomicalCalculator = New NOAACalculator()
        Else
            TempCZC.AstronomicalCalculator = New ZmanimCalculator()
        End If
        Try
            Return CDate(TempCZC.GetSunsetOffsetByDegrees(90 + 8.5).ToString).AddMinutes(-13.5) ' ToLongTimeString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ZmanGetBeinHashmases8Point5Degrees5Point3(DateIn As Date, LocationIn As GeoLocation)
        TempCZC = New ComplexZmanimCalendar(DateIn, LocationIn)
        If varSC.UseOlderUsnoAlgorithm = False Then
            TempCZC.AstronomicalCalculator = New NOAACalculator()
        Else
            TempCZC.AstronomicalCalculator = New ZmanimCalculator()
        End If
        Try
            Dim myTimeSpan = CDate(TempCZC.GetTzaisGeonim8Point5Degrees().ToString) - CDate(TempCZC.GetSunset().ToString)
            Return CDate(TempCZC.GetSunsetOffsetByDegrees(90 + 8.5).ToString).AddMilliseconds(-(myTimeSpan.TotalMilliseconds / 5.3333)) '.ToLongTimeString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ZmanGetParsha(ByVal DateIn As Date, inIsrael As Boolean, Optional EngNames As Boolean = False) As String
        'move to sab - needed for week of R"H 
        If DateIn.DayOfWeek <> 6 Then DateIn = DateIn.AddDays(Add_till_sab(DateIn.DayOfWeek))

        Dim jc As New JewishCalendar()
        Dim jewishYear = jc.GetYear(DateIn)
        Dim roshHashana As DateTime = jc.GetJewishDateTime(jewishYear, JewishCalendar.JewishMonth.TISHREI, 1)
        Dim roshHashanaDayOfWeek As Integer = jc.GetJewishDayOfWeek(roshHashana)
        Dim kvia As String = jc.GetJewishYearType(roshHashana).ToString
        Dim IsLeapYear As Boolean = jc.IsLeapYear(jewishYear)

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
        Dim EngParshiyos As String() = {"BERESHIS", "NOACH", "LECH LECHA", "VAYERA", "CHAYEI SARA", "TOLDOS", "VAYETZEI", "VAYISHLACH", "VAYESHEV", "MIKETZ", "VAYIGASH", "VAYECHI", "SHEMOS", "VAERA", "BO", "BESHALACH", "YISRO", "MISHPATIM", "TERUMAH", "TETZAVEH", "KI SISA", "VAYAKHEL", "PEKUDEI", "VAYIKRA", "TZAV", "SHMINI", "TAZRIA", "METZORA", "ACHREI MOS", "KEDOSHIM", "EMOR", "BEHAR", "BECHUKOSAI", "BAMIDBAR", "NASSO", "BEHAALOSCHA", "SHLACH", "KORACH", "CHUKAS", "BALAK", "PINCHAS", "MATOS", "MASEI", "DEVARIM", "VAESCHANAN", "EIKEV", "REEH", "SHOFTIM", "KI SEITZEI", "KI SAVO", "NITZAVIM", "VAYEILECH", "HAAZINU", "VAYAKHEL PEKUDEI", "TAZRIA METZORA", "ACHREI MOS KEDOSHIM", "BEHAR BECHUKOSAI", "CHUKAS BALAK", "NITZAVIM VAYEILECH", "MATOS MASEI"}

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

            If EngNames = True Then
                Return EngParshiyos(index)
            Else
                Return hebrewParshiyos(index)
            End If
        End If
    End Function
    Private Function Add_till_sab(ByVal day)
        'monday is 1
        Dim temp1
        If day = 0 Then temp1 = 6
        If day = 1 Then temp1 = 5
        If day = 2 Then temp1 = 4
        If day = 3 Then temp1 = 3
        If day = 4 Then temp1 = 2
        If day = 5 Then temp1 = 1
        Return temp1
    End Function
    Public Function GetParshaNew2(ByRef ParshaEng As String)
        'uses the JewishCalendar.cs name to get the heb name - this usess the new JewishCalendar.cs from 2021 - but the is an issue with some years like תשע"ח תשפ"ט
        Dim hebrewParshiyos As String() = {"", "בראשית", "נח", "לך לך", "וירא", "חיי שרה", "תולדות", "ויצא", "וישלח", "וישב", "מקץ", "ויגש", "ויחי", "שמות", "וארא", "בא", "בשלח", "יתרו", "משפטים", "תרומה", "תצוה", "כי תשא", "ויקהל", "פקודי", "ויקרא", "צו", "שמיני", "תזריע", "מצרע", "אחרי מות", "קדושים", "אמור", "בהר", "בחקתי", "במדבר", "נשא", "בהעלתך", "שלח לך", "קרח", "חוקת", "בלק", "פינחס", "מטות", "מסעי", "דברים", "ואתחנן", "עקב", "ראה", "שופטים", "כי תצא", "כי תבוא", "ניצבים", "וילך", "האזינו", "ויקהל פקודי", "תזריע מצרע", "אחרי מות קדושים", "בהר בחקתי", "חוקת בלק", "מטות מסעי", "ניצבים וילך"}
        Dim EngParshiyos As String() = {"NONE", "BERESHIS", "NOACH", "LECH_LECHA", "VAYERA", "CHAYEI_SARA", "TOLDOS", "VAYETZEI", "VAYISHLACH", "VAYESHEV", "MIKETZ", "VAYIGASH", "VAYECHI", "SHEMOS", "VAERA", "BO", "BESHALACH", "YISRO", "MISHPATIM", "TERUMAH", "TETZAVEH", "KI_SISA", "VAYAKHEL", "PEKUDEI", "VAYIKRA", "TZAV", "SHMINI", "TAZRIA", "METZORA", "ACHREI_MOS", "KEDOSHIM", "EMOR", "BEHAR", "BECHUKOSAI", "BAMIDBAR", "NASSO", "BEHAALOSCHA", "SHLACH", "KORACH", "CHUKAS", "BALAK", "PINCHAS", "MATOS", "MASEI", "DEVARIM", "VAESCHANAN", "EIKEV", "REEH", "SHOFTIM", "KI_SEITZEI", "KI_SAVO", "NITZAVIM", "VAYEILECH", "HAAZINU", "VAYAKHEL_PEKUDEI", "TAZRIA_METZORA", "ACHREI_MOS_KEDOSHIM", "BEHAR_BECHUKOSAI", "CHUKAS_BALAK", "NITZAVIM_VAYEILECH", "MATOS_MASEI"}
        Dim ParshaHeb As String = ""

        For i = 0 To UBound(EngParshiyos)
            If ParshaEng = EngParshiyos(i) Then ParshaHeb = hebrewParshiyos(i)
        Next
        Return ParshaHeb
    End Function
    '=================================================================================
    'for below add new function to JewishCalendar.cs - 
    '//public int[] GetweekforParshaAyg(DateTime date, bool inIsrael)
    '//{
    '//    int[] MyArray = new int[2];
    '//    MyArray[0] = GetParshaYearType(date, inIsrael);
    '//    int roshHashanaDayOfWeek = (int)base.GetDayOfWeek(base.ToDateTime(base.GetYear(date), 1, 1, 14, 0, 0, 0));
    '//    TimeSpan daysSinceRoshHashana = date - base.ToDateTime(base.GetYear(date), 1, 1, 14, 0, 0, 0);
    '//    int day = roshHashanaDayOfWeek + daysSinceRoshHashana.Days + 1;
    '//    MyArray[1] = day / 7;
    '//    return MyArray;
    '//}
    'sample using in vb
    '0 is the ParshaYearType 1 is week
    'hebday.Text = hebday.Text & " " & GetParshaNew(jc.GetweekforParshaAyg(engdate.Value, False)(0), jc.GetweekforParshaAyg(engdate.Value, False)(1))
    '=================================================================================

End Class

