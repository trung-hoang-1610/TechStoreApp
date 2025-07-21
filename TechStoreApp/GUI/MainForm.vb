Public Class MainForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        UpdateSelectedButton(_btnStockTransaction)
        ShowFormInPanel("StockTransactionListForm")
    End Sub

    Private Sub Button_MouseEnter(sender As Object, e As EventArgs) Handles _btnStockTransaction.MouseEnter, _btnProductManagement.MouseEnter
        Dim btn As Button = DirectCast(sender, Button)
        If btn IsNot _selectedButton Then
            btn.BackColor = Color.FromArgb(46, 56, 76)
        End If
    End Sub

    Private Sub Button_MouseLeave(sender As Object, e As EventArgs) Handles _btnStockTransaction.MouseLeave, _btnProductManagement.MouseLeave
        Dim btn As Button = DirectCast(sender, Button)
        If btn IsNot _selectedButton Then
            btn.BackColor = Color.FromArgb(36, 46, 66)
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles _btnStockTransaction.Click, _btnProductManagement.Click
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
            Case Else
                Return
        End Select

        _currentForm.TopLevel = False
        _currentForm.FormBorderStyle = FormBorderStyle.None
        _contentPanel.Controls.Add(_currentForm)
        _currentForm.Show()
    End Sub
End Class