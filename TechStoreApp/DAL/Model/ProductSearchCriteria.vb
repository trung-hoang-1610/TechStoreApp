Public Class ProductSearchCriteria
    Public Property Name As String
    Public Property CategoryId As Integer?
    Public Property MinPrice As Decimal?
    Public Property MaxPrice As Decimal?
    Public Property IsActive As Boolean?
    Public Property PageIndex As Integer
    Public Property PageSize As Integer
    Public Property SortBy As String
    Public Property TotalCount As Integer
End Class