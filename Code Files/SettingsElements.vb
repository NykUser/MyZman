﻿Public Class aZman
    Inherits SerializableData
    Public Property DisplayName As String
    Public Property ObjectName As String
    Public Property FunctionName As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal ozman As aZman)
        Me.DisplayName = ozman.DisplayName
        Me.ObjectName = ozman.ObjectName
        Me.FunctionName = ozman.FunctionName
    End Sub

End Class
Public Class aZmanimFunc
    Inherits SerializableData
    Public Property HebName As String
    Public Property HebDescription As String
    Public Property EngName As String
    Public Property EngDescription As String
    Public Property ObjectName As String
    Public Property FunctionName As String
    Public Property Num As Integer

    Public Sub New()

    End Sub

    Public Sub New(ByVal oZmanimFunc As aZmanimFunc)
        Me.HebName = oZmanimFunc.HebName
        Me.HebDescription = oZmanimFunc.HebDescription
        Me.EngName = oZmanimFunc.EngName
        Me.EngDescription = oZmanimFunc.EngDescription
        Me.ObjectName = oZmanimFunc.ObjectName
        Me.FunctionName = oZmanimFunc.FunctionName
        Me.Num = oZmanimFunc.Num
    End Sub

End Class
Public Class aLocation
    Inherits SerializableData
    Public Num As Integer
    Public EngName As String
    Public HebName As String
    Public EngCountry As String
    Public HebCountry As String
    Public Longitude As Double
    Public Latitude As Double
    Public Elevation As Double
    Public TimeOffset As Double
    Public TimeZoneID As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal oplace As aLocation) ' byval oplace as alocation
        Me.Num = oplace.Num
        Me.EngName = oplace.EngName
        Me.HebName = oplace.HebName
        Me.EngCountry = oplace.EngCountry
        Me.HebCountry = oplace.HebCountry
        Me.Longitude = oplace.Longitude
        Me.Latitude = oplace.Latitude
        Me.Elevation = oplace.Elevation
        Me.TimeOffset = oplace.TimeOffset
        Me.TimeZoneID = oplace.TimeZoneID
    End Sub
End Class

Public Class aSchedule
    Inherits SerializableData
    Public Time As String
    Public IsFunc As Boolean
    Public Minutes As String
    Public Message As String
    Public IsActive As Boolean
    Public NotToday As Boolean
    Public Sound As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal oSchedule As aSchedule) ' byval oplace as alocation
        Me.Time = oSchedule.Time
        Me.IsFunc = oSchedule.IsFunc
        Me.Minutes = oSchedule.Minutes
        Me.Message = oSchedule.Message
        Me.IsActive = oSchedule.IsActive
        Me.NotToday = oSchedule.NotToday
        Me.Sound = oSchedule.Sound
    End Sub
End Class