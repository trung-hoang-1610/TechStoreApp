Public Class StockTransactionCreateForm
    Inherits Form

    Private ReadOnly _transactionService As IStockTransactionService
    Private ReadOnly _productService As IProductService
    Private ReadOnly _supplierService As ISupplierService
    Private ReadOnly _categoryService As ICategoryService
    Private ReadOnly _transactionType As String
    Private ReadOnly _supplierId As Integer?  ' Lưu supplierId cho phiếu nhập
    Private WithEvents _txtTransactionCode As TextBox
    Private WithEvents _txtNote As TextBox
    Private WithEvents _cmbSupplier As ComboBox
    Private WithEvents _txtProductSearch As TextBox
    Private WithEvents _cmbCategory As ComboBox
    Private WithEvents _gridProducts As DataGridView
    Private WithEvents _numQuantity As NumericUpDown
    Private WithEvents _btnAddProduct As Button
    Private WithEvents _gridSelectedProducts As DataGridView
    Private WithEvents _btnRemoveProduct As Button
    Private WithEvents _btnSave As Button
    Private WithEvents _btnCancel As Button
    Private ReadOnly _selectedProducts As List(Of StockTransactionDetailDTO)

    Public Sub New(transactionType As String, Optional supplierId As Integer? = Nothing)
        If String.IsNullOrEmpty(transactionType) Then
            Throw New ArgumentNullException(NameOf(transactionType), "Transaction type cannot be null or empty.")
        End If
        If transactionType = "IN" AndAlso Not supplierId.HasValue Then
            Throw New ArgumentException("SupplierId is required for stock-in transactions.")
        End If

        _transactionType = transactionType
        _supplierId = supplierId
        _transactionService = ServiceFactory.CreateStockTransactionService()
        _productService = ServiceFactory.CreateProductService()
        _supplierService = ServiceFactory.CreateSupplierService()
        _categoryService = ServiceFactory.CreateCategoryService()
        _selectedProducts = New List(Of StockTransactionDetailDTO)()

        InitializeComponent()
        ConfigureForm()
        InitializeData()
    End Sub

    Private Sub ConfigureForm()
        If _txtTransactionCode Is Nothing OrElse _cmbSupplier Is Nothing Then Return

        Me.Text = If(_transactionType = "IN", "Tạo phiếu nhập", "Tạo phiếu xuất")
        _txtTransactionCode.Text = GenerateTransactionCode()
        _txtTransactionCode.Enabled = False  ' Vô hiệu hóa chỉnh sửa mã giao dịch

        If _transactionType = "IN" Then
            _cmbSupplier.Enabled = False  ' Vô hiệu hóa chọn NCC cho phiếu nhập
            If _supplierId.HasValue Then
                Dim supplier = _supplierService.GetSupplierById(_supplierId.Value)
                If supplier IsNot Nothing Then
                    _cmbSupplier.Items.Add(supplier)
                    _cmbSupplier.DisplayMember = "SupplierName"
                    _cmbSupplier.ValueMember = "SupplierId"
                    _cmbSupplier.SelectedIndex = 0
                Else
                    ShowErrorMessage("Không tìm thấy nhà cung cấp với SupplierId: " & _supplierId.Value)
                End If
            End If
        Else
            _cmbSupplier.Enabled = True  ' Bật chọn NCC cho phiếu xuất
        End If

        _gridProducts.AutoGenerateColumns = False
        _gridSelectedProducts.AutoGenerateColumns = False
    End Sub

    Private Sub InitializeData()
        RemoveEventHandlers()
        Try
            LoadSuppliers()
            LoadCategories()
            LoadProducts()
        Finally
            AddEventHandlers()
        End Try
    End Sub

    Private Sub RemoveEventHandlers()
        If _cmbSupplier IsNot Nothing Then RemoveHandler _cmbSupplier.SelectedIndexChanged, AddressOf OnSupplierSelectedIndexChanged
        If _cmbCategory IsNot Nothing Then RemoveHandler _cmbCategory.SelectedIndexChanged, AddressOf OnCategorySelectedIndexChanged
        If _txtProductSearch IsNot Nothing Then RemoveHandler _txtProductSearch.TextChanged, AddressOf OnProductSearchTextChanged
    End Sub

    Private Sub AddEventHandlers()
        If _cmbSupplier IsNot Nothing Then AddHandler _cmbSupplier.SelectedIndexChanged, AddressOf OnSupplierSelectedIndexChanged
        If _cmbCategory IsNot Nothing Then AddHandler _cmbCategory.SelectedIndexChanged, AddressOf OnCategorySelectedIndexChanged
        If _txtProductSearch IsNot Nothing Then AddHandler _txtProductSearch.TextChanged, AddressOf OnProductSearchTextChanged
    End Sub

    Private Function GenerateTransactionCode() As String
        Try
            If _transactionService Is Nothing Then Return String.Empty
            Dim prefix = If(_transactionType = "IN", "IN", "OUT")
            Dim datePart = DateTime.Now.ToString("yyyyMMdd")
            Dim transactions = _transactionService.GetTransactions(_transactionType, Nothing)
            Dim sequence = If(transactions IsNot Nothing, transactions.Count + 1, 1)
            Return String.Format("{0}-{1}-{2:D3}", prefix, datePart, sequence)
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tạo mã giao dịch: " & ex.Message)
            Return String.Empty
        End Try
    End Function

    Private Sub LoadSuppliers()
        If _cmbSupplier Is Nothing OrElse _supplierService Is Nothing Then
            ShowErrorMessage("ComboBox nhà cung cấp hoặc SupplierService không được khởi tạo.")
            Return
        End If

        Try
            If _transactionType = "OUT" Then
                _cmbSupplier.Items.Clear()
                _cmbSupplier.Items.Add("Tất cả")  ' Thêm tùy chọn "Tất cả" cho phiếu xuất
                Dim suppliers = _supplierService.GetAllSuppliers()
                If suppliers IsNot Nothing AndAlso suppliers.Any() Then
                    _cmbSupplier.DataSource = suppliers
                    _cmbSupplier.DisplayMember = "SupplierName"
                    _cmbSupplier.ValueMember = "SupplierId"
                    _cmbSupplier.SelectedIndex = 0  ' Chọn "Tất cả" làm mặc định
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tải danh sách nhà cung cấp: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadCategories()
        If _cmbCategory Is Nothing OrElse _categoryService Is Nothing Then Return

        Try
            _cmbCategory.Items.Clear()
            _cmbCategory.Items.Add("Tất cả")  ' Thêm tùy chọn "Tất cả"
            Dim categories = _categoryService.GetAllCategories()
            If categories IsNot Nothing AndAlso categories.Any() Then
                _cmbCategory.DataSource = categories
                _cmbCategory.DisplayMember = "CategoryName"
                _cmbCategory.ValueMember = "CategoryId"
                _cmbCategory.SelectedIndex = 0  ' Chọn "Tất cả" làm mặc định
            End If
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tải danhLists danh mục: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadProducts()
        If _gridProducts Is Nothing OrElse _productService Is Nothing Then Return

        Try
            Dim products As List(Of ProductDTO)
            If _transactionType = "IN" AndAlso _supplierId.HasValue Then
                products = _productService.GetProductsBySupplierId(_supplierId.Value)
            ElseIf _transactionType = "OUT" AndAlso _cmbSupplier.SelectedIndex > 0 AndAlso _cmbSupplier.SelectedValue IsNot Nothing Then
                products = _productService.GetProductsBySupplierId(CInt(_cmbSupplier.SelectedValue))
            Else
                products = _productService.GetAllProducts()
            End If

            If products Is Nothing OrElse Not products.Any() Then
                _gridProducts.DataSource = Nothing
                MessageBox.Show("Không có sản phẩm nào để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim category As String = If(_cmbCategory?.Text, "Tất cả")
            Dim searchText As String = If(_txtProductSearch?.Text, String.Empty)

            If category <> "Tất cả" Then
                products = products.Where(Function(p) p?.CategoryName = category).ToList()
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                products = products.Where(Function(p) p IsNot Nothing AndAlso
                    (p.ProductName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                     p.ProductId.ToString().IndexOf(searchText) >= 0)).ToList()
            End If

            _gridProducts.DataSource = Nothing
            _gridProducts.DataSource = products
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tải danh sách sản phẩm: " & ex.Message)
            _gridProducts.DataSource = Nothing
        End Try
    End Sub

    Private Sub OnSupplierSelectedIndexChanged(sender As Object, e As EventArgs)
        If _transactionType = "OUT" Then
            LoadProducts()
        End If
    End Sub

    Private Sub OnProductSearchTextChanged(sender As Object, e As EventArgs)
        LoadProducts()
    End Sub

    Private Sub OnCategorySelectedIndexChanged(sender As Object, e As EventArgs)
        LoadProducts()
    End Sub

    Private Sub _btnAddProduct_Click(sender As Object, e As EventArgs) Handles _btnAddProduct.Click
        If _gridProducts Is Nothing OrElse _gridSelectedProducts Is Nothing OrElse _numQuantity Is Nothing Then Return

        Try
            If Not _gridProducts.Columns.Contains("ChkSelect") Then
                ShowErrorMessage("Cột 'ChkSelect' không tồn tại trong lưới sản phẩm.")
                Return
            End If

            For Each row As DataGridViewRow In _gridProducts.Rows
                If row.Cells("ChkSelect")?.Value IsNot Nothing AndAlso CBool(row.Cells("ChkSelect").Value) Then
                    Dim productId = If(row.Cells("ProductId_Products")?.Value IsNot Nothing, CInt(row.Cells("ProductId_Products").Value), 0)
                    If productId = 0 Then Continue For

                    Dim product = _productService.GetProductById(productId)
                    If product Is Nothing Then
                        ShowErrorMessage($"Không tìm thấy sản phẩm với ProductId: {productId}")
                        Continue For
                    End If

                    Dim quantity = CInt(_numQuantity.Value)
                    If _transactionType = "OUT" AndAlso product.Quantity < quantity Then
                        ShowErrorMessage($"Sản phẩm {product.ProductName} không đủ tồn kho (hiện có: {product.Quantity}, yêu cầu: {quantity}).")
                        Continue For
                    End If
                    If _transactionType = "IN" AndAlso quantity > 10000 Then
                        ShowErrorMessage($"Số lượng nhập cho sản phẩm {product.ProductName} vượt quá giới hạn (tối đa 10,000).")
                        Continue For
                    End If

                    Dim detail As New StockTransactionDetailDTO With {
                        .ProductId = productId,
                        .ProductName = product.ProductName,
                        .Unit = product.Unit,
                        .Quantity = quantity,
                        .Note = String.Empty
                    }
                    _selectedProducts.Add(detail)
                    row.Cells("ChkSelect").Value = False
                End If
            Next

            RefreshSelectedProductsGrid()
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi thêm sản phẩm: " & ex.Message)
        End Try
    End Sub

    Private Sub _btnRemoveProduct_Click(sender As Object, e As EventArgs) Handles _btnRemoveProduct.Click
        If _gridSelectedProducts Is Nothing Then Return

        Try
            If _gridSelectedProducts.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim toRemove As New List(Of StockTransactionDetailDTO)
            For Each row As DataGridViewRow In _gridSelectedProducts.SelectedRows
                Dim detail = TryCast(row.DataBoundItem, StockTransactionDetailDTO)
                If detail IsNot Nothing Then toRemove.Add(detail)
            Next

            For Each detail In toRemove
                _selectedProducts.Remove(detail)
            Next

            RefreshSelectedProductsGrid()
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi xóa sản phẩm: " & ex.Message)
        End Try
    End Sub

    Private Sub _btnSave_Click(sender As Object, e As EventArgs) Handles _btnSave.Click
        Try
            If _selectedProducts Is Nothing OrElse _selectedProducts.Count = 0 Then
                ShowErrorMessage("Vui lòng chọn ít nhất một sản phẩm.")
                Return
            End If
            If _transactionType = "IN" AndAlso Not _supplierId.HasValue Then
                ShowErrorMessage("Vui lòng cung cấp nhà cung cấp cho phiếu nhập.")
                Return
            End If

            If _txtTransactionCode Is Nothing OrElse _transactionService Is Nothing Then Return

            Dim transaction As New StockTransactionDTO With {
                .TransactionCode = _txtTransactionCode.Text,
                .TransactionType = _transactionType,
                .Note = _txtNote?.Text,
                .CreatedBy = SessionManager.GetCurrentUser()?.UserId,
                .SupplierId = If(_transactionType = "IN", _supplierId.Value,
                                If(_transactionType = "OUT" AndAlso _cmbSupplier.SelectedIndex > 0 AndAlso _cmbSupplier.SelectedValue IsNot Nothing,
                                   CInt(_cmbSupplier.SelectedValue), 0))
            }

            Dim result = If(_transactionType = "IN",
                _transactionService.CreateStockInTransaction(transaction, _selectedProducts),
                _transactionService.CreateStockOutTransaction(transaction, _selectedProducts))

            If result.Success Then
                MessageBox.Show("Tạo phiếu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                ShowErrorMessage(String.Join(Environment.NewLine, result.Errors.ToArray()))
            End If
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi lưu giao dịch: " & ex.Message)
        End Try
    End Sub

    Private Sub _btnCancel_Click(sender As Object, e As EventArgs) Handles _btnCancel.Click
        Me.Close()
    End Sub

    Private Sub RefreshSelectedProductsGrid()
        If _gridSelectedProducts IsNot Nothing Then
            _gridSelectedProducts.DataSource = Nothing
            _gridSelectedProducts.DataSource = _selectedProducts
        End If
    End Sub

    Private Sub ShowErrorMessage(message As String)
        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class