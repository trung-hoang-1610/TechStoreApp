Public Class StockTransactionCreateForm
    Inherits Form

    Private ReadOnly _transactionService As IStockTransactionService
    Private ReadOnly _productService As IProductService
    Private ReadOnly _supplierService As ISupplierService
    Private ReadOnly _categoryService As ICategoryService
    Private ReadOnly _transactionType As String
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

    Public Sub New(transactionType As String)
        If String.IsNullOrEmpty(transactionType) Then
            Throw New ArgumentNullException(NameOf(transactionType), "Transaction type cannot be null or empty.")
        End If

        _transactionType = transactionType
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
        If _txtTransactionCode Is Nothing OrElse _cmbSupplier Is Nothing Then
            Return
        End If

        Me.Text = If(_transactionType = "IN", "Tạo phiếu nhập", "Tạo phiếu xuất")
        _cmbSupplier.Visible = _transactionType = "IN"
        _txtTransactionCode.Text = GenerateTransactionCode()
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
        If _cmbSupplier IsNot Nothing Then
            RemoveHandler _cmbSupplier.SelectedIndexChanged, AddressOf OnSupplierSelectedIndexChanged
        End If
        If _cmbCategory IsNot Nothing Then
            RemoveHandler _cmbCategory.SelectedIndexChanged, AddressOf OnCategorySelectedIndexChanged
        End If
        If _txtProductSearch IsNot Nothing Then
            RemoveHandler _txtProductSearch.TextChanged, AddressOf OnProductSearchTextChanged
        End If
    End Sub

    Private Sub AddEventHandlers()
        If _cmbSupplier IsNot Nothing Then
            AddHandler _cmbSupplier.SelectedIndexChanged, AddressOf OnSupplierSelectedIndexChanged
        End If
        If _cmbCategory IsNot Nothing Then
            AddHandler _cmbCategory.SelectedIndexChanged, AddressOf OnCategorySelectedIndexChanged
        End If
        If _txtProductSearch IsNot Nothing Then
            AddHandler _txtProductSearch.TextChanged, AddressOf OnProductSearchTextChanged
        End If
    End Sub

    Private Function GenerateTransactionCode() As String
        Try
            If _transactionService Is Nothing Then
                Return String.Empty
            End If

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
            Return
        End If

        Try
            If _transactionType = "IN" Then
                Dim suppliers = _supplierService.GetAllSuppliers()
                If suppliers IsNot Nothing AndAlso suppliers.Any() Then
                    _cmbSupplier.DataSource = suppliers
                    _cmbSupplier.DisplayMember = "SupplierName"
                    _cmbSupplier.ValueMember = "SupplierId"
                    _cmbSupplier.SelectedIndex = -1
                Else
                    _cmbSupplier.Enabled = False
                End If
            Else
                _cmbSupplier.Enabled = False
            End If
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tải danh sách nhà cung cấp: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadCategories()
        If _cmbCategory Is Nothing OrElse _categoryService Is Nothing Then
            Return
        End If

        Try
            _cmbCategory.Items.Clear()
            _cmbCategory.Items.Add("All")
            Dim categories = _categoryService.GetAllCategories()
            If categories IsNot Nothing AndAlso categories.Any() Then
                _cmbCategory.DataSource = categories
                _cmbCategory.DisplayMember = "CategoryName"
                _cmbCategory.ValueMember = "CategoryId"
                _cmbCategory.SelectedIndex = -1
            End If
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tải danh sách danh mục: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadProducts()
        If _gridProducts Is Nothing OrElse _productService Is Nothing Then
            ShowErrorMessage("Grid hoặc ProductService không được khởi tạo.")
            Return
        End If

        Try
            ' Kiểm tra _transactionType
            If String.IsNullOrEmpty(_transactionType) Then
                ShowErrorMessage("TransactionType không được khởi tạo.")
                Return
            End If

            ' Kiểm tra _cmbSupplier
            Dim supplierId As Integer? = Nothing
            If _transactionType = "IN" Then
                If _cmbSupplier Is Nothing Then
                    ShowErrorMessage("ComboBox nhà cung cấp không được khởi tạo.")
                    Return
                End If
                If _cmbSupplier.SelectedIndex >= 0 AndAlso _cmbSupplier.SelectedValue IsNot Nothing Then
                    Try
                        supplierId = CInt(_cmbSupplier.SelectedValue)
                        Debug.WriteLine($"SupplierId: {supplierId}")
                    Catch ex As InvalidCastException
                        ShowErrorMessage("Lỗi khi chuyển đổi SupplierId: " & ex.Message)
                        Return
                    End Try
                End If
            End If

            ' Kiểm tra _cmbCategory
            Dim category As String = Nothing
            If _cmbCategory Is Nothing Then
                ShowErrorMessage("ComboBox danh mục không được khởi tạo.")
                Return
            End If
            If _cmbCategory.SelectedIndex > 0 AndAlso _cmbCategory.SelectedItem IsNot Nothing Then
                category = _cmbCategory.SelectedItem.ToString()
                Debug.WriteLine($"Category: {category}")
            End If

            ' Kiểm tra _txtProductSearch
            Dim searchText As String = If(_txtProductSearch?.Text, String.Empty)
            Debug.WriteLine($"SearchText: {searchText}")

            ' Lấy danh sách sản phẩm
            Dim products As List(Of ProductDTO) = Nothing
            If _transactionType = "IN" Then
                Debug.WriteLine("Gọi GetProductsBySupplierId...")
                Try
                    products = _productService.GetProductsBySupplierId(supplierId)
                Catch ex As ArgumentOutOfRangeException
                    ShowErrorMessage($"Lỗi trong GetProductsBySupplierId: {ex.Message}{vbCrLf}{ex.StackTrace}")
                    _gridProducts.DataSource = Nothing
                    Return
                End Try
            Else
                Debug.WriteLine("Gọi GetAllProducts...")
                Try
                    products = _productService.GetAllProducts()
                Catch ex As ArgumentOutOfRangeException
                    ShowErrorMessage($"Lỗi trong GetAllProducts: {ex.Message}{vbCrLf}{ex.StackTrace}")
                    _gridProducts.DataSource = Nothing
                    Return
                End Try
            End If

            ' Kiểm tra danh sách sản phẩm
            If products Is Nothing OrElse Not products.Any() Then
                MessageBox.Show("Không lấy được danh sách sản phẩm. Danh sách rỗng hoặc null.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _gridProducts.DataSource = Nothing
                Return
            Else
                MessageBox.Show($"Đã lấy được {products.Count} sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Log danh sách sản phẩm
                Debug.WriteLine("Danh sách sản phẩm:")
                For Each product In products
                    If product IsNot Nothing Then
                        Debug.WriteLine($"Product: ID={product.ProductId}, Name={product.ProductName}, Category={product.CategoryName}, Unit={product.Unit}, Quantity={product.Quantity}")
                    Else
                        Debug.WriteLine("Product: NULL")
                    End If
                Next
            End If

            ' Lọc sản phẩm
            If Not String.IsNullOrEmpty(category) AndAlso category <> "All" Then
                products = products.Where(Function(p) p?.CategoryName = category).ToList()
            End If
            If Not String.IsNullOrEmpty(searchText) Then
                products = products.Where(Function(p) p IsNot Nothing AndAlso
                ((p.ProductName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) OrElse
                 (p.ProductId.ToString().IndexOf(searchText) >= 0))).ToList()
            End If

            _gridProducts.DataSource = products
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi tải danh sách sản phẩm: " & ex.Message & vbCrLf & ex.StackTrace)
            _gridProducts.DataSource = Nothing
        End Try
    End Sub

    Private Sub OnSupplierSelectedIndexChanged(sender As Object, e As EventArgs)
        LoadProducts()
    End Sub

    Private Sub OnProductSearchTextChanged(sender As Object, e As EventArgs)
        LoadProducts()
    End Sub

    Private Sub OnCategorySelectedIndexChanged(sender As Object, e As EventArgs)
        LoadProducts()
    End Sub

    Private Sub _btnAddProduct_Click(sender As Object, e As EventArgs) Handles _btnAddProduct.Click
        If _gridProducts Is Nothing OrElse _gridSelectedProducts Is Nothing OrElse _numQuantity Is Nothing Then
            Return
        End If

        Try
            If Not _gridProducts.Columns.Contains("Select") Then
                ShowErrorMessage("Cột 'Select' không tồn tại trong lưới sản phẩm.")
                Return
            End If

            For Each row As DataGridViewRow In _gridProducts.Rows
                If row.Cells("Select")?.Value IsNot Nothing AndAlso CBool(row.Cells("Select").Value) Then
                    Dim productId = If(row.Cells("ProductId")?.Value IsNot Nothing, CInt(row.Cells("ProductId").Value), 0)
                    If productId = 0 Then Continue For

                    Dim product = _productService?.GetProductById(productId)
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
                    row.Cells("Select").Value = False
                End If
            Next

            RefreshSelectedProductsGrid()
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi thêm sản phẩm: " & ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub

    Private Sub _btnRemoveProduct_Click(sender As Object, e As EventArgs) Handles _btnRemoveProduct.Click
        If _gridSelectedProducts Is Nothing Then
            Return
        End If

        Try
            If _gridSelectedProducts.SelectedRows.Count > 0 Then
                Dim selectedDetail = TryCast(_gridSelectedProducts.SelectedRows(0).DataBoundItem, StockTransactionDetailDTO)
                If selectedDetail IsNot Nothing Then
                    _selectedProducts.Remove(selectedDetail)
                    RefreshSelectedProductsGrid()
                End If
            Else
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            ShowErrorMessage("Lỗi khi xóa sản phẩm: " & ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub

    Private Sub _btnSave_Click(sender As Object, e As EventArgs) Handles _btnSave.Click
        Try
            If _selectedProducts Is Nothing OrElse _selectedProducts.Count = 0 Then
                ShowErrorMessage("Vui lòng chọn ít nhất một sản phẩm.")
                Return
            End If

            If _txtTransactionCode Is Nothing OrElse _transactionService Is Nothing Then
                Return
            End If

            Dim transaction As New StockTransactionDTO With {
                .TransactionCode = _txtTransactionCode.Text,
                .TransactionType = _transactionType,
                .Note = _txtNote?.Text,
                .CreatedBy = SessionManager.GetCurrentUser()?.UserId,
                .SupplierId = If(_transactionType = "IN" AndAlso _cmbSupplier?.SelectedValue IsNot Nothing, CInt(_cmbSupplier.SelectedValue), 0)
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
            ShowErrorMessage("Lỗi khi lưu giao dịch: " & ex.Message & vbCrLf & ex.StackTrace)
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