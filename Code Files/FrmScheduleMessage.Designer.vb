<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmScheduleMessage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmScheduleMessage))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btYes = New System.Windows.Forms.Button()
        Me.btNo = New System.Windows.Forms.Button()
        Me.LabelZman = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.LabelMessage = New System.Windows.Forms.Label()
        Me.LabelRemindAgain = New System.Windows.Forms.Label()
        Me.LabelTime = New System.Windows.Forms.Label()
        Me.TimerTimeOut = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.btYes)
        Me.Panel1.Controls.Add(Me.btNo)
        Me.Panel1.Location = New System.Drawing.Point(-2, 119)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(380, 52)
        Me.Panel1.TabIndex = 0
        '
        'btYes
        '
        Me.btYes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btYes.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.btYes.Location = New System.Drawing.Point(212, 16)
        Me.btYes.Name = "btYes"
        Me.btYes.Size = New System.Drawing.Size(75, 23)
        Me.btYes.TabIndex = 6
        Me.btYes.Text = "Yes"
        Me.btYes.UseVisualStyleBackColor = True
        '
        'btNo
        '
        Me.btNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btNo.DialogResult = System.Windows.Forms.DialogResult.No
        Me.btNo.Location = New System.Drawing.Point(293, 16)
        Me.btNo.Name = "btNo"
        Me.btNo.Size = New System.Drawing.Size(75, 23)
        Me.btNo.TabIndex = 5
        Me.btNo.Text = "No"
        Me.btNo.UseVisualStyleBackColor = True
        '
        'LabelZman
        '
        Me.LabelZman.AutoEllipsis = True
        Me.LabelZman.Location = New System.Drawing.Point(72, 37)
        Me.LabelZman.Name = "LabelZman"
        Me.LabelZman.Size = New System.Drawing.Size(294, 20)
        Me.LabelZman.TabIndex = 1
        Me.LabelZman.Text = "Zman"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.zman.My.Resources.Resources.alarm_bell_notification_reminder
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(54, 48)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'LabelMessage
        '
        Me.LabelMessage.AutoEllipsis = True
        Me.LabelMessage.Location = New System.Drawing.Point(72, 62)
        Me.LabelMessage.Name = "LabelMessage"
        Me.LabelMessage.Size = New System.Drawing.Size(294, 20)
        Me.LabelMessage.TabIndex = 3
        Me.LabelMessage.Text = "Message"
        '
        'LabelRemindAgain
        '
        Me.LabelRemindAgain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelRemindAgain.AutoEllipsis = True
        Me.LabelRemindAgain.Location = New System.Drawing.Point(72, 87)
        Me.LabelRemindAgain.Name = "LabelRemindAgain"
        Me.LabelRemindAgain.Size = New System.Drawing.Size(294, 20)
        Me.LabelRemindAgain.TabIndex = 4
        Me.LabelRemindAgain.Text = "Remind Again"
        '
        'LabelTime
        '
        Me.LabelTime.AutoEllipsis = True
        Me.LabelTime.Location = New System.Drawing.Point(72, 12)
        Me.LabelTime.Name = "LabelTime"
        Me.LabelTime.Size = New System.Drawing.Size(294, 20)
        Me.LabelTime.TabIndex = 5
        Me.LabelTime.Text = "Time"
        '
        'TimerTimeOut
        '
        Me.TimerTimeOut.Interval = 30000
        '
        'FrmScheduleMessage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.CancelButton = Me.btYes
        Me.ClientSize = New System.Drawing.Size(378, 170)
        Me.Controls.Add(Me.LabelTime)
        Me.Controls.Add(Me.LabelRemindAgain)
        Me.Controls.Add(Me.LabelMessage)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.LabelZman)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmScheduleMessage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MyZman Scheduler Message"
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents btYes As Button
    Friend WithEvents btNo As Button
    Friend WithEvents LabelZman As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents LabelMessage As Label
    Friend WithEvents LabelRemindAgain As Label
    Friend WithEvents LabelTime As Label
    Friend WithEvents TimerTimeOut As Windows.Forms.Timer
End Class
