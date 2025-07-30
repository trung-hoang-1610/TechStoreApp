' BLL/Interfaces/IProductService.vb

Imports System.Threading.Tasks

Public Interface IProductService
    ''' <summary>
    ''' Lấy danh sách tất cả sản phẩm
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Product</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetAllProductsAsync() As Task(Of List(Of ProductDTO))
    Function GetProductsBySupplierIdAsync(supplierId As Integer) As Task(Of List(Of ProductDTO))
    ''' <summary>
    ''' Lấy sản phẩm theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của sản phẩm</param>
    ''' <returns>Đối tượng Product hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetProductByIdAsync(ByVal id As Integer) As Task(Of ProductDTO)

    ''' <summary>
    ''' Thêm sản phẩm mới
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần thêm</param>
    ''' <returns>Tuple chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số product là Nothing</exception>
    Function AddProductAsync(ByVal product As Product) As Task(Of OperationResult)

    ''' <summary>
    ''' Cập nhật thông tin sản phẩm
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần cập nhật</param>
    ''' <returns>Tuple chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số product là Nothing</exception>
    Function UpdateProductAsync(ByVal product As Product) As Task(Of OperationResult)

    ''' <summary>
    ''' Xóa sản phẩm theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của sản phẩm</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Function DeleteProductAsync(ByVal id As Integer) As Task(Of Boolean)

    ''' <summary>
    ''' Lấy danh sách sản phẩm có phân trang
    ''' </summary>
    ''' <param name="pageIndex">Chỉ số trang (bắt đầu từ 0)</param>
    ''' <param name="pageSize">Số lượng sản phẩm mỗi trang</param>
    ''' <returns>Danh sách sản phẩm tương ứng với trang</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetProductsByPageAsync(ByVal pageIndex As Integer, ByVal pageSize As Integer) As Task(Of List(Of ProductDTO))

    ''' <summary>
    ''' Lấy tổng số lượng sản phẩm trong hệ thống
    ''' </summary>
    ''' <returns>Tổng số sản phẩm</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetTotalProductCountAsync() As Task(Of Integer)


    ''' <summary>
    ''' Tìm kiếm sản phẩm theo các tiêu chí
    ''' </summary>
    ''' <param name="criteria">Tiêu chí tìm kiếm (tên, danh mục, giá, trạng thái, phân trang, sắp xếp)</param>
    ''' <returns>Danh sách sản phẩm phù hợp với tiêu chí</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function SearchProductsAsync(ByVal criteria As ProductSearchCriteriaDTO) As Task(Of List(Of ProductDTO))

    Function GetProductStatisticsAsync(timeRange As String) As Task(Of ProductStatisticsDTO)

End Interface