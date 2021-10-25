' * Zmanim .NET API
' * Copyright (C) 2004-2011 Eliyahu Hershfeld
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

Namespace Zmanim.JewishCalendar
    ''' <summary>
    ''' An Object representing a Daf in the Daf Yomi cycle.
    ''' 
    ''' @author &copy; Eliyahu Hershfeld 2011
    ''' @version 0.0.1
    ''' </summary>
    Public Class Daf
        Private Shared masechtosBavliTransliterated As String() = {"Berachos", "Shabbos", "Eruvin", "Pesachim", "Shekalim", "Yoma", "Sukkah", "Beitzah", "Rosh Hashana", "Taanis", "Megillah", "Moed Katan", "Chagigah", "Yevamos", "Kesubos", "Nedarim", "Nazir", "Sotah", "Gitin", "Kiddushin", "Bava Kamma", "Bava Metzia", "Bava Basra", "Sanhedrin", "Makkos", "Shevuos", "Avodah Zarah", "Horiyos", "Zevachim", "Menachos", "Chullin", "Bechoros", "Arachin", "Temurah", "Kerisos", "Meilah", "Kinnim", "Tamid", "Midos", "Niddah"}
        Private Shared masechtosBavli As String() = {"ברכות", "שבת", "עירובין", "פסחים", "שקלים", "יומא", "סוכה", "ביצה", "ראש השנה", "תענית", "מגילה", "מועד קטן", "חגיגה", "יבמות", "כתובות", "נדרים", "נזיר", "סוטה", "גיטין", "קידושין", "בבא קמא", "בבא מציעא", "בבא בתרא", "סנהדרין", "מכות", "שבועות", "עבודה זרה", "הוריות", "זבחים", "מנחות", "חולין", "בכורות", "ערכין", "תמורה", "כריתות", "מעילה", "תמיד", "קינים", "מידות", "נדה"}

        ''' <summary>
        ''' Constructor that creates a Daf setting the <seealsocref=""/> and
        ''' </summary>
        ''' <paramname="masechtaNumber"> </param>
        ''' <paramname="page"> </param>
        ''' <paramname="hasSecondaryMesechta"></param>
        Public Sub New(ByVal masechtaNumber As Integer, ByVal page As Integer, ByVal Optional hasSecondaryMesechta As Boolean = False)
            Me.MasechtaNumber = masechtaNumber
            Me.Page = page
            Me.HasSecondaryMesechta = hasSecondaryMesechta
        End Sub

        ''' <returns> the masechtaNumber </returns>
        Public Overridable ReadOnly Property MasechtaNumber As Integer

        ''' <summary>
        ''' Returns the daf (page number) of the Daf Yomi </summary>
        ''' <returns> the daf (page number) of the Daf Yomi </returns>
        Public Overridable Property Page As Integer
        Public ReadOnly Property HasSecondaryMesechta As Boolean

        Public ReadOnly Property SecondaryMesechtaNumber As Integer
            Get
                Return If(HasSecondaryMesechta, MasechtaNumber + 1, 0)
            End Get
        End Property

        ''' <summary>
        ''' Returns the transliterated name of the masechta (tractate) of the Daf Yomi. The list of mashechtos is: Berachos,
        ''' Shabbos, Eruvin, Pesachim, Shekalim, Yoma, Sukkah, Beitzah, Rosh Hashana, Taanis, Megillah, Moed Katan, Chagigah,
        ''' Yevamos, Kesubos, Nedarim, Nazir, Sotah, Gitin, Kiddushin, Bava Kamma, Bava Metzia, Bava Basra, Sanhedrin,
        ''' Makkos, Shevuos, Avodah Zarah, Horiyos, Zevachim, Menachos, Chullin, Bechoros, Arachin, Temurah, Kerisos, Meilah,
        ''' Kinnim, Tamid, Midos and Niddah.
        ''' </summary>
        ''' <returns> the transliterated name of the masechta (tractate) of the Daf Yomi such as Berachos. </returns>
        Public Overridable ReadOnly Property MasechtaTransliterated As String
            Get
                Return GetMesechtaName(masechtosBavliTransliterated, MasechtaNumber)
            End Get
        End Property

        ''' <summary>
        ''' Returns the transliterated name of the second masechta (tractate) of the Daf Yomi.
        ''' Kinnim, Tamid.
        ''' </summary>
        ''' <returns> the transliterated name of the second masechta (tractate) of the Daf Yomi
        ''' when on the first Daf of Kinnim or Tamid. </returns>
        Public Overridable ReadOnly Property SecondaryMasechtaTransliterated As String
            Get
                Return GetMesechtaName(masechtosBavliTransliterated, SecondaryMesechtaNumber)
            End Get
        End Property

        ''' <summary>
        ''' Returns the masechta (tractate) of the Daf Yomi in Hebrew, It will return
        ''' &#x05D1;&#x05E8;&#x05DB;&#x05D5;&#x05EA; for Berachos.
        ''' </summary>
        ''' <returns> the masechta (tractate) of the Daf Yomi in Hebrew, It will return
        '''         &#x05D1;&#x05E8;&#x05DB;&#x05D5;&#x05EA; for Berachos. </returns>
        Public Overridable ReadOnly Property Masechta As String
            Get
                Return GetMesechtaName(masechtosBavli, MasechtaNumber)
            End Get
        End Property

        ''' <summary>
        ''' Returns the secondary masechta (tractate) of the Daf Yomi in Hebrew, It will only return
        ''' For Kinnim and Tamid when on the first Daf \u05E7\u05D9\u05E0\u05D9\u05DD for Tamid.
        ''' </summary>
        ''' <returns> the secondary masechta (tractate) of the Daf Yomi in Hebrew, It will only return
        ''' For Kinnim and Tamid when on the first Daf \u05E7\u05D9\u05E0\u05D9\u05DD for Tamid.</returns>
        Public Overridable ReadOnly Property SecondaryMasechta As String
            Get
                Return GetMesechtaName(masechtosBavli, SecondaryMesechtaNumber)
            End Get
        End Property

        Private Shared Function GetMesechtaName(ByVal mesechtaNames As String(), ByVal masechtaNumber As Integer) As String
            If masechtaNumber < 0 Then Return ""
            Return mesechtaNames(masechtaNumber)
        End Function
    End Class
End Namespace
