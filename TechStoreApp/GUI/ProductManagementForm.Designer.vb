<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ProductManagementForm
    Inherits System.Windows.Forms.Form

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer
    Friend WithEvents dgvProducts As System.Windows.Forms.DataGridView
    Friend WithEvents txtProductName As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents txtPrice As System.Windows.Forms.TextBox
    Friend WithEvents txtQuantity As System.Windows.Forms.TextBox
    Friend WithEvents txtUnit As System.Windows.Forms.TextBox
    Friend WithEvents txtMinStockLevel As System.Windows.Forms.TextBox
    Friend WithEvents cboCategory As System.Windows.Forms.ComboBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents lblPage As System.Windows.Forms.Label
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents lblProductName As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblPrice As System.Windows.Forms.Label
    Friend WithEvents lblQuantity As System.Windows.Forms.Label
    Friend WithEvents lblUnit As System.Windows.Forms.Label
    Friend WithEvents lblMinStockLevel As System.Windows.Forms.Label
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents txtSearchName As System.Windows.Forms.TextBox
    Friend WithEvents lblSearchName As System.Windows.Forms.Label
    Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cboSortBy As System.Windows.Forms.ComboBox
    Friend WithEvents lblSortBy As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnClearSearch As System.Windows.Forms.Button
    Friend WithEvents lblCategorySort As System.Windows.Forms.Label
    Friend WithEvents cboCategorySort As System.Windows.Forms.ComboBox
    Friend WithEvents ProductId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ProductName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Unit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Price As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Quantity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MinStockLevel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CategoryName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SupplierName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreatedBy As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreatedAt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IsActive As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents CategoryId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cboSupplier As System.Windows.Forms.ComboBox
    Friend WithEvents lblSupplier As System.Windows.Forms.Label

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvProducts = New System.Windows.Forms.DataGridView()
        Me.ProductId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Unit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Price = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MinStockLevel = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CategoryName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SupplierName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CreatedBy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CreatedAt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsActive = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.CategoryId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtProductName = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtPrice = New System.Windows.Forms.TextBox()
        Me.txtQuantity = New System.Windows.Forms.TextBox()
        Me.txtUnit = New System.Windows.Forms.TextBox()
        Me.txtMinStockLevel = New System.Windows.Forms.TextBox()
        Me.cboCategory = New System.Windows.Forms.ComboBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.lblPage = New System.Windows.Forms.Label()
        Me.lblError = New System.Windows.Forms.Label()
        Me.lblProductName = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.lblQuantity = New System.Windows.Forms.Label()
        Me.lblUnit = New System.Windows.Forms.Label()
        Me.lblMinStockLevel = New System.Windows.Forms.Label()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtSearchName = New System.Windows.Forms.TextBox()
        Me.lblSearchName = New System.Windows.Forms.Label()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.cboSortBy = New System.Windows.Forms.ComboBox()
        Me.lblSortBy = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnClearSearch = New System.Windows.Forms.Button()
        Me.lblCategorySort = New System.Windows.Forms.Label()
        Me.cboCategorySort = New System.Windows.Forms.ComboBox()
        Me.lblSupplier = New System.Windows.Forms.Label()
        Me.cboSupplier = New System.Windows.Forms.ComboBox()
        CType(Me.dgvProducts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvProducts
        '
        Me.dgvProducts.AllowUserToAddRows = False
        Me.dgvProducts.ColumnHeadersHeight = 29
        Me.dgvProducts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ProductId, Me.ProductName, Me.Description, Me.Unit, Me.Price, Me.Quantity, Me.MinStockLevel, Me.CategoryName, Me.SupplierName, Me.CreatedBy, Me.CreatedAt, Me.IsActive})
        Me.dgvProducts.Location = New System.Drawing.Point(20, 80)
        Me.dgvProducts.MultiSelect = False
        Me.dgvProducts.Name = "dgvProducts"
        Me.dgvProducts.ReadOnly = True
        Me.dgvProducts.RowHeadersWidth = 51
        Me.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvProducts.Size = New System.Drawing.Size(935, 180)
        Me.dgvProducts.TabIndex = 0
        '
        'ProductId
        '
        Me.ProductId.HeaderText = "ID"
        Me.ProductId.MinimumWidth = 6
        Me.ProductId.Name = "ProductId"
        Me.ProductId.ReadOnly = True
        Me.ProductId.Width = 80
        '
        'ProductName
        '
        Me.ProductName.HeaderText = "Tên sản phẩm"
        Me.ProductName.MinimumWidth = 6
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
        Me.ProductName.Width = 120
        '
        'Description
        '
        Me.Description.HeaderText = "Mô tả"
        Me.Description.MinimumWidth = 6
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.Width = 150
        '
        'Unit
        '
        Me.Unit.HeaderText = "Đơn vị"
        Me.Unit.MinimumWidth = 6
        Me.Unit.Name = "Unit"
        Me.Unit.ReadOnly = True
        Me.Unit.Width = 80
        '
        'Price
        '
        Me.Price.HeaderText = "Giá"
        Me.Price.MinimumWidth = 6
        Me.Price.Name = "Price"
        Me.Price.ReadOnly = True
        Me.Price.Width = 125
        '
        'Quantity
        '
        Me.Quantity.HeaderText = "Số lượng"
        Me.Quantity.MinimumWidth = 6
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ReadOnly = True
        Me.Quantity.Width = 80
        '
        'MinStockLevel
        '
        Me.MinStockLevel.HeaderText = "Tồn tối thiểu"
        Me.MinStockLevel.MinimumWidth = 6
        Me.MinStockLevel.Name = "MinStockLevel"
        Me.MinStockLevel.ReadOnly = True
        Me.MinStockLevel.Width = 80
        '
        'CategoryName
        '
        Me.CategoryName.HeaderText = "Danh mục"
        Me.CategoryName.MinimumWidth = 6
        Me.CategoryName.Name = "CategoryName"
        Me.CategoryName.ReadOnly = True
        Me.CategoryName.Width = 120
        '
        'CreatedBy
        '
        Me.CreatedBy.HeaderText = "Người tạo"
        Me.CreatedBy.MinimumWidth = 6
        Me.CreatedBy.Name = "CreatedBy"
        Me.CreatedBy.ReadOnly = True
        Me.CreatedBy.Width = 125
        '
        'CreatedAt
        '
        DataGridViewCellStyle1.Format = "dd/MM/yyyy"
        Me.CreatedAt.DefaultCellStyle = DataGridViewCellStyle1
        Me.CreatedAt.HeaderText = "Ngày tạo"
        Me.CreatedAt.MinimumWidth = 6
        Me.CreatedAt.Name = "CreatedAt"
        Me.CreatedAt.ReadOnly = True
        Me.CreatedAt.Width = 125
        '
        'IsActive
        '
        Me.IsActive.HeaderText = "Hoạt động"
        Me.IsActive.MinimumWidth = 6
        Me.IsActive.Name = "IsActive"
        Me.IsActive.ReadOnly = True
        Me.IsActive.Width = 80
        '
        'CategoryId
        '
        Me.CategoryId.HeaderText = "ID Danh mục"
        Me.CategoryId.MinimumWidth = 6
        Me.CategoryId.Name = "CategoryId"
        Me.CategoryId.ReadOnly = True
        Me.CategoryId.Width = 80

        '
        ' lblSupplier
        '
        Me.lblSupplier.Location = New System.Drawing.Point(20, 510)
        Me.lblSupplier.Name = "lblSupplier"
        Me.lblSupplier.Size = New System.Drawing.Size(100, 20)
        Me.lblSupplier.TabIndex = 39
        Me.lblSupplier.Text = "Nhà cung cấp:"
        '
        ' cboSupplier
        '
        Me.cboSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSupplier.Location = New System.Drawing.Point(130, 510)
        Me.cboSupplier.Name = "cboSupplier"
        Me.cboSupplier.Size = New System.Drawing.Size(200, 24)
        Me.cboSupplier.TabIndex = 40
        '
        'txtProductName
        '
        Me.txtProductName.Location = New System.Drawing.Point(130, 263)
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.Size = New System.Drawing.Size(200, 22)
        Me.txtProductName.TabIndex = 1
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(130, 298)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(200, 22)
        Me.txtDescription.TabIndex = 2
        '
        'txtPrice
        '
        Me.txtPrice.Location = New System.Drawing.Point(130, 333)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(200, 22)
        Me.txtPrice.TabIndex = 3
        '
        'txtQuantity
        '
        Me.txtQuantity.Location = New System.Drawing.Point(130, 368)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(200, 22)
        Me.txtQuantity.TabIndex = 4
        '
        'txtUnit
        '
        Me.txtUnit.Location = New System.Drawing.Point(130, 403)
        Me.txtUnit.Name = "txtUnit"
        Me.txtUnit.Size = New System.Drawing.Size(200, 22)
        Me.txtUnit.TabIndex = 5
        '
        'txtMinStockLevel
        '
        Me.txtMinStockLevel.Location = New System.Drawing.Point(130, 438)
        Me.txtMinStockLevel.Name = "txtMinStockLevel"
        Me.txtMinStockLevel.Size = New System.Drawing.Size(200, 22)
        Me.txtMinStockLevel.TabIndex = 6
        '
        'cboCategory
        '
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.Location = New System.Drawing.Point(130, 481)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(200, 24)
        Me.cboCategory.TabIndex = 10
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(400, 280)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 30)
        Me.btnAdd.TabIndex = 11
        Me.btnAdd.Text = "Thêm"
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(400, 315)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 30)
        Me.btnUpdate.TabIndex = 12
        Me.btnUpdate.Text = "Cập nhật"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(400, 350)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 13
        Me.btnDelete.Text = "Xóa"
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(770, 259)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(40, 30)
        Me.btnPrev.TabIndex = 14
        Me.btnPrev.Text = "←"
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(816, 259)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(40, 30)
        Me.btnNext.TabIndex = 15
        Me.btnNext.Text = "→"
        '
        'lblPage
        '
        Me.lblPage.Location = New System.Drawing.Point(862, 266)
        Me.lblPage.Name = "lblPage"
        Me.lblPage.Size = New System.Drawing.Size(120, 20)
        Me.lblPage.TabIndex = 16
        Me.lblPage.Text = "Trang 1/1"
        '
        'lblError
        '
        Me.lblError.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(260, 553)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(520, 30)
        Me.lblError.TabIndex = 17
        '
        'lblProductName
        '
        Me.lblProductName.Location = New System.Drawing.Point(20, 263)
        Me.lblProductName.Name = "lblProductName"
        Me.lblProductName.Size = New System.Drawing.Size(100, 20)
        Me.lblProductName.TabIndex = 18
        Me.lblProductName.Text = "Tên sản phẩm:"
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(20, 298)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(100, 20)
        Me.lblDescription.TabIndex = 19
        Me.lblDescription.Text = "Mô tả:"
        '
        'lblPrice
        '
        Me.lblPrice.Location = New System.Drawing.Point(20, 333)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(100, 20)
        Me.lblPrice.TabIndex = 20
        Me.lblPrice.Text = "Giá:"
        '
        'lblQuantity
        '
        Me.lblQuantity.Location = New System.Drawing.Point(20, 368)
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Size = New System.Drawing.Size(100, 20)
        Me.lblQuantity.TabIndex = 21
        Me.lblQuantity.Text = "Số lượng:"
        '
        'lblUnit
        '
        Me.lblUnit.Location = New System.Drawing.Point(20, 403)
        Me.lblUnit.Name = "lblUnit"
        Me.lblUnit.Size = New System.Drawing.Size(100, 20)
        Me.lblUnit.TabIndex = 22
        Me.lblUnit.Text = "Đơn vị:"
        '
        'lblMinStockLevel
        '
        Me.lblMinStockLevel.Location = New System.Drawing.Point(20, 438)
        Me.lblMinStockLevel.Name = "lblMinStockLevel"
        Me.lblMinStockLevel.Size = New System.Drawing.Size(100, 20)
        Me.lblMinStockLevel.TabIndex = 23
        Me.lblMinStockLevel.Text = "Tồn tối thiểu:"
        '
        'lblCategory
        '
        Me.lblCategory.Location = New System.Drawing.Point(20, 481)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(100, 20)
        Me.lblCategory.TabIndex = 27
        Me.lblCategory.Text = "Danh mục:"
        '
        'btnCancel
        '
        Me.btnCancel.Enabled = False
        Me.btnCancel.Location = New System.Drawing.Point(400, 385)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 28
        Me.btnCancel.Text = "Hủy"
        '
        'txtSearchName
        '
        Me.txtSearchName.Location = New System.Drawing.Point(130, 20)
        Me.txtSearchName.Name = "txtSearchName"
        Me.txtSearchName.Size = New System.Drawing.Size(200, 22)
        Me.txtSearchName.TabIndex = 29
        '
        'lblSearchName
        '
        Me.lblSearchName.Location = New System.Drawing.Point(20, 20)
        Me.lblSearchName.Name = "lblSearchName"
        Me.lblSearchName.Size = New System.Drawing.Size(100, 20)
        Me.lblSearchName.TabIndex = 30
        Me.lblSearchName.Text = "Tên sản phẩm:"
        '
        'cboStatus
        '
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Location = New System.Drawing.Point(130, 48)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(200, 24)
        Me.cboStatus.TabIndex = 31
        '
        'lblStatus
        '
        Me.lblStatus.Location = New System.Drawing.Point(20, 48)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(100, 20)
        Me.lblStatus.TabIndex = 32
        Me.lblStatus.Text = "Trạng thái:"
        '
        'cboSortBy
        '
        Me.cboSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSortBy.Location = New System.Drawing.Point(400, 20)
        Me.cboSortBy.Name = "cboSortBy"
        Me.cboSortBy.Size = New System.Drawing.Size(200, 24)
        Me.cboSortBy.TabIndex = 33
        '
        'lblSortBy
        '
        Me.lblSortBy.Location = New System.Drawing.Point(350, 20)
        Me.lblSortBy.Name = "lblSortBy"
        Me.lblSortBy.Size = New System.Drawing.Size(50, 20)
        Me.lblSortBy.TabIndex = 34
        Me.lblSortBy.Text = "Sắp xếp:"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(650, 20)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 35
        Me.btnSearch.Text = "Tìm kiếm"
        '
        'btnClearSearch
        '
        Me.btnClearSearch.Location = New System.Drawing.Point(760, 20)
        Me.btnClearSearch.Name = "btnClearSearch"
        Me.btnClearSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnClearSearch.TabIndex = 36
        Me.btnClearSearch.Text = "Xóa tìm kiếm"
        '
        'lblCategorySort
        '
        Me.lblCategorySort.AutoSize = True
        Me.lblCategorySort.Location = New System.Drawing.Point(350, 52)
        Me.lblCategorySort.Name = "lblCategorySort"
        Me.lblCategorySort.Size = New System.Drawing.Size(67, 16)
        Me.lblCategorySort.TabIndex = 37
        Me.lblCategorySort.Text = "Danh mục:"
        '
        'cboCategorySort
        '
        Me.cboCategorySort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategorySort.Location = New System.Drawing.Point(400, 48)
        Me.cboCategorySort.Name = "cboCategorySort"
        Me.cboCategorySort.Size = New System.Drawing.Size(200, 24)
        Me.cboCategorySort.TabIndex = 38
        '
        'ProductManagementForm
        '
        Me.ClientSize = New System.Drawing.Size(960, 694)
        Me.Controls.Add(Me.cboCategorySort)
        Me.Controls.Add(Me.lblCategorySort)
        Me.Controls.Add(Me.txtSearchName)
        Me.Controls.Add(Me.lblSearchName)
        Me.Controls.Add(Me.cboStatus)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cboSortBy)
        Me.Controls.Add(Me.lblSortBy)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnClearSearch)
        Me.Controls.Add(Me.dgvProducts)
        Me.Controls.Add(Me.txtProductName)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtPrice)
        Me.Controls.Add(Me.txtQuantity)
        Me.Controls.Add(Me.txtUnit)
        Me.Controls.Add(Me.txtMinStockLevel)
        Me.Controls.Add(Me.cboCategory)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnPrev)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.lblPage)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.lblProductName)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblPrice)
        Me.Controls.Add(Me.lblQuantity)
        Me.Controls.Add(Me.lblUnit)
        Me.Controls.Add(Me.lblMinStockLevel)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.cboSupplier)
        Me.Controls.Add(Me.lblSupplier)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "ProductManagementForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Quản Lý Sản Phẩm"
        CType(Me.dgvProducts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
End Class