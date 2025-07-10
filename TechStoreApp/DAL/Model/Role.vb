' Model/Role.vb
Public Class Role
    ''' <summary>
    ''' Trường riêng lưu mã định danh duy nhất của vai trò
    ''' </summary>
    Private _roleId As Integer

    ''' <summary>
    ''' Trường riêng lưu tên vai trò
    ''' </summary>
    Private _roleName As String

    ''' <summary>
    ''' Lấy mã định danh duy nhất của vai trò
    ''' </summary>
    ''' <returns>Mã vai trò (RoleId)</returns>
    Public ReadOnly Property RoleId As Integer
        Get
            Return _roleId
        End Get
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt tên vai trò, không được trùng lặp
    ''' </summary>
    ''' <returns>Tên vai trò (RoleName)</returns>
    Public Property RoleName As String
        Get
            Return _roleName
        End Get
        Set(value As String)
            _roleName = value
        End Set
    End Property
End Class