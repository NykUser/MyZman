'not in use - issue getting RTL for Heb properly rendered
'based on \Projects\Samples\EasyToolStrip_CbBox_VB and https://vbdotnetforums.com/threads/toolstrip-textbox-labels.43349/
'with edits for size, Property's and events 


'adding to form via designer will make trouble do via code 
'here it is done in ProjectsGlobalVariables
'Public varTransparencyLabel As New ToolStripMenuTextBoxAndLabel("Transparency: ", 40)
'and Frminfo_Load
'SettingsToolStripMenuItem.DropDownItems.Add(varTransparencyLabel)
'events are belwo


Public Class ToolStripMenuTextBoxAndLabel
    Inherits ToolStripControlHost

    Public Sub New(Optional ByVal lblText As String = "label", Optional ByVal txtWidth As Integer = 30, Optional RTL As Integer = 0)
        MyBase.New(New ControlPanel(lblText, txtWidth, RTL))

    End Sub

    Public ReadOnly Property ControlPanelControl() As ControlPanel
        Get
            Return CType(Me.Control, ControlPanel)
        End Get
    End Property

End Class


Public Class ControlPanel
    Inherits Panel

    Public WithEvents txt As New TextBox
    Friend WithEvents lbl As New Label

    Public Sub New(ByVal lblText As String, txtWidth As Integer, RTL As Integer)

        Me.Height = 15
        Me.RightToLeft = RTL

        lbl.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Bottom
        lbl.Text = lblText
        lbl.TextAlign = ContentAlignment.BottomLeft
        lbl.AutoSize = True
        lbl.Height = Me.Height
        lbl.Location = New Point(0, 3)
        lbl.Parent = Me

        'get the label size
        Dim graphics = (New System.Windows.Forms.Label()).CreateGraphics()
        Dim CurrentSize As Integer = graphics.MeasureString(lblText, lbl.Font).Width

        txt.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        txt.Location = New Point(CurrentSize + 10, 0) '(lbl.Right + 10, 0)
        txt.Width = txtWidth 'Me.Width - txt.Left
        txt.Height = 15
        txt.Parent = Me

    End Sub
    Public Overrides Property Text() As String
        Get
            Return txt.Text
        End Get
        Set(ByVal value As String)
            txt.Text = value
        End Set
    End Property
    Private Sub tblatitude_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt.KeyPress
        If e.KeyChar = Chr(13) Then Frminfo.ToolStripDropDownButton1.DropDown.Close()
        'SettingsToolStripMenuItem.DropDownClosed will so the change
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "%" Then e.KeyChar = ""
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class