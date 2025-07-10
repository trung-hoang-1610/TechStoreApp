' BLL/CategoryService.vb
Imports Common
Imports Common.Utilities
Imports Model
Imports DAL.Interfaces
Imports BLL.Interfaces

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
    Public Function GetAllCategories() As List(Of Category) Implements ICategoryService.GetAllCategories
        Return _categoryRepository.GetAllCategories()
    End Function

    ''' <summary>
    ''' Lấy danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>Đối tượng Category hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetCategoryById(ByVal id As Integer) As Category Implements ICategoryService.GetCategoryById
        Return _categoryRepository.GetCategoryById(id)
    End Function

    ''' <summary>
    ''' Thêm danh mục mới
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần thêm</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số category là Nothing</exception>
    Public Function AddCategory(ByVal category As Category) As OperationResult Implements ICategoryService.AddCategory
        If category Is Nothing Then
            Throw New ArgumentNullException("category", "Đối tượng Category không được là Nothing.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(category.CategoryName, "Tên danh mục", errors, True, 100)
        ValidationHelper.ValidateString(category.Description, "Mô tả", errors, False, 500)

        If errors.Count > 0 Then
            Return New OperationResult(False, errors)
        End If

        Dim newId As Integer = _categoryRepository.AddCategory(category)
        Return New OperationResult(newId > 0, Nothing)
    End Function

    ''' <summary>
    ''' Cập nhật thông tin danh mục
    ''' </summary>
    ''' <param name="category">Đối tượng Category cần cập nhật</param>
    ''' <returns>OperationResult chứa trạng thái thành công và danh sách lỗi (nếu có)</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi cập nhật cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu tham số category là Nothing</exception>
    Public Function UpdateCategory(ByVal category As Category) As OperationResult Implements ICategoryService.UpdateCategory
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

        Dim success As Boolean = _categoryRepository.UpdateCategory(category)
        Return New OperationResult(success, Nothing)
    End Function

    ''' <summary>
    ''' Xóa danh mục theo mã định danh
    ''' </summary>
    ''' <param name="id">Mã định danh của danh mục</param>
    ''' <returns>True nếu xóa thành công, False nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi xóa khỏi cơ sở dữ liệu</exception>
    Public Function DeleteCategory(ByVal id As Integer) As Boolean Implements ICategoryService.DeleteCategory
        Return _categoryRepository.DeleteCategory(id)
    End Function
End Class