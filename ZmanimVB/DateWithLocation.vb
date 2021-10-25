Imports Zmanim.Utilities
Namespace Zmanim
    ''' <summary>
    ''' A simple implementation of ITimeZoneDateTime.
    ''' </summary>
    Public Class DateWithLocation
        Implements IDateWithLocation
        ''' <summary>
        ''' Initializes a new instance of the <seecref="DateWithLocation"/> class.
        ''' </summary>
        ''' <paramname="date">The date.</param>
        ''' <paramname="location"></param>
        Public Sub New(ByVal [date] As Date, ByVal location As IGeoLocation)
            Me.Date = [date]
            Me.Location = location
        End Sub

        ''' <summary>
        ''' Gets or sets the date.
        ''' </summary>
        ''' <value>The date.</value>
        Public Property [Date] As Date Implements IDateWithLocation.Date

        ''' <summary>
        ''' Gets or sets the location.
        ''' </summary>
        ''' <value>The location.</value>
        Public Property Location As IGeoLocation Implements IDateWithLocation.Location
    End Class
End Namespace
