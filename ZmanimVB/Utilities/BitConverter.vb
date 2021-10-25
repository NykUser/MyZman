Namespace Zmanim.Utilities
    ''' <summary>
    ''' System.BitConverter.DoubleToInt64Bits method is not presents in Silverlight 3.
    ''' </summary>
    Friend Class BitConverter
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <paramname="x"></param>
        ''' <returns></returns>
        Public Shared Function DoubleToInt64Bits(ByVal x As Double) As Long
            Dim bytes = System.BitConverter.GetBytes(x)
            Dim value = System.BitConverter.ToInt64(bytes, 0)
            Return value
        End Function
    End Class
End Namespace
