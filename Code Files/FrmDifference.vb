Public Class FrmDifference

    Private Sub FormDifference_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New System.Drawing.Point(Frminfo.Location.X + ((Frminfo.Width - Me.Width) / 2), Frminfo.Bounds.Bottom - (Me.Height + 45))

        GroupBox1.Font = MemoryFonts.GetFont(1, 12, FontStyle.Regular)
        GroupBox2.Font = MemoryFonts.GetFont(1, 12, FontStyle.Regular)
        tbTimeR.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        CbTimeA.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        CbTimeB.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        tbTimeA.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        tbTimeB.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        CBMday.Font = MemoryFonts.GetFont(0, 8.75, FontStyle.Regular)

        If Frminfo.mHebrewMenus.Checked = True Then
            GroupBox1.RightToLeft = 1
            GroupBox2.RightToLeft = 1
            CbTimeA.RightToLeft = 0
            CbTimeB.RightToLeft = 0
            tbTimeA.RightToLeft = 0
            tbTimeB.RightToLeft = 0
            tbTimeR.RightToLeft = 0

            GroupBox1.Text = "זמן א"
            GroupBox2.Text = "זמן ב"
            LabelDifference.Text = "הבדל"
            LabelDifference.Font = MemoryFonts.GetFont(0, 8.75, FontStyle.Regular)
        Else
            GroupBox1.RightToLeft = 0
            GroupBox2.RightToLeft = 0
            GroupBox1.Text = "Zman 1"
            GroupBox2.Text = "Zman 2"
            LabelDifference.Text = "Difference"
            LabelDifference.Font = MemoryFonts.GetFont(0, 7.75, FontStyle.Regular)
        End If

        'for Compare Zmanim dropboxlists 
        CbTimeA.Items.Clear()
        CbTimeB.Items.Clear()
        For Each s As String In varZmanimFunc
            CbTimeA.Items.Add(s)
            CbTimeB.Items.Add(s)
        Next
    End Sub
    Private Sub CbTimeA_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbTimeA.DropDown, CbTimeB.DropDown
        SetAutoComplete(sender, False)
    End Sub
    Private Sub CbTimeA_DropDownclosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbTimeA.DropDownClosed, CbTimeB.DropDownClosed
        SetAutoComplete(sender, True)
        If sender.SelectedIndex > -1 Then run_difference()
    End Sub
    Private Sub CbTimeA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbTimeA.SelectedIndexChanged
        run_difference()
    End Sub
    Private Sub CbTimeB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbTimeB.SelectedIndexChanged
        run_difference()
    End Sub
    Private Sub CBMday_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBMday.CheckedChanged
        run_difference()
    End Sub
    Public Sub SetAutoComplete(ByVal sender As Object, ByVal AutoComplete As Boolean)
        If AutoComplete = True Then
            sender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Else
            sender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None 'false
        End If
    End Sub

    'hidden
    '--------------------------------------------------------------
    Private Sub btSunOffset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSunOffset.Click
        'check dates that SunOffset is not applicable 

        If TextBox1.Text = "" Then TextBox1.Text = "16.1"

        Dim timeZone As ITimeZone = New PZmanimTimeZone(varZmanTimeZone)
        Dim location As New GeoLocation(Frminfo.cbLocationList.Text, Frminfo.tblatitude.Text, Frminfo.tblongitude.Text, 0, timeZone)


        Dim offsetZenith As Double = TextBox1.Text.ToString + varCZC.GEOMETRIC_ZENITH
        Dim my_date As Date = #1/1/2020#
        Dim temp1 As String
        Dim sp As Date
        Dim ep As Date

        For i = 1 To 365
            my_date = my_date.AddDays(1)
            varCZC = New ComplexZmanimCalendar(my_date, location)
            temp1 = varCZC.GetSunsetOffsetByDegrees(offsetZenith).ToString
            If temp1 = "" Then
                ep = my_date
                If sp < #1/1/2000# Then sp = my_date
            End If
        Next

        If sp > #1/1/2000# Then
            MsgBox(TextBox1.Text.ToString & vbCrLf & "תחת האופק יש בו בעיה מיום" &
                    vbCrLf & sp.ToString("MM/dd") & "=" & sp.ToLongDateString & vbCrLf &
                    "עד יום" & vbCrLf & ep.ToString("MM/dd") & "=" & ep.ToLongDateString)
        Else
            MsgBox("שייך כאן כל השנה")
        End If
    End Sub
    Private Sub btSystemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSystemInfo.Click
        FileOpen(1, Environment.CurrentDirectory & "\SystemInfo.txt", OpenMode.Output)

        Print(1, "=====System Input Languages=====" & vbCr)
        Print(1, "CurrentInputLanguage: " & InputLanguage.CurrentInputLanguage.LayoutName & vbCr)
        Dim i As Integer = 1
        For Each ILanguage As InputLanguage In InputLanguage.InstalledInputLanguages
            Print(1, "-- " & i & " --" & vbCr & "InputLanguage.LayoutName: " & ILanguage.LayoutName & vbCr & "Culture.DisplayName: " & ILanguage.Culture.DisplayName & vbCr & "Culture.EnglishName: " & ILanguage.Culture.EnglishName & vbCr & "Culture.NativeName: " & ILanguage.Culture.NativeName & vbCr & "Culture.Name: " & ILanguage.Culture.Name & vbCr)
            i += 1
        Next

        Dim AllTimeZones As ObjectModel.ReadOnlyCollection(Of TimeZoneInfo) = TimeZoneInfo.GetSystemTimeZones
        Print(1, vbCr & "=====System Time Zones=====" & vbCr)
        For Each TZ In AllTimeZones
            Print(1, TZ.DisplayName & vbCr & "Id: " & TZ.Id & vbCr & "UtcOffset:" & TZ.BaseUtcOffset.TotalHours & vbCr)
        Next
        FileClose(1)

        Process.Start(Environment.CurrentDirectory & "\SystemInfo.txt")

    End Sub
    Private Sub make_dat_file_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles make_dat_file.Click
        Dim alist As New SettingsCollection

        Dim filename As String
        Dim fd As New OpenFileDialog

        fd.Title = "Selcet A File"
        fd.InitialDirectory = Environment.CurrentDirectory
        fd.Filter = "Xml files (*.xml)|*.xml*|All files (*.*)|*.*"
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            filename = fd.FileName
        End If
        If My.Computer.FileSystem.FileExists(filename) = False Then MsgBox("File not found") : Exit Sub

        alist = SerializableData.Load(filename, GetType(SettingsCollection))


        'remove hebname is eng
        For Each place As aLocation In alist.Location
            If Regex.Match(place.HebName, "[A-z]").Success = True Then
                place.HebName = ""
            End If
        Next

        Debug.Print(alist.Location.Count)

        Dim counter As Integer
        Dim write As New BinaryWriter(File.Open(filename & ".DAT", FileMode.Create))
        With write
            .Write(alist.Location.Count)
            For Each place As aLocation In alist.Location
                counter += 1
                Debug.Print(counter)
                Debug.Print(place.EngName)
                .Write(place.Country)
                .Write(place.Elevation)
                .Write(place.Latitude)
                .Write(place.Longitude)
                .Write(place.EngName)
                .Write(place.HebName)
                .Write(place.TimeOffset)
                .Write(place.TimeZoneID)
            Next
            .Close()
        End With


    End Sub
    Private Sub AddTimeZoneId_Click(sender As Object, e As EventArgs) Handles btAddTimeZoneId.Click
        'Add timezone to each locationm in location list using GeoTimeZone.TimeZoneLookup
        Dim MyStopwatch As New Stopwatch
        MyStopwatch.Start()
        Dim IANATimezone As String
        Dim WinTimezone As String

        For Each Li As aLocation In varEngPlaceList.OrderBy(Function(temp) temp.EngName)
            IANATimezone = GeoTimeZone.TimeZoneLookup.GetTimeZone(Li.Latitude, Li.Longitude).Result
            WinTimezone = TimeZoneConverter.TZConvert.IanaToWindows(IANATimezone)
            Li.TimeZoneID = WinTimezone
        Next

        varEngPlaceList.Save(varLocationsFile)

        Debug.Print("Time elapsed: {0}", MyStopwatch.Elapsed)
    End Sub

    Private Sub Butexit_Click(sender As Object, e As EventArgs) Handles Butexit.Click
        Me.Close()
    End Sub
End Class