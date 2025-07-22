Imports System.Data.Odbc

''' <summary>
''' Triển khai chức năng truy xuất dữ liệu nhà cung cấp.
''' </summary>
Public Class SupplierRepository
    Implements ISupplierRepository

    ''' <summary>
    ''' Lấy danh sách tất cả nhà cung cấp.
    ''' </summary>
    ''' <returns>Danh sách đối tượng Supplier.</returns>
    Public Function GetAllSuppliers() As List(Of Supplier) Implements ISupplierRepository.GetAllSuppliers
        Dim suppliers As New List(Of Supplier)()

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT SupplierId, SupplierName, ContactInfo FROM suppliers"
            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
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
    Public Function GetSupplierById(supplierId As Integer) As Supplier Implements ISupplierRepository.GetSupplierById
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT SupplierId, SupplierName, ContactInfo FROM Suppliers WHERE SupplierId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", supplierId)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If reader.Read() Then
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
    Public Function AddSupplier(supplier As Supplier) As Integer Implements ISupplierRepository.AddSupplier
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "INSERT INTO Suppliers (SupplierName, ContactInfo) VALUES (?, ?)"
            Dim getIdQuery As String = "SELECT LAST_INSERT_ID()"

            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("name", supplier.SupplierName)
                command.Parameters.AddWithValue("contact", If(String.IsNullOrEmpty(supplier.ContactInfo), DBNull.Value, supplier.ContactInfo))
                command.ExecuteNonQuery()
            End Using

            Using getIdCommand As New OdbcCommand(getIdQuery, connection)
                Dim lastId As Object = getIdCommand.ExecuteScalar()
                Return If(lastId IsNot Nothing, Convert.ToInt32(lastId), 0)
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Cập nhật thông tin một nhà cung cấp trong cơ sở dữ liệu.
    ''' </summary>
    ''' <param name="supplier">Đối tượng Supplier chứa thông tin cần cập nhật.</param>
    Public Function UpdateSupplier(supplier As Supplier) As Boolean Implements ISupplierRepository.UpdateSupplier
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "UPDATE Suppliers SET SupplierName = ?, ContactInfo = ? WHERE SupplierId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("name", supplier.SupplierName)
                command.Parameters.AddWithValue("contact", If(String.IsNullOrEmpty(supplier.ContactInfo), DBNull.Value, supplier.ContactInfo))
                command.Parameters.AddWithValue("id", supplier.GetType().GetField("_supplierId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).GetValue(supplier))
                Return command.ExecuteNonQuery() > 0
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

    ''' <summary>
    ''' Xóa một nhà cung cấp khỏi cơ sở dữ liệu theo ID.
    ''' </summary>
    ''' <param name="supplierId">ID của nhà cung cấp cần xóa.</param>
    Public Function DeleteSupplier(supplierId As Integer) As Boolean Implements ISupplierRepository.DeleteSupplier
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "DELETE FROM Suppliers WHERE SupplierId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", supplierId)
                Return command.ExecuteNonQuery() > 0
            End Using


            ConnectionHelper.CloseConnection(connection)
        End Using
    End Function

End Class