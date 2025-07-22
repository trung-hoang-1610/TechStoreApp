''' <summary>
''' Data Transfer Object cho nhà cung cấp, dùng để truyền dữ liệu giữa các tầng.
''' </summary>
Public Class SupplierDTO
    ''' <summary>
    ''' ID của nhà cung cấp.
    ''' </summary>
    Public Property SupplierId As Integer

    ''' <summary>
    ''' Tặ của nhà cung cấp.
    ''' </summary>
    Public Property SupplierName As String

    ''' <summary>
    ''' Thông tin liên hệ của nhà cung cấp.
    ''' </summary>
    Public Property ContactInfo As String

    ''' <summary>
    ''' Khởi tạo một SupplierDTO mới với các giá trị mặc định.
    ''' </summary>
    Public Sub New()
        SupplierId = 0
        SupplierName = String.Empty
        ContactInfo = String.Empty
    End Sub

    ''' <summary>
    ''' Khởi tạo một SupplierDTO với các giá trị được chỉ định.
    ''' </summary>
    ''' <param name="supplierId">ID của nhà cung cấp.</param>
    ''' <param name="supplierName">Tên của nhà cung cấp.</param>
    ''' <param name="contactInfo">Thông tin liên hệ của nhà cung cấp.</param>
    Public Sub New(ByVal supplierId As Integer, ByVal supplierName As String, ByVal contactInfo As String)
        Me.SupplierId = supplierId
        Me.SupplierName = supplierName
        Me.ContactInfo = contactInfo
    End Sub
End Class
