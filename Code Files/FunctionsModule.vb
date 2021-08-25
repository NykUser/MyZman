'left-to-right mark: ChrW(&H200E) 
'right-to-left mark: ChrW(&H200F)

'needs to be Module it should be global
Module FunctionsModule
    Public Sub LoadSettingsandVariables()
        'load SettingsCollection from xml file
        If My.Computer.FileSystem.FileExists(varUserFile) Then
            varSC = SerializableData.Load(varUserFile, GetType(SettingsCollection))
        End If

        'Frminfo_Load Location and size
        If varSC.LocationX > -1 And varSC.LocationY > 1 Then
            Frminfo.Location = New System.Drawing.Point(varSC.LocationX, varSC.LocationY)
            'the is a check on Screen.PrimaryScreen.Bounds in Frminfo_Load
        End If
        If varSC.SizeH > -1 And varSC.SizeW > -1 Then
            Frminfo.Size = New System.Drawing.Size(varSC.SizeW, varSC.SizeH)
        End If
        If varSC.DataGridCol1W > -1 Then Frminfo.DataGridView1.Columns(1).Width = varSC.DataGridCol1W
        If varSC.DataGridCol2W > -1 Then Frminfo.DataGridView1.Columns(2).Width = varSC.DataGridCol2W

        Frminfo.mColorZmanMenuItem.Checked = varSC.ColorZman
        Frminfo.mStayOnTopToolStripMenuItem.Checked = varSC.StayOnTop
        Frminfo.mUseUSNO.Checked = varSC.UseOlderUsnoAlgorithm
        Frminfo.mCalculateElevation.Checked = varSC.CalculateElevation
        Frminfo.m24HourFormatTime.Checked = varSC.Clock24Hour
        Frminfo.mAskWhenChanging.Checked = varSC.AskWhenChanging
        Frminfo.mBackUpWhenChanging.Checked = varSC.BackUpWhenChanging
        Frminfo.mChangeKeybordLayout.Checked = varSC.ChangeKeybordLayout
        Frminfo.mShowTimesOnStatusBar.Checked = varSC.ShowTimesOnStatusBar
        Frminfo.mStayOnTopToolStripMenuItem.Checked = varSC.StayOnTop
        Frminfo.mShowTooltips.Checked = varSC.ShowTooltips
        Frminfo.StayOnTopToolStripMenuItem_Click()
        Frminfo.Opacity = varSC.TransparencyValue
        Frminfo.mPlaceListInHebrew.Checked = varSC.PlaceListInHebrew
        Frminfo.mLineBetweenZmanim.Checked = varSC.LineBetweenZmanim
        Frminfo.mIsraeliYomTov.Checked = varSC.IsraeliYomTov


        Frminfo.mHebrewMenus.Checked = varSC.HebrewMenus
        'don't force Hebrew Menus if it was changed by user on last run
        If varSC.MenuLanguageWasChanged = False Then If InStr(CultureInfo.CurrentCulture.Name, "he-IL") Then Frminfo.mHebrewMenus.Checked = True
        Frminfo.mHebrewMenus_Click()

        If varSC.DefaultType = "Default" Then
            Frminfo.mPlaceListInHebrew.Checked = varSC.DefaultPlaceListInHebrew
            Frminfo.mPlaceListInHebrew_Click()
            varSC.LastSelectedIndex = varSC.DefaultSelectedindex
        End If


        'TimerLocationsLoad will load PlaceList into dropdownbox, it calls LoadPlaceLists()

        If varSC.Zmanim.Items.Count < 1 Then
            UseDefultzmnaim()
        End If

        Load_Zmanim_Func()

    End Sub
    Public Sub LoadPlaceLists()
        'load into memory sorted eng & heb list
        'varEngPlaceList = Locations_List.AllLocations 'AllLocations gets it in eng order

        'user list empty 
        If varSC.Location.Count < 1 Then
            If My.Computer.FileSystem.FileExists(varLocationsFile) = False Then
                Using New Centered_MessageBox(Frminfo, "ParentCenter")
                    If varSC.HebrewMenus = True Then
                        MsgBox(":לא מצליח למצוא קובץ מיקומים" & vbCr & varLocationsFile, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxRight)
                    Else
                        MsgBox("Can't find Locations File:" & vbCr & varLocationsFile, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                    End If
                End Using
                Exit Sub
            End If
            UseDefultLocationsFile()
        End If

        For Each Li As aLocation In varSC.Location.OrderBy(Function(temp) temp.EngName)
            varEngPlaceList.Add(New aLocation(Li))
        Next
        For Each Li As aLocation In varSC.Location.OrderBy(Function(temp) temp.HebName)
            varHebPlaceList.Add(New aLocation(Li))
        Next

        If varSC.PlaceListInHebrew = False Then
            Frminfo.cbLocationList.RightToLeft = 0
            For Each LI As aLocation In varEngPlaceList
                'ToolStripProgressBar1.Value = ToolStripProgressBar1.Value + 1
                Frminfo.cbLocationList.Items.Add(LI.EngName & If(LI.HebName <> "", " | " & LI.HebName, ""))
            Next
        Else
            Frminfo.cbLocationList.RightToLeft = 1
            For Each LI As aLocation In varHebPlaceList
                Frminfo.cbLocationList.Items.Add(If(LI.HebName <> "", LI.HebName & " | " & LI.EngName, LI.EngName))
            Next
        End If
    End Sub
    Public Sub UseDefultLocationsFile()
        Dim TempPlaceList As LocationCollection
        TempPlaceList = SerializableData.Load(varLocationsFile, GetType(LocationCollection))
        For Each Li As aLocation In TempPlaceList.OrderBy(Function(temp) temp.EngName)
            varSC.Location.Add(New aLocation(Li))
        Next
        varSC.Save(varUserFile)
    End Sub
    Public Sub UseDefultzmnaim()
        varSC.Zmanim.Items.Clear()
        Dim MyZmanim = DefaultZmanim()
        MyZmanim.ForEach(Sub(x) varSC.Zmanim.Add(x))
    End Sub
    Sub ClearAndReLoadPlaceLists(Optional SelectName As String = "", Optional SelectNum As Integer = 0)
        varFinishedLoading = False
        'reset all Location lists
        Frminfo.cbLocationList.Items.Clear()
        Frminfo.cbLocationList.Text = ""
        varEngPlaceList.Items.Clear()
        varHebPlaceList.Items.Clear()

        'ReLoadPlace Lists
        LoadPlaceLists()

        varSC.LastSelectedIndex = SelectNum

        'find a name's num
        If SelectName <> "" Then
            Dim counter As Integer = 0
            If varSC.PlaceListInHebrew = True Then
                For Each LI As aLocation In varHebPlaceList
                    If LI.EngName = SelectName Then
                        Exit For
                    End If
                    counter += 1
                Next
            Else
                For Each LI As aLocation In varEngPlaceList
                    If LI.EngName = SelectName Then
                        Exit For
                    End If
                    counter += 1
                Next
            End If
            Debug.Print(counter)
            varSC.LastSelectedIndex = counter
        End If

        If varSC.LastSelectedIndex <> "-1" Then
            change_place(varSC.LastSelectedIndex)
        End If
        varFinishedLoading = True
        Place_orDate_changed()
    End Sub
    Public Sub Load_Zmanim_Func()
        varZmanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
        Dim timeZone As ITimeZone = New PZmanimTimeZone(varZmanTimeZone) 'WindowsTimeZone("Eastern Standard Time") '
        Dim location As New GeoLocation("", 35, 31, 0, timeZone)
        varCZC = New ComplexZmanimCalendar(Now, location)

        Dim assembly As Reflection.Assembly = Reflection.Assembly.GetAssembly(GetType(Zmanim.ComplexZmanimCalendar))
        For Each t As Type In assembly.GetTypes()
            If t.ToString() = "Zmanim.ComplexZmanimCalendar" Then
                For Each m In t.GetMembers.OrderBy(Function(temp) temp.Name)
                    If Check_valid_zman(m.Name, True) = True Then
                        varZmanimFunc.Add(New String(m.Name))
                    Else
                        'to add ShaahZmanis
                        If InStr(m.Name, "ShaahZmanis") Then varZmanimFunc.Add(New String(m.Name))
                    End If
                Next
            End If
        Next
        'add AddedGets
        varZmanimFunc.Add("ZmanGetMisheyakir50")
        varZmanimFunc.Add("ZmanGetPlagHamincha8Point5Degrees")
        varZmanimFunc.Add("ZmanGetBeinHashmases8Point5Degrees13Min")
        varZmanimFunc.Add("ZmanGetBeinHashmases8Point5Degrees5Point3")
        'varZmanimFunc.ForEach(Sub(x) Debug.Print(x))
    End Sub
    Public Function Check_valid_zman(ByVal s As String, ByVal checkIsDate As Boolean)
        If varZmanTimeZone Is Nothing Then
            Return False
            Exit Function
        End If
        Dim mytime As Date
        Try
            'Debug.Print(s)
            s = CallByName(varCZC, s, CallType.Get)
            'Debug.Print(s)
            If checkIsDate = True Then If DateTime.TryParse(s, mytime) = False Then Return False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Sub change_place(ByVal num As Integer)
        If varSC.Location.Count < 1 Then Exit Sub
        'Debug.Print(num)
        Dim SelectedPlace As Object
        'if num is not valid with .Items - can happen when loading on start
        Try
            If varSC.PlaceListInHebrew = True Then
                SelectedPlace = varHebPlaceList.Item(num)
                Frminfo.cbLocationList.Text = If(SelectedPlace.HebName <> "", SelectedPlace.HebName & " | " & SelectedPlace.EngName, SelectedPlace.EngName)
            Else
                SelectedPlace = varEngPlaceList.Item(num)
                Frminfo.cbLocationList.Text = SelectedPlace.EngName & If(SelectedPlace.HebName <> "", " | " & SelectedPlace.HebName, "")
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        Frminfo.tblatitude.Text = SelectedPlace.Latitude
        Frminfo.tblongitude.Text = SelectedPlace.Longitude
        Frminfo.tbzone.Text = SelectedPlace.TimeOffset
        Frminfo.tbcountry.Text = SelectedPlace.Country
        Frminfo.tbElevation.Text = SelectedPlace.Elevation

        'varSC.LastSelectedIndex = num

        'the is a saved time zone id
        If SelectedPlace.TimeZoneId <> "" Then
            'Get DisplayName from ID - ID looks like standerd all over
            Frminfo.CbTimeZone.Text = TimeZoneInfo.FindSystemTimeZoneById(SelectedPlace.TimeZoneId).DisplayName
            'not seting ZmanTimeZone leting CbTimeZone_TextChanged take care of it
        Else
            'set to nothing so Place_orDate_changed() should change according to timeoffset
            Frminfo.CbTimeZone.Text = ""
        End If

        Place_orDate_changed()
    End Sub
    Sub Place_orDate_changed()
        If varFinishedLoading = False Then Exit Sub
        'Validated longitude & latitude info
        Dim testDouble As Double
        If Double.TryParse(Frminfo.tblongitude.Text, testDouble) = False Or Double.TryParse(Frminfo.tblatitude.Text, testDouble) = False Then
            'Frminfo.ListView1.Items.Clear()
            Frminfo.DataGridView1.Rows.Clear()
            Frminfo.TimerStatusLabel.Enabled = False
            varSavedStatusLabel = "Missing longitude or latitude info!"
            If varSC.HebrewMenus = True Then varSavedStatusLabel = "חסר מידע קו אורך או קו רוחב!"
            Frminfo.StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, Frminfo.StatusLabel.Font, Frminfo.StatusStrip1.Size.Width - 70)
            'wait 3s
            Frminfo.TimerStatusLabel.Interval = 3000
            Frminfo.TimerStatusLabel.Enabled = True
            Exit Sub
        End If
        If Double.TryParse(Frminfo.tbElevation.Text, testDouble) = False Then Frminfo.tbElevation.Text = 0

        If Frminfo.CbTimeZone.Text = "" Then 'dont change if the is a Time zone in CbTimeZone
            Try
                'varZmanTimeZone = TimeZoneInfo.FindSystemTimeZoneById(getnamebyTimeOffset(Frminfo.tbzone.Text, True))
                'using now GeoTimeZone.TimeZoneLooku
                Dim IANATimezone As String = GeoTimeZone.TimeZoneLookup.GetTimeZone(Frminfo.tblatitude.Text, Frminfo.tblongitude.Text).Result
                Dim WinTimezone As String = TimeZoneConverter.TZConvert.IanaToWindows(IANATimezone)
                varZmanTimeZone = TimeZoneInfo.FindSystemTimeZoneById(WinTimezone)
                Frminfo.CbTimeZone.Text = varZmanTimeZone.DisplayName
                Frminfo.tbzone.Text = TimeZoneInfo.FindSystemTimeZoneById(WinTimezone).BaseUtcOffset.TotalHours
            Catch ex As Exception
                varZmanTimeZone = Nothing
            End Try
        End If

        'Validated Time Zone info
        If varZmanTimeZone Is Nothing Then
            'Frminfo.ListView1.Items.Clear()
            Frminfo.DataGridView1.Rows.Clear()
            Frminfo.TimerStatusLabel.Enabled = False
            varSavedStatusLabel = "Time Zone not recognised!"
            If varSC.HebrewMenus = True Then varSavedStatusLabel = "אזור הזמן לא מזוהה!"
            Frminfo.StatusLabel.Text = TrimStringEllipsis(varSavedStatusLabel, Frminfo.StatusLabel.Font, Frminfo.StatusStrip1.Size.Width - 70)
            'wait 3s
            Frminfo.TimerStatusLabel.Interval = 3000
            Frminfo.TimerStatusLabel.Enabled = True
            Exit Sub
        End If
        Frminfo.TimerStatusLabel.Interval = 250

        'convert from win time zone to zmanim project time zone
        Dim timeZone As ITimeZone = New PZmanimTimeZone(varZmanTimeZone) 'New WindowsTimeZone(varZmanTimeZone)
        varGeoLocation = New GeoLocation(Frminfo.cbLocationList.Text, Frminfo.tblatitude.Text, Frminfo.tblongitude.Text, If(Frminfo.mCalculateElevation.Checked = True, Frminfo.tbElevation.Text, 0), timeZone)
        varCZC = New ComplexZmanimCalendar(Frminfo.dpEngdate.Value, varGeoLocation)

        If Frminfo.mUseUSNO.Checked = False Then
            'NOAA algorithm more accurate
            varCZC.AstronomicalCalculator = New NOAACalculator()
        Else
            varCZC.AstronomicalCalculator = New ZmanimCalculator() ' USNO older
        End If

        change_zman()
        'this is for the hidden drop down lists
        run_difference()
    End Sub
    Sub change_zman()
        'DataGridViewComboBoxCell needs items for it to be able to be set to its value - not like ComboBox

        If varSC.Zmanim.Count < 1 Then Exit Sub
        If varFinishedLoading = False Then Exit Sub
        'dont let DataGridView1 Events recall this
        varFinishedLoading = False

        Dim mySelectedRow As Integer = 0
        'If Frminfo.DataGridView1.SelectedRows.Count > 0 Then mySelectedRow = Frminfo.DataGridView1.SelectedRows(0).Index
        'use what DataGridView1.RowEnter saved - good also for delete
        If varDataGridNumRow > 0 Then mySelectedRow = varDataGridNumRow

        Frminfo.TimerStatusLabel.Enabled = False
        Dim DVItems As New List(Of DataGridViewRow)
        Dim timeFormat As String = "h:mm:ss tt"
        If varSC.Clock24Hour = True Then timeFormat = "H:mm:ss"
        Dim MyZman As Date
        Dim MyTimeSpan As TimeSpan
        Dim Args() = {Frminfo.dpEngdate.Value, varGeoLocation} ' for AddedGets


        Dim i As Integer = 0
        For Each Z In varSC.Zmanim
            'Debug.Print(i)
            i += 1
            DVItems.Add(New DataGridViewRow())
            If InStr(Z.FunctionName, "ShaahZmanis") > 0 Then
                Try
                    MyZman = #12:00:00 AM#.AddMilliseconds(CallByName(varCZC, Z.FunctionName, CallType.Get))
                    DVItems(DVItems.Count - 1).CreateCells(Frminfo.DataGridView1, i, Z.DisplayName)
                    CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).Items.Add(MyZman.ToString("H:mm:ss"))
                Catch ex As Exception
                    DVItems(DVItems.Count - 1).CreateCells(Frminfo.DataGridView1, Z.DisplayName)
                    CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).Items.Add("")
                End Try
            Else
                Try
                    If Z.ObjectName = "varCZC" Then
                        MyZman = CDate(CallByName(varCZC, Z.FunctionName, CallType.Get)).ToString(timeFormat)
                    Else
                        MyZman = CDate(CallByName(varAddedGets, Z.FunctionName, CallType.Get, Args))
                    End If
                    DVItems(DVItems.Count - 1).CreateCells(Frminfo.DataGridView1, i, Z.DisplayName)
                    CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).Items.Add(ChrW(&H200E) & StrConv(MyZman.ToString(timeFormat), VbStrConv.Lowercase))
                Catch ex As Exception
                    DVItems(DVItems.Count - 1).CreateCells(Frminfo.DataGridView1, i, Z.DisplayName)
                    CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).Items.Add("")
                End Try
                'Color setings - are now done in TimerStatusLabel
            End If
            'set to not be bold
            DVItems(DVItems.Count - 1).DefaultCellStyle.Font = New Font(Frminfo.DataGridView1.Font, FontStyle.Regular)
            'selcet value for ComboBoxCell
            CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).Value = CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).Items(0)
            'set ToolTip's
            If varSC.ShowTooltips = True Then
                If varSC.HebrewMenus = True Then
                    CType(DVItems(DVItems.Count - 1).Cells(1), DataGridViewTextBoxCell).ToolTipText =  "לחץ פעמיים לעריכה" & vbCr & "גרור ושחרר לסידור מחדש" & vbCr & "קליק ימני לאפשרויות נוספות"
                Else
                    CType(DVItems(DVItems.Count - 1).Cells(1), DataGridViewTextBoxCell).ToolTipText = "Double Click to Edit" & vbCr & "Drag and Drop to Rearrange" & vbCr & "Right Click for More Options"
                End If
            End If
            CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewComboBoxCell).ToolTipText = Z.FunctionName
        Next

        Frminfo.DataGridView1.Rows.Clear()
        'Debug.Print(Frminfo.DataGridView1.Rows.Count & vbCr & DVItems.Count)
        Frminfo.DataGridView1.Rows.AddRange(DVItems.ToArray)
        Try
            Frminfo.DataGridView1.CurrentCell = Frminfo.DataGridView1(2, mySelectedRow)
        Catch ex As Exception
        End Try

        Frminfo.TimerStatusLabel.Enabled = True
        varFinishedLoading = True

        'using listview
        'Dim LVItems As New List(Of ListViewItem)
        'LVItems.Add(New ListViewItem(New String() {Z.DisplayName, MyZman.ToString("H:mm:ss")}))
        'If varSC.ColorZman = True And LVItems.Item(LVItems.Count - 1).SubItems(1).Text <> "" Then
        'LVItems.Item(LVItems.Count - 1).UseItemStyleForSubItems = False ' to be able to change things on sub items
        'LVItems.Item(LVItems.Count - 1).SubItems(1).Font = New Font(Frminfo.ListView1.Font, FontStyle.Regular) ' if not it whould go back to bold
        'If MyTimeSpan.TotalMilliseconds < 0 Then LVItems.Item(LVItems.Count - 1).SubItems(1).ForeColor = Color.Red '"עבר"
        'LVItems.Item(LVItems.Count - 1).Font = New Font(Frminfo.ListView1.Font, FontStyle.Regular)
        'this is done befor - save for exmple of ForEach
        'LVItems.ForEach(Sub(x) x.Font = New Font(ListView1.Font, FontStyle.Regular))
        'Frminfo.ListView1.Items.Clear()
        'Frminfo.ListView1.Items.AddRange(LVItems.ToArray)
    End Sub
    Sub run_difference()
        Dim Args() = {Frminfo.dpEngdate.Value, varGeoLocation} ' for AddedGets
        If varZmanTimeZone Is Nothing Then
            Exit Sub
        End If

        If FrmDifference.CbTimeA.Text <> "" Then
            If InStr(FrmDifference.CbTimeA.Text, "ZmanGet") Then
                FrmDifference.tbTimeA.Text = CDate(CallByName(varAddedGets, FrmDifference.CbTimeA.Text, CallType.Get, Args))
            Else
                FrmDifference.tbTimeA.Text = CDate(CallByName(varCZC, FrmDifference.CbTimeA.Text, CallType.Get))
            End If
        End If
        If FrmDifference.CbTimeB.Text <> "" Then
            If InStr(FrmDifference.CbTimeB.Text, "ZmanGet") Then
                FrmDifference.tbTimeB.Text = CDate(CallByName(varAddedGets, FrmDifference.CbTimeB.Text, CallType.Get, Args))
            Else
                FrmDifference.tbTimeB.Text = CDate(CallByName(varCZC, FrmDifference.CbTimeB.Text, CallType.Get))
            End If
        End If

        Dim myTimeSpan As TimeSpan
        If FrmDifference.tbTimeA.Text <> "" And FrmDifference.tbTimeB.Text <> "" Then
            Try
                If FrmDifference.CBMday.Checked = True Then
                    myTimeSpan = CDate(FrmDifference.tbTimeB.Text) - CDate(FrmDifference.tbTimeA.Text).AddDays(-1)
                Else
                    myTimeSpan = CDate(FrmDifference.tbTimeB.Text) - CDate(FrmDifference.tbTimeA.Text)
                End If
                'tbTimeR.Text = #12:00:00 AM#.AddMilliseconds(myTimeSpan.TotalMilliseconds / 12).ToString("hh:mm:ss")
                'tbTimeR.Text = myTimeSpan.TotalHours & ":" & myTimeSpan.TotalMinutes & ":" & myTimeSpan.TotalSeconds
                FrmDifference.tbTimeR.Text = myTimeSpan.ToString
            Catch ex As Exception
            End Try
        End If
    End Sub
    Sub parse_hebdate()
        varHculture.DateTimeFormat.Calendar = varHC
        'Thread.CurrentThread.CurrentCulture = varHculture

        Dim myhebdate() As String = Split(Frminfo.rtbHebrewDate.Text, "/", -1)
        myhebdate(0) = Trim(myhebdate(0))

        Try
            DateTime.Parse(HebDateStringtoNom(myhebdate(0)), varHculture)
        Catch ex As Exception
            Using New Centered_MessageBox(Frminfo, "MouseCenter")
                MsgBox("!אי אפשר להחליף תאריך זה", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxRight) ' + MsgBoxStyle.MsgBoxRtlReading
            End Using

            change_hebdate()
            'hebdate.Text = engdate.Value.ToString("dd MMM yyy")
            Exit Sub
        End Try

        Dim mydate As DateTime = DateTime.Parse(HebDateStringtoNom(myhebdate(0)), varHculture)
        Frminfo.dpEngdate.Value = mydate '.ToString("ddddMM/dd/yyyy", New CultureInfo("en-US"))
    End Sub
    Sub change_hebdate()
        'Frminfo.dpEngdate.Value
        Dim ResultArray = Get_HebDate(Frminfo.dpEngdate.Value)
        Dim HebDatetoShow As String

        HebDatetoShow = ResultArray(0) & " / " & ResultArray(1)
        If ResultArray(2) <> " " Then HebDatetoShow = HebDatetoShow & " / " & ResultArray(2)
        If ResultArray(3) <> "קודם הדף" And ResultArray(3) <> "??" Then HebDatetoShow = HebDatetoShow & " / " & ResultArray(3)
        Frminfo.rtbHebrewDate.Text = HebDatetoShow
    End Sub
    Public Function Get_HebDate(DateIn As Date) As String()
        Dim Hebdate, Holiday, Hebday, Daf As String
        Dim JC As New JewishCalendar()
        Dim HDF As New HebrewDateFormatter()
        Dim YC As New YomiCalculator()
        HDF.HebrewFormat = True
        HDF.LongWeekFormat = False
        varHculture.DateTimeFormat.Calendar = varHC

        Hebdate = DateIn.ToString("dd MMM yyy", varHculture)
        Holiday = HDF.FormatYomTov(DateIn, False) & " " & HDF.FormatOmer(DateIn)
        If varSC.IsraeliYomTov = True Then Holiday = HDF.FormatYomTov(DateIn, True) & " " & HDF.FormatOmer(DateIn)
        Hebday = "יום " & HDF.FormatDayOfWeek(DateIn)

        Try
            If varSC.IsraeliYomTov = True Then
                Hebday = Hebday & " " & GetParshaNew(DateIn, True)
            Else
                Hebday = Hebday & " " & GetParshaNew(DateIn, False)
            End If
        Catch
        End Try

        If DateIn > #9/11/1923# Then
            Try
                'used to have a isue with some days, looks fine with newer YomiCalculator.cs
                Daf = HDF.FormatDafYomiBavli(YomiCalculator.GetDafYomiBavli(DateIn))
            Catch
                Daf = "??"
                GoTo A
            End Try
        Else
            Daf = "קודם הדף"
        End If
