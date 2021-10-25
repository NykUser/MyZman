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
    ''' Math helpers.
    ''' </summary>
    Public Module MathExtensions
        ''' <summary>
        ''' Convert degree angle to radians.
        ''' </summary>
        ''' <paramname="angleDegree">The angle degree.</param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToRadians(ByVal angleDegree As Double) As Double
            Return Math.PI * angleDegree / 180.0
        End Function

        ''' <summary>
        ''' Convert radian angle to degrees.
        ''' </summary>
        ''' <paramname="angleRadians">The angle radians.</param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToDegree(ByVal angleRadians As Double) As Double
            Return 180.0 * angleRadians / Math.PI
        End Function
    End Module
End Namespace
