
Partial Public Class LoginForm
    Inherits Form

    Private ReadOnly _authService As IAuthService

    ''' <summary>
    ''' Khởi tạo LoginForm và các service cần thiết
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        _authService = ServiceFactory.CreateAuthService()
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Đăng nhập
    ''' </summary>
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try
            lblError.Text = String.Empty
            Dim user = _authService.ValidateUser(txtUsername.Text, txtPassword.Text)
            If user IsNot Nothing Then
                SessionManager.SetCurrentUser(user)

                Me.Hide()
                Dim MainForm As New MainForm()
                MainForm.ShowDialog()
                Me.Close()
                'Me.Hide()
                'Dim StockTransactionListForm As New StockTransactionListForm()
                'StockTransactionListForm.ShowDialog()
                'Me.Close()
                'Me.Hide()
                'Dim ProductManagementForm As New ProductManagementForm(user.UserId)
                'ProductManagementForm.ShowDialog()

                'Me.Close()
            Else
                lblError.Text = "Tên đăng nhập hoặc mật khẩu không đúng."
            End If
        Catch ex As ArgumentNullException
            lblError.Text = ex.Message
        Catch ex As Exception
            lblError.Text = "Đã xảy ra lỗi: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Đăng ký
    ''' </summary>
    Private Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click
        Dim registerForm As New RegisterForm()
        registerForm.Show()
        Me.Hide()
    End Sub

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
