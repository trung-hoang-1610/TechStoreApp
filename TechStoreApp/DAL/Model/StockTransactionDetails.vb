''' <summary>
''' Đại diện cho chi tiết phiếu nhập/xuất kho trong hệ thống.
''' </summary>
Public Class StockTransactionDetail
    Private _detailId As Integer
    Private _transactionId As Integer
    Private _productId As Integer
    Private _quantity As Integer
    Private _note As String

    ''' <summary>
    ''' Mã định danh duy nhất của chi tiết phiếu.
    ''' </summary>
    Public ReadOnly Property DetailId() As Integer
        Get
            Return _detailId
        End Get
    End Property

    ''' <summary>
    ''' Gán mã định danh cho chi tiết phiếu (chỉ sử dụng nội bộ từ DAL).
    ''' </summary>
    Friend Sub SetDetailId(ByVal id As Integer)
        _detailId = id
    End Sub

    ''' <summary>
    ''' Mã phiếu nhập/xuất.
    ''' </summary>
    Public Property TransactionId() As Integer
        Get
            Return _transactionId
        End Get
        Set(ByVal value As Integer)
            _transactionId = value
        End Set
    End Property

    ''' <summary>
    ''' Mã sản phẩm.
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
    ''' Số lượng sản phẩm trong chi tiết phiếu.
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
    ''' Ghi chú của chi tiết phiếu.
    ''' </summary>
    Public Property Note() As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property
End Class