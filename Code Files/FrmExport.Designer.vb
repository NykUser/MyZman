<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmExport))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbDays = New System.Windows.Forms.TextBox()
        Me.tbPer = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btCancel = New System.Windows.Forms.Button()
        Me.btCSV = New System.Windows.Forms.Button()
        Me.btVCS = New System.Windows.Forms.Button()
        Me.dpEngdate = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(15, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Days To Export"
        '
        'tbDays
        '
        Me.tbDays.Location = New System.Drawing.Point(18, 67)
        Me.tbDays.Name = "tbDays"
        Me.tbDays.Size = New System.Drawing.Size(110, 20)
        Me.tbDays.TabIndex = 1
        '
        'tbPer
        '
        Me.tbPer.Location = New System.Drawing.Point(18, 111)
        Me.tbPer.Name = "tbPer"
        Me.tbPer.Size = New System.Drawing.Size(110, 20)
        Me.tbPer.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(15, 95)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Days Per File (vcs)"
        '
        'btCancel
        '
        Me.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btCancel.Location = New System.Drawing.Point(151, 26)
        Me.btCancel.Name = "btCancel"
        Me.btCancel.Size = New System.Drawing.Size(75, 23)
        Me.btCancel.TabIndex = 4
        Me.btCancel.Text = "Cancel"
        Me.btCancel.UseVisualStyleBackColor = True
        '
        'btCSV
        '
        Me.btCSV.Image = Global.zman.My.Resources.Resources.excel
        Me.btCSV.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btCSV.Location = New System.Drawing.Point(151, 64)
        Me.btCSV.Name = "btCSV"
        Me.btCSV.Size = New System.Drawing.Size(75, 23)
        Me.btCSV.TabIndex = 5
        Me.btCSV.Text = "To Excel"
        Me.btCSV.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btCSV.UseVisualStyleBackColor = True
        '
        'btVCS
        '
        Me.btVCS.Image = Global.zman.My.Resources.Resources.Calendar_16x
        Me.btVCS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btVCS.Location = New System.Drawing.Point(151, 108)
        Me.btVCS.Name = "btVCS"
        Me.btVCS.Size = New System.Drawing.Size(75, 23)
        Me.btVCS.TabIndex = 6
        Me.btVCS.Text = "To VCS"
        Me.btVCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btVCS.UseVisualStyleBackColor = True
        '
        'dpEngdate
        '
        Me.dpEngdate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.dpEngdate.CalendarFont = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dpEngdate.CustomFormat = "MM / dd / yyyy"
        Me.dpEngdate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right
        Me.dpEngdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dpEngdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dpEngdate.Location = New System.Drawing.Point(18, 26)
        Me.dpEngdate.MaxDate = New Date(2239, 9, 29, 0, 0, 0, 0)
        Me.dpEngdate.Name = "dpEngdate"
        Me.dpEngdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.dpEngdate.Size = New System.Drawing.Size(110, 20)
        Me.dpEngdate.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(15, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Start Date"
        '
        'FrmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AliceBlue
        Me.CancelButton = Me.btCancel
        Me.ClientSize = New System.Drawing.Size(238, 143)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dpEngdate)
        Me.Controls.Add(Me.btVCS)
        Me.Controls.Add(Me.btCSV)
        Me.Controls.Add(Me.btCancel)
        Me.Controls.Add(Me.tbPer)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbDays)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmExport"
        Me.Text = "Export"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents tbDays As TextBox
    Friend WithEvents tbPer As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btCancel As Button
    Friend WithEvents btCSV As Button
    Friend WithEvents btVCS As Button
    Friend WithEvents dpEngdate As DateTimePicker
    Friend WithEvents Label3 As Label
End Class
