Public Class SerializableData
    ' Save—serialize the object to disk..
    Public Sub Save(ByVal filename As String)
        ' make a temporary filename..
        Dim tempFilename As String
        tempFilename = filename & ".tmp"
        ' does the file exist?
        Dim tempFileInfo As New FileInfo(tempFilename)
        If tempFileInfo.Exists = True Then tempFileInfo.Delete()
        ' open the file..
        Dim stream As New FileStream(tempFilename, FileMode.Create)
        ' save the object..
        Save(stream)
        ' close the file..
        stream.Close()
        ' remove the existing data file and
        ' rename the temp file..
        tempFileInfo.CopyTo(filename, True)
        tempFileInfo.Delete()
    End Sub
    ' Save—actually perform the serialization..
    Public Sub Save(ByVal stream As Stream)
        ' create a serializer..
        Dim serializer As New XmlSerializer(Me.GetType)
        ' save the file..
        serializer.Serialize(stream, Me)
    End Sub
    ' Load—deserialize from disk..
    Public Shared Function Load(ByVal filename As String,
    ByVal newType As Type) As Object
        ' does the file exist?
        Dim fileInfo As New FileInfo(filename)
        If fileInfo.Exists = False Then
            ' create a blank version of the object and return that..
            Return System.Activator.CreateInstance(newType)
        End If
        ' open the file..
        Dim stream As New FileStream(filename, FileMode.Open)
        ' load the object from the stream..
        Dim newObject As Object = Load(stream, newType)
        ' close the stream..
        stream.Close()
        ' return the object..
        Return newObject
    End Function
    ' Load-actually perform the deserialization
    Public Shared Function Load(ByVal stream As Stream,
    ByVal newType As Type) As Object
        ' create a serializer and load the object..
        Dim serializer As New XmlSerializer(newType)
        Dim newObject As Object = serializer.Deserialize(stream)
        ' return the new object..
        Return newObject
    End Function
End Class
