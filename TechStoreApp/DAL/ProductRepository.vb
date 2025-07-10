' DAL/ProductRepository.vb
Imports System.Data.Odbc

Public Class ProductRepository
    Implements IProductRepository

    ''' <summary>
    ''' Lấy danh sách tất cả sản phẩm từ cơ sở dữ liệu
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Product</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetAllProducts() As List(Of Product) Implements IProductRepository.GetAllProducts
        Dim products As New List(Of Product)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Price, Quantity, CategoryId, CreatedBy, CreatedAt FROM Products"
            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim product As New Product
                        product.GetType().GetField("_productId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, reader.GetInt32(0))
                        product.ProductName = reader.GetString(1)
                        product.Description = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                        product.Price = reader.GetDecimal(3)
                        product.Quantity = reader.GetInt32(4)
                        product.CategoryId = reader.GetInt32(5)
                        product.CreatedBy = If(reader.IsDBNull(6), 0, reader.GetInt32(6))
                        product.GetType().GetField("_createdAt", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, reader.GetDateTime(7))
                        products.Add(product)
                    End While
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return products
    End Function

    ''' <summary>
    ''' Lấy sản phẩm theo mã định danh từ cơ sở dữ liệu
    ''' </summary>
    ''' <param name="id">Mã định danh của sản phẩm</param>
    ''' <returns>Đối tượng Product hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetProductById(ByVal id As Integer) As Product Implements IProductRepository.GetProductById
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT ProductId, ProductName, Description, Price, Quantity, CategoryId, CreatedBy, CreatedAt FROM Products WHERE ProductId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim product As New Product
                        product.GetType().GetField("_productId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, reader.GetInt32(0))
                        product.ProductName = reader.GetString(1)
                        product.Description = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                        product.Price = reader.GetDecimal(3)
                        product.Quantity = reader.GetInt32(4)
                        product.CategoryId = reader.GetInt32(5)
                        product.CreatedBy = If(reader.IsDBNull(6), 0, reader.GetInt32(6))
                        product.GetType().GetField("_createdAt", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, reader.GetDateTime(7))
                        Return product
                    End If
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return Nothing
    End Function

    Public Function AddProduct(ByVal product As Product) As Integer Implements IProductRepository.AddProduct
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "INSERT INTO Products (ProductName, Description, Price, Quantity, CategoryId, CreatedBy) VALUES (?, ?, ?, ?, ?, ?);"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("", product.ProductName)
                command.Parameters.AddWithValue("", If(String.IsNullOrEmpty(product.Description), DBNull.Value, product.Description))
                command.Parameters.AddWithValue("", product.Price)
                command.Parameters.AddWithValue("", product.Quantity)
                command.Parameters.AddWithValue("", product.CategoryId)
                command.Parameters.AddWithValue("", If(product.CreatedBy = 0, DBNull.Value, product.CreatedBy))
                command.ExecuteNonQuery()
            End Using

            ' Lấy ID mới chèn
            Dim idQuery As String = "SELECT LAST_INSERT_ID();"
            Using idCommand As New OdbcCommand(idQuery, connection)
                Return Convert.ToInt32(idCommand.ExecuteScalar())
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function
    Public Function UpdateProduct(ByVal product As Product) As Boolean Implements IProductRepository.UpdateProduct
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Products SET ProductName = ?, Description = ?, Price = ?, Quantity = ?, CategoryId = ?, CreatedBy = ? WHERE ProductId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("", product.ProductName)
                command.Parameters.AddWithValue("", If(String.IsNullOrEmpty(product.Description), DBNull.Value, product.Description))
                command.Parameters.AddWithValue("", product.Price)
                command.Parameters.AddWithValue("", product.Quantity)
                command.Parameters.AddWithValue("", product.CategoryId)
                command.Parameters.AddWithValue("", If(product.CreatedBy = 0, DBNull.Value, product.CreatedBy))
                command.Parameters.AddWithValue("", product.ProductId)
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function
    Public Function DeleteProduct(ByVal id As Integer) As Boolean Implements IProductRepository.DeleteProduct
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "DELETE FROM Products WHERE ProductId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("", id)
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Lấy danh sách sản phẩm theo trang
    ''' </summary>
    ''' <param name="pageIndex">Trang số (bắt đầu từ 0)</param>
    ''' <param name="pageSize">Số sản phẩm mỗi trang</param>
    ''' <returns>Danh sách sản phẩm trong trang</returns>
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
            Dim query As String = $"SELECT ProductId, ProductName, Description, Price, Quantity, CategoryId, CreatedBy, CreatedAt " &
                              $"FROM Products ORDER BY ProductId DESC LIMIT {pageSize} OFFSET {offset}"

            Try
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)
                    command.Parameters.AddWithValue("limit", pageSize)
                    command.Parameters.AddWithValue("offset", pageIndex * pageSize)

                    Using reader As OdbcDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim product As New Product
                            ' Kiểm tra và log giá trị ProductId
                            Dim productId As Integer = If(reader.IsDBNull(0), 0, reader.GetInt32(0))
                            Debug.WriteLine("ProductId from DB: " & productId)

                            ' Kiểm tra trường _productId
                            Dim productIdField = product.GetType().GetField("_productId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
                            If productIdField Is Nothing Then
                                Debug.WriteLine("Error: _productId field not found in Product class")
                            Else
                                productIdField.SetValue(product, productId)
                            End If

                            product.ProductName = reader.GetString(1)
                            product.Description = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                            product.Price = reader.GetDecimal(3)
                            product.Quantity = reader.GetInt32(4)
                            product.CategoryId = reader.GetInt32(5)
                            product.CreatedBy = If(reader.IsDBNull(6), 0, reader.GetInt32(6))

                            ' Kiểm tra trường _createdAt
                            Dim createdAtField = product.GetType().GetField("_createdAt", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
                            If createdAtField Is Nothing Then
                                Debug.WriteLine("Error: _createdAt field not found in Product class")
                            Else
                                createdAtField.SetValue(product, reader.GetDateTime(7))
                            End If

                            products.Add(product)
                        End While
                    End Using
                End Using
            Catch ex As OdbcException
                Debug.WriteLine("Lỗi cơ sở dữ liệu: " & ex.Message & ", Query: " & query)
                Throw
            End Try
        End Using
        Return products
    End Function

    Public Function GetTotalProductCount() As Integer Implements IProductRepository.GetTotalProductCount
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT COUNT(*) FROM Products"
            Using command As New OdbcCommand(query, connection)
                Dim count = Convert.ToInt32(command.ExecuteScalar())
                Return count
            End Using
        End Using
    End Function

End Class