Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        'Public mutex As System.Threading.Mutex
        Private Sub AppStart(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblies
            MemoryFonts.AddMemoryFont(My.Resources.ArialUnicodeCompact)
            MemoryFonts.AddMemoryFont(My.Resources.VarelaRound_Regular)

            'switch for Scheduler  
            Dim args As String() = e.CommandLine.ToArray ' Environment.GetCommandLineArgs()
            If args.Count() > 0 Then
                If args(0) = "/s" Or args(0) = "/S" Then FrmSchedule.RunSchedulecheck(True)
                'close this instance as this was called from Scheduler and ther is no active Instance the user is using
                Environment.Exit(0)
            End If
        End Sub
        Private Sub MyApplication_StartupNextInstance(sender As Object, e As StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            'AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblies

            'dont bring ToForeground when Schedule is run

            e.BringToForeground = False
            'switch for Scheduler  - this is for when Myzman is already running
            Dim args As String() = e.CommandLine.ToArray ' Environment.GetCommandLineArgs()
            If args.Count() > 0 Then
                If args(0) = "/s" Or args(0) = "/S" Then FrmSchedule.RunSchedulecheck(False)
                'don't close let the first Instance continue
            End If


            'this uses Mutex And will work on none Single instance application
            'Dim prevInstance As Boolean
            'mutex = New System.Threading.Mutex(True, "Application Name", prevInstance)
            'If (prevInstance = False) Then
            '    If args.Count() > 0 Then
            '        If args(0) = "/s" Or args(0) = "/S" Then FrmSchedule.RunSchedulecheck(False)
            '    End If
            '    'MessageBox.Show("There is another instance running")
            '    Environment.Exit(0)
            'End If

        End Sub
        'for DLL's
        'Add the desired assembly to the project's resources.
        'Build Action = None
        'Copy to Output Directory = do not copy
        Private Function ResolveAssemblies(ByVal sender As Object, ByVal e As System.ResolveEventArgs) As Reflection.Assembly
            Dim desiredAssembly = New Reflection.AssemblyName(e.Name)

            '  MsgBox(desiredAssembly.Name)

            If desiredAssembly.Name = "Zmanim" Then
                Return Reflection.Assembly.Load(My.Resources.Zmanim)
            ElseIf desiredAssembly.Name = "GeoTimeZone" Then
                Return Reflection.Assembly.Load(My.Resources.GeoTimeZone)
            ElseIf desiredAssembly.Name = "TimeZoneConverter" Then
                Return Reflection.Assembly.Load(My.Resources.TimeZoneConverter)
            ElseIf desiredAssembly.Name = "Microsoft.WindowsAPICodePack.Shell" Then
                Return Reflection.Assembly.Load(My.Resources.Microsoft_WindowsAPICodePack_Shell)
            ElseIf desiredAssembly.Name = "Microsoft.WindowsAPICodePack" Then
                Return Reflection.Assembly.Load(My.Resources.Microsoft_WindowsAPICodePack)
            Else
                Return Nothing
            End If

        End Function
    End Class


End Namespace

