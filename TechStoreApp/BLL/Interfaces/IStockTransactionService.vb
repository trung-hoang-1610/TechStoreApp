Imports System.Threading.Tasks
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
    Function CreateStockInTransactionAsync(ByVal transaction As StockTransactionDTO, ByVal details As List(Of StockTransactionDetailDTO)) As Task(Of OperationResult)

    ''' <summary>
    ''' Tạo phiếu xuất kho.
    ''' </summary>
    ''' <param name="transaction">Thông tin phiếu xuất</param>
    ''' <param name="details">Danh sách chi tiết phiếu</param>
    ''' <returns>Kết quả thao tác, bao gồm mã phiếu nếu thành công</returns>
    Function CreateStockOutTransactionAsync(ByVal transaction As StockTransactionDTO, ByVal details As List(Of StockTransactionDetailDTO)) As Task(Of OperationResult)

    ''' <summary>
    ''' Duyệt phiếu nhập/xuất (chỉ admin).
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <param name="approvedBy">Mã admin duyệt</param>
    ''' <param name="isApproved">True nếu duyệt, False nếu từ chối</param>
    ''' <returns>Kết quả thao tác</returns>
    Function ApproveTransactionAsync(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As Task(Of OperationResult)

    Function GetTransactionByIdAsync(ByVal transactionId As Integer) As Task(Of StockTransactionDTO)


    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="userId">Mã người dùng, Nothing nếu là admin</param>
    ''' <returns>Danh sách phiếu</returns>
    Function GetTransactionsAsync(ByVal transactionType As String, ByVal userId As Integer?) As Task(Of List(Of StockTransactionDTO))

    ''' <summary>
    ''' Tìm kiếm phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="userId">Mã người dùng, Nothing nếu là admin</param>
    ''' <param name="criteria">Tiêu chí tìm kiếm</param>
    ''' <returns>Danh sách phiếu thỏa mãn</returns>
    Function SearchTransactionsAsync(ByVal transactionType As String, ByVal userId As Integer?, ByVal criteria As StockTransationSearchCriterialDTO) As Task(Of List(Of StockTransactionDTO))

    ''' <summary>
    ''' Lấy chi tiết phiếu theo mã phiếu.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Danh sách chi tiết phiếu</returns>
    Function GetTransactionDetailsAsync(ByVal transactionId As Integer) As Task(Of List(Of StockTransactionDetailDTO))

    ''' <summary>
    ''' Lấy thống kê giao dịch trong khoảng thời gian.
    ''' </summary>
    ''' <param name="criteria">Tiêu chí tìm kiếm (mã phiếu, trạng thái, ngày bắt đầu, ngày kết thúc)</param>
    ''' <returns>Đối tượng chứa các số liệu thống kê</returns>
    Function GetTransactionStatisticsAsync(ByVal criteria As StockTransationSearchCriterialDTO) As Task(Of TransactionStatisticsDTO)
End Interface
