Class AddedGets
    'need to use a new instacnce of CZC when date sent in
    Private TempCZC As ComplexZmanimCalendar
    Public Function ZmanGetMisheyakir50(DateIn As Date, LocationIn As GeoLocation)
        TempCZC = New ComplexZmanimCalendar(DateIn, LocationIn)
        If Frminfo.mUseUSNO.Checked = False Then
            TempCZC.AstronomicalCalculator = New NOAACalculator()
        Else
            TempCZC.AstronomicalCalculator = New ZmanimCalculator() ' USNO older
        End If

        Try
            Return CDate(TempCZC.GetSunrise.ToString).AddMinutes(-50).ToLongTimeString
        Catch ex As Exception
            Return Nothing
        End Try
        'with time formats
        'MsgBox(CDate(CallByName(Me, "ZmanGetMisheyakir50", CallType.Get)).ToString("tt h:mm:ss"))
    End Function
    Public Function ZmanGetPlagHamincha8Point5Degrees(DateIn As Date, LocationIn As GeoLocation)
        Try
            Dim mydate As Time
            'was giving truble with out .ToString("H:mm:ss")
            Return CDate(TempCZC.GetPlagHamincha(TempCZC.GetSunriseOffsetByDegrees(90 + 8.5), TempCZC.GetSunsetOffsetByDegrees(90 + 8.5))).ToString("H:mm:ss")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ZmanGetBeinHashmases8Point5Degrees13Min(DateIn As Date, LocationIn As GeoLocation)
        Try
            Return CDate(TempCZC.GetSunsetOffsetByDegrees(90 + 8.5).ToString).AddMinutes(-13.5).ToLongTimeString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ZmanGetBeinHashmases8Point5Degrees5Point3(DateIn As Date, LocationIn As GeoLocation)
        Try
            Dim myTimeSpan = CDate(TempCZC.GetTzaisGeonim8Point5Degrees().ToString) - CDate(TempCZC.GetSunset().ToString)
            Return CDate(TempCZC.GetSunsetOffsetByDegrees(90 + 8.5).ToString).AddMilliseconds(-(myTimeSpan.TotalMilliseconds / 5.3333)).ToLongTimeString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class

