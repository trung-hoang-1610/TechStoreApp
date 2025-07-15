''' <summary>
''' DTO dùng để nhập liệu khi tạo giao dịch nhập/xuất kho.
''' </summary>
Public Class InventoryTransactionInputDto

    ''' <summary>
    ''' Mã sản phẩm liên quan đến giao dịch
    ''' </summary>
    Public Property ProductId As Integer

    ''' <summary>
    ''' Số lượng nhập hoặc xuất
    ''' </summary>
    Public Property Quantity As Integer

    ''' <summary>
    ''' Loại giao dịch: "Import" hoặc "Export"
    ''' </summary>
    Public Property TransactionType As String

    ''' <summary>
    ''' Ghi chú tùy chọn cho giao dịch
    ''' </summary>
    Public Property Note As String

    ''' <summary>
    ''' Mã người thực hiện giao dịch
    ''' </summary>
    Public Property PerformedBy As Integer

End Class
