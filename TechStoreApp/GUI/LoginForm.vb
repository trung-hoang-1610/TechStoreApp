
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
    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            lblError.Text = ""
            Dim user = Await _authService.ValidateUser(txtUsername.Text, txtPassword.Text)
            If user IsNot Nothing Then
                SessionManager.SetCurrentUser(user)
                Dim mainForm As New MainForm()
                Me.Hide()
                mainForm.ShowDialog()
                Me.Close() ' Khi mainForm đóng, quay lại LoginForm
            Else
                lblError.Text = "Tên đăng nhập hoặc mật khẩu không đúng."
            End If
        Catch ex As Exception
            lblError.Text = "Đã xảy ra lỗi: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Đăng ký
    ''' </summary>
    Private Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click
        Dim registerForm As New RegisterForm()

        registerForm.ShowDialog()
    End Sub

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub LoginForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub
End Class
