Public Interface IInventoryTransactionRepository
    Function AddTransaction(ByVal transaction As InventoryTransaction) As Boolean
    Function GetTransactionById(ByVal transactionId As Integer) As InventoryTransaction
    Function GetTransactionsByProduct(ByVal productId As Integer) As List(Of InventoryTransaction)
    Function GetAllTransactions() As List(Of InventoryTransaction)
    ''' <summary>
    ''' Cập nhật trạng thái duyệt cho một giao dịch (Approved hoặc Rejected).
    ''' </summary>
    ''' <param name="transactionId">ID giao dịch.</param>
    ''' <param name="newStatus">Trạng thái mới: "Approved" hoặc "Rejected".</param>
    ''' <returns>True nếu cập nhật thành công.</returns>
    Function UpdateTransactionStatus(ByVal transactionId As Integer, ByVal newStatus As String) As Boolean

    ' Phân trang
    Function GetTransactionsByPage(ByVal pageNumber As Integer, ByVal pageSize As Integer) As List(Of InventoryTransaction)
    Function GetTotalTransactionCount() As Integer

    ' Tính tồn kho hiện tại
    Function GetCurrentStock(ByVal productId As Integer) As Integer
End Interface
