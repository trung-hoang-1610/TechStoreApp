''' <summary>
''' DTO cho tiêu chí tìm kiếm phiếu nhập/xuất.
''' </summary>
Public Class SearchCriteriaDTO
    Public Property TransactionCode As String ' Mã phiếu (tìm gần đúng)
    Public Property Status As String ' Trạng thái (Pending/Approved/Rejected, có thể null)
    Public Property StartDate As DateTime? ' Từ ngày (có thể null)
    Public Property EndDate As DateTime? ' Đến ngày (có thể null)
    Public Property SupplierId As Integer? ' Mã nhà cung cấp (chỉ cho phiếu nhập, có thể null)
End Class