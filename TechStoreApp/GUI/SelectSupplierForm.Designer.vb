<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectSupplierForm
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
        Me.Text = "Chọn nhà cung cấp"
        Me.Size = New Size(350, 150)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False

        Dim lblSupplier As New Label()
        lblSupplier.Text = "Nhà cung cấp:"
        lblSupplier.Location = New Point(10, 20)
        lblSupplier.Size = New Size(80, 20)
        Me.Controls.Add(lblSupplier)

        _cmbSupplier = New ComboBox()
        _cmbSupplier.Location = New Point(100, 20)
        _cmbSupplier.Size = New Size(200, 30)
        Me.Controls.Add(_cmbSupplier)

        _btnContinue = New Button()
        _btnContinue.Text = "Tiếp tục"
        _btnContinue.Location = New Point(100, 60)
        _btnContinue.Size = New Size(100, 30)
        Me.Controls.Add(_btnContinue)

        _btnCancel = New Button()
        _btnCancel.Text = "Hủy"
        _btnCancel.Location = New Point(210, 60)
        _btnCancel.Size = New Size(100, 30)
        Me.Controls.Add(_btnCancel)
    End Sub
End Class
