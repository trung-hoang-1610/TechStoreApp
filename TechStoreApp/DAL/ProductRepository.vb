Imports System.Data.Odbc
Imports System.Reflection
Imports System.Diagnostics
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' Repository xử lý các thao tác dữ liệu cho bảng Products.
''' </summary>
Public Class ProductRepository
    Implements IProductRepository


    Public Async Function GetAllProductsAsync() As Task(Of List(Of Product)) Implements IProductRepository.GetAllProductsAsync
        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, isActive FROM Products WHERE IsActive = TRUE ORDER BY ProductId DESC"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim product As New Product
                        MapProductFields(product, reader)
                        products.Add(product)
                    End While
                End Using
            End Using
        End Using
        Return products
    End Function


    Public Async Function GetProductByIdAsync(ByVal id As Integer) As Task(Of Product) Implements IProductRepository.GetProductByIdAsync
        If id <= 0 Then
            Throw New ArgumentException("ID sản phẩm phải lớn hơn 0.", NameOf(id))
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, isActive FROM Products WHERE ProductId = ? AND IsActive = TRUE"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.Int).Value = id
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    If Await reader.ReadAsync() Then
                        Dim product As New Product
                        MapProductFields(product, reader)
                        Return product
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function


    Public Async Function AddProductAsync(ByVal product As Product) As Task(Of Integer) Implements IProductRepository.AddProductAsync
        If product Is Nothing Then
            Throw New ArgumentNullException(NameOf(product), "Sản phẩm không được null.")
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "INSERT INTO Products (ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, IsActive) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);"
            Dim getIdQuery As String = "SELECT LAST_INSERT_ID()"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.VarChar).Value = product.ProductName
                command.Parameters.Add("", OdbcType.Text).Value = If(String.IsNullOrEmpty(product.Description), DBNull.Value, product.Description)
                command.Parameters.Add("", OdbcType.VarChar).Value = If(String.IsNullOrEmpty(product.Unit), "Chiếc", product.Unit)
                command.Parameters.Add("", OdbcType.Decimal).Value = product.Price
                command.Parameters.Add("", OdbcType.Int).Value = product.Quantity
                command.Parameters.Add("", OdbcType.Int).Value = product.MinStockLevel
                command.Parameters.Add("", OdbcType.Int).Value = product.CategoryId
                command.Parameters.Add("", OdbcType.Int).Value = product.SupplierId
                command.Parameters.Add("", OdbcType.Int).Value = If(product.CreatedBy = 0, DBNull.Value, product.CreatedBy)
                command.Parameters.Add("", OdbcType.Int).Value = product.IsActive
                Await command.ExecuteNonQueryAsync()
            End Using

            Using getIdCommand As New OdbcCommand(getIdQuery, connection)
                Dim lastId As Object = Await getIdCommand.ExecuteScalarAsync()
                Return If(lastId IsNot Nothing, Convert.ToInt32(lastId), 0)
            End Using
        End Using
    End Function

    Public Async Function UpdateProductAsync(ByVal product As Product) As Task(Of Boolean) Implements IProductRepository.UpdateProductAsync
        If product Is Nothing OrElse product.ProductId <= 0 Then
            Throw New ArgumentException("Sản phẩm và ID phải hợp lệ.", NameOf(product))
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Products SET ProductName = ?, Description = ?, Unit = ?, Price = ?, Quantity = ?, MinStockLevel = ?, CategoryId = ?, SupplierId = ?, IsActive = ? WHERE ProductId = ?"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.VarChar).Value = product.ProductName
                command.Parameters.Add("", OdbcType.Text).Value = If(String.IsNullOrEmpty(product.Description), DBNull.Value, product.Description)
                command.Parameters.Add("", OdbcType.VarChar).Value = If(String.IsNullOrEmpty(product.Unit), "Chiếc", product.Unit)
                command.Parameters.Add("", OdbcType.Decimal).Value = product.Price
                command.Parameters.Add("", OdbcType.Int).Value = product.Quantity
                command.Parameters.Add("", OdbcType.Int).Value = product.MinStockLevel
                command.Parameters.Add("", OdbcType.Int).Value = product.CategoryId
                command.Parameters.Add("", OdbcType.Int).Value = product.SupplierId
                command.Parameters.Add("", OdbcType.Bit).Value = product.IsActive
                command.Parameters.Add("", OdbcType.Int).Value = product.ProductId
                Return Await command.ExecuteNonQueryAsync() > 0
            End Using
        End Using
    End Function

    Public Async Function DeleteProductAsync(ByVal id As Integer) As Task(Of Boolean) Implements IProductRepository.DeleteProductAsync
        If id <= 0 Then
            Throw New ArgumentException("ID sản phẩm phải lớn hơn 0.", NameOf(id))
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "DELETE FROM Products WHERE ProductId = ?"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.Int).Value = id
                Return Await command.ExecuteNonQueryAsync() > 0
            End Using
        End Using
    End Function

    Public Async Function GetProductsByPageAsync(pageIndex As Integer, pageSize As Integer) As Task(Of List(Of Product)) Implements IProductRepository.GetProductsByPageAsync
        If pageIndex < 0 Then
            Throw New ArgumentException("Chỉ số trang không được nhỏ hơn 0.", NameOf(pageIndex))
        End If
        If pageSize <= 0 Then
            Throw New ArgumentException("Kích thước trang phải lớn hơn 0.", NameOf(pageSize))
        End If

        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim offset = pageIndex * pageSize
            Dim query As String = "SELECT ProductId, ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, isActive FROM Products WHERE IsActive = TRUE ORDER BY ProductId LIMIT ? OFFSET ?"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.Int).Value = pageSize
                command.Parameters.Add("", OdbcType.Int).Value = offset
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim product As New Product
                        MapProductFields(product, reader)
                        products.Add(product)
                    End While
                End Using
            End Using
        End Using
        Return products
    End Function

    Public Async Function GetTotalProductCountAsync() As Task(Of Integer) Implements IProductRepository.GetTotalProductCountAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT COUNT(*) FROM Products WHERE IsActive = TRUE"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                Dim result = Await command.ExecuteScalarAsync()
                Return If(result IsNot Nothing AndAlso Not IsDBNull(result), Convert.ToInt32(result), 0)
            End Using
        End Using
    End Function

    Public Async Function SearchProductsAsync(ByVal criteria As ProductSearchCriteriaDTO) As Task(Of List(Of Product)) Implements IProductRepository.SearchProductsAsync
        If criteria Is Nothing Then
            Throw New ArgumentNullException(NameOf(criteria), "Tiêu chí tìm kiếm không được null.")
        End If

        Dim products As New List(Of Product)
        Dim queryBuilder As New StringBuilder("SELECT ProductId, ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, IsActive FROM Products")
        Dim countQuery As New StringBuilder("SELECT COUNT(*) FROM Products")
        Dim hasConditions As Boolean = False

        ' WHERE conditions
        If Not String.IsNullOrEmpty(criteria.Name) Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & "ProductName LIKE ?")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & "ProductName LIKE ?")
            hasConditions = True
        End If
        If criteria.CategoryId > 0 Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & "CategoryId = ?")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & "CategoryId = ?")
            hasConditions = True
        End If
        If criteria.MinPrice.HasValue Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & "Price >= ?")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & "Price >= ?")
            hasConditions = True
        End If
        If criteria.MaxPrice.HasValue Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & "Price <= ?")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & "Price <= ?")
            hasConditions = True
        End If
        If criteria.IsActive.HasValue Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & "IsActive = ?")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & "IsActive = ?")
            hasConditions = True
        End If
        If criteria.LowStockOnly Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & "Quantity < MinStockLevel")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & "Quantity < MinStockLevel")
            hasConditions = True
        End If

        ' ORDER BY
        Dim sortColumn As String = "ProductId"
        Dim sortDirection As String = "ASC"
        If Not String.IsNullOrEmpty(criteria.SortBy) Then
            Select Case criteria.SortBy.ToLower()
                Case "name"
                    sortColumn = "ProductName"
                Case "priceasc"
                    sortColumn = "Price"
                Case "pricedesc"
                    sortColumn = "Price"
                    sortDirection = "DESC"
            End Select
        End If
        queryBuilder.Append($" ORDER BY {sortColumn} {sortDirection}")

        ' LIMIT & OFFSET
        If criteria.PageSize > 0 Then
            queryBuilder.Append(" LIMIT ? OFFSET ?")
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            ' Đếm số dòng
            Using countCmd As New OdbcCommand(countQuery.ToString(), connection)
                If Not String.IsNullOrEmpty(criteria.Name) Then
                    countCmd.Parameters.Add("", OdbcType.VarChar).Value = $"%{criteria.Name.Replace("'", "''")}%"
                End If
                If criteria.CategoryId > 0 Then
                    countCmd.Parameters.Add("", OdbcType.Int).Value = criteria.CategoryId.Value
                End If
                If criteria.MinPrice.HasValue Then
                    countCmd.Parameters.Add("", OdbcType.Decimal).Value = criteria.MinPrice.Value
                End If
                If criteria.MaxPrice.HasValue Then
                    countCmd.Parameters.Add("", OdbcType.Decimal).Value = criteria.MaxPrice.Value
                End If
                If criteria.IsActive.HasValue Then
                    countCmd.Parameters.Add("", OdbcType.Bit).Value = criteria.IsActive.Value
                End If

                Dim countResult = Await countCmd.ExecuteScalarAsync()
                criteria.TotalCount = If(countResult IsNot Nothing AndAlso Not IsDBNull(countResult), Convert.ToInt32(countResult), 0)
                Debug.WriteLine($"[LOG {DateTime.Now}] Total Count: {criteria.TotalCount}, Query: {countQuery}")
            End Using

            ' Truy vấn dữ liệu
            Using command As New OdbcCommand(queryBuilder.ToString(), connection)
                If Not String.IsNullOrEmpty(criteria.Name) Then
                    command.Parameters.Add("", OdbcType.VarChar).Value = $"%{criteria.Name.Replace("'", "''")}%"
                End If
                If criteria.CategoryId > 0 Then
                    command.Parameters.Add("", OdbcType.Int).Value = criteria.CategoryId.Value
                End If
                If criteria.MinPrice.HasValue Then
                    command.Parameters.Add("", OdbcType.Decimal).Value = criteria.MinPrice.Value
                End If
                If criteria.MaxPrice.HasValue Then
                    command.Parameters.Add("", OdbcType.Decimal).Value = criteria.MaxPrice.Value
                End If
                If criteria.IsActive.HasValue Then
                    command.Parameters.Add("", OdbcType.Bit).Value = criteria.IsActive.Value
                End If
                If criteria.PageSize > 0 Then
                    command.Parameters.Add("", OdbcType.Int).Value = criteria.PageSize
                    command.Parameters.Add("", OdbcType.Int).Value = criteria.PageIndex * criteria.PageSize
                End If

                Debug.WriteLine($"[LOG {DateTime.Now}] Executing Query: {queryBuilder}")
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim product As New Product
                        MapProductFields(product, reader)
                        products.Add(product)
                    End While
                End Using
            End Using
        End Using
        Return products
    End Function


    ''' <summary>
    ''' Ánh xạ dữ liệu từ OdbcDataReader sang đối tượng Product sử dụng reflection.
    ''' </summary>
    ''' <param name="product">Đối tượng Product để ánh xạ.</param>
    ''' <param name="reader">OdbcDataReader chứa dữ liệu.</param>
    Private Sub MapProductFields(ByVal product As Product, ByVal reader As OdbcDataReader)
        product.SetProductId(If(IsDBNull(reader("ProductId")), 0, Convert.ToInt32(reader("ProductId"))))
        product.ProductName = If(IsDBNull(reader("ProductName")), String.Empty, reader("ProductName").ToString())
        product.Description = If(IsDBNull(reader("Description")), String.Empty, reader("Description").ToString())
        product.Unit = If(reader.GetSchemaTable.Columns.Contains("Unit") AndAlso Not IsDBNull(reader("Unit")), reader("Unit").ToString(), "Chiếc")
        product.Price = If(IsDBNull(reader("Price")), 0D, Convert.ToDecimal(reader("Price")))
        product.Quantity = Convert.ToInt32(reader("Quantity"))
        product.MinStockLevel = Convert.ToInt32(reader("MinStockLevel"))
        product.CategoryId = If(IsDBNull(reader("CategoryId")), 0, Convert.ToInt32(reader("CategoryId")))
        product.SupplierId = If(IsDBNull(reader("SupplierId")), 0, Convert.ToInt32(reader("SupplierId")))
        product.CreatedBy = Convert.ToInt32(reader("CreatedBy"))
        product.CreatedAt = Convert.ToDateTime(reader("CreatedAt"))
        product.IsActive = Convert.ToBoolean(reader("IsActive"))
    End Sub

    Public Async Function GetProductStatisticsAsync(timeRange As String) As Task(Of ProductStatisticsDTO) Implements IProductRepository.GetProductStatisticsAsync
        Dim stats As New ProductStatisticsDTO
        stats.ProductsByCategory = New Dictionary(Of String, Integer)

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT COUNT(*) AS Total, " &
                                 "SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS Active, " &
                                 "SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS Inactive, " &
                                 "SUM(CASE WHEN Quantity < MinStockLevel THEN 1 ELSE 0 END) AS LowStock, " &
                                 "SUM(Price * Quantity) AS InventoryValue " &
                                 "FROM Products"
            If timeRange = "7 ngày qua" Then
                query &= " WHERE CreatedAt >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)"
            ElseIf timeRange = "30 ngày qua" Then
                query &= " WHERE CreatedAt >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)"
            End If

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using cmd As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = Await cmd.ExecuteReaderAsync()
                    If Await reader.ReadAsync() Then
                        stats.TotalProducts = reader.GetInt32(0)
                        stats.ActiveProducts = reader.GetInt32(1)
                        stats.InactiveProducts = reader.GetInt32(2)
                        stats.LowStockProducts = reader.GetInt32(3)
                        stats.InventoryValue = If(reader.IsDBNull(4), 0, reader.GetDecimal(4))
                    End If
                End Using
            End Using

            query = "SELECT c.CategoryName, COUNT(p.ProductId) " &
                    "FROM Products p INNER JOIN Categories c ON p.CategoryId = c.CategoryId "
            If timeRange = "7 ngày qua" Then
                query &= "WHERE p.CreatedAt >= DATE_SUB(CURDATE(), INTERVAL 7 DAY) "
            ElseIf timeRange = "30 ngày qua" Then
                query &= "WHERE p.CreatedAt >= DATE_SUB(CURDATE(), INTERVAL 30 DAY) "
            End If
            query &= "GROUP BY c.CategoryName"

            Using cmd As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = Await cmd.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        stats.ProductsByCategory.Add(reader.GetString(0), reader.GetInt32(1))
                    End While
                End Using
            End Using
        End Using
        Return stats
    End Function


    ''' <summary>
    ''' Cập nhật số lượng tồn kho của sản phẩm.
    ''' </summary>
    Public Async Function UpdateProductQuantityAsync(ByVal productId As Integer, ByVal quantityChange As Integer) As Task(Of Boolean) Implements IProductRepository.UpdateProductQuantityAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Products SET Quantity = Quantity + ? WHERE ProductId = ?"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.Int).Value = quantityChange
                command.Parameters.Add("", OdbcType.Int).Value = productId
                Return Await command.ExecuteNonQueryAsync() > 0
            End Using
        End Using
    End Function

    Public Async Function GetProductsBySupplierIdAsync(id As Integer) As Task(Of List(Of Product)) Implements IProductRepository.GetProductsBySupplierIdAsync
        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, isActive FROM Products WHERE IsActive = TRUE AND SupplierId = ? ORDER BY ProductId DESC"

            If connection.State <> ConnectionState.Open Then
                Await connection.OpenAsync()
            End If

            Using command As New OdbcCommand(query, connection)
                command.Parameters.Add("", OdbcType.Int).Value = id
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim product As New Product
                        MapProductFields(product, reader)
                        products.Add(product)
                    End While
                End Using
            End Using
        End Using
        Return products
    End Function

End Class