' Model/Product.vb
Public Class Product
    ''' <summary>
    ''' Trường riêng lưu mã định danh duy nhất của sản phẩm
    ''' </summary>
    Private _productId As Integer

    ''' <summary>
    ''' Trường riêng lưu tên sản phẩm
    ''' </summary>
    Private _productName As String

    ''' <summary>
    ''' Trường riêng lưu mô tả sản phẩm
    ''' </summary>
    Private _description As String

    ''' <summary>
    ''' Trường riêng lưu giá sản phẩm
    ''' </summary>
    Private _price As Decimal

    ''' <summary>
    ''' Trường riêng lưu số lượng sản phẩm
    ''' </summary>
    Private _quantity As Integer

    ''' <summary>
    ''' Trường riêng lưu mã danh mục
    ''' </summary>
    Private _categoryId As Integer

    ''' <summary>
    ''' Trường riêng lưu mã người tạo
    ''' </summary>
    Private _createdBy As Integer

    ''' <summary>
    ''' Trường riêng lưu thời điểm tạo
    ''' </summary>
    Private _createdAt As DateTime

    ''' <summary>
    ''' Lấy mã định danh duy nhất của sản phẩm
    ''' </summary>
    ''' <returns>Mã sản phẩm (ProductId)</returns>
    Public ReadOnly Property ProductId As Integer
        Get
            Return _productId
        End Get
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt tên sản phẩm
    ''' </summary>
    ''' <returns>Tên sản phẩm (ProductName)</returns>
    Public Property ProductName As String
        Get
            Return _productName
        End Get
        Set(value As String)
            _productName = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt mô tả chi tiết của sản phẩm
    ''' </summary>
    ''' <returns>Mô tả (Description)</returns>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt giá sản phẩm
    ''' </summary>
    ''' <returns>Giá sản phẩm (Price)</returns>
    Public Property Price As Decimal
        Get
            Return _price
        End Get
        Set(value As Decimal)
            _price = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt số lượng sản phẩm
    ''' </summary>
    ''' <returns>Số lượng (Quantity)</returns>
    Public Property Quantity As Integer
        Get
            Return _quantity
        End Get
        Set(value As Integer)
            _quantity = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt mã danh mục của sản phẩm
    ''' </summary>
    ''' <returns>Mã danh mục (CategoryId)</returns>
    Public Property CategoryId As Integer
        Get
            Return _categoryId
        End Get
        Set(value As Integer)
            _categoryId = value
        End Set
    End Property


    ''' <summary>
    ''' Lấy hoặc đặt mã người tạo sản phẩm
    ''' </summary>
    ''' <returns>Mã người tạo (CreatedBy)</returns>
    Public Property CreatedBy As Integer
        Get
            Return _createdBy
        End Get
        Set(value As Integer)
            _createdBy = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy thời điểm tạo sản phẩm
    ''' </summary>
    ''' <returns>Thời điểm tạo (CreatedAt)</returns>
    Public ReadOnly Property CreatedAt As DateTime
        Get
            Return _createdAt
        End Get
    End Property
End Class