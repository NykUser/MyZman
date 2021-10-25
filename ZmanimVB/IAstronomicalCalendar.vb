Imports Zmanim.Calculator
Namespace Zmanim
    ''' <summary>
    ''' A calendar that calculates astronomical time calculations such as
    '''  <seecref="GetSunrise">sunrise</see> and <seecref="GetSunset">sunset</see> times.
    ''' </summary>
    Public Interface IAstronomicalCalendar
        ''' <summary>
        '''  The getSunrise method Returns a <c>DateTime</c> representing the
        '''  sunrise time. The zenith used for the calculation uses
        '''  <seealsocref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> of 90°. This is adjusted
        '''  by the <seealsocref="AstronomicalCalendar.AstronomicalCalculator"/> that adds approximately 50/60 of a
        '''  degree to account for 34 archminutes of refraction and 16 archminutes for
        '''  the sun's radius for a total of
        '''  <seealsocref="Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</seealso>. See
        '''  documentation for the specific implementation of the
        '''  <seealsocref="AstronomicalCalendar.AstronomicalCalculator"/> that you are using.
        ''' </summary>
        ''' <returns> the <c>DateTime</c> representing the exact sunrise time. If
        '''  the calculation can not be computed null will be returned. </returns>
        ''' <seealsocref="Calculator.AstronomicalCalculator.AdjustZenith"/>
        Function GetSunrise() As DateTime?

        ''' <summary>
        '''  The getSunset method Returns a <c>DateTime</c> representing the
        '''  sunset time. The zenith used for the calculation uses
        '''  <seecref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see> of 90°. This is adjusted
        '''  by the <seecref="AstronomicalCalendar.AstronomicalCalculator"/> that adds approximately 50/60 of a
        '''  degree to account for 34 archminutes of refraction and 16 archminutes for
        '''  the sun's radius for a total of
        '''  <seecref="Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</see>. See
        '''  documentation for the specific implementation of the
        '''  <seecref="Calculator.AstronomicalCalculator"/> that you are using. Note: In certain cases
        '''  the calculates sunset will occur before sunrise. This will typically
        '''  happen when a timezone other than the local timezone is used (calculating
        '''  Los Angeles sunset using a GMT timezone for example). In this case the
        '''  sunset date will be incremented to the following date.
        ''' </summary>
        ''' <returns> the <c>DateTime</c> representing the exact sunset time. If
        '''  the calculation can not be computed null will be returned. If the
        '''  time calculation </returns>
        ''' <seealsocref="Calculator.AstronomicalCalculator.AdjustZenith"/>
        Function GetSunset() As DateTime?

        ''' <summary>
        ''' Gets or Sets the current AstronomicalCalculator set.
        ''' </summary>
        ''' <value>Returns the astronimicalCalculator.</value>
        Property AstronomicalCalculator As IAstronomicalCalculator

        ''' <summary>
        ''' Gets or Sets the calender to be used in the calculations.
        ''' </summary>
        ''' <value>The calendar to set.</value>
        Property DateWithLocation As IDateWithLocation
    End Interface
End Namespace