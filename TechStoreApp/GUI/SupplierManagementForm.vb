Imports System.Windows.Forms
Imports System.Data.Odbc

Partial Public Class SupplierManagementForm
    Inherits Form

    Private ReadOnly _supplierService As ISupplierService
    Private _currentSupplierId As Integer = -1

    Private Enum FormMode
        None
        Add
        Edit
    End Enum

    Private _formState As FormMode = FormMode.None

    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        _supplierService = ServiceFactory.CreateSupplierService()
    End Sub

    Private Sub SupplierManagementForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadSuppliers()
        ClearForm(Nothing, Nothing)
    End Sub

    Private Sub SetFormState(mode As FormMode)
        _formState = mode

        Select Case mode
            Case FormMode.None
                txtName.Enabled = False
                txtContact.Enabled = False
                btnAdd.Text = "Thêm"
                btnUpdate.Text = "Cập nhật"
                btnDelete.Enabled = True
                dgvSuppliers.Enabled = True
                btnClear.Visible = False

            Case FormMode.Add
                txtName.Enabled = True
                txtContact.Enabled = True
                btnAdd.Text = "Lưu"
                btnUpdate.Enabled = False
                btnDelete.Enabled = False
                dgvSuppliers.Enabled = False
                btnClear.Visible = True

            Case FormMode.Edit
                txtName.Enabled = True
                txtContact.Enabled = True
                btnUpdate.Text = "Lưu"
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                dgvSuppliers.Enabled = False
                btnClear.Visible = True
        End Select
    End Sub

    Private Sub LoadSuppliers()
        Try
            dgvSuppliers.Rows.Clear()
            Dim suppliers = _supplierService.GetAllSuppliers()

            For Each s In suppliers
                dgvSuppliers.Rows.Add(s.SupplierId, s.SupplierName, s.ContactInfo)
            Next

            lblCount.Text = $"Tổng số: {suppliers.Count}"
        Catch ex As Exception
            MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " & ex.Message, "Lỗi nghiêm trọng")
        End Try
    End Sub


    Private Sub ClearForm(sender As Object, e As EventArgs)
        txtName.Text = ""
        txtContact.Text = ""
        _currentSupplierId = -1
        dgvSuppliers.ClearSelection()
        SetFormState(FormMode.None)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If _formState = FormMode.None Then
            SetFormState(FormMode.Add)
            txtName.Text = ""
            txtContact.Text = ""
            txtName.Focus()
        ElseIf _formState = FormMode.Add Then
            Try
                Dim newSupplier As New Supplier With {
                .SupplierName = txtName.Text.Trim(),
                .ContactInfo = txtContact.Text.Trim()
                }
                If IsDuplicateName(newSupplier.SupplierName) Then
                    Dim _result = MessageBox.Show("Tên nhà cung cấp đã tồn tại. Bạn có muốn tiếp tục thêm?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If _result = DialogResult.No Then Exit Sub
                End If


                Dim result = _supplierService.AddSupplier(newSupplier)

                If result.Success Then
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo")
                    LoadSuppliers()
                    ClearForm(Nothing, Nothing)
                Else
                    MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi")
                End If
            Catch ex As Exception
                MessageBox.Show("Lỗi hệ thống khi thêm nhà cung cấp: " & ex.Message, "Lỗi nghiêm trọng")
            End Try
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If _formState = FormMode.None Then
            If _currentSupplierId <= 0 Then
                MessageBox.Show("Vui lòng chọn nhà cung cấp để cập nhật.", "Thông báo")
                Return
            End If
            SetFormState(FormMode.Edit)
        ElseIf _formState = FormMode.Edit Then
            ' Lấy dữ liệu cũ từ dòng đã chọn
            Dim currentRow = dgvSuppliers.SelectedRows(0)
            Dim oldName = currentRow.Cells("colName").Value?.ToString()
            Dim oldContact = currentRow.Cells("colContact").Value?.ToString()

            ' Dữ liệu người dùng nhập
            Dim newName = txtName.Text.Trim()
            Dim newContact = txtContact.Text.Trim()

            ' Kiểm tra thay đổi
            If newName = oldName AndAlso newContact = oldContact Then
                MessageBox.Show("Thông tin không có thay đổi nào. Không cần cập nhật.", "Thông báo")
                Return
            End If

            ' Tạo đối tượng cập nhật
            Dim updatedSupplier As New Supplier With {
            .SupplierId = _currentSupplierId,
            .SupplierName = newName,
            .ContactInfo = newContact
            }
            If IsDuplicateName(newName, _currentSupplierId) Then
                Dim result = MessageBox.Show("Tên nhà cung cấp đã tồn tại. Bạn có muốn tiếp tục thêm?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.No Then Exit Sub
            End If

            Try
                Dim result = _supplierService.UpdateSupplier(updatedSupplier)

                If result.Success Then
                    MessageBox.Show("Cập nhật thành công!", "Thông báo")
                    LoadSuppliers()
                    ClearForm(Nothing, Nothing)
                Else
                    MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi")
                End If
            Catch ex As Exception
                MessageBox.Show("Lỗi hệ thống khi cập nhật nhà cung cấp: " & ex.Message, "Lỗi nghiêm trọng")
            End Try
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If _currentSupplierId <= 0 Then
            MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa.", "Thông báo")
            Return
        End If

        If MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                Dim result = _supplierService.DeleteSupplier(_currentSupplierId)

                If result Then
                    MessageBox.Show("Xóa thành công!", "Thông báo")
                    LoadSuppliers()
                    ClearForm(Nothing, Nothing)
                Else
                    MessageBox.Show("Không thể xóa nhà cung cấp này.", "Lỗi")
                End If

            Catch ex As OdbcException
                If ex.Message.ToLower().Contains("constraint") OrElse ex.Message.ToLower().Contains("foreign key") Then
                    MessageBox.Show("Không thể xóa nhà cung cấp vì đang được liên kết với đơn hàng hoặc dữ liệu khác.", "Thông báo")
                Else
                    MessageBox.Show("Đã xảy ra lỗi kết nối cơ sở dữ liệu.", "Lỗi hệ thống")
                End If

            Catch ex As Exception
                MessageBox.Show("Đã xảy ra lỗi không xác định. Vui lòng thử lại hoặc liên hệ hỗ trợ.", "Lỗi nghiêm trọng")
            End Try

        End If

    End Sub

    Private Sub dgvSuppliers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSuppliers.SelectionChanged
        If _formState <> FormMode.None Then Return ' Tránh thay đổi khi đang thêm/cập nhật

        If dgvSuppliers.SelectedRows.Count > 0 Then
            Dim row = dgvSuppliers.SelectedRows(0)

            If row.Cells("colId").Value IsNot Nothing Then
                Dim idStr = row.Cells("colId").Value.ToString()
                Dim nameStr = row.Cells("colName").Value?.ToString()
                Dim contactStr = row.Cells("colContact").Value?.ToString()

                If Integer.TryParse(idStr, _currentSupplierId) Then
                    txtName.Text = nameStr
                    txtContact.Text = contactStr
                    btnUpdate.Enabled = True
                    btnDelete.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If MessageBox.Show("Bạn có muốn hủy thao tác hiện tại?", "Xác nhận hủy", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            ClearForm(Nothing, Nothing)
        End If
    End Sub

    Private Function IsDuplicateName(supplierName As String, Optional excludeId As Integer = -1) As Boolean
        Dim allSuppliers = _supplierService.GetAllSuppliers()
        Return allSuppliers.Any(Function(s) s.SupplierName.Trim().ToLower() = supplierName.Trim().ToLower() AndAlso s.SupplierId <> excludeId)
    End Function
End Class
