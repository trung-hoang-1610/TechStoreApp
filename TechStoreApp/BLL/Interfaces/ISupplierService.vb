Imports System.Threading.Tasks
Public Interface ISupplierService
    ''' <summary>
    ''' Lấy danh sách tất cả danh mục
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Category</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetAllSuppliers() As Task(Of List(Of Supplier))

    ''' <summary>
    ''' Lấy danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>Đối tượng Supplier hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetSupplierById(ByVal id As Integer) As Task(Of Supplier)

    ''' <summary>
    ''' Thêm danh mục mới
    ''' </summary>
    ''' <param name="Supplier">Đối tượng Supplier cần thêm</param>
    ''' <returns>Tuple chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số Supplier là Nothing</exception>
    Function AddSupplier(ByVal Supplier As Supplier) As Task(Of OperationResult)

    ''' <summary>
    ''' Cập nhật thông tin danh mục
    ''' </summary>
    ''' <param name="Supplier">Đối tượng Supplier cần cập nhật</param>
    ''' <returns>Tuple chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số Supplier là Nothing</exception>
    Function UpdateSupplier(ByVal Supplier As Supplier) As Task(Of OperationResult)

    ''' <summary>
    ''' Xóa danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Function DeleteSupplier(ByVal id As Integer) As Task(Of Boolean)
End Interface