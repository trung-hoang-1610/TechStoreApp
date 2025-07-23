Public Class TransactionStatisticsDTO
    Public TotalInTransactions As Integer
    Public TotalOutTransactions As Integer
    Public TotalTransactionValue As Decimal
    Public StatusBreakdown As Dictionary(Of String, Integer)
    Public TopProducts As List(Of ProductTransactionDTO)
    Public LowStockProducts As List(Of ProductStockDTO)
End Class

Public Class ProductTransactionDTO
    Public Property ProductId As Integer
    Public Property ProductName As String
    Public Property TotalQuantity As Integer
    Public Property TotalValue As Decimal
End Class

Public Class ProductStockDTO
    Public Property ProductId As Integer
    Public Property ProductName As String
    Public Property CurrentStock As Integer
    Public Property MinimumStock As Integer
End Class
