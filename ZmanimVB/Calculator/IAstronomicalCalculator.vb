
Namespace Zmanim.Calculator

    ''' <summary>
    ''' A interface that defines the sservices needed to calculate sunrise and sunset.
    ''' </summary>
    Public Interface IAstronomicalCalculator
        ''' <summary>
        ''' A descriptive name of the algorithm.
        ''' </summary>
        ReadOnly Property CalculatorName As String

        ''' <summary>
        ''' A method that calculates UTC sunrise as well as any time based on an
        ''' angle above or below sunrise. This abstract method is implemented by the
        ''' classes that extend this class.
        ''' </summary>
        ''' <paramname="astronomicalCalendar">Used to calculate day of year.</param>
        ''' <paramname="zenith">the azimuth below the vertical zenith of 90 degrees. for
        ''' sunrise typically the <seecref="AstronomicalCalculator.AdjustZenith">zenith</see> used for
        ''' the calculation uses geometric zenith of 90°; and
        ''' <seecref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        ''' solar refraction and the sun's radius. Another example would
        ''' be <seecref="AstronomicalCalendar.GetBeginNauticalTwilight"/>
        ''' that passes <seecref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to
        ''' this method.</param>
        ''' <paramname="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        ''' <returns>
        ''' The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        ''' 5.75.0. If an error was encountered in the calculation (expected
        ''' behavior for some locations such as near the poles,
        ''' <seecref="Double.NaN"/> will be returned.
        ''' </returns>
        Function GetUtcSunrise(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean) As Double

        ''' <summary>
        ''' A method that calculates UTC sunset as well as any time based on an angle
        ''' above or below sunset. This abstract method is implemented by the classes
        ''' that extend this class.
        ''' </summary>
        ''' <paramname="astronomicalCalendar">Used to calculate day of year.</param>
        ''' <paramname="zenith">the azimuth below the vertical zenith of 90°;. For sunset
        ''' typically the <seecref="AstronomicalCalculator.AdjustZenith">zenith</see> used for the
        ''' calculation uses geometric zenith of 90°; and
        ''' <seecref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        ''' solar refraction and the sun's radius. Another example would
        ''' be <seecref="AstronomicalCalendar.GetEndNauticalTwilight"/> that
        ''' passes <seecref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to this
        ''' method.</param>
        ''' <paramname="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        ''' <returns>
        ''' The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        ''' 5.75.0. If an error was encountered in the calculation (expected
        ''' behavior for some locations such as near the poles,
        ''' <seealsocref="Double.NaN"/> will be returned.
        ''' </returns>
        Function GetUtcSunset(ByVal dateWithLocation As IDateWithLocation, ByVal zenith As Double, ByVal adjustForElevation As Boolean) As Double
    End Interface
End Namespace
