''' <summary>
''' Đại diện cho một giao dịch nhập hoặc xuất kho.
''' </summary>
Public Class InventoryTransaction
    Private _inventoryTransactionId As Integer
    Private _productId As Integer
    Private _quantity As Integer
    Private _transactionType As String
    Private _note As String
    Private _performedBy As Integer
    Private _performedAt As DateTime
    Private _status As String

    ''' <summary>
    ''' Mã giao dịch.
    ''' </summary>
    Public Property TransactionId() As Integer
        Get
            Return _inventoryTransactionId
        End Get
        Friend Set(ByVal value As Integer) ' Chỉ cho phép gán nội bộ
            _inventoryTransactionId = value
        End Set
    End Property
    ''' <summary>
    ''' Mã sản phẩm liên quan.
    ''' </summary>
    Public Property ProductId() As Integer
        Get
            Return _productId
        End Get
        Set(ByVal value As Integer)
            _productId = value
        End Set
    End Property

    ''' <summary>
    ''' Số lượng giao dịch. Dương: nhập, Âm: xuất.
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
    ''' Loại giao dịch (IN, OUT).
    ''' </summary>
    Public Property TransactionType() As String
        Get
            Return _transactionType
        End Get
        Set(ByVal value As String)
            _transactionType = value
        End Set
    End Property

    ''' <summary>
    ''' Ghi chú thêm cho giao dịch.
    ''' </summary>
    Public Property Note() As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property

    ''' <summary>
    ''' Mã người thực hiện giao dịch.
    ''' </summary>
    Public Property PerformedBy() As Integer
        Get
            Return _performedBy
        End Get
        Set(ByVal value As Integer)
            _performedBy = value
        End Set
    End Property

    ''' <summary>
    ''' Thời gian thực hiện giao dịch.
    ''' </summary>
    Public Property PerformedAt() As DateTime
        Get
            Return _performedAt
        End Get
        Set(ByVal value As DateTime)
            _performedAt = value
        End Set
    End Property

    ''' <summary>
    ''' Trạng thái duyệt của giao dịch (Pending, Approved, Rejected).
    ''' </summary>
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property
End Class
