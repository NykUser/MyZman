'from https://stackoverflow.com/questions/15904610/show-a-messagebox-centered-in-form
'with some edits
'PositionTypes - added in
'ParentCenter
'MouseBelow
'MouseCenter

Imports System.Text
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Class Centered_MessageBox
    Implements IDisposable
    Private mTries As Integer = 0
    Private mOwner As Form
    Private mPositionType As String


    Public Sub New(owner As Form, Optional PositionType As String = "MouseCenter")
        mPositionType = PositionType
        mOwner = owner
        owner.BeginInvoke(New MethodInvoker(AddressOf findDialog))
    End Sub

    Private Sub findDialog()
        ' Enumerate windows to find the message box
        If mTries < 0 Then
            Return
        End If
        Dim callback As New EnumThreadWndProc(AddressOf checkWindow)
        If EnumThreadWindows(GetCurrentThreadId(), callback, IntPtr.Zero) Then
            If System.Threading.Interlocked.Increment(mTries) < 10 Then
                mOwner.BeginInvoke(New MethodInvoker(AddressOf findDialog))
            End If
        End If
    End Sub
    Private Function checkWindow(hWnd As IntPtr, lp As IntPtr) As Boolean
        ' Checks if <hWnd> is a dialog
        Dim sb As New StringBuilder(260)
        GetClassName(hWnd, sb, sb.Capacity)
        If sb.ToString() <> "#32770" Then
            Return True
        End If
        ' Got it
        Dim frmRect As New Rectangle(mOwner.Location, mOwner.Size)
        Dim dlgRect As RECT
        GetWindowRect(hWnd, dlgRect)

        '----'edits here
        'first was the original
        If mPositionType = "ParentCenter" Then MoveWindow(hWnd, frmRect.Left + (frmRect.Width - dlgRect.Right + dlgRect.Left) \ 2, frmRect.Top + (frmRect.Height - dlgRect.Bottom + dlgRect.Top) \ 2, dlgRect.Right - dlgRect.Left, dlgRect.Bottom - dlgRect.Top, True)
        If mPositionType = "MouseBelow" Then MoveWindow(hWnd, Control.MousePosition.X, Control.MousePosition.Y, dlgRect.Right - dlgRect.Left, dlgRect.Bottom - dlgRect.Top, True)
        If mPositionType = "MouseCenter" Then MoveWindow(hWnd, Control.MousePosition.X - (dlgRect.Right - dlgRect.Left) \ 2, Control.MousePosition.Y - (dlgRect.Bottom - dlgRect.Top) \ 2, dlgRect.Right - dlgRect.Left, dlgRect.Bottom - dlgRect.Top, True)
        Return False
    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
        mTries = -1
    End Sub

    ' P/Invoke declarations
    Private Delegate Function EnumThreadWndProc(hWnd As IntPtr, lp As IntPtr) As Boolean
    <DllImport("user32.dll")>
    Private Shared Function EnumThreadWindows(tid As Integer, callback As EnumThreadWndProc, lp As IntPtr) As Boolean
    End Function
    <DllImport("kernel32.dll")>
    Private Shared Function GetCurrentThreadId() As Integer
    End Function
    <DllImport("user32.dll")>
    Private Shared Function GetClassName(hWnd As IntPtr, buffer As StringBuilder, buflen As Integer) As Integer
    End Function
    <DllImport("user32.dll")>
    Private Shared Function GetWindowRect(hWnd As IntPtr, ByRef rc As RECT) As Boolean
    End Function
    <DllImport("user32.dll")>
    Private Shared Function MoveWindow(hWnd As IntPtr, x As Integer, y As Integer, w As Integer, h As Integer, repaint As Boolean) As Boolean
    End Function
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure
End Class