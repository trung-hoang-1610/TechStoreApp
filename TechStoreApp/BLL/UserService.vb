' BLL/UserService.vb

Public Class UserService
    Implements IUserService

    Private ReadOnly _userRepository As IUserRepository

    ''' <summary>
    ''' Khởi tạo UserService với repository tương ứng
    ''' </summary>
    ''' <param name="userRepository">Đối tượng IUserRepository để truy cập dữ liệu</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu userRepository là Nothing</exception>
    Public Sub New(ByVal userRepository As IUserRepository)
        If userRepository Is Nothing Then
            Throw New ArgumentNullException("userRepository", "UserRepository không được là Nothing.")
        End If
        _userRepository = userRepository
    End Sub

    ''' <summary>
    ''' Lấy người dùng theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của người dùng</param>
    ''' <returns>Đối tượng User hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetUserById(ByVal id As Integer) As User Implements IUserService.GetUserById
        Return _userRepository.GetUserById(id)
    End Function

    ''' <summary>
    ''' Thêm người dùng mới
    ''' </summary>
    ''' <param name="user">Đối tượng User cần thêm</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số user là Nothing</exception>
    Public Function AddUser(ByVal user As User) As OperationResult Implements IUserService.AddUser
        If user Is Nothing Then
            Throw New ArgumentNullException("user", "Đối tượng User không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(user.Username, "Tên đăng nhập", errors, True, 50)
        ValidationHelper.ValidateString(user.PasswordHash, "Mật khẩu", errors, True, 255)
        ValidationHelper.ValidateString(user.Email, "Email", errors, False, 100)
        ValidationHelper.ValidateInteger(user.RoleId, "Mã vai trò", errors, 1)

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim newId As Integer = _userRepository.AddUser(user)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
    ''' </summary>
    ''' <param name="username">Tên đăng nhập</param>
    ''' <param name="password">Mật khẩu chưa mã hóa</param>
    ''' <returns>Đối tượng User nếu xác thực thành công, Nothing nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu username hoặc password là Nothing</exception>
    Public Function ValidateUser(ByVal username As String, ByVal password As String) As User Implements IUserService.ValidateUser
        If username Is Nothing Then
            Throw New ArgumentNullException("username", "Tên đăng nhập không được là Nothing.")
        End If
        If password Is Nothing Then
            Throw New ArgumentNullException("password", "Mật khẩu không được là Nothing.")
        End If

        Return _userRepository.ValidateUser(username, password)
    End Function
End Class