''' <summary>
''' DTO cho giao dịch nhập/xuất kho.
''' </summary>
Public Class InventoryTransactionDto
    Public Property TransactionId As Integer
    Public Property ProductName As String
    Public Property Quantity As Integer
    Public Property TransactionType As String
    Public Property Note As String
    Public Property PerformedBy As String
    Public Property PerformedAt As DateTime
    Public Property Status As String
End Class
