Public Class ProductStatistics
    Public Property TotalProducts As Integer
    Public Property ActiveProducts As Integer
    Public Property InactiveProducts As Integer
    Public Property LowStockProducts As Integer
    Public Property InventoryValue As Decimal
    Public Property ProductsByCategory As Dictionary(Of String, Integer)
End Class