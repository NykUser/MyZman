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


Namespace Zmanim.Utilities
    ''' <summary>
    ''' Wrapper class for an astronomical time, mostly used to sort collections of
    ''' astronomical times.
    ''' </summary>
    ''' <author>Eliyahu Hershfeld</author>
    Public Class Zman
        ''' <summary>
        ''' Initializes a new instance of the <seecref="Zman"/> class.
        ''' </summary>
        ''' <paramname="date">The date.</param>
        ''' <paramname="label">The label.</param>
        Public Sub New(ByVal [date] As Date, ByVal label As String)
            ZmanLabel = label
            ZmanTime = [date]
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <seecref="Zman"/> class.
        ''' </summary>
        ''' <paramname="duration">The duration.</param>
        ''' <paramname="label">The label.</param>
        Public Sub New(ByVal duration As Long, ByVal label As String)
            ZmanLabel = label
            Me.Duration = duration
        End Sub

        ''' <summary>
        ''' Gets the duration.
        ''' </summary>
        ''' <value></value>
        Public Overridable Property Duration As Long

        ''' <summary>
        ''' Gets the zman.
        ''' </summary>
        ''' <value></value>
        Public Overridable Property ZmanTime As Date

        ''' <summary>
        ''' Gets the zman label.
        ''' </summary>
        ''' <value></value>
        Public Overridable Property ZmanLabel As String
    End Class
End Namespace
