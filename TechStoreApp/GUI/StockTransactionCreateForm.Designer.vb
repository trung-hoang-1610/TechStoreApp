
Partial Class StockTransactionCreateForm
    Inherits Form

    Private Sub InitializeComponent()
        Me.SuspendLayout()

        ' Thiết lập form
        Me.Text = "Tạo phiếu nhập/xuất"
        Me.Size = New System.Drawing.Size(800, 650)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        ' GroupBox cho thông tin giao dịch
        Dim grpTransactionInfo As New GroupBox()
        grpTransactionInfo.Text = "Thông tin giao dịch"
        grpTransactionInfo.Location = New Point(10, 10)
        grpTransactionInfo.Size = New Size(760, 80)
        Me.Controls.Add(grpTransactionInfo)

        Dim lblTransactionCode As New Label()
        lblTransactionCode.Text = "Mã giao dịch:"
        lblTransactionCode.Location = New Point(10, 20)
        lblTransactionCode.Size = New Size(80, 20)
        grpTransactionInfo.Controls.Add(lblTransactionCode)

        _txtTransactionCode = New TextBox()
        _txtTransactionCode.Location = New Point(100, 20)
        _txtTransactionCode.Size = New Size(150, 30)
        _txtTransactionCode.Enabled = False  ' Vô hiệu hóa chỉnh sửa mã giao dịch
        grpTransactionInfo.Controls.Add(_txtTransactionCode)

        Dim lblNote As New Label()
        lblNote.Text = "Ghi chú:"
        lblNote.Location = New Point(260, 20)
        lblNote.Size = New Size(50, 20)
        grpTransactionInfo.Controls.Add(lblNote)

        _txtNote = New TextBox()
        _txtNote.Location = New Point(320, 20)
        _txtNote.Size = New Size(150, 30)
        grpTransactionInfo.Controls.Add(_txtNote)



        ' GroupBox cho chọn sản phẩm
        Dim grpProductSelection As New GroupBox()
        grpProductSelection.Text = "Chọn sản phẩm"
        grpProductSelection.Location = New Point(10, 100)
        grpProductSelection.Size = New Size(760, 240)
        Me.Controls.Add(grpProductSelection)

        Dim lblProductSearch As New Label()
        lblProductSearch.Text = "Tìm kiếm:"
        lblProductSearch.Location = New Point(10, 20)
        lblProductSearch.Size = New Size(60, 20)
        grpProductSelection.Controls.Add(lblProductSearch)

        _txtProductSearch = New TextBox()
        _txtProductSearch.Location = New Point(80, 20)
        _txtProductSearch.Size = New Size(150, 30)
        grpProductSelection.Controls.Add(_txtProductSearch)

        Dim lblCategory As New Label()
        lblCategory.Text = "Danh mục:"
        lblCategory.Location = New Point(240, 20)
        lblCategory.Size = New Size(60, 20)
        grpProductSelection.Controls.Add(lblCategory)

        _cmbCategory = New ComboBox()
        _cmbCategory.Location = New Point(310, 20)
        _cmbCategory.Size = New Size(150, 30)
        grpProductSelection.Controls.Add(_cmbCategory)


        Dim lblSupplier As New Label()
        lblSupplier.Text = "Nhà cung cấp:"
        lblSupplier.Location = New Point(480, 20)
        lblSupplier.Size = New Size(80, 20)
        grpProductSelection.Controls.Add(lblSupplier)

        _cmbSupplier = New ComboBox()
        _cmbSupplier.Location = New Point(560, 20)
        _cmbSupplier.Size = New Size(150, 30)
        grpProductSelection.Controls.Add(_cmbSupplier)

        _gridProducts = New DataGridView()
        _gridProducts.Location = New Point(10, 60)
        _gridProducts.Size = New Size(500, 170)
        _gridProducts.AutoGenerateColumns = False
        _gridProducts.MultiSelect = False
        Dim chkSelect As New DataGridViewCheckBoxColumn()
        chkSelect.Name = "ChkSelect"
        chkSelect.HeaderText = "Chọn"
        chkSelect.Width = 50
        chkSelect.ReadOnly = False
        _gridProducts.Columns.Add(chkSelect)

        Dim colProductIdProducts As New DataGridViewTextBoxColumn()
        colProductIdProducts.Name = "ProductId_Products"
        colProductIdProducts.HeaderText = "Mã sản phẩm"
        colProductIdProducts.DataPropertyName = "ProductId"
        colProductIdProducts.Width = 80
        colProductIdProducts.ReadOnly = True
        _gridProducts.Columns.Add(colProductIdProducts)

        Dim colProductNameProducts As New DataGridViewTextBoxColumn()
        colProductNameProducts.Name = "ProductName_Products"
        colProductNameProducts.HeaderText = "Tên sản phẩm"
        colProductNameProducts.DataPropertyName = "ProductName"
        colProductNameProducts.Width = 200
        colProductNameProducts.ReadOnly = True
        _gridProducts.Columns.Add(colProductNameProducts)

        Dim colCategoryNameProducts As New DataGridViewTextBoxColumn()
        colCategoryNameProducts.Name = "CategoryName_Products"
        colCategoryNameProducts.HeaderText = "Danh mục"
        colCategoryNameProducts.DataPropertyName = "CategoryName"
        colCategoryNameProducts.Width = 100
        colCategoryNameProducts.ReadOnly = True
        _gridProducts.Columns.Add(colCategoryNameProducts)

        Dim colQuantityProducts As New DataGridViewTextBoxColumn()
        colQuantityProducts.Name = "Quantity_Products"
        colQuantityProducts.HeaderText = "Số lượng tồn"
        colQuantityProducts.DataPropertyName = "Quantity"
        colQuantityProducts.Width = 80
        colQuantityProducts.ReadOnly = True
        _gridProducts.Columns.Add(colQuantityProducts)

        grpProductSelection.Controls.Add(_gridProducts)

        Dim lblQuantity As New Label()
        lblQuantity.Text = "Số lượng:"
        lblQuantity.Location = New Point(520, 60)
        lblQuantity.Size = New Size(60, 20)
        grpProductSelection.Controls.Add(lblQuantity)

        _numQuantity = New NumericUpDown()
        _numQuantity.Location = New Point(590, 60)
        _numQuantity.Size = New Size(100, 30)
        _numQuantity.Minimum = 1
        _numQuantity.Maximum = 10000
        _numQuantity.Value = 1
        grpProductSelection.Controls.Add(_numQuantity)

        _btnAddProduct = New Button()
        _btnAddProduct.Text = "Thêm"
        _btnAddProduct.Location = New Point(590, 100)
        _btnAddProduct.Size = New Size(100, 30)
        grpProductSelection.Controls.Add(_btnAddProduct)

        ' GroupBox cho sản phẩm đã chọn
        Dim grpSelectedProducts As New GroupBox()
        grpSelectedProducts.Text = "Sản phẩm đã chọn"
        grpSelectedProducts.Location = New Point(10, 350)
        grpSelectedProducts.Size = New Size(760, 190)
        Me.Controls.Add(grpSelectedProducts)

        _gridSelectedProducts = New DataGridView()
        _gridSelectedProducts.Location = New Point(10, 20)
        _gridSelectedProducts.Size = New Size(620, 160)
        _gridSelectedProducts.AutoGenerateColumns = False
        _gridSelectedProducts.ReadOnly = True

        Dim colProductSelectedId As New DataGridViewTextBoxColumn()
        colProductSelectedId.Name = "ProductId_Selected"
        colProductSelectedId.HeaderText = "Mã sản phẩm"
        colProductSelectedId.DataPropertyName = "ProductId"
        colProductSelectedId.Width = 80
        _gridSelectedProducts.Columns.Add(colProductSelectedId)

        Dim colProductSelectedName As New DataGridViewTextBoxColumn()
        colProductSelectedName.Name = "ProductName_Selected"
        colProductSelectedName.HeaderText = "Tên sản phẩm"
        colProductSelectedName.DataPropertyName = "ProductName"
        colProductSelectedName.Width = 200
        _gridSelectedProducts.Columns.Add(colProductSelectedName)

        Dim colProductSelectedUnit As New DataGridViewTextBoxColumn()
        colProductSelectedUnit.Name = "Unit_Selected"
        colProductSelectedUnit.HeaderText = "Đơn vị"
        colProductSelectedUnit.DataPropertyName = "Unit"
        colProductSelectedUnit.Width = 80
        _gridSelectedProducts.Columns.Add(colProductSelectedUnit)

        Dim colProductSelectedQuantity As New DataGridViewTextBoxColumn()
        colProductSelectedQuantity.Name = "Quantity_Selected"
        colProductSelectedQuantity.HeaderText = "Số lượng"
        colProductSelectedQuantity.DataPropertyName = "Quantity"
        colProductSelectedQuantity.Width = 80
        colProductSelectedQuantity.ReadOnly = False
        _gridSelectedProducts.Columns.Add(colProductSelectedQuantity)

        Dim colProductSelectedNote As New DataGridViewTextBoxColumn()
        colProductSelectedNote.Name = "Note_Selected"
        colProductSelectedNote.HeaderText = "Ghi chú"
        colProductSelectedNote.DataPropertyName = "Note"
        colProductSelectedNote.Width = 150
        colProductSelectedNote.ReadOnly = False
        _gridSelectedProducts.Columns.Add(colProductSelectedNote)

        grpSelectedProducts.Controls.Add(_gridSelectedProducts)

        _btnRemoveProduct = New Button()
        _btnRemoveProduct.Text = "Xóa"
        _btnRemoveProduct.Location = New Point(640, 20)
        _btnRemoveProduct.Size = New Size(100, 30)
        grpSelectedProducts.Controls.Add(_btnRemoveProduct)

        ' Nút Save và Cancel
        _btnSave = New Button()
        _btnSave.Text = "Lưu"
        _btnSave.Location = New Point(570, 550)
        _btnSave.Size = New Size(100, 30)
        Me.Controls.Add(_btnSave)

        _btnCancel = New Button()
        _btnCancel.Text = "Hủy"
        _btnCancel.Location = New Point(680, 550)
        _btnCancel.Size = New Size(100, 30)
        Me.Controls.Add(_btnCancel)

        Me.ResumeLayout(False)
    End Sub
End Class