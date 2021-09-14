Imports Microsoft.VisualBasic.ApplicationServices
Public Class FrmSchedule
    Private TempCZC As ComplexZmanimCalendar
    Private TempTimeZone As TimeZoneInfo
    Private TimechangedProgrammatic As Boolean = False
    Private MinuteschangedProgrammatic As Boolean = False

    Private Sub FrmSchedule_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New System.Drawing.Point(Frminfo.Location.X + ((Frminfo.Width - Me.Width) / 2), Frminfo.Bounds.Bottom - (Me.Height + 45))

        'Dim myBrush As System.Drawing.SolidBrush = New System.Drawing.SolidBrush(System.Drawing.Color.Red)
        'Dim formGraphics As System.Drawing.Graphics
        'formGraphics = PictureBox1.CreateGraphics() 'Me.CreateGraphics()
        'formGraphics.InterpolationMode = InterpolationMode.Bilinear
        'formGraphics.PixelOffsetMode = PixelOffsetMode.Half
        'formGraphics.SmoothingMode = SmoothingMode.None
        'formGraphics.FillEllipse(myBrush, 0, 0, 16, 16) 'New Rectangle(0, 0, 20, 20)

        GroupBox2.Font = MemoryFonts.GetFont(1, 12, FontStyle.Regular)
        cbLocationList.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        cbTimeZone.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tblatitude.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tblongitude.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tbElevation.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        cbZman.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        tbMessage.Font = MemoryFonts.GetFont(1, 9, FontStyle.Regular)
        tbSound.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)
        dtpTime.Font = MemoryFonts.GetFont(0, 9, FontStyle.Regular)

        If varSC.Clock24Hour = True Then
            dtpTime.CustomFormat = " HH:mm:ss"
        Else
            dtpTime.CustomFormat = " hh:mm:ss  tt"
        End If

        MinuteschangedProgrammatic = True
        dtpMinutes.Value = #1/2/2000#


        If Frminfo.mHebrewMenus.Checked = True Then
            'for layout changes for RightToLeft
            Dim saveReminderNum, saveReminderTotal
            saveReminderNum = tbReminderNum.Location
            saveReminderTotal = tbReminderTotal.Location
            tbReminderNum.Location = saveReminderTotal
            tbReminderTotal.Location = saveReminderNum
            btFileDialog.Location = New System.Drawing.Point(LabelSound.Location.X, LabelSound.Location.Y - 2)
            'change Event Handler - changing Location will not work as the pictures will not be right
            RemoveHandler btFrist.Click, AddressOf Me.btFrist_Click
            RemoveHandler btPrevious.Click, AddressOf Me.btPrevious_Click
            RemoveHandler btNext.Click, AddressOf Me.btNext_Click
            RemoveHandler btLast.Click, AddressOf Me.btLast_Click
            AddHandler btFrist.Click, AddressOf Me.btLast_Click
            AddHandler btPrevious.Click, AddressOf Me.btNext_Click
            AddHandler btNext.Click, AddressOf Me.btPrevious_Click
            AddHandler btLast.Click, AddressOf Me.btFrist_Click
            LabelLatitude.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            LabelLongitude.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            LabelElevation.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            LabelTimeZone.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            LabelMessage.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            LabelMinutes.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            cbIsActive.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            cbNotToday.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            btOpenScheduler.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            btToggleTaskScheduler.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            btTest.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            TabPageTime.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            TabPageZman.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            TabControlTimeZman.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)

            Me.Text = "מתזמן"
            pbTasksON.Location = New System.Drawing.Point(78, 2.5)
            GroupBox1.RightToLeft = 1
            GroupBox2.RightToLeft = 1
            cbTimeZone.RightToLeft = 0
            If InStr(CultureInfo.CurrentCulture.Name, "he-IL") Then cbTimeZone.RightToLeft = 1
            tbElevation.RightToLeft = 0
            tblatitude.RightToLeft = 0
            tblongitude.RightToLeft = 0
            cbZman.RightToLeft = 0
            tbSound.RightToLeft = 0
            LabelLatitude.RightToLeft = 1
            LabelLongitude.RightToLeft = 1
            LabelElevation.RightToLeft = 1
            LabelTimeZone.RightToLeft = 1
            LabelFrom.RightToLeft = 1

            GroupBox1.Text = "פרטי מקום"
            GroupBox2.Text = "מתזמן משימות        " '"תזכורות        "
            LabelLatitude.Text = "קו רוחב"
            LabelLongitude.Text = "קו אורך"
            LabelElevation.Text = "גובה"
            LabelTimeZone.Text = "אזור זמן"
            LabelFrom.Text = "מתוך:"
            cbIsActive.Text = "פעיל"
            cbNotToday.Text = "לא היום"
            LabelMessage.RightToLeft = 1
            LabelMessage.Text = "הודעת תזכורת"
            LabelMinutes.RightToLeft = 1
            LabelMinutes.Text = "תזכיר לפני"
            LabelSound.RightToLeft = 1
            LabelSound.Text = " תזכורות קולי"
            btTest.Text = "בדק"
            btOpenScheduler.Text = "פתח מתזמן"

            LabelReminders.Text = "תזכורות"
            LabelReminders.Font = MemoryFonts.GetFont(1, 9.5, FontStyle.Regular)
            LabelReminders.Location = New System.Drawing.Point(155, LabelReminders.Location.Y)
            LabelDivider2.Location = New System.Drawing.Point(20, LabelDivider2.Location.Y)
            LabelDivider2.Size = New System.Drawing.Size(133, 2)
            LabelLocation.Text = "מיקום לזמן"
            LabelLocation.Font = MemoryFonts.GetFont(1, 9.5, FontStyle.Regular)
            LabelLocation.Location = New System.Drawing.Point(140, LabelLocation.Location.Y)
            LabelDivider1.Location = New System.Drawing.Point(20, LabelDivider1.Location.Y)
            LabelDivider1.Size = New System.Drawing.Size(118, 2)

            TabControlTimeZman.RightToLeftLayout = True
            TabPageTime.Text = "שעה"
            TabPageZman.Text = "זמן"

        Else
            GroupBox1.Text = "Location info"
            GroupBox1.Text = "Reminders"
            GroupBox1.RightToLeft = 0
            GroupBox2.RightToLeft = 0
            LabelLatitude.RightToLeft = 0
            LabelLongitude.RightToLeft = 0
            LabelElevation.RightToLeft = 0
            LabelTimeZone.RightToLeft = 0
            If InStr(CultureInfo.CurrentCulture.Name, "en-") Then cbTimeZone.RightToLeft = 0
            LabelLatitude.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            LabelLongitude.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            LabelElevation.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            LabelTimeZone.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            LabelMessage.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            LabelMinutes.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            cbIsActive.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            cbNotToday.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            TabPageTime.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            TabPageZman.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)

            LabelLatitude.RightToLeft = 0
            LabelLongitude.RightToLeft = 0
            LabelElevation.RightToLeft = 0
            LabelTimeZone.RightToLeft = 0
            LabelFrom.RightToLeft = 0
            LabelLatitude.Text = "Latitude"
            LabelLongitude.Text = "Longitude"
            LabelElevation.Text = "Elevation"
            LabelTimeZone.Text = "Time Zone"
            LabelFrom.Text = "From: "
            cbIsActive.Text = "Is Active"
            cbNotToday.Text = "Not Today"
            LabelMessage.RightToLeft = 0
            LabelMessage.Text = "Reminder Message"
            LabelMinutes.RightToLeft = 0
            LabelMinutes.Text = "Remind Before"
            LabelSound.RightToLeft = 0
            LabelSound.Text = "Audio Reminder"
            btTest.Text = "Test"
            btOpenScheduler.Text = "Open Scheduler"

        End If

        Dim LocationNum As Integer = -1
        Dim counter As Integer = 0
        If varSC.PlaceListInHebrew = False Then
            cbLocationList.RightToLeft = 0
            For Each LI As aLocation In varEngPlaceList
                cbLocationList.Items.Add(LI.EngName & If(LI.HebName <> "", " | " & LI.HebName, ""))
                If varSC.SchedulerLocation <> "" And varSC.SchedulerLocation = LI.EngName Then LocationNum = counter
                counter += 1
            Next
        Else
            cbLocationList.RightToLeft = 1
            For Each LI As aLocation In varHebPlaceList
                cbLocationList.Items.Add(If(LI.HebName <> "", LI.HebName & " | " & LI.EngName, LI.EngName))
                If varSC.SchedulerLocation <> "" And varSC.SchedulerLocation = LI.EngName Then LocationNum = counter
                counter += 1
            Next
        End If

        'set Location list - if no saved seting use frminfo
        If LocationNum > -1 Then
            FrmSchedule_change_place(LocationNum)
        Else
            FrmSchedule_change_place(Frminfo.cbLocationList.SelectedIndex)
        End If


        varZmanimFunc.ForEach(Sub(x) cbZman.Items.Add(x))
        'remove the last 4 that are AddedGets
        'For i = 1 To 4
        '    cbTime.Items.RemoveAt(cbTime.Items.Count - 1)
        'Next


        tbReminderNum.Text = 1
        tbReminderTotal.Text = varSC.Schedule.Count
        If varSC.Schedule.Count = 0 Then tbReminderTotal.Text = 1


        'issue with system32 on 64 bit as explined here
        'https://social.technet.microsoft.com/wiki/contents/articles/19256.avoid-file-system-redirection-in-visual-studio-2012.aspx
        'changed to anyCPU
        If System.IO.File.Exists(Environment.GetEnvironmentVariable("systemroot") & "\System32\Tasks\MyZmanScheduler") = True Then 'Or  My.Computer.FileSystem.FileExists("C:\Windows\SysWOW64\Tasks\MyZmanScheduler") = True
            'Debug.Print("yes")
            pbTasksON.Image = My.Resources.green_16
            btToggleTaskScheduler.Text = "Remove Task"
            If Frminfo.mHebrewMenus.Checked = True Then btToggleTaskScheduler.Text = "הסר משימה"

        Else
            'Debug.Print("no")
            pbTasksON.Image = My.Resources.red_16
            btToggleTaskScheduler.Text = "Add Task"
            If Frminfo.mHebrewMenus.Checked = True Then btToggleTaskScheduler.Text = "הוסיף משימה"
        End If
    End Sub
    Private Sub Butclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Butclose.Click
        Me.Close()
    End Sub
    Private Sub FrmSchedule_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        NoActiveReminders()
        varSC.Save(varUserFile)
    End Sub
    Private Function NoActiveReminders()
        'remove task if the are no active reminders
        Dim ActiveReminders As Boolean
        For Each Schedule As aSchedule In varSC.Schedule
            If Schedule.IsActive = True Then
                ActiveReminders = True
                Exit For
            End If
        Next
        If ActiveReminders = False And System.IO.File.Exists(Environment.GetEnvironmentVariable("systemroot") & "\System32\Tasks\MyZmanScheduler") = True Then
            Shell("SCHTASKS /Delete /TN MyZmanScheduler /F")
        End If
        If varSC.Schedule.Count < 1 And System.IO.File.Exists(Environment.GetEnvironmentVariable("systemroot") & "\System32\Tasks\MyZmanScheduler") = True Then
            Shell("SCHTASKS /Delete /TN MyZmanScheduler /F")
        End If
        Return ActiveReminders
    End Function
    Public Sub RunSchedulecheck(FirstInstance As Boolean)
        'MsgBox("1 " & FirstInstance)
        If FirstInstance = True Then
            'load New varSC
            'varUserFile set in ProjectsGlobalVariables is Environment.CurrentDirectory & "\MYZman.exe.UserSettings.xml" - not good when it runs from task scheduler it will be C:\Windows\System32\Tasks
            Dim args As String() = Environment.GetCommandLineArgs()
            varUserFile = args(0) & ".UserSettings.xml"
            'MsgBox("2" & vbCr & FirstInstance & vbCr & varUserFile)
            If My.Computer.FileSystem.FileExists(varUserFile) Then
                varSC = SerializableData.Load(varUserFile, GetType(SettingsCollection))
            End If
        End If

        'MsgBox("3" & vbCr & FirstInstance & vbCr & My.Computer.FileSystem.FileExists(varUserFile) & vbCr & varSC.Schedule.Count)

        'no Reminders remove task from scheduler and end
        If NoActiveReminders() = False Or varSC.Schedule.Count < 1 Then
            Exit Sub
        End If

        Dim NotToday As Boolean
        For Each oSchedule As aSchedule In varSC.Schedule
            If oSchedule.IsActive = True And oSchedule.NotToday = False Then
                NotToday = CheckSchedule(oSchedule, False) ', FirstInstance
                oSchedule.NotToday = NotToday
                'if FrmSchedule is open
                If Me.IsHandleCreated Then cbNotToday.Checked = NotToday
            End If
        Next
        varSC.Save(varUserFile)

        'If this is the FirstInstance Me.Startup will close it down
    End Sub
    Function CheckSchedule(oSchedule As aSchedule, Optional TestRuning As Boolean = False) ', Optional FirstInstance As Boolean = False
        'MsgBox("2 " & FirstInstance)
        'Debug.Print("2 " & FirstInstance)
        Dim Response
        Dim mytimespan As TimeSpan
        Dim myzman As Date
        Dim MinutesBefore = oSchedule.Minutes
        If MinutesBefore = "" Then MinutesBefore = 1

        Try
            TempTimeZone = TimeZoneInfo.FindSystemTimeZoneById(varSC.SchedulerTimeZone)
            Dim timeZone As ITimeZone = New PZmanimTimeZone(TempTimeZone) 'WindowsTimeZone("Eastern Standard Time") '
            Dim location As New GeoLocation("", varSC.SchedulerLatitude, varSC.SchedulerLongitude, varSC.SchedulerElevation, timeZone)
            TempCZC = New ComplexZmanimCalendar(Now, location)
            If varSC.UseOlderUsnoAlgorithm = False Then
                TempCZC.AstronomicalCalculator = New NOAACalculator()
            Else
                TempCZC.AstronomicalCalculator = New ZmanimCalculator()
            End If
            Dim Args() = {Now, location} ' for AddedGets

            If oSchedule.IsFunc = True Then
                If InStr(oSchedule.Time, "ZmanGet") Then
                    myzman = CDate(CallByName(varAddedGets, oSchedule.Time, CallType.Get, Args))
                    'Debug.Print(mytime)
                Else
                    myzman = CDate(CallByName(TempCZC, oSchedule.Time, CallType.Get))
                    'Debug.Print(mytime)
                End If
            Else
                DateTime.TryParse(oSchedule.Time, myzman)
            End If
        Catch ex As Exception
        End Try
        'MsgBox("3 " & vbCr & oSchedule.Time & vbCr & myzman)
        'Debug.Print("3 " & myzman & vbCr & myzman.AddMinutes("-" & MinutesBefore))

        If TestRuning = False Then
            'leave if not in time - and reset for next day
            If Now < myzman.AddMinutes("-" & MinutesBefore) Or Now > myzman Then Return False
        End If

        If myzman = Nothing Then Return False
        mytimespan = myzman - Now
        'MsgBox("4 " & FirstInstance)
        'Debug.Print("4 " & myzman)

        Dim MyPlayer As SoundPlayer
        If oSchedule.Sound <> "" And System.IO.File.Exists(oSchedule.Sound) Then
            Try
                MyPlayer = New SoundPlayer(oSchedule.Sound)
                MyPlayer.PlayLooping()
            Catch ex As Exception
            End Try
        Else
            SystemSounds.Hand.Play()
        End If

        Dim timeFormat As String = "h:mm tt"
        If varSC.Clock24Hour = True Then timeFormat = "H:mm"
        Dim TimeBefore As String
        If mytimespan.TotalHours > 1 Then
            TimeBefore = #1/2/2000#.AddMilliseconds(mytimespan.TotalMilliseconds).ToString("H:mm") & If(varSC.HebrewMenus = True, ChrW(&H200F) & " שעות לפני " & ChrW(&H200E), " Hours Before ")
        Else
            TimeBefore = #1/2/2000#.AddMilliseconds(mytimespan.TotalMilliseconds).ToString("mm") & If(varSC.HebrewMenus = True, ChrW(&H200F) & " דקות לפני " & ChrW(&H200E), " Minutes Before ")
            If TestRuning = True And Now > myzman Then TimeBefore = #1/2/2000#.AddMilliseconds(mytimespan.TotalMilliseconds).ToString("H:mm") & If(varSC.HebrewMenus = True, ChrW(&H200F) & " שעות לפני " & ChrW(&H200E), " Hours Before ")
        End If

        If varSC.HebrewMenus = True Then
            FrmScheduleMessage.LabelTime.Text = "השעה עכשיו " & ChrW(&H200E) & StrConv(Now.ToString(timeFormat), VbStrConv.Lowercase)
            FrmScheduleMessage.LabelZman.Text = "זה " & ChrW(&H200E) & TimeBefore & StrConv(myzman.ToString(timeFormat), VbStrConv.Lowercase)
            FrmScheduleMessage.LabelMessage.Text = oSchedule.Message
            FrmScheduleMessage.LabelRemindAgain.Text = "להזכיר שוב?"
        Else
            FrmScheduleMessage.LabelTime.Text = "Time Is Now " & StrConv(Now.ToString(timeFormat), VbStrConv.Lowercase)
            FrmScheduleMessage.LabelZman.Text = "That Is " & TimeBefore & StrConv(myzman.ToString(timeFormat), VbStrConv.Lowercase)
            FrmScheduleMessage.LabelMessage.Text = oSchedule.Message
            FrmScheduleMessage.LabelRemindAgain.Text = "Remind Again?"
        End If
        FrmScheduleMessage.btYes.Select()
        FrmScheduleMessage.Focus()
        FrmScheduleMessage.BringToFront()
        FrmScheduleMessage.TopMost = True
        Response = FrmScheduleMessage.ShowDialog()

        If MyPlayer IsNot Nothing Then MyPlayer.Stop()

        If Response = vbNo Then
            Return True
        Else 'yes or cancel by closing
            Return False
        End If
    End Function
    'bring to front
    'Activate frminfo to Foreground
    'If FirstInstance = False Then
    '    Frminfo.Activate()
    'Else
    '    'the is no me(FrmSchedule) yet but this works most of the time to get the msg to the front
    '    Me.Activate()
    'End If
    'Dim handle As IntPtr
    'handle = Process.GetCurrentProcess().MainWindowHandle
    'SwitchToThisWindow(handle, True)
    'To Set window In front
    '<DllImport("User32.dll", SetLastError:=True)>
    'Private Shared Sub SwitchToThisWindow(ByVal hWnd As IntPtr, ByVal fAltTab As Boolean)
    'End Sub
    Sub makexml()
        Dim args As String() = Environment.GetCommandLineArgs()
        Dim doc As XDocument
        doc = <?xml version="1.0" encoding="UTF-16"?>
              <Task version="1.2" xmlns="http://schemas.microsoft.com/windows/2004/02/mit/task">
                  <Triggers>
                      <TimeTrigger>
                          <Repetition>
                              <Interval>PT1M</Interval>
                              <StopAtDurationEnd>false</StopAtDurationEnd>
                          </Repetition>
                          <StartBoundary><%= Now.Year & "-01-01T01:00:00" %></StartBoundary>
                          <Enabled>true</Enabled>
                      </TimeTrigger>
                  </Triggers>
                  <Settings>
                      <IdleSettings>
                          <StopOnIdleEnd>true</StopOnIdleEnd>
                          <RestartOnIdle>false</RestartOnIdle>
                      </IdleSettings>
                      <MultipleInstancesPolicy>StopExisting</MultipleInstancesPolicy>
                      <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
                      <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
                      <AllowHardTerminate>true</AllowHardTerminate>
                      <StartWhenAvailable>true</StartWhenAvailable>
                      <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
                      <AllowStartOnDemand>true</AllowStartOnDemand>
                      <Enabled>true</Enabled>
                      <Hidden>false</Hidden>
                      <RunOnlyIfIdle>false</RunOnlyIfIdle>
                      <WakeToRun>false</WakeToRun>
                      <ExecutionTimeLimit>PT2H</ExecutionTimeLimit>
                  </Settings>
                  <Actions Context="Author">
                      <Exec>
                          <Command><%= args(0) %></Command>
                          <Arguments>"/S"</Arguments>
                      </Exec>
                  </Actions>
              </Task>
        doc.Save(Environment.CurrentDirectory & "\MyZmanScheduler.xml")
    End Sub
    Private Sub cbLocationList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLocationList.SelectedIndexChanged
        FrmSchedule_change_place(cbLocationList.SelectedIndex)
    End Sub
    Private Sub cbLocationList_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLocationList.DropDown
        SetAutoComplete(sender, False)
    End Sub
    Private Sub cbLocationList_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLocationList.DropDownClosed
        SetAutoComplete(sender, True)
        If cbLocationList.SelectedIndex > -1 Then
            FrmSchedule_change_place(cbLocationList.SelectedIndex)
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
    Private Sub CbTimeZone_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTimeZone.DropDown
        SetAutoComplete(sender, False)
        'load list for this offset
        cbTimeZone.Items.Clear()

        If tbzone.Text <> "" Then
            Dim AllTimeZones As ObjectModel.ReadOnlyCollection(Of TimeZoneInfo) = TimeZoneInfo.GetSystemTimeZones
            For Each TZ In AllTimeZones
                If TZ.BaseUtcOffset = New TimeSpan(tbzone.Text, 0, 0) Then
                    cbTimeZone.Items.Add(TZ.DisplayName)
                End If
            Next
        End If
    End Sub
    Private Sub CbTimeZone_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbLocationList.DropDownClosed
        SetAutoComplete(sender, True)
    End Sub
    Private Sub CbTimeZone_TextChanged(sender As Object, e As EventArgs) Handles cbTimeZone.TextChanged
        'Get ID from DisplayName
        TempTimeZone = Nothing
        Dim AllTimeZones As ObjectModel.ReadOnlyCollection(Of TimeZoneInfo) = TimeZoneInfo.GetSystemTimeZones
        For Each TZ In AllTimeZones
            If TZ.DisplayName = cbTimeZone.Text Then
                TempTimeZone = TZ
                Exit For
            End If
        Next
        'Debug.Print(TempTimeZone.DisplayName)
    End Sub
    Private Sub cbTimeZone_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbTimeZone.KeyPress, tblatitude.KeyPress, tblongitude.KeyPress, tbElevation.KeyPress
        e.Handled = True
    End Sub
    Sub FrmSchedule_change_place(ByVal num As Integer)
        If varSC.Location.Count < 1 Then Exit Sub
        Dim SelectedPlace As Object
        Try
            If varSC.PlaceListInHebrew = True Then
                SelectedPlace = varHebPlaceList.Item(num)
                cbLocationList.Text = If(SelectedPlace.HebName <> "", SelectedPlace.HebName & " | " & SelectedPlace.EngName, SelectedPlace.EngName)
            Else
                SelectedPlace = varEngPlaceList.Item(num)
                cbLocationList.Text = SelectedPlace.EngName & If(SelectedPlace.HebName <> "", " | " & SelectedPlace.HebName, "")
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        tblatitude.Text = SelectedPlace.Latitude
        tblongitude.Text = SelectedPlace.Longitude
        tbElevation.Text = SelectedPlace.Elevation
        tbzone.Text = SelectedPlace.TimeOffset

        'the is a saved time zone id
        If SelectedPlace.TimeZoneId <> "" Then
            cbTimeZone.Text = TimeZoneInfo.FindSystemTimeZoneById(SelectedPlace.TimeZoneId).DisplayName
            'leting CbTimeZone_TextChanged take care of it
        Else
            'set to nothing so Place_orDate_changed() should change according to timeoffset
            cbTimeZone.Text = ""
        End If

        'set to varSC
        varSC.SchedulerLocation = SelectedPlace.EngName
        varSC.SchedulerLatitude = SelectedPlace.Latitude
        varSC.SchedulerLongitude = SelectedPlace.Longitude
        varSC.SchedulerElevation = If(varSC.CalculateElevation = True, SelectedPlace.Elevation, 0)
        varSC.SchedulerTimeZone = TempTimeZone.Id
        varSC.Save(varUserFile)

    End Sub
    Private Sub tbReminderNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbReminderNum.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then e.KeyChar = ""
    End Sub
    Private Sub tbReminderTotal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbReminderTotal.KeyPress
        e.KeyChar = ""
    End Sub
    Private Sub tbReminderNum_TextChanged(sender As Object, e As EventArgs) Handles tbReminderNum.TextChanged
        'Debug.Print("tbReminderNum.TextChanged")
        If tbReminderNum.Text = "" Then Exit Sub
        If varSC.Schedule.Count < 1 Then
            TimechangedProgrammatic = True
            dtpTime.Value = #1/2/2000# & " 12:00:00 pm"
            'not to loop
            If tbReminderNum.Text = 1 Then Exit Sub
            tbReminderNum.Text = 1
            Exit Sub
        End If

        'move tbReminderTotal it this is +1 from count
        If tbReminderNum.Text = (varSC.Schedule.Count + 1) Then tbReminderTotal.Text = varSC.Schedule.Count + 1
        If tbReminderNum.Text = varSC.Schedule.Count Then tbReminderTotal.Text = varSC.Schedule.Count

        'if number is more than total by 1 then make empty 
        If tbReminderNum.Text = (varSC.Schedule.Count + 1) Then
            TabControlTimeZman.SelectedTab = TabPageZman
            cbZman.Text = ""
            TimechangedProgrammatic = True
            dtpTime.Value = #1/2/2000# & " 12:00:00 pm"
            tbMessage.Text = ""
            MinuteschangedProgrammatic = True
            dtpMinutes.Value = #1/2/2000#
            tbSound.Text = ""
            cbIsActive.Checked = False
            cbNotToday.Checked = False
            'exit and this will not fire agian as no change has been made to tbReminderNum
            Exit Sub
        End If

        'if number is more than total by more then 1 move number To last
        If tbReminderNum.Text > (varSC.Schedule.Count + 1) Then
            tbReminderNum.Text = varSC.Schedule.Count
            'exit now and event will fire again and then set fields and tbReminderTotal
            Exit Sub
        End If

        'only get here if number is some were in list
        If varSC.Schedule(tbReminderNum.Text - 1).IsFunc = True Then
            cbZman.Text = varSC.Schedule(tbReminderNum.Text - 1).Time
            TabControlTimeZman.SelectedTab = TabPageZman
            TimechangedProgrammatic = True
            dtpTime.Value = #1/2/2000# & " 12:00:00 pm"
        Else
            TimechangedProgrammatic = True
            dtpTime.Value = #1/2/2000# & " " & varSC.Schedule(tbReminderNum.Text - 1).Time
            'change tab after time is set so Parse_ZmanOrTime ahould be on time saved by user
            TabControlTimeZman.SelectedTab = TabPageTime
            cbZman.Text = ""
        End If
        tbMessage.Text = varSC.Schedule(tbReminderNum.Text - 1).Message
        MinuteschangedProgrammatic = True
        dtpMinutes.Value = #1/2/2000#.AddMinutes(varSC.Schedule(tbReminderNum.Text - 1).Minutes)
        cbIsActive.Checked = varSC.Schedule(tbReminderNum.Text - 1).IsActive
        cbNotToday.Checked = varSC.Schedule(tbReminderNum.Text - 1).NotToday
        tbSound.Text = varSC.Schedule(tbReminderNum.Text - 1).Sound
    End Sub
    Function CheckValidTime(StringIn As String, isFunc As Boolean)
        Dim mytime As Date
        If isFunc = False Then
            If DateTime.TryParse(StringIn, mytime) = False Then
                Return 2
            End If
        Else
            If IsNumeric(tblongitude.Text) = False Or IsNumeric(tblatitude.Text) = False Or TempTimeZone Is Nothing Then Return 3  '
            Try
                Dim timeZone As ITimeZone = New PZmanimTimeZone(varZmanTimeZone)
                Dim location As New GeoLocation("", tblatitude.Text, tblongitude.Text, 0, timeZone)
                TempCZC = New ComplexZmanimCalendar(Now, location)
                Dim Args() = {Now, location} ' for AddedGets
                If InStr(StringIn, "ZmanGet") Then
                    mytime = CDate(CallByName(varAddedGets, StringIn, CallType.Get, Args))
                    'Debug.Print(mytime)
                Else
                    mytime = CDate(CallByName(TempCZC, StringIn, CallType.Get))
                    'Debug.Print(mytime)
                End If
            Catch
                Return 4
            End Try
        End If
        Return 1
    End Function
    Private Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click
        If tbReminderNum.Text > varSC.Schedule.Count Then
            Exit Sub
        Else
            tbReminderNum.Text += 1
        End If
    End Sub
    Private Sub btPrevious_Click(sender As Object, e As EventArgs) Handles btPrevious.Click
        If tbReminderNum.Text = 1 Then
            Exit Sub
        Else
            tbReminderNum.Text -= 1
        End If
    End Sub
    Private Sub btFrist_Click(sender As Object, e As EventArgs) Handles btFrist.Click
        tbReminderNum.Text = 1
    End Sub
    Private Sub btLast_Click(sender As Object, e As EventArgs) Handles btLast.Click
        tbReminderNum.Text = varSC.Schedule.Count
    End Sub
    Private Sub cbIsActive_CheckedChanged(sender As Object, e As EventArgs) Handles cbIsActive.CheckedChanged
        Dim Response
        If cbIsActive.Checked = True Then
            'Dim valid = CheckValidTime(cbZman.Text, cbZman.SelectedIndex > -1)
            Dim IsFunc As Boolean = CBool(TabControlTimeZman.SelectedTab.Name = "TabPageZman") 'true if tab is now TabPageZman
            Dim valid = CheckValidTime(If(IsFunc = True, cbZman.Text, dtpTime.Value.ToLongTimeString), IsFunc)
            If valid = 1 Then
                If varSC.Schedule.Count < tbReminderNum.Text Then
                    varSC.Schedule.Add(New aSchedule)
                End If
                varSC.Schedule(tbReminderNum.Text - 1).IsActive = cbIsActive.Checked
                varSC.Save(varUserFile)
                'ask to add task
                If System.IO.File.Exists(Environment.GetEnvironmentVariable("systemroot") & "\System32\Tasks\MyZmanScheduler") = False Then
                    Response = msgMaker(If(varSC.HebrewMenus = True, ".מתזמן המשימות של ווינדוס לא מוגדר" & vbCr & "?האם ברצונך להוסיף את המשימה", "Windows task scheduler not set." & vbCr & "Do you want to add the task?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
                End If
                If Response = vbYes Then btToggleTaskScheduler_Click()
                Exit Sub

            ElseIf valid = 2 Then
                Response = msgMaker(If(varSC.HebrewMenus = True, ".אינו שעה תקף" & vbCr & "?האם ברצונך לנקות תזכורת זו", "Not a valid time." & vbCr & "Do you want to clear this Reminder?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
            ElseIf valid = 3 Then
                Response = msgMaker(If(varSC.HebrewMenus = True, ".חסרים פרטי מיקום" & vbCr & "?האם ברצונך לנקות תזכורת זו", "Location info Missing." & vbCr & "Do you want to clear this Reminder?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
            ElseIf valid = 4 Then
                Response = msgMaker(If(varSC.HebrewMenus = True, ".בעיה להשיג את הזמן הזה" & vbCr & "?האם ברצונך לנקות תזכורת זו", "Problem getting this zman." & vbCr & "Do you want to clear this Reminder?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
            End If
            cbIsActive.Checked = False
            If Response = vbNo Then Exit Sub
            'remove
            rbtClear_Click()
        End If
        If varSC.Schedule.Count >= tbReminderNum.Text Then
            varSC.Schedule(tbReminderNum.Text - 1).IsActive = cbIsActive.Checked
            varSC.Save(varUserFile)
        End If
    End Sub
    Function msgMaker(msgString As String, Optional BoxStyle As MsgBoxStyle = MsgBoxStyle.OkOnly, Optional CenteredStyle As String = "MouseCenter")
        Dim Response
        Using New Centered_MessageBox(Me, CenteredStyle)
            Response = MsgBox(msgString, BoxStyle)
        End Using
        Return Response
    End Function
    Private Sub cbNotToday_CheckedChanged(sender As Object, e As EventArgs) Handles cbNotToday.CheckedChanged
        If varSC.Schedule.Count >= tbReminderNum.Text Then varSC.Schedule(tbReminderNum.Text - 1).NotToday = cbNotToday.Checked
        varSC.Save(varUserFile)
    End Sub
    'Private Sub cbzman_TextChanged(sender As Object, e As EventArgs) Handles cbZman.TextChanged
    '    parse_cbTime()
    'End Sub
    Private Sub cbzman_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbZman.DropDown
        SetAutoComplete(sender, False)
    End Sub
    Private Sub Cbzman_DropDownclosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbZman.DropDownClosed
        SetAutoComplete(sender, True)
        If sender.SelectedIndex > -1 Then Parse_ZmanOrTime(True)
    End Sub
    Private Sub Cbzman_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbZman.SelectedIndexChanged
        Parse_ZmanOrTime(True)
    End Sub
    Private Sub cbZman_Leave(sender As Object, e As EventArgs) Handles cbZman.Leave
        'check if user typed in not a valid zman name
        Dim valid = CheckValidTime(cbZman.Text, True)
        If valid <> 1 Then cbZman.Text = ""
    End Sub
    Private Sub TabControlTimeZman_Selected(sender As Object, e As TabControlEventArgs) Handles TabControlTimeZman.Selected
        If CBool(TabControlTimeZman.SelectedTab.Name = "TabPageZman") = True Then 'true if tab is now TabPageZman
            If cbZman.Text <> "" Then Parse_ZmanOrTime(True)
        Else
            Parse_ZmanOrTime(False)
        End If
    End Sub
    Private Sub dtpTime_ValueChanged(sender As Object, e As EventArgs) Handles dtpTime.ValueChanged
        If TimechangedProgrammatic = False Then
            Parse_ZmanOrTime(False)
        Else
            TimechangedProgrammatic = False
        End If
    End Sub
    Private Sub dtpMinutes_ValueChanged(sender As Object, e As EventArgs) Handles dtpMinutes.ValueChanged
        If MinuteschangedProgrammatic = False Then
            Dim IsFunc As Boolean = CBool(TabControlTimeZman.SelectedTab.Name = "TabPageZman") 'true if tab is now TabPageZman
            Dim valid = CheckValidTime(If(IsFunc = True, cbZman.Text, dtpTime.Value.ToLongTimeString), IsFunc)
            If valid = 1 Then
                If varSC.Schedule.Count < tbReminderNum.Text Then
                    varSC.Schedule.Add(New aSchedule)
                End If
                varSC.Schedule(tbReminderNum.Text - 1).Minutes = dtpMinutes.Value.TimeOfDay.TotalMinutes
                varSC.Save(varUserFile)
            End If
        Else
            MinuteschangedProgrammatic = False
        End If
    End Sub
    Private Function Parse_ZmanOrTime(IsFunc As Boolean)
        Dim Response, valid
        'Dim valid = CheckValidTime(cbZman.Text, cbZman.SelectedIndex > -1)
        If IsFunc = True Then
            valid = CheckValidTime(cbZman.Text, True)
        Else
            valid = CheckValidTime(dtpTime.Value.ToLongTimeString, False)
        End If
        If valid = 1 Then
            If varSC.Schedule.Count < tbReminderNum.Text Then
                varSC.Schedule.Add(New aSchedule)
            End If
            varSC.Schedule(tbReminderNum.Text - 1).Time = If(IsFunc = True, cbZman.Text, dtpTime.Value.ToLongTimeString)
            varSC.Schedule(tbReminderNum.Text - 1).IsFunc = IsFunc
            varSC.Save(varUserFile)
            Return valid
        ElseIf valid = 2 Then
            Response = msgMaker(If(varSC.HebrewMenus = True, ".אינו שעה תקף" & vbCr & "?האם ברצונך לנקות תזכורת זו", "Not a valid time." & vbCr & "Do you want to clear this Reminder?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
        ElseIf valid = 3 Then
            Response = msgMaker(If(varSC.HebrewMenus = True, ".חסרים פרטי מיקום" & vbCr & "?האם ברצונך לנקות תזכורת זו", "Location info Missing." & vbCr & "Do you want to clear this Reminder?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
        ElseIf valid = 4 Then
            Response = msgMaker(If(varSC.HebrewMenus = True, ".בעיה להשיג את הזמן הזה" & vbCr & "?האם ברצונך לנקות תזכורת זו", "Problem getting this zman." & vbCr & "Do you want to clear this Reminder?"), If(varSC.HebrewMenus = True, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.YesNo + MsgBoxStyle.Question))
        End If
        cbZman.Text = ""
        If varSC.Schedule.Count >= tbReminderNum.Text Then varSC.Schedule(tbReminderNum.Text - 1).Time = cbZman.Text

        If Response = vbNo Then Return valid
        'remove
        rbtClear_Click()
        Return valid
    End Function
    Private Sub tbMessage_TextChanged(sender As Object, e As EventArgs) Handles tbMessage.TextChanged
        Dim IsFunc As Boolean = CBool(TabControlTimeZman.SelectedTab.Name = "TabPageZman") 'true if tab is now TabPageZman
        Dim valid = CheckValidTime(If(IsFunc = True, cbZman.Text, dtpTime.Value.ToLongTimeString), IsFunc)
        If valid = 1 Then
            If varSC.Schedule.Count < tbReminderNum.Text Then
                varSC.Schedule.Add(New aSchedule)
            End If
            varSC.Schedule(tbReminderNum.Text - 1).Message = tbMessage.Text
            varSC.Save(varUserFile)
        End If
    End Sub
    Private Sub btFileDialog_Click(sender As Object, e As EventArgs) Handles btFileDialog.Click
        Dim My_FileDialog As New OpenFileDialog()
        My_FileDialog.Filter = "Wav files (*.wav)|*.wav|All files (*.*)|*.*"
        My_FileDialog.FilterIndex = 1
        My_FileDialog.InitialDirectory = Environment.GetEnvironmentVariable("systemroot") & "\Media"
        If My_FileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If System.IO.Path.GetExtension(My_FileDialog.FileName) <> ".wav" Then
                msgMaker(If(varSC.HebrewMenus = True, "אפשר רק קבצי wav", "only .wav files allowed."), If(varSC.HebrewMenus = True, MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.OkOnly + MsgBoxStyle.Information), "ParentCenter")
            Else
                tbSound.Text = My_FileDialog.FileName
                'to update varSC
                parse_tbSound()
            End If
        End If
    End Sub
    Private Sub tbSound_Leave(sender As Object, e As EventArgs) Handles tbSound.Leave
        If tbSound.Text <> "" Then parse_tbSound()
    End Sub
    Private Sub tbSound_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbSound.KeyPress
        If e.KeyChar = Chr(13) Then
            parse_tbSound()
        Else
            If varSC.Schedule.Count >= tbReminderNum.Text Then varSC.Schedule(tbReminderNum.Text - 1).Sound = ""
        End If
    End Sub
    Private Sub parse_tbSound()
        If System.IO.Path.GetExtension(tbSound.Text) <> ".wav" Then
            msgMaker(If(varSC.HebrewMenus = True, "אפשר רק קבצי wav", "only .wav files allowed."), If(varSC.HebrewMenus = True, MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight, MsgBoxStyle.OkOnly + MsgBoxStyle.Information), "ParentCenter")
            tbSound.Text = ""
            If varSC.Schedule.Count >= tbReminderNum.Text Then varSC.Schedule(tbReminderNum.Text - 1).Sound = ""
        Else
            Dim IsFunc As Boolean = CBool(TabControlTimeZman.SelectedTab.Name = "TabPageZman") 'true if tab is now TabPageZman
            Dim valid = CheckValidTime(If(IsFunc = True, cbZman.Text, dtpTime.Value.ToLongTimeString), IsFunc)
            If valid = 1 Then
                If varSC.Schedule.Count < tbReminderNum.Text Then
                    varSC.Schedule.Add(New aSchedule)
                End If
                varSC.Schedule(tbReminderNum.Text - 1).Sound = tbSound.Text
            End If
        End If
    End Sub
    Private Sub btTest_Click(sender As Object, e As EventArgs) Handles btTest.Click
        'test time with msg
        Dim valid = Parse_ZmanOrTime(CBool(TabControlTimeZman.SelectedTab.Name = "TabPageZman")) 'true if tab is now TabPageZman
        If valid <> 1 Then Exit Sub

        cbNotToday.Checked = CheckSchedule(varSC.Schedule(tbReminderNum.Text - 1), True)

    End Sub
    Private Sub rbtClear_Click() Handles rbtClear.Click
        If varSC.Schedule.Count >= tbReminderNum.Text Then
            varSC.Schedule.Items.RemoveAt(tbReminderNum.Text - 1)
            Dim savenum = tbReminderNum.Text
            tbReminderTotal.Text = varSC.Schedule.Count
            tbReminderNum.Text = ""
            tbReminderNum.Text = savenum
            'clean up will be done by tbReminderNum text change
            If varSC.Schedule.Count > 0 Then Exit Sub
        End If
        cbIsActive.Checked = False
        cbNotToday.Checked = False
        cbZman.Text = ""
        TabControlTimeZman.SelectedTab = TabPageZman
        tbMessage.Text = ""
        dtpMinutes.Value = #1/2/2000#
        tbSound.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Process.Start("taskschd.msc")
    End Sub

    Private Sub btToggleTaskScheduler_Click() Handles btToggleTaskScheduler.Click
        If System.IO.File.Exists(Environment.GetEnvironmentVariable("systemroot") & "\System32\Tasks\MyZmanScheduler") = True Then
            'remove
            pbTasksON.Image = My.Resources.red_16
            btToggleTaskScheduler.Text = "Add Task"
            If Frminfo.mHebrewMenus.Checked = True Then btToggleTaskScheduler.Text = "הוסיף משימה"
            Shell("SCHTASKS /Delete /TN MyZmanScheduler /F")
        Else
            'add
            If NoActiveReminders() = False Or varSC.Schedule.Count < 1 Then
                Using New Centered_MessageBox(Me, "MouseCenter")
                    If varSC.HebrewMenus = True Then
                        MsgBox("!לא נקבעו תזכורות פעילות", MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight)
                    Else
                        MsgBox("No active reminders set!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                    End If
                End Using
                Exit Sub
            End If
            If varSC.SchedulerLongitude = "" Or varSC.SchedulerLatitude = "" Or varSC.SchedulerTimeZone = "" Then
                Using New Centered_MessageBox(Me, "MouseCenter")
                    If varSC.HebrewMenus = True Then
                        MsgBox("!חסרים פרטי מיקום", MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight)
                    Else
                        MsgBox("Missing Location Info!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                    End If
                End Using
                Exit Sub
            End If
            pbTasksON.Image = My.Resources.green_16
            btToggleTaskScheduler.Text = "Remove Task"
            If Frminfo.mHebrewMenus.Checked = True Then btToggleTaskScheduler.Text = "הסר משימה"
            makexml()
            Shell("SCHTASKS /Create /TN MyZmanScheduler /F /XML " & """" & Environment.CurrentDirectory & "\MyZmanScheduler.xml" & """")
            'removing it here dose not let SCHTASKS time to do its job - use a timer
            Dim WaitUntil As Date = Now.AddSeconds(5)
            Do Until Now > WaitUntil
                If System.IO.File.Exists(Environment.GetEnvironmentVariable("systemroot") & "\System32\Tasks\MyZmanScheduler") = True Then Exit Do
                System.Windows.Forms.Application.DoEvents()
            Loop
            My.Computer.FileSystem.DeleteFile(Environment.CurrentDirectory & "\MyZmanScheduler.xml")
        End If
    End Sub
    Private Sub btOpenScheduler_Click(sender As Object, e As EventArgs) Handles btOpenScheduler.Click
        Process.Start("taskschd.msc")
    End Sub
    Private Sub GroupBox2_Paint(sender As Object, e As PaintEventArgs) Handles GroupBox2.Paint
        Using borderPen As Pen = New Pen(Color.LightGray)
            e.Graphics.DrawRectangle(borderPen, New Rectangle(tbMessage.Location.X - 1, tbMessage.Location.Y - 1, tbMessage.Width + 1, tbMessage.Height + 1))
            e.Graphics.DrawRectangle(borderPen, New Rectangle(tbSound.Location.X - 1, tbSound.Location.Y - 1, tbSound.Width + 1, tbSound.Height + 1))
            e.Graphics.DrawRectangle(borderPen, New Rectangle(tbReminderTotal.Location.X - 1, tbReminderTotal.Location.Y - 1, tbReminderTotal.Width + 1, tbReminderTotal.Height + 1))
            e.Graphics.DrawRectangle(borderPen, New Rectangle(tbReminderNum.Location.X - 1, tbReminderNum.Location.Y - 1, tbReminderNum.Width + 1, tbReminderNum.Height + 1))
            e.Graphics.DrawRectangle(borderPen, New Rectangle(PanelOnTimeBefor.Location.X - 1, PanelOnTimeBefor.Location.Y - 1, PanelOnTimeBefor.Width + 1, PanelOnTimeBefor.Height + 1))
            e.Graphics.DrawRectangle(borderPen, New Rectangle(cbLocationList.Location.X - 1, cbLocationList.Location.Y - 1, cbLocationList.Width + 1, cbLocationList.Height + 1))
        End Using
    End Sub
    'Private Sub tabControl1_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles TabControlTimeZman.DrawItem
    '    '    Dim page As TabPage = TabControl1.TabPages(e.Index)
    '    '    Dim col As Color = Color.AliceBlue
    '    '    e.Graphics.FillRectangle(New SolidBrush(col), e.Bounds)
    '    '    Dim paddedBounds As Rectangle = e.Bounds
    '    '    Dim yOffset As Integer = If((e.State = DrawItemState.Selected), -2, 1)
    '    '    paddedBounds.Offset(1, yOffset)
    '    '    TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor)
    'End Sub

End Class

''draw dots
'Imports System.Drawing.Drawing2D
'Dim myBrush As System.Drawing.SolidBrush = New System.Drawing.SolidBrush(System.Drawing.Color.Red)
'Dim formGraphics As System.Drawing.Graphics
'formGraphics = PictureBox1.CreateGraphics() 'Me.CreateGraphics()
'formGraphics.InterpolationMode = InterpolationMode.Bilinear
'formGraphics.PixelOffsetMode = PixelOffsetMode.Half
'formGraphics.SmoothingMode = SmoothingMode.None
'formGraphics.FillEllipse(myBrush, 0, 0, 16, 16) 'New Rectangle(0, 0, 20, 20)
'myBrush.Dispose()
'formGraphics.Dispose()




'snd = New SoundPlayer(My.Resources.Ring08)
'snd.Play()
'SystemSounds.Hand.Play()