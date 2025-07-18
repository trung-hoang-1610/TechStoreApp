
Imports System.Data.Odbc
Imports System.Collections.Generic

''' <summary>
''' Lớp DAL cho các thao tác liên quan đến phiếu nhập/xuất kho.
''' </summary>
Public Class StockTransactionRepository
    Implements IStockTransactionRepository

    ''' <summary>
    ''' Tạo một phiếu nhập/xuất kho mới.
    ''' </summary>
    Public Function CreateTransaction(ByVal transaction As StockTransaction) As Integer Implements IStockTransactionRepository.CreateTransaction
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "INSERT INTO StockTransactions (TransactionCode, TransactionType, Note, CreatedBy, SupplierId, Status) VALUES (?, ?, ?, ?, ?, ?)"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transaction.TransactionCode)
                command.Parameters.AddWithValue("?", transaction.TransactionType)
                command.Parameters.AddWithValue("?", If(String.IsNullOrEmpty(transaction.Note), DBNull.Value, transaction.Note))
                command.Parameters.AddWithValue("?", transaction.CreatedBy)
                command.Parameters.AddWithValue("?", If(transaction.SupplierId = 0, DBNull.Value, transaction.SupplierId))
                command.Parameters.AddWithValue("?", transaction.Status)
                command.ExecuteNonQuery()

                ' Lấy TransactionId vừa tạo
                command.CommandText = "SELECT LAST_INSERT_ID()"
                Return Convert.ToInt32(command.ExecuteScalar())
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Cập nhật thông tin phiếu nhập/xuất.
    ''' </summary>
    Public Function UpdateTransaction(ByVal transaction As StockTransaction) As Boolean Implements IStockTransactionRepository.UpdateTransaction
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE StockTransactions SET Status = ?, ApprovedBy = ?, ApprovedAt = ? WHERE TransactionId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transaction.Status)
                command.Parameters.AddWithValue("?", If(transaction.ApprovedBy = 0, DBNull.Value, transaction.ApprovedBy))
                command.Parameters.AddWithValue("?", If(transaction.ApprovedAt = DateTime.MinValue, DBNull.Value, transaction.ApprovedAt))
                command.Parameters.AddWithValue("?", transaction.TransactionId)
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
    End Function

    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất theo loại và người tạo.
    ''' </summary>
    Public Function GetTransactions(ByVal transactionType As String, ByVal createdBy As Integer?) As List(Of StockTransaction) Implements IStockTransactionRepository.GetTransactions
        Dim transactions As New List(Of StockTransaction)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT t.*, u1.Username AS CreatedByName, u2.Username AS ApprovedByName, s.SupplierName " &
                                 "FROM StockTransactions t " &
                                 "LEFT JOIN Users u1 ON t.CreatedBy = u1.UserId " &
                                 "LEFT JOIN Users u2 ON t.ApprovedBy = u2.UserId " &
                                 "LEFT JOIN Suppliers s ON t.SupplierId = s.SupplierId " &
                                 "WHERE t.TransactionType = ?"
            If createdBy.HasValue Then
                query &= " AND t.CreatedBy = ?"
            End If
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transactionType)
                If createdBy.HasValue Then
                    command.Parameters.AddWithValue("?", createdBy.Value)
                End If
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim transaction As New StockTransaction()
                        transaction.SetTransactionId(reader.GetInt32(reader.GetOrdinal("TransactionId")))
                        transaction.TransactionCode = reader.GetString(reader.GetOrdinal("TransactionCode"))
                        transaction.TransactionType = reader.GetString(reader.GetOrdinal("TransactionType"))
                        transaction.Note = If(reader.IsDBNull(reader.GetOrdinal("Note")), Nothing, reader.GetString(reader.GetOrdinal("Note")))
                        transaction.CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"))
                        transaction.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        transaction.ApprovedBy = If(reader.IsDBNull(reader.GetOrdinal("ApprovedBy")), 0, reader.GetInt32(reader.GetOrdinal("ApprovedBy")))
                        transaction.ApprovedAt = If(reader.IsDBNull(reader.GetOrdinal("ApprovedAt")), DateTime.MinValue, reader.GetDateTime(reader.GetOrdinal("ApprovedAt")))
                        transaction.Status = reader.GetString(reader.GetOrdinal("Status"))
                        transaction.SupplierId = If(reader.IsDBNull(reader.GetOrdinal("SupplierId")), 0, reader.GetInt32(reader.GetOrdinal("SupplierId")))
                        transactions.Add(transaction)
                    End While
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
        Return transactions
    End Function

    ''' <summary>
    ''' Lấy thông tin phiếu theo mã phiếu.
    ''' </summary>
    Public Function GetTransactionById(ByVal transactionId As Integer) As StockTransaction Implements IStockTransactionRepository.GetTransactionById
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT t.*, u1.Username AS CreatedByName, u2.Username AS ApprovedByName, s.SupplierName " &
                                 "FROM StockTransactions t " &
                                 "LEFT JOIN Users u1 ON t.CreatedBy = u1.UserId " &
                                 "LEFT JOIN Users u2 ON t.ApprovedBy = u2.UserId " &
                                 "LEFT JOIN Suppliers s ON t.SupplierId = s.SupplierId " &
                                 "WHERE t.TransactionId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transactionId)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim transaction As New StockTransaction()
                        transaction.SetTransactionId(reader.GetInt32(reader.GetOrdinal("TransactionId")))
                        transaction.TransactionCode = reader.GetString(reader.GetOrdinal("TransactionCode"))
                        transaction.TransactionType = reader.GetString(reader.GetOrdinal("TransactionType"))
                        transaction.Note = If(reader.IsDBNull(reader.GetOrdinal("Note")), Nothing, reader.GetString(reader.GetOrdinal("Note")))
                        transaction.CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"))
                        transaction.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        transaction.ApprovedBy = If(reader.IsDBNull(reader.GetOrdinal("ApprovedBy")), 0, reader.GetInt32(reader.GetOrdinal("ApprovedBy")))
                        transaction.ApprovedAt = If(reader.IsDBNull(reader.GetOrdinal("ApprovedAt")), DateTime.MinValue, reader.GetDateTime(reader.GetOrdinal("ApprovedAt")))
                        transaction.Status = reader.GetString(reader.GetOrdinal("Status"))
                        transaction.SupplierId = If(reader.IsDBNull(reader.GetOrdinal("SupplierId")), 0, reader.GetInt32(reader.GetOrdinal("SupplierId")))
                        Return transaction
                    End If
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Tìm kiếm phiếu theo tiêu chí.
    ''' </summary>
    Public Function SearchTransactions(ByVal transactionType As String, ByVal createdBy As Integer?, ByVal searchCriteria As String) As List(Of StockTransaction) Implements IStockTransactionRepository.SearchTransactions
        Dim transactions As New List(Of StockTransaction)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Try
                Dim query As String = "SELECT t.TransactionId, t.TransactionCode, t.TransactionType, t.Note, t.CreatedBy, t.CreatedAt, " &
                                  "t.ApprovedBy, t.ApprovedAt, t.Status, t.SupplierId, " &
                                  "u1.UserName AS CreatedByName, u2.UserName AS ApprovedByName, s.SupplierName AS SupplierName " &
                                  "FROM StockTransactions t " &
                                  "LEFT JOIN Users u1 ON t.CreatedBy = u1.UserId " &
                                  "LEFT JOIN Users u2 ON t.ApprovedBy = u2.UserId " &
                                  "LEFT JOIN Suppliers s ON t.SupplierId = s.SupplierId " &
                                  "WHERE t.TransactionType = ? " &
                                  "AND (t.TransactionCode LIKE ? OR t.Status LIKE ?)"

                If createdBy.HasValue Then
                    query &= " AND t.CreatedBy = ?"
                End If


                Using command As New OdbcCommand(query, connection)
                    ' Thứ tự tham số PHẢI khớp với dấu ? trong query
                    command.Parameters.AddWithValue("@Type", transactionType)
                    command.Parameters.AddWithValue("@CodeLike", "%" & searchCriteria & "%")
                    command.Parameters.AddWithValue("@StatusLike", "%" & searchCriteria & "%")
                    If createdBy.HasValue Then
                        command.Parameters.AddWithValue("@CreatedBy", createdBy.Value)
                    End If

                    Using reader As OdbcDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim transaction As New StockTransaction()
                            transaction.SetTransactionId(reader.GetInt32(reader.GetOrdinal("TransactionId")))
                            transaction.TransactionCode = reader.GetString(reader.GetOrdinal("TransactionCode"))
                            transaction.TransactionType = reader.GetString(reader.GetOrdinal("TransactionType"))
                            transaction.Note = If(reader.IsDBNull(reader.GetOrdinal("Note")), Nothing, reader.GetString(reader.GetOrdinal("Note")))
                            transaction.CreatedBy = reader("CreatedBy")
                            transaction.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            transaction.ApprovedBy = If(reader.IsDBNull(reader.GetOrdinal("ApprovedBy")), 0, reader.GetInt32(reader.GetOrdinal("ApprovedBy")))
                            transaction.ApprovedAt = If(reader.IsDBNull(reader.GetOrdinal("ApprovedAt")), DateTime.MinValue, reader.GetDateTime(reader.GetOrdinal("ApprovedAt")))
                            transaction.Status = reader.GetString(reader.GetOrdinal("Status"))
                            transaction.SupplierId = If(reader.IsDBNull(reader.GetOrdinal("SupplierId")), 0, reader.GetInt32(reader.GetOrdinal("SupplierId")))
                            transactions.Add(transaction)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("ERROR: " & ex.Message & vbCrLf & ex.StackTrace)
                Throw
            Finally
                ConnectionHelper.CloseConnection(connection)
            End Try
        End Using
        Return transactions
    End Function

    ''' <summary>
    ''' Tạo phiếu nhập/xuất kho cùng với chi tiết trong một transaction.
    ''' </summary>
    Public Function CreateTransactionWithDetails(ByVal transaction As StockTransaction, ByVal details As List(Of StockTransactionDetail)) As Integer Implements IStockTransactionRepository.CreateTransactionWithDetails
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim transactionScope As OdbcTransaction = connection.BeginTransaction()
            Dim query As String = "INSERT INTO StockTransactions (TransactionCode, TransactionType, Note, CreatedBy, SupplierId, Status) VALUES (?, ?, ?, ?, ?, ?)"
            Using command As New OdbcCommand(query, connection, transactionScope)
                command.Parameters.AddWithValue("?", transaction.TransactionCode)
                command.Parameters.AddWithValue("?", transaction.TransactionType)
                command.Parameters.AddWithValue("?", If(String.IsNullOrEmpty(transaction.Note), DBNull.Value, transaction.Note))
                command.Parameters.AddWithValue("?", transaction.CreatedBy)
                command.Parameters.AddWithValue("?", If(transaction.SupplierId = 0, DBNull.Value, transaction.SupplierId))
                command.Parameters.AddWithValue("?", transaction.Status)
                command.ExecuteNonQuery()
                command.CommandText = "SELECT LAST_INSERT_ID()"
                Dim transactionId = Convert.ToInt32(command.ExecuteScalar())

                query = "INSERT INTO StockTransactionDetails (TransactionId, ProductId, Quantity, Note) VALUES (?, ?, ?, ?)"
                command.CommandText = query
                For Each detail In details
                    command.Parameters.Clear()
                    command.Parameters.AddWithValue("?", transactionId)
                    command.Parameters.AddWithValue("?", detail.ProductId)
                    command.Parameters.AddWithValue("?", detail.Quantity)
                    command.Parameters.AddWithValue("?", If(String.IsNullOrEmpty(detail.Note), DBNull.Value, detail.Note))
                    command.ExecuteNonQuery()
                Next

                transactionScope.Commit()
                Return transactionId
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
    End Function

    ''' <summary>
    ''' Duyệt phiếu nhập/xuất và cập nhật tồn kho.
    ''' </summary>
    Public Function ApproveTransaction(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As Boolean Implements IStockTransactionRepository.ApproveTransaction
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim transactionScope As OdbcTransaction = connection.BeginTransaction()
            Dim query As String = "SELECT * FROM StockTransactions WHERE TransactionId = ?"
            Using command As New OdbcCommand(query, connection, transactionScope)
                command.Parameters.AddWithValue("?", transactionId)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If Not reader.Read() Then
                        transactionScope.Rollback()
                        Throw New InvalidOperationException("Phiếu không tồn tại.")
                    End If
                    Dim status = reader.GetString(reader.GetOrdinal("Status"))
                    If status <> "Pending" Then
                        transactionScope.Rollback()
                        Throw New InvalidOperationException("Phiếu đã được xử lý.")
                    End If
                    Dim transactionType = reader.GetString(reader.GetOrdinal("TransactionType"))
                    reader.Close()

                    query = "UPDATE StockTransactions SET Status = ?, ApprovedBy = ?, ApprovedAt = ? WHERE TransactionId = ?"
                    command.CommandText = query
                    command.Parameters.Clear()
                    command.Parameters.AddWithValue("?", If(isApproved, "Approved", "Rejected"))
                    command.Parameters.AddWithValue("?", approvedBy)
                    command.Parameters.AddWithValue("?", DateTime.Now)
                    command.Parameters.AddWithValue("?", transactionId)
                    If command.ExecuteNonQuery() = 0 Then
                        transactionScope.Rollback()
                        Throw New InvalidOperationException("Lỗi khi cập nhật trạng thái phiếu.")
                    End If

                    If isApproved Then
                        query = "SELECT ProductId, Quantity FROM StockTransactionDetails WHERE TransactionId = ?"
                        command.CommandText = query
                        command.Parameters.Clear()
                        command.Parameters.AddWithValue("?", transactionId)
                        Using detailReader As OdbcDataReader = command.ExecuteReader()
                            While detailReader.Read()
                                Dim productId = detailReader.GetInt32(detailReader.GetOrdinal("ProductId"))
                                Dim quantity = detailReader.GetInt32(detailReader.GetOrdinal("Quantity"))
                                Dim quantityChange = If(transactionType = "IN", quantity, -quantity)
                                query = "UPDATE Products SET Quantity = Quantity + ? WHERE ProductId = ?"
                                Using updateCommand As New OdbcCommand(query, connection, transactionScope)
                                    updateCommand.Parameters.AddWithValue("?", quantityChange)
                                    updateCommand.Parameters.AddWithValue("?", productId)
                                    If updateCommand.ExecuteNonQuery() = 0 Then
                                        transactionScope.Rollback()
                                        Throw New InvalidOperationException($"Lỗi khi cập nhật tồn kho cho sản phẩm ID {productId}.")
                                    End If
                                End Using
                            End While
                        End Using
                    End If

                    transactionScope.Commit()
                    Return True
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
    End Function
End Class
