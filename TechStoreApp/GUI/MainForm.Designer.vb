<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
                If _currentForm IsNot Nothing Then
                    _currentForm.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents _btnStockTransaction As Button
    Friend WithEvents _btnProductManagement As Button
    Friend WithEvents _btnSupplierManagement As Button

    Friend _contentPanel As Panel
    Friend _menuPanel As Panel
    Friend _currentForm As Form
    Friend _selectedButton As Button

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._menuPanel = New System.Windows.Forms.Panel()
        Me._btnStockTransaction = New System.Windows.Forms.Button()
        Me._btnProductManagement = New System.Windows.Forms.Button()
        Me._btnSupplierManagement = New System.Windows.Forms.Button()

        Me._contentPanel = New System.Windows.Forms.Panel()
        Me._menuPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        '_menuPanel
        '
        Me._menuPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(44, Byte), Integer))
        Me._menuPanel.Controls.Add(Me._btnProductManagement)
        Me._menuPanel.Controls.Add(Me._btnStockTransaction)
        Me._menuPanel.Controls.Add(Me._btnSupplierManagement)
        Me._menuPanel.Location = New System.Drawing.Point(0, 0)
        Me._menuPanel.Name = "_menuPanel"
        Me._menuPanel.Size = New System.Drawing.Size(1200, 60)
        Me._menuPanel.TabIndex = 0
        '
        '_btnStockTransaction
        '
        Me._btnStockTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(46, Byte), Integer), CType(CType(66, Byte), Integer))
        Me._btnStockTransaction.FlatAppearance.BorderSize = 0
        Me._btnStockTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me._btnStockTransaction.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me._btnStockTransaction.ForeColor = System.Drawing.Color.White
        Me._btnStockTransaction.Location = New System.Drawing.Point(29, 12)
        Me._btnStockTransaction.Name = "_btnStockTransaction"
        Me._btnStockTransaction.Size = New System.Drawing.Size(200, 40)
        Me._btnStockTransaction.TabIndex = 0
        Me._btnStockTransaction.Tag = "StockTransactionListForm"
        Me._btnStockTransaction.Text = "Quản lý phiếu nhập/xuất"
        Me._btnStockTransaction.UseVisualStyleBackColor = False
        '
        '_btnProductManagement
        '
        Me._btnProductManagement.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(46, Byte), Integer), CType(CType(66, Byte), Integer))
        Me._btnProductManagement.FlatAppearance.BorderSize = 0
        Me._btnProductManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me._btnProductManagement.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me._btnProductManagement.ForeColor = System.Drawing.Color.White
        Me._btnProductManagement.Location = New System.Drawing.Point(235, 12)
        Me._btnProductManagement.Name = "_btnProductManagement"
        Me._btnProductManagement.Size = New System.Drawing.Size(200, 40)
        Me._btnProductManagement.TabIndex = 1
        Me._btnProductManagement.Tag = "ProductManagementForm"
        Me._btnProductManagement.Text = "Quản lý sản phẩm"
        Me._btnProductManagement.UseVisualStyleBackColor = False

        ' _btnSupplierManagement
        Me._btnSupplierManagement.BackColor = System.Drawing.Color.FromArgb(36, 46, 66)
        Me._btnSupplierManagement.FlatAppearance.BorderSize = 0
        Me._btnSupplierManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me._btnSupplierManagement.Font = New System.Drawing.Font("Segoe UI", 10.0!, FontStyle.Bold)
        Me._btnSupplierManagement.ForeColor = Color.White
        Me._btnSupplierManagement.Location = New System.Drawing.Point(441, 12)
        Me._btnSupplierManagement.Name = "_btnSupplierManagement"
        Me._btnSupplierManagement.Size = New System.Drawing.Size(200, 40)
        Me._btnSupplierManagement.TabIndex = 2
        Me._btnSupplierManagement.Tag = "SupplierManagementForm"
        Me._btnSupplierManagement.Text = "Quản lý nhà cung cấp"
        Me._btnSupplierManagement.UseVisualStyleBackColor = False
        '
        '_contentPanel
        '
        Me._contentPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
        Me._contentPanel.Location = New System.Drawing.Point(0, 60)
        Me._contentPanel.Name = "_contentPanel"
        Me._contentPanel.Size = New System.Drawing.Size(1200, 640)
        Me._contentPanel.TabIndex = 1
        '
        'MainForm
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1200, 700)
        Me.Controls.Add(Me._menuPanel)
        Me.Controls.Add(Me._contentPanel)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Quản lý kho"
        Me._menuPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
End Class