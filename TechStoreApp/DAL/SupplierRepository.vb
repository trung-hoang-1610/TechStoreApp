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
                        suppliers.Add(Supplier)
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
            Dim query As String = "SELECT SupplierId, SupplierName, ContactInfo FROM Categories WHERE supplierId = ?"
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

End Class
