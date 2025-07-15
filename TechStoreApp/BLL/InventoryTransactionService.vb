

''' <summary>
''' Dịch vụ quản lý giao dịch kho, xử lý các thao tác liên quan đến nhập/xuất kho.
''' </summary>
Public Class InventoryTransactionService
    Implements IInventoryTransactionService

    Private ReadOnly _transactionRepo As IInventoryTransactionRepository
    Private ReadOnly _productRepo As IProductRepository
    Private ReadOnly _userRepo As IUserRepository

    ''' <summary>
    ''' Khởi tạo một phiên bản mới của InventoryTransactionService.
    ''' </summary>
    ''' <param name="transactionRepo">Repository cho giao dịch kho.</param>
    ''' <param name="productRepo">Repository cho sản phẩm.</param>
    ''' <param name="userRepo">Repository cho người dùng.</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu bất kỳ repository nào là null.</exception>
    Public Sub New(ByVal transactionRepo As IInventoryTransactionRepository, ByVal productRepo As IProductRepository, ByVal userRepo As IUserRepository)
        If transactionRepo Is Nothing Then
            Throw New ArgumentNullException("transactionRepo", "Repository giao dịch không được null.")
        End If
        If productRepo Is Nothing Then
            Throw New ArgumentNullException("productRepo", "Repository sản phẩm không được null.")
        End If
        If userRepo Is Nothing Then
            Throw New ArgumentNullException("userRepo", "Repository người dùng không được null.")
        End If

        _transactionRepo = transactionRepo
        _productRepo = productRepo
        _userRepo = userRepo
    End Sub

    ''' <summary>
    ''' Tạo một giao dịch kho mới (nhập hoặc xuất).
    ''' </summary>
    ''' <param name="transaction">DTO chứa thông tin giao dịch.</param>
    ''' <returns>True nếu tạo giao dịch thành công, ngược lại False.</returns>
    ''' <exception cref="ArgumentNullException">Ném ra nếu transaction là null.</exception>
    ''' <exception cref="ArgumentException">Ném ra nếu dữ liệu đầu vào không hợp lệ.</exception>
    ''' <exception cref="InvalidOperationException">Ném ra nếu sản phẩm hoặc người thực hiện không tồn tại.</exception>
    Public Function CreateTransaction(ByVal transaction As InventoryTransactionInputDto) As Boolean Implements IInventoryTransactionService.CreateTransaction
        If transaction Is Nothing Then
            Throw New ArgumentNullException("transaction", "Thông tin giao dịch không được null.")
        End If

        ' Kiểm tra dữ liệu đầu vào
        Dim errors As New List(Of String)
        ValidateTransactionInput(transaction, errors)

        If errors.Count > 0 Then
            Throw New ArgumentException(String.Join("; ", errors.ToArray()))
        End If

        ' Kiểm tra tồn tại của sản phẩm và người dùng
        Dim product = _productRepo.GetProductById(transaction.ProductId)
        If product Is Nothing Then
            Throw New InvalidOperationException("Sản phẩm với ID " & transaction.ProductId & " không tồn tại.")
        End If

        Dim user = _userRepo.GetUserById(transaction.PerformedBy)
        If user Is Nothing Then
            Throw New InvalidOperationException("Người dùng với ID " & transaction.PerformedBy & " không tồn tại.")
        End If

        ' Tạo model giao dịch
        Dim model As New InventoryTransaction
        With model
            .ProductId = transaction.ProductId
            .Quantity = If(transaction.TransactionType = "Export", -Math.Abs(transaction.Quantity), Math.Abs(transaction.Quantity))
            .TransactionType = transaction.TransactionType
            .Note = If(transaction.Note Is Nothing, String.Empty, transaction.Note.Trim())
            .PerformedBy = transaction.PerformedBy
            .PerformedAt = DateTime.Now
            .Status = "Pending"
        End With

        Try
            Return _transactionRepo.AddTransaction(model)
        Catch ex As Exception
            Throw New InvalidOperationException("Lỗi khi thêm giao dịch vào cơ sở dữ liệu.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Lấy thông tin giao dịch theo ID.
    ''' </summary>
    ''' <param name="transactionId">ID của giao dịch.</param>
    ''' <returns>DTO của giao dịch nếu tìm thấy, ngược lại null.</returns>
    ''' <exception cref="ArgumentException">Ném ra nếu transactionId không hợp lệ.</exception>
    Public Function GetTransactionById(ByVal transactionId As Integer) As InventoryTransactionDto Implements IInventoryTransactionService.GetTransactionById
        If transactionId <= 0 Then
            Throw New ArgumentException("ID giao dịch phải lớn hơn 0.", "transactionId")
        End If

        Dim transaction = _transactionRepo.GetTransactionById(transactionId)
        If transaction Is Nothing Then
            Return Nothing
        End If
        Return MapToDto(transaction)
    End Function

    ''' <summary>
    ''' Lấy danh sách giao dịch theo sản phẩm.
    ''' </summary>
    ''' <param name="productId">ID của sản phẩm.</param>
    ''' <returns>Danh sách DTO của các giao dịch liên quan đến sản phẩm.</returns>
    ''' <exception cref="ArgumentException">Ném ra nếu productId không hợp lệ.</exception>
    Public Function GetTransactionsByProduct(ByVal productId As Integer) As List(Of InventoryTransactionDto) Implements IInventoryTransactionService.GetTransactionsByProduct
        If productId <= 0 Then
            Throw New ArgumentException("ID sản phẩm phải lớn hơn 0.", "productId")
        End If

        Dim transactions = _transactionRepo.GetTransactionsByProduct(productId)
        If transactions Is Nothing Then
            Return New List(Of InventoryTransactionDto)
        End If
        Return transactions.Select(AddressOf MapToDto).ToList()
    End Function

    ''' <summary>
    ''' Lấy tất cả giao dịch trong hệ thống.
    ''' </summary>
    ''' <returns>Danh sách DTO của tất cả giao dịch.</returns>
    Public Function GetAllTransactions() As List(Of InventoryTransactionDto) Implements IInventoryTransactionService.GetAllTransactions
        Dim transactions = _transactionRepo.GetAllTransactions()
        If transactions Is Nothing Then
            Return New List(Of InventoryTransactionDto)
        End If
        Return transactions.Select(AddressOf MapToDto).ToList()
    End Function

    ''' <summary>
    ''' Lấy danh sách giao dịch theo trang.
    ''' </summary>
    ''' <param name="pageNumber">Số trang (bắt đầu từ 1).</param>
    ''' <param name="pageSize">Số lượng giao dịch mỗi trang.</param>
    ''' <returns>Danh sách DTO của giao dịch trong trang được yêu cầu.</returns>
    ''' <exception cref="ArgumentException">Ném ra nếu pageNumber hoặc pageSize không hợp lệ.</exception>
    Public Function GetTransactionsByPage(ByVal pageNumber As Integer, ByVal pageSize As Integer) As List(Of InventoryTransactionDto) Implements IInventoryTransactionService.GetTransactionsByPage
        If pageNumber < 1 Then
            Throw New ArgumentException("Số trang phải lớn hơn 0.", "pageNumber")
        End If
        If pageSize < 1 Then
            Throw New ArgumentException("Kích thước trang phải lớn hơn 0.", "pageSize")
        End If

        Dim transactions = _transactionRepo.GetTransactionsByPage(pageNumber, pageSize)
        If transactions Is Nothing Then
            Return New List(Of InventoryTransactionDto)
        End If
        Return transactions.Select(AddressOf MapToDto).ToList()
    End Function

    ''' <summary>
    ''' Lấy tổng số giao dịch trong hệ thống.
    ''' </summary>
    ''' <returns>Tổng số giao dịch.</returns>
    Public Function GetTotalTransactionCount() As Integer Implements IInventoryTransactionService.GetTotalTransactionCount
        Return _transactionRepo.GetTotalTransactionCount()
    End Function

    ''' <summary>
    ''' Lấy số lượng tồn kho hiện tại của một sản phẩm.
    ''' </summary>
    ''' <param name="productId">ID của sản phẩm.</param>
    ''' <returns>Số lượng tồn kho hiện tại.</returns>
    ''' <exception cref="ArgumentException">Ném ra nếu productId không hợp lệ.</exception>
    Public Function GetCurrentStock(ByVal productId As Integer) As Integer Implements IInventoryTransactionService.GetCurrentStock
        If productId <= 0 Then
            Throw New ArgumentException("ID sản phẩm phải lớn hơn 0.", "productId")
        End If

        Return _transactionRepo.GetCurrentStock(productId)
    End Function

    ''' <summary>
    ''' Cập nhật trạng thái của một giao dịch.
    ''' </summary>
    ''' <param name="transactionId">ID của giao dịch.</param>
    ''' <param name="newStatus">Trạng thái mới (Approved hoặc Rejected).</param>
    ''' <returns>True nếu cập nhật thành công, ngược lại False.</returns>
    ''' <exception cref="ArgumentException">Ném ra nếu transactionId hoặc newStatus không hợp lệ.</exception>
    ''' <exception cref="InvalidOperationException">Ném ra nếu giao dịch không tồn tại.</exception>
    Public Function UpdateTransactionStatus(ByVal transactionId As Integer, ByVal newStatus As String) As Boolean Implements IInventoryTransactionService.UpdateTransactionStatus
        If transactionId <= 0 Then
            Throw New ArgumentException("ID giao dịch phải lớn hơn 0.", "transactionId")
        End If
        If String.IsNullOrEmpty(newStatus) Then
            Throw New ArgumentException("Trạng thái không được rỗng.", "newStatus")
        End If
        If newStatus <> "Approved" AndAlso newStatus <> "Rejected" Then
            Throw New ArgumentException("Trạng thái chỉ được là 'Approved' hoặc 'Rejected'.", "newStatus")
        End If

        Dim transaction = _transactionRepo.GetTransactionById(transactionId)
        If transaction Is Nothing Then
            Throw New InvalidOperationException("Giao dịch với ID " & transactionId & " không tồn tại.")
        End If

        Try
            Return _transactionRepo.UpdateTransactionStatus(transactionId, newStatus)
        Catch ex As Exception
            Throw New InvalidOperationException("Lỗi khi cập nhật trạng thái giao dịch.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Phê duyệt một giao dịch.
    ''' </summary>
    ''' <param name="transactionId">ID của giao dịch.</param>
    ''' <param name="newStatus">Trạng thái phê duyệt (Approved hoặc Rejected).</param>
    ''' <returns>True nếu phê duyệt thành công, ngược lại False.</returns>
    ''' <exception cref="ArgumentException">Ném ra nếu transactionId hoặc newStatus không hợp lệ.</exception>
    ''' <exception cref="InvalidOperationException">Ném ra nếu giao dịch không tồn tại.</exception>
    Public Function ApproveTransaction(ByVal transactionId As Integer, ByVal newStatus As String) As Boolean Implements IInventoryTransactionService.ApproveTransaction
        Return UpdateTransactionStatus(transactionId, newStatus)
    End Function

    ''' <summary>
    ''' Lấy báo cáo tồn kho cho tất cả sản phẩm.
    ''' </summary>
    ''' <returns>Danh sách DTO của báo cáo tồn kho.</returns>
    ''' <exception cref="NotImplementedException">Ném ra hồi chức năng chưa được triển khai.</exception>
    Public Function GetInventoryStockReport() As List(Of InventoryStockReportDTO) Implements IInventoryTransactionService.GetInventoryStockReport
        Throw New NotImplementedException("Chức năng báo cáo tồn kho chưa được triển khai.")
    End Function

    ''' <summary>
    ''' Kiểm tra dữ liệu đầu vào của giao dịch.
    ''' </summary>
    ''' <param name="transaction">DTO chứa thông tin giao dịch.</param>
    ''' <param name="errors">Danh sách lỗi.</param>
    Private Sub ValidateTransactionInput(ByVal transaction As InventoryTransactionInputDto, ByVal errors As List(Of String))
        ValidationHelper.ValidateInteger(transaction.ProductId, "Mã sản phẩm", errors, 1)
        ValidationHelper.ValidateInteger(transaction.Quantity, "Số lượng", errors, 1)
        ValidationHelper.ValidateString(transaction.TransactionType, "Loại giao dịch", errors, True, 10)
        ValidationHelper.ValidateString(transaction.Note, "Ghi chú", errors, False, 500)
        ValidationHelper.ValidateInteger(transaction.PerformedBy, "Mã người thực hiện", errors, 1)

        If Not New String() {"Import", "Export"}.Contains(transaction.TransactionType) Then
            errors.Add("Loại giao dịch chỉ được là 'Import' hoặc 'Export'.")
        End If
    End Sub

    ''' <summary>
    ''' Chuyển đổi model InventoryTransaction sang DTO để hiển thị.
    ''' </summary>
    ''' <param name="transaction">Model giao dịch kho.</param>
    ''' <returns>DTO của giao dịch.</returns>
    Private Function MapToDto(ByVal transaction As InventoryTransaction) As InventoryTransactionDto
        If transaction Is Nothing Then
            Return Nothing
        End If

        Dim product = _productRepo.GetProductById(transaction.ProductId)
        Dim user = _userRepo.GetUserById(transaction.PerformedBy)

        Dim dto As New InventoryTransactionDto
        With dto
            .TransactionId = transaction.TransactionId
            .ProductName = If(product Is Nothing, "N/A", product.ProductName)
            .Quantity = Math.Abs(transaction.Quantity)
            .TransactionType = transaction.TransactionType
            .Note = If(transaction.Note Is Nothing, String.Empty, transaction.Note.Trim())
            .PerformedBy = If(user Is Nothing, "Unknown", user.Username)
            .PerformedAt = transaction.PerformedAt
            .Status = If(transaction.Status Is Nothing, "Unknown", transaction.Status)
        End With
        Return dto
    End Function
End Class