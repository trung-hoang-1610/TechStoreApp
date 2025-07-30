Imports System.Threading.Tasks
Public Class SupplierService
    Implements ISupplierService

    Private ReadOnly _SupplierRepository As ISupplierRepository

    ''' <summary>
    ''' Khởi tạo SupplierService với repository tương ứng
    ''' </summary>
    ''' <param name="SupplierRepository">Đối tượng ISupplierRepository để truy cập dữ liệu</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu SupplierRepository là Nothing</exception>
    Public Sub New(ByVal SupplierRepository As ISupplierRepository)
        If SupplierRepository Is Nothing Then
            Throw New ArgumentNullException("SupplierRepository", "SupplierRepository không được là Nothing.")
        End If
        _SupplierRepository = SupplierRepository
    End Sub

    ''' <summary>
    ''' Lấy danh sách tất cả danh mục
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Supplier</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetAllSuppliers() As Task(Of List(Of Supplier)) Implements ISupplierService.GetAllSuppliers
        Return Await _SupplierRepository.GetAllSuppliersAsync()
    End Function

    ''' <summary>
    ''' Lấy danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>Đối tượng Supplier hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetSupplierById(ByVal id As Integer) As Task(Of Supplier) Implements ISupplierService.GetSupplierById
        Return Await _SupplierRepository.GetSupplierByIdAsync(id)
    End Function

    ''' <summary>
    ''' Thêm danh mục mới
    ''' </summary>
    ''' <param name="Supplier">Đối tượng Supplier cần thêm</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số Supplier là Nothing</exception>
    Public Async Function AddSupplier(ByVal Supplier As Supplier) As Task(Of OperationResult) Implements ISupplierService.AddSupplier
        If Supplier Is Nothing Then
            Throw New ArgumentNullException("Supplier", "Đối tượng Supplier không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(Supplier.SupplierName, "Tên danh mục", errors, True, 100)
        ValidationHelper.ValidateString(Supplier.ContactInfo, "Thông tin liên hệ", errors, False, 500)

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim newId As Integer = Await _SupplierRepository.AddSupplierAsync(Supplier)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Cập nhật thông tin danh mục
    ''' </summary>
    ''' <param name="Supplier">Đối tượng Supplier cần cập nhật</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số Supplier là Nothing</exception>
    Public Async Function UpdateSupplier(ByVal Supplier As Supplier) As Task(Of OperationResult) Implements ISupplierService.UpdateSupplier
        If Supplier Is Nothing Then
            Throw New ArgumentNullException("Supplier", "Đối tượng Supplier không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(Supplier.SupplierName, "Tên nhà cung cấp", errors, True, 100)
        ValidationHelper.ValidateString(Supplier.ContactInfo, "Thông tin liên lạc", errors, False, 500)
        ValidationHelper.ValidateInteger(Supplier.SupplierId, "Mã nhà cung cấp", errors, 1)

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim success As Boolean = Await _SupplierRepository.UpdateSupplierAsync(Supplier)
        Return New OperationResult(success, Nothing)
    End Function

    ''' <summary>
    ''' Xóa danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Public Async Function DeleteSupplier(ByVal id As Integer) As Task(Of Boolean) Implements ISupplierService.DeleteSupplier
        Return Await _SupplierRepository.DeleteSupplierAsync(id)
    End Function
End Class