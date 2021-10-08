<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmDifference
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDifference))
        Me.btAddTimeZoneId = New System.Windows.Forms.Button()
        Me.btSystemInfo = New System.Windows.Forms.Button()
        Me.make_dat_file = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CbTimeA = New System.Windows.Forms.ComboBox()
        Me.CbTimeB = New System.Windows.Forms.ComboBox()
        Me.tbTimeA = New System.Windows.Forms.TextBox()
        Me.tbTimeB = New System.Windows.Forms.TextBox()
        Me.btSunOffset = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Butexit = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.tbTimeR = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btAddTimeZoneId
        '
        Me.btAddTimeZoneId.Enabled = False
        Me.btAddTimeZoneId.Location = New System.Drawing.Point(361, 72)
        Me.btAddTimeZoneId.Name = "btAddTimeZoneId"
        Me.btAddTimeZoneId.Size = New System.Drawing.Size(75, 21)
        Me.btAddTimeZoneId.TabIndex = 74
        Me.btAddTimeZoneId.Text = "ListAddZoneId"
        Me.btAddTimeZoneId.UseVisualStyleBackColor = True
        '
        'btSystemInfo
        '
        Me.btSystemInfo.Location = New System.Drawing.Point(361, 19)
        Me.btSystemInfo.Name = "btSystemInfo"
        Me.btSystemInfo.Size = New System.Drawing.Size(75, 21)
        Me.btSystemInfo.TabIndex = 67
        Me.btSystemInfo.Text = "System Info"
        Me.btSystemInfo.UseVisualStyleBackColor = True
        '
        'make_dat_file
        '
        Me.make_dat_file.Location = New System.Drawing.Point(361, 45)
        Me.make_dat_file.Name = "make_dat_file"
        Me.make_dat_file.Size = New System.Drawing.Size(75, 21)
        Me.make_dat_file.TabIndex = 66
        Me.make_dat_file.Text = "make_dat_file"
        Me.make_dat_file.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(280, 48)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(75, 20)
        Me.TextBox1.TabIndex = 64
        '
        'CbTimeA
        '
        Me.CbTimeA.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbTimeA.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbTimeA.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.CbTimeA.DropDownHeight = 400
        Me.CbTimeA.DropDownWidth = 220
        Me.CbTimeA.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.CbTimeA.FormattingEnabled = True
        Me.CbTimeA.IntegralHeight = False
        Me.CbTimeA.Location = New System.Drawing.Point(6, 24)
        Me.CbTimeA.MaxDropDownItems = 5
        Me.CbTimeA.Name = "CbTimeA"
        Me.CbTimeA.Size = New System.Drawing.Size(224, 23)
        Me.CbTimeA.TabIndex = 68
        '
        'CbTimeB
        '
        Me.CbTimeB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbTimeB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbTimeB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.CbTimeB.DropDownHeight = 400
        Me.CbTimeB.DropDownWidth = 220
        Me.CbTimeB.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.CbTimeB.FormattingEnabled = True
        Me.CbTimeB.IntegralHeight = False
        Me.CbTimeB.Location = New System.Drawing.Point(6, 23)
        Me.CbTimeB.MaxDropDownItems = 5
        Me.CbTimeB.Name = "CbTimeB"
        Me.CbTimeB.Size = New System.Drawing.Size(224, 23)
        Me.CbTimeB.TabIndex = 69
        '
        'tbTimeA
        '
        Me.tbTimeA.Location = New System.Drawing.Point(6, 54)
        Me.tbTimeA.Name = "tbTimeA"
        Me.tbTimeA.Size = New System.Drawing.Size(224, 24)
        Me.tbTimeA.TabIndex = 70
        '
        'tbTimeB
        '
        Me.tbTimeB.Location = New System.Drawing.Point(6, 53)
        Me.tbTimeB.Name = "tbTimeB"
        Me.tbTimeB.Size = New System.Drawing.Size(224, 24)
        Me.tbTimeB.TabIndex = 71
        '
        'btSunOffset
        '
        Me.btSunOffset.Location = New System.Drawing.Point(280, 19)
        Me.btSunOffset.Name = "btSunOffset"
        Me.btSunOffset.Size = New System.Drawing.Size(75, 21)
        Me.btSunOffset.TabIndex = 63
        Me.btSunOffset.Text = "מעלת"
        Me.btSunOffset.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tbTimeA)
        Me.GroupBox1.Controls.Add(Me.CbTimeA)
        Me.GroupBox1.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.DodgerBlue
        Me.GroupBox1.Location = New System.Drawing.Point(9, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(236, 90)
        Me.GroupBox1.TabIndex = 75
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Zman 1"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tbTimeB)
        Me.GroupBox2.Controls.Add(Me.CbTimeB)
        Me.GroupBox2.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.DodgerBlue
        Me.GroupBox2.Location = New System.Drawing.Point(9, 106)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(236, 90)
        Me.GroupBox2.TabIndex = 76
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Zman 2"
        '
        'Butexit
        '
        Me.Butexit.BackColor = System.Drawing.SystemColors.Info
        Me.Butexit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Butexit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info
        Me.Butexit.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Info
        Me.Butexit.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Info
        Me.Butexit.Location = New System.Drawing.Point(246, -2)
        Me.Butexit.Name = "Butexit"
        Me.Butexit.Size = New System.Drawing.Size(1, 1)
        Me.Butexit.TabIndex = 78
        Me.Butexit.Text = "&exit"
        Me.Butexit.UseVisualStyleBackColor = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.tbTimeR)
        Me.GroupBox3.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.Color.DodgerBlue
        Me.GroupBox3.Location = New System.Drawing.Point(9, 202)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(236, 61)
        Me.GroupBox3.TabIndex = 78
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Difference"
        '
        'tbTimeR
        '
        Me.tbTimeR.Location = New System.Drawing.Point(6, 24)
        Me.tbTimeR.Name = "tbTimeR"
        Me.tbTimeR.Size = New System.Drawing.Size(224, 24)
        Me.tbTimeR.TabIndex = 71
        '
        'FrmDifference
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AliceBlue
        Me.CancelButton = Me.Butexit
        Me.ClientSize = New System.Drawing.Size(255, 276)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Butexit)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btAddTimeZoneId)
        Me.Controls.Add(Me.btSystemInfo)
        Me.Controls.Add(Me.make_dat_file)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btSunOffset)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(500, 315)
        Me.MinimumSize = New System.Drawing.Size(271, 315)
        Me.Name = "FrmDifference"
        Me.Text = "Compare Zmanim"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btAddTimeZoneId As Button
    Friend WithEvents btSystemInfo As Button
    Friend WithEvents make_dat_file As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents CbTimeA As ComboBox
    Friend WithEvents CbTimeB As ComboBox
    Friend WithEvents tbTimeA As TextBox
    Friend WithEvents tbTimeB As TextBox
    Friend WithEvents btSunOffset As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Butexit As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents tbTimeR As TextBox
End Class
