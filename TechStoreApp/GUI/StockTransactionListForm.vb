
Public Class StockTransactionListForm
    Inherits Form

    Private ReadOnly _transactionService As IStockTransactionBLL
    Private WithEvents _tabControl As TabControl
    Private WithEvents _gridIn As DataGridView
    Private WithEvents _gridOut As DataGridView
    Private WithEvents _btnCreateIn As Button
    Private WithEvents _btnCreateOut As Button
    Private WithEvents _btnViewDetails As Button
    Private WithEvents _btnApprove As Button
    Private WithEvents _txtSearch As TextBox
    Private WithEvents _cmbStatus As ComboBox

    Public Sub New(ByVal userId As Integer)
        InitializeComponent()
        If _gridIn Is Nothing OrElse _gridOut Is Nothing OrElse _txtSearch Is Nothing OrElse _cmbStatus Is Nothing OrElse _tabControl Is Nothing OrElse _btnCreateIn Is Nothing OrElse _btnCreateOut Is Nothing OrElse _btnViewDetails Is Nothing OrElse _btnApprove Is Nothing Then
            Dim missingControls As New List(Of String)
            If _gridIn Is Nothing Then missingControls.Add("_gridIn")
            If _gridOut Is Nothing Then missingControls.Add("_gridOut")
            If _txtSearch Is Nothing Then missingControls.Add("_txtSearch")
            If _cmbStatus Is Nothing Then missingControls.Add("_cmbStatus")
            If _tabControl Is Nothing Then missingControls.Add("_tabControl")
            If _btnCreateIn Is Nothing Then missingControls.Add("_btnCreateIn")
            If _btnCreateOut Is Nothing Then missingControls.Add("_btnCreateOut")
            If _btnViewDetails Is Nothing Then missingControls.Add("_btnViewDetails")
            If _btnApprove Is Nothing Then missingControls.Add("_btnApprove")
            MessageBox.Show($"Các control chưa được khởi tạo: {String.Join(", ", missingControls.ToArray())}", "Lỗi khởi tạo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If
        _transactionService = ServiceFactory.CreateStockTransactionService()
        If _transactionService Is Nothing Then
            MessageBox.Show("Không thể khởi tạo StockTransactionService.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If
        LoadTransactions()
    End Sub

    Private Sub LoadTransactions()
        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser Is Nothing Then
            MessageBox.Show("Người dùng chưa đăng nhập. Vui lòng đăng nhập để tiếp tục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        Dim userId = If(currentUser.RoleId = 1, Nothing, currentUser.UserId)
        Dim criteria As New SearchCriteriaDTO With {
            .TransactionCode = If(_txtSearch.Text, ""),
            .Status = If(_cmbStatus.SelectedIndex > 0, _cmbStatus.SelectedItem?.ToString(), Nothing)
        }

        Try
            _gridIn.DataSource = _transactionService.SearchTransactions("IN", userId, criteria)
            _gridOut.DataSource = _transactionService.SearchTransactions("OUT", userId, criteria)
        Catch ex As Exception
            MessageBox.Show($"Lỗi khi tải danh sách phiếu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub _btnCreateIn_Click(sender As Object, e As EventArgs) Handles _btnCreateIn.Click
        Dim createForm As New StockTransactionCreateForm("IN")
        If createForm.ShowDialog() = DialogResult.OK Then
            LoadTransactions()
        End If
    End Sub

    Private Sub _btnCreateOut_Click(sender As Object, e As EventArgs) Handles _btnCreateOut.Click
        Dim createForm As New StockTransactionCreateForm("OUT")
        If createForm.ShowDialog() = DialogResult.OK Then
            LoadTransactions()
        End If
    End Sub

    Private Sub _gridIn_SelectionChanged(sender As Object, e As EventArgs) Handles _gridIn.SelectionChanged, _gridOut.SelectionChanged
        _btnViewDetails.Enabled = _gridIn.SelectedRows.Count > 0 OrElse _gridOut.SelectedRows.Count > 0
        _btnApprove.Enabled = (_gridIn.SelectedRows.Count > 0 OrElse _gridOut.SelectedRows.Count > 0) AndAlso SessionManager.GetCurrentUser()?.RoleId = 1
    End Sub

    Private Sub _gridIn_DoubleClick(sender As Object, e As EventArgs) Handles _gridIn.DoubleClick, _gridOut.DoubleClick
        If _gridIn.SelectedRows.Count > 0 Then
            Dim transactionId = CInt(_gridIn.SelectedRows(0).Cells("TransactionId").Value)
            Dim detailForm As New StockTransactionDetailForm(transactionId)
            detailForm.ShowDialog()
        ElseIf _gridOut.SelectedRows.Count > 0 Then
            Dim transactionId = CInt(_gridOut.SelectedRows(0).Cells("TransactionId").Value)
            Dim detailForm As New StockTransactionDetailForm(transactionId)
            detailForm.ShowDialog()
        End If
    End Sub

    Private Sub _btnViewDetails_Click(sender As Object, e As EventArgs) Handles _btnViewDetails.Click
        If _gridIn.SelectedRows.Count > 0 Then
            Dim transactionId = CInt(_gridIn.SelectedRows(0).Cells("TransactionId").Value)
            Dim detailForm As New StockTransactionDetailForm(transactionId)
            detailForm.ShowDialog()
        ElseIf _gridOut.SelectedRows.Count > 0 Then
            Dim transactionId = CInt(_gridOut.SelectedRows(0).Cells("TransactionId").Value)
            Dim detailForm As New StockTransactionDetailForm(transactionId)
            detailForm.ShowDialog()
        End If
    End Sub

    Private Sub _btnApprove_Click(sender As Object, e As EventArgs) Handles _btnApprove.Click
        If _gridIn.SelectedRows.Count > 0 OrElse _gridOut.SelectedRows.Count > 0 Then
            Dim transactionId = If(_gridIn.SelectedRows.Count > 0, CInt(_gridIn.SelectedRows(0).Cells("TransactionId").Value), CInt(_gridOut.SelectedRows(0).Cells("TransactionId").Value))
            Dim dialogResult = MessageBox.Show("Duyệt phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dialogResult = DialogResult.Yes Then
                Dim currentUser = SessionManager.GetCurrentUser()
                If currentUser Is Nothing Then
                    MessageBox.Show("Người dùng chưa đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim result = _transactionService.ApproveTransaction(transactionId, currentUser.UserId, True)
                If result.Success Then
                    MessageBox.Show("Duyệt phiếu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadTransactions()
                Else
                    MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub _txtSearch_TextChanged(sender As Object, e As EventArgs) Handles _txtSearch.TextChanged, _cmbStatus.SelectedIndexChanged
        LoadTransactions()
    End Sub
End Class
