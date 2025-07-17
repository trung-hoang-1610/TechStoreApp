Imports System.Data.Odbc
Imports System.Reflection
Imports System.Diagnostics
Imports System.Text

''' <summary>
''' Repository xử lý các thao tác dữ liệu cho bảng Products.
''' </summary>
Public Class ProductRepository
    Implements IProductRepository


    Public Function GetAllProducts() As List(Of Product) Implements IProductRepository.GetAllProducts
        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Price, Quantity, CategoryId, SupplierId, CreatedBy, CreatedAt FROM Products WHERE IsActive = FALSE ORDER BY ProductId DESC"


            If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)
                    Using reader As OdbcDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim product As New Product
                            MapProductFields(product, reader)
                            products.Add(product)
                        End While
                    End Using
                End Using

        End Using
        Return products
    End Function

    Public Function GetProductById(ByVal id As Integer) As Product Implements IProductRepository.GetProductById
        If id <= 0 Then
            Throw New ArgumentException("ID sản phẩm phải lớn hơn 0.", NameOf(id))
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Price, Quantity, CategoryId, SupplierId, CreatedBy, CreatedAt FROM Products WHERE ProductId = ? AND IsActive = FALSE"


            If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)
                    command.Parameters.Add("", OdbcType.Int).Value = id
                    Using reader As OdbcDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            Dim product As New Product
                            MapProductFields(product, reader)
                            Return product
                        End If
                    End Using
                End Using

        End Using
        Return Nothing
    End Function

    Public Function AddProduct(ByVal product As Product) As Integer Implements IProductRepository.AddProduct
        If product Is Nothing Then
            Throw New ArgumentNullException(NameOf(product), "Sản phẩm không được null.")
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "INSERT INTO Products (ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, IsActive) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);"
            Dim getIdQuery As String = "SELECT LAST_INSERT_ID()"

            If connection.State <> ConnectionState.Open Then
                    connection.Open()
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


                    command.ExecuteNonQuery()
                End Using
                ' Lấy ID của bản ghi vừa chèn
                Using getIdCommand As New OdbcCommand(getIdQuery, connection)
                    Dim lastId As Object = getIdCommand.ExecuteScalar()
                    Return If(lastId IsNot Nothing, Convert.ToInt32(lastId), 0)
                End Using

        End Using
    End Function

    Public Function UpdateProduct(ByVal product As Product) As Boolean Implements IProductRepository.UpdateProduct
        If product Is Nothing OrElse product.ProductId <= 0 Then
            Throw New ArgumentException("Sản phẩm và ID phải hợp lệ.", NameOf(product))
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Products SET ProductName = ?, Description = ?, Unit = ?, Price = ?, Quantity = ?, MinStockLevel = ?, CategoryId = ?, SupplierId = ?, IsActive = ? WHERE ProductId = ?"


            If connection.State <> ConnectionState.Open Then
                    connection.Open()
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
                command.Parameters.AddWithValue("", product.IsActive)
                command.Parameters.Add("", OdbcType.Int).Value = product.ProductId

                Return command.ExecuteNonQuery() > 0
                End Using

        End Using
    End Function

    Public Function DeleteProduct(ByVal id As Integer) As Boolean Implements IProductRepository.DeleteProduct
        If id <= 0 Then
            Throw New ArgumentException("ID sản phẩm phải lớn hơn 0.", NameOf(id))
        End If

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = $"Delete FROM Products WHERE ProductId = {id}"


            If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)
                    command.Parameters.Add("", OdbcType.Int).Value = id
                    Return command.ExecuteNonQuery() > 0
                End Using

        End Using
    End Function

    Public Function GetProductsByPage(pageIndex As Integer, pageSize As Integer) As List(Of Product) Implements IProductRepository.GetProductsByPage
        If pageIndex < 0 Then
            Throw New ArgumentException("Chỉ số trang không được nhỏ hơn 0.", NameOf(pageIndex))
        End If
        If pageSize <= 0 Then
            Throw New ArgumentException("Kích thước trang phải lớn hơn 0.", NameOf(pageSize))
        End If

        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim offset = pageIndex * pageSize
            Dim query As String = $"SELECT ProductId, ProductName, Description, Unit, Price, Quantity, 
                                            MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, isActive 
                                    FROM Products WHERE IsActive = TRUE 
                                    ORDER BY ProductId 
                                    LIMIT {pageSize} OFFSET {offset}"

            If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)

                    Using reader As OdbcDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim product As New Product
                            MapProductFields(product, reader)
                            products.Add(product)
                        End While
                    End Using
                End Using

        End Using
        Return products
    End Function

    Public Function GetTotalProductCount() As Integer Implements IProductRepository.GetTotalProductCount
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT COUNT(*) FROM Products WHERE IsActive = TRUE"

            Try
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            Catch ex As OdbcException
                Debug.WriteLine("Lỗi cơ sở dữ liệu: " & ex.Message & ", Query: " & query)
                Throw
            End Try
        End Using
    End Function

    Public Function SearchProducts(ByVal criteria As ProductSearchCriteria) As List(Of Product) Implements IProductRepository.SearchProducts
        If criteria Is Nothing Then
            Throw New ArgumentNullException(NameOf(criteria), "Tiêu chí tìm kiếm không được null.")
        End If

        Dim products As New List(Of Product)
        Dim queryBuilder As New StringBuilder("SELECT ProductId, ProductName, Description, Unit, Price, Quantity, MinStockLevel, CategoryId, SupplierId, CreatedBy, CreatedAt, IsActive FROM Products")
        Dim countQuery As New StringBuilder("SELECT COUNT(*) FROM Products")

        Dim hasConditions As Boolean = False

        ' WHERE conditions
        If Not String.IsNullOrEmpty(criteria.Name) Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & $"ProductName LIKE '%{criteria.Name.Replace("'", "''")}%'")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & $"ProductName LIKE '%{criteria.Name.Replace("'", "''")}%'")
            hasConditions = True
        End If

        If criteria.CategoryId > 0 Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & $"CategoryId = {criteria.CategoryId.Value}")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & $"CategoryId = {criteria.CategoryId.Value}")
            hasConditions = True
        End If

        If criteria.MinPrice.HasValue Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & $"Price >= {criteria.MinPrice.Value}")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & $"Price >= {criteria.MinPrice.Value}")
            hasConditions = True
        End If

        If criteria.MaxPrice.HasValue Then
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & $"Price <= {criteria.MaxPrice.Value}")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & $"Price <= {criteria.MaxPrice.Value}")
            hasConditions = True
        End If

        If criteria.IsActive.HasValue Then
            Dim isActiveInt As Integer = If(criteria.IsActive.Value, 1, 0)
            queryBuilder.Append(If(hasConditions, " AND ", " WHERE ") & $"IsActive = {isActiveInt}")
            countQuery.Append(If(hasConditions, " AND ", " WHERE ") & $"IsActive = {isActiveInt}")
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
                Case Else

            End Select
        End If
        queryBuilder.Append($" ORDER BY {sortColumn} {sortDirection}")

        ' LIMIT & OFFSET
        If criteria.PageSize > 0 Then
            Dim limit = criteria.PageSize
            Dim offset = criteria.PageIndex * criteria.PageSize
            queryBuilder.Append($" LIMIT {limit} OFFSET {offset}")
        End If

        Try
            Using connection As OdbcConnection = ConnectionHelper.GetConnection()
                If connection.State <> ConnectionState.Open Then connection.Open()

                ' Đếm số dòng
                Using countCmd As New OdbcCommand(countQuery.ToString(), connection)
                    Dim countResult As Object = countCmd.ExecuteScalar()
                    criteria.TotalCount = If(countResult IsNot DBNull.Value, Convert.ToInt32(countResult), 0)
                    Debug.WriteLine($"[LOG {DateTime.Now}] Total Count: {criteria.TotalCount}, Query: {countQuery}")
                End Using

                ' Truy vấn dữ liệu
                Using command As New OdbcCommand(queryBuilder.ToString(), connection)
                    Debug.WriteLine($"[LOG {DateTime.Now}] Executing Query: {queryBuilder}")
                    Using reader As OdbcDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim product As New Product
                            MapProductFields(product, reader)
                            products.Add(product)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As OdbcException
            Debug.WriteLine($"[ERROR {DateTime.Now}] OdbcException in SearchProducts: {ex.Message}, Query: {queryBuilder}")
            Throw
        Catch ex As Exception
            Debug.WriteLine($"[ERROR {DateTime.Now}] Exception in SearchProducts: {ex.Message}, Query: {queryBuilder}")
            Throw
        End Try

        Return products
    End Function


    ''' <summary>
    ''' Ánh xạ dữ liệu từ OdbcDataReader sang đối tượng Product sử dụng reflection.
    ''' </summary>
    ''' <param name="product">Đối tượng Product để ánh xạ.</param>
    ''' <param name="reader">OdbcDataReader chứa dữ liệu.</param>
    Private Sub MapProductFields(ByVal product As Product, ByVal reader As OdbcDataReader)
        Try
            product.SetProductId(If(IsDBNull(reader("ProductId")), 0, Convert.ToInt32(reader("ProductId"))))
            Debug.WriteLine("ProductId thô: " & reader.GetValue(reader.GetOrdinal("ProductId")).ToString())

        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại ProductId: " & ex.Message)
        End Try

        Try
            product.ProductName = If(IsDBNull(reader("ProductName")), String.Empty, reader("ProductName").ToString())
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại ProductName: " & ex.Message)
        End Try

        Try
            product.Description = If(IsDBNull(reader("Description")), String.Empty, reader("Description").ToString())
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại Description: " & ex.Message)
        End Try

        Try
            product.Unit = If(reader.GetSchemaTable.Columns.Contains("Unit") AndAlso Not IsDBNull(reader("Unit")), reader("Unit").ToString(), "Chiếc")
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại Unit: " & ex.Message)
        End Try

        Try
            product.Price = If(IsDBNull(reader("Price")), 0D, Convert.ToDecimal(reader("Price")))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại Price: " & ex.Message)
        End Try

        Try
            product.Quantity = Convert.ToInt32(reader("Quantity"))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại Quantity: " & ex.Message)
        End Try

        Try
            product.MinStockLevel = Convert.ToInt32(reader("MinStockLevel"))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại MinStockLevel: " & ex.Message)
        End Try

        Try
            product.CategoryId = If(IsDBNull(reader("CategoryId")), 0, Convert.ToInt32(reader("CategoryId")))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại CategoryId: " & ex.Message)
        End Try
        Try
            product.SupplierId = If(IsDBNull(reader("SupplierId")), 0, Convert.ToInt32(reader("SupplierId")))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại SupplierId: " & ex.Message)
        End Try
        Try
            product.CreatedBy = Convert.ToInt32(reader("CreatedBy"))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại CreatedBy: " & ex.Message)
        End Try

        Try
            product.CreatedAt = Convert.ToDateTime(reader("CreatedAt"))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại CreatedAt: " & ex.Message)
        End Try

        Try
            product.IsActive = Convert.ToBoolean(reader("IsActive"))
        Catch ex As Exception
            Debug.WriteLine("❌ Lỗi tại IsActive: " & ex.Message)
        End Try
    End Sub

    Public Function GetProductStatistics(timeRange As String) As ProductStatistics Implements IProductRepository.GetProductStatistics
        Dim stats As New ProductStatistics
        stats.ProductsByCategory = New Dictionary(Of String, Integer)
        Try
            Using conn As OdbcConnection = ConnectionHelper.GetConnection()
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

                Using cmd As New OdbcCommand(query, conn)
                    Using reader As OdbcDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
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

                Using cmd As New OdbcCommand(query, conn)
                    Using reader As OdbcDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            stats.ProductsByCategory.Add(reader.GetString(0), reader.GetInt32(1))
                        End While
                    End Using
                End Using
            End Using
        Catch ex As OdbcException
            Throw ex
        End Try
        Return stats
    End Function


    ''' <summary>
    ''' Cập nhật số lượng tồn kho của sản phẩm.
    ''' </summary>
    Public Function UpdateProductQuantity(ByVal productId As Integer, ByVal quantityChange As Integer) As Boolean Implements IProductRepository.UpdateProductQuantity
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "UPDATE Products SET Quantity = Quantity + ? WHERE ProductId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", quantityChange)
                command.Parameters.AddWithValue("?", productId)
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
    End Function

    Public Function GetProductsBySupplierId(id As Integer) As List(Of Product) Implements IProductRepository.GetProductsBySupplierId
        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = $"SELECT ProductId, ProductName, Quantity, CategoryId FROM Products WHERE IsActive = TRUE AND Products.SupplierId = {id} ORDER BY ProductId DESC"


            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If

            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
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