' DAL/Interfaces/ICategoryRepository.vb
Imports System.Threading.Tasks
Public Interface ICategoryRepository
    ''' <summary>
    ''' Lấy danh sách tất cả danh mục
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Category</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetAllCategoriesAsync() As Task(Of List(Of Category))

    ''' <summary>
    ''' Lấy danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>Đối tượng Category hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetCategoryByIdAsync(ByVal id As Integer) As Task(Of Category)

    ''' <summary>
    ''' Thêm danh mục mới vào cơ sở dữ liệu
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần thêm</param>
    ''' <returns>Mã định danh của danh mục mới</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    Function AddCategoryAsync(ByVal category As Category) As Task(Of Integer)

    ''' <summary>
    ''' Cập nhật thông tin danh mục
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần cập nhật</param>
    ''' <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    Function UpdateCategoryAsync(ByVal category As Category) As Task(Of Boolean)

    ''' <summary>
    ''' Xóa danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Function DeleteCategoryAsync(ByVal id As Integer) As Task(Of Boolean)
End Interface