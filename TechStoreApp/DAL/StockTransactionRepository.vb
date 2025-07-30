
Imports System.Data.Odbc
Imports System.Threading.Tasks
''' <summary>
''' Lớp DAL cho các thao tác liên quan đến phiếu nhập/xuất kho.
''' </summary>
Public Class StockTransactionRepository
    Implements IStockTransactionRepository

    ''' <summary>
    ''' Tạo một phiếu nhập/xuất kho mới.
    ''' </summary>
    Public Async Function CreateTransactionAsync(ByVal transaction As StockTransaction) As Task(Of Integer) Implements IStockTransactionRepository.CreateTransactionAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim query As String = "INSERT INTO StockTransactions (TransactionCode, TransactionType, Note, CreatedBy, SupplierId, Status) VALUES (?, ?, ?, ?, ?, ?)"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transaction.TransactionCode)
                command.Parameters.AddWithValue("?", transaction.TransactionType)
                command.Parameters.AddWithValue("?", If(String.IsNullOrEmpty(transaction.Note), DBNull.Value, transaction.Note))
                command.Parameters.AddWithValue("?", transaction.CreatedBy)
                command.Parameters.AddWithValue("?", If(transaction.SupplierId = 0, DBNull.Value, transaction.SupplierId))
                command.Parameters.AddWithValue("?", transaction.Status)
                Await command.ExecuteNonQueryAsync()

                ' Lấy TransactionId vừa tạo
                command.CommandText = "SELECT LAST_INSERT_ID()"
                Dim lastId = Await command.ExecuteScalarAsync()
                Return If(lastId IsNot Nothing, Convert.ToInt32(lastId), 0)
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Cập nhật thông tin phiếu nhập/xuất.
    ''' </summary>
    Public Async Function UpdateTransaction(ByVal transaction As StockTransaction) As Task(Of Boolean) Implements IStockTransactionRepository.UpdateTransactionAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim query As String = "UPDATE StockTransactions SET Status = ?, ApprovedBy = ?, ApprovedAt = ? WHERE TransactionId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transaction.Status)
                command.Parameters.AddWithValue("?", If(transaction.ApprovedBy = 0, DBNull.Value, transaction.ApprovedBy))
                command.Parameters.AddWithValue("?", If(transaction.ApprovedAt = DateTime.MinValue, DBNull.Value, transaction.ApprovedAt))
                command.Parameters.AddWithValue("?", transaction.TransactionId)
                Return Await command.ExecuteNonQueryAsync() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
    End Function

    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất theo loại và người tạo.
    ''' </summary>
    Public Async Function GetTransactions(ByVal transactionType As String, ByVal createdBy As Integer?) As Task(Of List(Of StockTransaction)) Implements IStockTransactionRepository.GetTransactionsAsync
        Dim transactions As New List(Of StockTransaction)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
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
                    While Await reader.ReadAsync()
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
    Public Async Function GetTransactionById(ByVal transactionId As Integer) As Task(Of StockTransaction) Implements IStockTransactionRepository.GetTransactionByIdAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim query As String = "SELECT t.*, u1.Username AS CreatedByName, u2.Username AS ApprovedByName, s.SupplierName " &
                                 "FROM StockTransactions t " &
                                 "LEFT JOIN Users u1 ON t.CreatedBy = u1.UserId " &
                                 "LEFT JOIN Users u2 ON t.ApprovedBy = u2.UserId " &
                                 "LEFT JOIN Suppliers s ON t.SupplierId = s.SupplierId " &
                                 "WHERE t.TransactionId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transactionId)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    If Await reader.ReadAsync() Then
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

    Public Async Function SearchTransactions(transactionType As String, createdBy As Integer?, criteria As StockTransationSearchCriterialDTO) As Task(Of List(Of StockTransaction)) Implements IStockTransactionRepository.SearchTransactionsAsync
        Dim transactions As New List(Of StockTransaction)

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Dim query As String = "SELECT t.TransactionId, t.TransactionCode, t.TransactionType, t.Note, t.CreatedBy, t.CreatedAt, " &
                                  "t.ApprovedBy, t.ApprovedAt, t.Status, t.SupplierId, " &
                                  "u1.UserName AS CreatedByName, u2.UserName AS ApprovedByName, s.SupplierName AS SupplierName " &
                                  "FROM StockTransactions t " &
                                  "LEFT JOIN Users u1 ON t.CreatedBy = u1.UserId " &
                                  "LEFT JOIN Users u2 ON t.ApprovedBy = u2.UserId " &
                                  "LEFT JOIN Suppliers s ON t.SupplierId = s.SupplierId " &
                                  "WHERE t.TransactionType = ?"

            ' ========== Bộ lọc tìm kiếm ==========
            ' Mã phiếu
            If Not String.IsNullOrEmpty(criteria.TransactionCode) Then
                query &= " AND t.TransactionCode LIKE ?"
            End If

            ' Trạng thái
            If Not String.IsNullOrEmpty(criteria.Status) Then
                query &= " AND t.Status LIKE ?"
            End If

            ' Người tạo
            If createdBy.HasValue Then
                query &= " AND t.CreatedBy = ?"
            End If

            ' Ngày tạo từ
            If criteria.StartDate.HasValue Then
                query &= " AND t.CreatedAt >= ?"
            End If

            ' Ngày tạo đến
            If criteria.EndDate.HasValue Then
                query &= " AND t.CreatedAt <= ?"
            End If

            ' Nhà cung cấp (chỉ với phiếu nhập)
            If criteria.SupplierId.HasValue AndAlso transactionType = "IN" Then
                query &= " AND t.SupplierId = ?"
            End If

            query &= " ORDER BY t.TransactionId DESC LIMIT ? OFFSET ?"

            Using command As New OdbcCommand(query, connection)
                ' === Thêm tham số khớp thứ tự ===
                command.Parameters.AddWithValue("@TransactionType", transactionType)

                If Not String.IsNullOrEmpty(criteria.TransactionCode) Then
                    command.Parameters.AddWithValue("@CodeLike", "%" & criteria.TransactionCode.Trim() & "%")
                End If

                If Not String.IsNullOrEmpty(criteria.Status) Then
                    command.Parameters.AddWithValue("@StatusLike", "%" & criteria.Status.Trim() & "%")
                End If

                If createdBy.HasValue Then
                    command.Parameters.AddWithValue("@CreatedBy", createdBy.Value)
                End If

                If criteria.StartDate.HasValue Then
                    command.Parameters.AddWithValue("@StartDate", criteria.StartDate.Value)
                End If

                If criteria.EndDate.HasValue Then
                    command.Parameters.AddWithValue("@EndDate", criteria.EndDate.Value)
                End If

                If criteria.SupplierId.HasValue AndAlso transactionType = "IN" Then
                    command.Parameters.AddWithValue("@SupplierId", criteria.SupplierId.Value)
                End If

                command.Parameters.AddWithValue("@Limit", criteria.PageSize)
                command.Parameters.AddWithValue("@Offset", (criteria.PageIndex - 1) * criteria.PageSize)

                ' === Mapping kết quả ===
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
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


            ConnectionHelper.CloseConnection(connection)
        End Using

        Return transactions
    End Function


    Public Async Function CountTransactions(transactionType As String, createdBy As Integer?, criteria As StockTransationSearchCriterialDTO) As Task(Of Integer) Implements IStockTransactionRepository.CountTransactionsAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim count As Integer = 0

            Dim query As String = "SELECT COUNT(*) FROM StockTransactions t WHERE t.TransactionType = ?"

            If Not String.IsNullOrEmpty(criteria.TransactionCode) Then
                query &= " AND t.TransactionCode LIKE ?"
            End If

            If Not String.IsNullOrEmpty(criteria.Status) Then
                query &= " AND t.Status LIKE ?"
            End If

            If createdBy.HasValue Then
                query &= " AND t.CreatedBy = ?"
            End If

            If criteria.StartDate.HasValue Then
                query &= " AND t.CreatedAt >= ?"
            End If

            If criteria.EndDate.HasValue Then
                query &= " AND t.CreatedAt <= ?"
            End If

            If criteria.SupplierId.HasValue AndAlso transactionType = "IN" Then
                query &= " AND t.SupplierId = ?"
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@TransactionType", transactionType)

                If Not String.IsNullOrEmpty(criteria.TransactionCode) Then
                    command.Parameters.AddWithValue("@CodeLike", "%" & criteria.TransactionCode.Trim() & "%")
                End If

                If Not String.IsNullOrEmpty(criteria.Status) Then
                    command.Parameters.AddWithValue("@StatusLike", "%" & criteria.Status.Trim() & "%")
                End If

                If createdBy.HasValue Then
                    command.Parameters.AddWithValue("@CreatedBy", createdBy.Value)
                End If

                If criteria.StartDate.HasValue Then
                    command.Parameters.AddWithValue("@StartDate", criteria.StartDate.Value)
                End If

                If criteria.EndDate.HasValue Then
                    command.Parameters.AddWithValue("@EndDate", criteria.EndDate.Value)
                End If

                If criteria.SupplierId.HasValue AndAlso transactionType = "IN" Then
                    command.Parameters.AddWithValue("@SupplierId", criteria.SupplierId.Value)
                End If


                count = Await command.ExecuteScalarAsync()
            End Using

            ConnectionHelper.CloseConnection(connection)
            Return count
        End Using
    End Function

    ''' <summary>
    ''' Tạo phiếu nhập/xuất kho cùng với chi tiết trong một transaction.
    ''' </summary>
    Public Async Function CreateTransactionWithDetails(ByVal transaction As StockTransaction, ByVal details As List(Of StockTransactionDetail)) As Task(Of Integer) Implements IStockTransactionRepository.CreateTransactionWithDetailsAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim transactionScope As OdbcTransaction = connection.BeginTransaction()
            Dim query As String = "INSERT INTO StockTransactions (TransactionCode, TransactionType, Note, CreatedBy, SupplierId, Status) VALUES (?, ?, ?, ?, ?, ?)"
            Using command As New OdbcCommand(query, connection, transactionScope)
                command.Parameters.AddWithValue("?", transaction.TransactionCode)
                command.Parameters.AddWithValue("?", transaction.TransactionType)
                command.Parameters.AddWithValue("?", If(String.IsNullOrEmpty(transaction.Note), DBNull.Value, transaction.Note))
                command.Parameters.AddWithValue("?", transaction.CreatedBy)
                command.Parameters.AddWithValue("?", If(transaction.SupplierId = 0, DBNull.Value, transaction.SupplierId))
                command.Parameters.AddWithValue("?", transaction.Status)
                Await command.ExecuteNonQueryAsync()
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
                    Await command.ExecuteNonQueryAsync()
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
    Public Async Function ApproveTransaction(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As Task(Of Boolean) Implements IStockTransactionRepository.ApproveTransactionAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim transactionScope As OdbcTransaction = connection.BeginTransaction()
            Dim query As String = "SELECT * FROM StockTransactions WHERE TransactionId = ?"
            Using command As New OdbcCommand(query, connection, transactionScope)
                command.Parameters.AddWithValue("?", transactionId)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    If Not Await reader.ReadAsync() Then
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

                    ' Nếu duyệt phiếu xuất, cần kiểm tra tồn kho trước
                    If isApproved AndAlso transactionType = "OUT" Then
                        query = "SELECT d.ProductId, d.Quantity, p.Quantity AS CurrentStock
                             FROM StockTransactionDetails d
                             INNER JOIN Products p ON d.ProductId = p.ProductId
                             WHERE d.TransactionId = ?"
                        command.CommandText = query
                        command.Parameters.Clear()
                        command.Parameters.AddWithValue("?", transactionId)

                        Using checkReader As OdbcDataReader = Await command.ExecuteReaderAsync()
                            While Await checkReader.ReadAsync()
                                Dim productId = checkReader.GetInt32(checkReader.GetOrdinal("ProductId"))
                                Dim quantity = checkReader.GetInt32(checkReader.GetOrdinal("Quantity"))
                                Dim currentStock = checkReader.GetInt32(checkReader.GetOrdinal("CurrentStock"))
                                If quantity > currentStock Then
                                    transactionScope.Rollback()
                                    Throw New InvalidOperationException($"Không đủ tồn kho để xuất sản phẩm ID {productId}. Hiện còn {currentStock}, cần {quantity}.")
                                End If
                            End While
                        End Using
                    End If

                    ' Cập nhật trạng thái phiếu
                    query = "UPDATE StockTransactions SET Status = ?, ApprovedBy = ?, ApprovedAt = ? WHERE TransactionId = ?"
                    command.CommandText = query
                    command.Parameters.Clear()
                    command.Parameters.AddWithValue("?", If(isApproved, "Approved", "Rejected"))
                    command.Parameters.AddWithValue("?", approvedBy)
                    command.Parameters.AddWithValue("?", DateTime.Now)
                    command.Parameters.AddWithValue("?", transactionId)
                    If Await command.ExecuteNonQueryAsync() = 0 Then
                        transactionScope.Rollback()
                        Throw New InvalidOperationException("Lỗi khi cập nhật trạng thái phiếu.")
                    End If

                    ' Nếu là duyệt => cập nhật tồn kho
                    If isApproved Then
                        query = "SELECT ProductId, Quantity FROM StockTransactionDetails WHERE TransactionId = ?"
                        command.CommandText = query
                        command.Parameters.Clear()
                        command.Parameters.AddWithValue("?", transactionId)

                        Using detailReader As OdbcDataReader = Await command.ExecuteReaderAsync()
                            While Await detailReader.ReadAsync()
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
        End Using
    End Function


    ''' <summary>
    ''' Lấy danh sách chi tiết sản phẩm trong một phiếu nhập/xuất.
    ''' </summary>
    ''' <param name="transactionId">Mã phiếu</param>
    ''' <returns>Danh sách chi tiết</returns>
    Public Async Function GetTransactionDetails(ByVal transactionId As Integer) As Task(Of List(Of StockTransactionDetail)) Implements IStockTransactionRepository.GetTransactionDetailsAsync
        Dim details As New List(Of StockTransactionDetail)

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If
            Dim query As String = "SELECT DetailId, TransactionId, ProductId, Quantity, Note FROM StockTransactionDetails WHERE TransactionId = ?"

            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transactionId)

                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim detail As New StockTransactionDetail()

                        detail.SetDetailId(reader.GetInt32(reader.GetOrdinal("DetailId")))
                        detail.TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId"))
                        detail.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"))
                        detail.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                        detail.Note = If(reader.IsDBNull(reader.GetOrdinal("Note")), String.Empty, reader.GetString(reader.GetOrdinal("Note")))

                        details.Add(detail)
                    End While
                End Using
            End Using

            ConnectionHelper.CloseConnection(connection)

        End Using

        Return details
    End Function
    Public Async Function GetTransactionStatistics(ByVal criteria As StockTransationSearchCriterialDTO) As Task(Of TransactionStatisticsDTO) Implements IStockTransactionRepository.GetTransactionStatisticsAsync
        Dim stats As New TransactionStatisticsDTO
        stats.StatusBreakdown = New Dictionary(Of String, Integer)
        stats.TopProducts = New List(Of ProductTransactionDTO)
        stats.LowStockProducts = New List(Of ProductStockDTO)

        Using conn As OdbcConnection = ConnectionHelper.GetConnection()
            If conn.State <> ConnectionState.Open Then
                Await conn.OpenAsync()
            End If

            ' Tổng số phiếu nhập/xuất và trạng thái
            Dim sql As String = "SELECT `TransactionType`, `Status`, COUNT(*) as `Count` " &
                           "FROM `StockTransactions` " &
                           "WHERE `CreatedAt` >= ? " &
                           "GROUP BY `TransactionType`, `Status`"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("?", DateTime.Now.AddDays(-30))
                Using reader As OdbcDataReader = Await cmd.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim transType As String = reader.GetString(reader.GetOrdinal("TransactionType"))
                        Dim status As String = reader.GetString(reader.GetOrdinal("Status"))
                        Dim count As Integer = reader.GetInt32(reader.GetOrdinal("Count"))
                        If transType = "IN" Then
                            stats.TotalInTransactions += count
                        ElseIf transType = "OUT" Then
                            stats.TotalOutTransactions += count
                        End If
                        stats.StatusBreakdown(status) = count
                    End While
                End Using
            End Using

            ' Tổng giá trị giao dịch
            sql = "SELECT SUM(d.`Quantity` * p.`Price`) as `TotalValue` " &
              "FROM `StockTransactionDetails` d " &
              "JOIN `StockTransactions` t ON d.`TransactionId` = t.`TransactionId` " &
              "JOIN `Products` p ON d.`ProductId` = p.`ProductId` " &
              "WHERE t.`CreatedAt` >= ?"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("?", DateTime.Now.AddDays(-30))
                Using reader As OdbcDataReader = Await cmd.ExecuteReaderAsync()
                    If Await reader.ReadAsync() AndAlso Not reader.IsDBNull(reader.GetOrdinal("TotalValue")) Then
                        stats.TotalTransactionValue = reader.GetDecimal(reader.GetOrdinal("TotalValue"))
                    Else
                        stats.TotalTransactionValue = 0
                    End If
                End Using
            End Using

            ' Top 10 sản phẩm giao dịch nhiều nhất
            sql = "SELECT d.`ProductId`, p.`ProductName`, SUM(d.`Quantity`) as `TotalQuantity`, SUM(d.`Quantity` * p.`Price`) as `TotalValue` " &
              "FROM `StockTransactionDetails` d " &
              "JOIN `StockTransactions` t ON d.`TransactionId` = t.`TransactionId` " &
              "JOIN `Products` p ON d.`ProductId` = p.`ProductId` " &
              "WHERE t.`CreatedAt` >= ? " &
              "GROUP BY d.`ProductId`, p.`ProductName` " &
              "ORDER BY `TotalQuantity` DESC " &
              "LIMIT 10"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("?", DateTime.Now.AddDays(-30))
                Using reader As OdbcDataReader = Await cmd.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        stats.TopProducts.Add(New ProductTransactionDTO With {
                        .ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        .ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        .TotalQuantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                        .TotalValue = reader.GetDecimal(reader.GetOrdinal("TotalValue"))
                    })
                    End While
                End Using
            End Using

            ' Sản phẩm dưới mức tồn kho
            sql = "SELECT p.`ProductId`, p.`ProductName`, p.`Quantity`, p.`MinStockLevel` " &
              "FROM `Products` p " &
              "WHERE p.`Quantity` < p.`MinStockLevel`"
            Using cmd As New OdbcCommand(sql, conn)
                Using reader As OdbcDataReader = Await cmd.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        stats.LowStockProducts.Add(New ProductStockDTO With {
                        .ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        .ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        .CurrentStock = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        .MinimumStock = reader.GetInt32(reader.GetOrdinal("MinStockLevel"))
                    })
                    End While
                End Using
            End Using

            ConnectionHelper.CloseConnection(conn)
        End Using

        Return stats
    End Function
End Class
