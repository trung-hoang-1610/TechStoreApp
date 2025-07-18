Public Class MainForm
    Inherits Form

    Private WithEvents _menuListView As ListView
    Private _contentPanel As Panel
    Private _currentForm As Form
    Private WithEvents _btnStockTransaction As Button
    Private WithEvents _btnProductManagement As Button
    Private _menuPanel As Panel
    Private _selectedButton As Button



    Private Sub MenuListView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _menuListView.SelectedIndexChanged
        If _menuListView.SelectedItems.Count > 0 Then
            Dim selectedFormName As String = _menuListView.SelectedItems(0).Tag.ToString()
            ShowFormInPanel(selectedFormName)
        End If
    End Sub

    Private Sub ShowFormInPanel(formName As String)
        ' Xóa form hiện tại trong panel
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _contentPanel.Controls.Clear()
        End If

        ' Tạo và hiển thị form mới
        Select Case formName
            Case "StockTransactionListForm"
                _currentForm = New StockTransactionListForm()
            Case "ProductManagementForm"
                _currentForm = New ProductManagementForm(SessionManager.GetCurrentUser.UserId)
            Case Else
                Return
        End Select

        _currentForm.TopLevel = False
        _currentForm.FormBorderStyle = FormBorderStyle.None
        _currentForm.Dock = DockStyle.Fill
        _contentPanel.Controls.Add(_currentForm)
        _currentForm.Show()
    End Sub


End Class