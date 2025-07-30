' BLL/Interfaces/IAuthService.vb
Imports System.Threading.Tasks
''' <summary>
''' Giao diện dịch vụ xác thực người dùng
''' </summary>
''' <remarks>Chứa các phương thức để xác thực và đăng ký người dùng</remarks>
Public Interface IAuthService
    ''' <summary>
    ''' Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
    ''' </summary>
    ''' <param name="username">Tên đăng nhập</param>
    ''' <param name="password">Mật khẩu chưa mã hóa</param>
    ''' <returns>Đối tượng User nếu xác thực thành công, Nothing nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu username hoặc password là Nothing</exception>
    Function ValidateUser(ByVal username As String, ByVal password As String) As Task(Of User)

    ''' <summary>
    ''' Đăng ký người dùng mới
    ''' </summary>
    ''' <param name="user">Đối tượng User chứa thông tin đăng ký</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số user là Nothing</exception>
    Function RegisterUser(ByVal user As User) As Task(Of OperationResult)
End Interface