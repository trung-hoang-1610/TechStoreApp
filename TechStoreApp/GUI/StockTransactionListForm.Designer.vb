
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
        _gridIn.Size = New System.Drawing.Size(760, 400)
        _gridIn.Location = New System.Drawing.Point(10, 10)
        _gridIn.AutoGenerateColumns = False
        _gridIn.Columns.Add("TransactionId", "Mã phiếu")
        _gridIn.Columns.Add("TransactionCode", "Mã giao dịch")
        _gridIn.Columns.Add("CreatedByName", "Người tạo")
        _gridIn.Columns.Add("CreatedAt", "Ngày tạo")
        _gridIn.Columns.Add("SupplierName", "Nhà cung cấp")
        _gridIn.Columns.Add("Status", "Trạng thái")
        _gridIn.Columns.Add("ApprovedByName", "Người duyệt")
        _gridIn.Columns.Add("ApprovedAt", "Ngày duyệt")
        tabIn.Controls.Add(_gridIn)

        ' Khởi tạo và thiết lập DataGridView cho phiếu xuất
        _gridOut = New DataGridView()
        _gridOut.Size = New System.Drawing.Size(760, 400)
        _gridOut.Location = New System.Drawing.Point(10, 10)
        _gridOut.AutoGenerateColumns = False
        _gridOut.Columns.Add("TransactionId", "Mã phiếu")
        _gridOut.Columns.Add("TransactionCode", "Mã giao dịch")
        _gridOut.Columns.Add("CreatedByName", "Người tạo")
        _gridOut.Columns.Add("CreatedAt", "Ngày tạo")
        _gridOut.Columns.Add("Status", "Trạng thái")
        _gridOut.Columns.Add("ApprovedByName", "Người duyệt")
        _gridOut.Columns.Add("ApprovedAt", "Ngày duyệt")
        tabOut.Controls.Add(_gridOut)

        ' Khởi tạo và thiết lập các nút
        _btnCreateIn = New Button()
        _btnCreateIn.Text = "Tạo phiếu nhập"
        _btnCreateIn.Size = New System.Drawing.Size(100, 30)
        _btnCreateIn.Location = New System.Drawing.Point(10, 10)
        tabIn.Controls.Add(_btnCreateIn)

        _btnCreateOut = New Button()
        _btnCreateOut.Text = "Tạo phiếu xuất"
        _btnCreateOut.Size = New System.Drawing.Size(100, 30)
        _btnCreateOut.Location = New System.Drawing.Point(10, 10)
        tabOut.Controls.Add(_btnCreateOut)

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
        _btnApprove.Visible = SessionManager.GetCurrentUser()?.RoleId = 1
        Me.Controls.Add(_btnApprove)

        ' Khởi tạo và thiết lập tìm kiếm
        _txtSearch = New TextBox()
        _txtSearch.Location = New System.Drawing.Point(340, 10)
        _txtSearch.Size = New System.Drawing.Size(200, 30)
        _txtSearch.Text = "Tìm kiếm mã phiếu..."
        Me.Controls.Add(_txtSearch)

        _cmbStatus = New ComboBox()
        _cmbStatus.Location = New System.Drawing.Point(550, 10)
        _cmbStatus.Size = New System.Drawing.Size(100, 30)
        _cmbStatus.Items.AddRange({"All", "Pending", "Approved", "Rejected"})
        _cmbStatus.SelectedIndex = 0
        Me.Controls.Add(_cmbStatus)

        Me.ResumeLayout(False)
    End Sub
End Class
