' BLL/Interfaces/IRoleService.vb
Public Interface IRoleService
    ''' <summary>
    ''' Lấy danh sách tất cả vai trò
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Role</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetAllRoles() As List(Of Role)

    ''' <summary>
    ''' Lấy vai trò theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của vai trò</param>
    ''' <returns>Đối tượng Role hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetRoleById(ByVal id As Integer) As Role
End Interface