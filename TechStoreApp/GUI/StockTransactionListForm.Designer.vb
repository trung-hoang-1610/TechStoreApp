Imports System.ComponentModel

Partial Class StockTransactionListForm
    Inherits System.Windows.Forms.Form
    Private WithEvents _backgroundWorkerLoad As BackgroundWorker
    Private WithEvents _backgroundWorkerStats As BackgroundWorker

    Private WithEvents _tabControl As TabControl
    Private WithEvents _gridIn As DataGridView
    Private WithEvents _gridOut As DataGridView
    Private WithEvents _btnCreateIn As Button
    Private WithEvents _btnCreateOut As Button
    Private WithEvents _btnViewDetails As Button
    Private WithEvents _btnApprove As Button
    Private WithEvents _txtSearch As TextBox
    Private WithEvents _cmbStatus As ComboBox

    Friend WithEvents _btnPrevPageIn As Button
    Friend WithEvents _btnNextPageIn As Button
    Friend WithEvents _lblPagingStatusIn As Label

    Friend WithEvents _btnPrevPageOut As Button
    Friend WithEvents _btnNextPageOut As Button
    Friend WithEvents _lblPagingStatusOut As Label

    Private ReadOnly _criteriaIn As StockTransationSearchCriterialDTO
    Private ReadOnly _criteriaOut As StockTransationSearchCriterialDTO
    Private Sub InitializeComponent()
        Me._tabControl = New System.Windows.Forms.TabControl()
        Me.tabIn = New System.Windows.Forms.TabPage()
        Me._gridIn = New System.Windows.Forms.DataGridView()
        Me._btnCreateIn = New System.Windows.Forms.Button()
        Me._btnPrevPageIn = New System.Windows.Forms.Button()
        Me._btnNextPageIn = New System.Windows.Forms.Button()
        Me._lblPagingStatusIn = New System.Windows.Forms.Label()
        Me.tabOut = New System.Windows.Forms.TabPage()
        Me._gridOut = New System.Windows.Forms.DataGridView()
        Me._btnCreateOut = New System.Windows.Forms.Button()
        Me._btnPrevPageOut = New System.Windows.Forms.Button()
        Me._btnNextPageOut = New System.Windows.Forms.Button()
        Me._lblPagingStatusOut = New System.Windows.Forms.Label()
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
        Me.Label1 = New System.Windows.Forms.Label()
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
        Me.tabIn.Controls.Add(Me._btnPrevPageIn)
        Me.tabIn.Controls.Add(Me._btnNextPageIn)
        Me.tabIn.Controls.Add(Me._lblPagingStatusIn)
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
        '
        '_btnCreateIn
        '
        Me._btnCreateIn.Location = New System.Drawing.Point(10, 10)
        Me._btnCreateIn.Name = "_btnCreateIn"
        Me._btnCreateIn.Size = New System.Drawing.Size(100, 30)
        Me._btnCreateIn.TabIndex = 1
        Me._btnCreateIn.Text = "Tạo phiếu nhập"
        '
        '_btnPrevPageIn
        '
        Me._btnPrevPageIn.Location = New System.Drawing.Point(10, 420)
        Me._btnPrevPageIn.Name = "_btnPrevPageIn"
        Me._btnPrevPageIn.Size = New System.Drawing.Size(80, 28)
        Me._btnPrevPageIn.TabIndex = 2
        Me._btnPrevPageIn.Text = "Trang trước"
        '
        '_btnNextPageIn
        '
        Me._btnNextPageIn.Location = New System.Drawing.Point(100, 420)
        Me._btnNextPageIn.Name = "_btnNextPageIn"
        Me._btnNextPageIn.Size = New System.Drawing.Size(80, 28)
        Me._btnNextPageIn.TabIndex = 3
        Me._btnNextPageIn.Text = "Trang sau"
        '
        '_lblPagingStatusIn
        '
        Me._lblPagingStatusIn.Location = New System.Drawing.Point(200, 425)
        Me._lblPagingStatusIn.Name = "_lblPagingStatusIn"
        Me._lblPagingStatusIn.Size = New System.Drawing.Size(250, 22)
        Me._lblPagingStatusIn.TabIndex = 4
        Me._lblPagingStatusIn.Text = "Trang 1 / 1 — Hiển thị 0 / 0 phiếu"
        '
        'tabOut
        '
        Me.tabOut.Controls.Add(Me._gridOut)
        Me.tabOut.Controls.Add(Me._btnCreateOut)
        Me.tabOut.Controls.Add(Me._btnPrevPageOut)
        Me.tabOut.Controls.Add(Me._btnNextPageOut)
        Me.tabOut.Controls.Add(Me._lblPagingStatusOut)
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
        '
        '_btnCreateOut
        '
        Me._btnCreateOut.Location = New System.Drawing.Point(10, 10)
        Me._btnCreateOut.Name = "_btnCreateOut"
        Me._btnCreateOut.Size = New System.Drawing.Size(100, 30)
        Me._btnCreateOut.TabIndex = 1
        Me._btnCreateOut.Text = "Tạo phiếu xuất"
        '
        '_btnPrevPageOut
        '
        Me._btnPrevPageOut.Location = New System.Drawing.Point(10, 420)
        Me._btnPrevPageOut.Name = "_btnPrevPageOut"
        Me._btnPrevPageOut.Size = New System.Drawing.Size(80, 28)
        Me._btnPrevPageOut.TabIndex = 2
        Me._btnPrevPageOut.Text = "Trang trước"
        '
        '_btnNextPageOut
        '
        Me._btnNextPageOut.Location = New System.Drawing.Point(100, 420)
        Me._btnNextPageOut.Name = "_btnNextPageOut"
        Me._btnNextPageOut.Size = New System.Drawing.Size(80, 28)
        Me._btnNextPageOut.TabIndex = 3
        Me._btnNextPageOut.Text = "Trang sau"
        '
        '_lblPagingStatusOut
        '
        Me._lblPagingStatusOut.Location = New System.Drawing.Point(200, 425)
        Me._lblPagingStatusOut.Name = "_lblPagingStatusOut"
        Me._lblPagingStatusOut.Size = New System.Drawing.Size(250, 22)
        Me._lblPagingStatusOut.TabIndex = 4
        Me._lblPagingStatusOut.Text = "Trang 1 / 1 — Hiển thị 0 / 0 phiếu"
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
        Me._gridStats.Location = New System.Drawing.Point(7, 106)
        Me._gridStats.Name = "_gridStats"
        Me._gridStats.ReadOnly = True
        Me._gridStats.RowHeadersWidth = 51
        Me._gridStats.Size = New System.Drawing.Size(585, 176)
        Me._gridStats.TabIndex = 4
        '
        '_gridLowStock
        '
        Me._gridLowStock.ColumnHeadersHeight = 29
        Me._gridLowStock.Location = New System.Drawing.Point(8, 324)
        Me._gridLowStock.Name = "_gridLowStock"
        Me._gridLowStock.ReadOnly = True
        Me._gridLowStock.RowHeadersWidth = 51
        Me._gridLowStock.Size = New System.Drawing.Size(584, 130)
        Me._gridLowStock.TabIndex = 5
        '
        '_btnExportCsv
        '
        Me._btnExportCsv.Location = New System.Drawing.Point(609, 3)
        Me._btnExportCsv.Name = "_btnExportCsv"
        Me._btnExportCsv.Size = New System.Drawing.Size(100, 30)
        Me._btnExportCsv.TabIndex = 6
        Me._btnExportCsv.Text = "Xuất CSV"
        '
        '_lblStatsHeader
        '
        Me._lblStatsHeader.AutoSize = True
        Me._lblStatsHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._lblStatsHeader.Location = New System.Drawing.Point(3, 82)
        Me._lblStatsHeader.Name = "_lblStatsHeader"
        Me._lblStatsHeader.Size = New System.Drawing.Size(338, 23)
        Me._lblStatsHeader.TabIndex = 7
        Me._lblStatsHeader.Text = "Top 10 sản phẩm có nhiều giao dịch nhất"
        '
        '_lblLowStockHeader
        '
        Me._lblLowStockHeader.AutoSize = True
        Me._lblLowStockHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._lblLowStockHeader.Location = New System.Drawing.Point(4, 298)
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
        Me._txtSearch.Location = New System.Drawing.Point(427, 10)
        Me._txtSearch.Name = "_txtSearch"
        Me._txtSearch.Size = New System.Drawing.Size(200, 22)
        Me._txtSearch.TabIndex = 3
        '
        '_cmbStatus
        '
        Me._cmbStatus.Location = New System.Drawing.Point(646, 10)
        Me._cmbStatus.Name = "_cmbStatus"
        Me._cmbStatus.Size = New System.Drawing.Size(100, 24)
        Me._cmbStatus.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(359, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Tìm kiếm"
        '
        'StockTransactionListForm
        '
        Me.ClientSize = New System.Drawing.Size(882, 553)
        Me.Controls.Add(Me.Label1)
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
    Friend WithEvents Label1 As Label
End Class