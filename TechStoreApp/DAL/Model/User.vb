''' <summary>
''' Đại diện cho người dùng trong hệ thống.
''' </summary>
Public Class User
    Private _userId As Integer
    Private _username As String
    Private _passwordHash As String
    Private _email As String
    Private _roleId As Integer
    Private _createdAt As DateTime

    ''' <summary>
    ''' Mã định danh người dùng.
    ''' </summary>
    Public Property UserId() As Integer
        Get
            Return _userId
        End Get
        Set(ByVal value As Integer)
            _userId = value
        End Set
    End Property

    ''' <summary>
    ''' Tên đăng nhập.
    ''' </summary>
    Public Property Username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    ''' <summary>
    ''' Mật khẩu đã mã hóa (hash).
    ''' </summary>
    Public Property PasswordHash() As String
        Get
            Return _passwordHash
        End Get
        Set(ByVal value As String)
            _passwordHash = value
        End Set
    End Property

    ''' <summary>
    ''' Địa chỉ email.
    ''' </summary>
    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    ''' <summary>
    ''' Mã vai trò (1: Admin, 2: User).
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
    ''' Ngày tạo tài khoản.
    ''' </summary>
    Public Property CreatedAt() As DateTime
        Get
            Return _createdAt
        End Get
        Set(ByVal value As DateTime)
            _createdAt = value
        End Set
    End Property
End Class
