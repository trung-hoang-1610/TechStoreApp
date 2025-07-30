Imports System.Threading.Tasks

''' <summary>
''' Interface cho các thao tác dữ liệu liên quan đến phiếu nhập/xuất kho.
''' </summary>
Public Interface IStockTransactionRepository
    ''' <summary>
    ''' Tạo một phiếu nhập/xuất kho mới.
    ''' </summary>
    ''' <param name="transaction">Đối tượng phiếu nhập/xuất</param>
    ''' <returns>Mã phiếu được tạo</returns>
    Function CreateTransactionAsync(ByVal transaction As StockTransaction) As Task(Of Integer)

    ''' <summary>
    ''' Cập nhật thông tin phiếu nhập/xuất (bao gồm trạng thái).
    ''' </summary>
    ''' <param name="transaction">Đối tượng phiếu cần cập nhật</param>
    ''' <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
    Function UpdateTransactionAsync(ByVal transaction As StockTransaction) As Task(Of Boolean)

    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất theo loại và người tạo.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="createdBy">Mã người dùng, Nothing nếu muốn lấy tất cả</param>
    ''' <returns>Danh sách phiếu nhập/xuất</returns>
    Function GetTransactionsAsync(ByVal transactionType As String, ByVal createdBy As Integer?) As Task(Of List(Of StockTransaction))

    ''' <summary>
    ''' Lấy thông tin phiếu theo mã phiếu.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Đối tượng phiếu hoặc Nothing nếu không tìm thấy</returns>
    Function GetTransactionByIdAsync(ByVal transactionId As Integer) As Task(Of StockTransaction)

    ''' <summary>
    ''' Tìm kiếm phiếu theo tiêu chí.
    ''' </summary>
    ''' <param name="transactionType">Loại phiếu ("IN" hoặc "OUT")</param>
    ''' <param name="createdBy">Mã người dùng, Nothing nếu muốn lấy tất cả</param>
    ''' <param name="searchCriteria">Tiêu chí tìm kiếm (mã phiếu, trạng thái, ...)</param>
    ''' <returns>Danh sách phiếu thỏa mãn tiêu chí</returns>
    Function SearchTransactionsAsync(ByVal transactionType As String, ByVal createdBy As Integer?, ByVal searchCriteria As StockTransationSearchCriterialDTO) As Task(Of List(Of StockTransaction))

    Function CreateTransactionWithDetailsAsync(ByVal transaction As StockTransaction, ByVal details As List(Of StockTransactionDetail)) As Task(Of Integer)
    Function ApproveTransactionAsync(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As Task(Of Boolean)

    ''' <summary>
    ''' Lấy danh sách chi tiết sản phẩm trong một phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Danh sách chi tiết sản phẩm thuộc phiếu</returns>
    Function GetTransactionDetailsAsync(ByVal transactionId As Integer) As Task(Of List(Of StockTransactionDetail))

    ''' <summary>
    ''' Lấy thống kê giao dịch trong khoảng thời gian.
    ''' </summary>
    ''' <param name="criteria">Tiêu chí tìm kiếm (mã phiếu, trạng thái, ngày bắt đầu, ngày kết thúc)</param>
    ''' <returns>Đối tượng chứa các số liệu thống kê</returns>
    Function GetTransactionStatisticsAsync(ByVal criteria As StockTransationSearchCriterialDTO) As Task(Of TransactionStatisticsDTO)
    Function CountTransactionsAsync(transactionType As String, createdBy As Integer?, criteria As StockTransationSearchCriterialDTO) As Task(Of Integer)
End Interface