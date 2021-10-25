' * Zmanim .NET API
' * Copyright (C) 2004-2010 Eliyahu Hershfeld
' *
' * Converted to C# by AdminJew
' *
' * This file is part of Zmanim .NET API.
' *
' * Zmanim .NET API is free software: you can redistribute it and/or modify
' * it under the terms of the GNU Lesser General Public License as published by
' * the Free Software Foundation, either version 3 of the License, or
' * (at your option) any later version.
' *
' * Zmanim .NET API is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU Lesser General Public License for more details.
' *
' * You should have received a copy of the GNU Lesser General Public License
' * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

Imports System

Namespace Zmanim.Utilities
    ''' <summary>
    ''' A class that represents a numeric time. Times that represent a time of day
    ''' are stored as <seealsocref="DateTime"/>s in this API. The time class is used to
    ''' represent numeric time such as the time in hours, minutes, seconds and
    ''' milliseconds of a
    ''' <seecref="AstronomicalCalendar.GetTemporalHour()">temporal hour</see>.
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class Time
        Private Const SECOND_MILLIS As Integer = 1000
        Private Const MINUTE_MILLIS As Integer = SECOND_MILLIS * 60
        Private Const HOUR_MILLIS As Integer = MINUTE_MILLIS * 60

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Time"/> class.
        ''' </summary>
        ''' <paramname="hours">The hours.</param>
        ''' <paramname="minutes">The minutes.</param>
        ''' <paramname="seconds">The seconds.</param>
        ''' <paramname="milliseconds">The milliseconds.</param>
        Public Sub New(ByVal hours As Integer, ByVal minutes As Integer, ByVal seconds As Integer, ByVal milliseconds As Integer)
            Me.Hours = hours
            Me.Minutes = minutes
            Me.Seconds = seconds
            Me.Milliseconds = milliseconds
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Time"/> class.
        ''' </summary>
        ''' <paramname="millis">The millis.</param>
        Public Sub New(ByVal millis As Double)
            Me.New(CInt(millis))
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Time"/> class.
        ''' </summary>
        ''' <paramname="millis">The millis.</param>
        Public Sub New(ByVal millis As Integer)
            Dim timeSpan As TimeSpan = TimeSpan.FromMilliseconds(millis)

            If millis < 0 Then
                IsNegative = True
                millis = Math.Abs(millis)
            End If

            Hours = timeSpan.Hours
            Minutes = timeSpan.Minutes
            Seconds = timeSpan.Seconds
            Milliseconds = timeSpan.Milliseconds
        End Sub

        ''' <summary>
        ''' Determines whether this instance is negative.
        ''' </summary>
        ''' <value>
        '''   &lt;c&gt;true&lt;/c&gt; if this instance is negative; otherwise, &lt;c&gt;false&lt;/c&gt;.
        ''' </value>
        Public Overridable Property IsNegative As Boolean

        ''' <summary>
        ''' Gets the hours.
        ''' </summary>
        ''' <value>Returns the hour.</value>
        Public Overridable Property Hours As Integer

        ''' <summary>
        ''' Gets the minutes.
        ''' </summary>
        ''' <value>Returns the minutes.</value>
        Public Overridable Property Minutes As Integer

        ''' <summary>
        ''' Gets the seconds.
        ''' </summary>
        ''' <value>Returns the seconds.</value>
        Public Overridable Property Seconds As Integer

        ''' <summary>
        ''' Gets the milliseconds.
        ''' </summary>
        ''' <value>Returns the milliseconds.</value>
        Public Overridable Property Milliseconds As Integer

        ''' <summary>
        ''' Gets the time.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetTime() As Double
            Return Hours * HOUR_MILLIS + Minutes * MINUTE_MILLIS + Seconds * SECOND_MILLIS + Milliseconds
        End Function

        ''' <summary>
        ''' Returns a <seecref="System.String"/> that represents this instance.
        ''' </summary>
        ''' <returns>
        ''' A <seecref="System.String"/> that represents this instance.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return New ZmanimFormatter().Format(Me)
        End Function
    End Class
End Namespace
