﻿Public Class FrmExport
    Private Sub ExportQuestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New System.Drawing.Point(Frminfo.Location.X + ((Frminfo.Width - Me.Width) / 2), Frminfo.Bounds.Bottom - (Me.Height + 45))
        If Frminfo.mHebrewMenus.Checked = True Then
            Label1.Text = "ימים לייצוא"
            Label2.Text = "ימים לכל קובץ (vcs)"
            Label1.RightToLeft = 1
            Label2.RightToLeft = 1
            btCSV.Text = "לאקסל"
            btVCS.Text = "VCS"
        Else
            Label1.Text = "Days To Export"
            Label2.Text = "Days Per File (vcs)"
            Label1.RightToLeft = 0
            Label2.RightToLeft = 0
            btCSV.Text = "To Excel"
            btVCS.Text = "To VCS"
        End If
        tbDays.Text = 365
        tbPer.Text = 365

        If varSC.HebrewMenus = True Then
            Try
                Label1.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                Label2.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                btCSV.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
                btVCS.Font = MemoryFonts.GetFont(1, 8.75, FontStyle.Regular)
            Catch ex As Exception
            End Try
        Else
            Try
                Label1.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                Label2.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                btCSV.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
                btVCS.Font = MemoryFonts.GetFont(1, 7.75, FontStyle.Regular)
            Catch ex As Exception
            End Try
        End If


    End Sub
    Private Sub btCancel_Click(sender As Object, e As EventArgs) Handles btCancel.Click
        Me.Close()
    End Sub

    Private Sub tbDays_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbDays.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.KeyChar = ""
    End Sub
    Private Sub tbPer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbPer.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.KeyChar = ""
    End Sub

    Private Sub tbDays_TextChanged(sender As Object, e As EventArgs) Handles tbDays.TextChanged
        If tbDays.Text < tbPer.Text Then tbPer.Text = tbDays.Text
    End Sub

    Private Sub btVCS_Click(sender As Object, e As EventArgs) Handles btVCS.Click
        If IsNumeric(tbDays.Text) = False Or IsNumeric(tbPer.Text) = False Or tbDays.Text = 0 Or tbPer.Text = 0 Then
            Using New Centered_MessageBox(Me, "ParentCenter")
                If varSC.HebrewMenus = True Then
                    MsgBox("!מידע על ימים לייצוא חסרה", MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight)
                Else
                    MsgBox("Missing Days To Export Info!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                End If
            End Using
            Exit Sub
        End If

        If varSC.Zmanim.Count < 1 Then
            Using New Centered_MessageBox(Me, "ParentCenter")
                If varSC.HebrewMenus = True Then
                    MsgBox("!רשימת הזמנים חסרה", MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight)
                Else
                    MsgBox("Missing Zmanim List!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                End If
            End Using
            Exit Sub
        End If

        Dim folder As String
        Dim MY_FolderBrowser As New FolderBrowserDialog
        MY_FolderBrowser.ShowNewFolderButton = True
        MY_FolderBrowser.SelectedPath = My.Computer.FileSystem.SpecialDirectories.Desktop
        Using New Centered_MessageBox(Me, "ParentCenter")
            If MY_FolderBrowser.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                folder = MY_FolderBrowser.SelectedPath
            Else
                Exit Sub
            End If
        End Using

        'folder = My.Computer.FileSystem.SpecialDirectories.Desktop & "\New folder"

        Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
        Dim file As System.IO.StreamWriter

        Dim timeFormat As String = "h:mm:ss" ' no am pm RTL
        If varSC.Clock24Hour = True Then timeFormat = "H:mm:ss"
        Dim MyZman As Date
        Dim Work_Date As Date = Frminfo.dpEngdate.Value
        Dim NumDays As Integer = tbDays.Text
        Dim DaysPerFile As Integer = tbPer.Text
        Dim DayCount As Integer
        Dim ResultArray
        Dim HebDatetoShow As String
        Dim i As Integer = 0
        Dim Args() ' for AddedGets
        Dim TempLocation As GeoLocation = varGeoLocation
        Dim TempCZC As ComplexZmanimCalendar


        Debug.Print("numdays: " & NumDays & " daysper: " & DaysPerFile & " daycount: " & DayCount)
        file = My.Computer.FileSystem.OpenTextFileWriter(folder & "\" & work_date.ToString("MM-dd-yy") & ".vcs", False, utf8WithoutBom) ' .GetEncoding(1255) 'System.Text.Encoding.UTF8
        file.WriteLine("BEGIN:VCALENDAR" & vbCrLf & "VERSION:1.0")

        For i = 1 To NumDays

            DayCount = DayCount + 1
            If DayCount > DaysPerFile Then
                Debug.Print("numdays: " & NumDays & " daysper: " & DaysPerFile & " daycount: " & DayCount)
                DayCount = DayCount - DaysPerFile
                file.WriteLine("END:VCALENDAR")
                file.Close()
                file = My.Computer.FileSystem.OpenTextFileWriter(folder & "\" & work_date.ToString("MM-dd-yy") & ".vcs", False, utf8WithoutBom) ' .GetEncoding(1255) 'System.Text.Encoding.UTF8
                file.WriteLine("BEGIN:VCALENDAR" & vbCrLf & "VERSION:1.0")
            End If

            ResultArray = Get_HebDate(work_date)
            HebDatetoShow = ResultArray(0) & " " & ResultArray(1)
            If ResultArray(2) <> " " Then HebDatetoShow = HebDatetoShow & " " & ResultArray(2)
            If ResultArray(3) <> "קודם הדף" And ResultArray(3) <> "??" Then HebDatetoShow = HebDatetoShow & " " & ResultArray(3)
            TempCZC = New ComplexZmanimCalendar(Work_Date, TempLocation)

            file.WriteLine("BEGIN:VEVENT" & vbCrLf & "DTSTART:" & work_date.ToString("yyyyMMdd") & vbCrLf _
                           & "DTEND:" & work_date.AddDays(1).ToString("yyyyMMdd") & vbCrLf _
                           & "SUMMARY:" & ResultArray(0))

            'dont use WriteLine - zmanim al need to be on DESCRIPTION line
            file.Write("DESCRIPTION:" & HebDatetoShow & " " & "\n\n")

            For Each Z In varSC.Zmanim
                If InStr(Z.FunctionName, "ShaahZmanis") > 0 Then
                    Try
                        MyZman = #12:00:00 AM#.AddMilliseconds(CallByName(TempCZC, Z.FunctionName, CallType.Get))
                        file.Write(Z.DisplayName & "\n" & ChrW(&H200F) & MyZman.ToString("H:mm:ss") & "\n\n") '
                    Catch ex As Exception
                    End Try
                Else
                    Try
                        If Z.ObjectName = "varCZC" Then
                            MyZman = CDate(CallByName(TempCZC, Z.FunctionName, CallType.Get)).ToString(timeFormat)
                        Else
                            Args = {Work_Date, TempLocation} ' for AddedGets
                            MyZman = CDate(CallByName(varAddedGets, Z.FunctionName, CallType.Get, Args))
                        End If
                        file.Write(Z.DisplayName & "\n" & ChrW(&H200F) & StrConv(MyZman.ToString(timeFormat), VbStrConv.Lowercase) & "\n\n")
                    Catch ex As Exception
                    End Try
                End If
            Next
            file.Write(ChrW(&H200E) & "Lat: " & varGeoLocation.Latitude & " long: " & varGeoLocation.Longitude)
            file.WriteLine(vbCrLf & "END:VEVENT")

            work_date = work_date.AddDays(1)
        Next
        file.WriteLine("END:VCALENDAR")
        file.Close()

        Process.Start(folder)
        Me.Close()
    End Sub
    Private Sub btCSV_Click(sender As Object, e As EventArgs) Handles btCSV.Click

        If IsNumeric(tbDays.Text) = False Or tbDays.Text = 0 Then
            Using New Centered_MessageBox(Me, "ParentCenter")
                If varSC.HebrewMenus = True Then
                    MsgBox("!מידע על ימים לייצוא חסרה", MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight)
                Else
                    MsgBox("Missing Days To Export Info!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                End If
            End Using
            Exit Sub
        End If

        If varSC.Zmanim.Count < 1 Then
            Using New Centered_MessageBox(Me, "ParentCenter")
                If varSC.HebrewMenus = True Then
                    MsgBox("!רשימת הזמנים חסרה", MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxRight)
                Else
                    MsgBox("Missing Zmanim List!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                End If
            End Using
            Exit Sub
        End If

        Dim folder As String
        Dim MY_FolderBrowser As New FolderBrowserDialog
        MY_FolderBrowser.ShowNewFolderButton = True
        MY_FolderBrowser.SelectedPath = My.Computer.FileSystem.SpecialDirectories.Desktop
        Using New Centered_MessageBox(Me, "ParentCenter")
            If MY_FolderBrowser.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                folder = MY_FolderBrowser.SelectedPath
            Else
                Exit Sub
            End If
        End Using

        'folder = My.Computer.FileSystem.SpecialDirectories.Desktop & "\New folder"

        Dim File As System.IO.StreamWriter
        Dim FileFullPath As String = folder & "\MyZman VCS Export Lat " & varGeoLocation.Latitude & " long " & varGeoLocation.Longitude & ".csv" '& Now.ToString("M-d-yy H.m")

        Dim timeFormat As String = "h:mm:ss" ' no am pm RTL
        If varSC.Clock24Hour = True Then timeFormat = "H:mm:ss"
        Dim MyZman As Date
        Dim Work_Date As Date = Frminfo.dpEngdate.Value
        Dim NumDays As Integer = tbDays.Text
        Dim ResultArray
        Dim i As Integer = 0
        Dim Args() ' for AddedGets
        Dim TempLocation As GeoLocation = varGeoLocation
        Dim TempCZC As ComplexZmanimCalendar

        File = My.Computer.FileSystem.OpenTextFileWriter(FileFullPath, False, System.Text.Encoding.UTF8) 'System.Text.Encoding.UTF8
        Dim RowString As String
        Dim HeaderString As String = "Date,תאריך,פרשה,יום טוב,דף היומי,"

        For Each Z In varSC.Zmanim
            HeaderString = HeaderString & Z.DisplayName & ","
        Next

        file.WriteLine(HeaderString)

        For i = 1 To NumDays
            TempCZC = New ComplexZmanimCalendar(Work_Date, TempLocation)
            ResultArray = Get_HebDate(Work_Date)
            RowString = Work_Date.ToShortDateString & "," & ResultArray(0) & "," & ResultArray(1) & "," & ResultArray(2) & "," & ResultArray(3) & ","

            For Each Z In varSC.Zmanim
                If InStr(Z.FunctionName, "ShaahZmanis") > 0 Then
                    Try
                        MyZman = #12:00:00 AM#.AddMilliseconds(CallByName(TempCZC, Z.FunctionName, CallType.Get))
                        RowString = RowString & MyZman.ToString("H:mm:ss") & ","
                    Catch ex As Exception
                    End Try
                Else
                    Try
                        If Z.ObjectName = "varCZC" Then
                            MyZman = CDate(CallByName(TempCZC, Z.FunctionName, CallType.Get)).ToString(timeFormat)
                        Else
                            Args = {Work_Date, TempLocation} ' for AddedGets
                            MyZman = CDate(CallByName(varAddedGets, Z.FunctionName, CallType.Get, Args))
                        End If
                        RowString = RowString & MyZman.ToString(timeFormat) & ","
                    Catch ex As Exception
                    End Try
                End If
            Next
            File.WriteLine(RowString)

            Work_Date = Work_Date.AddDays(1)
        Next
        File.Close()

        Process.Start(FileFullPath)
        Me.Close()
    End Sub

End Class