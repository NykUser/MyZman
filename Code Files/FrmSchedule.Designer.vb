<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSchedule
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSchedule))
        Me.LabelMinutes = New System.Windows.Forms.Label()
        Me.cbIsActive = New System.Windows.Forms.CheckBox()
        Me.tbMessage = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TabControlTimeZman = New System.Windows.Forms.TabControl()
        Me.TabPageZman = New System.Windows.Forms.TabPage()
        Me.cbZman = New System.Windows.Forms.ComboBox()
        Me.TabPageTime = New System.Windows.Forms.TabPage()
        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
        Me.LabelDivider1 = New System.Windows.Forms.Label()
        Me.cbLocationList = New System.Windows.Forms.ComboBox()
        Me.LabelLocation = New System.Windows.Forms.Label()
        Me.btOpenScheduler = New System.Windows.Forms.Button()
        Me.pbTasksON = New System.Windows.Forms.PictureBox()
        Me.btToggleTaskScheduler = New System.Windows.Forms.Button()
        Me.LabelDivider2 = New System.Windows.Forms.Label()
        Me.LabelReminders = New System.Windows.Forms.Label()
        Me.btFileDialog = New System.Windows.Forms.Button()
        Me.LabelSound = New System.Windows.Forms.Label()
        Me.btTest = New System.Windows.Forms.Button()
        Me.tbSound = New System.Windows.Forms.TextBox()
        Me.tbReminderTotal = New System.Windows.Forms.TextBox()
        Me.LabelMessage = New System.Windows.Forms.Label()
        Me.tbReminderNum = New System.Windows.Forms.TextBox()
        Me.btNext = New System.Windows.Forms.Button()
        Me.LabelFrom = New System.Windows.Forms.Label()
        Me.btLast = New System.Windows.Forms.Button()
        Me.btPrevious = New System.Windows.Forms.Button()
        Me.btFrist = New System.Windows.Forms.Button()
        Me.cbNotToday = New System.Windows.Forms.CheckBox()
        Me.PanelOnTimeBefor = New System.Windows.Forms.Panel()
        Me.dtpMinutes = New System.Windows.Forms.DateTimePicker()
        Me.Butclose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tblatitude = New System.Windows.Forms.TextBox()
        Me.tblongitude = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Labeloffset = New System.Windows.Forms.Label()
        Me.LabelTimeZone = New System.Windows.Forms.Label()
        Me.tbzone = New System.Windows.Forms.TextBox()
        Me.LabelElevation = New System.Windows.Forms.Label()
        Me.tbElevation = New System.Windows.Forms.TextBox()
        Me.LabelLongitude = New System.Windows.Forms.Label()
        Me.cbTimeZone = New System.Windows.Forms.ComboBox()
        Me.LabelLatitude = New System.Windows.Forms.Label()
        Me.rbtClear = New zman.WindowsFormsApplication1.RoundButton()
        Me.GroupBox2.SuspendLayout()
        Me.TabControlTimeZman.SuspendLayout()
        Me.TabPageZman.SuspendLayout()
        Me.TabPageTime.SuspendLayout()
        CType(Me.pbTasksON, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelOnTimeBefor.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelMinutes
        '
        Me.LabelMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelMinutes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMinutes.Location = New System.Drawing.Point(18, 285)
        Me.LabelMinutes.Name = "LabelMinutes"
        Me.LabelMinutes.Size = New System.Drawing.Size(86, 14)
        Me.LabelMinutes.TabIndex = 1
        Me.LabelMinutes.Text = "Remind Before"
        '
        'cbIsActive
        '
        Me.cbIsActive.AutoSize = True
        Me.cbIsActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.cbIsActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbIsActive.Location = New System.Drawing.Point(18, 120)
        Me.cbIsActive.Name = "cbIsActive"
        Me.cbIsActive.Size = New System.Drawing.Size(67, 17)
        Me.cbIsActive.TabIndex = 7
        Me.cbIsActive.Text = "Is Active"
        Me.cbIsActive.UseVisualStyleBackColor = True
        '
        'tbMessage
        '
        Me.tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbMessage.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.tbMessage.Location = New System.Drawing.Point(18, 208)
        Me.tbMessage.Multiline = True
        Me.tbMessage.Name = "tbMessage"
        Me.tbMessage.Size = New System.Drawing.Size(184, 22)
        Me.tbMessage.TabIndex = 12
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TabControlTimeZman)
        Me.GroupBox2.Controls.Add(Me.LabelDivider1)
        Me.GroupBox2.Controls.Add(Me.cbLocationList)
        Me.GroupBox2.Controls.Add(Me.LabelLocation)
        Me.GroupBox2.Controls.Add(Me.btOpenScheduler)
        Me.GroupBox2.Controls.Add(Me.pbTasksON)
        Me.GroupBox2.Controls.Add(Me.btToggleTaskScheduler)
        Me.GroupBox2.Controls.Add(Me.LabelDivider2)
        Me.GroupBox2.Controls.Add(Me.LabelReminders)
        Me.GroupBox2.Controls.Add(Me.btFileDialog)
        Me.GroupBox2.Controls.Add(Me.LabelSound)
        Me.GroupBox2.Controls.Add(Me.btTest)
        Me.GroupBox2.Controls.Add(Me.tbSound)
        Me.GroupBox2.Controls.Add(Me.tbReminderTotal)
        Me.GroupBox2.Controls.Add(Me.LabelMessage)
        Me.GroupBox2.Controls.Add(Me.rbtClear)
        Me.GroupBox2.Controls.Add(Me.tbReminderNum)
        Me.GroupBox2.Controls.Add(Me.btNext)
        Me.GroupBox2.Controls.Add(Me.LabelMinutes)
        Me.GroupBox2.Controls.Add(Me.LabelFrom)
        Me.GroupBox2.Controls.Add(Me.btLast)
        Me.GroupBox2.Controls.Add(Me.btPrevious)
        Me.GroupBox2.Controls.Add(Me.btFrist)
        Me.GroupBox2.Controls.Add(Me.cbIsActive)
        Me.GroupBox2.Controls.Add(Me.cbNotToday)
        Me.GroupBox2.Controls.Add(Me.tbMessage)
        Me.GroupBox2.Controls.Add(Me.PanelOnTimeBefor)
        Me.GroupBox2.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.DodgerBlue
        Me.GroupBox2.Location = New System.Drawing.Point(14, 17)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(217, 395)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Task Scheduler       "
        '
        'TabControlTimeZman
        '
        Me.TabControlTimeZman.Controls.Add(Me.TabPageZman)
        Me.TabControlTimeZman.Controls.Add(Me.TabPageTime)
        Me.TabControlTimeZman.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.TabControlTimeZman.Location = New System.Drawing.Point(16, 143)
        Me.TabControlTimeZman.Name = "TabControlTimeZman"
        Me.TabControlTimeZman.Padding = New System.Drawing.Point(0, 0)
        Me.TabControlTimeZman.SelectedIndex = 0
        Me.TabControlTimeZman.Size = New System.Drawing.Size(186, 47)
        Me.TabControlTimeZman.TabIndex = 84
        '
        'TabPageZman
        '
        Me.TabPageZman.BackColor = System.Drawing.Color.White
        Me.TabPageZman.Controls.Add(Me.cbZman)
        Me.TabPageZman.Location = New System.Drawing.Point(4, 22)
        Me.TabPageZman.Name = "TabPageZman"
        Me.TabPageZman.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageZman.Size = New System.Drawing.Size(178, 21)
        Me.TabPageZman.TabIndex = 1
        Me.TabPageZman.Text = "Zman"
        '
        'cbZman
        '
        Me.cbZman.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbZman.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbZman.DropDownWidth = 185
        Me.cbZman.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbZman.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.cbZman.FormattingEnabled = True
        Me.cbZman.Location = New System.Drawing.Point(-2, -2)
        Me.cbZman.Name = "cbZman"
        Me.cbZman.Size = New System.Drawing.Size(180, 24)
        Me.cbZman.TabIndex = 74
        '
        'TabPageTime
        '
        Me.TabPageTime.BackColor = System.Drawing.Color.White
        Me.TabPageTime.Controls.Add(Me.dtpTime)
        Me.TabPageTime.Location = New System.Drawing.Point(4, 22)
        Me.TabPageTime.Name = "TabPageTime"
        Me.TabPageTime.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageTime.Size = New System.Drawing.Size(178, 21)
        Me.TabPageTime.TabIndex = 0
        Me.TabPageTime.Text = "Time"
        '
        'dtpTime
        '
        Me.dtpTime.CustomFormat = " hh:mm:ss  tt"
        Me.dtpTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTime.Location = New System.Drawing.Point(-2, -1)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.ShowUpDown = True
        Me.dtpTime.Size = New System.Drawing.Size(182, 21)
        Me.dtpTime.TabIndex = 74
        '
        'LabelDivider1
        '
        Me.LabelDivider1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelDivider1.Location = New System.Drawing.Point(106, 343)
        Me.LabelDivider1.Name = "LabelDivider1"
        Me.LabelDivider1.Size = New System.Drawing.Size(96, 2)
        Me.LabelDivider1.TabIndex = 70
        '
        'cbLocationList
        '
        Me.cbLocationList.AutoCompleteCustomSource.AddRange(New String() {"Squre", "york"})
        Me.cbLocationList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbLocationList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbLocationList.DropDownWidth = 105
        Me.cbLocationList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbLocationList.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.cbLocationList.FormattingEnabled = True
        Me.cbLocationList.Location = New System.Drawing.Point(18, 357)
        Me.cbLocationList.Name = "cbLocationList"
        Me.cbLocationList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbLocationList.Size = New System.Drawing.Size(184, 24)
        Me.cbLocationList.TabIndex = 1
        '
        'LabelLocation
        '
        Me.LabelLocation.AutoSize = True
        Me.LabelLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelLocation.Location = New System.Drawing.Point(15, 332)
        Me.LabelLocation.Name = "LabelLocation"
        Me.LabelLocation.Size = New System.Drawing.Size(89, 15)
        Me.LabelLocation.TabIndex = 69
        Me.LabelLocation.Text = "Zman Location"
        '
        'btOpenScheduler
        '
        Me.btOpenScheduler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.btOpenScheduler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btOpenScheduler.Location = New System.Drawing.Point(110, 27)
        Me.btOpenScheduler.Name = "btOpenScheduler"
        Me.btOpenScheduler.Size = New System.Drawing.Size(92, 23)
        Me.btOpenScheduler.TabIndex = 68
        Me.btOpenScheduler.Text = "Open Scheduler"
        Me.btOpenScheduler.UseVisualStyleBackColor = True
        '
        'pbTasksON
        '
        Me.pbTasksON.Image = Global.zman.My.Resources.Resources.red_16
        Me.pbTasksON.ImageLocation = ""
        Me.pbTasksON.Location = New System.Drawing.Point(137, 2)
        Me.pbTasksON.Name = "pbTasksON"
        Me.pbTasksON.Size = New System.Drawing.Size(16, 17)
        Me.pbTasksON.TabIndex = 68
        Me.pbTasksON.TabStop = False
        '
        'btToggleTaskScheduler
        '
        Me.btToggleTaskScheduler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.btToggleTaskScheduler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btToggleTaskScheduler.Location = New System.Drawing.Point(13, 27)
        Me.btToggleTaskScheduler.Name = "btToggleTaskScheduler"
        Me.btToggleTaskScheduler.Size = New System.Drawing.Size(92, 23)
        Me.btToggleTaskScheduler.TabIndex = 66
        Me.btToggleTaskScheduler.Text = "Add"
        Me.btToggleTaskScheduler.UseVisualStyleBackColor = True
        '
        'LabelDivider2
        '
        Me.LabelDivider2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelDivider2.Location = New System.Drawing.Point(85, 66)
        Me.LabelDivider2.Name = "LabelDivider2"
        Me.LabelDivider2.Size = New System.Drawing.Size(115, 2)
        Me.LabelDivider2.TabIndex = 65
        '
        'LabelReminders
        '
        Me.LabelReminders.AutoSize = True
        Me.LabelReminders.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelReminders.Location = New System.Drawing.Point(13, 55)
        Me.LabelReminders.Name = "LabelReminders"
        Me.LabelReminders.Size = New System.Drawing.Size(68, 15)
        Me.LabelReminders.TabIndex = 64
        Me.LabelReminders.Text = "Reminders"
        '
        'btFileDialog
        '
        Me.btFileDialog.FlatAppearance.BorderSize = 0
        Me.btFileDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btFileDialog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.btFileDialog.Image = CType(resources.GetObject("btFileDialog.Image"), System.Drawing.Image)
        Me.btFileDialog.Location = New System.Drawing.Point(174, 236)
        Me.btFileDialog.Name = "btFileDialog"
        Me.btFileDialog.Size = New System.Drawing.Size(28, 16)
        Me.btFileDialog.TabIndex = 14
        Me.btFileDialog.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btFileDialog.UseVisualStyleBackColor = True
        '
        'LabelSound
        '
        Me.LabelSound.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelSound.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelSound.Location = New System.Drawing.Point(18, 239)
        Me.LabelSound.Name = "LabelSound"
        Me.LabelSound.Size = New System.Drawing.Size(187, 14)
        Me.LabelSound.TabIndex = 63
        Me.LabelSound.Text = "Audio Reminder"
        '
        'btTest
        '
        Me.btTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.btTest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btTest.Location = New System.Drawing.Point(118, 300)
        Me.btTest.Name = "btTest"
        Me.btTest.Size = New System.Drawing.Size(84, 22)
        Me.btTest.TabIndex = 16
        Me.btTest.Text = "Test"
        Me.btTest.UseVisualStyleBackColor = True
        '
        'tbSound
        '
        Me.tbSound.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbSound.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbSound.Location = New System.Drawing.Point(18, 254)
        Me.tbSound.Multiline = True
        Me.tbSound.Name = "tbSound"
        Me.tbSound.Size = New System.Drawing.Size(184, 22)
        Me.tbSound.TabIndex = 13
        '
        'tbReminderTotal
        '
        Me.tbReminderTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbReminderTotal.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbReminderTotal.Location = New System.Drawing.Point(129, 81)
        Me.tbReminderTotal.Multiline = True
        Me.tbReminderTotal.Name = "tbReminderTotal"
        Me.tbReminderTotal.Size = New System.Drawing.Size(35, 22)
        Me.tbReminderTotal.TabIndex = 4
        '
        'LabelMessage
        '
        Me.LabelMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMessage.Location = New System.Drawing.Point(18, 193)
        Me.LabelMessage.Name = "LabelMessage"
        Me.LabelMessage.Size = New System.Drawing.Size(187, 14)
        Me.LabelMessage.TabIndex = 61
        Me.LabelMessage.Text = "Reminder Message"
        '
        'tbReminderNum
        '
        Me.tbReminderNum.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbReminderNum.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbReminderNum.Location = New System.Drawing.Point(55, 81)
        Me.tbReminderNum.Multiline = True
        Me.tbReminderNum.Name = "tbReminderNum"
        Me.tbReminderNum.Size = New System.Drawing.Size(35, 22)
        Me.tbReminderNum.TabIndex = 3
        '
        'btNext
        '
        Me.btNext.FlatAppearance.BorderSize = 0
        Me.btNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btNext.Image = Global.zman.My.Resources.Resources.next_32
        Me.btNext.Location = New System.Drawing.Point(166, 80)
        Me.btNext.Name = "btNext"
        Me.btNext.Size = New System.Drawing.Size(20, 23)
        Me.btNext.TabIndex = 5
        Me.btNext.UseVisualStyleBackColor = True
        '
        'LabelFrom
        '
        Me.LabelFrom.AutoSize = True
        Me.LabelFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFrom.Location = New System.Drawing.Point(91, 85)
        Me.LabelFrom.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelFrom.Name = "LabelFrom"
        Me.LabelFrom.Size = New System.Drawing.Size(33, 13)
        Me.LabelFrom.TabIndex = 55
        Me.LabelFrom.Text = "From:"
        '
        'btLast
        '
        Me.btLast.FlatAppearance.BorderSize = 0
        Me.btLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btLast.Image = Global.zman.My.Resources.Resources.last_32
        Me.btLast.Location = New System.Drawing.Point(182, 80)
        Me.btLast.Name = "btLast"
        Me.btLast.Size = New System.Drawing.Size(20, 23)
        Me.btLast.TabIndex = 6
        Me.btLast.UseVisualStyleBackColor = True
        '
        'btPrevious
        '
        Me.btPrevious.FlatAppearance.BorderSize = 0
        Me.btPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btPrevious.Image = Global.zman.My.Resources.Resources.previous_32
        Me.btPrevious.Location = New System.Drawing.Point(30, 80)
        Me.btPrevious.Name = "btPrevious"
        Me.btPrevious.Size = New System.Drawing.Size(20, 23)
        Me.btPrevious.TabIndex = 2
        Me.btPrevious.UseVisualStyleBackColor = True
        '
        'btFrist
        '
        Me.btFrist.FlatAppearance.BorderSize = 0
        Me.btFrist.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btFrist.Image = Global.zman.My.Resources.Resources.first_32
        Me.btFrist.Location = New System.Drawing.Point(14, 80)
        Me.btFrist.Name = "btFrist"
        Me.btFrist.Size = New System.Drawing.Size(20, 23)
        Me.btFrist.TabIndex = 1
        Me.btFrist.UseVisualStyleBackColor = True
        '
        'cbNotToday
        '
        Me.cbNotToday.AutoSize = True
        Me.cbNotToday.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.cbNotToday.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cbNotToday.Location = New System.Drawing.Point(90, 120)
        Me.cbNotToday.Name = "cbNotToday"
        Me.cbNotToday.Size = New System.Drawing.Size(76, 17)
        Me.cbNotToday.TabIndex = 8
        Me.cbNotToday.Text = "Not Today"
        Me.cbNotToday.UseVisualStyleBackColor = True
        '
        'PanelOnTimeBefor
        '
        Me.PanelOnTimeBefor.Controls.Add(Me.dtpMinutes)
        Me.PanelOnTimeBefor.Location = New System.Drawing.Point(18, 302)
        Me.PanelOnTimeBefor.Name = "PanelOnTimeBefor"
        Me.PanelOnTimeBefor.Size = New System.Drawing.Size(87, 19)
        Me.PanelOnTimeBefor.TabIndex = 86
        '
        'dtpMinutes
        '
        Me.dtpMinutes.CustomFormat = "HH:mm"
        Me.dtpMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.dtpMinutes.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpMinutes.Location = New System.Drawing.Point(-1, -1)
        Me.dtpMinutes.Name = "dtpMinutes"
        Me.dtpMinutes.ShowUpDown = True
        Me.dtpMinutes.Size = New System.Drawing.Size(89, 21)
        Me.dtpMinutes.TabIndex = 85
        '
        'Butclose
        '
        Me.Butclose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Butclose.Location = New System.Drawing.Point(24, 257)
        Me.Butclose.Name = "Butclose"
        Me.Butclose.Size = New System.Drawing.Size(0, 0)
        Me.Butclose.TabIndex = 50
        Me.Butclose.Text = "close"
        Me.Butclose.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 0
        Me.ToolTip1.ShowAlways = True
        '
        'tblatitude
        '
        Me.tblatitude.Location = New System.Drawing.Point(18, 37)
        Me.tblatitude.Name = "tblatitude"
        Me.tblatitude.Size = New System.Drawing.Size(57, 24)
        Me.tblatitude.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.tblatitude, "רוחב - Latitude")
        '
        'tblongitude
        '
        Me.tblongitude.Location = New System.Drawing.Point(81, 37)
        Me.tblongitude.Name = "tblongitude"
        Me.tblongitude.Size = New System.Drawing.Size(57, 24)
        Me.tblongitude.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.tblongitude, "אורך - Longitude")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Labeloffset)
        Me.GroupBox1.Controls.Add(Me.LabelTimeZone)
        Me.GroupBox1.Controls.Add(Me.tbzone)
        Me.GroupBox1.Controls.Add(Me.tblatitude)
        Me.GroupBox1.Controls.Add(Me.LabelElevation)
        Me.GroupBox1.Controls.Add(Me.tblongitude)
        Me.GroupBox1.Controls.Add(Me.tbElevation)
        Me.GroupBox1.Controls.Add(Me.LabelLongitude)
        Me.GroupBox1.Controls.Add(Me.cbTimeZone)
        Me.GroupBox1.Controls.Add(Me.LabelLatitude)
        Me.GroupBox1.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.DodgerBlue
        Me.GroupBox1.Location = New System.Drawing.Point(14, 444)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(217, 114)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Location info"
        '
        'Labeloffset
        '
        Me.Labeloffset.AutoSize = True
        Me.Labeloffset.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.Labeloffset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Labeloffset.Location = New System.Drawing.Point(141, 67)
        Me.Labeloffset.Name = "Labeloffset"
        Me.Labeloffset.Size = New System.Drawing.Size(33, 13)
        Me.Labeloffset.TabIndex = 66
        Me.Labeloffset.Text = "offset"
        '
        'LabelTimeZone
        '
        Me.LabelTimeZone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelTimeZone.ForeColor = System.Drawing.Color.Black
        Me.LabelTimeZone.Location = New System.Drawing.Point(15, 66)
        Me.LabelTimeZone.Name = "LabelTimeZone"
        Me.LabelTimeZone.Size = New System.Drawing.Size(122, 14)
        Me.LabelTimeZone.TabIndex = 62
        Me.LabelTimeZone.Text = "Time Zone"
        '
        'tbzone
        '
        Me.tbzone.Location = New System.Drawing.Point(141, 82)
        Me.tbzone.Name = "tbzone"
        Me.tbzone.Size = New System.Drawing.Size(60, 24)
        Me.tbzone.TabIndex = 65
        '
        'LabelElevation
        '
        Me.LabelElevation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelElevation.ForeColor = System.Drawing.Color.Black
        Me.LabelElevation.Location = New System.Drawing.Point(141, 21)
        Me.LabelElevation.Name = "LabelElevation"
        Me.LabelElevation.Size = New System.Drawing.Size(60, 14)
        Me.LabelElevation.TabIndex = 50
        Me.LabelElevation.Text = "Elevation"
        '
        'tbElevation
        '
        Me.tbElevation.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbElevation.Location = New System.Drawing.Point(144, 37)
        Me.tbElevation.Name = "tbElevation"
        Me.tbElevation.Size = New System.Drawing.Size(57, 24)
        Me.tbElevation.TabIndex = 4
        '
        'LabelLongitude
        '
        Me.LabelLongitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelLongitude.ForeColor = System.Drawing.Color.Black
        Me.LabelLongitude.Location = New System.Drawing.Point(78, 21)
        Me.LabelLongitude.Name = "LabelLongitude"
        Me.LabelLongitude.Size = New System.Drawing.Size(60, 14)
        Me.LabelLongitude.TabIndex = 48
        Me.LabelLongitude.Text = "Longitude"
        '
        'cbTimeZone
        '
        Me.cbTimeZone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbTimeZone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbTimeZone.DropDownWidth = 184
        Me.cbTimeZone.FormattingEnabled = True
        Me.cbTimeZone.Location = New System.Drawing.Point(16, 82)
        Me.cbTimeZone.Name = "cbTimeZone"
        Me.cbTimeZone.Size = New System.Drawing.Size(122, 24)
        Me.cbTimeZone.TabIndex = 5
        '
        'LabelLatitude
        '
        Me.LabelLatitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelLatitude.ForeColor = System.Drawing.Color.Black
        Me.LabelLatitude.Location = New System.Drawing.Point(15, 21)
        Me.LabelLatitude.Name = "LabelLatitude"
        Me.LabelLatitude.Size = New System.Drawing.Size(60, 14)
        Me.LabelLatitude.TabIndex = 47
        Me.LabelLatitude.Text = "Latitude"
        '
        'rbtClear
        '
        Me.rbtClear.FlatAppearance.BorderSize = 0
        Me.rbtClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.rbtClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.rbtClear.Image = Global.zman.My.Resources.Resources.broombrushclearsweep22
        Me.rbtClear.Location = New System.Drawing.Point(176, 119)
        Me.rbtClear.Name = "rbtClear"
        Me.rbtClear.Size = New System.Drawing.Size(26, 23)
        Me.rbtClear.TabIndex = 9
        Me.rbtClear.UseVisualStyleBackColor = True
        '
        'FrmSchedule
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AliceBlue
        Me.CancelButton = Me.Butclose
        Me.ClientSize = New System.Drawing.Size(244, 426)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Butclose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(260, 610)
        Me.MinimumSize = New System.Drawing.Size(260, 465)
        Me.Name = "FrmSchedule"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Schedule"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabControlTimeZman.ResumeLayout(False)
        Me.TabPageZman.ResumeLayout(False)
        Me.TabPageTime.ResumeLayout(False)
        CType(Me.pbTasksON, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelOnTimeBefor.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelMinutes As System.Windows.Forms.Label
    Friend WithEvents cbIsActive As System.Windows.Forms.CheckBox
    Friend WithEvents tbMessage As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Butclose As System.Windows.Forms.Button
    Friend WithEvents cbNotToday As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btNext As Button
    Friend WithEvents tbReminderNum As TextBox
    Friend WithEvents LabelFrom As Label
    Friend WithEvents btPrevious As Button
    Friend WithEvents btFrist As Button
    Friend WithEvents btLast As Button
    Friend WithEvents LabelMessage As Label
    Friend WithEvents rbtClear As WindowsFormsApplication1.RoundButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LabelTimeZone As Label
    Friend WithEvents cbLocationList As ComboBox
    Friend WithEvents tblatitude As TextBox
    Friend WithEvents LabelElevation As Label
    Friend WithEvents tblongitude As TextBox
    Friend WithEvents tbElevation As TextBox
    Friend WithEvents LabelLongitude As Label
    Friend WithEvents cbTimeZone As ComboBox
    Friend WithEvents LabelLatitude As Label
    Friend WithEvents tbReminderTotal As TextBox
    Friend WithEvents btTest As Button
    Friend WithEvents LabelSound As Label
    Friend WithEvents tbSound As TextBox
    Friend WithEvents btFileDialog As Button
    Friend WithEvents btToggleTaskScheduler As Button
    Friend WithEvents pbTasksON As PictureBox
    Friend WithEvents LabelDivider2 As Label
    Friend WithEvents LabelReminders As Label
    Friend WithEvents btOpenScheduler As Button
    Friend WithEvents tbzone As TextBox
    Friend WithEvents Labeloffset As Label
    Friend WithEvents LabelDivider1 As Label
    Friend WithEvents LabelLocation As Label
    Friend WithEvents TabControlTimeZman As TabControl
    Friend WithEvents TabPageTime As TabPage
    Friend WithEvents TabPageZman As TabPage
    Friend WithEvents dtpTime As DateTimePicker
    Friend WithEvents cbZman As ComboBox
    Friend WithEvents dtpMinutes As DateTimePicker
    Friend WithEvents PanelOnTimeBefor As Panel
End Class
