Imports System.Threading.Tasks
Public Class CategoryService
    Implements ICategoryService

    Private ReadOnly _categoryRepository As ICategoryRepository

    ''' <summary>
    ''' Khởi tạo CategoryService với repository tương ứng
    ''' </summary>
    ''' <param name="categoryRepository">Đối tượng ICategoryRepository để truy cập dữ liệu</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu categoryRepository là Nothing</exception>
    Public Sub New(ByVal categoryRepository As ICategoryRepository)
        If categoryRepository Is Nothing Then
            Throw New ArgumentNullException("categoryRepository", "CategoryRepository không được là Nothing.")
        End If
        _categoryRepository = categoryRepository
    End Sub

    ''' <summary>
    ''' Lấy danh sách tất cả danh mục
    ''' </summary>
    ''' <returns>Danh sách các đối tượng Category</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetAllCategoriesAsync() As Task(Of List(Of Category)) Implements ICategoryService.GetAllCategoriesAsync
        Return Await _categoryRepository.GetAllCategoriesAsync()
    End Function

    ''' <summary>
    ''' Lấy danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>Đối tượng Category hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Async Function GetCategoryByIdAsync(ByVal id As Integer) As Task(Of Category) Implements ICategoryService.GetCategoryByIdAsync
        Return Await _categoryRepository.GetCategoryByIdAsync(id)
    End Function

    ''' <summary>
    ''' Thêm danh mục mới
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần thêm</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số category là Nothing</exception>
    Public Async Function AddCategoryAsync(ByVal category As Category) As Task(Of OperationResult) Implements ICategoryService.AddCategoryAsync
        If category Is Nothing Then
            Throw New ArgumentNullException("category", "Đối tượng Category không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(category.CategoryName, "Tên danh mục", errors, True, 100)
        ValidationHelper.ValidateString(category.Description, "Mô tả", errors, False, 500)

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim newId As Integer = Await _categoryRepository.AddCategoryAsync(category)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Cập nhật thông tin danh mục
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần cập nhật</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số category là Nothing</exception>
    Public Async Function UpdateCategoryAsync(ByVal category As Category) As Task(Of OperationResult) Implements ICategoryService.UpdateCategoryAsync
        If category Is Nothing Then
            Throw New ArgumentNullException("category", "Đối tượng Category không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(category.CategoryName, "Tên danh mục", errors, True, 100)
        ValidationHelper.ValidateString(category.Description, "Mô tả", errors, False, 500)
        ValidationHelper.ValidateInteger(category.CategoryId, "Mã danh mục", errors, 1)

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim success As Boolean = Await _categoryRepository.UpdateCategoryAsync(category)
        Return New OperationResult(success, Nothing)
    End Function

    ''' <summary>
    ''' Xóa danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Public Async Function DeleteCategoryAsync(ByVal id As Integer) As Task(Of Boolean) Implements ICategoryService.DeleteCategoryAsync
        Return Await _categoryRepository.DeleteCategoryAsync(id)
    End Function
End Class