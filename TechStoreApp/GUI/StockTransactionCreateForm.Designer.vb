
Partial Class StockTransactionCreateForm
    Inherits System.Windows.Forms.Form

    Private Sub InitializeComponent()
        Me.SuspendLayout()

        ' Thiết lập form
        Me.Text = "Tạo phiếu nhập/xuất"
        Me.Size = New System.Drawing.Size(800, 600)

        ' Khởi tạo _txtTransactionCode
        _txtTransactionCode = New TextBox()
        _txtTransactionCode.Location = New System.Drawing.Point(10, 10)
        _txtTransactionCode.Size = New System.Drawing.Size(200, 30)
        Me.Controls.Add(_txtTransactionCode)

        ' Khởi tạo _txtNote
        _txtNote = New TextBox()
        _txtNote.Location = New System.Drawing.Point(220, 10)
        _txtNote.Size = New System.Drawing.Size(200, 30)
        Me.Controls.Add(_txtNote)

        ' Khởi tạo _cmbSupplier
        _cmbSupplier = New ComboBox()
        _cmbSupplier.Location = New System.Drawing.Point(430, 10)
        _cmbSupplier.Size = New System.Drawing.Size(200, 30)
        Me.Controls.Add(_cmbSupplier)

        ' Khởi tạo _txtProductSearch
        _txtProductSearch = New TextBox()
        _txtProductSearch.Location = New System.Drawing.Point(10, 50)
        _txtProductSearch.Size = New System.Drawing.Size(200, 30)
        _txtProductSearch.Text = "Tìm kiếm sản phẩm..."
        Me.Controls.Add(_txtProductSearch)

        ' Khởi tạo _cmbCategory
        _cmbCategory = New ComboBox()
        _cmbCategory.Location = New System.Drawing.Point(220, 50)
        _cmbCategory.Size = New System.Drawing.Size(200, 30)
        Me.Controls.Add(_cmbCategory)

        ' Khởi tạo _gridProducts
        _gridProducts = New DataGridView()
        _gridProducts.Size = New System.Drawing.Size(380, 200)
        _gridProducts.Location = New System.Drawing.Point(10, 90)
        _gridProducts.AutoGenerateColumns = False
        Dim selectCol As New DataGridViewCheckBoxColumn()
        selectCol.Name = "Select"
        selectCol.HeaderText = "Chọn"
        _gridProducts.Columns.Add(selectCol)
        _gridProducts.Columns.Add("ProductId", "Mã sản phẩm")
        _gridProducts.Columns.Add("ProductName", "Tên sản phẩm")
        _gridProducts.Columns.Add("Category", "Danh mục")
        _gridProducts.Columns.Add("Quantity", "Số lượng tồn")
        Me.Controls.Add(_gridProducts)

        ' Khởi tạo _numQuantity
        _numQuantity = New NumericUpDown()
        _numQuantity.Location = New System.Drawing.Point(430, 90)
        _numQuantity.Size = New System.Drawing.Size(100, 30)
        _numQuantity.Minimum = 0
        _numQuantity.Maximum = 10000
        Me.Controls.Add(_numQuantity)

        ' Khởi tạo _btnAddProduct
        _btnAddProduct = New Button()
        _btnAddProduct.Text = "Thêm sản phẩm"
        _btnAddProduct.Size = New System.Drawing.Size(100, 30)
        _btnAddProduct.Location = New System.Drawing.Point(430, 130)
        Me.Controls.Add(_btnAddProduct)

        ' Khởi tạo _gridSelectedProducts
        _gridSelectedProducts = New DataGridView()
        _gridSelectedProducts.Size = New System.Drawing.Size(760, 200)
        _gridSelectedProducts.Location = New System.Drawing.Point(10, 290)
        _gridSelectedProducts.AutoGenerateColumns = False
        _gridSelectedProducts.Columns.Add("ProductId", "Mã sản phẩm")
        _gridSelectedProducts.Columns.Add("ProductName", "Tên sản phẩm")
        _gridSelectedProducts.Columns.Add("Unit", "Đơn vị")
        _gridSelectedProducts.Columns.Add("Quantity", "Số lượng")
        _gridSelectedProducts.Columns.Add("Note", "Ghi chú")
        Me.Controls.Add(_gridSelectedProducts)

        ' Khởi tạo _btnRemoveProduct
        _btnRemoveProduct = New Button()
        _btnRemoveProduct.Text = "Xóa sản phẩm"
        _btnRemoveProduct.Size = New System.Drawing.Size(100, 30)
        _btnRemoveProduct.Location = New System.Drawing.Point(10, 260)
        Me.Controls.Add(_btnRemoveProduct)

        ' Khởi tạo _btnSave
        _btnSave = New Button()
        _btnSave.Text = "Lưu"
        _btnSave.Size = New System.Drawing.Size(100, 30)
        _btnSave.Location = New System.Drawing.Point(540, 500)
        Me.Controls.Add(_btnSave)

        ' Khởi tạo _btnCancel
        _btnCancel = New Button()
        _btnCancel.Text = "Hủy"
        _btnCancel.Size = New System.Drawing.Size(100, 30)
        _btnCancel.Location = New System.Drawing.Point(650, 500)
        Me.Controls.Add(_btnCancel)

        Me.ResumeLayout(False)
    End Sub
End Class
