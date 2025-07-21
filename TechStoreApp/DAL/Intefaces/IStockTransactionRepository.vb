''' <summary>
''' Interface cho các thao tác dữ liệu liên quan đến phiếu nhập/xuất kho.
''' </summary>
Public Interface IStockTransactionRepository
    ''' <summary>
    ''' Tạo một phiếu nhập/xuất kho mới.
    ''' </summary>
    ''' <param name="transaction">Đối tượng phiếu nhập/xuất</param>
    ''' <returns>Mã phiếu được tạo</returns>
    Function CreateTransaction(ByVal transaction As StockTransaction) As Integer

    ''' <summary>
    ''' Cập nhật thông tin phiếu nhập/xuất (bao gồm trạng thái).
    ''' </summary>
    ''' <param name="transaction">Đối tượng phiếu cần cập nhật</param>
    ''' <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
    Function UpdateTransaction(ByVal transaction As StockTransaction) As Boolean

    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất theo loại và người tạo.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="createdBy">Mã người dùng, Nothing nếu muốn lấy tất cả</param>
    ''' <returns>Danh sách phiếu nhập/xuất</returns>
    Function GetTransactions(ByVal transactionType As String, ByVal createdBy As Integer?) As List(Of StockTransaction)

    ''' <summary>
    ''' Lấy thông tin phiếu theo mã phiếu.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Đối tượng phiếu hoặc Nothing nếu không tìm thấy</returns>
    Function GetTransactionById(ByVal transactionId As Integer) As StockTransaction

    ''' <summary>
    ''' Tìm kiếm phiếu theo tiêu chí.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="createdBy">Mã người dùng, Nothing nếu muốn lấy tất cả</param>
    ''' <param name="searchCriteria">Tiêu chí tìm kiếm (mã phiếu, trạng thái, ...)</param>
    ''' <returns>Danh sách phiếu thỏa mãn tiêu chí</returns>
    Function SearchTransactions(ByVal transactionType As String, ByVal createdBy As Integer?, ByVal searchCriteria As String) As List(Of StockTransaction)

    Function CreateTransactionWithDetails(ByVal transaction As StockTransaction, ByVal details As List(Of StockTransactionDetail)) As Integer
    Function ApproveTransaction(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As Boolean

    ''' <summary>
    ''' Lấy danh sách chi tiết sản phẩm trong một phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Danh sách chi tiết sản phẩm thuộc phiếu</returns>
    Function GetTransactionDetails(ByVal transactionId As Integer) As List(Of StockTransactionDetail)

    ''' <summary>
    ''' Lấy thống kê giao dịch trong khoảng thời gian.
    ''' </summary>
    ''' <param name="criteria">Tiêu chí tìm kiếm (mã phiếu, trạng thái, ngày bắt đầu, ngày kết thúc)</param>
    ''' <returns>Đối tượng chứa các số liệu thống kê</returns>
    Function GetTransactionStatistics(ByVal criteria As SearchCriteriaDTO) As TransactionStatisticsDTO

End Interface