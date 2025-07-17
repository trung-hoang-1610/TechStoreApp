Imports System.Collections.Generic

''' <summary>
''' Lớp BLL cho các thao tác liên quan đến phiếu nhập/xuất kho.
''' </summary>
Public Class StockTransactionService
    Implements IStockTransactionService

    Private ReadOnly _transactionRepository As IStockTransactionRepository
    Private ReadOnly _productRepository As IProductRepository
    Private ReadOnly _userRepository As IUserRepository
    Private ReadOnly _supplierRepository As ISupplierRepository
    Private ReadOnly _supplierCache As Dictionary(Of Integer, Supplier)

    ''' <summary>
    ''' Khởi tạo StockTransactionService với các repository tương ứng.
    ''' </summary>
    Public Sub New(ByVal transactionRepository As IStockTransactionRepository, ByVal productRepository As IProductRepository, ByVal userRepository As IUserRepository, ByVal supplierRepository As ISupplierRepository)
        If transactionRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(transactionRepository), "TransactionRepository không được là Nothing.")
        End If
        If productRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(productRepository), "ProductRepository không được là Nothing.")
        End If
        If userRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(userRepository), "UserRepository không được là Nothing.")
        End If
        If supplierRepository Is Nothing Then
            Throw New ArgumentNullException(NameOf(supplierRepository), "SupplierRepository không được là Nothing.")
        End If
        _transactionRepository = transactionRepository
        _productRepository = productRepository
        _userRepository = userRepository
        _supplierRepository = supplierRepository
        _supplierCache = LoadSupplierCache()
    End Sub

    ''' <summary>
    ''' Tải danh sách nhà cung cấp vào bộ nhớ cache.
    ''' </summary>
    Private Function LoadSupplierCache() As Dictionary(Of Integer, Supplier)
        Dim suppliers = _supplierRepository.GetAllSuppliers()
        Return suppliers.ToDictionary(Function(s) s.SupplierId, Function(s) s)
    End Function

    ''' <summary>
    ''' Tạo phiếu nhập kho.
    ''' </summary>
    Public Function CreateStockInTransaction(ByVal transaction As StockTransactionDTO, ByVal details As List(Of StockTransactionDetailDTO)) As OperationResult Implements IStockTransactionService.CreateStockInTransaction
        If transaction Is Nothing Then
            Throw New ArgumentNullException(NameOf(transaction), "Đối tượng StockTransactionDTO không được là Nothing.")
        End If
        If details Is Nothing OrElse details.Count = 0 Then
            Throw New ArgumentNullException(NameOf(details), "Danh sách chi tiết phiếu không được rỗng.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(transaction.TransactionCode, "Mã phiếu", errors, True, 50)
        ValidationHelper.ValidateString(transaction.TransactionType, "Loại phiếu", errors, True, 10)
        ValidationHelper.ValidateInteger(transaction.CreatedBy, "Mã người tạo", errors, 1)
        ValidationHelper.ValidateInteger(transaction.SupplierId, "Mã nhà cung cấp", errors, 1)
        For Each detail In details
            ValidationHelper.ValidateInteger(detail.ProductId, "Mã sản phẩm", errors, 1)
            ValidationHelper.ValidateInteger(detail.Quantity, "Số lượng", errors, 1)
            ValidationHelper.ValidateString(detail.Note, "Ghi chú chi tiết", errors, False, 500)
        Next

        If transaction.TransactionType <> "IN" Then
            errors.Add("Loại phiếu phải là 'IN' cho phiếu nhập.")
        End If
        If Not _supplierCache.ContainsKey(transaction.SupplierId) Then
            errors.Add("Nhà cung cấp không tồn tại.")
        End If
        For Each detail In details
            If Not _productRepository.GetProductById(detail.ProductId) IsNot Nothing Then
                errors.Add($"Sản phẩm với ID {detail.ProductId} không tồn tại.")
            End If
        Next

        If errors.Count > 0 Then
            Return New OperationResult(False, Nothing, errors)
        End If

        Try
            Dim stockTransaction As New StockTransaction With {
                .TransactionCode = transaction.TransactionCode,
                .TransactionType = transaction.TransactionType,
                .Note = transaction.Note,
                .CreatedBy = transaction.CreatedBy,
                .SupplierId = transaction.SupplierId,
                .Status = "Pending"
            }
            Dim stockDetails As New List(Of StockTransactionDetail)
            For Each detail In details
                stockDetails.Add(New StockTransactionDetail With {
                    .TransactionId = 0, ' Sẽ được gán trong DAL
                    .ProductId = detail.ProductId,
                    .Quantity = detail.Quantity,
                    .Note = detail.Note
                })
            Next
            Dim transactionId = _transactionRepository.CreateTransactionWithDetails(stockTransaction, stockDetails)
            Return New OperationResult(True, transactionId, Nothing)
        Catch ex As Exception
            Return New OperationResult(False, Nothing, New List(Of String) From {ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Tạo phiếu xuất kho.
    ''' </summary>
    Public Function CreateStockOutTransaction(ByVal transaction As StockTransactionDTO, ByVal details As List(Of StockTransactionDetailDTO)) As OperationResult Implements IStockTransactionService.CreateStockOutTransaction
        If transaction Is Nothing Then
            Throw New ArgumentNullException(NameOf(transaction), "Đối tượng StockTransactionDTO không được là Nothing.")
        End If
        If details Is Nothing OrElse details.Count = 0 Then
            Throw New ArgumentNullException(NameOf(details), "Danh sách chi tiết phiếu không được rỗng.")
        End If

        Dim errors As New List(Of String)
        ValidationHelper.ValidateString(transaction.TransactionCode, "Mã phiếu", errors, True, 50)
        ValidationHelper.ValidateString(transaction.TransactionType, "Loại phiếu", errors, True, 10)
        ValidationHelper.ValidateInteger(transaction.CreatedBy, "Mã người tạo", errors, 1)
        For Each detail In details
            ValidationHelper.ValidateInteger(detail.ProductId, "Mã sản phẩm", errors, 1)
            ValidationHelper.ValidateInteger(detail.Quantity, "Số lượng", errors, 1)
            ValidationHelper.ValidateString(detail.Note, "Ghi chú chi tiết", errors, False, 500)
        Next

        If transaction.TransactionType <> "OUT" Then
            errors.Add("Loại phiếu phải là 'OUT' cho phiếu xuất.")
        End If
        If transaction.SupplierId.HasValue Then
            errors.Add("Phiếu xuất không được có nhà cung cấp.")
        End If
        For Each detail In details
            Dim product = _productRepository.GetProductById(detail.ProductId)
            If product Is Nothing Then
                errors.Add($"Sản phẩm với ID {detail.ProductId} không tồn tại.")
            ElseIf product.Quantity < detail.Quantity Then
                errors.Add($"Sản phẩm {product.ProductName} không đủ tồn kho (hiện có: {product.Quantity}, yêu cầu: {detail.Quantity}).")
            End If
        Next

        If errors.Count > 0 Then
            Return New OperationResult(False, Nothing, errors)
        End If

        Try
            Dim stockTransaction As New StockTransaction With {
                .TransactionCode = transaction.TransactionCode,
                .TransactionType = transaction.TransactionType,
                .Note = transaction.Note,
                .CreatedBy = transaction.CreatedBy,
                .SupplierId = 0,
                .Status = "Pending"
            }
            Dim stockDetails As New List(Of StockTransactionDetail)
            For Each detail In details
                stockDetails.Add(New StockTransactionDetail With {
                    .TransactionId = 0, ' Sẽ được gán trong DAL
                    .ProductId = detail.ProductId,
                    .Quantity = detail.Quantity,
                    .Note = detail.Note
                })
            Next
            Dim transactionId = _transactionRepository.CreateTransactionWithDetails(stockTransaction, stockDetails)
            Return New OperationResult(True, transactionId, Nothing)
        Catch ex As Exception
            Return New OperationResult(False, Nothing, New List(Of String) From {ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Duyệt phiếu nhập/xuất (chỉ admin).
    ''' </summary>
    Public Function ApproveTransaction(ByVal transactionId As Integer, ByVal approvedBy As Integer, ByVal isApproved As Boolean) As OperationResult Implements IStockTransactionService.ApproveTransaction
        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser Is Nothing OrElse currentUser.RoleId <> 1 Then
            Return New OperationResult(False, Nothing, New List(Of String) From {"Chỉ admin mới có quyền duyệt phiếu."})
        End If
        If transactionId <= 0 Then
            Return New OperationResult(False, Nothing, New List(Of String) From {"Mã phiếu không hợp lệ."})
        End If
        If approvedBy <= 0 Then
            Return New OperationResult(False, Nothing, New List(Of String) From {"Mã người duyệt không hợp lệ."})
        End If

        Try
            Dim success = _transactionRepository.ApproveTransaction(transactionId, approvedBy, isApproved)
            Return New OperationResult(success, transactionId, Nothing)
        Catch ex As Exception
            Return New OperationResult(False, Nothing, New List(Of String) From {ex.Message})
        End Try
    End Function

    ''' <summary>
    ''' Lấy danh sách phiếu nhập/xuất.
    ''' </summary>
    Public Function GetTransactions(ByVal transactionType As String, ByVal userId As Integer?) As List(Of StockTransactionDTO) Implements IStockTransactionService.GetTransactions
        If transactionType <> "IN" AndAlso transactionType <> "OUT" Then
            Throw New ArgumentException("Loại phiếu phải là 'IN' hoặc 'OUT'.", NameOf(transactionType))
        End If
        If userId.HasValue AndAlso userId <= 0 Then
            Throw New ArgumentException("Mã người dùng không hợp lệ.", NameOf(userId))
        End If

        Dim currentUser = SessionManager.GetCurrentUser()
        If userId.HasValue AndAlso currentUser IsNot Nothing AndAlso userId.Value <> currentUser.UserId Then
            Throw New UnauthorizedAccessException("Bạn không có quyền xem phiếu của người dùng khác.")
        End If

        Dim transactions = _transactionRepository.GetTransactions(transactionType, If(currentUser IsNot Nothing AndAlso currentUser.RoleId = 1, Nothing, userId))
        Return MapToDTOList(transactions)
    End Function

    ''' <summary>
    ''' Tìm kiếm phiếu nhập/xuất.
    ''' </summary>
    Public Function SearchTransactions(ByVal transactionType As String, ByVal userId As Integer?, ByVal criteria As SearchCriteriaDTO) As List(Of StockTransactionDTO) Implements IStockTransactionService.SearchTransactions
        If transactionType <> "IN" AndAlso transactionType <> "OUT" Then
            Throw New ArgumentException("Loại phiếu phải là 'IN' hoặc 'OUT'.", NameOf(transactionType))
        End If
        If userId.HasValue AndAlso userId <= 0 Then
            Throw New ArgumentException("Mã người dùng không hợp lệ.", NameOf(userId))
        End If
        If criteria Is Nothing Then
            Throw New ArgumentNullException(NameOf(criteria), "Tiêu chí tìm kiếm không được null.")
        End If

        Dim currentUser = SessionManager.GetCurrentUser()
        If userId.HasValue AndAlso currentUser IsNot Nothing AndAlso userId.Value <> currentUser.UserId Then
            Throw New UnauthorizedAccessException("Bạn không có quyền tìm kiếm phiếu của người dùng khác.")
        End If

        Dim searchString = String.Empty
        If Not String.IsNullOrEmpty(criteria.TransactionCode) Then
            searchString = criteria.TransactionCode
        ElseIf Not String.IsNullOrEmpty(criteria.Status) Then
            searchString = criteria.Status
        End If

        Dim transactions = _transactionRepository.SearchTransactions(transactionType, If(currentUser IsNot Nothing AndAlso currentUser.RoleId = 1, Nothing, userId), searchString)
        Return MapToDTOList(transactions)
    End Function

    ''' <summary>
    ''' Lấy chi tiết phiếu theo mã phiếu.
    ''' </summary>
    Public Function GetTransactionDetails(ByVal transactionId As Integer) As List(Of StockTransactionDetailDTO) Implements IStockTransactionService.GetTransactionDetails
        If transactionId <= 0 Then
            Throw New ArgumentException("Mã phiếu không hợp lệ.", NameOf(transactionId))
        End If

        Dim transaction = _transactionRepository.GetTransactionById(transactionId)
        If transaction Is Nothing Then
            Throw New InvalidOperationException("Phiếu không tồn tại.")
        End If

        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser IsNot Nothing AndAlso currentUser.RoleId <> 1 AndAlso transaction.CreatedBy <> currentUser.UserId Then
            Throw New UnauthorizedAccessException("Bạn không có quyền xem chi tiết phiếu này.")
        End If

        ' Gọi StockTransactionDetailDAL để lấy chi tiết phiếu
        Dim detailRepository As New StockTransactionDetailDAL()
        Dim details = detailRepository.GetTransactionDetails(transactionId)
        Return MapToDetailDTOList(details)
    End Function

    ''' <summary>
    ''' Chuyển đổi đối tượng StockTransaction sang StockTransactionDTO.
    ''' </summary>
    Private Function MapToDTO(ByVal transaction As StockTransaction) As StockTransactionDTO
        Dim createdByUser As User = _userRepository.GetUserById(transaction.CreatedBy)
        Dim approvedByUser As User = If(transaction.ApprovedBy > 0, _userRepository.GetUserById(transaction.ApprovedBy), Nothing)
        Dim supplier As Supplier = If(transaction.SupplierId > 0 AndAlso _supplierCache.ContainsKey(transaction.SupplierId), _supplierCache(transaction.SupplierId), Nothing)

        If transaction.SupplierId > 0 AndAlso supplier Is Nothing Then
            supplier = _supplierRepository.GetSupplierById(transaction.SupplierId)
            If supplier IsNot Nothing Then
                _supplierCache(transaction.SupplierId) = supplier
            End If
        End If

        Return New StockTransactionDTO With {
            .TransactionId = transaction.TransactionId,
            .TransactionCode = transaction.TransactionCode,
            .TransactionType = transaction.TransactionType,
            .Note = transaction.Note,
            .CreatedBy = transaction.CreatedBy,
            .CreatedByName = If(createdByUser IsNot Nothing, createdByUser.Username, "Không xác định"),
            .CreatedAt = transaction.CreatedAt,
            .SupplierId = If(transaction.SupplierId > 0, transaction.SupplierId, Nothing),
            .SupplierName = If(supplier IsNot Nothing, supplier.SupplierName, "Không xác định"),
            .Status = transaction.Status,
            .ApprovedBy = If(transaction.ApprovedBy > 0, transaction.ApprovedBy, Nothing),
            .ApprovedByName = If(approvedByUser IsNot Nothing, approvedByUser.Username, "Không xác định"),
            .ApprovedAt = If(transaction.ApprovedAt > DateTime.MinValue, transaction.ApprovedAt, Nothing)
        }
    End Function

    ''' <summary>
    ''' Chuyển đổi danh sách StockTransaction sang danh sách StockTransactionDTO.
    ''' </summary>
    Private Function MapToDTOList(ByVal transactions As List(Of StockTransaction)) As List(Of StockTransactionDTO)
        If transactions Is Nothing Then Return New List(Of StockTransactionDTO)
        Return transactions.Select(Function(t) MapToDTO(t)).ToList()
    End Function

    ''' <summary>
    ''' Chuyển đổi danh sách StockTransactionDetail sang danh sách StockTransactionDetailDTO.
    ''' </summary>
    Private Function MapToDetailDTOList(ByVal details As List(Of StockTransactionDetail)) As List(Of StockTransactionDetailDTO)
        If details Is Nothing Then Return New List(Of StockTransactionDetailDTO)
        Dim result As New List(Of StockTransactionDetailDTO)
        For Each detail In details
            Dim product = _productRepository.GetProductById(detail.ProductId)
            result.Add(New StockTransactionDetailDTO With {
                .DetailId = detail.DetailId,
                .TransactionId = detail.TransactionId,
                .ProductId = detail.ProductId,
                .ProductName = If(product IsNot Nothing, product.ProductName, "Không xác định"),
                .Unit = If(product IsNot Nothing, product.Unit, "Không xác định"),
                .Quantity = detail.Quantity,
                .Note = detail.Note
            })
        Next
        Return result
    End Function

    Public Function GetTransactionById(transactionId As Integer) As StockTransactionDTO Implements IStockTransactionService.GetTransactionById
        Return MapToDTO(_transactionRepository.GetTransactionById(transactionId))
    End Function
End Class
