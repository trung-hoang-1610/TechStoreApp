' BLL/ProductService.vb
Imports System.Threading.Tasks

Public Class ProductService
    Implements IProductService

    Private ReadOnly _productRepository As IProductRepository
    Private ReadOnly _categoryRepository As ICategoryRepository
    Private ReadOnly _supplierRepository As ISupplierRepository
    Private ReadOnly _userRepository As IUserRepository
    Private _categoryCache As Dictionary(Of Integer, Category)
    Private _supplierCache As Dictionary(Of Integer, Supplier)
    Private _userCache As Dictionary(Of Integer, User)

    ''' <summary>
    ''' Khởi tạo ProductService với repository tương ứng
    ''' </summary>
    ''' <param name="productRepository">Đối tượng IProductRepository</param>
    ''' <param name="categoryRepository">Đối tượng ICategoryRepository</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu productRepository hoặc categoryRepository là Nothing</exception>
    Public Sub New(ByVal productRepository As IProductRepository, ByVal categoryRepository As ICategoryRepository, ByVal supplierRepository As ISupplierRepository, ByVal userRepository As IUserRepository)
        If productRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(productRepository), "ProductRepository không được là Nothing.")
        End If
        If categoryRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(categoryRepository), "CategoryRepository không được là Nothing.")
        End If
        _productRepository = productRepository
        _categoryRepository = categoryRepository
        _supplierRepository = supplierRepository
        _userRepository = userRepository

        _categoryCache = New Dictionary(Of Integer, Category)
        _supplierCache = New Dictionary(Of Integer, Supplier)
        _userCache = New Dictionary(Of Integer, User)
    End Sub

    ''' <summary>
    ''' Tải danh sách danh mục vào bộ nhớ cache để cải thiện hiệu suất
    ''' </summary>
    ''' <returns>Danh sách danh mục dưới dạng Dictionary</returns>
    Private Async Function LoadCategoryCache() As Task(Of Dictionary(Of Integer, Category))

        Dim categories = Await _categoryRepository.GetAllCategoriesAsync()
        Return categories.ToDictionary(Function(c) c.CategoryId, Function(c) c)

    End Function

    Private Async Function LoadSupplierCache() As Task(Of Dictionary(Of Integer, Supplier))

        Dim suppliers = Await _supplierRepository.GetAllSuppliersAsync()
        Return suppliers.ToDictionary(Function(s) s.SupplierId, Function(s) s)

    End Function
    Private Async Function LoadUserCache() As Task(Of Dictionary(Of Integer, User))

        Dim users = Await _userRepository.GetAllUsersAsync()
        Return users.ToDictionary(Function(u) u.UserId, Function(u) u)

    End Function
    ''' <summary>
    ''' Trả về danh sách tất cả sản phẩm dưới dạng DTO
    ''' </summary>
    ''' <returns>Danh sách ProductDTO</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetAllProducts() As Task(Of List(Of ProductDTO)) Implements IProductService.GetAllProductsAsync
        Dim products = Await _productRepository.GetAllProductsAsync()
        Return Await MapToDTOList(products)
    End Function

    ''' <summary>
    ''' Trả về sản phẩm theo ID dưới dạng DTO
    ''' </summary>
    ''' <param name="id">Mã sản phẩm</param>
    ''' <returns>ProductDTO hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetProductById(id As Integer) As Task(Of ProductDTO) Implements IProductService.GetProductByIdAsync
        Dim p = Await _productRepository.GetProductByIdAsync(id)
        If p Is Nothing Then Return Nothing
        Return Await MapToDTO(p)
    End Function

    ''' <summary>
    ''' Trả về danh sách sản phẩm theo trang dưới dạng DTO
    ''' </summary>
    ''' <param name="pageIndex">Chỉ số trang (bắt đầu từ 0)</param>
    ''' <param name="pageSize">Kích thước trang</param>
    ''' <returns>Danh sách ProductDTO</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentException">Ném ra nếu pageIndex hoặc pageSize không hợp lệ</exception>
    Public Async Function GetProductsByPage(pageIndex As Integer, pageSize As Integer) As Task(Of List(Of ProductDTO)) Implements IProductService.GetProductsByPageAsync
        Console.WriteLine("NCC: " + _supplierCache.ToList().ToString())
        If pageIndex < 0 Then
            Throw New ArgumentException("Chỉ số trang không được nhỏ hơn 0.", NameOf(pageIndex))
        End If
        If pageSize <= 0 Then
            Throw New ArgumentException("Kích thước trang phải lớn hơn 0.", NameOf(pageSize))
        End If
        Dim products = Await _productRepository.GetProductsByPageAsync(pageIndex, pageSize)
        Return Await MapToDTOList(products)
    End Function

    ''' <summary>
    ''' Trả về tổng số sản phẩm
    ''' </summary>
    ''' <returns>Tổng số sản phẩm</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetTotalProductCount() As Task(Of Integer) Implements IProductService.GetTotalProductCountAsync
        Return Await _productRepository.GetTotalProductCountAsync()
    End Function

    ''' <summary>
    ''' Thêm mới sản phẩm
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần thêm</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu product là Nothing</exception>
    Public Async Function AddProduct(product As Product) As Task(Of OperationResult) Implements IProductService.AddProductAsync
        _categoryCache = Await LoadCategoryCache()
        _supplierCache = Await LoadSupplierCache()
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

        Dim newId = Await _productRepository.AddProductAsync(product)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Cập nhật sản phẩm
    ''' </summary>
    ''' <param name="product">Đối tượng Product cần cập nhật</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu product là Nothing</exception>
    Public Async Function UpdateProduct(product As Product) As Task(Of OperationResult) Implements IProductService.UpdateProductAsync
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

        _categoryCache = Await LoadCategoryCache()
        _supplierCache = Await LoadSupplierCache()
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

        Dim success = Await _productRepository.UpdateProductAsync(product)
        Return New OperationResult(success, Nothing)
    End Function

    ''' <summary>
    ''' Xóa sản phẩm theo ID
    ''' </summary>
    ''' <param name="id">Mã sản phẩm</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Public Async Function DeleteProduct(id As Integer) As Task(Of Boolean) Implements IProductService.DeleteProductAsync
        Return Await _productRepository.DeleteProductAsync(id)
    End Function


    Public Async Function SearchProducts(ByVal criteria As ProductSearchCriteriaDTO) As Task(Of List(Of ProductDTO)) Implements IProductService.SearchProductsAsync
        If criteria Is Nothing Then
            Throw New ArgumentNullException(NameOf(criteria), "Tiêu chí tìm kiếm không được null.")
        End If
        Dim userRoleId = SessionManager.GetCurrentUser.RoleId

        Dim products = Await _productRepository.SearchProductsAsync(criteria)
        Return Await MapToDTOList(products)
    End Function
    ''' <summary>
    ''' Chuyển đổi đối tượng Product sang ProductDTO
    ''' </summary>
    Private Async Function MapToDTO(p As Product) As Task(Of ProductDTO)
        Dim category As Category = Nothing
        Dim supplier As Supplier = Nothing
        Dim user As User = Nothing
        If Not _categoryCache.TryGetValue(p.CategoryId, category) Then
            category = Await _categoryRepository.GetCategoryByIdAsync(p.CategoryId)
            If category IsNot Nothing Then
                _categoryCache(p.CategoryId) = category
            End If
        End If

        If Not _supplierCache.TryGetValue(p.SupplierId, supplier) Then
            supplier = Await _supplierRepository.GetSupplierByIdAsync(p.SupplierId)
            If category IsNot Nothing Then
                _supplierCache(p.SupplierId) = supplier
            End If
        End If

        If Not _userCache.TryGetValue(p.CreatedBy, user) Then
            user = Await _userRepository.GetUserByIdAsync(p.CreatedBy)
            If user IsNot Nothing Then
                _userCache(p.CreatedBy) = user
            End If
        End If

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
            .SupplierName = If(supplier IsNot Nothing, supplier.SupplierName, "Không xác định"),
            .CreatedByName = If(user IsNot Nothing, user.Username, "Không xác định")
        }
    End Function


    ''' <summary>
    ''' Chuyển đổi danh sách Product sang danh sách ProductDTO
    ''' </summary>
    Private Async Function MapToDTOList(products As List(Of Product)) As Task(Of List(Of ProductDTO))
        If products Is Nothing Then Return New List(Of ProductDTO)

        Dim dtoTasks As IEnumerable(Of Task(Of ProductDTO)) = products.Select(Function(p) MapToDTO(p))
        Dim dtoList As ProductDTO() = Await Task.WhenAll(dtoTasks)

        Return dtoList.ToList()
    End Function

    Public Async Function GetProductStatistics(timeRange As String) As Task(Of ProductStatisticsDTO) Implements IProductService.GetProductStatisticsAsync
        Return Await _productRepository.GetProductStatisticsAsync(timeRange)
    End Function

    Public Async Function GetProductsBySupplierId(supplierId As Integer) As Task(Of List(Of ProductDTO)) Implements IProductService.GetProductsBySupplierIdAsync
        Dim products = Await _productRepository.GetProductsBySupplierIdAsync(supplierId)
        Return Await MapToDTOList(products)
    End Function
End Class