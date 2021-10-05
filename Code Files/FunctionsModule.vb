'left-to-right mark: ChrW(&H200E) 
'right-to-left mark: ChrW(&H200F)

'needs to be Module it should be global
Module FunctionsModule
    Public Sub LoadSettingsandVariables()
        'load SettingsCollection from xml file
        If My.Computer.FileSystem.FileExists(varUserFile) Then
            varSC = SerializableData.Load(varUserFile, GetType(SettingsCollection))
        End If
        'Frminfo Location
        If varSC.LocationX > -1 And varSC.LocationY > 1 Then
            Frminfo.Location = New System.Drawing.Point(varSC.LocationX, varSC.LocationY)
            'the is a check on Screen.PrimaryScreen.Bounds in Frminfo_Load
        End If
        'Frminfo Location and size
        Frminfo.mHideLocationInfo.Checked = varSC.HideLocationBox
        If varSC.HideLocationBox = True Then Frminfo.DoHideLocationBox(varSC.FirstRun)
        If varSC.SizeH > -1 And varSC.SizeW > -1 Then
            Frminfo.Size = New System.Drawing.Size(varSC.SizeW, varSC.SizeH)
            Frminfo.DataGridView1.Size = New System.Drawing.Size(Frminfo.DataGridView1.Size.Width, varSC.DataGridSizeH)
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
        Frminfo.mDisplayDafYomi.Checked = varSC.DisplayDafYomi


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

        varZmanimFuncList = GetListOfZmanimFunctions()
        Load_Zmanim_Func()

        varSC.FirstRun = False
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
            'Debug.Print(counter)
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
        varZmanimFunc.Add("ZmanGetCandleLighting40")
        'varZmanimFunc.ForEach(Sub(x) Debug.Print(x))
    End Sub
    Public Function Check_valid_zman(ByVal s As String, ByVal checkIsDate As Boolean)
        If varZmanTimeZone Is Nothing Then
            Return False
            Exit Function
        End If
        Dim mytime As Date
        Try
            s = CallByName(varCZC, s, CallType.Get)
            If checkIsDate = True Then If DateTime.TryParse(s, mytime) = False Then Return False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Sub change_place(ByVal num As Integer)
        If varSC.Location.Count < 1 Then Exit Sub
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

        Dim ZmanListNum As Integer
        Dim i As Integer = 0
        For Each Z In varSC.Zmanim
            i += 1
            DVItems.Add(New DataGridViewRow())
            Try
                ZmanListNum = varZmanimFuncList.Where(Function(x) x.FunctionName = Z.FunctionName).FirstOrDefault.Num
            Catch ex As Exception
            End Try
            DVItems(DVItems.Count - 1).CreateCells(Frminfo.DataGridView1, i, ZmanListNum, Z.DisplayName)
            If InStr(Z.FunctionName, "ShaahZmanis") > 0 Then
                Try
                    MyZman = #12:00:00 AM#.AddMilliseconds(CallByName(varCZC, Z.FunctionName, CallType.Get))
                    CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).Items.Add(MyZman.ToString("H:mm:ss"))
                Catch ex As Exception
                    CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).Items.Add("")
                End Try
            Else
                Try
                    If Z.ObjectName = "varCZC" Then
                        MyZman = CDate(CallByName(varCZC, Z.FunctionName, CallType.Get)).ToString(timeFormat)
                    Else
                        MyZman = CDate(CallByName(varAddedGets, Z.FunctionName, CallType.Get, Args))
                    End If
                    CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).Items.Add(ChrW(&H200E) & StrConv(MyZman.ToString(timeFormat), VbStrConv.Lowercase))
                Catch ex As Exception
                    CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).Items.Add("")
                End Try
                'Color setings - are now done in TimerStatusLabel
            End If
            'add func name to ColumnFunc

            'set to not be bold
            DVItems(DVItems.Count - 1).DefaultCellStyle.Font = New Font(Frminfo.DataGridView1.Font, FontStyle.Regular)
            'selcet value for ComboBoxCell
            CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).Value = CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).Items(0)
            'set ToolTip's
            If varSC.ShowTooltips = True Then
                If varSC.HebrewMenus = True Then
                    CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewTextBoxCell).ToolTipText = "לחץ פעמיים לעריכה" & vbCr & "גרור ושחרר לסידור מחדש" & vbCr & "קליק ימני לאפשרויות נוספות"
                Else
                    CType(DVItems(DVItems.Count - 1).Cells(2), DataGridViewTextBoxCell).ToolTipText = "Double Click to Edit" & vbCr & "Drag and Drop to Rearrange" & vbCr & "Right Click for More Options"
                End If
            End If
            'CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).ToolTipText = Z.FunctionName
            If varSC.HebrewMenus = False Then
                CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).ToolTipText = varZmanimFuncList.Where(Function(x) x.FunctionName = Z.FunctionName).FirstOrDefault.EngName
            Else
                CType(DVItems(DVItems.Count - 1).Cells(3), DataGridViewComboBoxCell).ToolTipText = varZmanimFuncList.Where(Function(x) x.FunctionName = Z.FunctionName).FirstOrDefault.HebName
            End If

        Next

        Frminfo.DataGridView1.Rows.Clear()
        Frminfo.DataGridView1.Rows.AddRange(DVItems.ToArray)
        Try
            Frminfo.DataGridView1.CurrentCell = Frminfo.DataGridView1(3, mySelectedRow)
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

        Dim timeFormat As String = "h:mm:ss tt"
        If varSC.Clock24Hour = True Then timeFormat = "H:mm:ss"

        'If FrmDifference.CbTimeA.Text <> "" Then
        Try
            If FrmDifference.CbTimeA.SelectedIndex > -1 Then
                If varZmanimFuncList.Item(FrmDifference.CbTimeA.SelectedIndex).ObjectName = "varCZC" Then
                    If InStr(varZmanimFuncList.Item(FrmDifference.CbTimeA.SelectedIndex).FunctionName, "ShaahZmanis") > 0 Then
                        FrmDifference.tbTimeA.Text = #12:00:00 AM#.AddMilliseconds(CallByName(varCZC, varZmanimFuncList.Item(FrmDifference.CbTimeA.SelectedIndex).FunctionName, CallType.Get)).ToString("H:mm:ss")
                    Else
                        FrmDifference.tbTimeA.Text = CDate(CallByName(varCZC, varZmanimFuncList.Item(FrmDifference.CbTimeA.SelectedIndex).FunctionName, CallType.Get)).ToString(timeFormat)
                    End If
                Else
                    FrmDifference.tbTimeA.Text = CDate(CallByName(varAddedGets, varZmanimFuncList.Item(FrmDifference.CbTimeA.SelectedIndex).FunctionName, CallType.Get, Args)).ToString(timeFormat)
                End If

            End If

            If FrmDifference.CbTimeB.SelectedIndex > -1 Then
                If varZmanimFuncList.Item(FrmDifference.CbTimeB.SelectedIndex).ObjectName = "varCZC" Then
                    If InStr(varZmanimFuncList.Item(FrmDifference.CbTimeB.SelectedIndex).FunctionName, "ShaahZmanis") > 0 Then
                        FrmDifference.tbTimeB.Text = #12:00:00 AM#.AddMilliseconds(CallByName(varCZC, varZmanimFuncList.Item(FrmDifference.CbTimeB.SelectedIndex).FunctionName, CallType.Get)).ToString("H:mm:ss")
                    Else
                        FrmDifference.tbTimeB.Text = CDate(CallByName(varCZC, varZmanimFuncList.Item(FrmDifference.CbTimeB.SelectedIndex).FunctionName, CallType.Get)).ToString(timeFormat)
                    End If
                Else
                    FrmDifference.tbTimeB.Text = CDate(CallByName(varAddedGets, varZmanimFuncList.Item(FrmDifference.CbTimeB.SelectedIndex).FunctionName, CallType.Get, Args)).ToString(timeFormat)
                End If

            End If
        Catch ex As Exception
        End Try


        Dim myTimeSpan As TimeSpan
        'If FrmDifference.tbTimeA.Text <> "" And FrmDifference.tbTimeB.Text <> "" Then
        If FrmDifference.CbTimeA.SelectedIndex > -1 And FrmDifference.CbTimeB.SelectedIndex > -1 Then
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

        'If InStr(FrmDifference.CbTimeA.Text, "ZmanGet") Then
        '    FrmDifference.tbTimeA.Text = CDate(CallByName(varAddedGets, FrmDifference.CbTimeA.Text, CallType.Get, Args))
        'Else
        '    FrmDifference.tbTimeA.Text = CDate(CallByName(varCZC, FrmDifference.CbTimeA.Text, CallType.Get))
        'End If
    End Sub
    Sub parse_hebdate()
        If varFinishedLoading = False Then Exit Sub
        varHculture.DateTimeFormat.Calendar = varHC
        'Thread.CurrentThread.CurrentCulture = varHculture

        'Dim myhebdate() As String = Split(Frminfo.rtbHebrewDate.Text, "/", -1)
        'myhebdate(0) = Trim(myhebdate(0))
        Dim myhebdate As String = Frminfo.rtbHebrewDate.Text.Replace(vbCr, "").Replace(vbLf, "")

        Try
            'use HebDateStringtoNom to convert day and year to number this will help when the are no גרשיים
            DateTime.Parse(HebDateStringtoNom(myhebdate), varHculture)
        Catch ex As Exception
            'add this year
            Try
                myhebdate = myhebdate & " " & Now.ToString("yyy", varHculture)
                DateTime.Parse(HebDateStringtoNom(myhebdate), varHculture)
            Catch
                DateNonconvertible()
                change_hebdate()
                Exit Sub
            End Try
        End Try

        Try
            Dim mydate As DateTime = DateTime.Parse(HebDateStringtoNom(myhebdate), varHculture)
            Frminfo.dpEngdate.Value = mydate '.ToString("ddddMM/dd/yyyy", New CultureInfo("en-US"))
        Catch ex As Exception
            DateNonconvertible()
            change_hebdate()
            Exit Sub
        End Try
    End Sub
    Sub DateNonconvertible()
        Using New Centered_MessageBox(Frminfo, "MouseCenter")
            If varSC.HebrewMenus = True Then
                MsgBox("!תאריך לא ניתן להמרה", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxRight) ' + MsgBoxStyle.MsgBoxRtlReading
            Else
                MsgBox("Date Not Convertible!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
        End Using
    End Sub
    Sub change_hebdate()
        If varFinishedLoading = False Then Exit Sub

        Dim ResultArray()
        ResultArray = Get_HebDate(Frminfo.dpEngdate.Value)
        Dim HebDatetoShow As String

        Frminfo.rtbHebrewDate.Text = ResultArray(0)
        If ResultArray(3) <> Nothing Then 'the is a holiday
            If ResultArray(2) = Nothing Then 'the is No Parsha
                ResultArray = Get_HebDate(Frminfo.dpEngdate.Value, True)
                Frminfo.rtbParsha.Text = "יום " & ResultArray(1) & " " & ChrW(&H2022) & " " & ResultArray(3) & If(ResultArray(4) = Nothing, "", " " & ChrW(&H2022) & " " & ResultArray(4)) 'ChrW(&H2022)
            Else
                Debug.Print(ResultArray(3) & "=")
                Frminfo.rtbParsha.Text = ResultArray(1) & If(ResultArray(2) <> "", " " & ResultArray(2), "") & " " & ChrW(&H2022) & " " & ResultArray(3) & If(ResultArray(4) = Nothing, "", " " & ChrW(&H2022) & " " & ResultArray(4))
            End If
        Else 'no holiday
            ResultArray = Get_HebDate(Frminfo.dpEngdate.Value, True)
            Frminfo.rtbParsha.Text = "יום " & ResultArray(1) & If(ResultArray(2) <> "", " " & If(ResultArray(4) = Nothing, "פרשת ", "") & ResultArray(2), "") & If(ResultArray(4) = Nothing, "", " " & ChrW(&H2022) & " " & ResultArray(4))
        End If
        RichTextBoxAlignment()
    End Sub
    Sub RichTextBoxAlignment()
        Frminfo.rtbParsha.SelectAll()
        Frminfo.rtbParsha.SelectionAlignment = HorizontalAlignment.Center
        Frminfo.rtbHebrewDate.SelectAll()
        Frminfo.rtbHebrewDate.SelectionAlignment = HorizontalAlignment.Center
    End Sub
    Public Function Get_HebDate(DateIn As Date, Optional LongHebDayFormat As Boolean = False) As String()
        Dim Hebdate, Holiday, Hebday, Parsha, Daf As String
        Dim JC As New JewishCalendar()
        Dim HDF As New HebrewDateFormatter()
        Dim YC As New YomiCalculator()
        HDF.HebrewFormat = True
        HDF.LongWeekFormat = LongHebDayFormat
        varHculture.DateTimeFormat.Calendar = varHC

        Hebdate = DateIn.ToString("dd MMM yyy", varHculture)
        Holiday = HDF.FormatYomTov(DateIn, False) & If(HDF.FormatOmer(DateIn) = Nothing, "", " " & HDF.FormatOmer(DateIn))
        If varSC.IsraeliYomTov = True Then Holiday = HDF.FormatYomTov(DateIn, True) & " " & HDF.FormatOmer(DateIn)
        Hebday = HDF.FormatDayOfWeek(DateIn)

        Try
            Parsha = varAddedGets.ZmanGetParsha(DateIn, varSC.IsraeliYomTov)
        Catch
        End Try

        'only send back daf if not closed by user, is after 1923 and was able to get - else will be Nothing
        If varSC.DisplayDafYomi = True Then
            If DateIn > #9/11/1923# Then
                Try
                    'used to have a isue with some days, looks fine with newer YomiCalculator.cs
                    Daf = HDF.FormatDafYomiBavli(YomiCalculator.GetDafYomiBavli(DateIn))
                Catch
                End Try
            End If
        End If
A:
        Return New String() {Hebdate, Hebday, Parsha, Holiday, Daf}
        '0 Hebdate
        '1 Hebday of week 'long or short
        '2 Parsha
        '3 Holiday
        '4 DafYomi
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
    Public Function ZmanFromName(ZmanName As String) As aZmanimFunc
        Dim ZmanInfo
        If varSC.HebrewMenus = False Then
            ZmanInfo = varZmanimFuncList.Where(Function(x) x.EngName = ZmanName).FirstOrDefault
        Else
            ZmanInfo = varZmanimFuncList.Where(Function(x) x.HebName = ZmanName).FirstOrDefault
            'check for ChrW(&H200F) & " "
            If ZmanInfo Is Nothing Then ZmanInfo = varZmanimFuncList.Where(Function(x) x.HebName & ChrW(&H200F) & " " = ZmanName).FirstOrDefault
        End If
        Return ZmanInfo
        'If ZmanInfo Is Nothing Then Return 
    End Function
    Public Sub Dropdowns_DrawItem(sender As Object, e As DrawItemEventArgs, Optional AddToRight As Double = 0)
        'for zmanim dropdown lists to make right to left and BalloonTip
        'in part based on this https://stackoverflow.com/questions/680373/tooltip-for-each-items-in-a-combo-box
        Dim comboBox1 As ComboBox = CType(sender, ComboBox)
        'Dim rgx As Regex = New Regex("(.{50}\s)")
        Dim x, y As Double

        If comboBox1 IsNot Nothing Then e.DrawBackground()
        If e.Index >= 0 Then
            Dim MyRightAlignment As StringFormat = New StringFormat()
            MyRightAlignment.LineAlignment = StringAlignment.Far
            MyRightAlignment.Alignment = StringAlignment.Far
            Dim text As String = comboBox1.GetItemText(comboBox1.Items(e.Index))
            Using br As SolidBrush = New SolidBrush(e.ForeColor)
                If varSC.HebrewMenus = False Then
                    e.Graphics.DrawString(text, e.Font, br, e.Bounds)
                Else
                    e.Graphics.DrawString(text, e.Font, br, e.Bounds, MyRightAlignment)
                End If
            End Using
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected AndAlso comboBox1.DroppedDown Then ' AndAlso e.Index > 0
                Try
                    'close old
                    If varDescriptionBalloonTip IsNot Nothing Then varDescriptionBalloonTip.Close()
                    'x = sender.RectangleToScreen(sender.ClientRectangle).Left + sender.Width + 15
                    x = sender.RectangleToScreen(sender.ClientRectangle).Right + AddToRight
                    'x = Control.MousePosition.X
                    'y = sender.RectangleToScreen(sender.ClientRectangle).Top
                    y = Control.MousePosition.Y
                    If varSC.HebrewMenus = False Then
                        'ToolTipDescription.ToolTipTitle = varZmanimFuncList.Item(e.Index).EngName
                        'ToolTipDescription.Show(rgx.Replace(varZmanimFuncList.Item(e.Index).EngDescription, "$1" & vbLf), comboBox1, e.Bounds.Right + 20, e.Bounds.Top)
                        varDescriptionBalloonTip = New CustomBalloonTip(varZmanimFuncList.Item(e.Index).EngName, varZmanimFuncList.Item(e.Index).EngDescription & ChrW(&H200E) & " ", sender, x, y, False, 0, 5000)
                    Else
                        varDescriptionBalloonTip = New CustomBalloonTip(varZmanimFuncList.Item(e.Index).HebName, varZmanimFuncList.Item(e.Index).HebDescription & ChrW(&H200F) & " ", sender, x, y, True, 0, 5000)
                    End If
                Catch tp As Exception
                    Debug.WriteLine("Error in comboBox tooltip: " & e.Index)
                End Try
            Else
                'ToolTipDescription.Hide(comboBox1)
                If varDescriptionBalloonTip IsNot Nothing Then varDescriptionBalloonTip.Close()
            End If
        Else
            'ToolTipDescription.Hide(comboBox1)
            If varDescriptionBalloonTip IsNot Nothing Then varDescriptionBalloonTip.Close()
        End If
        e.DrawFocusRectangle()
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
                New aZman With {.DisplayName = "שעה זמנית ע-צ", .FunctionName = "GetShaahZmanis72Minutes", .ObjectName = "varCZC"}
         }

        'New aZman With {.DisplayName = "סזק""ש שוות מחצות", .FunctionName = "GetSofZmanShma3HoursBeforeChatzos", .ObjectName = "varCZC"},
        'New aZman With {.DisplayName = "סז""ת שוות מחצות", .FunctionName = "GetSofZmanTfila2HoursBeforeChatzos", .ObjectName = "varCZC"},
        'New aZman With {.DisplayName = "שעה זמנית 16.1°", .FunctionName = "GetShaahZmanis16Point1Degrees", .ObjectName = "varCZC"}
        'New aZman With {.DisplayName = "מנחה ק' לבוש נ-ש", .FunctionName = "GetMinchaKetana", .ObjectName = "varCZC"},
        'New aZman With {.DisplayName = "מנחה ק' מג""א ע-צ", .FunctionName = "GetMinchaKetana72Minutes", .ObjectName = "varCZC"},
    End Function
    Public Function GetListOfZmanimFunctions() As List(Of aZmanimFunc)
        'Collections start from 0
        Return New List(Of aZmanimFunc) From
         {
            New aZmanimFunc With {.Num = "1", .EngName = "Alos 26 Degrees", .FunctionName = "GetAlos26Degrees", .HebName = "עלות השחר 26 מעלות", .HebDescription = "עלות השחר מחושב כאשר השמש נמצאת 26 מעלות השחר מתחת לאופק הגיאומטרי המזרחי לפני הזריחה.", .EngDescription = "Alos (dawn) calculated when the sun is 26° below the eastern geometric horizon before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "2", .EngName = "Alos 19.8 Degrees", .FunctionName = "GetAlos19Point8Degrees", .HebName = "עלות השחר 19.8 מעלות", .HebDescription = "עלות השחר מחושב כאשר השמש נמצאת 19.8 מעלות מתחת לאופק הגיאומטרי המזרחי לפני הזריחה.", .EngDescription = "Alos (dawn) calculated when the sun is 19.8° below the eastern geometric horizon before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "3", .EngName = "Alos 19 Degrees", .FunctionName = "GetAlos19Degrees", .HebName = "עלות השחר 19 מעלות", .HebDescription = "עלות השחר מחושב כאשר השמש נמצאת 19 מעלות מתחת לאופק המזרחי לפני הזריחה. זהו דעת כמה אחרונים בשיטת הרמב''ם (ראה מעגלי צדק עמ' 88, יום ולילה של תורה עמ' 222)", .EngDescription = "Alos (dawn) calculated when the sun is 19° below the eastern geometric horizon before sunrise. This is the Rambam's alos according to Rabbi Moshe Kosower's Maaglei Tzedek, page 88, Ayeles Hashachar Vol. I, page 12, Yom Valayla Shel Torah, Ch. 34, p. 222 and Rabbi Yaakov Shakow's Luach Ikvei Hayom", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "4", .EngName = "Alos 18 Degrees", .FunctionName = "GetAlos18Degrees", .HebName = "עלות השחר 18 מעלות", .HebDescription = "עלות השחר מחושב כאשר השמש נמצאת 18 מעלות מתחת לאופק הגיאומטרי המזרחי לפני הזריחה.", .EngDescription = "Alos (dawn) calculated when the sun is 18° below the eastern geometric horizon before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "5", .EngName = "Alos 16.1 Degrees", .FunctionName = "GetAlos16Point1Degrees", .HebName = "עלות השחר 16.1 מעלות", .HebDescription = "עלות השחר מחושב כאשר השמש נמצאת 16.1 מעלות מתחת לאופק הגיאומטרי המזרחי לפני הזריחה.", .EngDescription = "Alos (dawn) calculated when the sun is 16.1° below the eastern geometric horizon before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "6", .EngName = "Alos 120 Minutes", .FunctionName = "GetAlos120", .HebName = "עלות השחר 120 דקות", .HebDescription = "עלות השחר מחושב באמצעות 120 דקות לפני הזריחה בגובה פני הים (לא בוצעה התאמה לגובה) בהתבסס על זמן ההליכה למרחק של 5 מיל ב -24 דקות למיל.", .EngDescription = "Alos (dawn) calculated using 120 minutes before sea level sunrise (no adjustment for elevation is made) based on the time to walk the distance of 5 mil at 24 minutes a mil.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "7", .EngName = "Alos 120 Minutes Zmanis", .FunctionName = "GetAlos120Zmanis", .HebName = "עלות השחר 120 דקות זמנית", .HebDescription = "עלות השחר מחושב באמצעות 120 דקות זמנית או 1/6 של היום לפני הזריחה או הזריחה בגובה פני הים.", .EngDescription = "Alos (dawn) calculated using 120 minutes zmaniyos or 1/6th of the day before sunrise or sea level sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "8", .EngName = "Alos 96 Minutes", .FunctionName = "GetAlos96", .HebName = "עלות השחר 96 דקות", .HebDescription = "עלות השחר מחושב באמצעות 96 דקות לפני הזריחה או הזריחה בגובה פני הים המבוססת על זמן הליכה של 4 מיל ב -24 דקות למיל.", .EngDescription = "Alos (dawn) calculated using 96 minutes before before sunrise or sea level sunrise  that is based on the time to walk the distance of 4 mil at 24 minutes a mil.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "9", .EngName = "Alos 96 Minutes Zmanis", .FunctionName = "GetAlos96Zmanis", .HebName = "עלות השחר 96 דקות זמנית", .HebDescription = "עלות השחר מחושב באמצעות 96 דקות זמנית או 1/7 של היום לפני הזריחה או הזריחה בגובה פני הים.", .EngDescription = "Alos (dawn) calculated using 96 minutes zmaniyos or 1/7.5th of the day before sunrise or sea level sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "10", .EngName = "Alos 90 Minutes", .FunctionName = "GetAlos90", .HebName = "עלות השחר 90 דקות", .HebDescription = "עלות השחר מחושב באמצעות 90 דקות לפני הזריחה בגובה פני הים בהתבסס על זמן הליכה של 4 מיל ב -22.5 דקות למיל.", .EngDescription = "Alos (dawn) calculated using 90 minutes before sea level sunrise based on the time to walk the distance of 4 mil at 22.5 minutes a mil.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "11", .EngName = "Alos 90 Minutes Zmanis", .FunctionName = "GetAlos90Zmanis", .HebName = "עלות השחר 90 דקות זמנית", .HebDescription = "עלות השחר מחושב באמצעות 90 דקות זמנית או 1/8 מהיממה לפני הזריחה או הזריחה בגובה פני הים.", .EngDescription = "Alos (dawn) calculated using 90 minutes zmaniyos or 1/8th of the day before sunrise or sea level sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "12", .EngName = "Alos 72 Minutes", .FunctionName = "GetAlos72", .HebName = "עלות השחר 72 דקות", .HebDescription = "עלות השחר מחושב באמצעות 72 דקות לפני הזריחה בגובה פני הים (ללא התאמה לגובה) בהתבסס על זמן ההליכה למרחק של 4 מיל ב -18 דקות למיל. זה מבוסס על חוות דעתם של רוב הראשונים שאמרו שזמן הנשף (זמן בין עלות השחר לזריחה) אינו משתנה בזמן השנה או במיקום, אך תלוי אך ורק בזמן שלוקח למרחק של 4 מיל.", .EngDescription = "Alos (dawn) calculated using 72 minutes before sea level sunrise (no adjustment for elevation) based on the time to walk the distance of 4 mil at 18 minutes a mil. this is based on the opinion of most rishonim who stated that the time of the neshef (time between dawn and sunrise) does not vary by the time of year or location but purely depends on the time it takes to walk the distance of 4 mil.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "13", .EngName = "Alos 72 Minutes Zmanis", .FunctionName = "GetAlos72Zmanis", .HebName = "עלות השחר 72 דקות זמנית", .HebDescription = "עלות השחר מחושב באמצעות 72 דקות זמנית או 1/10 של היום לפני הזריחה.", .EngDescription = "Alos (dawn) calculated using 72 minutes zmaniyos or 1/10th of the day before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "14", .EngName = "Alos 60 Minutes", .FunctionName = "GetAlos60", .HebName = "עלות השחר 60 דקות", .HebDescription = "עלות השחר מחושב כ -60 דקות לפני הזריחה.", .EngDescription = "Alos (dawn) calculated as 60 minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "15", .EngName = "Alos Baal Hatanya", .FunctionName = "GetAlosBaalHatanya", .HebName = "עלות השחר לדעת בעל התניא", .HebDescription = "עלות השחר לשיטת הבעל התניא, מחושב כאשר השמש נמצאת 16.9 מעלות מתחת לאופק המזרחי לפני הזריחה. היא מבוססת על מיקום השמש 72 דקות לפני הזריחה בירושלים ביום השוויון (16 במרץ, היום שבו שעה זמנית היא 60 דקות) הוא 16.9 מעלות מתחת לאופק", .EngDescription = " Baal Hatanya's alos (dawn) calculated as the time when the sun is 16.9° below the eastern geometric horizon before sunrise.  It is based on the sun's position at 72 minutes before sunrise in Jerusalem on the equinox (March 16, the day that a solar hour is 60 minutes) is 16.9° below geometric zenith.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "16", .EngName = "Misheyakir 11.5 Degrees", .FunctionName = "GetMisheyakir11Point5Degrees", .HebName = "משיכיר 11.5 מעלות", .HebDescription = "משיכיר מבוסס על מיקום השמש כשהיא 11.5 מעלות מתחת לאופק הגיאומטרי (90 דקות°).", .EngDescription = "Misheyakir based on the position of the sun when it is 11.5° below geometric zenith (90 minutes°).", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "17", .EngName = "Misheyakir 11 Degrees", .FunctionName = "GetMisheyakir11Degrees", .HebName = "משיכיר 11 מעלות", .HebDescription = "משיכיר מבוסס על מיקום השמש כשהיא 11° מתחת לאופק הגיאומטרי (90 דקות°).", .EngDescription = "Misheyakir based on the position of the sun when it is 11° below geometric zenith (90 minutes°).", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "18", .EngName = "Misheyakir 10.2 Degrees", .FunctionName = "GetMisheyakir10Point2Degrees", .HebName = "משיכיר 10.2 מעלות", .HebDescription = "משיכיר מבוסס על מיקום השמש כשהיא 10.2 מעלות מתחת לאופק הגיאומטרי (90 דקות°).", .EngDescription = "Misheyakir based on the position of the sun when it is 10.2° below geometric zenith (90 minutes°).", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "19", .EngName = "Misheyakir 9.5 Degrees", .FunctionName = "GetMisheyakir9Point5Degrees", .HebName = "מכשיכיר 9.5 מעלות", .HebDescription = "משיכיר מבוסס על מיקום השמש כשהיא 9.5 מעלות מתחת לאופק הגיאומטרי (90 דקות°).", .EngDescription = "Misheyakir based on the position of the sun when it is 9.5° below geometric zenith (90 minutes°).", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "20", .EngName = "Misheyakir 7.65 Degrees", .FunctionName = "GetMisheyakir7Point65Degrees", .HebName = "משיכיר 7.65 מעלות", .HebDescription = "משיכיר מבוסס על מיקום השמש כשהיא 7.65 מעלות מתחת לאופק הגיאומטרי (90 דקות°).", .EngDescription = "Misheyakir based on the position of the sun when it is 7.65° below geometric zenith (90 minutes°).", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "21", .EngName = "Misheyakir 50 Minutes", .FunctionName = "ZmanGetMisheyakir50", .HebName = "משיכיר 50 דקות", .HebDescription = "משיכיר מבוסס על 50 דקות לפני הזריחה (שיטה זו נוספה על ידי myzman ואינה חלק מ-kosher java)", .EngDescription = "Misheyakir based on 50 minutes befor sunrise (this method was added by myzman and is not part of kosher java)", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "22", .EngName = "Sunrise", .FunctionName = "GetSunrise", .HebName = "נץ החמה", .HebDescription = "הזריחה מותאם לגובה.", .EngDescription = "Elevation adjusted sunrise time.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "23", .EngName = "Sea Level Sunrise", .FunctionName = "GetSeaLevelSunrise", .HebName = "נץ החמה במפלס פני הים", .HebDescription = "זריחה ללא התאמת גובה.", .EngDescription = "Sunrise without elevation adjustment.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "24", .EngName = "Sunrise Baal Hatanya", .FunctionName = "GetSunriseBaalHatanya", .HebName = "נץ החמה לדעת בעל התניא", .HebDescription = "נץ הלכתי לדעת בעל התניא (ללא התאמת גובה), שלדעתו הנץ נחשב כאשר החלק העליון של השמש נראה בגובה הדומה להרי ארץ ישראל. הזמן מחושב כנקודה שבה מרכז השמש נמצא 1.583° מתחת לאופק. חישוב זה ניתן למצוא אצל הרב דוב בער לווין בביאור סדר הכנסת שבת של הבעל התניא, שים לב: נץ הלכתי מיעוד לחישוב כמה זמנים אבל אין להשתמש בו כדי לעשות מצות שזמנו ביום כגון לולב", .EngDescription = "Baal Hatanya's halachic netz (sunrise) without elevation adjustment.  According to the Baal Hatanya halachic sunrise, is when the top of the sun's disk is visible at an elevation similar to the mountains of Eretz Yisrael. The time is calculated as the point at which the center of the sun's disk is 1.583° below the horizon. This degree based calculation can be found in Rabbi Shalom DovBer Levine's commentary on The Baal Hatanya's Seder Hachnasas Shabbos  Note: netz amiti is used only for calculating certain zmanim. For practical purposes, daytime mitzvos like shofar and lulav should not be done until after the published time for netz-sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "25", .EngName = "Sof Zman Shma Alos-Tzais 19.8 Degrees", .FunctionName = "GetSofZmanShmaMGA19Point8Degrees", .HebName = "סו''ז שמע עלות-צאת 19.8 מעלות", .HebDescription = "זמן האחרון לקריאת שמע (בבוקר) להמגן אברהם (מג''א) המבוסס על עלות 19.8 מעלות לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being 19.8° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "26", .EngName = "Sof Zman Shma Alos-Tzais 18 Degrees", .FunctionName = "GetSofZmanShmaMGA18Degrees", .HebName = "סו''ז שמע עלות-צאת 18 מעלות", .HebDescription = "זמן האחרון לקריאת שמע (בבוקר) להמגן אברהם (מג''א) המבוסס על עלות 18 מעלות לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being 18° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "27", .EngName = "Sof Zman Shma Alos-Tzais 16.1 Degrees", .FunctionName = "GetSofZmanShmaMGA16Point1Degrees", .HebName = "סו''ז שמע עלות-צאת 16.1 מעלות", .HebDescription = "זמן האחרון לקריאת שמע (בבוקר) להמגן אברהם (מג''א) המבוסס על עלות 16.1 מעלות לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being 16.1° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "28", .EngName = "Sof Zman Shma Alos 16.1 To Sunset", .FunctionName = "GetSofZmanShmaAlos16Point1ToSunset", .HebName = "סו''ז שמע עלות 16.1 לשקיעה", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) בהתבסס על הדעה שהיום מתחיל ב -16.1 מעלות וחצי ומסתיים בשקיעת גובה פני הים.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) based on the opinion that the day starts at alos 16.1° and ends at sea level sunset. 3 shaos zmaniyos are calculated based on this day and added to alos to reach this time.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "29", .EngName = "Sof Zman Shma Alos 16.1 To Tzais Geonim 7.083 Degrees", .FunctionName = "GetSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees", .HebName = "סו''ז שמע עלות 16.1 לצאת 7.083 מעלות", .HebDescription = "זמן קריאת שמע העדכני ביותר (זמן קריאת שמע בבוקר) על סמך הדעה שהיום מתחיל ב -16.1 מעלות וחצי ומסתיים ב -7.083 מעלות.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) based on the opinion that the day starts at alos 16.1° and ends at tzais 7.083°. 3 shaos zmaniyos are calculated based on this day and added to alos to reach this time.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "30", .EngName = "Sof Zman Shma Alos-Tzais 120 Minutes", .FunctionName = "GetSofZmanShmaMGA120Minutes", .HebName = "סו''ז שמע עלות-צאת 120 דקות", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) להמגן אברהם (מג''א) המבוססת על היותו 120 דקות או 1/6 מהיממה לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  120 minutes minutes or 1/6th of the day before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "31", .EngName = "Sof Zman Shma Alos-Tzais 96 Minutes", .FunctionName = "GetSofZmanShmaMGA96Minutes", .HebName = "סו''ז שמע עלות-צאת 96 דקות", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) להמגן אברהם (מג''א) המבוססת על היותו 96 דקות לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  96 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "32", .EngName = "Sof Zman Shma Alos-Tzais 96 Minutes Zmanis", .FunctionName = "GetSofZmanShmaMGA96MinutesZmanis", .HebName = "סו''ז שמע עלות-צאת 96 דקות זמנית", .HebDescription = "זמן קריאת שמע  האחרונה (בבוקר) להמגן אברהם (מג''א) המבוסס על היות ואל 96 דקות זמניה לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  96 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "33", .EngName = "Sof Zman Shma Alos-Tzais 90 Minutes", .FunctionName = "GetSofZmanShmaMGA90Minutes", .HebName = "סו''ז שמע עלות-צאת 90 דקות", .HebDescription = "זמן קריאת שמע האחרון (בבוקר) להמגן אברהם (מג''א) המבוסס על כך שעלות הוא 90 דקות לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  90 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "34", .EngName = "Sof Zman Shma Alos-Tzais 90 Minutes Zmanis", .FunctionName = "GetSofZmanShmaMGA90MinutesZmanis", .HebName = "סו''ז שמע עלות-צאת 90 דקות זמנית", .HebDescription = "זמן האחרון לקריאת שמע (בבוקר) להמגן אברהם (מג''א) המבוסס על היות ואל 90 דקות זמניה לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  90 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "35", .EngName = "Sof Zman Shma Alos-Tzais 72 Minutes", .FunctionName = "GetSofZmanShmaMGA72Minutes", .HebName = "סו''ז שמע עלות-צאת 72 דקות", .HebDescription = "זמן קריאת שמע  האחרון (בבוקר) להמגן אברהם (מג''א) המבוסס על כך שעלות הוא 72 דקות לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  72 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "36", .EngName = "Sof Zman Shma Alos-Tzais 72 Minutes Zmanis", .FunctionName = "GetSofZmanShmaMGA72MinutesZmanis", .HebName = "סו''ז שמע עלות-צאת 72 דקות זמנית", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) להמגן אברהם (מג''א) המבוססת על כך שעלות הם 72 דקות זמניה, או 1/10 מהיום לפני הזריחה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the opinion of the magen avraham based on alos being  72 minutes zmaniyos, or 1/10th of the day before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "37", .EngName = "Sof Zman Shma sunrise-sunset (Gra)", .FunctionName = "GetSofZmanShmaGRA", .HebName = "סו''ז שמע נץ-שקיעה (גר''א)", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) על פי הגר''א כשהיום מתחיל בזריחה ומסתיים בשקיעה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) according to the Gra with the day starting at sunrise and ending at sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "38", .EngName = "Sof Zman Shma 3 Hours Before Chatzos", .FunctionName = "GetSofZmanShma3HoursBeforeChatzos", .HebName = "סו''ז שמע 3 שעות לפני חצות", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) מחושבת כ -3 שעות (רגילות ולא זמנית) לפני חצות.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) calculated as 3 hours (regular and not zmaniyos) before chatzos.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "39", .EngName = "Sof Zman Shma 3 Hours Befor Average Chatzos", .FunctionName = "GetSofZmanShmaFixedLocal", .HebName = "סו''ז שמע 3 שעות לפני חצות ממוצע", .HebDescription = "זמן קריאת שמע האחרונה (בבוקר) מחושבת כשלוש שעות לפני חצות ממוצע מקומי.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) calculated as 3 hours before fixed local chatzos.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "40", .EngName = "Sof Zman Shma Baal Hatanya", .FunctionName = "GetSofZmanShmaBaalHatanya", .HebName = "סו''ז תפילה לדעת בעל התניא", .HebDescription = "סו''ז תפילה לדעת בעל התניא המחושב לפי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Sof Zman Shma according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "41", .EngName = "Sof Zman Shma Ateret Torah", .FunctionName = "GetSofZmanShmaAteretTorah", .HebName = "סו''ז שמע עטרת תורה", .HebDescription = "זמן האחרון לקריאת שמע (בבוקר) מבוסס על חישוב חכם יוסף הררי-רפול של ישיבת עטרת תורה, שהיום מתחיל 1/10 מהיום לפני הזריחה ומחושב בדרך כלל כמסתיים 40 דקות אחרי השקיעה.", .EngDescription = "Latest zman krias shema (time to recite shema in the morning) based on the calculation of chacham yosef harari-raful of yeshivat ateret torah, that the day starts 1/10th of the day before sunrise and is usually calculated as ending 40 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "42", .EngName = "Sof Zman Tfila Alos-Tzais 19.8 Degrees", .FunctionName = "GetSofZmanTfilaMGA19Point8Degrees", .HebName = "סו''ז תפילה עלות-צאת 19.8 מעלות", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוסס על עלות 19.8 מעלות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being 19.8° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "43", .EngName = "Sof Zman Tfila Alos-Tzais 18 Degrees", .FunctionName = "GetSofZmanTfilaMGA18Degrees", .HebName = "סו''ז תפילה עלות-צאת 18 מעלות", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוסס על עלות 18 מעלות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being 18° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "44", .EngName = "Sof Zman Tfila Alos-Tzais 16.1 Degrees", .FunctionName = "GetSofZmanTfilaMGA16Point1Degrees", .HebName = "סו''ז תפילה עלות-צאת 16.1 מעלות", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוסס על עלות 16.1 מעלות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being 16.1° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "45", .EngName = "Sof Zman Tfila Alos-Tzais 120 Minutes", .FunctionName = "GetSofZmanTfilaMGA120Minutes", .HebName = "סו''ז תפילה עלות-צאת 120 דקות", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוססת על עלוה''ש 120 דקות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being  120 minutes minutes before sunrise .", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "46", .EngName = "Sof Zman Tfila Alos-Tzais 96 Minutes", .FunctionName = "GetSofZmanTfilaMGA96Minutes", .HebName = "סו''ז תפילה עלות-צאת 96 דקות", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוססת על עלוה''ש 96 דקות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being  96 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "47", .EngName = "Sof Zman Tfila Alos-Tzais 96 Minutes Zmanis", .FunctionName = "GetSofZmanTfilaMGA96MinutesZmanis", .HebName = "סו''ז תפילה עלות-צאת 96 דקות זמנית", .HebDescription = "זמן תפילה האחרון (זמן לתפילת שחרית) להמגן אברהם (מג''א) המבוססת על עלוה''ש 96 דקות זמניה לפני הזריחה.", .EngDescription = "Latest zman tfila (time to the morning prayers) according to the opinion of the magen avraham based on alos being  96 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "48", .EngName = "Sof Zman Tfila Alos-Tzais 90 Minutes", .FunctionName = "GetSofZmanTfilaMGA90Minutes", .HebName = "סו''ז תפילה עלות-צאת 90 דקות", .HebDescription = "זמן תפילה האחרון (שחרית) להמגן אברהם (מג''א) המבוססת על היותם 90 דקות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being  90 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "49", .EngName = "Sof Zman Tfila Alos-Tzais 90 Minutes Zmanis", .FunctionName = "GetSofZmanTfilaMGA90MinutesZmanis", .HebName = "סו''ז תפילה עלות-צאת 90 דקות זמנית", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוססת על עלוה''ש 90 דקות זמניות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to the morning prayers) according to the opinion of the magen avraham based on alos being  90 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "50", .EngName = "Sof Zman Tfila Alos-Tzais 72 Minutes", .FunctionName = "GetSofZmanTfilaMGA72Minutes", .HebName = "סו''ז תפילה עלות-צאת 72 דקות", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) להמגן אברהם (מג''א) המבוססת על כך שעלות היא 72 דקות לפני הזריחה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the opinion of the magen avraham based on alos being  72 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "51", .EngName = "Sof Zman Tfila Alos-Tzais 72 Minutes Zmanis", .FunctionName = "GetSofZmanTfilaMGA72MinutesZmanis", .HebName = "סו''ז תפילה עלות-צאת 72 דקות זמנית", .HebDescription = "זמן תפילה האחרון (זמן לתפילת שחרית) להמגן אברהם (מג''א) המבוססת על כך שעלות היא 72 דקות זמניה לפני הזריחה.", .EngDescription = "Latest zman tfila (time to the morning prayers) according to the opinion of the magen avraham based on alos being  72 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "52", .EngName = "Sof Zman Tfila sunrise-sunset (Gra)", .FunctionName = "GetSofZmanTfilaGRA", .HebName = "סו''ז תפילה נץ-שקיעה (גר''א)", .HebDescription = "זמן תפילה האחרון (שחרית) על פי הגר''א כשהיום מתחיל בזריחה ומסתיים בשקיעה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) according to the Gra with the day starting at sunrise and ending at sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "53", .EngName = "Sof Zman Tfila 2 Hours Before Chatzos", .FunctionName = "GetSofZmanTfila2HoursBeforeChatzos", .HebName = "סו''ז תפילה 2 שעות לפני חצות", .HebDescription = "זמן תפילה האחרון (שחרית) מחושב כשעתיים לפני חצות ממוצע מקומי.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) calculated as 2 hours before chatzos.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "54", .EngName = "Sof Zman Tfila 2 Hours Before Average Chatzos", .FunctionName = "GetSofZmanTfilaFixedLocal", .HebName = "סו''ז תפילה 2 שעות לפני חצות ממוצע", .HebDescription = "זמן התפילה האחרונה (שחרית) מחושבת כשעתיים לפני חצות ממיץוע מקומי.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) calculated as 2 hours before fixed local chatzos.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "55", .EngName = "Sof Zman Tfila Baal Hatanya", .FunctionName = "GetSofZmanTfilaBaalHatanya", .HebName = "סו''ז שמע לדעת בעל התניא", .HebDescription = "סו''ז שמע לדעת בעל התניא המחושב לפי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Sof Zman Tfila according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "56", .EngName = "Mincha Gedola Ateret Torah", .FunctionName = "GetMinchaGedolaAteretTorah", .HebName = "מנחה גדולה עטרת תורה", .HebDescription = "מנחה גדולה מבוסס על חישוב החכם יוסף הררי-רפול של ישיבת עטרת תורה, שהיום מתחיל 1/10 מהיום לפני הזריחה ומחושב בדרך כלל כמסתיים 40 דקות לאחר השקיעה.", .EngDescription = "Mincha gedola based on the calculation of chacham yosef harari-raful of yeshivat ateret torah, that the day starts 1/10th of the day before sunrise and is usually calculated as ending 40 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "57", .EngName = "Sof Zman Tfilah Ateret Torah", .FunctionName = "GetSofZmanTfilahAteretTorah", .HebName = "סו''ז תפילה עטרת תורה", .HebDescription = "זמן תפילה האחרון (תפילת שחרית) מבוסס על חישוב חכם יוסף הררי-רפול מישיבת עטרת תורה, שהיום מתחיל 1/10 מהיום לפני הזריחה ובדרך כלל מחושב כמסתיים 40 דקות לאחר השקיעה.", .EngDescription = "Latest zman tfila (time to recite the morning prayers) based on the calculation of chacham yosef harari-raful of yeshivat ateret torah, that the day starts 1/10th of the day before sunrise and is usually calculated as ending 40 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "58", .EngName = "Chatzos", .FunctionName = "GetChatzos", .HebName = "חצות היום", .HebDescription = "חצות היום (צהריים) מחושב כמחצית הדרך בין הזריחה לשקיעה.", .EngDescription = "Chatzos (midday) calculated as halfway between sunrise and sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "59", .EngName = "Chatzos Local Mean (average)", .FunctionName = "GetFixedLocalChatzos", .HebName = "חצות ממוצע מקומי", .HebDescription = "זמן מקומי לחצות ממוצע. שעה זו היא צהריים וחצות (12:00) מותאמת משעה רגילה כדי להתחשב בקו הרוחב המקומי", .EngDescription = "Local time for fixed chatzos. This time is noon and midnight (12:00) adjusted from standard time to account for the local latitude", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "60", .EngName = "Mincha Gedola sunrise-sunset (Gra)", .FunctionName = "GetMinchaGedola", .HebName = "מנחה גדולה נץ-שקיעה (גר''א)", .HebDescription = "מנחה גדולה על פי הגר''א כשהיום מתחיל בזריחה ומסתיים בשקיעה.", .EngDescription = "Mincha gedola according to the Gra with the day starting at sunrise and ending at sunset.", .ObjectName = ""},
            New aZmanimFunc With {.Num = "61", .EngName = "Mincha Gedola 30 Minutes", .FunctionName = "GetMinchaGedola30Minutes", .HebName = "מנחה גדולה 30 דקות", .HebDescription = "מנחה גדולה מחושב כ -30 דקות לאחר חצות ולא 1/2 משעה זמנית.", .EngDescription = "Mincha gedola calculated as 30 minutes after chatzos and not 1/2 of a shaah zmanis after chatzos.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "62", .EngName = "Mincha Gedola Greater Than 30 Minutes", .FunctionName = "GetMinchaGedolaGreaterThan30", .HebName = "מנחה גדולה גדול מ-30 דקות", .HebDescription = "בחורף כשחצי שעה זמנית פחות מ -30 דקות נחשב 30 דקות אחרי חצות, ובשאר הפעמים נחשב מנחה גדולה לפי שעה זמנית נץ-שקיעה (גר''א).", .EngDescription = "In the winter when 1/2 of a shaah zmanis is less than 30 minutes Mincha Gedola 30 Minutes will be returned, otherwise Mincha Gedola sunrise-sunset (Gra) will be returned.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "63", .EngName = "Mincha Gedola 72 Minutes (MGA)", .FunctionName = "GetMinchaGedola72Minutes", .HebName = "מנחה גדולה עלות-צאת 72 דקות (מג''א)", .HebDescription = "מנחה גדולה על פי מג''א כשהיום מתחיל 72 דקות לפני הזריחה ומסתיים 72 דקות אחרי השקיעה.", .EngDescription = "Mincha gedola according to the magen avraham with the day starting 72 minutes before sunrise and ending 72 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "64", .EngName = "Mincha Gedola 16.1 Degrees", .FunctionName = "GetMinchaGedola16Point1Degrees", .HebName = "מנחה גדולה עלות-צאת 16.1 מעלות", .HebDescription = "מנחה גדולה לפי מג''א כשהיום מתחיל ומסתיים 16.1 מעלות מתחת לאופק.", .EngDescription = "Mincha gedola according to the magen avraham with the day starting and ending 16.1° below the horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "65", .EngName = "Mincha Gedola Baal Hatanya", .FunctionName = "GetMinchaGedolaBaalHatanya", .HebName = "מנחה גדולה לדעת בעל התניא", .HebDescription = "מנחה גדולה לדעת בעל התניא המחושב לפי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Mincha gedola according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "66", .EngName = "Mincha Gedola Baal Hatanya Greater Than 30", .FunctionName = "GetMinchaGedolaBaalHatanyaGreaterThan30", .HebName = "מנחה גדולה גדול מ-30 דקות לדעת בעל התניא", .HebDescription = "בחורף כשחצי שעה זמנית פחות מ -30 דקות נחשב 30 דקות אחרי חצות, ובשאר זמנים נחשב מנחה גדולה לפי בעל התניא.", .EngDescription = "In the winter when 1/2 of a shaah zmanis is less than 30 minutes Mincha Gedola 30 Minutes will be returned, otherwise Baal Hatanya's Mincha Gedola will be returned.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "67", .EngName = "Mincha Ketana sunrise-sunset (Gra)", .FunctionName = "GetMinchaKetana", .HebName = "מנחה קטנה נץ-שקיעה (גר''א)", .HebDescription = "מנחה קטנה על פי הגר''א כשהיום מתחיל בזריחה ומסתיים בשקיעה.", .EngDescription = "Mincha ketana according to the Gra with the day starting at sunrise and ending at sunset.", .ObjectName = ""},
            New aZmanimFunc With {.Num = "68", .EngName = "Mincha Ketana 72 Minutes", .FunctionName = "GetMinchaKetana72Minutes", .HebName = "מנחה קטנה עלות-צאת 72 דקות (מג''א)", .HebDescription = "מנחה קטנה על פי מג''א כשהיום מתחיל 72 דקות לפני הזריחה ומסתיים 72 דקות אחרי השקיעה.", .EngDescription = "Mincha ketana according to the magen avraham with the day starting 72 minutes before sunrise and ending 72 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "69", .EngName = "Mincha Ketana 16.1 Degrees", .FunctionName = "GetMinchaKetana16Point1Degrees", .HebName = "מנחה קטנה 16.1 מעלות", .HebDescription = "מנחה קטנה על פי מג''א כשהיום מתחיל ומסתיים 16.1 מעלות מתחת לאופק.", .EngDescription = "Mincha ketana according to the magen avraham with the day starting and ending 16.1° below the horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "70", .EngName = "Mincha Ketana Baal Hatanya", .FunctionName = "GetMinchaKetanaBaalHatanya", .HebName = "מנחה קטנה לדעת בעל התניא", .HebDescription = "מנחה קטנה לדעת בעל התניא על פי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לבעל התניא)", .EngDescription = "Mincha ketana according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "71", .EngName = "Mincha Ketana Ateret Torah", .FunctionName = "GetMinchaKetanaAteretTorah", .HebName = "מנחה קטנה עטרת תורה", .HebDescription = "מנחה קטנה המבוססת על חישוב חכם יוסף הררי-רפול של ישיבת עטרת תורה, שהיום מתחיל 1/10 מהיום לפני הזריחה ומחושב בדרך כלל כמסתיים 40 דקות לאחר השקיעה.", .EngDescription = "Mincha ketana based on the calculation of chacham yosef harari-raful of yeshivat ateret torah, that the day starts 1/10th of the day before sunrise and is usually calculated as ending 40 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "72", .EngName = "Plag Hamincha 26 Degrees", .FunctionName = "GetPlagHamincha26Degrees", .HebName = "פלג המנחה 26 מעלות", .HebDescription = "פלג המנחה מבוססת על הדעה שהיום מתחיל ב-26° ומסתיים ב-26°.", .EngDescription = "Plag hamincha based on the opinion that the day starts at alos 26° and ends at tzais 26°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "73", .EngName = "Plag Hamincha 19.8 Degrees", .FunctionName = "GetPlagHamincha19Point8Degrees", .HebName = "פלג המנחה 19.8 מעלות", .HebDescription = "פלג המנחה מבוססת על הדעה שהיום מתחיל ב-19.8° ומסתיים ב-19.8°.", .EngDescription = "Plag hamincha based on the opinion that the day starts at alos 19.8° and ends at tzais 19.8°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "74", .EngName = "Plag Hamincha 18 Degrees", .FunctionName = "GetPlagHamincha18Degrees", .HebName = "פלג המנחה 18 מעלות", .HebDescription = "פלג המנחה מבוססת על הדעה שהיום מתחיל ב-18° ומסתיים ב-18°.", .EngDescription = "Plag hamincha based on the opinion that the day starts at alos 18° and ends at tzais 18°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "75", .EngName = "Plag Hamincha 16.1 Degrees", .FunctionName = "GetPlagHamincha16Point1Degrees", .HebName = "פלג המנחה 16.1 מעלות", .HebDescription = "פלג המנחה מבוססת על הדעה שהיום מתחיל בעלות 16.1° ומסתיים בצאת 16.1°.", .EngDescription = "Plag hamincha based on the opinion that the day starts at alos 16.1° and ends at tzais 16.1°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "76", .EngName = "Plag Hamincha Alos (16.1°) To Sunset", .FunctionName = "GetPlagAlosToSunset", .HebName = "פלג המחנה עלות (16.1°) לשקיעה", .HebDescription = "פלג המנחה מבוססת על הדעה שהיום מתחיל בעלות 16.1 מעלות ומסתיים בשקיעה.", .EngDescription = "Plag hamincha based on the opinion that the day starts at alos 16.1° and ends at sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "77", .EngName = "Plag Hamincha Alos 16.1 To Tzais Geonim 7.083 Degrees", .FunctionName = "GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees", .HebName = "פלג המנחה עלות 16.1 לצאת 7.083 מעלות", .HebDescription = "פלג המנחה מבוססת על הדעה שהיום מתחיל בעלות 16.1 מעלות ומסתיים בצאת 7.083°.", .EngDescription = "Plag hamincha based on the opinion that the day starts at alos 16.1° and ends at tzais 7.083°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "78", .EngName = "Plag Hamincha 8.5 Degrees", .FunctionName = "ZmanGetPlagHamincha8Point5Degrees", .HebName = "פלג המנחה 8.5 מעלות", .HebDescription = "פלג המנחה מחושבת כשהיום מתחיל ב 8.5° ומסתיים ב 8.5°. (זמן זו נוספה על ידי myzman ואינה חלק מ-kosher java)", .EngDescription = "Plag hamincha calculated with the day starting at 8.5° and ending at 8.5°. (this zman was added by myzman and is not part of kosher java)", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "79", .EngName = "Plag Hamincha 120 Minutes", .FunctionName = "GetPlagHamincha120Minutes", .HebName = "פלג המנחה 120 דקות", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 120 דקות לפני הזריחה ומסתיים 120 דקות אחרי השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 120 minutes before sunrise and ending 120 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "80", .EngName = "Plag Hamincha 120 Minutes Zmanis", .FunctionName = "GetPlagHamincha120MinutesZmanis", .HebName = "פלג המנחה 120 דקות זמנית", .HebDescription = "פלג המנחה המבוססת על זריחת השמש היא 120 דקות זמנית או 1/6 של היום לפני הזריחה.", .EngDescription = "Plag hamincha based on sunrise being 120 minutes zmaniyos or 1/6th of the day before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "81", .EngName = "Plag Hamincha 96 Minutes", .FunctionName = "GetPlagHamincha96Minutes", .HebName = "פלג המנחה 96 דקות", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 96 דקות לפני הזריחה ומסתיים 96 דקות אחרי השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 96 minutes before sunrise and ending 96 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "82", .EngName = "Plag Hamincha 96 Minutes Zmanis", .FunctionName = "GetPlagHamincha96MinutesZmanis", .HebName = "פלג המנחה 96 דקות זמנית", .HebDescription = "פלג המנחה על פי מג''א כשהיום מתחיל 96 דקות זמניות לפני הזריחה וכלה 96 דקות זמניות לאחר השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 96 minutes zmanis before sunrise and ending 96 minutes zmanis after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "83", .EngName = "Plag Hamincha 90 Minutes", .FunctionName = "GetPlagHamincha90Minutes", .HebName = "פלג המנחה 90 דקות", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 90 דקות לפני הזריחה ומסתיים 90 דקות אחרי השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 90 minutes before sunrise and ending 90 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "84", .EngName = "Plag Hamincha 90 Minutes Zmanis", .FunctionName = "GetPlagHamincha90MinutesZmanis", .HebName = "פלג המנחה 90 דקות זמנית", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 90 דקות זמניות לפני הזריחה ומסתיים 90 דקות זמניות לאחר השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 90 minutes zmanis before sunrise and ending 90 minutes zmanis after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "85", .EngName = "Plag Hamincha 72 Minutes", .FunctionName = "GetPlagHamincha72Minutes", .HebName = "פלג המנחה 72 דקות", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 72 דקות לפני הזריחה ומסתיים 72 דקות אחרי השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 72 minutes before sunrise and ending 72 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "86", .EngName = "Plag Hamincha 72 Minutes Zmanis", .FunctionName = "GetPlagHamincha72MinutesZmanis", .HebName = "פלג המנחה 72 דקות זמנית", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 72 דקות זמניות לפני הזריחה ומסתיים 72 דקות זמניות לאחר השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 72 minutes zmanis before sunrise and ending 72 minutes zmanis after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "87", .EngName = "Plag Hamincha 60 Minutes", .FunctionName = "GetPlagHamincha60Minutes", .HebName = "פלג המנחה 60 דקות", .HebDescription = "פלג המנחה לפי מג''א כשהיום מתחיל 60 דקות לפני הזריחה ומסתיים 60 דקות אחרי השקיעה.", .EngDescription = "Plag hamincha according to the magen avraham with the day starting 60 minutes before sunrise and ending 60 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "88", .EngName = "Plag Hamincha sunrise-sunset (Gra)", .FunctionName = "GetPlagHamincha", .HebName = "פלג המנחה נץ-שקיעה (גר''א)", .HebDescription = "פלג המנחה על פי הגר''א כשהיום מתחיל בזריחה ומסתיים בשקיעה.", .EngDescription = "Plag Hamincha according to the Gra with the day starting at sunrise and ending at sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "89", .EngName = "Plag Hamincha Baal Hatanya", .FunctionName = "GetPlagHaminchaBaalHatanya", .HebName = "פלג המנחה לדעת בעל התניא", .HebDescription = "פלג המנחה לדעת בעל התניא המחושב לפי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Plag Hamincha according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "90", .EngName = "Plag Hamincha Ateret Torah", .FunctionName = "GetPlagHaminchaAteretTorah", .HebName = "פלג המנחה עטרת תורה", .HebDescription = "פלג המנחה מבוססת על חישוב החכם יוסף הררי-רפול של ישיבת עטרת תורה, שהיום מתחיל 1/10 מהיום לפני הזריחה ומחושב בדרך כלל כמסתיים 40 דקות לאחר השקיעה.", .EngDescription = "Plag hamincha based on the calculation of chacham yosef harari-raful of yeshivat ateret torah, that the day starts 1/10th of the day before sunrise and is usually calculated as ending 40 minutes after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "91", .EngName = "Bain Hasmashos Yereim 3.05 Degrees", .FunctionName = "GetBainHasmashosYereim3Point05Degrees", .HebName = "בין השמשות יראים 3.05 מעלות", .HebDescription = "תחילת בין השמשות לדעת היראים (רבי אליעזר ממיץ) מחושב כשהשמש 3.05 מעלות מעל האופק ביום השוויון בירושלים, שהיא 18 דקות (או 3/4 מתוך מיל בת 24 דקות) לפני השקיעה. והבין השמשות מתחיל ב -3/4 מיל לפני השקיעה והלילה בשקיעה.", .EngDescription = "beginning of bain hashmashos (twilight) according to the Yereim (Rabbi Eliezer of Metz) calculated as the sun's position 3.05° above the horizon during the equinox in Yerushalayim, its position 18 minutes or 3/4 of an 24 minute Mil before sunset. According to the Yereim, bain hashmashos starts 3/4 of a Mil before sunset and tzais or nightfall starts at sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "92", .EngName = "Bain Hasmashos Yereim 2.8 Degrees", .FunctionName = "GetBainHasmashosYereim2Point8Degrees", .HebName = "בין השמשות יראים 2.8 מעלות", .HebDescription = "תחילת בין השמשות לדעת היראים (רבי אליעזר ממיץ) מחושב כשהשמש 2.8 מעלות מעל האופק ביום השוויון בירושלים, שהיא 16.8 דקות (או 3/4 מתוך מיל בת 22.5 דקות) לפני השקיעה. והבין השמשות מתחיל ב -3/4 מיל לפני השקיעה והלילה בשקיעה.", .EngDescription = "beginning of bain hashmashos (twilight) according to the Yereim (Rabbi Eliezer of Metz) calculated as the sun's position 2.8° above the horizon during the equinox in Yerushalayim, its position 16.8 minutes or 3/4 of an 22.5 minute Mil before sunset. According to the Yereim, bain hashmashos starts 3/4 of a Mil before sunset and tzais or nightfall starts at sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "93", .EngName = "Bain Hasmashos Yereim 2.1 Degrees", .FunctionName = "GetBainHasmashosYereim2Point1Degrees", .HebName = "בין השמשות יראים 2.1 מעלות", .HebDescription = "תחילת בין השמשות לדעת היראים (רבי אליעזר ממיץ) מחושב כשהשמש 2.1 מעלות מעל האופק ביום השוויון בירושלים, שהיא 13.5 דקות (או 3/4 מתוך מיל בת 18 דקות) לפני השקיעה. והבין השמשות מתחיל ב -3/4 מיל לפני השקיעה והלילה בשקיעה.", .EngDescription = "beginning of bain hashmashos (twilight) according to the Yereim (Rabbi Eliezer of Metz) calculated as the sun's position 2.1° above the horizon during the equinox in Yerushalayim, its position 13.5 minutes or 3/4 of an 18 minute Mil before sunset. According to the Yereim, bain hashmashos starts 3/4 of a Mil before sunset and tzais or nightfall starts at sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "94", .EngName = "Bain Hasmashos Yereim 18 Minutes", .FunctionName = "GetBainHasmashosYereim18Minutes", .HebName = "בין השמשות יראים 18 דקות", .HebDescription = "תחילת בין השמשות לדעת היראים (רבי אליעזר ממיץ) מחושב כ -18 דקות או 3/4 מתוך מיל בת 24 דקות לפני השקיעה. והבין השמשות מתחיל ב -3/4 מיל לפני השקיעה והלילה בשקיעה.", .EngDescription = "beginning of bain hashmashos (twilight) according to the Yereim (Rabbi Eliezer of Metz) calculated as 18 minutes or 3/4 of an 24 minute Mil before sunset. According to the Yereim, bain hashmashos starts 3/4 of a Mil before sunset and tzais or nightfall starts at sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "95", .EngName = "Bain Hasmashos Yereim 16.875 Minutes", .FunctionName = "GetBainHasmashosYereim16Point875Minutes", .HebName = "בין השמשות יראים 16.8 דקות", .HebDescription = "תחילת בין השמשות לדעת היראים (רבי אליעזר ממיץ) מחושב כ -16.8 דקות או 3/4 מתוך מיל בת 22.5 דקות לפני השקיעה. והבין השמשות מתחיל ב -3/4 מיל לפני השקיעה והלילה בשקיעה.", .EngDescription = "beginning of bain hashmashos (twilight) according to the Yereim (Rabbi Eliezer of Metz) calculated as 16.8 minutes or 3/4 of an 22.5 minute Mil before sunset. According to the Yereim, bain hashmashos starts 3/4 of a Mil before sunset and tzais or nightfall starts at sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "96", .EngName = "Bain Hasmashos Yereim 13.5 Minutes", .FunctionName = "GetBainHasmashosYereim13Point5Minutes", .HebName = "בין השמשות יראים 13.5 דקות", .HebDescription = "תחילת בין השמשות לדעת היראים (רבי אליעזר ממיץ) מחושב כ -13.5 דקות או 3/4 מתוך מיל בת 18 דקות לפני השקיעה. והבין השמשות מתחיל ב -3/4 מיל לפני השקיעה והלילה בשקיעה.", .EngDescription = "beginning of bain hashmashos (twilight) according to the Yereim (Rabbi Eliezer of Metz) calculated as 13.5 minutes or 3/4 of an 18 minute Mil before sunset. According to the Yereim, bain hashmashos starts 3/4 of a Mil before sunset and tzais or nightfall starts at sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "97", .EngName = "Sunset", .FunctionName = "GetSunset", .HebName = "שקיעות החמה", .HebDescription = "זמן השקיעה מותאם לגובה.", .EngDescription = "Elevation adjusted sunset time.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "98", .EngName = "Sea Level Sunset", .FunctionName = "GetSeaLevelSunset", .HebName = "שקיעות החמה במפלס פני הים", .HebDescription = "שקיעה ללא התאמת גובה", .EngDescription = "Sunset without elevation adjustment", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "99", .EngName = "Sunset Baal Hatanya", .FunctionName = "GetSunsetBaalHatanya", .HebName = "שקיעות החמה לדעת בעל התניא", .HebDescription = "שקיעה הלכתי לדעת בעל התניא (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Baal Hatanya's shkiah amiti (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "100", .EngName = "Tzais 3.65 Degrees", .FunctionName = "GetTzaisGeonim3Point65Degrees", .HebName = "צאה''כ 3.65 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 3.65 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 3.65° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "101", .EngName = "Tzais 3.676 Degrees", .FunctionName = "GetTzaisGeonim3Point676Degrees", .HebName = "צאה''כ 3.676 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 3.676 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 3.676° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "102", .EngName = "Tzais 3.7 Degrees", .FunctionName = "GetTzaisGeonim3Point7Degrees", .HebName = "צאה''כ 3.7 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 3.7 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 3.7° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "103", .EngName = "Tzais 3.8 Degrees", .FunctionName = "GetTzaisGeonim3Point8Degrees", .HebName = "צאה''כ 3.8 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 3.8 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 3.8° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "104", .EngName = "Tzais 4.37 Degrees", .FunctionName = "GetTzaisGeonim4Point37Degrees", .HebName = "צאה''כ 4.37 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 4.37 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 4.37° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "105", .EngName = "Tzais 4.61 Degrees", .FunctionName = "GetTzaisGeonim4Point61Degrees", .HebName = "צאה''כ 4.61 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 4.61 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 4.61° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "106", .EngName = "Tzais 4.8 Degrees", .FunctionName = "GetTzaisGeonim4Point8Degrees", .HebName = "צאה''כ 4.8 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 4.8 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 4.8° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "107", .EngName = "Tzais 5.88 Degrees", .FunctionName = "GetTzaisGeonim5Point88Degrees", .HebName = "צאה''כ 5.88 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 5.88 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 5.88° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "108", .EngName = "Tzais 5.95 Degrees", .FunctionName = "GetTzaisGeonim5Point95Degrees", .HebName = "צאה''כ 5.95 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 5.95 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 5.95° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "109", .EngName = "Tzais 6.45 Degrees", .FunctionName = "GetTzaisGeonim6Point45Degrees", .HebName = "צאה''כ 6.45 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 6.45 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 6.45° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "110", .EngName = "Tzais 7.083 Degrees", .FunctionName = "GetTzaisGeonim7Point083Degrees", .HebName = "צאה''כ 7.083 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 7.083 מעלות מתחת לאופק. על בסיס 30 דקות לאחר השקיעה ביום השוויון (16 במרץ) בירושלים.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 7.083° below the western horizon. based on 30 minutes after sunset during the equinox in yerushalayim.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "111", .EngName = "Tzais 7.67 Degrees", .FunctionName = "GetTzaisGeonim7Point67Degrees", .HebName = "צאה''כ 7.67 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 7.67 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 7.67° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "112", .EngName = "Tzais 8.5 Degrees", .FunctionName = "GetTzaisGeonim8Point5Degrees", .HebName = "צאה''כ 8.5 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מבוסס על הגאונים המחושבים במיקום השמש ב 8.5 מעלות מתחת לאופק המערבי.", .EngDescription = "Tzais (nightfall) based on the opinion of the geonim calculated at the sun's position at 8.5° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "113", .EngName = "Tzais 9.3 Degrees", .FunctionName = "GetTzaisGeonim9Point3Degrees", .HebName = "צאה''כ 9.3 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 9.3 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 9.3° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "114", .EngName = "Tzais 9.75 Degrees", .FunctionName = "GetTzaisGeonim9Point75Degrees", .HebName = "צאה''כ 9.75 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 9.75 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) based on the opinion of the Geonim calculated at the sun's position at 9.75° below the western horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "115", .EngName = "Tzais 16.1 Degrees", .FunctionName = "GetTzais16Point1Degrees", .HebName = "צאה''כ 16.1 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 16.1 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) calculated when the sun is 16.1° below the horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "116", .EngName = "Tzais 18 Degrees", .FunctionName = "GetTzais18Degrees", .HebName = "צאה''כ 18 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 18 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) calculated when the sun is 18° below the horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "117", .EngName = "Tzais 19.8 Degrees", .FunctionName = "GetTzais19Point8Degrees", .HebName = "צאה''כ 19.8 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 19.8 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) calculated when the sun is 19.8° below the horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "118", .EngName = "Tzais 26 Degrees", .FunctionName = "GetTzais26Degrees", .HebName = "צאה''כ 26 מעלות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כאשר השמש נמצאת 26 מעלות מתחת לאופק.", .EngDescription = "Tzais (nightfall) calculated when the sun is 26° below the horizon.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "119", .EngName = "Tzais 60 Minutes", .FunctionName = "GetTzais60", .HebName = "צאה''כ 60 דקות", .HebDescription = "צאה''כ (רדת הלילה) מבוססת על דעתם של החוות יאיר  ודברי מלכיאל שאורך מיל הוא 15 דקות ונמצא ל4 מיל 60 דקות מהשקיע.", .EngDescription = "Tzais (nightfall) based on the opinion of the chavas yair and divrei malkiel that the time to walk the distance of a mil is 15 minutes for a total of 60 minutes for 4 mil after sea level sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "120", .EngName = "Tzais 72 Minute", .FunctionName = "GetTzais72", .HebName = "צאת 72 דקות", .HebDescription = "צאת הכוכבים מחושב ב-72 דקות שוות אחרי השקיעה.", .EngDescription = "Tzais (nightfall) calculated as 72 minutes after sea level sunset", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "121", .EngName = "Tzais 72 Minutes Zmanis", .FunctionName = "GetTzais72Zmanis", .HebName = "צאה''כ 72 דקות זמנית", .HebDescription = "צאה''כ (רדת הלילה) מחושב כ- 72 דקות זמנית, או 1/10 מהיום לאחר שקיעת החמה. זו הדרך שבה מנחת כהן במאמר 2: 4 מחשב דעתו של רבינו תם.", .EngDescription = "Tzais (nightfall) calculated as 72 minutes zmaniyos, or 1/10th of the day after sea level sunset.this is the way that the minchas cohen in ma'amar 2:4 calculates rebbeinu tam's time of tzeis.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "122", .EngName = "Tzais 90 Minutes", .FunctionName = "GetTzais90", .HebName = "צאה''כ 90 דקות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כ 90 דקות לאחר שקיעת החמה.", .EngDescription = "Tzais (nightfall) calculated as 90 minutes after sea level sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "123", .EngName = "Tzais 90 Minutes Zmanis", .FunctionName = "GetTzais90Zmanis", .HebName = "צאה''כ 90 דקות זמנית", .HebDescription = "צאה''כ (רדת הלילה) מחושב באמצעות 90 דקות זמניה לאחר שקיעת החמה.", .EngDescription = "Tzais (nightfall) calculated using 90 minutes zmaniyos after sea level sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "124", .EngName = "Tzais 96 Minutes", .FunctionName = "GetTzais96", .HebName = "צאה''כ 96 דקות", .HebDescription = "צאה''כ (רדת הלילה) מחושב כ 96 דקות לאחר שקיעת החמה.", .EngDescription = "Tzais (nightfall) calculated as 96 minutes after sea level sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "125", .EngName = "Tzais 96 Minutes Zmanis", .FunctionName = "GetTzais96Zmanis", .HebName = "צאה''כ 96 דקות זמנית", .HebDescription = "צאה''כ (רדת הלילה) מחושב באמצעות 96 דקות זמןיות לאחר שקיעת החמה.", .EngDescription = "Tzais (nightfall) calculated using 96 minutes zmaniyos after sea level sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "126", .EngName = "Tzais 120 Minutes", .FunctionName = "GetTzais120", .HebName = "צאה''כ 120 דקות", .HebDescription = "צאה''כ (רדת הלילה) 120 דקות אחרי השקיעה, על פי חישוב 5 מיל שכל אחת בת 24 דקות.", .EngDescription = " tzais (nightfall) based on the opinion of the magen avraham that the time to walk the distance of a mil according to the rambam's opinion is 2/5 of an hour (24 minutes) for a total of 120 minutes based on the opinion of ula who calculated tzais as 5 mil after sea level shkiah (sunset).", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "127", .EngName = "Tzais 120 Minutes Zmanis", .FunctionName = "GetTzais120Zmanis", .HebName = "צאה''כ 120 דקות זמנית", .HebDescription = "צאה''כ (רדת הלילה) מחושב תוך שימוש ב- 120 דקות זמנית לאחר שקיעת החמה.", .EngDescription = "Tzais (nightfall) calculated using 120 minutes zmaniyos after sea level sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "128", .EngName = "Tzais Baal Hatanya", .FunctionName = "GetTzaisBaalHatanya", .HebName = "צאה''כ לדעת בעל התניא", .HebDescription = "צאה''כ לדעת בעל התניא, מחושב כאשר השמש נמצאת 6 מעלות מתחת לאופק המזרחי לפני הזריחה. היא מבוססת על מיקום השמש 24 דקות אחרי השקיעה בירושלים ביום השוויון (16 במרץ, היום שבו שעה זמנית היא 60 דקות).", .EngDescription = "Tzais according to the Baal Hatanya based on the position of the sun 24 minutes after sunset in Jerusalem on the equinox (March 16, the day that a solar hour is 60 minutes), which is 6° below geometric zenith", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "129", .EngName = "Tzais Ateret Torah", .FunctionName = "GetTzaisAteretTorah", .HebName = "צאה''כ עטרת תורה", .HebDescription = "צאת מחושב בדרך כלל כ -40 דקות.", .EngDescription = "Tzais usually calculated as 40 minutes.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "130", .EngName = "Bain Hasmashos Rabbeinu Tam 58.5 Minutes", .FunctionName = "GetBainHasmashosRT58Point5Minutes", .HebName = "בין השמשות רבינו תם 58.5 דקות", .HebDescription = "בין השמשות של רבנו תם, מחושב ב-58.5 דקות לאחר השקיעה.", .EngDescription = "Bain hashmashos of rabbeinu tam calculated as a 58.5 minute after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "131", .EngName = "Bain Hasmashos Rabbeinu Tam 13.24 Degrees", .FunctionName = "GetBainHasmashosRT13Point24Degrees", .HebName = "בין השמשות רבינו תם 13.24 מעלות", .HebDescription = "בין השמשות של רבנו תם מחושב כאשר השמש נמצאת 13.24° מתחת לאופק הגיאומטרי המערבי (90 דקות°) לאחר השקיעה.", .EngDescription = "Bain hashmashos of rabbeinu tam calculated when the sun is 13.24° below the western geometric horizon (90 minutes°) after sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "132", .EngName = "Bain Hasmashos Rabbeinu Tam 13.5 Minutes Before 7.083 Degrees", .FunctionName = "GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees", .HebName = "בין השמשות רבינו תם 13.5 דקות לפני 7.083 מעלות", .HebDescription = "בין השמשות של רבנו תם מבוסס על חישוב של 13.5 דקות (3/4 מתוך 18 דקות למיל) לפני צאת מחושב כ 7.083°.", .EngDescription = "Bain hashmashos based on the calculation of 13.5 minutes (3/4 of an 18 minute mil) before tzais calculated as 7.083°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "133", .EngName = "Bain Hasmashos Rabbeinu Tam 13.5 Minutes Before 8.5 Degrees", .FunctionName = "ZmanGetBeinHashmases8Point5Degrees13Min", .HebName = "בין השמשות רבינו תם 13.5 דקות מלפני צאת 8.5 מעלות", .HebDescription = "בין השמשות מבוסס על חישוב של 13.5 דקות (3/4 מתוך 18 דקות מיל) לפני צאת, מחושב כ 8.5°. (זמן זו נוספה על ידי myzman ואינה חלק מ-kosher java)", .EngDescription = "Bain hashmashos based on the calculation of 13.5 minutes (3/4 of an 18 minute mil) before tzais calculated as 8.5°. (this zman was added by myzman and is not part of kosher java)", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "134", .EngName = "Bain Hasmashos Rabbeinu Tam Minchas Cohen", .FunctionName = "ZmanGetBeinHashmases8Point5Degrees5Point3", .HebName = "בין השמשות רבינו תם לפי מנחת כהן", .HebDescription = "בין השמשות של רבנו תם לפי המנחת כהן (מבוא השמש מאמר ב פרק ד). מחושב על ידי חלוקת הזמן בין השקיעה לצאת (מחושב כ 8.5°) ב-5.3 (זמן זו נוספה על ידי myzman ואינה חלק מ-kosher java)", .EngDescription = "Bain hashmashos of rabbeinu tam according to the opinion of the  calculated dividing the time between sunset and tzais (calculated as 8.5°) by 5.3 based on 72 minutes/13.5 minutes (this zman was added by myzman and is not part of kosher java)", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "135", .EngName = "Bain Hasmashos Rabbeinu Tam Divrei Yosef", .FunctionName = "GetBainHasmashosRT2Stars", .HebName = "בין השמשות רבינו תם דברי יוסף", .HebDescription = "בין השמשות של רבנו תם מחושב לפי הדברי יוסף (ראו ישראל והזמנים) חישב 5/18 (27.77%) מהזמן בין עלות (מחושב כ 19.8° לפני הזריחה) לזריחה.", .EngDescription = "Bain hashmashos of rabbeinu tam calculated according to the opinion of the divrei yosef (see Yisroel Vehazmanim) calculated 5/18th (27.77%) of the time between alos (calculated as 19.8° before sunrise) and sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "136", .EngName = "Solar Midnight", .FunctionName = "GetSolarMidnight", .HebName = "חצות לילה", .HebDescription = "חצות לילה.", .EngDescription = "Solar midnight, or the time when the sun is at its nadir.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "137", .EngName = "Candle Lighting 15 Minutes", .FunctionName = "ZmanGetCandleLighting15", .HebName = "זמן הדלקת נרות 15 דקות", .HebDescription = "זמן הדלקת נרות מחושב ב-15 דקות פני השקיעה.", .EngDescription = "candle lighting time calculated at 15  minutes before sunset.", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "138", .EngName = "Candle Lighting 18 Minutes", .FunctionName = "GetCandleLighting", .HebName = "זמן הדלקת נרות 18 דקות", .HebDescription = "זמן הדלקת נרות מחושב ב-18 דקות פני השקיעה.", .EngDescription = "candle lighting time calculated at 18  minutes before sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "139", .EngName = "Candle Lighting 22 Minutes", .FunctionName = "ZmanGetCandleLighting22", .HebName = "זמן הדלקת נרות 22 דקות", .HebDescription = "זמן הדלקת נרות מחושב ב-22 דקות פני השקיעה.", .EngDescription = "candle lighting time calculated at 22  minutes before sunset.", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "140", .EngName = "Candle Lighting 25 Minutes", .FunctionName = "ZmanGetCandleLighting25", .HebName = "זמן הדלקת נרות 25 דקות", .HebDescription = "זמן הדלקת נרות מחושב ב-25 דקות פני השקיעה.", .EngDescription = "candle lighting time calculated at 25  minutes before sunset.", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "141", .EngName = "Candle Lighting 30 Minutes", .FunctionName = "ZmanGetCandleLighting30", .HebName = "זמן הדלקת נרות 30 דקות", .HebDescription = "זמן הדלקת נרות מחושב ב-30 דקות פני השקיעה.", .EngDescription = "candle lighting time calculated at 30  minutes before sunset.", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "142", .EngName = "Candle Lighting 40 Minutes", .FunctionName = "ZmanGetCandleLighting40", .HebName = "זמן הדלקת נרות 40 דקות", .HebDescription = "זמן הדלקת נרות מחושב ב-40 דקות פני השקיעה.", .EngDescription = "candle lighting time calculated at 40  minutes before sunset.", .ObjectName = "varAddedGets"},
            New aZmanimFunc With {.Num = "143", .EngName = "Sof Zman Achilas Chametz Alos-Tzais 16.1 Degrees", .FunctionName = "GetSofZmanAchilasChametzMGA16Point1Degrees", .HebName = "סו''ז אכילת חמץ מג''א 16.1 מעלות", .HebDescription = "סו''ז אכילת חמץ בערב פסח לפי המגן אברהם (מג''א) המבוסס על עלות 16.1 מעלות לפני הזריחה.", .EngDescription = "The latest time one is allowed eating chametz on erev pesach according to the opinion of the magen avraham based on alos being 16.1° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "144", .EngName = "Sof Zman Achilas Chametz Alos-Tzais 72 Minutes", .FunctionName = "GetSofZmanAchilasChametzMGA72Minutes", .HebName = "סו''ז אכילת חמץ מג''א 72 דקות", .HebDescription = "סו''ז אכילת חמץ בערב פסח להמגן אברהם (מג''א) על סמך היותו של 72 דקות לפני הזריחה.", .EngDescription = "The latest time one is allowed eating chametz on erev pesach according to the opinion of the magen avraham based on alos being  72 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "145", .EngName = "Sof Zman Achilas Chametz sunrise-sunset (Gra)", .FunctionName = "GetSofZmanAchilasChametzGRA", .HebName = "סו''ז אכילת חמץ נץ-שקיעה (גר''א)", .HebDescription = "סו''ז אכילת חמץ בערב פסח לפי דעת הגר''א.", .EngDescription = "The latest time one is allowed eating chametz on erev pesach according to the opinion of the gra.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "146", .EngName = "Sof Zman Achilas Chametz Baal Hatanya", .FunctionName = "GetSofZmanAchilasChametzBaalHatanya", .HebName = "סו''ז אכילת חמץ לדעת בעל התניא", .HebDescription = "סו''ז אכילת חמץ לדעת בעל התניא המחושב לפי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Sof Zman Achilas Chametz according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "147", .EngName = "Sof Zman Biur Chametz Alos-Tzais 16.1 Degrees", .FunctionName = "GetSofZmanBiurChametzMGA16Point1Degrees", .HebName = "סו''ז ביעור חמץ מג''א 16.1 מעלות", .HebDescription = "הזמן האחרון לשריפת חמץ בערב פסח להמגן אברהם (מג''א) המבוסס על עלות 16.1 מעלות לפני הזריחה.", .EngDescription = "The latest time for burning chametz on erev pesach according to the opinion of the magen avraham based on alos being 16.1° before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "148", .EngName = "Sof Zman Biur Chametz Alos-Tzais 72 Minutes", .FunctionName = "GetSofZmanBiurChametzMGA72Minutes", .HebName = "סו''ז ביעור חמץ מג''א 72 דקות", .HebDescription = "הזמן האחרון לשריפת חמץ בערב פסח לפי חוות דעתו של מג''א (מג''א) המבוסס על כך שעלות הוא 72 דקות לפני הזריחה.", .EngDescription = "The latest time for burning chametz on erev pesach according to the opinion of the magen avraham based on alos being  72 minutes minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "149", .EngName = "Sof Zman Biur Chametz sunrise-sunset (Gra)", .FunctionName = "GetSofZmanBiurChametzGRA", .HebName = "סו''ז ביעור חמץ נץ-שקיעה (גר''א)", .HebDescription = "הזמן האחרון לשריפת חמץ בערב פסח לפי הגר''א הפעם הוא 5 שעות לתוך היום על סמך הגר''א שהיום מחושב מזריחת השקיעה.", .EngDescription = "The latest time for burning chametz on erev pesach according to the opinion of the gra this time is 5 hours into the day based on the opinion of the gra that the day is calculated from sunrise to sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "150", .EngName = "Sof Zman Biur Chametz Baal Hatanya", .FunctionName = "GetSofZmanBiurChametzBaalHatanya", .HebName = "סו''ז ביעור חמץ לדעת בעל התניא", .HebDescription = "סו''ז ביעור חמץ לדעת בעל התניא המחושב לפי נץ ושקיעה אמתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Sof Zman Biur Chametz according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "151", .EngName = "Shaah Zmanis 26 Degrees", .FunctionName = "GetShaahZmanis26Degrees", .HebName = "שעה זמנית 26 מעלות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 26°.", .EngDescription = "Shaah zmanis (temporal hour) calculated using alos-tzais of 26°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "152", .EngName = "Shaah Zmanis 19.8 Degrees", .FunctionName = "GetShaahZmanis19Point8Degrees", .HebName = "שעה זמנית 19.8 מעלות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 19.8°.", .EngDescription = "Shaah zmanis (temporal hour) calculated using alos-tzais of 19.8°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "153", .EngName = "Shaah Zmanis 18 Degrees", .FunctionName = "GetShaahZmanis18Degrees", .HebName = "שעה זמנית 18 מעלות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 18°.", .EngDescription = "Shaah zmanis (temporal hour) calculated using alos-tzais of 18°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "154", .EngName = "Shaah Zmanis 16.1 Degrees", .FunctionName = "GetShaahZmanis16Point1Degrees", .HebName = "שעה זמנית 16.1 מעלות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 16.1°.", .EngDescription = "Shaah zmanis (temporal hour) calculated using alos-tzais of 16.1°.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "155", .EngName = "Shaah Zmanis 120 Minutes", .FunctionName = "GetShaahZmanis120Minutes", .HebName = "שעה זמנית 120 דקות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 120 דקות.", .EngDescription = "Shaah zmanis (temporal hour) calculated using alos-tzais of 120 minutes.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "156", .EngName = "Shaah Zmanis 120 Minutes Zmanis", .FunctionName = "GetShaahZmanis120MinutesZmanis", .HebName = "שעה זמנית 120 דקות זמנית", .HebDescription = "שעה זמנית לפי מג''א המבוסס על כך שעלות הם 120 דקות זמנית לפני הזריחה.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham based on alos being  120 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "157", .EngName = "Shaah Zmanis 96 Minutes", .FunctionName = "GetShaahZmanis96Minutes", .HebName = "שעה זמנית 96 דקות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 96 דקות.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham  based on alos being  96 minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "158", .EngName = "Shaah Zmanis 96 Minutes Zmanis", .FunctionName = "GetShaahZmanis96MinutesZmanis", .HebName = "שעה זמנית 96 דקות זמנית", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 96 דקות זמנית.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham based on alos being  96 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "159", .EngName = "Shaah Zmanis 90 Minutes", .FunctionName = "GetShaahZmanis90Minutes", .HebName = "שעה זמנית 90 דקות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 90 דקות.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham  based on alos being 90 minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "160", .EngName = "Shaah Zmanis 90 Minutes Zmanis", .FunctionName = "GetShaahZmanis90MinutesZmanis", .HebName = "שעח דקות זמנית 90 דקות זמנית", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 90 דקות זמנית.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham based on alos being  90 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "161", .EngName = "Shaah Zmanis 72 Minutes", .FunctionName = "GetShaahZmanis72Minutes", .HebName = "שעה זמנית 72 דקות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 72 דקות.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham  based on alos being  72 minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "162", .EngName = "Shaah Zmanis 72 Minutes Zmanis", .FunctionName = "GetShaahZmanis72MinutesZmanis", .HebName = "שעה זמנית 72 דקות זמנית", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 72 דקות זמנית.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham based on alos being  72 minutes zmaniyos before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "163", .EngName = "Shaah Zmanis 60 Minutes", .FunctionName = "GetShaahZmanis60Minutes", .HebName = "שעה זמנית 60 דקות", .HebDescription = "שעה זמנית מחושב באמצעות עלות-צאת של 60 דקות.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the magen avraham  based on alos being 60 minutes before sunrise.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "164", .EngName = "Shaah Zmanis sunrise-sunset (Gra)", .FunctionName = "GetShaahZmanisGra", .HebName = "שעה זמנית נץ-שקיעה (גר''א)", .HebDescription = "שעה זמנית על פי הגר''א כשהיום מתחיל בזריחה ומסתיים בשקיעה.", .EngDescription = "Shaah Zmanis according to the Gra with the day starting at sunrise and ending at sunset.", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "165", .EngName = "Shaah Zmanis Baal Hatanya", .FunctionName = "GetShaahZmanisBaalHatanya", .HebName = "שעה זמנית לדעת בעל התניא", .HebDescription = "שעה זמנית לדעת בעל התניא המחושב לפי נץ ושקיעה הלכתי (ראה עוד אצל נץ החמה לדעת בעל התניא)", .EngDescription = "Shaah Zmanis according to the Baal Hatanya based on true sunrise to sunset (see Sunrise Baal Hatanya)", .ObjectName = "varCZC"},
            New aZmanimFunc With {.Num = "166", .EngName = "Shaah Zmanis Ateret Torah", .FunctionName = "GetShaahZmanisAteretTorah", .HebName = "שעה זמנית עטרת תורה", .HebDescription = "שעה זמנית לפי החכם יוסף הררי-רפול של ישיבת עטרת תורה מחושב כאשר עלות הוא 1/10 מזריחה עד יום שקיעה, או 72 דקות של יום כזה לפני הזריחה, וצאת הוא בדרך כלל מחושב כ- 40 דקות.", .EngDescription = "Shaah zmanis (temporal hour) according to the opinion of the chacham yosef harari-raful of yeshivat ateret torah calculated with alos being 1/10th of sunrise to sunset day, or  72 minutes zmaniyos of such a day before sunrise, and tzais is usually calculated as 40 minutes.", .ObjectName = "varCZC"}
         }
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

        'no need for it here
        'Month = HebMtoNom(Month)

        'HebDateStringtoNom = Month & "/" & day & "/" & Year
        Return day & " " & Month & " " & Year

    End Function
    Private Function HebLtoNom(ByVal hebstring As String)
        Dim temp1
        Dim mynum1() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 20, 20, 30, 40, 40, 50, 50, 60, 70, 80, 80, 90, 90,
            100, 200, 300, 400}

        Dim mynum2() As String = {"'", "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט",
        "י", "כ", "ך", "ל", "מ", "ם", "נ", "ן", "ס", "ע", "פ", "ף", "צ", "ץ",
        "ק", "ר", "ש", "ת"}

        For i = 0 To UBound(mynum2)
            If hebstring = mynum2(i) Then temp1 = mynum1(i)
        Next

        Return temp1
    End Function
    Private Function HebMtoNom(ByVal hebstring)
        Dim temp1
        Dim myMonthM1() As String = {"", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר א", "אדר ב", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול"}
        Dim myMonthM2() As Integer = {0, 1, 2, 3, 4, 5, 6.1, 6.2, 7, 8, 9, 10, 11, 12}

        For i = 0 To UBound(myMonthM1)
            If hebstring = myMonthM1(i) Then temp1 = myMonthM2(i)
        Next
        Return temp1
    End Function

    Private Function fix_month_num(ByVal mddate)
        'not in use anymore
        Dim month_num As Integer

        If varHC.GetMonth(mddate) <= 6 Then month_num = varHC.GetMonth(mddate) + 6

        If varHC.GetLeapMonth(varHC.GetYear(mddate)) = 7 Then 'כשהוא שנה מעובר/ שבלא זה מחזיר 0
            If varHC.GetMonth(mddate) = 7 Then month_num = 13 'אדר ב
            If varHC.GetMonth(mddate) >= 8 Then month_num = varHC.GetMonth(mddate) - 7
        Else
            If varHC.GetMonth(mddate) >= 7 Then month_num = varHC.GetMonth(mddate) - 6
        End If
        Return month_num
    End Function
    Public Function TrimStringEllipsis(TextIn As String, FontIn As System.Drawing.Font, MaxSizeInPixels As Integer) As String
        Dim TrimmedText As String
        Dim graphics = (New System.Windows.Forms.Label()).CreateGraphics()
        Dim CurrentSize As Integer = graphics.MeasureString(TextIn, FontIn).Width
        'no need to trim
        If CurrentSize <= MaxSizeInPixels Then Return TextIn

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

        'debug.print(name.PadRight(num, ".") & zman)
        Return name.PadRight(num, ".") & zman
    End Function

End Module

'old
'varZmanimFunc.ForEach(Sub(x) Debug.Print(x))



