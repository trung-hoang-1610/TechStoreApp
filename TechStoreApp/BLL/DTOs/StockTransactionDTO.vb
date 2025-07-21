''' <summary>
''' DTO cho phiếu nhập/xuất kho.
''' </summary>
Public Class StockTransactionDTO
    ' Trường dùng khi get và add
    Public Property TransactionId As Integer ' Mã phiếu (chỉ dùng khi get)
    Public Property TransactionCode As String ' Mã phiếu (tự sinh hoặc nhập khi add)
    Public Property TransactionType As String ' Loại phiếu (IN/OUT)
    Public Property Note As String ' Ghi chú
    Public Property CreatedBy As Integer ' Mã người tạo (dùng khi add, chỉ get ID)
    Public Property CreatedByName As String ' Tên người tạo (chỉ dùng khi get)
    Public Property CreatedAt As DateTime ' Ngày tạo (chỉ dùng khi get)
    Public Property SupplierId As Integer? ' Mã nhà cung cấp (dùng khi add cho phiếu nhập, có thể null)
    Public Property SupplierName As String ' Tên nhà cung cấp (chỉ dùng khi get)
    Public Property Status As String ' Trạng thái (Pending/Approved/Rejected, chỉ get)
    Public Property ApprovedBy As Integer? ' Mã người duyệt (chỉ get, có thể null)
    Public Property ApprovedByName As String ' Tên người duyệt (chỉ get, có thể null)
    Public Property ApprovedAt As DateTime? ' Ngày duyệt (chỉ get, có thể null)

    Public Property ApprovedAtString As String

End Class