' Common/DAL/ConnectionHelper.vb
Imports System.Data.Odbc
Imports System.Configuration

Public Class ConnectionHelper
    ''' <summary>
    ''' Lấy chuỗi kết nối từ tệp App.config
    ''' </summary>
    Private Shared ReadOnly Property ConnectionString As String
        Get
            Return ConfigurationManager.ConnectionStrings("ProductManagementConnection").ConnectionString
        End Get
    End Property

    ''' <summary>
    ''' Tạo và mở kết nối ODBC tới cơ sở dữ liệu
    ''' </summary>
    ''' <returns>Đối tượng OdbcConnection đã mở</returns>
    ''' <exception cref="OdbcException">Ném ra nếu không thể kết nối đến cơ sở dữ liệu</exception>
    Public Shared Function GetConnection() As OdbcConnection
        Dim connection As New OdbcConnection(ConnectionString)
        If connection.State = ConnectionState.Closed Then
            connection.Open()
        End If
        Return connection
    End Function

    ''' <summary>
    ''' Đóng và giải phóng kết nối ODBC
    ''' </summary>
    ''' <param name="connection">Đối tượng OdbcConnection cần đóng</param>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi đóng kết nối</exception>
    Public Shared Sub CloseConnection(ByVal connection As OdbcConnection)
        If connection IsNot Nothing AndAlso connection.State = ConnectionState.Open Then
            connection.Close()
            connection.Dispose()
        End If
    End Sub
End Class