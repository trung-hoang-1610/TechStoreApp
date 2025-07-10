
Partial Public Class LoginForm
    Inherits Form

    Private ReadOnly _authService As IAuthService

    ''' <summary>
    ''' Khởi tạo LoginForm và các service cần thiết
    ''' </summary>
    Public Sub New()
        InitializeComponent()
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
                Dim ProductManagementForm As New ProductManagementForm(user.UserId)
                ProductManagementForm.Show()
                Me.Hide()
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

End Class
