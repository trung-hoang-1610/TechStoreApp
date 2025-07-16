<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StatisticsForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    Friend WithEvents groupBoxStats As GroupBox
    Friend WithEvents lblTotalProducts As Label
    Friend WithEvents lblActiveProducts As Label
    Friend WithEvents lblInactiveProducts As Label
    Friend WithEvents lblLowStockProducts As Label
    Friend WithEvents lblInventoryValue As Label
    Friend WithEvents cboTimeRange As ComboBox
    Friend WithEvents btnRefreshStats As Button
    Friend WithEvents btnExportCsv As Button
    Friend WithEvents dgvStats As DataGridView

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.groupBoxStats = New System.Windows.Forms.GroupBox()
        Me.lblTotalProducts = New System.Windows.Forms.Label()
        Me.lblActiveProducts = New System.Windows.Forms.Label()
        Me.lblInactiveProducts = New System.Windows.Forms.Label()
        Me.lblLowStockProducts = New System.Windows.Forms.Label()
        Me.lblInventoryValue = New System.Windows.Forms.Label()
        Me.cboTimeRange = New System.Windows.Forms.ComboBox()
        Me.btnRefreshStats = New System.Windows.Forms.Button()
        Me.btnExportCsv = New System.Windows.Forms.Button()
        Me.dgvStats = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.groupBoxStats.SuspendLayout()
        CType(Me.dgvStats, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'groupBoxStats
        '
        Me.groupBoxStats.Controls.Add(Me.lblTotalProducts)
        Me.groupBoxStats.Controls.Add(Me.lblActiveProducts)
        Me.groupBoxStats.Controls.Add(Me.lblInactiveProducts)
        Me.groupBoxStats.Controls.Add(Me.lblLowStockProducts)
        Me.groupBoxStats.Controls.Add(Me.lblInventoryValue)
        Me.groupBoxStats.Controls.Add(Me.cboTimeRange)
        Me.groupBoxStats.Controls.Add(Me.btnRefreshStats)
        Me.groupBoxStats.Controls.Add(Me.btnExportCsv)
        Me.groupBoxStats.Controls.Add(Me.dgvStats)
        Me.groupBoxStats.Location = New System.Drawing.Point(10, 10)
        Me.groupBoxStats.Name = "groupBoxStats"
        Me.groupBoxStats.Size = New System.Drawing.Size(580, 409)
        Me.groupBoxStats.TabIndex = 0
        Me.groupBoxStats.TabStop = False
        Me.groupBoxStats.Text = "Thống kê sản phẩm"
        '
        'lblTotalProducts
        '
        Me.lblTotalProducts.Location = New System.Drawing.Point(20, 47)
        Me.lblTotalProducts.Name = "lblTotalProducts"
        Me.lblTotalProducts.Size = New System.Drawing.Size(200, 20)
        Me.lblTotalProducts.TabIndex = 0
        Me.lblTotalProducts.Text = "Tổng số sản phẩm: 0"
        '
        'lblActiveProducts
        '
        Me.lblActiveProducts.Location = New System.Drawing.Point(20, 82)
        Me.lblActiveProducts.Name = "lblActiveProducts"
        Me.lblActiveProducts.Size = New System.Drawing.Size(200, 20)
        Me.lblActiveProducts.TabIndex = 1
        Me.lblActiveProducts.Text = "Sản phẩm hoạt động: 0"
        '
        'lblInactiveProducts
        '
        Me.lblInactiveProducts.Location = New System.Drawing.Point(20, 114)
        Me.lblInactiveProducts.Name = "lblInactiveProducts"
        Me.lblInactiveProducts.Size = New System.Drawing.Size(200, 20)
        Me.lblInactiveProducts.TabIndex = 2
        Me.lblInactiveProducts.Text = "Sản phẩm ngưng hoạt động: 0"
        '
        'lblLowStockProducts
        '
        Me.lblLowStockProducts.Location = New System.Drawing.Point(20, 147)
        Me.lblLowStockProducts.Name = "lblLowStockProducts"
        Me.lblLowStockProducts.Size = New System.Drawing.Size(200, 20)
        Me.lblLowStockProducts.TabIndex = 3
        Me.lblLowStockProducts.Text = "Sản phẩm dưới mức tồn: 0"
        '
        'lblInventoryValue
        '
        Me.lblInventoryValue.Location = New System.Drawing.Point(20, 180)
        Me.lblInventoryValue.Name = "lblInventoryValue"
        Me.lblInventoryValue.Size = New System.Drawing.Size(359, 20)
        Me.lblInventoryValue.TabIndex = 4
        Me.lblInventoryValue.Text = "Tổng giá trị tồn kho: 0"
        '
        'cboTimeRange
        '
        Me.cboTimeRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTimeRange.Items.AddRange(New Object() {"Toàn bộ thời gian", "7 ngày qua", "30 ngày qua"})
        Me.cboTimeRange.Location = New System.Drawing.Point(241, 47)
        Me.cboTimeRange.Name = "cboTimeRange"
        Me.cboTimeRange.Size = New System.Drawing.Size(150, 24)
        Me.cboTimeRange.TabIndex = 5
        '
        'btnRefreshStats
        '
        Me.btnRefreshStats.Location = New System.Drawing.Point(460, 43)
        Me.btnRefreshStats.Name = "btnRefreshStats"
        Me.btnRefreshStats.Size = New System.Drawing.Size(100, 30)
        Me.btnRefreshStats.TabIndex = 6
        Me.btnRefreshStats.Text = "Làm mới"
        '
        'btnExportCsv
        '
        Me.btnExportCsv.Location = New System.Drawing.Point(460, 187)
        Me.btnExportCsv.Name = "btnExportCsv"
        Me.btnExportCsv.Size = New System.Drawing.Size(100, 30)
        Me.btnExportCsv.TabIndex = 7
        Me.btnExportCsv.Text = "Xuất CSV"
        '
        'dgvStats
        '
        Me.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStats.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.dgvStats.Location = New System.Drawing.Point(20, 223)
        Me.dgvStats.Name = "dgvStats"
        Me.dgvStats.RowHeadersWidth = 51
        Me.dgvStats.Size = New System.Drawing.Size(540, 180)
        Me.dgvStats.TabIndex = 8
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Tên danh mục"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 125
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Số lượng sản phẩm"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 6
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 125
        '
        'StatisticsForm
        '
        Me.ClientSize = New System.Drawing.Size(600, 431)
        Me.Controls.Add(Me.groupBoxStats)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "StatisticsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thống kê sản phẩm"
        Me.groupBoxStats.ResumeLayout(False)
        CType(Me.dgvStats, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
End Class