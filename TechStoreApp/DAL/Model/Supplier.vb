''' <summary>
''' Đại diện cho nhà cung cấp sản phẩm.
''' </summary>
Public Class Supplier
    Private _supplierId As Integer
    Private _supplierName As String
    Private _contactInfo As String

    ''' <summary>
    ''' Mã định danh của nhà cung cấp.
    ''' </summary>
    Public Property SupplierId() As Integer
        Get
            Return _supplierId
        End Get
        Set(ByVal value As Integer)
            _supplierId = value
        End Set
    End Property

    ''' <summary>
    ''' Tên nhà cung cấp.
    ''' </summary>
    Public Property SupplierName() As String
        Get
            Return _supplierName
        End Get
        Set(ByVal value As String)
            _supplierName = value
        End Set
    End Property

    ''' <summary>
    ''' Thông tin liên hệ nhà cung cấp.
    ''' </summary>
    Public Property ContactInfo() As String
        Get
            Return _contactInfo
        End Get
        Set(ByVal value As String)
            _contactInfo = value
        End Set
    End Property
End Class
