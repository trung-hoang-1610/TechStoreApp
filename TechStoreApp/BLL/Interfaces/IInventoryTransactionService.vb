''' <summary>
''' Interface cung cấp các dịch vụ nghiệp vụ liên quan đến giao dịch kho (Inventory Transactions).
''' </summary>
Public Interface IInventoryTransactionService

    ''' <summary>
    ''' Tạo mới một giao dịch nhập hoặc xuất kho.
    ''' </summary>
    ''' <param name="transaction">Thông tin giao dịch cần tạo</param>
    ''' <returns>True nếu tạo thành công</returns>
    Function CreateTransaction(ByVal transaction As InventoryTransactionInputDto) As Boolean

    ''' <summary>
    ''' Lấy thông tin chi tiết của một giao dịch theo mã.
    ''' </summary>
    ''' <param name="transactionId">Mã giao dịch</param>
    ''' <returns>Giao dịch tương ứng, hoặc Nothing nếu không tồn tại</returns>
    Function GetTransactionById(ByVal transactionId As Integer) As InventoryTransactionDto

    ''' <summary>
    ''' Lấy danh sách các giao dịch liên quan đến một sản phẩm cụ thể.
    ''' </summary>
    ''' <param name="productId">Mã sản phẩm</param>
    ''' <returns>Danh sách các giao dịch</returns>
    Function GetTransactionsByProduct(ByVal productId As Integer) As List(Of InventoryTransactionDto)

    ''' <summary>
    ''' Lấy toàn bộ danh sách giao dịch.
    ''' </summary>
    ''' <returns>Danh sách tất cả giao dịch</returns>
    Function GetAllTransactions() As List(Of InventoryTransactionDto)

    ''' <summary>
    ''' Lấy danh sách giao dịch theo phân trang.
    ''' </summary>
    ''' <param name="pageNumber">Trang hiện tại</param>
    ''' <param name="pageSize">Số giao dịch mỗi trang</param>
    ''' <returns>Danh sách giao dịch trong trang</returns>
    Function GetTransactionsByPage(ByVal pageNumber As Integer, ByVal pageSize As Integer) As List(Of InventoryTransactionDto)

    ''' <summary>
    ''' Lấy tổng số lượng giao dịch trong hệ thống.
    ''' </summary>
    ''' <returns>Tổng số giao dịch</returns>
    Function GetTotalTransactionCount() As Integer

    ''' <summary>
    ''' Lấy tồn kho hiện tại của một sản phẩm.
    ''' </summary>
    ''' <param name="productId">Mã sản phẩm</param>
    ''' <returns>Số lượng tồn kho hiện tại</returns>
    Function GetCurrentStock(ByVal productId As Integer) As Integer

    ''' <summary>
    ''' Cập nhật trạng thái duyệt của giao dịch (Pending → Approved/Rejected).
    ''' Nếu duyệt là Approved, cập nhật tồn kho tương ứng.
    ''' </summary>
    ''' <param name="transactionId">Mã giao dịch</param>
    ''' <param name="newStatus">Trạng thái mới: Approved hoặc Rejected</param>
    ''' <returns>True nếu cập nhật thành công</returns>
    Function UpdateTransactionStatus(ByVal transactionId As Integer, ByVal newStatus As String) As Boolean


    ''' <summary>
    ''' Duyệt giao dịch: chuyển trạng thái thành Approved hoặc Rejected.
    ''' </summary>
    ''' <param name="transactionId">Mã giao dịch</param>
    ''' <param name="newStatus">Trạng thái mới</param>
    ''' <returns>True nếu cập nhật thành công</returns>
    Function ApproveTransaction(transactionId As Integer, newStatus As String) As Boolean

    ''' <summary>
    ''' Lấy báo cáo tồn kho từng sản phẩm: tổng nhập, tổng xuất, tồn kho hiện tại, ngày giao dịch gần nhất.
    ''' </summary>
    Function GetInventoryStockReport() As List(Of InventoryStockReportDTO)
End Interface
