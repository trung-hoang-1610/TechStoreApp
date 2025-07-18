<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._menuListView = New System.Windows.Forms.ListView()
        Me._contentPanel = New System.Windows.Forms.Panel()
        Me.SuspendLayout()

        ' Thiết lập form
        Me.Text = "Quản lý kho"
        Me.ClientSize = New System.Drawing.Size(1400, 600) ' Kích thước lớn hơn cho giao diện
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0F)

        ' Menu ListView (bên trái)
        Me._menuListView.Location = New System.Drawing.Point(10, 10)
        Me._menuListView.Size = New System.Drawing.Size(200, 650)
        Me._menuListView.View = View.Details
        Me._menuListView.FullRowSelect = True
        Me._menuListView.MultiSelect = False
        Me._menuListView.HeaderStyle = ColumnHeaderStyle.None
        Me._menuListView.Columns.Add("Menu", 180)
        Me._menuListView.Font = New System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Bold)
        Me._menuListView.Items.Add(New ListViewItem("Quản lý phiếu nhập/xuất") With {.Tag = "StockTransactionListForm"})
        Me._menuListView.Items.Add(New ListViewItem("Quản lý sản phẩm") With {.Tag = "ProductManagementForm"})
        Me._menuListView.SelectedIndices.Add(0) ' Chọn mục đầu tiên mặc định
        Me.Controls.Add(Me._menuListView)

        ' Content Panel (bên phải)
        Me._contentPanel.Location = New System.Drawing.Point(220, 10)
        Me._contentPanel.Size = New System.Drawing.Size(1300, 650)
        Me._contentPanel.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(Me._contentPanel)

        Me.ResumeLayout(False)
        Me.PerformLayout()

        ' Hiển thị form mặc định
        ShowFormInPanel("StockTransactionListForm")
    End Sub


End Class
