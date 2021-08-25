<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frminfo
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frminfo))
        Me.LocationContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mGetCurrnetLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mSaveLocationChanges = New System.Windows.Forms.ToolStripMenuItem()
        Me.mRemoveLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mAddToUserLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mResetLocationList = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mPlaceListInHebrew = New System.Windows.Forms.ToolStripMenuItem()
        Me.mDefaultsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.mDefaultLastUsed = New System.Windows.Forms.ToolStripMenuItem()
        Me.mDefaultInUse = New System.Windows.Forms.ToolStripMenuItem()
        Me.mDefaultChangeNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.Butexit = New System.Windows.Forms.Button()
        Me.cbLocationList = New System.Windows.Forms.ComboBox()
        Me.tblatitude = New System.Windows.Forms.TextBox()
        Me.tblongitude = New System.Windows.Forms.TextBox()
        Me.tbzone = New System.Windows.Forms.TextBox()
        Me.TimerStatusLabel = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.rtbHebrewDate = New System.Windows.Forms.RichTextBox()
        Me.tbElevation = New System.Windows.Forms.TextBox()
        Me.tbcountry = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mHebrewMenus = New System.Windows.Forms.ToolStripMenuItem()
        Me.mIsraeliYomTov = New System.Windows.Forms.ToolStripMenuItem()
        Me.m24HourFormatTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.mShowTimesOnStatusBar = New System.Windows.Forms.ToolStripMenuItem()
        Me.mAskWhenChanging = New System.Windows.Forms.ToolStripMenuItem()
        Me.mBackUpWhenChanging = New System.Windows.Forms.ToolStripMenuItem()
        Me.mStayOnTopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mChangeKeybordLayout = New System.Windows.Forms.ToolStripMenuItem()
        Me.mShowTooltips = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mResetSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mOpenCompare = New System.Windows.Forms.ToolStripMenuItem()
        Me.mOpenSchedule = New System.Windows.Forms.ToolStripMenuItem()
        Me.mInfoHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.TimerLocationsLoad = New System.Windows.Forms.Timer(Me.components)
        Me.LabelOffSet = New System.Windows.Forms.Label()
        Me.CbTimeZone = New System.Windows.Forms.ComboBox()
        Me.LabelLatitude = New System.Windows.Forms.Label()
        Me.LabelLongitude = New System.Windows.Forms.Label()
        Me.LabelElevation = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LabelTimeZone = New System.Windows.Forms.Label()
        Me.LabelCountry = New System.Windows.Forms.Label()
        Me.ZmanimContextMenuHelper = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ZmanimContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mAddZman = New System.Windows.Forms.ToolStripMenuItem()
        Me.mDeleteZman = New System.Windows.Forms.ToolStripMenuItem()
        Me.mResetZmanimList = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mColorZmanMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mCalculateElevation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mUseUSNO = New System.Windows.Forms.ToolStripMenuItem()
        Me.mLineBetweenZmanim = New System.Windows.Forms.ToolStripMenuItem()
        Me.dpEngdate = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TimerTransparency = New System.Windows.Forms.Timer(Me.components)
        Me.TimerZmanimAfterChange = New System.Windows.Forms.Timer(Me.components)
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ColumnNum = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnZman = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnTime = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.LabelDisclaimer = New System.Windows.Forms.Label()
        Me.rbtLocationContexOpen = New zman.WindowsFormsApplication1.RoundButton()
        Me.rbtTodayRefresh = New zman.WindowsFormsApplication1.RoundButton()
        Me.LocationContextMenu.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ZmanimContextMenu.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LocationContextMenu
        '
        Me.LocationContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mGetCurrnetLocation, Me.mSaveLocationChanges, Me.mRemoveLocation, Me.mAddToUserLocation, Me.mResetLocationList, Me.ToolStripSeparator1, Me.mPlaceListInHebrew, Me.mDefaultsMenu})
        Me.LocationContextMenu.Name = "ContextMenuStrip1"
        Me.LocationContextMenu.Size = New System.Drawing.Size(207, 164)
        '
        'mGetCurrnetLocation
        '
        Me.mGetCurrnetLocation.Image = Global.zman.My.Resources.Resources.Location2__16
        Me.mGetCurrnetLocation.Name = "mGetCurrnetLocation"
        Me.mGetCurrnetLocation.Size = New System.Drawing.Size(206, 22)
        Me.mGetCurrnetLocation.Text = "Get Current Location"
        '
        'mSaveLocationChanges
        '
        Me.mSaveLocationChanges.Image = Global.zman.My.Resources.Resources.DownloadFolder_16x
        Me.mSaveLocationChanges.Name = "mSaveLocationChanges"
        Me.mSaveLocationChanges.Size = New System.Drawing.Size(206, 22)
        Me.mSaveLocationChanges.Text = "Save Location Changes"
        '
        'mRemoveLocation
        '
        Me.mRemoveLocation.Image = Global.zman.My.Resources.Resources.DeleteFolder_16x
        Me.mRemoveLocation.Name = "mRemoveLocation"
        Me.mRemoveLocation.Size = New System.Drawing.Size(206, 22)
        Me.mRemoveLocation.Text = "Remove Location"
        '
        'mAddToUserLocation
        '
        Me.mAddToUserLocation.Image = Global.zman.My.Resources.Resources.PlusFolder
        Me.mAddToUserLocation.Name = "mAddToUserLocation"
        Me.mAddToUserLocation.Size = New System.Drawing.Size(206, 22)
        Me.mAddToUserLocation.Text = "Add As New Location"
        '
        'mResetLocationList
        '
        Me.mResetLocationList.Image = Global.zman.My.Resources.Resources.ContinuousFolder_16x
        Me.mResetLocationList.Name = "mResetLocationList"
        Me.mResetLocationList.Size = New System.Drawing.Size(206, 22)
        Me.mResetLocationList.Text = "Reset Location List"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(203, 6)
        '
        'mPlaceListInHebrew
        '
        Me.mPlaceListInHebrew.CheckOnClick = True
        Me.mPlaceListInHebrew.Name = "mPlaceListInHebrew"
        Me.mPlaceListInHebrew.Size = New System.Drawing.Size(206, 22)
        Me.mPlaceListInHebrew.Text = "Sort Locations In Hebrew"
        '
        'mDefaultsMenu
        '
        Me.mDefaultsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mDefaultLastUsed, Me.mDefaultInUse, Me.mDefaultChangeNew})
        Me.mDefaultsMenu.Name = "mDefaultsMenu"
        Me.mDefaultsMenu.Size = New System.Drawing.Size(206, 22)
        Me.mDefaultsMenu.Text = "Default Location"
        '
        'mDefaultLastUsed
        '
        Me.mDefaultLastUsed.Name = "mDefaultLastUsed"
        Me.mDefaultLastUsed.Size = New System.Drawing.Size(124, 22)
        Me.mDefaultLastUsed.Text = "Last Used"
        '
        'mDefaultInUse
        '
        Me.mDefaultInUse.Name = "mDefaultInUse"
        Me.mDefaultInUse.Size = New System.Drawing.Size(124, 22)
        '
        'mDefaultChangeNew
        '
        Me.mDefaultChangeNew.Name = "mDefaultChangeNew"
        Me.mDefaultChangeNew.Size = New System.Drawing.Size(124, 22)
        '
        'Butexit
        '
        Me.Butexit.BackColor = System.Drawing.SystemColors.Info
        Me.Butexit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Butexit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info
        Me.Butexit.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Info
        Me.Butexit.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Info
        Me.Butexit.Location = New System.Drawing.Point(93, 538)
        Me.Butexit.Name = "Butexit"
        Me.Butexit.Size = New System.Drawing.Size(10, 10)
        Me.Butexit.TabIndex = 8
        Me.Butexit.Text = "&exit"
        Me.Butexit.UseVisualStyleBackColor = False
        '
        'cbLocationList
        '
        Me.cbLocationList.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbLocationList.AutoCompleteCustomSource.AddRange(New String() {"Squre", "york"})
        Me.cbLocationList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbLocationList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbLocationList.DropDownWidth = 231
        Me.cbLocationList.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.cbLocationList.FormattingEnabled = True
        Me.cbLocationList.Location = New System.Drawing.Point(9, 24)
        Me.cbLocationList.Name = "cbLocationList"
        Me.cbLocationList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbLocationList.Size = New System.Drawing.Size(231, 24)
        Me.cbLocationList.TabIndex = 1
        '
        'tblatitude
        '
        Me.tblatitude.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tblatitude.Location = New System.Drawing.Point(10, 70)
        Me.tblatitude.Name = "tblatitude"
        Me.tblatitude.Size = New System.Drawing.Size(57, 24)
        Me.tblatitude.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.tblatitude, "Enter Latitude" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "הזן קו רוחב")
        '
        'tblongitude
        '
        Me.tblongitude.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tblongitude.Location = New System.Drawing.Point(73, 70)
        Me.tblongitude.Name = "tblongitude"
        Me.tblongitude.Size = New System.Drawing.Size(57, 24)
        Me.tblongitude.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.tblongitude, "Enter Longitude" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "הזן קו אורך")
        '
        'tbzone
        '
        Me.tbzone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tbzone.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbzone.Location = New System.Drawing.Point(199, 115)
        Me.tbzone.Name = "tbzone"
        Me.tbzone.Size = New System.Drawing.Size(57, 24)
        Me.tbzone.TabIndex = 7
        '
        'TimerStatusLabel
        '
        Me.TimerStatusLabel.Enabled = True
        Me.TimerStatusLabel.Interval = 250
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 100
        Me.ToolTip1.AutoPopDelay = 50000
        Me.ToolTip1.InitialDelay = 100
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 20
        Me.ToolTip1.ShowAlways = True
        '
        'rtbHebrewDate
        '
        Me.rtbHebrewDate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.rtbHebrewDate.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.rtbHebrewDate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rtbHebrewDate.Location = New System.Drawing.Point(8, 55)
        Me.rtbHebrewDate.Name = "rtbHebrewDate"
        Me.rtbHebrewDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.rtbHebrewDate.Size = New System.Drawing.Size(246, 45)
        Me.rtbHebrewDate.TabIndex = 2
        Me.rtbHebrewDate.Text = ""
        '
        'tbElevation
        '
        Me.tbElevation.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tbElevation.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbElevation.Location = New System.Drawing.Point(136, 70)
        Me.tbElevation.Name = "tbElevation"
        Me.tbElevation.Size = New System.Drawing.Size(57, 24)
        Me.tbElevation.TabIndex = 4
        '
        'tbcountry
        '
        Me.tbcountry.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tbcountry.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tbcountry.Location = New System.Drawing.Point(199, 70)
        Me.tbcountry.Name = "tbcountry"
        Me.tbcountry.Size = New System.Drawing.Size(57, 24)
        Me.tbcountry.TabIndex = 5
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton1, Me.StatusLabel, Me.ToolStripProgressBar1})
        Me.StatusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 848)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(289, 21)
        Me.StatusStrip1.TabIndex = 27
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem, Me.mExport, Me.mOpenCompare, Me.mOpenSchedule, Me.mInfoHelp})
        Me.ToolStripDropDownButton1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(48, 19)
        Me.ToolStripDropDownButton1.Text = "Tools"
        Me.ToolStripDropDownButton1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mHebrewMenus, Me.mIsraeliYomTov, Me.m24HourFormatTime, Me.mShowTimesOnStatusBar, Me.mAskWhenChanging, Me.mBackUpWhenChanging, Me.mStayOnTopToolStripMenuItem, Me.mChangeKeybordLayout, Me.mShowTooltips, Me.ToolStripSeparator3, Me.mResetSettings})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'mHebrewMenus
        '
        Me.mHebrewMenus.CheckOnClick = True
        Me.mHebrewMenus.Name = "mHebrewMenus"
        Me.mHebrewMenus.Size = New System.Drawing.Size(254, 22)
        Me.mHebrewMenus.Text = "Hebrew Menus"
        '
        'mIsraeliYomTov
        '
        Me.mIsraeliYomTov.CheckOnClick = True
        Me.mIsraeliYomTov.Name = "mIsraeliYomTov"
        Me.mIsraeliYomTov.Size = New System.Drawing.Size(254, 22)
        Me.mIsraeliYomTov.Text = "Eretz Yisrael Yom Tov and Parsha"
        '
        'm24HourFormatTime
        '
        Me.m24HourFormatTime.CheckOnClick = True
        Me.m24HourFormatTime.Name = "m24HourFormatTime"
        Me.m24HourFormatTime.Size = New System.Drawing.Size(254, 22)
        Me.m24HourFormatTime.Text = "24 Hour Format Time"
        '
        'mShowTimesOnStatusBar
        '
        Me.mShowTimesOnStatusBar.CheckOnClick = True
        Me.mShowTimesOnStatusBar.Name = "mShowTimesOnStatusBar"
        Me.mShowTimesOnStatusBar.Size = New System.Drawing.Size(254, 22)
        Me.mShowTimesOnStatusBar.Text = "Show Time On Status Bar"
        '
        'mAskWhenChanging
        '
        Me.mAskWhenChanging.CheckOnClick = True
        Me.mAskWhenChanging.Name = "mAskWhenChanging"
        Me.mAskWhenChanging.Size = New System.Drawing.Size(254, 22)
        Me.mAskWhenChanging.Text = "Ask on Changes"
        '
        'mBackUpWhenChanging
        '
        Me.mBackUpWhenChanging.CheckOnClick = True
        Me.mBackUpWhenChanging.Name = "mBackUpWhenChanging"
        Me.mBackUpWhenChanging.Size = New System.Drawing.Size(254, 22)
        Me.mBackUpWhenChanging.Text = "Back Up When Changing"
        '
        'mStayOnTopToolStripMenuItem
        '
        Me.mStayOnTopToolStripMenuItem.CheckOnClick = True
        Me.mStayOnTopToolStripMenuItem.Name = "mStayOnTopToolStripMenuItem"
        Me.mStayOnTopToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.mStayOnTopToolStripMenuItem.Text = "Stay On Top"
        '
        'mChangeKeybordLayout
        '
        Me.mChangeKeybordLayout.CheckOnClick = True
        Me.mChangeKeybordLayout.Name = "mChangeKeybordLayout"
        Me.mChangeKeybordLayout.Size = New System.Drawing.Size(254, 22)
        Me.mChangeKeybordLayout.Text = "Change Keyboard to Hebrew"
        '
        'mShowTooltips
        '
        Me.mShowTooltips.CheckOnClick = True
        Me.mShowTooltips.Name = "mShowTooltips"
        Me.mShowTooltips.Size = New System.Drawing.Size(254, 22)
        Me.mShowTooltips.Text = "Display Tool Tips"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(251, 6)
        '
        'mResetSettings
        '
        Me.mResetSettings.Image = Global.zman.My.Resources.Resources.refresh_2
        Me.mResetSettings.Name = "mResetSettings"
        Me.mResetSettings.Size = New System.Drawing.Size(254, 22)
        Me.mResetSettings.Text = "Reset All Settings"
        '
        'mExport
        '
        Me.mExport.Image = Global.zman.My.Resources.Resources.export_more_icon
        Me.mExport.Name = "mExport"
        Me.mExport.Size = New System.Drawing.Size(149, 22)
        Me.mExport.Text = "Export"
        '
        'mOpenCompare
        '
        Me.mOpenCompare.Image = Global.zman.My.Resources.Resources.CompareFiles_16x
        Me.mOpenCompare.Name = "mOpenCompare"
        Me.mOpenCompare.Size = New System.Drawing.Size(149, 22)
        Me.mOpenCompare.Text = "Compare"
        '
        'mOpenSchedule
        '
        Me.mOpenSchedule.Image = Global.zman.My.Resources.Resources.BlueAlarmBell
        Me.mOpenSchedule.Name = "mOpenSchedule"
        Me.mOpenSchedule.Size = New System.Drawing.Size(149, 22)
        Me.mOpenSchedule.Text = "Scheduler"
        '
        'mInfoHelp
        '
        Me.mInfoHelp.Image = Global.zman.My.Resources.Resources.Info_16x
        Me.mInfoHelp.Name = "mInfoHelp"
        Me.mInfoHelp.Size = New System.Drawing.Size(149, 22)
        Me.mInfoHelp.Text = "Info and Help"
        '
        'StatusLabel
        '
        Me.StatusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(0, 0)
        Me.StatusLabel.Spring = True
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Enabled = False
        Me.ToolStripProgressBar1.Maximum = 1200
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        Me.ToolStripProgressBar1.Visible = False
        '
        'TimerLocationsLoad
        '
        Me.TimerLocationsLoad.Interval = 250
        '
        'LabelOffSet
        '
        Me.LabelOffSet.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelOffSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelOffSet.ForeColor = System.Drawing.Color.Black
        Me.LabelOffSet.Location = New System.Drawing.Point(196, 99)
        Me.LabelOffSet.Name = "LabelOffSet"
        Me.LabelOffSet.Size = New System.Drawing.Size(60, 16)
        Me.LabelOffSet.TabIndex = 43
        Me.LabelOffSet.Text = "OffSet"
        '
        'CbTimeZone
        '
        Me.CbTimeZone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CbTimeZone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbTimeZone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbTimeZone.DropDownWidth = 245
        Me.CbTimeZone.FormattingEnabled = True
        Me.CbTimeZone.Location = New System.Drawing.Point(10, 115)
        Me.CbTimeZone.Name = "CbTimeZone"
        Me.CbTimeZone.Size = New System.Drawing.Size(183, 24)
        Me.CbTimeZone.TabIndex = 6
        '
        'LabelLatitude
        '
        Me.LabelLatitude.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelLatitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelLatitude.ForeColor = System.Drawing.Color.Black
        Me.LabelLatitude.Location = New System.Drawing.Point(7, 54)
        Me.LabelLatitude.Name = "LabelLatitude"
        Me.LabelLatitude.Size = New System.Drawing.Size(60, 16)
        Me.LabelLatitude.TabIndex = 47
        Me.LabelLatitude.Text = "Latitude"
        '
        'LabelLongitude
        '
        Me.LabelLongitude.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelLongitude.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelLongitude.ForeColor = System.Drawing.Color.Black
        Me.LabelLongitude.Location = New System.Drawing.Point(70, 54)
        Me.LabelLongitude.Name = "LabelLongitude"
        Me.LabelLongitude.Size = New System.Drawing.Size(60, 16)
        Me.LabelLongitude.TabIndex = 48
        Me.LabelLongitude.Text = "Longitude"
        '
        'LabelElevation
        '
        Me.LabelElevation.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelElevation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelElevation.ForeColor = System.Drawing.Color.Black
        Me.LabelElevation.Location = New System.Drawing.Point(133, 54)
        Me.LabelElevation.Name = "LabelElevation"
        Me.LabelElevation.Size = New System.Drawing.Size(60, 16)
        Me.LabelElevation.TabIndex = 50
        Me.LabelElevation.Text = "Elevation"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.GroupBox2.Controls.Add(Me.rbtLocationContexOpen)
        Me.GroupBox2.Controls.Add(Me.LabelTimeZone)
        Me.GroupBox2.Controls.Add(Me.LabelCountry)
        Me.GroupBox2.Controls.Add(Me.tbcountry)
        Me.GroupBox2.Controls.Add(Me.cbLocationList)
        Me.GroupBox2.Controls.Add(Me.tblatitude)
        Me.GroupBox2.Controls.Add(Me.LabelElevation)
        Me.GroupBox2.Controls.Add(Me.LabelOffSet)
        Me.GroupBox2.Controls.Add(Me.tblongitude)
        Me.GroupBox2.Controls.Add(Me.tbElevation)
        Me.GroupBox2.Controls.Add(Me.LabelLongitude)
        Me.GroupBox2.Controls.Add(Me.CbTimeZone)
        Me.GroupBox2.Controls.Add(Me.tbzone)
        Me.GroupBox2.Controls.Add(Me.LabelLatitude)
        Me.GroupBox2.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.DarkRed
        Me.GroupBox2.Location = New System.Drawing.Point(11, 129)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(266, 153)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Location info"
        '
        'LabelTimeZone
        '
        Me.LabelTimeZone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelTimeZone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelTimeZone.ForeColor = System.Drawing.Color.Black
        Me.LabelTimeZone.Location = New System.Drawing.Point(7, 99)
        Me.LabelTimeZone.Name = "LabelTimeZone"
        Me.LabelTimeZone.Size = New System.Drawing.Size(186, 16)
        Me.LabelTimeZone.TabIndex = 62
        Me.LabelTimeZone.Text = "Time Zone"
        '
        'LabelCountry
        '
        Me.LabelCountry.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelCountry.ForeColor = System.Drawing.Color.Black
        Me.LabelCountry.Location = New System.Drawing.Point(196, 54)
        Me.LabelCountry.Name = "LabelCountry"
        Me.LabelCountry.Size = New System.Drawing.Size(60, 16)
        Me.LabelCountry.TabIndex = 58
        Me.LabelCountry.Text = "Country"
        '
        'ZmanimContextMenuHelper
        '
        Me.ZmanimContextMenuHelper.Name = "ZmanimContextMenuHelper"
        Me.ZmanimContextMenuHelper.Size = New System.Drawing.Size(61, 4)
        '
        'ZmanimContextMenu
        '
        Me.ZmanimContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mAddZman, Me.mDeleteZman, Me.mResetZmanimList, Me.ToolStripSeparator2, Me.mColorZmanMenuItem, Me.mCalculateElevation, Me.mUseUSNO, Me.mLineBetweenZmanim})
        Me.ZmanimContextMenu.Name = "ZmanimContextMenu"
        Me.ZmanimContextMenu.Size = New System.Drawing.Size(218, 164)
        '
        'mAddZman
        '
        Me.mAddZman.Image = Global.zman.My.Resources.Resources.Plus
        Me.mAddZman.Name = "mAddZman"
        Me.mAddZman.Size = New System.Drawing.Size(217, 22)
        Me.mAddZman.Text = "Add New Zman Below"
        '
        'mDeleteZman
        '
        Me.mDeleteZman.Image = Global.zman.My.Resources.Resources.Remove_thin_10x_16x
        Me.mDeleteZman.Name = "mDeleteZman"
        Me.mDeleteZman.Size = New System.Drawing.Size(217, 22)
        Me.mDeleteZman.Text = "Delete This Zman"
        '
        'mResetZmanimList
        '
        Me.mResetZmanimList.Image = Global.zman.My.Resources.Resources.refresh_2
        Me.mResetZmanimList.Name = "mResetZmanimList"
        Me.mResetZmanimList.Size = New System.Drawing.Size(217, 22)
        Me.mResetZmanimList.Text = "Reset Zmanim List"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(214, 6)
        '
        'mColorZmanMenuItem
        '
        Me.mColorZmanMenuItem.CheckOnClick = True
        Me.mColorZmanMenuItem.Name = "mColorZmanMenuItem"
        Me.mColorZmanMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.mColorZmanMenuItem.Text = "Color for Zmanim"
        '
        'mCalculateElevation
        '
        Me.mCalculateElevation.CheckOnClick = True
        Me.mCalculateElevation.Name = "mCalculateElevation"
        Me.mCalculateElevation.Size = New System.Drawing.Size(217, 22)
        Me.mCalculateElevation.Text = "Calculate Elevation"
        '
        'mUseUSNO
        '
        Me.mUseUSNO.CheckOnClick = True
        Me.mUseUSNO.Name = "mUseUSNO"
        Me.mUseUSNO.Size = New System.Drawing.Size(217, 22)
        Me.mUseUSNO.Text = "Use Older USNO Algorithm"
        '
        'mLineBetweenZmanim
        '
        Me.mLineBetweenZmanim.CheckOnClick = True
        Me.mLineBetweenZmanim.Name = "mLineBetweenZmanim"
        Me.mLineBetweenZmanim.Size = New System.Drawing.Size(217, 22)
        Me.mLineBetweenZmanim.Text = "Line Between Zmanim"
        '
        'dpEngdate
        '
        Me.dpEngdate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.dpEngdate.CalendarFont = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dpEngdate.CustomFormat = "dddd MM / dd / yyyy"
        Me.dpEngdate.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dpEngdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dpEngdate.Location = New System.Drawing.Point(8, 24)
        Me.dpEngdate.MaxDate = New Date(2239, 9, 29, 0, 0, 0, 0)
        Me.dpEngdate.Name = "dpEngdate"
        Me.dpEngdate.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.dpEngdate.Size = New System.Drawing.Size(222, 25)
        Me.dpEngdate.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.GroupBox1.Controls.Add(Me.rbtTodayRefresh)
        Me.GroupBox1.Controls.Add(Me.dpEngdate)
        Me.GroupBox1.Controls.Add(Me.rtbHebrewDate)
        Me.GroupBox1.Font = New System.Drawing.Font("David", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.DarkRed
        Me.GroupBox1.Location = New System.Drawing.Point(11, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(266, 114)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dates"
        '
        'TimerTransparency
        '
        Me.TimerTransparency.Enabled = True
        '
        'TimerZmanimAfterChange
        '
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowDrop = True
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnNum, Me.ColumnZman, Me.ColumnTime})
        Me.DataGridView1.ContextMenuStrip = Me.ZmanimContextMenuHelper
        Me.DataGridView1.Location = New System.Drawing.Point(11, 310)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.DataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.DataGridView1.RowHeadersVisible = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.DataGridView1.RowTemplate.Height = 20
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(266, 531)
        Me.DataGridView1.TabIndex = 3
        '
        'ColumnNum
        '
        Me.ColumnNum.HeaderText = "#"
        Me.ColumnNum.Name = "ColumnNum"
        Me.ColumnNum.ReadOnly = True
        Me.ColumnNum.Visible = False
        Me.ColumnNum.Width = 30
        '
        'ColumnZman
        '
        Me.ColumnZman.HeaderText = "זמן"
        Me.ColumnZman.Name = "ColumnZman"
        Me.ColumnZman.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ColumnZman.Width = 160
        '
        'ColumnTime
        '
        Me.ColumnTime.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.ColumnTime.DropDownWidth = 260
        Me.ColumnTime.HeaderText = "בשעה"
        Me.ColumnTime.Name = "ColumnTime"
        Me.ColumnTime.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'LabelDisclaimer
        '
        Me.LabelDisclaimer.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LabelDisclaimer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.LabelDisclaimer.ForeColor = System.Drawing.Color.DarkRed
        Me.LabelDisclaimer.Location = New System.Drawing.Point(11, 283)
        Me.LabelDisclaimer.Name = "LabelDisclaimer"
        Me.LabelDisclaimer.Size = New System.Drawing.Size(266, 22)
        Me.LabelDisclaimer.TabIndex = 28
        Me.LabelDisclaimer.Text = "Don't"
        Me.LabelDisclaimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbtLocationContexOpen
        '
        Me.rbtLocationContexOpen.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.rbtLocationContexOpen.FlatAppearance.BorderSize = 0
        Me.rbtLocationContexOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.rbtLocationContexOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.rbtLocationContexOpen.Image = Global.zman.My.Resources.Resources.three_dots_icon_gray
        Me.rbtLocationContexOpen.Location = New System.Drawing.Point(241, 22)
        Me.rbtLocationContexOpen.Name = "rbtLocationContexOpen"
        Me.rbtLocationContexOpen.Size = New System.Drawing.Size(17, 28)
        Me.rbtLocationContexOpen.TabIndex = 8
        Me.rbtLocationContexOpen.UseVisualStyleBackColor = True
        '
        'rbtTodayRefresh
        '
        Me.rbtTodayRefresh.FlatAppearance.BorderSize = 0
        Me.rbtTodayRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.rbtTodayRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.rbtTodayRefresh.Image = Global.zman.My.Resources.Resources.Undo_16x
        Me.rbtTodayRefresh.Location = New System.Drawing.Point(230, 23)
        Me.rbtTodayRefresh.Name = "rbtTodayRefresh"
        Me.rbtTodayRefresh.Size = New System.Drawing.Size(26, 23)
        Me.rbtTodayRefresh.TabIndex = 3
        Me.rbtTodayRefresh.UseVisualStyleBackColor = True
        '
        'Frminfo
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.CancelButton = Me.Butexit
        Me.ClientSize = New System.Drawing.Size(289, 869)
        Me.Controls.Add(Me.LabelDisclaimer)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Butexit)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(485, 1400)
        Me.MinimumSize = New System.Drawing.Size(305, 70)
        Me.Name = "Frminfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "זמנים"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LocationContextMenu.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ZmanimContextMenu.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Butexit As System.Windows.Forms.Button
    Friend WithEvents cbLocationList As System.Windows.Forms.ComboBox
    Friend WithEvents tblatitude As System.Windows.Forms.TextBox
    Friend WithEvents tblongitude As System.Windows.Forms.TextBox
    Friend WithEvents tbzone As System.Windows.Forms.TextBox
    Friend WithEvents TimerStatusLabel As System.Windows.Forms.Timer
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TimerLocationsLoad As System.Windows.Forms.Timer
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents mOpenSchedule As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mStayOnTopToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelOffSet As Label
    Friend WithEvents CbTimeZone As ComboBox
    Friend WithEvents LabelLatitude As Label
    Friend WithEvents LabelLongitude As Label
    Friend WithEvents tbElevation As TextBox
    Friend WithEvents LabelElevation As Label
    Friend WithEvents LocationContextMenu As ContextMenuStrip
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents LabelCountry As Label
    Friend WithEvents tbcountry As TextBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents mAddToUserLocation As ToolStripMenuItem
    Friend WithEvents mRemoveLocation As ToolStripMenuItem
    Friend WithEvents mDefaultsMenu As ToolStripMenuItem
    Friend WithEvents mDefaultLastUsed As ToolStripMenuItem
    Friend WithEvents mDefaultInUse As ToolStripMenuItem
    Friend WithEvents mDefaultChangeNew As ToolStripMenuItem
    Friend WithEvents ZmanimContextMenu As ContextMenuStrip
    Friend WithEvents ZmanimContextMenuHelper As ContextMenuStrip
    Friend WithEvents mChangeKeybordLayout As ToolStripMenuItem
    Friend WithEvents mAskWhenChanging As ToolStripMenuItem
    Friend WithEvents mSaveLocationChanges As ToolStripMenuItem
    Friend WithEvents mColorZmanMenuItem As ToolStripMenuItem
    Friend WithEvents mCalculateElevation As ToolStripMenuItem
    Friend WithEvents mUseUSNO As ToolStripMenuItem
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents mShowTimesOnStatusBar As ToolStripMenuItem
    Friend WithEvents mExport As ToolStripMenuItem
    Friend WithEvents mLineBetweenZmanim As ToolStripMenuItem
    Friend WithEvents rtbHebrewDate As RichTextBox
    Friend WithEvents dpEngdate As DateTimePicker
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TimerTransparency As Windows.Forms.Timer
    Friend WithEvents mPlaceListInHebrew As ToolStripMenuItem
    Friend WithEvents mResetLocationList As ToolStripMenuItem
    Friend WithEvents m24HourFormatTime As ToolStripMenuItem
    Friend WithEvents mIsraeliYomTov As ToolStripMenuItem
    Friend WithEvents mBackUpWhenChanging As ToolStripMenuItem
    Friend WithEvents mGetCurrnetLocation As ToolStripMenuItem
    Friend WithEvents TimerZmanimAfterChange As Windows.Forms.Timer
    Friend WithEvents mAddZman As ToolStripMenuItem
    Friend WithEvents mDeleteZman As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents mResetZmanimList As ToolStripMenuItem
    Friend WithEvents mOpenCompare As ToolStripMenuItem
    Friend WithEvents mHebrewMenus As ToolStripMenuItem
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents LabelTimeZone As Label
    Friend WithEvents rbtLocationContexOpen As WindowsFormsApplication1.RoundButton
    Friend WithEvents rbtTodayRefresh As WindowsFormsApplication1.RoundButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents mResetSettings As ToolStripMenuItem
    Friend WithEvents ColumnNum As DataGridViewTextBoxColumn
    Friend WithEvents ColumnZman As DataGridViewTextBoxColumn
    Friend WithEvents ColumnTime As DataGridViewComboBoxColumn
    Friend WithEvents LabelDisclaimer As Label
    Friend WithEvents mInfoHelp As ToolStripMenuItem
    Friend WithEvents mShowTooltips As ToolStripMenuItem
End Class
