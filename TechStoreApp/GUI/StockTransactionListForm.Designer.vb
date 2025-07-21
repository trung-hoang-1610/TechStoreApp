Partial Class StockTransactionListForm
    Inherits System.Windows.Forms.Form

    Private Sub InitializeComponent()
        Me._tabControl = New System.Windows.Forms.TabControl()
        Me.tabIn = New System.Windows.Forms.TabPage()
        Me._gridIn = New System.Windows.Forms.DataGridView()
        Me._btnCreateIn = New System.Windows.Forms.Button()
        Me.tabOut = New System.Windows.Forms.TabPage()
        Me._gridOut = New System.Windows.Forms.DataGridView()
        Me._btnCreateOut = New System.Windows.Forms.Button()
        Me.tabStats = New System.Windows.Forms.TabPage()
        Me._lblTotalIn = New System.Windows.Forms.Label()
        Me._lblTotalOut = New System.Windows.Forms.Label()
        Me._lblTotalValue = New System.Windows.Forms.Label()
        Me._lblStatusBreakdown = New System.Windows.Forms.Label()
        Me._gridStats = New System.Windows.Forms.DataGridView()
        Me._gridLowStock = New System.Windows.Forms.DataGridView()
        Me._btnExportCsv = New System.Windows.Forms.Button()
        Me._lblStatsHeader = New System.Windows.Forms.Label()
        Me._lblLowStockHeader = New System.Windows.Forms.Label()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me._btnViewDetails = New System.Windows.Forms.Button()
        Me._btnApprove = New System.Windows.Forms.Button()
        Me._txtSearch = New System.Windows.Forms.TextBox()
        Me._cmbStatus = New System.Windows.Forms.ComboBox()
        Me._tabControl.SuspendLayout()
        Me.tabIn.SuspendLayout()
        CType(Me._gridIn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabOut.SuspendLayout()
        CType(Me._gridOut, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabStats.SuspendLayout()
        CType(Me._gridStats, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._gridLowStock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_tabControl
        '
        Me._tabControl.Controls.Add(Me.tabIn)
        Me._tabControl.Controls.Add(Me.tabOut)
        Me._tabControl.Controls.Add(Me.tabStats)
        Me._tabControl.Location = New System.Drawing.Point(10, 50)
        Me._tabControl.Name = "_tabControl"
        Me._tabControl.SelectedIndex = 0
        Me._tabControl.Size = New System.Drawing.Size(860, 500)
        Me._tabControl.TabIndex = 0
        '
        'tabIn
        '
        Me.tabIn.Controls.Add(Me._gridIn)
        Me.tabIn.Controls.Add(Me._btnCreateIn)
        Me.tabIn.Location = New System.Drawing.Point(4, 25)
        Me.tabIn.Name = "tabIn"
        Me.tabIn.Size = New System.Drawing.Size(852, 471)
        Me.tabIn.TabIndex = 0
        Me.tabIn.Text = "Phiếu nhập"
        '
        '_gridIn
        '
        Me._gridIn.ColumnHeadersHeight = 29
        Me._gridIn.Location = New System.Drawing.Point(10, 50)
        Me._gridIn.Name = "_gridIn"
        Me._gridIn.ReadOnly = True
        Me._gridIn.RowHeadersWidth = 51
        Me._gridIn.Size = New System.Drawing.Size(830, 360)
        Me._gridIn.TabIndex = 0
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
        colCreatedAtIn.DefaultCellStyle.Format = "dd/MM/yyyy"
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
        colApprovedAtIn.DataPropertyName = "ApprovedAtString"
        colApprovedAtIn.Width = 100
        colApprovedAtIn.DefaultCellStyle.Format = "dd/MM/yyyy"
        _gridIn.Columns.Add(colApprovedAtIn)
        '
        '_btnCreateIn
        '
        Me._btnCreateIn.Location = New System.Drawing.Point(10, 10)
        Me._btnCreateIn.Name = "_btnCreateIn"
        Me._btnCreateIn.Size = New System.Drawing.Size(100, 30)
        Me._btnCreateIn.TabIndex = 1
        Me._btnCreateIn.Text = "Tạo phiếu nhập"
        '
        'tabOut
        '
        Me.tabOut.Controls.Add(Me._gridOut)
        Me.tabOut.Controls.Add(Me._btnCreateOut)
        Me.tabOut.Location = New System.Drawing.Point(4, 25)
        Me.tabOut.Name = "tabOut"
        Me.tabOut.Size = New System.Drawing.Size(852, 471)
        Me.tabOut.TabIndex = 1
        Me.tabOut.Text = "Phiếu xuất"
        '
        '_gridOut
        '
        Me._gridOut.ColumnHeadersHeight = 29
        Me._gridOut.Location = New System.Drawing.Point(10, 50)
        Me._gridOut.Name = "_gridOut"
        Me._gridOut.ReadOnly = True
        Me._gridOut.RowHeadersWidth = 51
        Me._gridOut.Size = New System.Drawing.Size(760, 360)
        Me._gridOut.TabIndex = 0


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
        colCreatedAtOut.DefaultCellStyle.Format = "dd/MM/yyyy"
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
        colApprovedAtOut.DataPropertyName = "ApprovedAtString"
        colApprovedAtOut.Width = 100
        colApprovedAtOut.DefaultCellStyle.Format = "dd/MM/yyyy"
        _gridOut.Columns.Add(colApprovedAtOut)
        '
        '_btnCreateOut
        '
        Me._btnCreateOut.Location = New System.Drawing.Point(10, 10)
        Me._btnCreateOut.Name = "_btnCreateOut"
        Me._btnCreateOut.Size = New System.Drawing.Size(100, 30)
        Me._btnCreateOut.TabIndex = 1
        Me._btnCreateOut.Text = "Tạo phiếu xuất"
        '
        'tabStats
        '
        Me.tabStats.Controls.Add(Me._lblTotalIn)
        Me.tabStats.Controls.Add(Me._lblTotalOut)
        Me.tabStats.Controls.Add(Me._lblTotalValue)
        Me.tabStats.Controls.Add(Me._lblStatusBreakdown)
        Me.tabStats.Controls.Add(Me._gridStats)
        Me.tabStats.Controls.Add(Me._gridLowStock)
        Me.tabStats.Controls.Add(Me._btnExportCsv)
        Me.tabStats.Controls.Add(Me._lblStatsHeader)
        Me.tabStats.Controls.Add(Me._lblLowStockHeader)
        Me.tabStats.Location = New System.Drawing.Point(4, 25)
        Me.tabStats.Name = "tabStats"
        Me.tabStats.Size = New System.Drawing.Size(852, 471)
        Me.tabStats.TabIndex = 2
        Me.tabStats.Text = "Thống kê"
        '
        '_lblTotalIn
        '
        Me._lblTotalIn.AutoSize = True
        Me._lblTotalIn.Location = New System.Drawing.Point(10, 10)
        Me._lblTotalIn.Name = "_lblTotalIn"
        Me._lblTotalIn.Size = New System.Drawing.Size(121, 16)
        Me._lblTotalIn.TabIndex = 0
        Me._lblTotalIn.Text = "Tổng phiếu nhập: 0"
        '
        '_lblTotalOut
        '
        Me._lblTotalOut.AutoSize = True
        Me._lblTotalOut.Location = New System.Drawing.Point(10, 35)
        Me._lblTotalOut.Name = "_lblTotalOut"
        Me._lblTotalOut.Size = New System.Drawing.Size(115, 16)
        Me._lblTotalOut.TabIndex = 1
        Me._lblTotalOut.Text = "Tổng phiếu xuất: 0"
        '
        '_lblTotalValue
        '
        Me._lblTotalValue.AutoSize = True
        Me._lblTotalValue.Location = New System.Drawing.Point(187, 10)
        Me._lblTotalValue.Name = "_lblTotalValue"
        Me._lblTotalValue.Size = New System.Drawing.Size(145, 16)
        Me._lblTotalValue.TabIndex = 2
        Me._lblTotalValue.Text = "Tổng giá trị giao dịch: 0"
        '
        '_lblStatusBreakdown
        '
        Me._lblStatusBreakdown.AutoSize = True
        Me._lblStatusBreakdown.Location = New System.Drawing.Point(187, 35)
        Me._lblStatusBreakdown.Name = "_lblStatusBreakdown"
        Me._lblStatusBreakdown.Size = New System.Drawing.Size(120, 16)
        Me._lblStatusBreakdown.TabIndex = 3
        Me._lblStatusBreakdown.Text = "Tình trạng: Chưa tải"
        '
        '_gridStats
        '
        Me._gridStats.ColumnHeadersHeight = 29
        Me._gridStats.Location = New System.Drawing.Point(10, 130)
        Me._gridStats.Name = "_gridStats"
        Me._gridStats.ReadOnly = True
        Me._gridStats.RowHeadersWidth = 51
        Me._gridStats.Size = New System.Drawing.Size(820, 130)
        Me._gridStats.TabIndex = 4
        '
        '_gridLowStock
        '
        Me._gridLowStock.ColumnHeadersHeight = 29
        Me._gridLowStock.Location = New System.Drawing.Point(10, 310)
        Me._gridLowStock.Name = "_gridLowStock"
        Me._gridLowStock.ReadOnly = True
        Me._gridLowStock.RowHeadersWidth = 51
        Me._gridLowStock.Size = New System.Drawing.Size(820, 130)
        Me._gridLowStock.TabIndex = 5
        '
        '_btnExportCsv
        '
        Me._btnExportCsv.Location = New System.Drawing.Point(730, 10)
        Me._btnExportCsv.Name = "_btnExportCsv"
        Me._btnExportCsv.Size = New System.Drawing.Size(100, 30)
        Me._btnExportCsv.TabIndex = 6
        Me._btnExportCsv.Text = "Xuất CSV"
        '
        '_lblStatsHeader
        '
        Me._lblStatsHeader.AutoSize = True
        Me._lblStatsHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._lblStatsHeader.Location = New System.Drawing.Point(10, 100)
        Me._lblStatsHeader.Name = "_lblStatsHeader"
        Me._lblStatsHeader.Size = New System.Drawing.Size(208, 23)
        Me._lblStatsHeader.TabIndex = 7
        Me._lblStatsHeader.Text = "Thống kê theo sản phẩm"
        '
        '_lblLowStockHeader
        '
        Me._lblLowStockHeader.AutoSize = True
        Me._lblLowStockHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._lblLowStockHeader.Location = New System.Drawing.Point(10, 280)
        Me._lblLowStockHeader.Name = "_lblLowStockHeader"
        Me._lblLowStockHeader.Size = New System.Drawing.Size(202, 23)
        Me._lblLowStockHeader.TabIndex = 8
        Me._lblLowStockHeader.Text = "Danh sách gần hết hàng"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Mã sản phẩm"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 125
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Tên sản phẩm"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 125
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Tổng số lượng"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 125
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Tổng giá trị"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 125
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Mã sản phẩm"
        Me.DataGridViewTextBoxColumn5.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 125
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Tên sản phẩm"
        Me.DataGridViewTextBoxColumn6.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 125
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Tồn kho hiện tại"
        Me.DataGridViewTextBoxColumn7.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Width = 125
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Tồn kho tối thiểu"
        Me.DataGridViewTextBoxColumn8.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        Me.DataGridViewTextBoxColumn8.Width = 125
        '
        '_btnViewDetails
        '
        Me._btnViewDetails.Enabled = False
        Me._btnViewDetails.Location = New System.Drawing.Point(120, 10)
        Me._btnViewDetails.Name = "_btnViewDetails"
        Me._btnViewDetails.Size = New System.Drawing.Size(100, 30)
        Me._btnViewDetails.TabIndex = 1
        Me._btnViewDetails.Text = "Xem chi tiết"
        '
        '_btnApprove
        '
        Me._btnApprove.Enabled = False
        Me._btnApprove.Location = New System.Drawing.Point(230, 10)
        Me._btnApprove.Name = "_btnApprove"
        Me._btnApprove.Size = New System.Drawing.Size(100, 30)
        Me._btnApprove.TabIndex = 2
        Me._btnApprove.Text = "Duyệt phiếu"
        Me._btnApprove.Visible = False
        '
        '_txtSearch
        '
        Me._txtSearch.Location = New System.Drawing.Point(340, 10)
        Me._txtSearch.Name = "_txtSearch"
        Me._txtSearch.Size = New System.Drawing.Size(200, 22)
        Me._txtSearch.TabIndex = 3
        '
        '_cmbStatus
        '
        Me._cmbStatus.Location = New System.Drawing.Point(550, 10)
        Me._cmbStatus.Name = "_cmbStatus"
        Me._cmbStatus.Size = New System.Drawing.Size(100, 24)
        Me._cmbStatus.TabIndex = 4
        '
        'StockTransactionListForm
        '
        Me.ClientSize = New System.Drawing.Size(882, 553)
        Me.Controls.Add(Me._tabControl)
        Me.Controls.Add(Me._btnViewDetails)
        Me.Controls.Add(Me._btnApprove)
        Me.Controls.Add(Me._txtSearch)
        Me.Controls.Add(Me._cmbStatus)
        Me.Name = "StockTransactionListForm"
        Me.Text = "Danh sách phiếu nhập/xuất"
        Me._tabControl.ResumeLayout(False)
        Me.tabIn.ResumeLayout(False)
        CType(Me._gridIn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabOut.ResumeLayout(False)
        CType(Me._gridOut, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabStats.ResumeLayout(False)
        Me.tabStats.PerformLayout()
        CType(Me._gridStats, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._gridLowStock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tabIn As TabPage
    Friend WithEvents tabOut As TabPage
    Friend WithEvents _lblTotalIn As Label
    Friend WithEvents _lblTotalOut As Label
    Friend WithEvents _lblTotalValue As Label
    Friend WithEvents _lblStatusBreakdown As Label
    Friend WithEvents _lblStatsHeader As Label
    Friend WithEvents _lblLowStockHeader As Label
    Friend WithEvents _gridStats As DataGridView
    Friend WithEvents _gridLowStock As DataGridView
    Friend WithEvents _btnExportCsv As Button
    Friend WithEvents tabStats As TabPage
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
End Class