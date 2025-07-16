
Partial Class StockTransactionDetailForm
    Inherits System.Windows.Forms.Form

    Private Sub InitializeComponent()
        _lblTransactionCode = New Label()
        _lblTransactionType = New Label()
        _lblCreatedBy = New Label()
        _lblCreatedAt = New Label()
        _lblSupplier = New Label()
        _lblStatus = New Label()
        _lblApprovedBy = New Label()
        _lblApprovedAt = New Label()
        _gridDetails = New DataGridView()
        _btnClose = New Button()

        ' Thiết lập form
        Me.Text = "Chi tiết phiếu"
        Me.Size = New System.Drawing.Size(600, 500)

        ' Thiết lập thông tin phiếu
        Dim panelInfo As New Panel()
        panelInfo.Size = New System.Drawing.Size(580, 150)
        panelInfo.Location = New System.Drawing.Point(10, 10)
        _lblTransactionCode.Location = New System.Drawing.Point(10, 10)
        _lblTransactionType.Location = New System.Drawing.Point(10, 30)
        _lblCreatedBy.Location = New System.Drawing.Point(10, 50)
        _lblCreatedAt.Location = New System.Drawing.Point(10, 70)
        _lblSupplier.Location = New System.Drawing.Point(10, 90)
        _lblStatus.Location = New System.Drawing.Point(10, 110)
        _lblApprovedBy.Location = New System.Drawing.Point(300, 10)
        _lblApprovedAt.Location = New System.Drawing.Point(300, 30)
        panelInfo.Controls.AddRange({_lblTransactionCode, _lblTransactionType, _lblCreatedBy, _lblCreatedAt, _lblSupplier, _lblStatus, _lblApprovedBy, _lblApprovedAt})
        Me.Controls.Add(panelInfo)

        ' Thiết lập DataGridView cho chi tiết
        _gridDetails.Size = New System.Drawing.Size(580, 250)
        _gridDetails.Location = New System.Drawing.Point(10, 170)
        _gridDetails.AutoGenerateColumns = False
        _gridDetails.Columns.Add("DetailId", "Mã chi tiết")
        _gridDetails.Columns.Add("ProductId", "Mã sản phẩm")
        _gridDetails.Columns.Add("ProductName", "Tên sản phẩm")
        _gridDetails.Columns.Add("Unit", "Đơn vị")
        _gridDetails.Columns.Add("Quantity", "Số lượng")
        _gridDetails.Columns.Add("Note", "Ghi chú")
        Me.Controls.Add(_gridDetails)

        ' Nút đóng
        _btnClose.Text = "Đóng"
        _btnClose.Size = New System.Drawing.Size(100, 30)
        _btnClose.Location = New System.Drawing.Point(490, 430)
        Me.Controls.Add(_btnClose)
    End Sub
End Class
