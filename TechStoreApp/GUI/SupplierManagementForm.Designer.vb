Partial Class SupplierManagementForm
    Inherits System.Windows.Forms.Form

    Private WithEvents dgvSuppliers As System.Windows.Forms.DataGridView
    Private txtName As System.Windows.Forms.TextBox
    Private txtContact As System.Windows.Forms.TextBox
    Private WithEvents btnAdd As System.Windows.Forms.Button
    Private WithEvents btnUpdate As System.Windows.Forms.Button
    Private WithEvents btnDelete As System.Windows.Forms.Button
    Private WithEvents btnClear As System.Windows.Forms.Button
    Private lblName As System.Windows.Forms.Label
    Private lblContact As System.Windows.Forms.Label
    Private lblCount As System.Windows.Forms.Label

    Private Sub InitializeComponent()
        Me.dgvSuppliers = New System.Windows.Forms.DataGridView()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtContact = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblContact = New System.Windows.Forms.Label()
        Me.lblCount = New System.Windows.Forms.Label()

        CType(Me.dgvSuppliers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()

        ' dgvSuppliers
        Me.dgvSuppliers.Location = New System.Drawing.Point(20, 20)
        Me.dgvSuppliers.Size = New System.Drawing.Size(640, 240)
        Me.dgvSuppliers.ReadOnly = True
        Me.dgvSuppliers.AllowUserToAddRows = False
        Me.dgvSuppliers.AllowUserToDeleteRows = False
        Me.dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.dgvSuppliers.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colId", .HeaderText = "Mã", .Width = 50})
        Me.dgvSuppliers.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colName", .HeaderText = "Tên nhà cung cấp", .Width = 200})
        Me.dgvSuppliers.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colContact", .HeaderText = "Thông tin liên hệ", .Width = 300})

        ' txtName
        Me.txtName.Location = New System.Drawing.Point(150, 276)
        Me.txtName.Size = New System.Drawing.Size(300, 22)

        ' txtContact
        Me.txtContact.Location = New System.Drawing.Point(150, 316)
        Me.txtContact.Size = New System.Drawing.Size(300, 22)

        ' lblName
        Me.lblName.Text = "Tên nhà cung cấp"
        Me.lblName.Location = New System.Drawing.Point(20, 280)

        ' lblContact
        Me.lblContact.Text = "Thông tin liên hệ"
        Me.lblContact.Location = New System.Drawing.Point(20, 320)

        ' lblCount
        Me.lblCount.Text = "Tổng số: 0"
        Me.lblCount.Location = New System.Drawing.Point(20, 370)

        ' btnAdd
        Me.btnAdd.Text = "Thêm"
        Me.btnAdd.Location = New System.Drawing.Point(480, 270)
        Me.btnAdd.Size = New System.Drawing.Size(120, 28)

        ' btnUpdate
        Me.btnUpdate.Text = "Cập nhật"
        Me.btnUpdate.Location = New System.Drawing.Point(480, 308)
        Me.btnUpdate.Size = New System.Drawing.Size(120, 28)

        ' btnDelete
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.Location = New System.Drawing.Point(480, 346)
        Me.btnDelete.Size = New System.Drawing.Size(120, 28)

        ' btnClear
        Me.btnClear.Text = "Hủy"
        Me.btnClear.Location = New System.Drawing.Point(480, 384)
        Me.btnClear.Size = New System.Drawing.Size(120, 28)
        Me.btnClear.Visible = False

        ' Form
        Me.ClientSize = New System.Drawing.Size(700, 450)
        Me.Controls.AddRange({dgvSuppliers, txtName, txtContact, btnAdd, btnUpdate, btnDelete, btnClear, lblName, lblContact, lblCount})
        Me.Text = "Quản lý nhà cung cấp"

        CType(Me.dgvSuppliers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
End Class
