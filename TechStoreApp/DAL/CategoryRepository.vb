' DAL/CategoryRepository.vb
Imports System.Data.Odbc
Imports Common.DAL
Imports Model
Imports DAL.Interfaces

Public Class CategoryRepository
    Implements ICategoryRepository

    ''' <summary>
    ''' Lấy danh sách tất cả danh mục từ cơ sở dữ liệu
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Category</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetAllCategories() As List(Of Category) Implements ICategoryRepository.GetAllCategories
        Dim categories As New List(Of Category)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT CategoryId, CategoryName, Description FROM Categories"
            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim category As New Category
                        category.GetType().GetField("_categoryId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(category, reader.GetInt32(0))
                        category.CategoryName = reader.GetString(1)
                        category.Description = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                        categories.Add(category)
                    End While
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return categories
    End Function

    ''' <summary>
    ''' Lấy danh mục theo mã định danh từ cơ sở dữ liệu
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>Đối tượng Category hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetCategoryById(ByVal id As Integer) As Category Implements ICategoryRepository.GetCategoryById
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT CategoryId, CategoryName, Description FROM Categories WHERE CategoryId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim category As New Category
                        category.GetType().GetField("_categoryId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(category, reader.GetInt32(0))
                        category.CategoryName = reader.GetString(1)
                        category.Description = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                        Return category
                    End If
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Thêm danh mục mới vào cơ sở dữ liệu
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần thêm</param>
    ''' <returns>Mã định danh của danh mục mới</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    Public Function AddCategory(ByVal category As Category) As Integer Implements ICategoryRepository.AddCategory
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim insertQuery As String = "INSERT INTO Categories (CategoryName, Description) VALUES (?, ?)"
            Using insertCommand As New OdbcCommand(insertQuery, connection)
                insertCommand.Parameters.AddWithValue("name", category.CategoryName)
                insertCommand.Parameters.AddWithValue("description", If(String.IsNullOrEmpty(category.Description), DBNull.Value, category.Description))
                insertCommand.ExecuteNonQuery()
            End Using

            ' Bước 2: lấy ID mới nhất
            Dim idQuery As String = "SELECT LAST_INSERT_ID()"
            Using idCommand As New OdbcCommand(idQuery, connection)
                Return Convert.ToInt32(idCommand.ExecuteScalar())
            End Using

            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function


    ''' <summary>
    ''' Cập nhật thông tin danh mục trong cơ sở dữ liệu
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần cập nhật</param>
    ''' <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    Public Function UpdateCategory(ByVal category As Category) As Boolean Implements ICategoryRepository.UpdateCategory
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Categories SET CategoryName = ?, Description = ? WHERE CategoryId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("name", category.CategoryName)
                command.Parameters.AddWithValue("description", If(String.IsNullOrEmpty(category.Description), DBNull.Value, category.Description))
                command.Parameters.AddWithValue("id", category.CategoryId)
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Xóa danh mục theo mã định danh từ cơ sở dữ liệu
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Public Function DeleteCategory(ByVal id As Integer) As Boolean Implements ICategoryRepository.DeleteCategory
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "DELETE FROM Categories WHERE CategoryId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", id)
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function
End Class