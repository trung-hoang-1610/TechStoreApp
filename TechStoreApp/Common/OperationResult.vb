''' <summary>
''' Kết quả trả về của một thao tác, có thể chứa dữ liệu và lỗi
''' </summary>
Public Class OperationResult
    ''' <summary>
    ''' Trạng thái thành công hay không
    ''' </summary>
    Public Property Success As Boolean

    ''' <summary>
    ''' Danh sách lỗi nếu có
    ''' </summary>
    Public Property Errors As List(Of String)

    ''' <summary>
    ''' Dữ liệu trả về (có thể là Nothing)
    ''' </summary>
    Public Property Data As Object

    ''' <summary>
    ''' Khởi tạo mặc định: thất bại, danh sách lỗi rỗng
    ''' </summary>
    Public Sub New()
        Success = False
        Errors = New List(Of String)()
        Data = Nothing
    End Sub

    ''' <summary>
    ''' Khởi tạo với trạng thái và danh sách lỗi (Data là Nothing)
    ''' </summary>
    Public Sub New(success As Boolean, errors As List(Of String))
        Me.Success = success
        Me.Errors = errors
        Me.Data = Nothing
    End Sub

    ''' <summary>
    ''' Khởi tạo với trạng thái, dữ liệu và danh sách lỗi
    ''' </summary>
    Public Sub New(success As Boolean, data As Object, errors As List(Of String))
        Me.Success = success
        Me.Data = data
        Me.Errors = errors
    End Sub
End Class
