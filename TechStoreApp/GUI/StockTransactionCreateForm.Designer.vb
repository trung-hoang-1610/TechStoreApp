
Partial Class StockTransactionCreateForm
    Inherits System.Windows.Forms.Form

    Private Sub InitializeComponent()
        _txtTransactionCode = New TextBox()
        _txtNote = New TextBox()
        _cmbSupplier = New ComboBox()
        _txtProductSearch = New TextBox()
        _cmbCategory = New ComboBox()
        _gridProducts = New DataGridView()
        _numQuantity = New NumericUpDown()
        _btnAddProduct = New Button()
        _gridSelectedProducts = New DataGridView()
        _btnRemoveProduct = New Button()
        _btnSave = New Button()
        _btnCancel = New Button()

        ' Thiết lập form
        Me.Size = New System.Drawing.Size(800, 600)

        ' Thiết lập thông tin phiếu
        Dim panelInfo As New Panel()
        panelInfo.Size = New System.Drawing.Size(780, 100)
        panelInfo.Location = New System.Drawing.Point(10, 10)
        _txtTransactionCode.Location = New System.Drawing.Point(10, 10)
        _txtTransactionCode.Size = New System.Drawing.Size(200, 30)
        panelInfo.Controls.Add(_txtTransactionCode)

        _txtNote.Location = New System.Drawing.Point(10, 50)
        _txtNote.Size = New System.Drawing.Size(200, 30)
        _txtNote.Text = "Ghi chú..."
        panelInfo.Controls.Add(_txtNote)

        _cmbSupplier.Location = New System.Drawing.Point(220, 10)
        _cmbSupplier.Size = New System.Drawing.Size(200, 30)
        panelInfo.Controls.Add(_cmbSupplier)
        Me.Controls.Add(panelInfo)

        ' Thiết lập tìm kiếm sản phẩm
        _txtProductSearch.Location = New System.Drawing.Point(10, 120)
        _txtProductSearch.Size = New System.Drawing.Size(200, 30)
        _txtProductSearch.Text = "Tìm kiếm sản phẩm..."
        Me.Controls.Add(_txtProductSearch)

        _cmbCategory.Location = New System.Drawing.Point(220, 120)
        _cmbCategory.Size = New System.Drawing.Size(200, 30)
        Me.Controls.Add(_cmbCategory)

        ' Thiết lập DataGridView sản phẩm
        _gridProducts.Size = New System.Drawing.Size(380, 200)
        _gridProducts.Location = New System.Drawing.Point(10, 160)
        _gridProducts.AutoGenerateColumns = False
        _gridProducts.Columns.Add(New DataGridViewCheckBoxColumn() With {.Name = "Select", .HeaderText = "Chọn"})
        _gridProducts.Columns.Add("ProductId", "Mã sản phẩm")
        _gridProducts.Columns.Add("ProductName", "Tên sản phẩm")
        _gridProducts.Columns.Add("Unit", "Đơn vị")
        _gridProducts.Columns.Add("Quantity", "Tồn kho")
        Me.Controls.Add(_gridProducts)

        _numQuantity.Location = New System.Drawing.Point(400, 160)
        _numQuantity.Size = New System.Drawing.Size(100, 30)
        _numQuantity.Minimum = 1
        _numQuantity.Maximum = 10000
        Me.Controls.Add(_numQuantity)

        _btnAddProduct.Text = "Thêm sản phẩm"
        _btnAddProduct.Size = New System.Drawing.Size(100, 30)
        _btnAddProduct.Location = New System.Drawing.Point(510, 160)
        Me.Controls.Add(_btnAddProduct)

        ' Thiết lập DataGridView sản phẩm đã chọn
        _gridSelectedProducts.Size = New System.Drawing.Size(780, 200)
        _gridSelectedProducts.Location = New System.Drawing.Point(10, 370)
        _gridSelectedProducts.AutoGenerateColumns = False
        _gridSelectedProducts.Columns.Add("ProductId", "Mã sản phẩm")
        _gridSelectedProducts.Columns.Add("ProductName", "Tên sản phẩm")
        _gridSelectedProducts.Columns.Add("Unit", "Đơn vị")
        _gridSelectedProducts.Columns.Add("Quantity", "Số lượng")
        _gridSelectedProducts.Columns.Add("Note", "Ghi chú")
        Me.Controls.Add(_gridSelectedProducts)

        _btnRemoveProduct.Text = "Xóa sản phẩm"
        _btnRemoveProduct.Size = New System.Drawing.Size(100, 30)
        _btnRemoveProduct.Location = New System.Drawing.Point(680, 330)
        Me.Controls.Add(_btnRemoveProduct)

        _btnSave.Text = "Lưu"
        _btnSave.Size = New System.Drawing.Size(100, 30)
        _btnSave.Location = New System.Drawing.Point(590, 530)
        Me.Controls.Add(_btnSave)

        _btnCancel.Text = "Hủy"
        _btnCancel.Size = New System.Drawing.Size(100, 30)
        _btnCancel.Location = New System.Drawing.Point(680, 530)
        Me.Controls.Add(_btnCancel)
    End Sub
End Class
