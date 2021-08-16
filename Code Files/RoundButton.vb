'set the RoundButton's FlatStyle property to Flat and BorderSize to 0 under FlatAppearance property

Imports System
Imports System.Collections.Generic
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Linq
Imports System.Text

Namespace WindowsFormsApplication1
    Public Class RoundButton
        Inherits Button

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            Dim grPath As GraphicsPath = New GraphicsPath()
            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height)
            Me.Region = New System.Drawing.Region(grPath)
            MyBase.OnPaint(e)
        End Sub
    End Class
End Namespace