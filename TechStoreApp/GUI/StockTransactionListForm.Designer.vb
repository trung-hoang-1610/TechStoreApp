Partial Class StockTransactionListForm
    Inherits System.Windows.Forms.Form

    Private Sub InitializeComponent()
        Me.SuspendLayout()

        ' Thiết lập form
        Me.Text = "Danh sách phiếu nhập/xuất"
        Me.Size = New System.Drawing.Size(800, 600)

        ' Khởi tạo TabControl
        _tabControl = New TabControl()
        _tabControl.Size = New System.Drawing.Size(780, 500)
        _tabControl.Location = New System.Drawing.Point(10, 50)
        Dim tabIn As New TabPage("Phiếu nhập")
        Dim tabOut As New TabPage("Phiếu xuất")
        _tabControl.TabPages.Add(tabIn)
        _tabControl.TabPages.Add(tabOut)
        Me.Controls.Add(_tabControl)

        ' Khởi tạo và thiết lập DataGridView cho phiếu nhập
        _gridIn = New DataGridView()
        _gridIn.Size = New System.Drawing.Size(760, 360)
        _gridIn.Location = New System.Drawing.Point(10, 50)
        _gridIn.AutoGenerateColumns = False

        Dim colTransactionIdIn As New DataGridViewTextBoxColumn()
        colTransactionIdIn.Name = "TransactionId"
        colTransactionIdIn.HeaderText = "Mã phiếu"
        colTransactionIdIn.DataPropertyName = "TransactionId"
        colTransactionIdIn.Width = 80
        _gridIn.Columns.Add(colTransactionIdIn)

        Dim colTransactionCodeIn As New DataGridViewTextBoxColumn()
        colTransactionCodeIn.Name = "TransactionCode"
        colTransactionCodeIn.HeaderText = "Mã giao dịch"
        colTransactionCodeIn.DataPropertyName = "TransactionCode"
        colTransactionCodeIn.Width = 120
        _gridIn.Columns.Add(colTransactionCodeIn)

        Dim colCreatedByNameIn As New DataGridViewTextBoxColumn()
        colCreatedByNameIn.Name = "CreatedByName"
        colCreatedByNameIn.HeaderText = "Người tạo"
        colCreatedByNameIn.DataPropertyName = "CreatedByName"
        colCreatedByNameIn.Width = 120
        _gridIn.Columns.Add(colCreatedByNameIn)

        Dim colCreatedAtIn As New DataGridViewTextBoxColumn()
        colCreatedAtIn.Name = "CreatedAt"
        colCreatedAtIn.HeaderText = "Ngày tạo"
        colCreatedAtIn.DataPropertyName = "CreatedAt"
        colCreatedAtIn.Width = 100
        _gridIn.Columns.Add(colCreatedAtIn)

        Dim colSupplierNameIn As New DataGridViewTextBoxColumn()
        colSupplierNameIn.Name = "SupplierName"
        colSupplierNameIn.HeaderText = "Nhà cung cấp"
        colSupplierNameIn.DataPropertyName = "SupplierName"
        colSupplierNameIn.Width = 150
        _gridIn.Columns.Add(colSupplierNameIn)

        Dim colStatusIn As New DataGridViewTextBoxColumn()
        colStatusIn.Name = "Status"
        colStatusIn.HeaderText = "Trạng thái"
        colStatusIn.DataPropertyName = "Status"
        colStatusIn.Width = 100
        _gridIn.Columns.Add(colStatusIn)

        Dim colApprovedByNameIn As New DataGridViewTextBoxColumn()
        colApprovedByNameIn.Name = "ApprovedByName"
        colApprovedByNameIn.HeaderText = "Người duyệt"
        colApprovedByNameIn.DataPropertyName = "ApprovedByName"
        colApprovedByNameIn.Width = 120
        _gridIn.Columns.Add(colApprovedByNameIn)

        Dim colApprovedAtIn As New DataGridViewTextBoxColumn()
        colApprovedAtIn.Name = "ApprovedAt"
        colApprovedAtIn.HeaderText = "Ngày duyệt"
        colApprovedAtIn.DataPropertyName = "ApprovedAt"
        colApprovedAtIn.Width = 100
        _gridIn.Columns.Add(colApprovedAtIn)

        _gridIn.Visible = True
        _gridIn.Enabled = True
        tabIn.Controls.Add(_gridIn)

        ' Khởi tạo và thiết lập DataGridView cho phiếu xuất
        _gridOut = New DataGridView()
        _gridOut.Size = New System.Drawing.Size(760, 360)
        _gridOut.Location = New System.Drawing.Point(10, 50)
        _gridOut.AutoGenerateColumns = False

        Dim colTransactionIdOut As New DataGridViewTextBoxColumn()
        colTransactionIdOut.Name = "TransactionIdOut"
        colTransactionIdOut.HeaderText = "Mã phiếu"
        colTransactionIdOut.DataPropertyName = "TransactionId"
        colTransactionIdOut.Width = 80
        _gridOut.Columns.Add(colTransactionIdOut)

        Dim colTransactionCodeOut As New DataGridViewTextBoxColumn()
        colTransactionCodeOut.Name = "TransactionCodeOut"
        colTransactionCodeOut.HeaderText = "Mã giao dịch"
        colTransactionCodeOut.DataPropertyName = "TransactionCode"
        colTransactionCodeOut.Width = 120
        _gridOut.Columns.Add(colTransactionCodeOut)

        Dim colCreatedByNameOut As New DataGridViewTextBoxColumn()
        colCreatedByNameOut.Name = "CreatedByNameOut"
        colCreatedByNameOut.HeaderText = "Người tạo"
        colCreatedByNameOut.DataPropertyName = "CreatedByName"
        colCreatedByNameOut.Width = 120
        _gridOut.Columns.Add(colCreatedByNameOut)

        Dim colCreatedAtOut As New DataGridViewTextBoxColumn()
        colCreatedAtOut.Name = "CreatedAtOut"
        colCreatedAtOut.HeaderText = "Ngày tạo"
        colCreatedAtOut.DataPropertyName = "CreatedAt"
        colCreatedAtOut.Width = 100
        _gridOut.Columns.Add(colCreatedAtOut)

        Dim colStatusOut As New DataGridViewTextBoxColumn()
        colStatusOut.Name = "StatusOut"
        colStatusOut.HeaderText = "Trạng thái"
        colStatusOut.DataPropertyName = "Status"
        colStatusOut.Width = 100
        _gridOut.Columns.Add(colStatusOut)

        Dim colApprovedByNameOut As New DataGridViewTextBoxColumn()
        colApprovedByNameOut.Name = "ApprovedByNameOut"
        colApprovedByNameOut.HeaderText = "Người duyệt"
        colApprovedByNameOut.DataPropertyName = "ApprovedByName"
        colApprovedByNameOut.Width = 120
        _gridOut.Columns.Add(colApprovedByNameOut)

        Dim colApprovedAtOut As New DataGridViewTextBoxColumn()
        colApprovedAtOut.Name = "ApprovedAtOut"
        colApprovedAtOut.HeaderText = "Ngày duyệt"
        colApprovedAtOut.DataPropertyName = "ApprovedAt"
        colApprovedAtOut.Width = 100
        _gridOut.Columns.Add(colApprovedAtOut)

        _gridOut.Visible = True
        _gridOut.Enabled = True
        tabOut.Controls.Add(_gridOut)

        ' Khởi tạo và thiết lập các nút
        _btnCreateIn = New Button()
        _btnCreateIn.Text = "Tạo phiếu nhập"
        _btnCreateIn.Size = New System.Drawing.Size(100, 30)
        _btnCreateIn.Location = New System.Drawing.Point(10, 10)
        tabIn.Controls.Add(_btnCreateIn)
        _btnCreateIn.BringToFront()

        _btnCreateOut = New Button()
        _btnCreateOut.Text = "Tạo phiếu xuất"
        _btnCreateOut.Size = New System.Drawing.Size(100, 30)
        _btnCreateOut.Location = New System.Drawing.Point(10, 10)
        tabOut.Controls.Add(_btnCreateOut)
        _btnCreateOut.BringToFront()

        _btnViewDetails = New Button()
        _btnViewDetails.Text = "Xem chi tiết"
        _btnViewDetails.Size = New System.Drawing.Size(100, 30)
        _btnViewDetails.Location = New System.Drawing.Point(120, 10)
        _btnViewDetails.Enabled = False
        Me.Controls.Add(_btnViewDetails)

        _btnApprove = New Button()
        _btnApprove.Text = "Duyệt phiếu"
        _btnApprove.Size = New System.Drawing.Size(100, 30)
        _btnApprove.Location = New System.Drawing.Point(230, 10)
        _btnApprove.Enabled = False
        _btnApprove.Visible = False ' Mặc định ẩn, sẽ được cấu hình trong ConfigureControls
        Me.Controls.Add(_btnApprove)

        ' Khởi tạo và thiết lập tìm kiếm
        _txtSearch = New TextBox()
        _txtSearch.Location = New System.Drawing.Point(340, 10)
        _txtSearch.Size = New System.Drawing.Size(200, 30)
        Me.Controls.Add(_txtSearch)

        _cmbStatus = New ComboBox()
        _cmbStatus.Location = New System.Drawing.Point(550, 10)
        _cmbStatus.Size = New System.Drawing.Size(100, 30)
        Me.Controls.Add(_cmbStatus)

        Me.ResumeLayout(False)
    End Sub
End Class