Public Class OperationResult
    Public Property Success As Boolean
    Public Property Errors As List(Of String)

    Public Sub New()
        Success = False
        Errors = New List(Of String)()
    End Sub

    Public Sub New(success As Boolean, errors As List(Of String))
        Me.Success = success
        Me.Errors = errors
    End Sub
End Class
