Partial Class StockTransactionDetailForm
    Inherits System.Windows.Forms.Form



    Private Sub InitializeComponent()
        Me._lblTransactionCode = New System.Windows.Forms.Label()
        Me._lblTransactionType = New System.Windows.Forms.Label()
        Me._lblCreatedBy = New System.Windows.Forms.Label()
        Me._lblCreatedAt = New System.Windows.Forms.Label()
        Me._lblSupplier = New System.Windows.Forms.Label()
        Me._lblStatus = New System.Windows.Forms.Label()
        Me._lblApprovedBy = New System.Windows.Forms.Label()
        Me._lblApprovedAt = New System.Windows.Forms.Label()
        Me._gridDetails = New System.Windows.Forms.DataGridView()
        Me._btnClose = New System.Windows.Forms.Button()
        Me.panelInfo = New System.Windows.Forms.Panel()
        Me._btnApprove = New System.Windows.Forms.Button()
        Me._btnReject = New System.Windows.Forms.Button()
        CType(Me._gridDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        '_lblTransactionCode
        '
        Me._lblTransactionCode.Location = New System.Drawing.Point(10, 10)
        Me._lblTransactionCode.Name = "_lblTransactionCode"
        Me._lblTransactionCode.Size = New System.Drawing.Size(300, 20)
        Me._lblTransactionCode.TabIndex = 0
        '
        '_lblTransactionType
        '
        Me._lblTransactionType.Location = New System.Drawing.Point(10, 30)
        Me._lblTransactionType.Name = "_lblTransactionType"
        Me._lblTransactionType.Size = New System.Drawing.Size(300, 20)
        Me._lblTransactionType.TabIndex = 1
        '
        '_lblCreatedBy
        '
        Me._lblCreatedBy.Location = New System.Drawing.Point(10, 50)
        Me._lblCreatedBy.Name = "_lblCreatedBy"
        Me._lblCreatedBy.Size = New System.Drawing.Size(300, 20)
        Me._lblCreatedBy.TabIndex = 2
        '
        '_lblCreatedAt
        '
        Me._lblCreatedAt.Location = New System.Drawing.Point(10, 70)
        Me._lblCreatedAt.Name = "_lblCreatedAt"
        Me._lblCreatedAt.Size = New System.Drawing.Size(300, 20)
        Me._lblCreatedAt.TabIndex = 3
        '
        '_lblSupplier
        '
        Me._lblSupplier.Location = New System.Drawing.Point(10, 90)
        Me._lblSupplier.Name = "_lblSupplier"
        Me._lblSupplier.Size = New System.Drawing.Size(300, 20)
        Me._lblSupplier.TabIndex = 4
        '
        '_lblStatus
        '
        Me._lblStatus.Location = New System.Drawing.Point(10, 110)
        Me._lblStatus.Name = "_lblStatus"
        Me._lblStatus.Size = New System.Drawing.Size(300, 20)
        Me._lblStatus.TabIndex = 5
        '
        '_lblApprovedBy
        '
        Me._lblApprovedBy.Location = New System.Drawing.Point(350, 10)
        Me._lblApprovedBy.Name = "_lblApprovedBy"
        Me._lblApprovedBy.Size = New System.Drawing.Size(300, 20)
        Me._lblApprovedBy.TabIndex = 6
        '
        '_lblApprovedAt
        '
        Me._lblApprovedAt.Location = New System.Drawing.Point(350, 30)
        Me._lblApprovedAt.Name = "_lblApprovedAt"
        Me._lblApprovedAt.Size = New System.Drawing.Size(300, 20)
        Me._lblApprovedAt.TabIndex = 7
        '
        '_gridDetails
        '
        Me._gridDetails.AllowUserToAddRows = False
        Me._gridDetails.AllowUserToDeleteRows = False
        Me._gridDetails.ColumnHeadersHeight = 29
        Me._gridDetails.Location = New System.Drawing.Point(10, 170)
        Me._gridDetails.Name = "_gridDetails"
        Me._gridDetails.ReadOnly = True
        Me._gridDetails.RowHeadersWidth = 51
        Me._gridDetails.Size = New System.Drawing.Size(659, 260)
        Me._gridDetails.TabIndex = 1
        '
        '_btnClose
        '
        Me._btnClose.Location = New System.Drawing.Point(569, 446)
        Me._btnClose.Name = "_btnClose"
        Me._btnClose.Size = New System.Drawing.Size(100, 30)
        Me._btnClose.TabIndex = 2
        Me._btnClose.Text = "Đóng"
        '
        'panelInfo
        '
        Me.panelInfo.Controls.Add(Me._btnApprove)
        Me.panelInfo.Controls.Add(Me._lblTransactionCode)
        Me.panelInfo.Controls.Add(Me._btnReject)
        Me.panelInfo.Controls.Add(Me._lblTransactionType)
        Me.panelInfo.Controls.Add(Me._lblCreatedBy)
        Me.panelInfo.Controls.Add(Me._lblCreatedAt)
        Me.panelInfo.Controls.Add(Me._lblSupplier)
        Me.panelInfo.Controls.Add(Me._lblStatus)
        Me.panelInfo.Controls.Add(Me._lblApprovedBy)
        Me.panelInfo.Controls.Add(Me._lblApprovedAt)
        Me.panelInfo.Location = New System.Drawing.Point(10, 10)
        Me.panelInfo.Name = "panelInfo"
        Me.panelInfo.Size = New System.Drawing.Size(659, 150)
        Me.panelInfo.TabIndex = 0
        '
        '_btnApprove
        '
        Me._btnApprove.Location = New System.Drawing.Point(444, 110)
        Me._btnApprove.Name = "_btnApprove"
        Me._btnApprove.Size = New System.Drawing.Size(100, 30)
        Me._btnApprove.TabIndex = 0
        Me._btnApprove.Text = "Duyệt phiếu"
        Me._btnApprove.Visible = False
        '
        '_btnReject
        '
        Me._btnReject.Location = New System.Drawing.Point(550, 110)
        Me._btnReject.Name = "_btnReject"
        Me._btnReject.Size = New System.Drawing.Size(100, 30)
        Me._btnReject.TabIndex = 1
        Me._btnReject.Text = "Từ chối"
        Me._btnReject.Visible = False
        '
        'StockTransactionDetailForm
        '
        Me.ClientSize = New System.Drawing.Size(677, 488)
        Me.Controls.Add(Me.panelInfo)
        Me.Controls.Add(Me._gridDetails)
        Me.Controls.Add(Me._btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "StockTransactionDetailForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Chi tiết phiếu"
        CType(Me._gridDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelInfo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelInfo As Panel
End Class
