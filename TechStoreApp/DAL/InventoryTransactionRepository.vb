Imports System.Data.Odbc

''' <summary>
''' Repository xử lý dữ liệu liên quan đến InventoryTransaction.
''' </summary>
Public Class InventoryTransactionRepository
    Implements IInventoryTransactionRepository

    ''' <summary>
    ''' Thêm giao dịch mới vào cơ sở dữ liệu.
    ''' </summary>
    ''' <param name="transaction">Giao dịch cần thêm.</param>
    ''' <returns>True nếu thêm thành công.</returns>
    Public Function AddTransaction(transaction As InventoryTransaction) As Boolean Implements IInventoryTransactionRepository.AddTransaction
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "INSERT INTO InventoryTransactions (ProductId, Quantity, TransactionType, Note, PerformedBy, PerformedAt, Status) " &
                                  "VALUES (?, ?, ?, ?, ?, ?, 'Pending')"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@ProductId", transaction.ProductId)
                command.Parameters.AddWithValue("@Quantity", transaction.Quantity)
                command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType)
                command.Parameters.AddWithValue("@Note", transaction.Note)
                command.Parameters.AddWithValue("@PerformedBy", transaction.PerformedBy)
                command.Parameters.AddWithValue("@PerformedAt", transaction.PerformedAt)
                Return command.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Lấy giao dịch theo ID.
    ''' </summary>
    Public Function GetTransactionById(transactionId As Integer) As InventoryTransaction Implements IInventoryTransactionRepository.GetTransactionById
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT * FROM InventoryTransactions WHERE TransactionId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@TransactionId", transactionId)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Return MapTransaction(reader)
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Lấy tất cả giao dịch.
    ''' </summary>
    Public Function GetAllTransactions() As List(Of InventoryTransaction) Implements IInventoryTransactionRepository.GetAllTransactions
        Dim list As New List(Of InventoryTransaction)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT * FROM InventoryTransactions ORDER BY PerformedAt DESC"
            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        list.Add(MapTransaction(reader))
                    End While
                End Using
            End Using
        End Using
        Return list
    End Function

    ''' <summary>
    ''' Lấy giao dịch theo sản phẩm.
    ''' </summary>
    Public Function GetTransactionsByProduct(productId As Integer) As List(Of InventoryTransaction) Implements IInventoryTransactionRepository.GetTransactionsByProduct
        Dim list As New List(Of InventoryTransaction)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT * FROM InventoryTransactions WHERE ProductId = ? ORDER BY PerformedAt DESC"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@ProductId", productId)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        list.Add(MapTransaction(reader))
                    End While
                End Using
            End Using
        End Using
        Return list
    End Function

    ''' <summary>
    ''' Lấy danh sách giao dịch có phân trang.
    ''' </summary>
    Public Function GetTransactionsByPage(pageNumber As Integer, pageSize As Integer) As List(Of InventoryTransaction) Implements IInventoryTransactionRepository.GetTransactionsByPage
        Dim list As New List(Of InventoryTransaction)
        Dim offset As Integer = pageNumber * pageSize
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT * FROM InventoryTransactions ORDER BY PerformedAt DESC LIMIT ? OFFSET ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@Limit", pageSize)
                command.Parameters.AddWithValue("@Offset", offset)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        list.Add(MapTransaction(reader))
                    End While
                End Using
            End Using
        End Using
        Return list
    End Function

    ''' <summary>
    ''' Đếm tổng số giao dịch.
    ''' </summary>
    Public Function GetTotalTransactionCount() As Integer Implements IInventoryTransactionRepository.GetTotalTransactionCount
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT COUNT(*) FROM InventoryTransactions"
            Using command As New OdbcCommand(query, connection)
                Return Convert.ToInt32(command.ExecuteScalar())
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Tính số lượng tồn kho hiện tại của sản phẩm (tổng Quantity đã duyệt).
    ''' </summary>
    Public Function GetCurrentStock(productId As Integer) As Integer Implements IInventoryTransactionRepository.GetCurrentStock
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT COALESCE(SUM(Quantity), 0) FROM InventoryTransactions WHERE ProductId = ? AND Status = 'Approved'"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@ProductId", productId)
                Return Convert.ToInt32(command.ExecuteScalar())
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Duyệt giao dịch: Cập nhật trạng thái và tồn kho nếu cần.
    ''' </summary>
    Public Function UpdateTransactionStatus(transactionId As Integer, newStatus As String) As Boolean Implements IInventoryTransactionRepository.UpdateTransactionStatus
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()

            Dim transaction As InventoryTransaction = GetTransactionById(transactionId)
            If transaction Is Nothing Then Throw New Exception("Giao dịch không tồn tại.")
            If transaction.Status <> "Pending" Then Throw New Exception("Chỉ có thể duyệt giao dịch đang ở trạng thái Pending.")

            Dim updateQuery As String = "UPDATE InventoryTransactions SET Status = ? WHERE TransactionId = ?"
            Using command As New OdbcCommand(updateQuery, connection)
                command.Parameters.AddWithValue("@Status", newStatus)
                command.Parameters.AddWithValue("@TransactionId", transactionId)
                Dim affected = command.ExecuteNonQuery()

                If newStatus = "Approved" AndAlso affected > 0 Then
                    Dim stockQuery As String = "UPDATE Products SET Quantity = Quantity + ? WHERE ProductId = ?"
                    Using stockCmd As New OdbcCommand(stockQuery, connection)
                        stockCmd.Parameters.AddWithValue("@QuantityChange", transaction.Quantity)
                        stockCmd.Parameters.AddWithValue("@ProductId", transaction.ProductId)
                        stockCmd.ExecuteNonQuery()
                    End Using
                End If

                Return affected > 0
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Chuyển dữ liệu từ OdbcDataReader sang InventoryTransaction.
    ''' </summary>
    Private Function MapTransaction(reader As OdbcDataReader) As InventoryTransaction
        Dim transaction As New InventoryTransaction()

        transaction.TransactionId = Convert.ToInt32(reader("TransactionId"))
        transaction.ProductId = Convert.ToInt32(reader("ProductId"))
        transaction.Quantity = Convert.ToInt32(reader("Quantity"))
        transaction.TransactionType = Convert.ToString(reader("TransactionType"))
        transaction.Note = If(reader.IsDBNull(reader.GetOrdinal("Note")), String.Empty, Convert.ToString(reader("Note")))
        transaction.PerformedBy = Convert.ToInt32(reader("PerformedBy"))
        transaction.PerformedAt = Convert.ToDateTime(reader("PerformedAt"))
        transaction.Status = Convert.ToString(reader("Status"))

        Return transaction
    End Function
End Class
