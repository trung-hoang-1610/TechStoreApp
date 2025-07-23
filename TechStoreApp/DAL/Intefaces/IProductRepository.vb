' DAL/Interfaces/IProductRepository.vb
Public Interface IProductRepository
    ''' <summary>
    ''' Lấy danh sách tất cả sản phẩm
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Product</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetAllProducts() As List(Of Product)

    ''' <summary>
    ''' Lấy sản phẩm theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của sản phẩm</param>
    ''' <returns>Đối tượng Product hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetProductById(ByVal id As Integer) As Product

    ''' <summary>
    ''' Lấy sản phẩm theo tên
    ''' </summary>
    ''' <param name="name">Tên sản phẩm</param>
    ''' <returns>Danh sách các đối tượng Product phù hợp với tên</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>

    Function GetProductsBySupplierId(ByVal id As Integer) As List(Of Product)

    ''' <summary>
    ''' Thêm sản phẩm mới vào cơ sở dữ liệu
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần thêm</param>
    ''' <returns>Mã định danh của sản phẩm mới</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    Function AddProduct(ByVal product As Product) As Integer

    ''' <summary>
    ''' Cập nhật thông tin sản phẩm
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần cập nhật</param>
    ''' <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    Function UpdateProduct(ByVal product As Product) As Boolean

    ''' <summary>
    ''' Xóa sản phẩm theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của sản phẩm</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Function DeleteProduct(ByVal id As Integer) As Boolean

    ''' <summary>
    ''' Lấy danh sách sản phẩm có phân trang
    ''' </summary>
    ''' <param name="pageIndex">Chỉ số trang (bắt đầu từ 0)</param>
    ''' <param name="pageSize">Số lượng sản phẩm mỗi trang</param>
    ''' <returns>Danh sách sản phẩm tương ứng với trang</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetProductsByPage(ByVal pageIndex As Integer, ByVal pageSize As Integer) As List(Of Product)

    ''' <summary>
    ''' Đếm tổng số sản phẩm trong cơ sở dữ liệu
    ''' </summary>
    ''' <returns>Tổng số sản phẩm</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function GetTotalProductCount() As Integer


    ''' <summary>
    ''' Tìm kiếm sản phẩm theo các tiêu chí
    ''' </summary>
    ''' <param name="criteria">Tiêu chí tìm kiếm (tên, danh mục, giá, trạng thái, phân trang, sắp xếp)</param>
    ''' <returns>Danh sách sản phẩm phù hợp với tiêu chí</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Function SearchProducts(ByVal criteria As ProductSearchCriteriaDTO) As List(Of Product)

    Function GetProductStatistics(timeRange As String) As ProductStatisticsDTO

    ''' <summary>
    ''' Cập nhật số lượng tồn kho của sản phẩm.
    ''' </summary>
    ''' <param name="productId">Mã sản phẩm</param>
    ''' <param name="quantityChange">Số lượng thay đổi (dương cho nhập, âm cho xuất)</param>
    ''' <returns>True nếu cập nhật thành công, False nếu thất bại</returns>
    Function UpdateProductQuantity(ByVal productId As Integer, ByVal quantityChange As Integer) As Boolean

End Interface