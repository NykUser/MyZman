Public Class FrmScheduleMessage
    Private Sub FrmScheduleMessage_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btNo.Font = MemoryFonts.GetFont(1, 9, FontStyle.Regular)
        btYes.Font = MemoryFonts.GetFont(1, 9, FontStyle.Regular)

        If varSC.HebrewMenus = True Then
            Me.Text = "MyZman " & ChrW(&H200F) & "הודעת מתזמן"
            LabelTime.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
            LabelZman.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
            LabelMessage.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
            LabelRemindAgain.Font = MemoryFonts.GetFont(0, 11, FontStyle.Regular)
            LabelTime.RightToLeft = 1
            LabelZman.RightToLeft = 1
            LabelMessage.RightToLeft = 1
            LabelRemindAgain.RightToLeft = 1
            PictureBox1.Location = New System.Drawing.Point(Me.Width - 82, PictureBox1.Location.Y)
            LabelTime.Location = New System.Drawing.Point(12, LabelTime.Location.Y)
            LabelZman.Location = New System.Drawing.Point(12, LabelZman.Location.Y)
            LabelMessage.Location = New System.Drawing.Point(12, LabelMessage.Location.Y)
            LabelRemindAgain.Location = New System.Drawing.Point(12, LabelRemindAgain.Location.Y)
            btNo.Location = New System.Drawing.Point(14, btNo.Location.Y)
            btYes.Location = New System.Drawing.Point(95, btYes.Location.Y)
            btNo.Text = "לא"
            btYes.Text = "כן"
        Else
            LabelTime.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
            LabelZman.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
            LabelMessage.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
            LabelRemindAgain.Font = MemoryFonts.GetFont(0, 10, FontStyle.Regular)
            LabelTime.RightToLeft = 0
            LabelZman.RightToLeft = 0
            LabelMessage.RightToLeft = 0
            LabelRemindAgain.RightToLeft = 0
            PictureBox1.Location = New System.Drawing.Point(12, PictureBox1.Location.Y)
            LabelTime.Location = New System.Drawing.Point(72, LabelTime.Location.Y)
            LabelZman.Location = New System.Drawing.Point(72, LabelZman.Location.Y)
            LabelMessage.Location = New System.Drawing.Point(72, LabelMessage.Location.Y)
            LabelRemindAgain.Location = New System.Drawing.Point(72, LabelRemindAgain.Location.Y)
            btNo.Location = New System.Drawing.Point(263, btNo.Location.Y)
            btYes.Location = New System.Drawing.Point(182, btYes.Location.Y)
            btNo.Text = "No"
            btYes.Text = "Yes"
        End If
    End Sub
    Private Sub btYes_Click(sender As Object, e As EventArgs) Handles btYes.Click
        Me.Close()
    End Sub

    Private Sub btNo_Click(sender As Object, e As EventArgs) Handles btNo.Click
        Me.Close()
    End Sub

    Private Sub FrmScheduleMessage_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'Me.Dispose()
    End Sub
End Class