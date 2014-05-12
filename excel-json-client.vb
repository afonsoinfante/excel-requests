Imports System
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


'======= Json class => handles deserialization ============================
Public Class Json
  Public Shared Function Deserialize(json As String) As Object
    Deserialize = ToObject(JToken.Parse(json))
  End Function

  Public Shared Function ToObject(token As Newtonsoft.Json.Linq.JToken) As Object
    If token.Type = JTokenType.Object Then
      Dim dict As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
      Dim prop As Object
      For Each prop In CType(token, JObject).Properties()
        dict.Add(prop.Name, ToObject(prop.Value))
      Next
      ToObject = dict
    ElseIf token.Type = JTokenType.Array Then
      Dim lst As List(Of Object) = New List(Of Object)
      Dim value As Object
      For Each value In token
        lst.Add(ToObject(value))
      Next
      ToObject = lst
    Else
      ToObject = DirectCast(token, JValue).Value
    End If
  End Function
End Class


'======= Cache class => caches our JSON objects ============================
Public Class Cache
  Private _objects As New Dictionary(Of String, Object)


  Public Sub Clear()
    _objects.Clear()
  End Sub


  Public Sub ImportObject(baseUrl As String)
    'Get object from source
    Dim wrGETURL As WebRequest
    wrGETURL = WebRequest.Create(baseUrl)
    Dim dictionary As New Dictionary(Of String, Object)

    Dim objStream As Stream
    objStream = wrGETURL.GetResponse.GetResponseStream()

    Dim objReader As New StreamReader(objStream)
    _objects.Add(baseUrl, Json.Deserialize(objReader.ReadToEnd))
    objReader.Close()
    objStream.Close()
  End Sub


  Public Function GetObject(url As String) As Object
    'Return object from _objects if it exists, otherwise from source
    Dim baseUrl As String = Nothing
    baseUrl = url.Split("#")(0)
    If Not _objects.ContainsKey(baseUrl) Then
      ImportObject(baseUrl)
    End If
    GetObject = _objects.Item(baseUrl)
  End Function


  Public Function GetObjectPartial(url As String) As Object
    Dim baseUrl As String = Nothing, obj As Object
    Dim key As String
    baseUrl = url.Split("#")(0)
    Try
      obj = GetObject(baseUrl)
      If url.Contains("#") Then
        For Each key In url.Split("#")(1).Split("/")
          obj = obj.Item(key)
        Next
      End If
      GetObjectPartial = obj
    Catch ex as Exception
      GetObjectPartial = ex.Message
    End Try
  End Function

End Class


'======= define our Excel helper UDFs ============================
Public Module Helpers

  Public Function DISTINCT(data As Object(,))
    Dim dict As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)
    Dim key As String
    Dim i As Integer = 0
    Dim j As Integer = 0
    For i = 0 To data.GetUpperBound(0)
        key = ""
        For j = 0 To data.GetUpperBound(1)
            key += data(i, j).ToString()
        Next
        If Not dict.ContainsKey(key) Then
            dict.Add(key, i)
        End If
    Next
    Dim out(dict.Count-1, data.GetUpperBound(1)) As Object
    i = 0
    For Each key In dict.Keys
        For j = 0 To data.GetUpperBound(1)
            out(i, j) = data(dict.Item(key), j)
        Next
        i += 1
    Next
    DISTINCT = out
  End Function
End Module





'======= define our Excel json client UDFs ============================
Public Module Objects
  Dim cache As Cache = New Cache()
  
  Public Function VERSION()
    VERSION = "v0.1.0"
  End Function


  Public Function OBJECT_CLEAR()
    cache.Clear()
    OBJECT_CLEAR = True
  End Function

  
  Public Function OBJECT_FIELDS(url As String)
    'List all fields of an object
    Dim obj As Object
    obj = cache.GetObjectPartial(url)
    If TypeOf obj Is Dictionary(Of String, Object) Then
      Dim keys As New List(Of String)(CType(obj, Dictionary(Of String, Object)).Keys)
      Dim data(keys.Count-1, 0) As String, key As String
      Dim i As Integer
      For Each key In keys
        data(i,0) = key
        i = i + 1
      Next
      OBJECT_FIELDS = data
    Else
      OBJECT_FIELDS = "ERROR: not defined"
    End If
  End Function

  Public Function OBJECT_GET(url As String)
    'Return the value of an endpoint
    'If the endpoint is an object return its url instead
    Dim path As String() = Nothing
    Dim obj As Object
    Dim i As Long = 0, j As Long
    Dim row As Object, col As Object, item As Object

    obj = cache.GetObjectPartial(url)
    If TypeOf obj Is List(Of Object) Then
      If TypeOf CType(obj, List(Of Object))(0) Is List(Of Object) Then
        Dim o(,) As Object
        ReDim o(CType(obj, List(Of Object)).Count()-1, CType(obj, List(Of Object))(0).Count()-1)
        For Each row In obj
            j = 0
            For Each col In row
                o(i, j) = col
                j += 1
            Next
            i += 1
        Next
        obj = o
      Else
        Dim o() As Object
        ReDim o(CType(obj, List(Of Object)).Count()-1)
        For Each item In obj
            o(i) = item
            i += 1
        Next
        obj = o
      End If
    ElseIf TypeOf obj Is Dictionary(Of String, Object) Then
      obj = url
    End If
    OBJECT_GET = obj
  End Function
End Module