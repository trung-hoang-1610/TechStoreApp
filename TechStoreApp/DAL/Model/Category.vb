' Model/Category.vb
Public Class Category
    ''' <summary>
    ''' Trường riêng lưu mã định danh duy nhất của danh mục
    ''' </summary>
    Private _categoryId As Integer

    ''' <summary>
    ''' Trường riêng lưu tên danh mục
    ''' </summary>
    Private _categoryName As String

    ''' <summary>
    ''' Trường riêng lưu mô tả danh mục
    ''' </summary>
    Private _description As String

    ''' <summary>
    ''' Lấy mã định danh duy nhất của danh mục
    ''' </summary>
    ''' <returns>Mã danh mục (CategoryId)</returns>
    Public ReadOnly Property CategoryId As Integer
        Get
            Return _categoryId
        End Get
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt tên danh mục, không được trùng lặp
    ''' </summary>
    ''' <returns>Tên danh mục (CategoryName)</returns>
    Public Property CategoryName As String
        Get
            Return _categoryName
        End Get
        Set(value As String)
            _categoryName = value
        End Set
    End Property

    ''' <summary>
    ''' Lấy hoặc đặt mô tả chi tiết của danh mục
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
End Class