' DAL/Interfaces/IUserRepository.vb
Imports System.Threading.Tasks
Public Interface IUserRepository
    Function GetAllUsersAsync() As Task(Of List(Of User))
    Function GetUserByUsernameAsync(ByVal username As String) As Task(Of User)

    ''' <summary>
    ''' Lấy người dùng theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của người dùng</param>
    ''' <returns>Đối tượng User hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetUserByIdAsync(ByVal id As Integer) As Task(Of User)

    ''' <summary>
    ''' Thêm người dùng mới vào cơ sở dữ liệu
    ''' </summary>
    ''' <param name="user">Đối tượng User cần thêm</param>
    ''' <returns>Mã định danh của người dùng mới</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    Function AddUserAsync(ByVal user As User) As Task(Of Integer)

    ''' <summary>
    ''' Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
    ''' </summary>
    ''' <param name="username">Tên đăng nhập</param>
    ''' <param name="password">Mật khẩu chưa mã hóa</param>
    ''' <returns>Đối tượng User nếu xác thực thành công, Nothing nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function ValidateUserAsync(ByVal username As String, ByVal password As String) As Task(Of User)
End Interface