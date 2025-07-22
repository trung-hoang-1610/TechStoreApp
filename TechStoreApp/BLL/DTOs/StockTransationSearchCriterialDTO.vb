''' <summary>
''' DTO cho tiêu chí tìm kiếm phiếu nhập/xuất.
''' </summary>
Public Class StockTransationSearchCriterialDTO
    Public Property TransactionCode As String ' Mã phiếu (tìm gần đúng)
    Public Property Status As String ' Trạng thái (Pending/Approved/Rejected, có thể null)
    Public Property StartDate As DateTime? ' Từ ngày (có thể null)
    Public Property EndDate As DateTime? ' Đến ngày (có thể null)
    Public Property SupplierId As Integer? ' Mã nhà cung cấp (chỉ cho phiếu nhập, có thể null)
    Public Property PageIndex As Integer
    Public Property PageSize As Integer
    Public Property TotalCount As Integer
End Class