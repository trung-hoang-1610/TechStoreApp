''' <summary>
''' Đại diện cho danh mục sản phẩm.
''' </summary>
Public Class Category
    Private _categoryId As Integer
    Private _categoryName As String
    Private _description As String

    ''' <summary>
    ''' Mã định danh của danh mục.
    ''' </summary>
    Public Property CategoryId() As Integer
        Get
            Return _categoryId
        End Get
        Set(ByVal value As Integer)
            _categoryId = value
        End Set
    End Property

    ''' <summary>
    ''' Tên danh mục.
    ''' </summary>
    Public Property CategoryName() As String
        Get
            Return _categoryName
        End Get
        Set(ByVal value As String)
            _categoryName = value
        End Set
    End Property

    ''' <summary>
    ''' Mô tả danh mục.
    ''' </summary>
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property
End Class
