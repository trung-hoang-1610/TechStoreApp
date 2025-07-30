' BLL/Interfaces/IUserService.vb
Imports System.Threading.Tasks

Public Interface IUserService
    ''' <summary>
    ''' Lấy người dùng theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của người dùng</param>
    ''' <returns>Đối tượng User hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetUserById(ByVal id As Integer) As Task(Of User)

    ''' <summary>
    ''' Thêm người dùng mới
    ''' </summary>
    ''' <param name="user">Đối tượng User cần thêm</param>
    ''' <returns>Tuple chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số user là Nothing</exception>
    Function AddUser(ByVal user As User) As Task(Of OperationResult)

    ''' <summary>
    ''' Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
    ''' </summary>
    ''' <param name="username">Tên đăng nhập</param>
    ''' <param name="password">Mật khẩu chưa mã hóa</param>
    ''' <returns>Đối tượng User nếu xác thực thành công, Nothing nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu username hoặc password là Nothing</exception>
    Function ValidateUser(ByVal username As String, ByVal password As String) As Task(Of User)
End Interface