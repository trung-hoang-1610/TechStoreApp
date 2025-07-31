Public Class SelectSupplierForm
    Inherits Form

    Private ReadOnly _supplierService As ISupplierService
    Private WithEvents _cmbSupplier As ComboBox
    Private WithEvents _btnContinue As Button
    Private WithEvents _btnCancel As Button
    Public SelectedSupplierId As Integer?

    Public Sub New()
        _supplierService = ServiceFactory.CreateSupplierService()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        LoadSuppliers()
    End Sub



    Private Async Sub LoadSuppliers()
        Try
            Dim suppliers = Await _supplierService.GetAllSuppliers()
            If suppliers Is Nothing OrElse Not suppliers.Any() Then
                MessageBox.Show("Không có nhà cung cấp nào trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If
            _cmbSupplier.DataSource = suppliers
            _cmbSupplier.DisplayMember = "SupplierName"
            _cmbSupplier.ValueMember = "SupplierId"
            _cmbSupplier.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub _btnContinue_Click(sender As Object, e As EventArgs) Handles _btnContinue.Click
        If _cmbSupplier.SelectedValue Is Nothing Then
            MessageBox.Show("Vui lòng chọn nhà cung cấp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        SelectedSupplierId = CInt(_cmbSupplier.SelectedValue)
        Dim createForm As New StockTransactionCreateForm("IN", SelectedSupplierId)
        Dim result = createForm.ShowDialog()
        If result = DialogResult.OK Then
            Me.DialogResult = DialogResult.OK
        End If
        Me.Close()

    End Sub

    Private Sub _btnCancel_Click(sender As Object, e As EventArgs) Handles _btnCancel.Click
        Me.Close()
    End Sub
End Class