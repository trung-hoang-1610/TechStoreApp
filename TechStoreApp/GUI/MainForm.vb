Public Class MainForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
        lbUserName.Text = SessionManager.GetCurrentUser.Username
        Me.StartPosition = FormStartPosition.CenterScreen
        UpdateSelectedButton(_btnStockTransaction)
        ShowFormInPanel("StockTransactionListForm")


        btnLogOut.Visible = False
    End Sub

    Private Sub Button_MouseEnter(sender As Object, e As EventArgs) Handles _btnStockTransaction.MouseEnter, _btnProductManagement.MouseEnter, _btnSupplierManagement.Click
        Dim btn As Button = DirectCast(sender, Button)
        If btn IsNot _selectedButton Then
            btn.BackColor = Color.FromArgb(46, 56, 76)
        End If
    End Sub

    Private Sub Button_MouseLeave(sender As Object, e As EventArgs) Handles _btnStockTransaction.MouseLeave, _btnProductManagement.MouseLeave, _btnSupplierManagement.Click
        Dim btn As Button = DirectCast(sender, Button)
        If btn IsNot _selectedButton Then
            btn.BackColor = Color.FromArgb(36, 46, 66)
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles _btnStockTransaction.Click, _btnProductManagement.Click, _btnSupplierManagement.Click
        Dim btn As Button = DirectCast(sender, Button)
        UpdateSelectedButton(btn)
        ShowFormInPanel(btn.Tag.ToString())
    End Sub

    Private Sub UpdateSelectedButton(btn As Button)
        If _selectedButton IsNot Nothing Then
            _selectedButton.BackColor = Color.FromArgb(36, 46, 66)
            _selectedButton.ForeColor = Color.White
        End If
        _selectedButton = btn
        _selectedButton.BackColor = Color.FromArgb(59, 130, 246)
        _selectedButton.ForeColor = Color.White
    End Sub

    Private Sub ShowFormInPanel(formName As String)
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _contentPanel.Controls.Clear()
        End If

        Select Case formName
            Case "StockTransactionListForm"
                _currentForm = New StockTransactionListForm()
            Case "ProductManagementForm"
                _currentForm = New ProductManagementForm(SessionManager.GetCurrentUser.UserId)
            Case "SupplierManagementForm"
                _currentForm = New SupplierManagementForm()
            Case Else
                Return
        End Select

        With _currentForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .StartPosition = FormStartPosition.Manual
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Location = New Point(
                (_contentPanel.Width - .Width) \ 2
            )
        End With

        _contentPanel.Controls.Add(_currentForm)
        _currentForm.Show()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles lbUserName.Click

    End Sub

    Private Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Dim result As DialogResult = MessageBox.Show(
            "Bạn có chắc chắn muốn đăng xuất không?",
            "Xác nhận đăng xuất",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result = DialogResult.Yes Then
            SessionManager.SetCurrentUser(Nothing)
            Me.Hide()

            ' Mở LoginForm theo modal
            Dim loginForm As New LoginForm()
            If loginForm.ShowDialog() = DialogResult.OK Then
                ' Nếu đăng nhập lại thành công, khởi động lại MainForm mới
                Dim mainForm As New MainForm()
                mainForm.Show()
                Me.Close() ' Đóng form hiện tại
            Else
                ' Nếu đăng nhập thất bại hoặc người dùng hủy, thoát app
                Application.Exit()
            End If
        End If
    End Sub






End Class