Public Class StockTransactionListForm
    Inherits System.Windows.Forms.Form

    Private ReadOnly _myTransactionService As IStockTransactionService
    Private WithEvents _tabControl As TabControl
    Private WithEvents _gridIn As DataGridView
    Private WithEvents _gridOut As DataGridView
    Private WithEvents _btnCreateIn As Button
    Private WithEvents _btnCreateOut As Button
    Private WithEvents _btnViewDetails As Button
    Private WithEvents _btnApprove As Button
    Private WithEvents _txtSearch As TextBox
    Private WithEvents _cmbStatus As ComboBox

    Public Sub New()
        _myTransactionService = ServiceFactory.CreateStockTransactionService
        MessageBox.Show("_transactionService được tạo: " & If(_myTransactionService IsNot Nothing, "OK", "NULL"))

        ' Gọi InitializeComponent trước để khởi tạo giao diện
        InitializeComponent()

        ' Khởi tạo dịch vụ




        ' Cấu hình giao diện
        ConfigureControls()

        ' Tải dữ liệu
        LoadTransactions()


    End Sub

    Private Sub ConfigureControls()
        ' Thêm các mục vào ComboBox và thiết lập SelectedIndex
        _cmbStatus.Items.Add("All")
        _cmbStatus.Items.Add("Pending")
        _cmbStatus.Items.Add("Approved")
        _cmbStatus.Items.Add("Rejected")
        If _cmbStatus.Items.Count > 0 Then
            _cmbStatus.SelectedIndex = 0
        End If

        ' Cấu hình nút Approve dựa trên vai trò người dùng
        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser IsNot Nothing AndAlso currentUser.RoleId = 1 Then
            _btnApprove.Visible = True
        Else
            _btnApprove.Visible = False
        End If
    End Sub

    Private Sub LoadTransactions()


        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser Is Nothing Then
            MessageBox.Show("Người dùng chưa đăng nhập. Vui lòng đăng nhập để tiếp tục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        Dim userId As Integer? = Nothing
        If currentUser.RoleId <> 1 Then
            userId = currentUser.UserId
        End If

        Dim criteria As New SearchCriteriaDTO
        If _txtSearch IsNot Nothing AndAlso Not String.IsNullOrEmpty(_txtSearch.Text) Then
            criteria.TransactionCode = _txtSearch.Text.Trim()
        Else
            criteria.TransactionCode = Nothing
        End If

        If _cmbStatus IsNot Nothing AndAlso _cmbStatus.SelectedIndex > 0 AndAlso _cmbStatus.SelectedItem IsNot Nothing Then
            criteria.Status = _cmbStatus.SelectedItem.ToString()
        Else
            criteria.Status = Nothing
        End If

        Try

            Dim inData = _myTransactionService.SearchTransactions("IN", userId, criteria)
            _gridIn.DataSource = inData
            Dim outData = _myTransactionService.SearchTransactions("OUT", userId, criteria)
            _gridOut.DataSource = outData
        Catch ex As Exception
            MessageBox.Show("Lỗi khi tải danh sách phiếu: " & ex.Message & vbCrLf & ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub _btnCreateIn_Click(sender As Object, e As EventArgs) Handles _btnCreateIn.Click
        Using createForm As New StockTransactionCreateForm("IN")
            If createForm.ShowDialog() = DialogResult.OK Then
                LoadTransactions()
            End If
        End Using
    End Sub

    Private Sub _btnCreateOut_Click(sender As Object, e As EventArgs) Handles _btnCreateOut.Click
        Using createForm As New StockTransactionCreateForm("OUT")
            If createForm.ShowDialog() = DialogResult.OK Then
                LoadTransactions()
            End If
        End Using
    End Sub

    Private Sub _gridIn_SelectionChanged(sender As Object, e As EventArgs) Handles _gridIn.SelectionChanged, _gridOut.SelectionChanged
        Dim isRowSelected = _gridIn.SelectedRows.Count > 0 OrElse _gridOut.SelectedRows.Count > 0
        _btnViewDetails.Enabled = isRowSelected
        Dim currentUser = SessionManager.GetCurrentUser()
        _btnApprove.Enabled = isRowSelected AndAlso (currentUser IsNot Nothing AndAlso currentUser.RoleId = 1)
    End Sub

    Private Sub _gridIn_DoubleClick(sender As Object, e As EventArgs) Handles _gridIn.DoubleClick, _gridOut.DoubleClick
        Dim selectedGrid As DataGridView = If(sender Is _gridIn, _gridIn, _gridOut)
        If selectedGrid.SelectedRows.Count > 0 Then
            Dim transactionId As Integer = CInt(selectedGrid.SelectedRows(0).Cells("TransactionId").Value)
            Using detailForm As New StockTransactionDetailForm(transactionId)
                detailForm.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub _btnViewDetails_Click(sender As Object, e As EventArgs) Handles _btnViewDetails.Click
        Dim selectedGrid As DataGridView = If(_gridIn.SelectedRows.Count > 0, _gridIn, _gridOut)
        If selectedGrid.SelectedRows.Count > 0 Then
            Dim transactionId As Integer = CInt(selectedGrid.SelectedRows(0).Cells("TransactionId").Value)
            Using detailForm As New StockTransactionDetailForm(transactionId)
                detailForm.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub _btnApprove_Click(sender As Object, e As EventArgs) Handles _btnApprove.Click
        Dim selectedGrid As DataGridView = If(_gridIn.SelectedRows.Count > 0, _gridIn, _gridOut)
        If selectedGrid.SelectedRows.Count > 0 Then
            Dim transactionId As Integer = CInt(selectedGrid.SelectedRows(0).Cells("TransactionId").Value)
            Dim dialogResult = MessageBox.Show("Duyệt phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dialogResult = DialogResult.Yes Then
                Dim currentUser = SessionManager.GetCurrentUser()
                If currentUser Is Nothing Then
                    MessageBox.Show("Người dùng chưa đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim result = _myTransactionService.ApproveTransaction(transactionId, currentUser.UserId, True)
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