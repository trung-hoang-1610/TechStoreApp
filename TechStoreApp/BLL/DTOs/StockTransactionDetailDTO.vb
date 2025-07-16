''' <summary>
''' DTO cho chi tiết phiếu nhập/xuất kho.
''' </summary>
Public Class StockTransactionDetailDTO
    ' Trường dùng khi get và add
    Public Property DetailId As Integer ' Mã chi tiết (chỉ dùng khi get)
    Public Property TransactionId As Integer ' Mã phiếu (dùng khi add và get)
    Public Property ProductId As Integer ' Mã sản phẩm
    Public Property ProductName As String ' Tên sản phẩm (chỉ dùng khi get)
    Public Property Unit As String ' Đơn vị tính (chỉ dùng khi get)
    Public Property Quantity As Integer ' Số lượng
    Public Property Note As String ' Ghi chú
End Class