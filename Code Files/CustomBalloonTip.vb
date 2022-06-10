'based on this https://stackoverflow.com/a/30054010/2392157
'see also https://www.codeproject.com/Articles/10459/BalloonTip-Show

Imports System.Collections.Generic
Imports System
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class CustomBalloonTip
    Private timer As New System.Timers.Timer()
    Private semaphore As New System.Threading.SemaphoreSlim(1)
    Private hWnd As IntPtr
    Public Sub New(text As String, control As Control, xPosition As Double, yPosition As Double)
        Show("", text, control, xPosition, yPosition)
    End Sub

    Public Sub New(title As String, text As String, control As Control, xPosition As Double, yPosition As Double, Optional RightToLeft As Boolean = False, Optional icon As ICON = 0, Optional timeOut As Double = 0, Optional focus As Boolean = False)
        Show(title, text, control, xPosition, yPosition, RightToLeft, icon, timeOut, focus)
    End Sub

    Public Sub Show(title As String, text As String, control As Control, xPosition As Double, yPosition As Double, Optional RightToLeft As Boolean = False, Optional icon As ICON = 0, Optional timeout As Double = 0, Optional focus As Boolean = False)

        'my change
        'Dim x As UShort = CType(Control.RectangleToScreen(Control.ClientRectangle).Left + Control.Width / 2, UShort)
        'Dim y As UShort = CType(Control.RectangleToScreen(Control.ClientRectangle).Top + Control.Height / 2, UShort)
        Dim x As UShort = CType(xPosition, Short)
        Dim y As UShort = CType(yPosition, Short)

        Dim toolInfo As New TOOLINFO()
        toolInfo.cbSize = CType(Marshal.SizeOf(toolInfo), UInteger)

        'my change
        'toolInfo.uFlags = &H20 ' TTF_TRACK ' not CENTER
        Const TTF_IDISHWND As Integer = &H1
        Const TTF_SUBCLASS As Integer = &H10
        Const TTF_TRACK As Integer = &H20
        Const TTF_ABSOLUTE As Integer = &H80
        Const TTF_TRANSPARENT As Integer = &H100
        Const TTF_CENTERTIP As Integer = &H2
        Const TTF_PARSELINKS As Integer = &H1000
        Const TTF_RTLREADING As Integer = &H4

        'toolInfo.uFlags = TTF_IDISHWND + TTF_CENTERTIP + TTF_TRACK + TTF_ABSOLUTE
        toolInfo.uFlags = TTF_IDISHWND + TTF_TRACK + TTF_ABSOLUTE + TTF_RTLREADING

        toolInfo.lpszText = text
        Dim pToolInfo As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(toolInfo))
        Marshal.StructureToPtr(toolInfo, pToolInfo, False)

        'my change
        'Dim buffer As Byte() = Encoding.UTF8.GetBytes(title)

        Dim buffer As Byte() = Encoding.GetEncoding("windows-1255").GetBytes(title)
        buffer = buffer.Concat(New Byte() {0}).ToArray()
        Dim pszTitle As IntPtr = Marshal.AllocCoTaskMem(buffer.Length)
        Marshal.Copy(buffer, 0, pszTitle, buffer.Length)

        'my change
        'hWnd = User32.CreateWindowEx(&H8, "tooltips_class32", "", &HC3, 0, 0, 0, 0, control.Parent.Handle, CType(0, IntPtr), CType(0, IntPtr), CType(0, IntPtr))
        If RightToLeft = False Then
            hWnd = User32.CreateWindowEx(&H8, "tooltips_class32", "", &H40, 0, 0, 0, 0, control.Parent.Handle, CType(0, IntPtr), CType(0, IntPtr), CType(0, IntPtr)) '&H40 will remove the x ' sending 0 will make it not balloon
        Else
            'this will make RTL 'WS_EX_LAYOUTRTL = 0x00400000L
            hWnd = User32.CreateWindowEx(&H400000L, "tooltips_class32", "", &H40, 0, 0, 0, 0, control.Parent.Handle, CType(0, IntPtr), CType(0, IntPtr), CType(0, IntPtr))
        End If

        User32.SendMessage(hWnd, 1028, CType(0, IntPtr), pToolInfo) ' TTM_ADDTOOL
        'User32.SendMessage(hWnd, 1043, CType(0, IntPtr), CType(0, IntPtr); ' TTM_SETTIPBKCOLOR
        'User32.SendMessage(hWnd, 1044, CType(&HFFFF, IntPtr), CType(0, IntPtr); ' TTM_SETTIPTEXTCOLOR
        User32.SendMessage(hWnd, 1056, CType(icon, IntPtr), pszTitle)
        ' TTM_SETTITLE 0:None, 1:Info, 2:Warning, 3:Error, >3:assumed to be an hIcon. ; 1057 for Unicode
        User32.SendMessage(hWnd, 1048, CType(0, IntPtr), CType(500, IntPtr))
        ' TTM_SETMAXTIPWIDTH
        User32.SendMessage(hWnd, &H40C, CType(0, IntPtr), pToolInfo)
        ' TTM_UPDATETIPTEXT; 0x439 for Unicode
        User32.SendMessage(hWnd, 1042, CType(0, IntPtr), CType(x Or (CUInt(y) << 16), IntPtr))
        ' TTM_TRACKPOSITION
        User32.SendMessage(hWnd, 1041, CType(1, IntPtr), pToolInfo)
        ' TTM_TRACKACTIVATE
        Marshal.FreeCoTaskMem(pszTitle)
        Marshal.DestroyStructure(pToolInfo, GetType(TOOLINFO))
        Marshal.FreeCoTaskMem(pToolInfo)
        If focus Then
            control.Focus()
        End If

        ' uncomment below to make balloon close when user changes focus,
        ' starts typing, resizes/moves parent window, minimizes parent window, etc
        ' adjust which control events to subscribe to depending on the control over which the balloon tip is shown
        'AddHandler control.Click, AddressOf control_Event
        'AddHandler control.Leave, AddressOf control_Event
        'AddHandler control.TextChanged, AddressOf control_Event
        'AddHandler control.LocationChanged, AddressOf control_Event
        'AddHandler control.SizeChanged, AddressOf control_Event
        'AddHandler control.VisibleChanged, AddressOf control_Event
        'Dim parent As Control = control.Parent
        'While Not (parent Is Nothing)
        '    AddHandler parent.VisibleChanged, AddressOf control_Event
        '    parent = parent.Parent
        'End While
        'AddHandler control.TopLevelControl.LocationChanged, AddressOf control_Event
        'AddHandler DirectCast(control.TopLevelControl, Form).Deactivate, AddressOf control_Event

        'my change
        If control.GetType.BaseType.Name = "ComboBox" OrElse control.GetType.Name = "ComboBox" Then
            AddHandler control.MouseMove, AddressOf control_Event
            AddHandler control.TextChanged, AddressOf control_Event
            Dim comboBox1 As ComboBox = CType(control, ComboBox)
            AddHandler comboBox1.DropDownClosed, AddressOf control_Event
        End If


        timer.AutoReset = False
        RemoveHandler timer.Elapsed, AddressOf timer_Elapsed
        If timeout > 0 Then
            timer.Interval = timeout
            timer.Start()
        End If
    End Sub

    Private Sub timer_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
        Close()
    End Sub

    Private Sub control_Event(sender As Object, e As EventArgs)
        Close()
    End Sub

    Sub Close()
        If Not semaphore.Wait(0) Then
            ' ensures one time only execution
            Return
        End If
        RemoveHandler timer.Elapsed, AddressOf timer_Elapsed
        timer.Close()
        User32.SendMessage(hWnd, &H10, CType(0, IntPtr), CType(0, IntPtr))
        ' WM_CLOSE
        'User32.SendMessage(hWnd, &H0002, CType(0, IntPtr), CType(0, IntPtr)); ' WM_DESTROY
        'User32.SendMessage(hWnd, &H0082, CType(0, IntPtr), CType(0, IntPtr)); ' WM_NCDESTROY
    End Sub

    <StructLayout(LayoutKind.Sequential)>
    Private Structure TOOLINFO
        Public cbSize As UInteger
        Public uFlags As UInteger
        Public hwnd As IntPtr
        Public uId As IntPtr
        Public rect As RECT
        Public hinst As IntPtr
        <MarshalAs(UnmanagedType.LPStr)>
        Public lpszText As String
        Public lParam As IntPtr
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Public Enum ICON
        NONE
        INFO
        WARNING
        [ERROR]
    End Enum
End Class

NotInheritable Class User32
        Private Sub New()
        End Sub
        <DllImportAttribute("user32.dll")>
        Public Shared Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As IntPtr, lParam As IntPtr) As Integer
        End Function
        <DllImportAttribute("user32.dll")>
        Public Shared Function CreateWindowEx(dwExStyle As UInteger, lpClassName As String, lpWindowName As String, dwStyle As UInteger, x As Integer, y As Integer,
            nWidth As Integer, nHeight As Integer, hWndParent As IntPtr, hMenu As IntPtr, hInstance As IntPtr, LPVOIDlpParam As IntPtr) As IntPtr
        End Function
    End Class
