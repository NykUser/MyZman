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
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(15, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Days To Export"
        '
        'tbDays
        '
        Me.tbDays.Location = New System.Drawing.Point(18, 27)
        Me.tbDays.Name = "tbDays"
        Me.tbDays.Size = New System.Drawing.Size(110, 20)
        Me.tbDays.TabIndex = 1
        '
        'tbPer
        '
        Me.tbPer.Location = New System.Drawing.Point(18, 71)
        Me.tbPer.Name = "tbPer"
        Me.tbPer.Size = New System.Drawing.Size(110, 20)
        Me.tbPer.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(15, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Days Per File (vcs)"
        '
        'btCancel
        '
        Me.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btCancel.Location = New System.Drawing.Point(148, 11)
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
        Me.btCSV.Location = New System.Drawing.Point(148, 40)
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
        Me.btVCS.Location = New System.Drawing.Point(148, 69)
        Me.btVCS.Name = "btVCS"
        Me.btVCS.Size = New System.Drawing.Size(75, 23)
        Me.btVCS.TabIndex = 6
        Me.btVCS.Text = "To VCS"
        Me.btVCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btVCS.UseVisualStyleBackColor = True
        '
        'FrmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AliceBlue
        Me.CancelButton = Me.btCancel
        Me.ClientSize = New System.Drawing.Size(238, 104)
        Me.Controls.Add(Me.btVCS)
        Me.Controls.Add(Me.btCSV)
        Me.Controls.Add(Me.btCancel)
        Me.Controls.Add(Me.tbPer)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbDays)
        Me.Controls.Add(Me.Label1)
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
End Class
