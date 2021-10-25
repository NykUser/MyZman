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
Imports System.Runtime.CompilerServices

Namespace Zmanim.Extensions
    ''' <summary>
    ''' DateTime extensions.
    ''' </summary>
    Public Module DateExtensions
        Friend ReadOnly UnixEpoch As Date = New DateTime(1970, 1, 1)

        ''' <summary>
        ''' Converts Unix Epoch Milliseconds to DateTime.
        ''' </summary>
        ''' <paramname="unixEpochMilliseconds">Milliseconds from the Unix Epoch.</param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToDateTime(ByVal unixEpochMilliseconds As Long) As Date
            Return UnixEpoch.AddMilliseconds(unixEpochMilliseconds)
        End Function

        ''' <summary>
        ''' Converts a DateTime to Unix Epoch Milliseconds.
        ''' </summary>
        ''' <paramname="dateTime">The date time.</param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToUnixEpochMilliseconds(ByVal dateTime As Date) As Long
            Return (dateTime - UnixEpoch).TotalMilliseconds
        End Function
    End Module
End Namespace
