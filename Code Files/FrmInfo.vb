'MemoryFonts.GetFont(0, = ArialUnicodeCompact
'MemoryFonts.GetFont(1, = Varela Round
Public Class Frminfo
    Private Sub Frminfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'see switch [/s] for Scheduler in ApplicationEvents.vb
        rtbHebrewDate.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
        rtbParsha.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
        dpEngdate.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
        cbLocationList.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        CbTimeZone.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
        tblatitude.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tblongitude.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tbElevation.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tbzone.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tbcountry.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        DataGridView1.Font = MemoryFonts.GetFont(0, 9.5, FontStyle.Bold)
        GroupBox1.Font = MemoryFonts.GetFont(1, 12, FontStyle.Regular)
        GroupBox2.Font = MemoryFonts.GetFont(1, 12, FontStyle.Regular)

        LoadSettingsandVariables()
        If Screen.PrimaryScreen.Bounds.Contains(Me.Bounds) = False Then
            Me.CenterToScreen()
        End If

        'Date picker set to CurrentCulture
        Dim CultureDateFL = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern(0)
        dpEngdate.CustomFormat = "  dddd   " & If(CultureDateFL = "M", "MM / dd / yyyy", "dd / MM / yyyy")

        change_hebdate()
        DataGridView1.Select()
        TimerLocationsLoad.Enabled = True 'used to load locations after form is open
    End Sub
    Private Sub Frminfo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        varSC.DataGridSizeH = DataGridView1.Size.Height 'doning it in Frminfo_Resize made issue
        varSC.LastSelectedIndex = cbLocationList.SelectedIndex
        'save SettingsCollection
        varSC.Save(varUserFile)
    End Sub
    '==============================
    'move the form without the title bar
    'Dim Pos As Point
    'Private Sub frminfo_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
    '    'If e.Button = Windows.Forms.MouseButtons.Left Then
    '    '    Me.Location += Control.MousePosition - Pos
    '    'End If
    '    'Pos = Control.MousePosition
    'End Sub
    '==============================
    Private Sub Frminfo_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'resize StatusLabel
        StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
        'Debug.Print(Me.Size.ToString)
        varSC.SizeH = Me.Height
        varSC.SizeW = Me.Width
    End Sub
    Private Sub Frminfo_Move(sender As Object, e As EventArgs) Handles Me.Move
        varSC.LocationY = Location.Y
        varSC.LocationX = Location.X
    End Sub
    Private Sub Frminfo_MouseEnter(sender As Object, e As EventArgs) Handles Me.MouseEnter, MyBase.MouseEnter, Me.Activated, Me.GotFocus, GroupBox1.MouseEnter, GroupBox2.MouseEnter, DataGridView1.MouseEnter, StatusStrip1.Enter, dpEngdate.GotFocus, rtbHebrewDate.GotFocus, cbLocationList.GotFocus, tblatitude.GotFocus, tblongitude.GotFocus, tbcountry.GotFocus, tbElevation.GotFocus, tbzone.GotFocus
        varMouseEnter = True
    End Sub
    Private Sub Frminfo_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave, MyBase.MouseLeave, Me.LostFocus, Me.Deactivate
        varMouseEnter = False
    End Sub
    Private Sub TimerTransparency_Tick(sender As Object, e As EventArgs) Handles TimerTransparency.Tick
        'If varSC.TransparencyValue = 1 Then Exit Sub

        'Me.MouseLeave did not respond to fast movements, using Bounds.Contains
        'expend the top and Bottom Bounds a bit for toolstrip and border
        Dim NewBounds = Me.Bounds
        NewBounds.Y -= 10
        NewBounds.Height += 10

        'making it Transparent when out of Bounds 
        If NewBounds.Contains(Cursor.Position) = False Then Me.Opacity = varSC.TransparencyValue 'Or varMouseEnter = False 'or MouseLeave

        'turning it back on only when mouse over and MouseEnter
        'add to check if MouseEnter was triggered - don't show if mouse is over something else
        If NewBounds.Contains(Cursor.Position) And varMouseEnter = True Then Me.Opacity = 1

        'for time on top of form
        If varSC.ShowTimesOnStatusBar = False Then
            Me.Text = "זמנים"
            StatusLabel.Text = ""
            Exit Sub
        End If

        If varZmanTimeZone Is Nothing Then Exit Sub
        Dim TimeFormat As String = "h:mm:ss"
        If varSC.Clock24Hour = True Then TimeFormat = "H:mm:ss"
        'Me.Text = DateAndTime.Now.ToString(timeFormt)
        Me.Text = TimeZoneInfo.ConvertTime(Now(), varZmanTimeZone).ToString(TimeFormat) & If(varSC.Clock24Hour = False, ChrW(&H200E) & " " & TimeZoneInfo.ConvertTime(Now(), varZmanTimeZone).ToString("tt"), "")

    End Sub

    Private Sub Butexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Butexit.Click
        Me.Close()
    End Sub
    Private Sub TimerLocationsLoad_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerLocationsLoad.Tick
        'used to load locations after form is open
        TimerLocationsLoad.Enabled = False
        LoadPlaceLists()

        If varSC.LastSelectedIndex <> "-1" Then
            change_place(varSC.LastSelectedIndex)
        End If

        '''till now Place_orDate_changed() and change_zman() did not run for every seting
        varFinishedLoading = True
        Place_orDate_changed()

        'for showing the time and time left till zman
        TimerStatusLabel.Enabled = True
    End Sub
    Private Sub TimerStatusLabel_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerStatusLabel.Tick
        'time at top of form is set by TimerTransparency
        'set Interval to Normal 
        TimerStatusLabel.Interval = 250

        Dim TimeFormat As String = "h:mm:ss tt"
        If varSC.Clock24Hour = True Then TimeFormat = "H:mm:ss"

        'exit if the is no zmanim
        If DataGridView1.RowCount < 2 Then Exit Sub
        Dim num As Integer

        If DataGridView1.SelectedRows.Count < 1 Then
            num = 0
        Else
            num = DataGridView1.SelectedRows(0).Index
        End If
        If DataGridView1.Rows(num).Cells.Count < 3 Then Exit Sub

        Dim myzman, DataGridZman As Date
        Dim myTimeSpan As TimeSpan

        If DataGridView1(2, num).Value = "" Then Exit Sub

        Dim Counter As Integer = 0
        If varSC.ColorZman = True Then
            For Each Row In DataGridView1.Rows
                Counter += 1
                'checking for empty string
                If Row.cells(2).Value = "" Then Exit For
                'skiping ShaahZmanis
                If InStr(varSC.Zmanim.Item(Counter - 1).FunctionName, "ShaahZmanis") Then Exit For
                DataGridZman = Row.cells(2).Value.Replace(ChrW(&H200E), "")
                myTimeSpan = DataGridZman.TimeOfDay - TimeZoneInfo.ConvertTime(Now(), varZmanTimeZone).TimeOfDay 'DateAndTime.TimeOfDay
                Try
                    If myTimeSpan.TotalMilliseconds < 0 Then Row.cells(2).Style.ForeColor = Color.Red '"עבר"
                    If myTimeSpan.TotalMilliseconds > 0 Then Row.cells(2).Style.ForeColor = Color.Green '"נשאר"
                Catch ex As Exception
                End Try
            Next
        End If

        If varSC.ShowTimesOnStatusBar = False Then Exit Sub
        'Remove RTL ChrW(&H200E) if the is 
        myzman = DataGridView1(2, num).Value.Replace(ChrW(&H200E), "")
        myTimeSpan = myzman.TimeOfDay - TimeZoneInfo.ConvertTime(Now(), varZmanTimeZone).TimeOfDay

        If myTimeSpan.TotalMilliseconds > 0 Then
            varSavedStatusLabel = ChrW(&H200E) & myzman.ToString(TimeFormat) & " \ " & myTimeSpan.ToString("h\:mm\:ss")
        Else
            varSavedStatusLabel = ChrW(&H200E) & myzman.ToString(TimeFormat) & " \ -" & myTimeSpan.ToString("h\:mm\:ss")
        End If
        'set autosize on StatusLabel to true and on statusstrip to true
        StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)

    End Sub
    'If ListView1.Items.Count < 1 Then Exit Sub
    ' ListView1.SelectedItems.Count = 0 Then
    'ListView1.FocusedItem.Index
    'If ListView1.Items(num).SubItems.Count < 2 Then Exit Sub
    'ListView1.Items(num).SubItems(1).Text.Replace(ChrW(&H200E), "")

    Private Sub cbLocationList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLocationList.SelectedIndexChanged
        change_place(cbLocationList.SelectedIndex)
        'removed, this will set it to 0 when form loads
        'varSC.LastSelectedIndex = cbLocationList.SelectedIndex
    End Sub
    Private Sub cbLocationList_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLocationList.DropDown
        SetAutoComplete(sender, False)
    End Sub
    Private Sub cbLocationList_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLocationList.DropDownClosed
        SetAutoComplete(sender, True)
        If cbLocationList.SelectedIndex > -1 Then
            change_place(cbLocationList.SelectedIndex)
        End If
    End Sub
    Private Sub cbLocationList_TextUpdate(sender As Object, e As EventArgs) Handles cbLocationList.TextUpdate
        'to save for use in SaveChanges 
        If cbLocationList.SelectedIndex > -1 Then varSelectedIndexBeforChange = cbLocationList.SelectedIndex
        'Debug.Print(cbLocationList.SelectedIndex & " - " & varSelectedIndexBeforChange)
    End Sub
    Public Sub SetAutoComplete(ByVal sender As Object, ByVal AutoComplete As Boolean)
        If AutoComplete = True Then
            sender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Else
            sender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None 'false
        End If
    End Sub
    Private Sub cbLocationList_Enter(sender As Object, e As EventArgs) Handles cbLocationList.Enter
        If varSC.ChangeKeybordLayout = False Then Exit Sub
        Try
            varSavedInputLanguage = InputLanguage.CurrentInputLanguage
            For Each ILanguage As InputLanguage In InputLanguage.InstalledInputLanguages
                If varSC.PlaceListInHebrew = True Then
                    If Not InStr(InputLanguage.CurrentInputLanguage.Culture.Name, "he-") And InStr(ILanguage.Culture.Name, "he-") Then
                        InputLanguage.CurrentInputLanguage = ILanguage
                        Exit For
                    End If
                Else
                    If Not InStr(InputLanguage.CurrentInputLanguage.Culture.Name, "en-") And InStr(ILanguage.Culture.Name, "en-") Then
                        InputLanguage.CurrentInputLanguage = ILanguage
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub cbLocationList_Leave(sender As Object, e As EventArgs) Handles cbLocationList.Leave
        If varSC.ChangeKeybordLayout = False Then Exit Sub
        Try
            InputLanguage.CurrentInputLanguage = varSavedInputLanguage
        Catch ex As Exception
        End Try
    End Sub
    Private Sub CbTimeZone_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbTimeZone.DropDown
        SetAutoComplete(sender, False)
        'load list for this offset
        CbTimeZone.Items.Clear()

        If tbzone.Text <> "" Then
            Dim AllTimeZones As ObjectModel.ReadOnlyCollection(Of TimeZoneInfo) = TimeZoneInfo.GetSystemTimeZones
            For Each TZ In AllTimeZones
                If TZ.BaseUtcOffset = New TimeSpan(tbzone.Text, 0, 0) Then
                    CbTimeZone.Items.Add(TZ.DisplayName)
                End If
            Next
        End If
    End Sub
    Private Sub CbTimeZone_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLocationList.DropDownClosed
        SetAutoComplete(sender, True)
    End Sub
    Private Sub CbTimeZone_TextChanged(sender As Object, e As EventArgs) Handles CbTimeZone.TextChanged
        'Get ID from DisplayName
        varZmanTimeZone = Nothing
        Dim AllTimeZones As ObjectModel.ReadOnlyCollection(Of TimeZoneInfo) = TimeZoneInfo.GetSystemTimeZones
        For Each TZ In AllTimeZones
            If TZ.DisplayName = CbTimeZone.Text Then
                varZmanTimeZone = TZ
                Exit For
            End If
        Next
        Place_orDate_changed()
        'CbTimeZone.Text = ZmanTimeZone.DisplayName
    End Sub
    Private Sub CbTimeZone_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CbTimeZone.KeyPress
        e.Handled = True
    End Sub
    Private Sub engdate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpEngdate.ValueChanged
        change_hebdate()
        Place_orDate_changed()
    End Sub
    Private Sub rtbHebrewDate_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtbHebrewDate.MouseClick
        'Selects all
        'rtbHebrewDate.[Select](0, Len(rtbHebrewDate.Text))
        If varSC.ChangeKeybordLayout = False Then Exit Sub
        Try
            varSavedInputLanguage = InputLanguage.CurrentInputLanguage
            For Each ILanguage As InputLanguage In InputLanguage.InstalledInputLanguages
                If Not InStr(InputLanguage.CurrentInputLanguage.Culture.Name, "he-") And InStr(ILanguage.Culture.Name, "he-") Then
                    InputLanguage.CurrentInputLanguage = ILanguage
                    Exit For
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub rtbHebrewDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rtbHebrewDate.KeyPress
        If e.KeyChar = Chr(13) Then
            rtbHebrewDate.Text = rtbHebrewDate.Text.Replace(vbCr, "").Replace(vbLf, "")
            parse_hebdate()
        End If
    End Sub
    Private Sub rtbHebrewDate_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtbHebrewDate.Leave
        parse_hebdate()
        If varSC.ChangeKeybordLayout = False Then Exit Sub
        Try
            InputLanguage.CurrentInputLanguage = varSavedInputLanguage
        Catch ex As Exception
        End Try
    End Sub
    Private Sub rtbParsha_KeyPress(sender As Object, e As KeyPressEventArgs) Handles rtbParsha.KeyPress
        e.KeyChar = ""
    End Sub
    Private Sub rtbDafYomi_KeyPress(sender As Object, e As KeyPressEventArgs)
        e.KeyChar = ""
    End Sub
    Private Sub rtbHoliday_KeyPress(sender As Object, e As KeyPressEventArgs)
        e.KeyChar = ""
    End Sub
    Private Sub tblatitude_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tblatitude.Leave
        Place_orDate_changed()
    End Sub
    Private Sub tblongitude_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tblongitude.Leave
        Place_orDate_changed()
    End Sub
    Private Sub tblatitude_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tblatitude.KeyPress
        If e.KeyChar = Chr(13) Then Place_orDate_changed()
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." AndAlso Not e.KeyChar = "-" Then e.KeyChar = ""
    End Sub
    Private Sub tblongitude_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tblongitude.KeyPress
        If e.KeyChar = Chr(13) Then Place_orDate_changed()
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." AndAlso Not e.KeyChar = "-" Then e.KeyChar = ""
    End Sub

    Private Sub mOpenSchedule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mOpenSchedule.Click
        If Me.TopMost = True Then FrmSchedule.TopMost = True
        FrmSchedule.Show()
    End Sub
    Private Sub mOpenCompere_Click(sender As Object, e As EventArgs) Handles mOpenCompare.Click
        If Me.TopMost = True Then FrmDifference.TopMost = True
        FrmDifference.Show()
    End Sub
    Private Sub mExport_Click(sender As Object, e As EventArgs) Handles mExport.Click
        If Me.TopMost = True Then FrmExport.TopMost = True
        FrmExport.Show()
    End Sub
    Private Sub mInfoHelp_Click(sender As Object, e As EventArgs) Handles mInfoHelp.Click
        Try
            Process.Start("https://github.com/NykUser/MyZman")
        Catch ex As Exception
            Process.Start("IExplore.exe", "https://github.com/NykUser/MyZman")
        End Try
    End Sub
    Public Sub StayOnTopToolStripMenuItem_Click() Handles mStayOnTopToolStripMenuItem.Click
        If mStayOnTopToolStripMenuItem.Checked = True Then
            Me.TopMost = 1
        Else
            Me.TopMost = 0
        End If
        varSC.StayOnTop = mStayOnTopToolStripMenuItem.Checked
    End Sub
    Private Sub mUseUSNO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mUseUSNO.Click
        varSC.UseOlderUsnoAlgorithm = mUseUSNO.Checked
        Place_orDate_changed()
    End Sub
    Private Sub mCalculateElevation_Click(sender As Object, e As EventArgs) Handles mCalculateElevation.Click
        varSC.CalculateElevation = mCalculateElevation.Checked
        Place_orDate_changed()
    End Sub
    Private Sub mDontChangeKeybordLayout_Click(sender As Object, e As EventArgs) Handles mChangeKeybordLayout.Click
        varSC.ChangeKeybordLayout = mChangeKeybordLayout.Checked
    End Sub
    Private Sub mShowTooltips_Click(sender As Object, e As EventArgs) Handles mShowTooltips.Click
        varSC.ShowTooltips = mShowTooltips.Checked
        mHebrewMenus_Click()
    End Sub
    Private Sub mDisplayDafYomi_Click(sender As Object, e As EventArgs) Handles mDisplayDafYomi.Click
        varSC.DisplayDafYomi = mDisplayDafYomi.Checked
        change_hebdate()
    End Sub
    Private Sub mAskWhenChanging_Click(sender As Object, e As EventArgs) Handles mAskWhenChanging.Click
        varSC.AskWhenChanging = mAskWhenChanging.Checked
    End Sub
    Private Sub mBackUpWhenChanging_Click(sender As Object, e As EventArgs) Handles mBackUpWhenChanging.Click
        varSC.BackUpWhenChanging = mBackUpWhenChanging.Checked
    End Sub
    Private Sub mShowTimesOnStatusBar_Click(sender As Object, e As EventArgs) Handles mShowTimesOnStatusBar.Click
        varSC.ShowTimesOnStatusBar = mShowTimesOnStatusBar.Checked
    End Sub
    Private Sub m24HourFormatTime_Click(sender As Object, e As EventArgs) Handles m24HourFormatTime.Click
        varSC.Clock24Hour = m24HourFormatTime.Checked
        change_zman()
    End Sub
    Private Sub mIsraeliYomTov_Click(sender As Object, e As EventArgs) Handles mIsraeliYomTov.Click
        varSC.IsraeliYomTov = mIsraeliYomTov.Checked
        change_hebdate()
    End Sub
    Private Sub mLineBetweenZmanim_Click(sender As Object, e As EventArgs) Handles mLineBetweenZmanim.Click
        varSC.LineBetweenZmanim = mLineBetweenZmanim.Checked
        change_zman()
    End Sub
    Private Sub rbtLocationContexOpen_Click(sender As Object, e As EventArgs) Handles rbtLocationContexOpen.Click
        If varSC.HebrewMenus = True Then
            LocationContextMenu.Show(rbtLocationContexOpen, 15, rbtLocationContexOpen.Height)
        Else
            LocationContextMenu.Show(rbtLocationContexOpen, 0, rbtLocationContexOpen.Height)
        End If
    End Sub
    Private Sub mAddZman_Click(sender As Object, e As EventArgs) Handles mAddZman.Click
        Dim NewZman As aZman = New aZman
        NewZman.DisplayName = "הזן פרטי הזמן"
        NewZman.FunctionName = "GetAlos120"
        NewZman.ObjectName = "varCZC"
        If DataGridView1.Rows.Count > 0 Then
            varSC.Zmanim.Items.Insert(DataGridView1.SelectedRows(0).Index + 1, NewZman)
            varDataGridNumRow = DataGridView1.SelectedRows(0).Index + 1
        Else
            varSC.Zmanim.Items.Insert(0, NewZman)
            varDataGridNumRow = 0
        End If
        Place_orDate_changed()
        DataGridView1.BeginEdit(True)
    End Sub
    Private Sub mResetZmanimList_Click(sender As Object, e As EventArgs) Handles mResetZmanimList.Click
        If varSC.AskWhenChanging = True Then
            Dim Response
            Using New Centered_MessageBox(Me, "MouseCenter")
                If varSC.HebrewMenus = True Then
                    Response = MsgBox("!שים לב" & vbCr & ".פעולה זו תאפס את כל השינויים שבוצעו על ידך ברשימת הזמנים" & vbCr & "?האם אתה בטוח שברצונך להמשיך", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight)
                Else
                    Response = MsgBox("This Will Remove Changes & Additions Made To The Zmanim List." & vbCr & "Sure You Want To Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                End If
            End Using
            If Response = vbNo Then Exit Sub
        End If
        If varSC.BackUpWhenChanging = True Then varSC.Save(varUserFile & ".Bak " & Now.ToString("M-d-yy H.m"))

        UseDefultzmnaim()
        varSC.Save(varUserFile)
        Place_orDate_changed()
    End Sub
    Private Sub mResetSettings_Click(sender As Object, e As EventArgs) Handles mResetSettings.Click
        If varSC.AskWhenChanging = True Then
            Dim Response
            'Using New Centered_MessageBox(Me, "MouseCenter")
            Using New Centered_MessageBox(Me, "ParentCenter")
                If varSC.HebrewMenus = True Then
                    Response = MsgBox("!שים לב" & vbCr & ".פעולה זו תסיר את כל הגדרות המשתמש" & vbCr & "?האם אתה בטוח שברצונך להמשיך", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight)
                Else
                    Response = MsgBox("This Will Remove All User Settings." & vbCr & "Sure You Want To Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                End If
            End Using
            If Response = vbNo Then Exit Sub
        End If
        If varSC.BackUpWhenChanging = True Then varSC.Save(varUserFile & ".Bak " & Now.ToString("M-d-yy H.m"))

        If varSC.HideLocationBox = True Then
            mHideLocationInfo.Checked = False
            mHideLocationInfo_Click()
        End If

        'save Locations & Zmanim
        Dim tempZmanim As New List(Of aZman)
        Dim tempLocation As New List(Of aLocation)
        varSC.Zmanim.Items.ForEach(Sub(x) tempZmanim.Add(x))
        varSC.Location.Items.ForEach(Sub(x) tempLocation.Add(x))
        varSC = New SettingsCollection
        tempZmanim.ForEach(Sub(x) varSC.Zmanim.Add(x))
        tempLocation.ForEach(Sub(x) varSC.Location.Add(x))

        varSC.Save(varUserFile)

        Me.Hide()
        LoadSettingsandVariables()
        Me.Size = New System.Drawing.Size(305, 818)
        DataGridView1.Size = New System.Drawing.Size(265, 529)
        DataGridView1.Columns(1).Width = 160
        DataGridView1.Columns(2).Width = 100
        Me.CenterToScreen()
        Me.Show()

        TimerLocationsLoad.Enabled = True
    End Sub
    Private Sub mGetCurrnetLocation_Click(sender As Object, e As EventArgs) Handles mGetCurrnetLocation.Click
        Dim MyGeowatcher As GeoCoordinateWatcher = New GeoCoordinateWatcher

        'Dim MyStopwatch As New Stopwatch
        'MyStopwatch.Start()

        MyGeowatcher.TryStart(True, TimeSpan.FromSeconds(5))

        TimerStatusLabel.Enabled = False
        'varSavedStatusLabel = "Working On Getting Current GeoLocation"
        'StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
        StatusLabel.Text = ""
        ToolStripProgressBar1.Maximum = 790000
        ToolStripProgressBar1.Value = 0
        ToolStripProgressBar1.Visible = True

        Dim WaitUntil As Date = Now.AddSeconds(5)
        Do Until Now > WaitUntil
            If ToolStripProgressBar1.Value < 789999 Then ToolStripProgressBar1.Value = ToolStripProgressBar1.Value + 1
            If MyGeowatcher.Status = GeoPositionStatus.Ready Then Exit Do
            System.Windows.Forms.Application.DoEvents()
        Loop
        ToolStripProgressBar1.Visible = False
        'Debug.Print("Time elapsed: {0}", MyStopwatch.Elapsed)

        If MyGeowatcher.Status = GeoPositionStatus.Ready Then
            varLocationSecondTry = False
            Dim GeoCoordinate As GeoCoordinate = MyGeowatcher.Position.Location
            If varSC.PlaceListInHebrew = True Then
                cbLocationList.Text = "מיקום נוכחי | Current Location"
            Else
                cbLocationList.Text = "Current Location | מיקום נוכחי"
            End If
            tblatitude.Text = GeoCoordinate.Latitude
            tblongitude.Text = GeoCoordinate.Longitude
            tbElevation.Text = GeoCoordinate.Altitude
            Dim IANATimezone As String = GeoTimeZone.TimeZoneLookup.GetTimeZone(tblatitude.Text, tblongitude.Text).Result
            Dim WinTimezone As String = TimeZoneConverter.TZConvert.IanaToWindows(IANATimezone)
            varZmanTimeZone = TimeZoneInfo.FindSystemTimeZoneById(WinTimezone)
            CbTimeZone.Text = varZmanTimeZone.DisplayName
            tbzone.Text = TimeZoneInfo.FindSystemTimeZoneById(WinTimezone).BaseUtcOffset.TotalHours
            tbcountry.Text = ""
            varSavedStatusLabel = "Check Location Accuracy, Info Retrieved From Windows"
            If varSC.HebrewMenus = True Then varSavedStatusLabel = "בדוק את דיוק המיקום, המידע התקבל מווינדוס"
            StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
            'wait 5s
            TimerStatusLabel.Interval = 5000
            'Debug.Print(String.Format("Lat: {0}, Long: {1}, H Accuracy: {2}, V Accuracy: {3}, Crse: {4}, Spd: {5}, Alt: {6}", GeoCoordinate.Latitude, GeoCoordinate.Longitude, GeoCoordinate.HorizontalAccuracy, GeoCoordinate.VerticalAccuracy, GeoCoordinate.Course, GeoCoordinate.Speed, GeoCoordinate.Altitude))
        Else
            'Check Windows Location Settings
            If varLocationSecondTry = False Then
                varLocationSecondTry = True
                varSavedStatusLabel = "Was Not Able To Get Current GeoLocation. Try Again"
                If varSC.HebrewMenus = True Then varSavedStatusLabel = "לא הצליח להשיג מיקום גיאוגרפי נוכחי. נסה שוב"
            Else
                varSavedStatusLabel = "Check Windows Location Settings."
                If varSC.HebrewMenus = True Then varSavedStatusLabel = "בדוק את הגדרות המיקום של ווינדוס."
            End If
            StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
            'wait 7s
            TimerStatusLabel.Interval = 7000
        End If

        TimerStatusLabel.Enabled = True
        MyGeowatcher.Dispose()

        'Debug.Print("Position Permission: " & MyGeowatcher.Permission.ToString)
        'Debug.Print("Watcher Status: " & MyGeowatcher.Status.ToString)

    End Sub
    Public Sub mPlaceListInHebrew_Click() Handles mPlaceListInHebrew.Click
        Dim EngName As String = fGetEngName(varSC.PlaceListInHebrew) 'as was befor change
        varSC.PlaceListInHebrew = mPlaceListInHebrew.Checked
        'If varFinishedLoading = False Then Exit Sub
        ClearAndReLoadPlaceLists(EngName)
    End Sub
    Function fGetEngName(HebFirst As Boolean) As String
        Dim EngName As String
        If cbLocationList.Text <> "" Then
            Dim splitname() As String = Split(cbLocationList.Text, "|", -1)
            If splitname.Count > 1 Then ' there is a | in string
                If HebFirst = False Then
                    EngName = Trim(splitname(0))
                Else
                    EngName = Trim(splitname(1))
                End If
            End If
            Return EngName
        Else
            Return Nothing
        End If
    End Function
    Private Sub mSaveLocationChanges_Click(sender As Object, e As EventArgs) Handles mSaveLocationChanges.Click
        'Debug.Print(cbLocationList.SelectedIndex & " - " & varSelectedIndexBeforChange)

        If cbLocationList.SelectedIndex < 0 And varSelectedIndexBeforChange < 0 Then Exit Sub
        Dim num As Integer = varSelectedIndexBeforChange
        If cbLocationList.SelectedIndex > -1 Then num = cbLocationList.SelectedIndex

        Dim EngName, HebName As String
        If varSC.PlaceListInHebrew = True Then
            EngName = varHebPlaceList.Item(num).EngName
            HebName = varHebPlaceList.Item(num).HebName
        Else
            EngName = varEngPlaceList.Item(num).EngName
            HebName = varEngPlaceList.Item(num).HebName
        End If

        If varSC.AskWhenChanging = True Then
            Dim Response
            Using New Centered_MessageBox(Me, "MouseCenter")
                If varSC.HebrewMenus = True Then
                    Response = MsgBox("!שים לב" & vbCr & ":פעולה זו תחליף מידע שמור של" & vbCr & EngName & " - " & ChrW(&H200F) & HebName & vbCr & "?האם אתה בטוח שברצונך להמשיך", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight)
                Else
                    Response = MsgBox("This Will Overwrite: " & EngName & " - " & HebName & " Info!" & vbCr & "Do You Want To Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                End If
            End Using
            If Response = vbNo Then Exit Sub
        End If

        Dim SelectedPlace As aLocation = MakeNewLocationGetForm()

        varSC.Location.Items.Clear()
        If varSC.PlaceListInHebrew = True Then
            varHebPlaceList.Item(num) = SelectedPlace
            For Each Li As aLocation In varHebPlaceList
                varSC.Location.Add(New aLocation(Li))
            Next
        Else
            varEngPlaceList.Item(num) = SelectedPlace
            For Each Li As aLocation In varEngPlaceList
                varSC.Location.Add(New aLocation(Li))
            Next
        End If
        varSC.Save(varUserFile)

        ClearAndReLoadPlaceLists(SelectedPlace.EngName)

        TimerStatusLabel.Enabled = False
        varSavedStatusLabel = "Changes Saved To Location List."
        If varSC.HebrewMenus = True Then varSavedStatusLabel = "שינויים נשמרו ברשימת המיקומים."
        StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
        'wait 5s
        TimerStatusLabel.Interval = 5000
        TimerStatusLabel.Enabled = True
    End Sub
    Private Sub mRemoveLocation_Click(sender As Object, e As EventArgs) Handles mRemoveLocation.Click
        If cbLocationList.Text = "" Then Exit Sub

        If cbLocationList.SelectedIndex < 0 And varSelectedIndexBeforChange < 0 Then Exit Sub
        Dim num As Integer = varSelectedIndexBeforChange
        If cbLocationList.SelectedIndex > -1 Then num = cbLocationList.SelectedIndex

        Dim PlaceBeingRemoved As String = cbLocationList.Text
        If varSC.AskWhenChanging = True Then
            Dim Response
            Using New Centered_MessageBox(Me, "MouseCenter")
                If varSC.HebrewMenus = True Then
                    Response = MsgBox(":אתה בטוח שאתה רוצה למחוק מרשימת המיקומים" & vbCr & cbLocationList.Text, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight)
                Else
                    Response = MsgBox("Are you sure you want to delete " & cbLocationList.Text & " From Locations List?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                End If
            End Using
            If Response = vbNo Then Exit Sub
        End If

        varSC.Location.Items.Clear()
        If varSC.PlaceListInHebrew = True Then
            varHebPlaceList.Items.RemoveAt(num)
            For Each Li As aLocation In varHebPlaceList
                varSC.Location.Add(New aLocation(Li))
            Next
        Else
            varEngPlaceList.Items.RemoveAt(num)
            For Each Li As aLocation In varEngPlaceList
                varSC.Location.Add(New aLocation(Li))
            Next
        End If
        varSC.Save(varUserFile)

        ClearAndReLoadPlaceLists("", num - 1)

        TimerStatusLabel.Enabled = False
        varSavedStatusLabel = PlaceBeingRemoved & " Was Removed From Location List."
        If varSC.HebrewMenus = True Then varSavedStatusLabel = ChrW(&H200E) & PlaceBeingRemoved & ChrW(&H200F) & " הוסר מרשימת המיקומים."
        StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
        'wait 5s
        TimerStatusLabel.Interval = 5000
        TimerStatusLabel.Enabled = True
    End Sub
    Private Sub mAddToUserLocation_Click(sender As Object, e As EventArgs) Handles mAddToUserLocation.Click
        If cbLocationList.Text = "" Then Exit Sub

        Dim splitname() As String = Split(cbLocationList.Text, "|", -1)
        Dim EngName, HebName As String
        If splitname.Count > 1 Then ' there is a | in string
            If varSC.PlaceListInHebrew = True Then
                HebName = Trim(splitname(0))
                EngName = Trim(splitname(1))
            Else
                EngName = Trim(splitname(0))
                HebName = Trim(splitname(1))
            End If
        End If
        For Each Li As aLocation In varEngPlaceList
            If Li.EngName = EngName Or Li.HebName = HebName Then
                Using New Centered_MessageBox(Me, "MouseCenter")
                    If varSC.HebrewMenus = True Then
                        MsgBox(".הרשימה מכילה מיקום עם אותם שמות" & vbCr & ".כדי להחליף השתמש בלחצן שמור שינויי מיקום" & vbCr & ".כדי להוסיף כמיקום חדש שנה את שמו", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight)
                    Else
                        MsgBox("List contains a Location With The Same Names." & vbCr & "To Overwrite Use Button 'Save Location Changes'." & vbCr & "To Add As a New Location Change Its Name.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                    End If
                End Using
                If varSC.PlaceListInHebrew = True Then
                    cbLocationList.Text = "מקום חדש" & " | " & "New Place"
                Else
                    cbLocationList.Text = "New Place" & " | " & "מקום חדש"
                End If

                Exit Sub
            End If
        Next

        Dim SelectedPlace As aLocation = MakeNewLocationGetForm()

        varSC.Location.Add(SelectedPlace)
        varSC.Save(varUserFile)

        ClearAndReLoadPlaceLists(SelectedPlace.EngName)

        TimerStatusLabel.Enabled = False
        varSavedStatusLabel = SelectedPlace.EngName & " | " & SelectedPlace.HebName & " Saved To Location List."
        If varSC.HebrewMenus = True Then varSavedStatusLabel = ChrW(&H200E) & SelectedPlace.EngName & " | " & SelectedPlace.HebName & ChrW(&H200F) & " נשמר ברשימת המיקומים."
        StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
        'wait 5s
        TimerStatusLabel.Interval = 5000
        TimerStatusLabel.Enabled = True
    End Sub
    Private Sub mResetLocationList_Click(sender As Object, e As EventArgs) Handles mResetLocationList.Click
        If varSC.AskWhenChanging = True Then
            Dim Response
            Using New Centered_MessageBox(Me, "MouseCenter")
                If varSC.HebrewMenus = True Then
                    Response = MsgBox("!שים לב" & vbCr & ".פעולה זו תסיר שינויים ותוספות שנעשו ברשימת המיקומים" & vbCr & "?בטוח שאתה רוצה להמשיך", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight)
                Else
                    Response = MsgBox("This Will Remove Changes & Additions Made To The Location List." & vbCr & "Sure You Want To Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                End If
            End Using
            If Response = vbNo Then Exit Sub
        End If

        If varSC.BackUpWhenChanging = True Then varSC.Save(varUserFile & ".Bak " & Now.ToString("M-d-yy H.m"))
        varSC.Location.Items.Clear()
        ClearAndReLoadPlaceLists()
        varSC.Save(varUserFile)

        TimerStatusLabel.Enabled = False
        varSavedStatusLabel = "Locations List Was Reset. Total: " & varSC.Location.Items.Count
        If varSC.HebrewMenus = True Then varSavedStatusLabel = "רשימת המיקומים התאפסה. סך הכל: " & varSC.Location.Items.Count
        StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, StatusLabel.Font, StatusStrip1.Size.Width - 70)
        'wait 5s
        TimerStatusLabel.Interval = 5000
        TimerStatusLabel.Enabled = True

    End Sub
    Private Sub DefaultsMenu_DropDownOpening(sender As Object, e As EventArgs) Handles mDefaultsMenu.DropDownOpening
        'MsgBox(Cbplace.SelectedIndex)
        If cbLocationList.SelectedIndex <> "-1" Then
            mDefaultChangeNew.Visible = True
            If varSC.HebrewMenus = True Then
                mDefaultChangeNew.Text = "שנה ל: " & cbLocationList.Text
            Else
                mDefaultChangeNew.Text = "Change to: " & cbLocationList.Text
            End If

        Else
            mDefaultChangeNew.Visible = False
        End If

        If varSC.DefaultType = "Last" Then
            mDefaultLastUsed.Checked = True
            mDefaultInUse.Visible = False
        Else
            mDefaultLastUsed.Checked = False
            mDefaultInUse.Visible = True
            mDefaultInUse.Checked = True
            mDefaultInUse.Text = varSC.DefaultName
            If varSC.DefaultName <> cbLocationList.Text And cbLocationList.SelectedIndex <> "-1" Then
                mDefaultChangeNew.Visible = True
            Else
                mDefaultChangeNew.Visible = False
            End If
        End If

    End Sub
    Private Sub DefaultLastUsed_Click(sender As Object, e As EventArgs) Handles mDefaultLastUsed.Click
        varSC.DefaultType = "Last"
    End Sub
    Private Sub DefaultChangeNew_Click(sender As Object, e As EventArgs) Handles mDefaultChangeNew.Click
        varSC.DefaultType = "Default"
        varSC.DefaultName = cbLocationList.Text
        varSC.DefaultPlaceListInHebrew = mPlaceListInHebrew.Checked
        varSC.DefaultSelectedindex = cbLocationList.SelectedIndex
        varSC.Save(varUserFile)
    End Sub
    Private Sub DefaultInUse_Click(sender As Object, e As EventArgs) Handles mDefaultInUse.Click
        'to reload Default place 
        mPlaceListInHebrew.Checked = varSC.DefaultPlaceListInHebrew
        'TogglePlaceListSorting()
        ClearAndReLoadPlaceLists("", varSC.DefaultSelectedindex)
    End Sub
    Private Sub mColorZmanMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles mColorZmanMenuItem.Click
        varSC.ColorZman = mColorZmanMenuItem.Checked
        change_zman()
    End Sub
    Private Sub ZmanimContextMenuHelper_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ZmanimContextMenuHelper.Opening
        'need this to anble left to right ContextMen for listview
        If varSC.HebrewMenus = True Then
            ZmanimContextMenu.Show(Control.MousePosition.X - ZmanimContextMenu.Width, Control.MousePosition.Y)
        Else
            ZmanimContextMenu.Show(Control.MousePosition)
        End If

    End Sub
    Private Sub rbtTodayRefresh_Click(sender As Object, e As EventArgs) Handles rbtTodayRefresh.Click
        dpEngdate.Value = Now
    End Sub
    Private Sub DataGridView1_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None
        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None
        If varSC.LineBetweenZmanim = False Then
            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None
        End If
    End Sub
    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick
        DataGridView1.BeginEdit(True)
        'ChangeKeybord
        If DataGridView1.CurrentCell.ColumnIndex = 1 Then
            If varSC.ChangeKeybordLayout = False Then Exit Sub
            Try
                varSavedInputLanguage = InputLanguage.CurrentInputLanguage
                For Each ILanguage As InputLanguage In InputLanguage.InstalledInputLanguages
                    If Not InStr(InputLanguage.CurrentInputLanguage.Culture.Name, "he-") And InStr(ILanguage.Culture.Name, "he-") Then
                        InputLanguage.CurrentInputLanguage = ILanguage
                        Exit For
                    End If
                Next
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub DataGridView1_Leave(sender As Object, e As EventArgs) Handles DataGridView1.Leave
        If varSC.ChangeKeybordLayout = False Then Exit Sub
        Try
            InputLanguage.CurrentInputLanguage = varSavedInputLanguage
        Catch ex As Exception
        End Try
    End Sub
    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        'this is needed it should not call Error
        Debug.Print("DataGridView1 Row " & e.RowIndex)
        Debug.Print(e.Exception.Message)
    End Sub
    Private Sub DataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
        If varFinishedLoading = False Then Exit Sub
        'CellBeginEdit fires befor EditingControlShowing
        'Debug.Print("CellBeginEdit")

        TimerStatusLabel.Enabled = False
        'change to black
        DataGridView1.Rows(e.RowIndex).Cells(2).Style.ForeColor = Color.Black

        'is ComboBox column
        If e.ColumnIndex = 2 Then
            'fill drop down with zmanim
            Dim myCombo As DataGridViewComboBoxCell
            myCombo = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
            myCombo.Items.Clear()
            For Each Z In varZmanimFunc
                myCombo.Items.Add(Z)
            Next
            'set to zman func
            If varSC.Zmanim.Count > e.RowIndex Then myCombo.Value = varSC.Zmanim(e.RowIndex).FunctionName
        End If
    End Sub
    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        If varFinishedLoading = False Then Exit Sub
        'set RightToLeft and DropDownClosed handler

        If DataGridView1.CurrentCell.ColumnIndex = 2 Then
            'looks like the is an isue when seting e.Control.RightToLeft will bug it not to fire a second time on DropDownClosed  'RemoveHandler did not help
            'seeting it back in DropDown and CellEndEdit via varDataGridCombo looks like it takes care of it - sending the DataGridViewEditingControlShowingEventArgs and changing that way did not work
            'anything done with  e.Control makes trouble even this varDataGridComboNewValue = e.Control.Text
            varDataGridCombo = CType(e.Control, ComboBox)
            varDataGridCombo.RightToLeft = 0
            RemoveHandler varDataGridCombo.DropDownClosed, AddressOf Me.DataGridView1_Combox_DropDownClosed
            AddHandler varDataGridCombo.DropDownClosed, AddressOf Me.DataGridView1_Combox_DropDownClosed
        End If

        'DataGridView1.CellValueChanged doesn't fire until you click somewhere else inside the DataGridView
        'CurrentCellDirtyStateChanged and CurrentCellChanged also don't fire as needed
        'see Note https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.editingcontrolshowing?view=net-5.0
    End Sub
    Public Sub DataGridView1_Combox_DropDownClosed(sender As Object, e As EventArgs)
        If varFinishedLoading = False Then Exit Sub
        'Debug.Print("This is DropDownClosed")
        'this will not fire as needed if changes to varDataGridCombo are not set back as befor
        varDataGridCombo.RightToLeft = 1

        'change varSc and call change zman - CurrentCell.value dose not have the updated value yet - use the Control's text
        varSC.Zmanim(DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value - 1).FunctionName = varDataGridCombo.Text
        If InStr(varDataGridCombo.Text, "ZmanGet") Then
            varSC.Zmanim(DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value - 1).ObjectName = "varAddedGets"
        Else
            varSC.Zmanim(DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value - 1).ObjectName = "varCZC"
        End If
        Place_orDate_changed()
        'DataGridView1.EndEdit()
    End Sub
    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        If varFinishedLoading = False Then Exit Sub
        'CellEndEdit fires befor EditingControlShowing, and after cell leave
        'Debug.Print("This is CellEndEdit")
        'Debug.Print(varSC.Zmanim(DataGridView1.CurrentCell.RowIndex).FunctionName)

        Debug.Print(DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value)

        'for zman name
        If DataGridView1.CurrentCell.ColumnIndex = 1 Then
            'use # column in case it was sorted
            varSC.Zmanim(DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value - 1).DisplayName = DataGridView1.CurrentCell.Value
        End If

        'for Combox when drop down was not used, no change was made just reload zmanim
        If DataGridView1.CurrentCell.ColumnIndex = 2 Then
            varDataGridCombo.RightToLeft = 1
            'cant call this here 'Place_orDate_changed() - will use a timer
            TimerZmanimAfterChange.Enabled = True
        End If
        TimerStatusLabel.Enabled = True
    End Sub
    Private Sub TimerZmanimAfterChange_Tick(sender As Object, e As EventArgs) Handles TimerZmanimAfterChange.Tick
        TimerZmanimAfterChange.Enabled = False
        Place_orDate_changed()
    End Sub
    Private Sub mDeleteZman_Click(sender As Object, e As EventArgs) Handles mDeleteZman.Click
        If DataGridView1.SelectedRows.Count > 0 Then DataGridView1.Rows.RemoveAt(DataGridView1.SelectedRows(0).Index)
        'DataGridView1.RowsRemoved will finish up
    End Sub
    Private Sub DataGridView1_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.RowEnter
        varDataGridNumColumn = DataGridView1(0, e.RowIndex).Value
        varDataGridNumRow = e.RowIndex
    End Sub
    Private Sub DataGridView1_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        'this is for when Delete was used
        If varFinishedLoading = False Then Exit Sub

        'use RowEnter to save the value of # column
        If varDataGridNumColumn > 0 Then varSC.Zmanim.Items.RemoveAt(varDataGridNumColumn - 1)
        Place_orDate_changed()
    End Sub

    '======== vars for DragAndDrop
    Private dragBoxFromMouseDown As Rectangle
    Private rowIndexFromMouseDown As Integer
    Private rowIndexOfItemUnderMouseToDrop As Integer
    Private Sub dataGridView1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles DataGridView1.MouseMove
        If (e.Button And MouseButtons.Left) = MouseButtons.Left Then

            If dragBoxFromMouseDown <> Rectangle.Empty AndAlso Not dragBoxFromMouseDown.Contains(e.X, e.Y) Then
                Dim dropEffect As DragDropEffects = DataGridView1.DoDragDrop(DataGridView1.Rows(rowIndexFromMouseDown), DragDropEffects.Move)
            End If
        End If
    End Sub

    Private Sub dataGridView1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles DataGridView1.MouseDown
        rowIndexFromMouseDown = DataGridView1.HitTest(e.X, e.Y).RowIndex

        If rowIndexFromMouseDown <> -1 Then
            Dim dragSize As Size = SystemInformation.DragSize
            dragBoxFromMouseDown = New Rectangle(New Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize)
        Else
            dragBoxFromMouseDown = Rectangle.Empty
        End If
    End Sub

    Private Sub dataGridView1_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles DataGridView1.DragOver
        e.Effect = DragDropEffects.Move
    End Sub
    Private Sub dataGridView1_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles DataGridView1.DragDrop
        varFinishedLoading = False
        Dim clientPoint As Point = DataGridView1.PointToClient(New Point(e.X, e.Y))
        rowIndexOfItemUnderMouseToDrop = DataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex

        If e.Effect = DragDropEffects.Move Then
            Dim rowToMove As DataGridViewRow = TryCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
            'this is for removing in datagrid no need for it change_zman() will reset
            'DataGridView1.Rows.RemoveAt(rowIndexFromMouseDown)
            'DataGridView1.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove)
            Dim zmanFromMouseDown As aZman = varSC.Zmanim(rowIndexFromMouseDown)
            'remove from point one and insert at point 2
            varSC.Zmanim.Items.RemoveAt(rowIndexFromMouseDown)
            varSC.Zmanim.Items.Insert(rowIndexOfItemUnderMouseToDrop, zmanFromMouseDown)
        End If
        'set to its new place
        varDataGridNumRow = rowIndexOfItemUnderMouseToDrop
        varFinishedLoading = True
        change_zman()
    End Sub
    Private Sub DataGridView1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridView1.ColumnWidthChanged
        If e.Column.Index = 1 Then varSC.DataGridCol1W = e.Column.Width
        If e.Column.Index = 2 Then varSC.DataGridCol2W = e.Column.Width
    End Sub
    Private Sub DataGridView1_Paint(sender As Object, e As PaintEventArgs) Handles DataGridView1.Paint
        Using borderPen As Pen = New Pen(Color.LightGray)
            e.Graphics.DrawRectangle(borderPen, New Rectangle(0, 0, DataGridView1.Width - 1, DataGridView1.Height - 1))
        End Using
        'MyBase.OnPaint(e)
    End Sub
    Private Sub DataGridView1_Scroll(sender As Object, e As ScrollEventArgs) Handles DataGridView1.Scroll
        DataGridView1.Invalidate()
    End Sub
    Public Sub mHebrewMenus_Click() Handles mHebrewMenus.Click
        varSC.HebrewMenus = mHebrewMenus.Checked

        If mHebrewMenus.Checked = True Then
            'change Locations Sort to Hebrew
            If varSC.PlaceListInHebrew = False Then
                mPlaceListInHebrew.Checked = True
                mPlaceListInHebrew_Click()
            End If
            Me.RightToLeft = 1
            LocationContextMenu.RightToLeft = 1
            ZmanimContextMenu.RightToLeft = 1
            'varSC.PlaceListInHebrew = True
            CbTimeZone.RightToLeft = 0
            If InStr(CultureInfo.CurrentCulture.Name, "he-IL") Then CbTimeZone.RightToLeft = 1
            tbcountry.RightToLeft = 0
            tbElevation.RightToLeft = 0
            tblatitude.RightToLeft = 0
            tblongitude.RightToLeft = 0
            tbzone.RightToLeft = 0
            LabelCountry.RightToLeft = 1
            LabelOffSet.RightToLeft = 1
            LabelLatitude.RightToLeft = 1
            LabelLongitude.RightToLeft = 1
            LabelElevation.RightToLeft = 1
            LabelTimeZone.RightToLeft = 1

            Try
                LabelCountry.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                LabelOffSet.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                LabelLatitude.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                LabelLongitude.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                LabelElevation.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                LabelTimeZone.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                LabelDisclaimer.Font = MemoryFonts.GetFont(1, 9.25, FontStyle.Regular)
            Catch ex As Exception
            End Try

            LabelDisclaimer.Text = "אין לסמוך על זמנים אלו בצמצום"
            mGetCurrnetLocation.Text = "קבל את המיקום הנוכחי"
            mSaveLocationChanges.Text = "שמור שינויי מיקום"
            mRemoveLocation.Text = "הסר מיקום"
            mAddToUserLocation.Text = "הוסף כמיקום חדש"
            mResetLocationList.Text = "אפס את רשימת המיקומים"
            mPlaceListInHebrew.Text = "מיון מיקומים בעברית"
            mDefaultsMenu.Text = "מיקום ברירת מחדל"
            mDefaultLastUsed.Text = "בשימוש אחרון"
            SettingsToolStripMenuItem.Text = "הגדרות"
            mIsraeliYomTov.Text = "פרשיות וימים טובים כארץ ישראל"
            m24HourFormatTime.Text = "פורמט 24 שעות"
            mShowTimesOnStatusBar.Text = "הצג זמן בשורת המצב"
            mAskWhenChanging.Text = "שאל על שינויים"
            mBackUpWhenChanging.Text = "גיבוי בעת שינוי"
            mStayOnTopToolStripMenuItem.Text = "השאר למעלה"
            mChangeKeybordLayout.Text = "שנה מקלדת לעברית"
            mExport.Text = "ייצוא"
            mResetSettings.Text = "אפס את כל ההגדרות"
            mOpenCompare.Text = "השווה זמנים"
            mOpenSchedule.Text = "מתזמן"
            GroupBox2.Text = "פרטי מקום    "
            mAddZman.Text = "הוסף זמן חדש למטה"
            mDeleteZman.Text = "מחק את זמן הנבחר"
            mResetZmanimList.Text = "אפס את רשימת הזמנים"
            mColorZmanMenuItem.Text = "הדגשת הזמנים לפי צבעים"
            mCalculateElevation.Text = "חישוב גובה"
            mUseUSNO.Text = "השתמש באלגוריתם USNO ישן"
            mLineBetweenZmanim.Text = "הוסף קו בין השורות"
            GroupBox1.Text = "תאריכים"
            mHebrewMenus.Text = "תפריטים בעברית"
            ToolStripDropDownButton1.Text = "כלים"
            mInfoHelp.Text = "מידע ועזרה"
            LabelCountry.Text = "מדינה"
            LabelOffSet.Text = "קיזוז זמן"
            LabelLatitude.Text = "קו רוחב"
            LabelLongitude.Text = "קו אורך"
            LabelElevation.Text = "גובה"
            LabelTimeZone.Text = "אזור זמן"
            mShowTooltips.Text = "הצג טולטיפס"
            mDisplayDafYomi.Text = "הצג דף היומי"
            mHideLocationInfo.Text = "הסתר פרטי מיקום"
            If varSC.ShowTooltips = True Then
                ToolTip1.SetToolTip(rbtTodayRefresh, "לחץ כדי לאפס את התאריך להיום")
                ToolTip1.SetToolTip(rtbHebrewDate, "הכנס כאן תאריך עברי בכדי לעבור אליו")
                ToolTip1.SetToolTip(cbLocationList, "בחר או הקלד מיקום")
                ToolTip1.SetToolTip(rbtLocationContexOpen, "לחץ כדי לפתוח את התפריט")
                ToolTip1.SetToolTip(tblatitude, "הזן קו רוחב")
                ToolTip1.SetToolTip(tblongitude, "הזן קו אורך")
                ToolTip1.SetToolTip(tbElevation, "הזן את גובה המיקום")
                ToolTip1.SetToolTip(CbTimeZone, "בחר אזור זמן")
                For i = 0 To DataGridView1.Rows.Count - 1
                    DataGridView1(1, i).ToolTipText = "לחץ פעמיים לעריכה" & vbCr & "גרור ושחרר לסידור מחדש" & vbCr & "קליק ימני לאפשרויות נוספות"
                Next
                If varSC.HideLocationBox = True Then
                    ToolTip1.SetToolTip(btHideLocationInfo, "הרחב פרטי מיקום")
                Else
                    ToolTip1.SetToolTip(btHideLocationInfo, "הסתר פרטי מיקום")
                End If
            End If
            btHideLocationInfo.Location = New System.Drawing.Point(153, btHideLocationInfo.Location.Y)

            'do it here after SettingsToolStripMenuItem.Width is set
            Dim TransparencyBox As New ToolStripMenuTextBoxAndLabel("אי שקיפות:", 35, True, SettingsToolStripMenuItem.Width)
            TransparencyBox.Text = varSC.TransparencyValue * 100 & "%" ' convert from decimal
            'ToolStripMenuTextBoxAndLabel already there
            If SettingsToolStripMenuItem.DropDownItems.Item(SettingsToolStripMenuItem.DropDownItems.Count - 3).Name = "ToolStripSeparator3" Then
                SettingsToolStripMenuItem.DropDownItems.RemoveAt(SettingsToolStripMenuItem.DropDownItems.Count - 2)
            End If
            SettingsToolStripMenuItem.DropDownItems.Insert(SettingsToolStripMenuItem.DropDownItems.Count - 1, TransparencyBox)
        Else
            'change Locations Sort to eng
            If varSC.PlaceListInHebrew = True Then
                mPlaceListInHebrew.Checked = False
                mPlaceListInHebrew_Click()
            End If

            Me.RightToLeft = 0
            LocationContextMenu.RightToLeft = 0
            ZmanimContextMenu.RightToLeft = 0
            If InStr(CultureInfo.CurrentCulture.Name, "en-") Then CbTimeZone.RightToLeft = 0

            Try
                LabelCountry.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                LabelOffSet.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                LabelLatitude.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                LabelLongitude.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                LabelElevation.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                LabelTimeZone.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                LabelDisclaimer.Font = MemoryFonts.GetFont(1, 8.5, FontStyle.Regular)
            Catch ex As Exception
            End Try
            LabelDisclaimer.Text = "Do not rely on zmanim to the last moment"
            mGetCurrnetLocation.Text = "Get Current Location"
            mSaveLocationChanges.Text = "Save Location Changes"
            mRemoveLocation.Text = "Remove Location"
            mAddToUserLocation.Text = "Add As New Location"
            mResetLocationList.Text = "Reset Location List"
            mPlaceListInHebrew.Text = "Sort Locations In Hebrew"
            mDefaultsMenu.Text = "Default Location"
            mDefaultLastUsed.Text = "Last Used"
            SettingsToolStripMenuItem.Text = "Settings"
            mIsraeliYomTov.Text = "Eretz Yisrael Yom Tov and Parsha"
            m24HourFormatTime.Text = "24 Hour Format Time"
            mShowTimesOnStatusBar.Text = "Show Time On Status Bar"
            mAskWhenChanging.Text = "Ask on Changes"
            mBackUpWhenChanging.Text = "Back Up When Changing"
            mStayOnTopToolStripMenuItem.Text = "Stay On Top"
            mChangeKeybordLayout.Text = "Change Keyboard to Hebrew"
            mExport.Text = "Export"
            mResetSettings.Text = "Reset All Settings"
            mOpenCompare.Text = "Compare"
            mOpenSchedule.Text = "Scheduler"
            GroupBox2.Text = "Location info    "
            mAddZman.Text = "Add New Zman Below"
            mDeleteZman.Text = "Delete This Zman"
            mResetZmanimList.Text = "Reset Zmanim List"
            mColorZmanMenuItem.Text = "Color for Zmanim"
            mCalculateElevation.Text = "Calculate Elevation"
            mUseUSNO.Text = "Use Older USNO Algorithm"
            mLineBetweenZmanim.Text = "Add Line Between Zmanim"
            GroupBox1.Text = "Dates"
            mHebrewMenus.Text = "Hebrew Menus"
            mHideLocationInfo.Text = "Hide Location Info"
            ToolStripDropDownButton1.Text = "Tools"
            mInfoHelp.Text = "Info and Help"
            LabelCountry.RightToLeft = 0
            LabelOffSet.RightToLeft = 0
            LabelLatitude.RightToLeft = 0
            LabelLongitude.RightToLeft = 0
            LabelElevation.RightToLeft = 0
            LabelTimeZone.RightToLeft = 0
            LabelCountry.Text = "Country"
            LabelOffSet.Text = "OffSet"
            LabelLatitude.Text = "Latitude"
            LabelLongitude.Text = "Longitude"
            LabelElevation.Text = "Elevation"
            LabelTimeZone.Text = "Time Zone"

            mShowTooltips.Text = "Display Tool Tips"
            mDisplayDafYomi.Text = "Display Daf Yomi"
            If varSC.ShowTooltips = True Then
                ToolTip1.SetToolTip(rbtTodayRefresh, "Press to Reset Date to Today")
                ToolTip1.SetToolTip(rtbHebrewDate, "Enter a Hebrew date to go to it")
                ToolTip1.SetToolTip(cbLocationList, "Select or Type Location")
                ToolTip1.SetToolTip(rbtLocationContexOpen, "Press to Open Menu")
                ToolTip1.SetToolTip(tblatitude, "Enter Latitude")
                ToolTip1.SetToolTip(tblongitude, "Enter Longitude")
                ToolTip1.SetToolTip(tbElevation, "Enter Location Height")
                ToolTip1.SetToolTip(CbTimeZone, "Select Time Zone")
                'DataGridView ToolTip's
                For i = 0 To DataGridView1.Rows.Count - 1
                    DataGridView1(1, i).ToolTipText = "Double Click to Edit" & vbCr & "Drag and Drop to Rearrange" & vbCr & "Right Click for More Options"
                Next
                If varSC.HideLocationBox = True Then
                    ToolTip1.SetToolTip(btHideLocationInfo, "More Location Details")
                Else
                    ToolTip1.SetToolTip(btHideLocationInfo, "Hide Location Details")
                End If
            End If
            btHideLocationInfo.Location = New System.Drawing.Point(113, btHideLocationInfo.Location.Y)

            Dim TransparencyBox As New ToolStripMenuTextBoxAndLabel("Opacity:", 35, False, SettingsToolStripMenuItem.Width)
            TransparencyBox.Text = varSC.TransparencyValue * 100 & "%" ' convert from decimal
            'ToolStripMenuTextBoxAndLabel already there
            If SettingsToolStripMenuItem.DropDownItems.Item(SettingsToolStripMenuItem.DropDownItems.Count - 3).Name = "ToolStripSeparator3" Then
                SettingsToolStripMenuItem.DropDownItems.RemoveAt(SettingsToolStripMenuItem.DropDownItems.Count - 2)
            End If
            SettingsToolStripMenuItem.DropDownItems.Insert(SettingsToolStripMenuItem.DropDownItems.Count - 1, TransparencyBox)
        End If

        If varSC.ShowTooltips = False Then
            ToolTip1.SetToolTip(rbtTodayRefresh, Nothing)
            ToolTip1.SetToolTip(rtbHebrewDate, Nothing)
            ToolTip1.SetToolTip(cbLocationList, Nothing)
            ToolTip1.SetToolTip(rbtLocationContexOpen, Nothing)
            ToolTip1.SetToolTip(tblatitude, Nothing)
            ToolTip1.SetToolTip(tblongitude, Nothing)
            ToolTip1.SetToolTip(tbElevation, Nothing)
            ToolTip1.SetToolTip(CbTimeZone, Nothing)
        End If
    End Sub
    Private Sub btHideLocationInfo_Click(sender As Object, e As EventArgs) Handles btHideLocationInfo.Click
        mHideLocationInfo.Checked = Not mHideLocationInfo.Checked
        mHideLocationInfo_Click()
    End Sub
    Private Sub mHideLocationInfo_Click() Handles mHideLocationInfo.Click
        varSC.HideLocationBox = mHideLocationInfo.Checked
        If mHideLocationInfo.Checked Then
            varSC.DataGridSizeH = DataGridView1.Size.Height
            varSC.SizeH = Me.Height
            DoHideLocationBox(True)
        Else
            GroupBox2.Size = New System.Drawing.Size(GroupBox2.Size.Width, 140)
            LabelLatitude.Location = New System.Drawing.Point(LabelLatitude.Location.X, LabelLatitude.Location.Y - 10)
            LabelLongitude.Location = New System.Drawing.Point(LabelLongitude.Location.X, LabelLongitude.Location.Y - 10)
            LabelCountry.Location = New System.Drawing.Point(LabelCountry.Location.X, LabelCountry.Location.Y - 10)
            LabelElevation.Location = New System.Drawing.Point(LabelElevation.Location.X, LabelElevation.Location.Y - 10)
            LabelDisclaimer.Location = New System.Drawing.Point(LabelDisclaimer.Location.X, LabelDisclaimer.Location.Y + 80)
            DataGridView1.Location = New System.Drawing.Point(DataGridView1.Location.X, DataGridView1.Location.Y + 80)
            Me.DataGridView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Size = New System.Drawing.Size(Me.Size.Width, Me.Size.Height + 80)
            Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            varSC.SizeH = Me.Height
            btHideLocationInfo.Image = My.Resources.Gray_Up_16
            btHideLocationInfo.Location = New System.Drawing.Point(btHideLocationInfo.Location.X, btHideLocationInfo.Location.Y - 5)
        End If
        'for tool tips
        mHebrewMenus_Click()
    End Sub
    Public Sub DoHideLocationBox(ShrinkFrmSize As Boolean)
        GroupBox2.Size = New System.Drawing.Size(GroupBox2.Size.Width, 60)
        LabelLatitude.Location = New System.Drawing.Point(LabelLatitude.Location.X, LabelLatitude.Location.Y + 10)
        LabelLongitude.Location = New System.Drawing.Point(LabelLongitude.Location.X, LabelLongitude.Location.Y + 10)
        LabelCountry.Location = New System.Drawing.Point(LabelCountry.Location.X, LabelCountry.Location.Y + 10)
        LabelElevation.Location = New System.Drawing.Point(LabelElevation.Location.X, LabelElevation.Location.Y + 10)
        LabelDisclaimer.Location = New System.Drawing.Point(LabelDisclaimer.Location.X, LabelDisclaimer.Location.Y - 80)
        DataGridView1.Location = New System.Drawing.Point(DataGridView1.Location.X, DataGridView1.Location.Y - 80)
        If ShrinkFrmSize = True Then
            'size is set in SettingsCollection to default and in mHideLocationInfo_Click() & Frminfo_FormClosed
            DataGridView1.Size = New System.Drawing.Size(DataGridView1.Size.Width, varSC.DataGridSizeH)
            Me.DataGridView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Size = New System.Drawing.Size(Me.Size.Width, Me.Size.Height - 80)
            Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        End If

        btHideLocationInfo.Image = My.Resources.Gray_Down_16
        btHideLocationInfo.Location = New System.Drawing.Point(btHideLocationInfo.Location.X, btHideLocationInfo.Location.Y + 5)
    End Sub
End Class









'Dim MyStopwatch As New Stopwatch
'MyStopwatch.Start()
'Debug.Print("Time elapsed: {0}", MyStopwatch.Elapsed)

'GetTemporalHour(GetAlos18Degrees(), GetTzais18Degrees())

'Dim args()
'args = {#1/2/2010#, #1/2/2010 2:00:00 PM#}
'MsgBox(CallByName(CZC, "GetTemporalHour", CallType.Get, args))
'args = {Now, 8855698}
'MsgBox(CallByName(CZC, "GetTimeOffset", CallType.Get, args))
