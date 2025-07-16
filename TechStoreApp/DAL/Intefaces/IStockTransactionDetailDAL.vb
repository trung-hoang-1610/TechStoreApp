''' <summary>
''' Interface cho các thao tác dữ liệu liên quan đến chi tiết phiếu nhập/xuất kho.
''' </summary>
Public Interface IStockTransactionDetailDAL
    ''' <summary>
    ''' Tạo danh sách chi tiết cho một phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="details">Danh sách chi tiết phiếu</param>
    ''' <returns>True nếu tạo thành công, False nếu thất bại</returns>
    Function CreateTransactionDetails(ByVal details As List(Of StockTransactionDetail)) As Boolean

    ''' <summary>
    ''' Lấy danh sách chi tiết của một phiếu.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Danh sách chi tiết phiếu</returns>
    Function GetTransactionDetails(ByVal transactionId As Integer) As List(Of StockTransactionDetail)
End Interface