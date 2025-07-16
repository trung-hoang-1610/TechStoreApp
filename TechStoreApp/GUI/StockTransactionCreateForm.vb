
Public Class StockTransactionCreateForm
    Inherits Form

    Private ReadOnly _transactionService As IStockTransactionBLL
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
    Private _selectedProducts As New List(Of StockTransactionDetailDTO)

    Public Sub New(ByVal transactionType As String)
        InitializeComponent()
        _transactionType = transactionType
        _transactionService = ServiceFactory.CreateStockTransactionService()
        _productService = ServiceFactory.CreateProductService()
        _supplierService = ServiceFactory.CreateSupplierService()
        _categoryService = ServiceFactory.CreateCategoryService()
        Me.Text = If(transactionType = "IN", "Tạo phiếu nhập", "Tạo phiếu xuất")
        _cmbSupplier.Visible = transactionType = "IN"
        _txtTransactionCode.Text = GenerateTransactionCode()
        LoadSuppliers()
        LoadCategories()
        LoadProducts()
    End Sub

    Private Function GenerateTransactionCode() As String
        Dim prefix = If(_transactionType = "IN", "IN", "OUT")
        Dim datePart = DateTime.Now.ToString("yyyyMMdd")
        Dim sequence = _transactionService.GetTransactions(_transactionType, Nothing).Count + 1
        Return $"{prefix}-{datePart}-{sequence.ToString("D3")}"
    End Function

    Private Sub LoadSuppliers()
        If _transactionType = "IN" Then
            Dim suppliers = _supplierService.GetAllSuppliers()
            _cmbSupplier.DataSource = suppliers
            _cmbSupplier.DisplayMember = "SupplierName"
            _cmbSupplier.ValueMember = "SupplierId"
            _cmbSupplier.SelectedIndex = -1
        End If
    End Sub

    Private Sub LoadCategories()
        _cmbCategory.Items.Add("All")
        Dim categories = _categoryService.GetAllCategories()
        _cmbCategory.Items.AddRange(categories.ToArray())
        _cmbCategory.SelectedIndex = 0
    End Sub

    Private Sub LoadProducts()
        Dim supplierId = If(_transactionType = "IN" AndAlso _cmbSupplier.SelectedValue IsNot Nothing, CInt(_cmbSupplier.SelectedValue), 0)
        Dim category = If(_cmbCategory.SelectedIndex > 0, _cmbCategory.SelectedItem.ToString(), Nothing)
        Dim searchText = _txtProductSearch.Text
        Dim products = If(_transactionType = "IN", _productService.GetProductsBySupplierId(supplierId), _productService.GetAllProducts())
        If Not String.IsNullOrEmpty(category) AndAlso category <> "All" Then
            products = products.Where(Function(p) p.Category = category).ToList()
        End If
        If Not String.IsNullOrEmpty(searchText) Then
            products = products.Where(Function(p) p.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase) OrElse p.ProductId.ToString().Contains(searchText)).ToList()
        End If
        _gridProducts.DataSource = products
    End Sub

    Private Sub _cmbSupplier_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _cmbSupplier.SelectedIndexChanged
        LoadProducts()
    End Sub

    Private Sub _txtProductSearch_TextChanged(sender As Object, e As EventArgs) Handles _txtProductSearch.TextChanged
        LoadProducts()
    End Sub

    Private Sub _cmbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _cmbCategory.SelectedIndexChanged
        LoadProducts()
    End Sub

    Private Sub _btnAddProduct_Click(sender As Object, e As EventArgs) Handles _btnAddProduct.Click
        For Each row As DataGridViewRow In _gridProducts.Rows
            If row.Cells("Select").Value = True Then
                Dim productId = CInt(row.Cells("ProductId").Value)
                Dim product = _productService.GetProductById(productId)
                Dim quantity = CInt(_numQuantity.Value)

                If _transactionType = "OUT" AndAlso product.Quantity < quantity Then
                    MessageBox.Show($"Sản phẩm {product.ProductName} không đủ tồn kho (hiện có: {product.Quantity}, yêu cầu: {quantity}).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                If _transactionType = "IN" AndAlso quantity > 10000 Then
                    MessageBox.Show($"Số lượng nhập cho sản phẩm {product.ProductName} vượt quá giới hạn (tối đa 10,000).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim detail As New StockTransactionDetailDTO With {
                    .ProductId = productId,
                    .ProductName = product.ProductName,
                    .Unit = product.Unit,
                    .Quantity = quantity,
                    .Note = ""
                }
                _selectedProducts.Add(detail)
                row.Cells("Select").Value = False
            End If
        Next
        _gridSelectedProducts.DataSource = Nothing
        _gridSelectedProducts.DataSource = _selectedProducts
    End Sub

    Private Sub _btnRemoveProduct_Click(sender As Object, e As EventArgs) Handles _btnRemoveProduct.Click
        If _gridSelectedProducts.SelectedRows.Count > 0 Then
            Dim selectedDetail = DirectCast(_gridSelectedProducts.SelectedRows(0).DataBoundItem, StockTransactionDetailDTO)
            _selectedProducts.Remove(selectedDetail)
            _gridSelectedProducts.DataSource = Nothing
            _gridSelectedProducts.DataSource = _selectedProducts
        End If
    End Sub

    Private Sub _btnSave_Click(sender As Object, e As EventArgs) Handles _btnSave.Click
        If _selectedProducts.Count = 0 Then
            MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim transaction As New StockTransactionDTO With {
            .TransactionCode = _txtTransactionCode.Text,
            .TransactionType = _transactionType,
            .Note = _txtNote.Text,
            .CreatedBy = SessionManager.GetCurrentUser().UserId,
            .SupplierId = If(_transactionType = "IN" AndAlso _cmbSupplier.SelectedValue IsNot Nothing, CInt(_cmbSupplier.SelectedValue), 0)
        }

        Dim result = If(_transactionType = "IN", _transactionService.CreateStockInTransaction(transaction, _selectedProducts), _transactionService.CreateStockOutTransaction(transaction, _selectedProducts))
        If result.Success Then
            MessageBox.Show("Tạo phiếu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub _btnCancel_Click(sender As Object, e As EventArgs) Handles _btnCancel.Click
        Me.Close()
    End Sub
End Class
