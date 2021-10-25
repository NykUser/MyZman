Imports Zmanim.Utilities

Namespace Zmanim
    ''' <summary>
    ''' The GeoLocation and DateTime.
    ''' </summary>
    Public Interface IDateWithLocation
        ''' <summary>
        ''' Gets or sets the date.
        ''' </summary>
        ''' <value>The date.</value>
        Property [Date] As Date

        ''' <summary>
        ''' Gets or sets the location.
        ''' </summary>
        ''' <value>The location.</value>
        Property Location As IGeoLocation
    End Interface
End Namespace
