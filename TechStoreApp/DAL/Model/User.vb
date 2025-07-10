' Model/User.vb
Public Class User
    ''' <summary>
    ''' Trường riêng lưu mã định danh duy nhất của người dùng
    ''' </summary>
    Private _userId As Integer

    ''' <summary>
    ''' Trường riêng lưu tên đăng nhập
    ''' </summary>
    Private _username As String

    ''' <summary>
    ''' Trường riêng lưu mật khẩu đã mã hóa
    ''' </summary>
    Private _passwordHash As String

    ''' <summary>
    ''' Trường riêng lưu địa chỉ email
    ''' </summary>
    Private _email As String

    ''' <summary>
    ''' Trường riêng lưu mã vai trò
    ''' </summary>
    Private _roleId As Integer

    ''' <summary>
    ''' Trường riêng lưu thời điểm tạo
    ''' </summary>
    Private _createdAt As DateTime

    ''' <summary>
    ''' Lấy mã định danh duy nhất của người dùng
    ''' </summary>
    ''' <returns>Mã người dùng (UserId)</returns>
    Public ReadOnly Property UserId As Integer
        Get
            Return _userId
        End Get
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt tên đăng nhập, không được trùng lặp
    ''' </summary>
    ''' <returns>Tên đăng nhập (Username)</returns>
    Public Property Username As String
        Get
            Return _username
        End Get
        Set(value As String)
            _username = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt mật khẩu đã được mã hóa
    ''' </summary>
    ''' <returns>Mật khẩu đã mã hóa (PasswordHash)</returns>
    Public Property PasswordHash As String
        Get
            Return _passwordHash
        End Get
        Set(value As String)
            _passwordHash = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt địa chỉ email của người dùng
    ''' </summary>
    ''' <returns>Email</returns>
    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt mã vai trò liên kết với người dùng
    ''' </summary>
    ''' <returns>Mã vai trò (RoleId)</returns>
    Public Property RoleId As Integer
        Get
            Return _roleId
        End Get
        Set(value As Integer)
            _roleId = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy thời điểm tạo người dùng
    ''' </summary>
    ''' <returns>Thời điểm tạo (CreatedAt)</returns>
    Public ReadOnly Property CreatedAt As DateTime
        Get
            Return _createdAt
        End Get
    End Property
End Class