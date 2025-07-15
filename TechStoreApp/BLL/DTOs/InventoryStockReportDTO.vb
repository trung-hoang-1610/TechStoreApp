Public Class InventoryStockReportDTO
    Public Property ProductId As Integer
    Public Property ProductName As String
    Public Property TotalImported As Integer   ' Tổng giao dịch IN
    Public Property TotalExported As Integer   ' Tổng giao dịch OUT (dạng dương)
    Public Property CurrentStock As Integer    ' = TotalImported - TotalExported
    Public Property LastTransactionAt As DateTime
End Class
