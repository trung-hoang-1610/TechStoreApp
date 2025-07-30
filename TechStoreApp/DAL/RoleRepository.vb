' DAL/RoleRepository.vb
Imports System.Data.Odbc
Imports System.Threading.Tasks
Public Class RoleRepository
    Implements IRoleRepository

    ''' <summary>
    ''' Lấy danh sách tất cả vai trò từ cơ sở dữ liệu
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Role</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetAllRolesAsync() As Task(Of List(Of Role)) Implements IRoleRepository.GetAllRolesAsync
        Dim roles As New List(Of Role)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT RoleId, RoleName FROM Roles"
            Using command As New OdbcCommand(query, connection)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    While Await reader.ReadAsync()
                        Dim role As New Role
                        role.GetType().GetField("_roleId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(role, reader.GetInt32(0))
                        role.RoleName = reader.GetString(1)
                        roles.Add(role)
                    End While
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return roles
    End Function

    ''' <summary>
    ''' Lấy vai trò theo mã định danh từ cơ sở dữ liệu
    ''' </summary>
    ''' <param name="id">Mã định danh của vai trò</param>
    ''' <returns>Đối tượng Role hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetRoleByIdAsync(ByVal id As Integer) As Task(Of Role) Implements IRoleRepository.GetRoleByIdAsync
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT RoleId, RoleName FROM Roles WHERE RoleId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = Await command.ExecuteReaderAsync()
                    If Await reader.ReadAsync() Then
                        Dim role As New Role
                        role.GetType().GetField("_roleId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(role, reader.GetInt32(0))
                        role.RoleName = reader.GetString(1)
                        Return role
                    End If
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return Nothing
    End Function
End Class