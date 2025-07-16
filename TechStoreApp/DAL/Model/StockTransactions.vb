''' <summary>
''' Đại diện cho phiếu nhập/xuất kho trong hệ thống.
''' </summary>
Public Class StockTransaction
    Private _transactionId As Integer
    Private _transactionCode As String
    Private _transactionType As String
    Private _note As String
    Private _createdBy As Integer
    Private _createdAt As DateTime
    Private _approvedBy As Integer
    Private _approvedAt As DateTime
    Private _status As String
    Private _supplierId As Integer

    ''' <summary>
    ''' Mã định danh duy nhất của phiếu nhập/xuất.
    ''' </summary>
    Public ReadOnly Property TransactionId() As Integer
        Get
            Return _transactionId
        End Get
    End Property

    ''' <summary>
    ''' Gán mã định danh cho phiếu (chỉ sử dụng nội bộ từ DAL).
    ''' </summary>
    Friend Sub SetTransactionId(ByVal id As Integer)
        _transactionId = id
    End Sub

    ''' <summary>
    ''' Mã phiếu nhập/xuất.
    ''' </summary>
    Public Property TransactionCode() As String
        Get
            Return _transactionCode
        End Get
        Set(ByVal value As String)
            _transactionCode = value
        End Set
    End Property

    ''' <summary>
    ''' Loại phiếu (IN: Nhập kho, OUT: Xuất kho).
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
    ''' Ghi chú của phiếu.
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
    ''' Mã người dùng tạo phiếu.
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
    ''' Thời điểm tạo phiếu.
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
    ''' Mã người dùng phê duyệt phiếu.
    ''' </summary>
    Public Property ApprovedBy() As Integer
        Get
            Return _approvedBy
        End Get
        Set(ByVal value As Integer)
            _approvedBy = value
        End Set
    End Property

    ''' <summary>
    ''' Thời điểm phê duyệt phiếu.
    ''' </summary>
    Public Property ApprovedAt() As DateTime
        Get
            Return _approvedAt
        End Get
        Set(ByVal value As DateTime)
            _approvedAt = value
        End Set
    End Property

    ''' <summary>
    ''' Trạng thái của phiếu (Pending, Approved, Rejected).
    ''' </summary>
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    ''' <summary>
    ''' Mã nhà cung cấp (dành cho phiếu nhập).
    ''' </summary>
    Public Property SupplierId() As Integer
        Get
            Return _supplierId
        End Get
        Set(ByVal value As Integer)
            _supplierId = value
        End Set
    End Property
End Class