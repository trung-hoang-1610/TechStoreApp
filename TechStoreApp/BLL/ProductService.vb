' BLL/ProductService.vb


Public Class ProductService
    Implements IProductService

    Private ReadOnly _productRepository As IProductRepository
    Private ReadOnly _categoryRepository As ICategoryRepository
    Private ReadOnly _supplierRepository As ISupplierRepository
    Private ReadOnly _categoryCache As Dictionary(Of Integer, Category)
    Private ReadOnly _supplierCache As Dictionary(Of Integer, Supplier)

    ''' <summary>
    ''' Khởi tạo ProductService với repository tương ứng
    ''' </summary>
    ''' <param name="productRepository">Đối tượng IProductRepository</param>
    ''' <param name="categoryRepository">Đối tượng ICategoryRepository</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu productRepository hoặc categoryRepository là Nothing</exception>
    Public Sub New(ByVal productRepository As IProductRepository, ByVal categoryRepository As ICategoryRepository, ByVal supplierRepository As ISupplierRepository)
        If productRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(productRepository), "ProductRepository không được là Nothing.")
        End If
        If categoryRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(categoryRepository), "CategoryRepository không được là Nothing.")
        End If
        _productRepository = productRepository
        _categoryRepository = categoryRepository
        _supplierRepository = supplierRepository
        _categoryCache = LoadCategoryCache()
        _supplierCache = LoadSupplierCache()

    End Sub

    ''' <summary>
    ''' Tải danh sách danh mục vào bộ nhớ cache để cải thiện hiệu suất
    ''' </summary>
    ''' <returns>Danh sách danh mục dưới dạng Dictionary</returns>
    Private Function LoadCategoryCache() As Dictionary(Of Integer, Category)

        Dim categories = _categoryRepository.GetAllCategories()
        Return categories.ToDictionary(Function(c) c.CategoryId, Function(c) c)

    End Function

    Private Function LoadSupplierCache() As Dictionary(Of Integer, Supplier)

        Dim suppliers = _supplierRepository.GetAllSuppliers()
        Return suppliers.ToDictionary(Function(s) s.SupplierId, Function(s) s)

    End Function

    ''' <summary>
    ''' Trả về danh sách tất cả sản phẩm dưới dạng DTO
    ''' </summary>
    ''' <returns>Danh sách ProductDTO</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetAllProducts() As List(Of ProductDTO) Implements IProductService.GetAllProducts
        Dim products = _productRepository.GetAllProducts()
        Return MapToDTOList(products)
    End Function

    ''' <summary>
    ''' Trả về sản phẩm theo ID dưới dạng DTO
    ''' </summary>
    ''' <param name="id">Mã sản phẩm</param>
    ''' <returns>ProductDTO hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetProductById(id As Integer) As ProductDTO Implements IProductService.GetProductById
        Dim p = _productRepository.GetProductById(id)
        If p Is Nothing Then Return Nothing
        Return MapToDTO(p)
    End Function

    ''' <summary>
    ''' Trả về danh sách sản phẩm theo trang dưới dạng DTO
    ''' </summary>
    ''' <param name="pageIndex">Chỉ số trang (bắt đầu từ 0)</param>
    ''' <param name="pageSize">Kích thước trang</param>
    ''' <returns>Danh sách ProductDTO</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentException">Ném ra nếu pageIndex hoặc pageSize không hợp lệ</exception>
    Public Function GetProductsByPage(pageIndex As Integer, pageSize As Integer) As List(Of ProductDTO) Implements IProductService.GetProductsByPage
        Console.WriteLine("NCC: " + _supplierCache.ToList().ToString())
        If pageIndex < 0 Then
            Throw New ArgumentException("Chỉ số trang không được nhỏ hơn 0.", NameOf(pageIndex))
        End If
        If pageSize <= 0 Then
            Throw New ArgumentException("Kích thước trang phải lớn hơn 0.", NameOf(pageSize))
        End If
        Dim products = _productRepository.GetProductsByPage(pageIndex, pageSize)
        Return MapToDTOList(products)
    End Function

    ''' <summary>
    ''' Trả về tổng số sản phẩm
    ''' </summary>
    ''' <returns>Tổng số sản phẩm</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetTotalProductCount() As Integer Implements IProductService.GetTotalProductCount
        Return _productRepository.GetTotalProductCount()
    End Function

    ''' <summary>
    ''' Thêm mới sản phẩm
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần thêm</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu product là Nothing</exception>
    Public Function AddProduct(product As Product) As OperationResult Implements IProductService.AddProduct
        If product Is Nothing Then
            Throw New ArgumentNullException(NameOf(product), "Đối tượng Product rỗng.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(product.ProductName, "Tên sản phẩm", errors, True, 150)
        ValidationHelper.ValidateString(product.Description, "Mô tả", errors, False, 500)
        ValidationHelper.ValidateDecimal(product.Price, "Giá sản phẩm", errors, 0)
        ValidationHelper.ValidateInteger(product.Quantity, "Số lượng", errors, 0)
        ValidationHelper.ValidateInteger(product.CategoryId, "Mã danh mục", errors, 1)
        ValidationHelper.ValidateInteger(product.CreatedBy, "Mã người tạo", errors, 0)
        ValidationHelper.ValidateInteger(product.MinStockLevel, "Mức tồn tối thiểu", errors, 0)
        ValidationHelper.ValidateString(product.Unit, "Đơn vị", errors, False, 50)


        ' Kiểm tra CategoryId tồn tại
        If Not _categoryCache.ContainsKey(product.CategoryId) Then
            errors.Add("Danh mục không tồn tại.")
        End If
        If Not _supplierCache.ContainsKey(product.SupplierId) Then
            errors.Add("Nhà cung cấp không tồn tại.")
        End If
        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim newId = _productRepository.AddProduct(product)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Cập nhật sản phẩm
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần cập nhật</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu product là Nothing</exception>
    Public Function UpdateProduct(product As Product) As OperationResult Implements IProductService.UpdateProduct
        If product Is Nothing Then
            Throw New ArgumentNullException(NameOf(product), "Đối tượng Product không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(product.ProductName, "Tên sản phẩm", errors, True, 150)
        ValidationHelper.ValidateString(product.Description, "Mô tả", errors, False, 500)
        ValidationHelper.ValidateDecimal(product.Price, "Giá sản phẩm", errors, 0)
        ValidationHelper.ValidateInteger(product.Quantity, "Số lượng", errors, 0)
        ValidationHelper.ValidateInteger(product.CategoryId, "Mã danh mục", errors, 1)
        ValidationHelper.ValidateInteger(product.CreatedBy, "Mã người tạo", errors, 0)
        ValidationHelper.ValidateInteger(product.ProductId, "Mã sản phẩm", errors, 1)
        ValidationHelper.ValidateInteger(product.MinStockLevel, "Mức tồn tối thiểu", errors, 0)
        ValidationHelper.ValidateString(product.Unit, "Đơn vị", errors, False, 50)


        ' Kiểm tra CategoryId tồn tại
        If Not _categoryCache.ContainsKey(product.CategoryId) Then
            errors.Add("Danh mục không tồn tại.")
        End If

        If Not _supplierCache.ContainsKey(product.SupplierId) Then
            errors.Add("Nhà cung cấp không tồn tại.")
        End If

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim success = _productRepository.UpdateProduct(product)
        Return New OperationResult(success, Nothing)
    End Function

    ''' <summary>
    ''' Xóa sản phẩm theo ID
    ''' </summary>
    ''' <param name="id">Mã sản phẩm</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Public Function DeleteProduct(id As Integer) As Boolean Implements IProductService.DeleteProduct
        Return _productRepository.DeleteProduct(id)
    End Function


    Public Function SearchProducts(ByVal criteria As ProductSearchCriteria) As List(Of ProductDTO) Implements IProductService.SearchProducts
        If criteria Is Nothing Then
            Throw New ArgumentNullException(NameOf(criteria), "Tiêu chí tìm kiếm không được null.")
        End If
        Dim userRoleId = SessionManager.GetCurrentUser.RoleId

        Dim products = _productRepository.SearchProducts(criteria)
        Return MapToDTOList(products)
    End Function
    ''' <summary>
    ''' Chuyển đổi đối tượng Product sang ProductDTO
    ''' </summary>
    Private Function MapToDTO(p As Product) As ProductDTO
        Dim category As Category = Nothing
        Dim supplier As Supplier = Nothing

        If Not _categoryCache.TryGetValue(p.CategoryId, category) Then
            category = _categoryRepository.GetCategoryById(p.CategoryId)
            If category IsNot Nothing Then
                _categoryCache(p.CategoryId) = category
            End If
        End If

        If Not _supplierCache.TryGetValue(p.SupplierId, supplier) Then
            supplier = _supplierRepository.GetSupplierById(p.SupplierId)
            If category IsNot Nothing Then
                _supplierCache(p.SupplierId) = supplier
            End If
        End If
        Console.WriteLine("categoryID: " & p.CategoryId)
        Console.WriteLine("supplierID: " & p.SupplierId)
        Return New ProductDTO With {
        .ProductId = p.ProductId,
        .ProductName = p.ProductName,
        .Description = p.Description,
        .Unit = p.Unit,
        .MinStockLevel = p.MinStockLevel,
        .CreatedBy = p.CreatedBy,
        .CreatedAt = p.CreatedAt,
        .IsActive = p.IsActive,
        .Price = p.Price,
        .Quantity = p.Quantity,
        .CategoryName = If(category IsNot Nothing, category.CategoryName, "Không xác định"),
        .SupplierName = If(supplier IsNot Nothing, supplier.SupplierName, "Không xác định")
    }
    End Function


    ''' <summary>
    ''' Chuyển đổi danh sách Product sang danh sách ProductDTO
    ''' </summary>
    Private Function MapToDTOList(products As List(Of Product)) As List(Of ProductDTO)
        If products Is Nothing Then Return New List(Of ProductDTO)
        Return products.Select(Function(p) MapToDTO(p)).ToList()
    End Function

    Public Function GetProductStatistics(timeRange As String) As ProductStatistics Implements IProductService.GetProductStatistics
        Return _productRepository.GetProductStatistics(timeRange)
    End Function

    Public Function GetProductsBySupplierId() As List(Of ProductDTO) Implements IProductService.GetProductsBySupplierId
        Dim products = _productRepository.GetAllProducts()
        Return MapToDTOList(products)
    End Function
End Class