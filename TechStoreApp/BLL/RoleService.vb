' BLL/RoleService.vb

Public Class RoleService
    Implements IRoleService

    Private ReadOnly _roleRepository As IRoleRepository

    ''' <summary>
    ''' Khởi tạo RoleService với repository tương ứng
    ''' </summary>
    ''' <param name="roleRepository">Đối tượng IRoleRepository để truy cập dữ liệu</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu roleRepository là Nothing</exception>
    Public Sub New(ByVal roleRepository As IRoleRepository)
        If roleRepository Is Nothing Then
            Throw New ArgumentNullException("roleRepository", "RoleRepository không được là Nothing.")
        End If
        _roleRepository = roleRepository
    End Sub

    ''' <summary>
    ''' Lấy danh sách tất cả vai trò
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Role</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetAllRoles() As List(Of Role) Implements IRoleService.GetAllRoles
        Return _roleRepository.GetAllRoles()
    End Function

    ''' <summary>
    ''' Lấy vai trò theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của vai trò</param>
    ''' <returns>Đối tượng Role hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetRoleById(ByVal id As Integer) As Role Implements IRoleService.GetRoleById
        Return _roleRepository.GetRoleById(id)
    End Function
End Class