A:
        Return New String() {Hebdate, Hebday, Holiday, Daf}
        '0 Hebdate
        '1 Hebday of week and Parsha
        '2 Holiday
        '3 DafYomi

        'Debug.Print("ParshaIndex " & GetParshaNew(Frminfo.dpEngdate.Value, False, True))
        'Debug.Print("ParshaYearType | " & JC.GetweekforParshaAyg(Frminfo.dpEngdate.Value, False)(0))
        'Debug.Print("ParshaWeek | " & JC.GetweekforParshaAyg(Frminfo.dpEngdate.Value, False)(1))
        'Debug.Print("Net Parshah Name | " & JC.GetParshah(Frminfo.dpEngdate.Value.AddDays(add_till_sab(Frminfo.dpEngdate.Value.DayOfWeek))).ToString)
        'Debug.Print("Heb Parshah Name | " & GetParshaNew(JC.GetParshah(Frminfo.dpEngdate.Value.AddDays(add_till_sab(Frminfo.dpEngdate.Value.DayOfWeek))).ToString))
        'belwo uses GetParshaNew2 that uses the Parsha in the new jewishCalendar.cs but someting is wrong with it in some years
        'need to move day of week to next sab
        'hebday.Text = hebday.Text & " " & GetParshaNew2(jc.GetParshah(engdate.Value.AddDays(add_till_sab(engdate.Value.DayOfWeek))).ToString)
        'If tbcountry.Text = "israel" Then hebday.Text = hebday.Text & " " & GetParshaNew2(jc.GetParshah(engdate.Value.AddDays(add_till_sab(engdate.Value.DayOfWeek)), True).ToString)
        'Was having isue with RoshChodesh 
        'rbholiday.Text = hdf.formatYomTov(jc) & " " & hdf.formatRoshChodesh(jc) & " " & hdf.formatOmer(jc)
    End Function
    Public Function MakeNewLocationGetForm()
        Dim SelectedPlace As New aLocation
        SelectedPlace.Latitude = Frminfo.tblatitude.Text
        SelectedPlace.Longitude = Frminfo.tblongitude.Text
        SelectedPlace.TimeOffset = Frminfo.tbzone.Text
        SelectedPlace.Country = Frminfo.tbcountry.Text
        SelectedPlace.Elevation = Frminfo.tbElevation.Text

        'Get ID from DisplayName
        Dim AllTimeZones As ObjectModel.ReadOnlyCollection(Of TimeZoneInfo) = TimeZoneInfo.GetSystemTimeZones
        For Each TZ In AllTimeZones
            If TZ.DisplayName = Frminfo.CbTimeZone.Text Then
                SelectedPlace.TimeZoneID = TZ.Id
                Exit For
            End If
        Next

        Dim splitname() As String = Split(Frminfo.cbLocationList.Text, "|", -1)
        'Debug.Print(splitname.Count)
        If splitname.Count > 1 Then ' there is a | in string
            If varSC.PlaceListInHebrew = True Then
                SelectedPlace.HebName = Trim(splitname(0))
                SelectedPlace.EngName = Trim(splitname(1))
            Else
                SelectedPlace.HebName = Trim(splitname(1))
                SelectedPlace.EngName = Trim(splitname(0))
            End If
        End If
        'Debug.Print(SelectedPlace.EngName)
        'Debug.Print(SelectedPlace.HebName)


        'check time zone
        Try
            Dim IANATimezone As String = GeoTimeZone.TimeZoneLookup.GetTimeZone(SelectedPlace.Latitude, SelectedPlace.Longitude).Result
            Dim WinTimezone As String = TimeZoneConverter.TZConvert.IanaToWindows(IANATimezone)
            If SelectedPlace.TimeZoneID <> WinTimezone Then
                Dim Response
                Using New Centered_MessageBox(Frminfo, "MouseCenter")
                    If varSC.HebrewMenus = True Then
                        Response = MsgBox(":חיפוש אזור זמן מציע כי אזור הזמן הנכון הוא" & vbCr & TimeZoneInfo.FindSystemTimeZoneById(WinTimezone).DisplayName & vbCr & "?ברצונך לשמור כך", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.MsgBoxRight)
                    Else
                        Response = MsgBox("GeoTimeZoneLookup Suggests That The Correct Time Zone Is:" & vbCr & TimeZoneInfo.FindSystemTimeZoneById(WinTimezone).DisplayName & vbCr & "Should That Be Saved?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                    End If
                End Using
                If Response = vbYes Then
                    SelectedPlace.TimeZoneID = WinTimezone
                    SelectedPlace.TimeOffset = TimeZoneInfo.FindSystemTimeZoneById(WinTimezone).BaseUtcOffset.TotalHours
                End If
            End If
        Catch ex As Exception
        End Try


        Return SelectedPlace
    End Function
    Public Sub SetAutoComplete(ByVal sender As Object, ByVal AutoComplete As Boolean)
        If AutoComplete = True Then
            sender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Else
            sender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None 'false
        End If
    End Sub
    Public Function DefaultZmanim() As List(Of aZman)
        'Collections start from 0
        Return New List(Of aZman) From
         {
                New aZman With {.DisplayName = "עלות (16.1°)", .FunctionName = "GetAlos16Point1Degrees", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "עלות (72)", .FunctionName = "GetAlos72", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "משיכר (50)", .FunctionName = "ZmanGetMisheyakir50", .ObjectName = "varAddedGets"},
                New aZman With {.DisplayName = "משיכר (11°)", .FunctionName = "GetMisheyakir11Degrees", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "נץ", .FunctionName = "GetSunrise", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "סזק""ש מעלת (16.1°)", .FunctionName = "GetSofZmanShmaMGA16Point1Degrees", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "סזק""ש מג""א ע-צ (72)", .FunctionName = "GetSofZmanShmaMGA72Minutes", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "סזק""ש לבוש נ-ש", .FunctionName = "GetSofZmanShmaGRA", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "סז""ת מג""א ע-צ", .FunctionName = "GetSofZmanTfilaMGA72Minutes", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "סז""ת לבוש נ-ש", .FunctionName = "GetSofZmanTfilaGRA", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "חצות", .FunctionName = "GetChatzos", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "פלג לבוש נ-ש", .FunctionName = "GetPlagHamincha", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "פלג מנחת כהן 8.5°", .FunctionName = "ZmanGetPlagHamincha8Point5Degrees", .ObjectName = "varAddedGets"},
                New aZman With {.DisplayName = "פלג מג""א (72)", .FunctionName = "GetPlagHamincha72Minutes", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "שקיעה", .FunctionName = "GetSunset", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "צאה""כ (7.08°)", .FunctionName = "GetTzaisGeonim7Point083Degrees", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "צאה""כ (8.5°)", .FunctionName = "GetTzaisGeonim8Point5Degrees", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "צאה""כ (72)", .FunctionName = "GetTzais72", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "צאה""כ (16.1°)", .FunctionName = "GetTzais16Point1Degrees", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "בה""ש (13.5 - 8.5°)", .FunctionName = "ZmanGetBeinHashmases8Point5Degrees13Min", .ObjectName = "varAddedGets"},
                New aZman With {.DisplayName = "בה""ש (5.33 מנשף-8.5°)", .FunctionName = "ZmanGetBeinHashmases8Point5Degrees5Point3", .ObjectName = "varAddedGets"},
                New aZman With {.DisplayName = "שעה זמנית נ-ש", .FunctionName = "GetShaahZmanisGra", .ObjectName = "varCZC"},
                New aZman With {.DisplayName = "שעה זמנית ע-צ", .FunctionName = "GetShaahZmanisMGA", .ObjectName = "varCZC"}
         }

        'New aZman With {.DisplayName = "סזק""ש שוות מחצות", .FunctionName = "GetSofZmanShma3HoursBeforeChatzos", .ObjectName = "varCZC"},
        'New aZman With {.DisplayName = "סז""ת שוות מחצות", .FunctionName = "GetSofZmanTfila2HoursBeforeChatzos", .ObjectName = "varCZC"},
        'New aZman With {.DisplayName = "שעה זמנית 16.1°", .FunctionName = "GetShaahZmanis16Point1Degrees", .ObjectName = "varCZC"}
        'New aZman With {.DisplayName = "מנחה ק' לבוש נ-ש", .FunctionName = "GetMinchaKetana", .ObjectName = "varCZC"},
        'New aZman With {.DisplayName = "מנחה ק' מג""א ע-צ", .FunctionName = "GetMinchaKetana72Minutes", .ObjectName = "varCZC"},
    End Function
    Function getnamebyTimeOffset(ByVal num As Double, ByVal windowsName As Boolean)
        'not in use
        'Using  GeoTimeZone.TimeZoneLookup and TimeZoneConverter.TZConvert
        Dim name As String = ""
        If windowsName = True Then
            'Names change in win version's - using .Id looks more stable then DisplayName
            If num = -11 Then name = "Dateline Standard Time"
            If num = -11 Then name = "UTC-11"
            If num = -10 Then name = "Hawaiian Standard Time"
            If num = -9 Then name = "Alaskan Standard Time"
            If num = -8 Then name = "Pacific Standard Time"
            If num = -7 Then name = "Mountain Standard Time"
            If num = -6 Then name = "Central Standard Time"
            If num = -5 Then name = "Eastern Standard Time"
            If num = -4 Then name = "Atlantic Standard Time"
            If num = -3.5 Then name = "Newfoundland Standard Time"
            If num = -3 Then name = "Argentina Standard Time"
            If num = -2 Then name = "UTC-02"
            If num = -1 Then name = "Azores Standard Time"
            If num = 0 Then name = "GMT Standard Time"
            If num = 1 Then name = "Central European Standard Time"
            If num = 2 Then name = "Israel Standard Time"
            If num = 3 Then name = "Russian Standard Time"
            If num = 3.5 Then name = "Iran Standard Time"
            If num = 4 Then name = "Arabian Standard Time"
            If num = 5 Then name = "West Asia Standard Time"
            If num = 6 Then name = "Central Asia Standard Time"
            If num = 7 Then name = "SE Asia Standard Time"
            If num = 8 Then name = "China Standard Time"
            If num = 9 Then name = "Tokyo Standard Time"
            If num = 10 Then name = "AUS Eastern Standard Time"
            If num = 11 Then name = "Central Pacific Standard Time"
            If num = 12 Then name = "New Zealand Standard Time"
            If num = 13 Then name = "Tonga Standard Time"
        Else
            If num = -11 Then name = "Pacific/Midway"
            If num = -10 Then name = "Pacific/Honolulu"
            If num = -9 Then name = "America/Anchorage"
            If num = -8 Then name = "America/Los_Angeles"
            If num = -7 Then name = "America/Denver"
            If num = -6 Then name = "America/Chicago"
            If num = -5 Then name = "America/New_York"
            If num = -4 Then name = "America/La_Paz"
            If num = -3 Then name = "America/Argentina/Buenos_Aires"
            If num = -2 Then name = "America/Noronha"
            If num = -1 Then name = "Atlantic/Azores"
            ' If num = 0 Then name = "Utc"
            If num = 0 Then name = "Europe/London"
            If num = 1 Then name = "Europe/Paris"
            ' If num = 2 Then name = "Europe/Athens"
            If num = 2 Then name = "Israel"
            If num = 3 Then name = "Europe/Moscow"
            If num = 3.5 Then name = "Asia/Tehran"
            If num = 4 Then name = "Asia/Dubai"
            If num = 5 Then name = "Asia/Calcutta"
            If num = 6 Then name = "Asia/Omsk"
            If num = 6.5 Then name = "Indian/Cocos"
            If num = 7 Then name = "Asia/Jakarta"
            If num = 8 Then name = "Asia/Shanghai"
            If num = 9 Then name = "Asia/Tokyo"
            'If num = 10 Then name = "Pacific/Guam"
            If num = 10 Then name = "Australia/LordHowe"
            If num = 11 Then name = "Pacific/Norfolk"
            If num = 12 Then name = "Pacific/Fiji"
            If num = 13 Then name = "Pacific/Enderbury"
            If num = 14 Then name = "Pacific/Kiritimati"
        End If

        Return name
    End Function
    '=======================================================================
    'utilities
    '=======================================================================
    Public Function HebDateStringtoNom(ByVal hebstring) 'based on VBA
        Dim i, temp1 As Integer, day, Month, Year As String

        Dim myHdate() As String = Split(hebstring, " ", -1)

        If myHdate.Count = 3 Then
            day = myHdate(0)
            Month = myHdate(1)
            Year = myHdate(2)
        End If

        If myHdate.Count = 4 Then
            day = myHdate(0)
            Month = myHdate(1) & " " & myHdate(2)
            Year = myHdate(3)
        End If


        temp1 = 0
        For i = 1 To Len(day)
            temp1 = temp1 + HebLtoNom(Mid(day, i, 1))
        Next
        day = temp1

        temp1 = 0
        For i = 1 To Len(Year)
            temp1 = temp1 + HebLtoNom(Mid(Year, i, 1))
            'If Mid(Year, i, 1) = "-" Then temp1 = temp1 * 1000
        Next
        Year = temp1 + 5000

        'Month = HebMtoNom(Month)

        'HebDateStringtoNom = Month & "/" & day & "/" & Year
        HebDateStringtoNom = day & " " & Month & " " & Year

    End Function
    Private Function HebLtoNom(ByVal hebstring As String) 'ðëúá òì éãé ayg
        Dim temp1
        Dim mynum1() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 20, 30, 40, 50, 60, 70, 80, 90,
            100, 200, 300, 400}


        Dim mynum2() As String = {"'", "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט",
        "י", "כ", "ל", "מ", "נ", "ס", "ע", "פ", "צ",
        "ק", "ר", "ש", "ת"}

        For i = 0 To UBound(mynum2)
            If hebstring = mynum2(i) Then temp1 = mynum1(i)
        Next


        HebLtoNom = temp1
    End Function
    Private Function HebMtoNom(ByVal hebstring) 'ðëúá òì éãé ayg
        Dim temp1
        Dim myMonthM1() As String = {"", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר א", "אדר ב", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול"}
        Dim myMonthM2() As Integer = {0, 1, 2, 3, 4, 5, 6.1, 6.2, 7, 8, 9, 10, 11, 12}

        For i = 0 To UBound(myMonthM1)
            If hebstring = myMonthM1(i) Then temp1 = myMonthM2(i)
        Next
        HebMtoNom = temp1
    End Function
    Private Function add_till_sab(ByVal day)
        'כדי להחזיר שם הפרשה של שבת הבא
        'monday is 1
        Dim temp1
        If day = 0 Then temp1 = 6
        If day = 1 Then temp1 = 5
        If day = 2 Then temp1 = 4
        If day = 3 Then temp1 = 3
        If day = 4 Then temp1 = 2
        If day = 5 Then temp1 = 1
        add_till_sab = temp1
    End Function
    Private Function fix_month_num(ByVal mddate)
        'for use with Dll - not in use anymore
        Dim month_num As Integer

        If varHC.GetMonth(mddate) <= 6 Then month_num = varHC.GetMonth(mddate) + 6

        If varHC.GetLeapMonth(varHC.GetYear(mddate)) = 7 Then 'כשהוא שנה מעובר/ שבלא זה מחזיר 0
            If varHC.GetMonth(mddate) = 7 Then month_num = 13 'אדר ב
            If varHC.GetMonth(mddate) >= 8 Then month_num = varHC.GetMonth(mddate) - 7
        Else
            If varHC.GetMonth(mddate) >= 7 Then month_num = varHC.GetMonth(mddate) - 6
        End If
        fix_month_num = month_num
    End Function
    Public Function GetParshaNew(ByVal DateIn As Date, inIsrael As Boolean, Optional EngNames As Boolean = False) As String
        'below is based on oldJewishCalendar.cs 2013 ParshaIndex 

        'to block not sab
        'If DateIn.DayOfWeek <> 6 Then
        '    Return ""
        '    Exit Function
        'End If
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
        Dim EngParshiyos As String() = {"BERESHIS", "NOACH", "LECH_LECHA", "VAYERA", "CHAYEI_SARA", "TOLDOS", "VAYETZEI", "VAYISHLACH", "VAYESHEV", "MIKETZ", "VAYIGASH", "VAYECHI", "SHEMOS", "VAERA", "BO", "BESHALACH", "YISRO", "MISHPATIM", "TERUMAH", "TETZAVEH", "KI_SISA", "VAYAKHEL", "PEKUDEI", "VAYIKRA", "TZAV", "SHMINI", "TAZRIA", "METZORA", "ACHREI_MOS", "KEDOSHIM", "EMOR", "BEHAR", "BECHUKOSAI", "BAMIDBAR", "NASSO", "BEHAALOSCHA", "SHLACH", "KORACH", "CHUKAS", "BALAK", "PINCHAS", "MATOS", "MASEI", "DEVARIM", "VAESCHANAN", "EIKEV", "REEH", "SHOFTIM", "KI_SEITZEI", "KI_SAVO", "NITZAVIM", "VAYEILECH", "HAAZINU", "VAYAKHEL_PEKUDEI", "TAZRIA_METZORA", "ACHREI_MOS_KEDOSHIM", "BEHAR_BECHUKOSAI", "CHUKAS_BALAK", "NITZAVIM_VAYEILECH", "MATOS_MASEI"}

        'ListBox1.Items.Clear()
        'ListBox1.Items.Add("day vb | " & day)
        'ListBox1.Items.Add("week vb | " & week)
        'ListBox1.Items.Add("day cs | " & jc.GetweekforParshaAyg(engdate.Value, False)(2))


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
    'for below use newer JewishCalendar.cs - add new function
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


    Public Function TrimStringEllipsis(TextIn As String, FontIn As System.Drawing.Font, MaxSizeInPixels As Integer) As String
        Dim TrimmedText As String
        Dim graphics = (New System.Windows.Forms.Label()).CreateGraphics()
        Dim CurrentSize As Integer = graphics.MeasureString(TextIn, FontIn).Width
        'no need to trim
        If CurrentSize <= maxSizeInPixels Then Return TextIn

        For Each c As Char In TextIn
            TrimmedText += c
            CurrentSize = graphics.MeasureString(TrimmedText & "...", FontIn).Width
            If CurrentSize >= MaxSizeInPixels Then Exit For
        Next
        Return TrimmedText & "..."
    End Function

    Private Function fill_strings(name As String, zman As String, Optional num As Integer = 36) As String
        Dim my_num As Integer
        my_num = Len(name) + Len(zman)
        my_num = my_num / 4

        'MsgBox(num)
        MsgBox(name.PadRight(num, ".") & zman)
        fill_strings = name.PadRight(num, ".") & zman
    End Function

End Module


'sample




