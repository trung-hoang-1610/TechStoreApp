' BLL/AuthService.vb
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Public Class AuthService
    Implements IAuthService

    Private ReadOnly _userRepository As IUserRepository

    ''' <summary>
    ''' Khởi tạo AuthService với repository tương ứng
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
    ''' Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
    ''' </summary>
    ''' <param name="username">Tên đăng nhập</param>
    ''' <param name="password">Mật khẩu chưa mã hóa</param>
    ''' <returns>Đối tượng User nếu xác thực thành công, Nothing nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu username hoặc password là Nothing</exception>
    Public Function ValidateUser(ByVal username As String, ByVal password As String) As User Implements IAuthService.ValidateUser
        If username Is Nothing Then
            Throw New ArgumentNullException("username", "Tên đăng nhập không được là Nothing.")
        End If
        If password Is Nothing Then
            Throw New ArgumentNullException("password", "Mật khẩu không được là Nothing.")
        End If

        Return _userRepository.ValidateUser(username, HashPassword(password))
    End Function

    ''' <summary>
    ''' Đăng ký người dùng mới
    ''' </summary>
    ''' <param name="user">Đối tượng User chứa thông tin đăng ký</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số user là Nothing</exception>
    Public Function RegisterUser(ByVal user As User) As OperationResult Implements IAuthService.RegisterUser
        If user Is Nothing Then
            Throw New ArgumentNullException("user", "Đối tượng User không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateUsername(user.Username, errors)
        ValidationHelper.ValidatePassword(user.PasswordHash, errors)
        ValidationHelper.ValidateEmail(user.Email, errors)
        ValidationHelper.ValidateInteger(user.RoleId, "Mã vai trò", errors, 1)


        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        user.PasswordHash = HashPassword(user.PasswordHash)
        Dim newId As Integer = _userRepository.AddUser(user)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Mã hóa mật khẩu sử dụng SHA1
    ''' </summary>
    ''' <param name="password">Mật khẩu cần mã hóa</param>
    ''' <returns>Chuỗi mật khẩu đã mã hóa</returns>
    ''' <exception cref="ArgumentNullException">Ném ra nếu mật khẩu là Nothing</exception>
    Private Shared Function HashPassword(ByVal password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using
    End Function
End Class