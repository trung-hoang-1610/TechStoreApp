''' <summary>
''' Đại diện cho sản phẩm trong hệ thống.
''' </summary>
Public Class Product
    Private _productId As Integer
    Private _productName As String
    Private _description As String
    Private _unit As String
    Private _price As Decimal
    Private _quantity As Integer
    Private _minStockLevel As Integer
    Private _categoryId As Integer
    Private _supplierId As Integer
    Private _createdBy As Integer
    Private _createdAt As DateTime
    Private _isActive As Boolean


    ''' <summary>
    ''' Mã định danh duy nhất của sản phẩm.
    ''' </summary>
    Public ReadOnly Property ProductId() As Integer
        Get
            Return _productId
        End Get
    End Property

    ''' <summary>
    ''' Gán mã định danh cho sản phẩm (chỉ sử dụng nội bộ từ DAL).
    ''' </summary>
    ''' <param name="id">ID sản phẩm</param>
    Friend Sub SetProductId(ByVal id As Integer)
        _productId = id
    End Sub
    ''' <summary>
    ''' Tên sản phẩm.
    ''' </summary>
    Public Property ProductName() As String
        Get
            Return _productName
        End Get
        Set(ByVal value As String)
            _productName = value
        End Set
    End Property

    ''' <summary>
    ''' Mô tả chi tiết sản phẩm.
    ''' </summary>
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    ''' <summary>
    ''' Đơn vị tính (Chiếc, Hộp, ...).
    ''' </summary>
    Public Property Unit() As String
        Get
            Return _unit
        End Get
        Set(ByVal value As String)
            _unit = value
        End Set
    End Property

    ''' <summary>
    ''' Giá sản phẩm.
    ''' </summary>
    Public Property Price() As Decimal
        Get
            Return _price
        End Get
        Set(ByVal value As Decimal)
            _price = value
        End Set
    End Property

    ''' <summary>
    ''' Số lượng tồn kho.
    ''' </summary>
    Public Property Quantity() As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            _quantity = value
        End Set
    End Property

    ''' <summary>
    ''' Ngưỡng cảnh báo tồn kho thấp.
    ''' </summary>
    Public Property MinStockLevel() As Integer
        Get
            Return _minStockLevel
        End Get
        Set(ByVal value As Integer)
            _minStockLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Mã danh mục chứa sản phẩm.
    ''' </summary>
    Public Property CategoryId() As Integer
        Get
            Return _categoryId
        End Get
        Set(ByVal value As Integer)
            _categoryId = value
        End Set
    End Property

    Public Property SupplierId() As Integer
        Get
            Return _supplierId
        End Get
        Set(ByVal value As Integer)
            _supplierId = value
        End Set
    End Property

    ''' <summary>
    ''' Mã người dùng tạo sản phẩm.
    ''' </summary>
    Public Property CreatedBy() As Integer
        Get
            Return _createdBy
        End Get
        Set(ByVal value As Integer)
            _createdBy = value
        End Set
    End Property

    ''' <summary>
    ''' Ngày tạo sản phẩm.
    ''' </summary>
    Public Property CreatedAt() As DateTime
        Get
            Return _createdAt
        End Get
        Set(ByVal value As DateTime)
            _createdAt = value
        End Set
    End Property

    ''' <summary>
    ''' Trạng thái hoạt động của sản phẩm (True: đang hoạt động).
    ''' </summary>
    Public Property IsActive() As Boolean
        Get
            Return _isActive
        End Get
        Set(ByVal value As Boolean)
            _isActive = value
        End Set
    End Property
End Class
