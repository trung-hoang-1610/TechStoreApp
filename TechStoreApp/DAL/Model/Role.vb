''' <summary>
''' Đại diện cho vai trò người dùng trong hệ thống (Admin / User)
''' </summary>
Public Class Role

    Private _roleId As Integer
    Private _roleName As String

    ''' <summary>
    ''' Mã vai trò (RoleId)
    ''' </summary>
    Public Property RoleId() As Integer
        Get
            Return _roleId
        End Get
        Set(ByVal value As Integer)
            _roleId = value
        End Set
    End Property

    ''' <summary>
    ''' Tên vai trò (Admin / User)
    ''' </summary>
    Public Property RoleName() As String
        Get
            Return _roleName
        End Get
        Set(ByVal value As String)
            _roleName = value
        End Set
    End Property

End Class
