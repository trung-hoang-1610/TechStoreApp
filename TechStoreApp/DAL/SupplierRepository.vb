Imports System.Data.Odbc
Imports System.Threading.Tasks
''' <summary>
''' Triển khai chức năng truy xuất dữ liệu nhà cung cấp.
''' </summary>
Public Class SupplierRepository
    Implements ISupplierRepository

    ''' <summary>
    ''' Lấy danh sách tất cả nhà cung cấp.
    ''' </summary>
    ''' <returns>Danh sách đối tượng Supplier.</returns>
    Public Async Function GetAllSuppliersAsync() As Task(Of List(Of Supplier)) Implements ISupplierRepository.GetAllSuppliersAsync
        Dim suppliers As New List(Of Supplier)()

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT SupplierId, SupplierName, ContactInfo FROM suppliers"
            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim supplier As New Supplier
                        supplier.GetType().GetField("_supplierId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(supplier, reader.GetInt32(0))
                        supplier.SupplierName = reader.GetString(1)
                        supplier.ContactInfo = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                        suppliers.Add(supplier)
                    End While
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return suppliers
    End Function

    ''' <summary>
    ''' Lấy thông tin một nhà cung cấp theo ID.
    ''' </summary>
    ''' <param name="supplierId">ID nhà cung cấp.</param>
    ''' <returns>Đối tượng Supplier nếu tồn tại, ngược lại Nothing.</returns>
    Public Async Function GetSupplierByIdAsync(supplierId As Integer) As Task(Of Supplier) Implements ISupplierRepository.GetSupplierByIdAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT SupplierId, SupplierName, ContactInfo FROM Suppliers WHERE SupplierId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", supplierId)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    If Await reader.ReadAsync() Then
                        Dim supplier As New Supplier
                        supplier.GetType().GetField("_supplierId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(supplier, reader.GetInt32(0))
                        supplier.SupplierName = reader.GetString(1)
                        supplier.ContactInfo = If(reader.IsDBNull(2), Nothing, reader.GetString(2))
                        Return supplier
                    End If
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Thêm một nhà cung cấp mới vào cơ sở dữ liệu.
    ''' </summary>
    ''' <param name="supplier">Đối tượng Supplier chứa thông tin nhà cung cấp.</param>
    Public Async Function AddSupplierAsync(supplier As Supplier) As Task(Of Integer) Implements ISupplierRepository.AddSupplierAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "INSERT INTO Suppliers (SupplierName, ContactInfo) VALUES (?, ?)"
            Dim getIdQuery As String = "SELECT LAST_INSERT_ID()"

            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("name", supplier.SupplierName)
                command.Parameters.AddWithValue("contact", If(String.IsNullOrEmpty(supplier.ContactInfo), DBNull.Value, supplier.ContactInfo))
                Await command.ExecuteNonQueryAsync()
            End Using

            Using getIdCommand As New OdbcCommand(getIdQuery, connection)
                Dim lastId As Object = Await getIdCommand.ExecuteScalarAsync()
                Return If(lastId IsNot Nothing, Convert.ToInt32(lastId), 0)
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Cập nhật thông tin một nhà cung cấp trong cơ sở dữ liệu.
    ''' </summary>
    ''' <param name="supplier">Đối tượng Supplier chứa thông tin cần cập nhật.</param>
    Public Async Function UpdateSupplierAsync(supplier As Supplier) As Task(Of Boolean) Implements ISupplierRepository.UpdateSupplierAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Suppliers SET SupplierName = ?, ContactInfo = ? WHERE SupplierId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("name", supplier.SupplierName)
                command.Parameters.AddWithValue("contact", If(String.IsNullOrEmpty(supplier.ContactInfo), DBNull.Value, supplier.ContactInfo))
                command.Parameters.AddWithValue("id", supplier.GetType().GetField("_supplierId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).GetValue(supplier))
                Return Await command.ExecuteNonQueryAsync() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Xóa một nhà cung cấp khỏi cơ sở dữ liệu theo ID.
    ''' </summary>
    ''' <param name="supplierId">ID của nhà cung cấp cần xóa.</param>
    Public Async Function DeleteSupplierAsync(supplierId As Integer) As Task(Of Boolean) Implements ISupplierRepository.DeleteSupplierAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "DELETE FROM Suppliers WHERE SupplierId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", supplierId)
                Return Await command.ExecuteNonQueryAsync() > 0
            End Using


            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

End Class