''' <summary>
''' Interface cho các thao tác nghiệp vụ liên quan đến phiếu nhập/xuất kho.
''' </summary>
Public Interface IStockTransactionService
    ''' <summary>
    ''' Tạo phiếu nhập kho.
    ''' </summary>
    ''' <param name="transaction">Thông tin phiếu nhập</param>
    ''' <param name="details">Danh sách chi tiết phiếu</param>
    ''' <returns>Kết quả thao tác, bao gồm mã phiếu nếu thành công</returns>
    Function CreateStockInTransaction(ByVal transaction As StockTransactionDTO, ByVal details As List(Of StockTransactionDetailDTO)) As OperationResult

    ''' <summary>
    ''' Tạo phiếu xuất kho.
    ''' </summary>
    ''' <param name="transaction">Thông tin phiếu xuất</param>
    ''' <param name="details">Danh sách chi tiết phiếu</param>
    ''' <returns>Kết quả thao tác, bao gồm mã phiếu nếu thành công</returns>
    Function CreateStockOutTransaction(ByVal transaction As StockTransactionDTO, ByVal details As List(Of StockTransactionDetailDTO)) As OperationResult

    ''' <summary>
    ''' Duyệt phiếu nhập/xuất (chỉ admin).
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <param name="approvedBy">Mã admin duyệt</param>
    ''' <param name="isApproved">True nếu duyệt, False nếu từ chối</param>
    ''' <returns>Kết quả thao tác</returns>
    Function ApproveTransaction(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As OperationResult

    Function GetTransactionById(ByVal transactionId As Integer) As StockTransactionDTO


    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="userId">Mã người dùng, Nothing nếu là admin</param>
    ''' <returns>Danh sách phiếu</returns>
    Function GetTransactions(ByVal transactionType As String, ByVal userId As Integer?) As List(Of StockTransactionDTO)

    ''' <summary>
    ''' Tìm kiếm phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="userId">Mã người dùng, Nothing nếu là admin</param>
    ''' <param name="criteria">Tiêu chí tìm kiếm</param>
    ''' <returns>Danh sách phiếu thỏa mãn</returns>
    Function SearchTransactions(ByVal transactionType As String, ByVal userId As Integer?, ByVal criteria As StockTransationSearchCriterialDTO) As List(Of StockTransactionDTO)

    ''' <summary>
    ''' Lấy chi tiết phiếu theo mã phiếu.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Danh sách chi tiết phiếu</returns>
    Function GetTransactionDetails(ByVal transactionId As Integer) As List(Of StockTransactionDetailDTO)

    ''' <summary>
    ''' Lấy thống kê giao dịch trong khoảng thời gian.
    ''' </summary>
    ''' <param name="criteria">Tiêu chí tìm kiếm (mã phiếu, trạng thái, ngày bắt đầu, ngày kết thúc)</param>
    ''' <returns>Đối tượng chứa các số liệu thống kê</returns>
    Function GetTransactionStatistics(ByVal criteria As StockTransationSearchCriterialDTO) As TransactionStatisticsDTO
End Interface
