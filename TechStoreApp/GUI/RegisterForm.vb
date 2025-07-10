Public Class RegisterForm
    Inherits Form

    Private ReadOnly _authService As IAuthService
    Private ReadOnly _roleService As IRoleService

    ' Các điều khiển giao diện
    Private WithEvents btnRegister As Button
    Private WithEvents btnBackToLogin As Button
    Private txtUsername As TextBox
    Private txtPassword As TextBox
    Private txtEmail As TextBox
    Private cboRole As ComboBox
    Private lblError As Label

    Public Sub New()
        _authService = ServiceFactory.CreateAuthService()
        _roleService = ServiceFactory.CreateRoleService()
        InitializeComponents()
        LoadRoles()
    End Sub

    Private Sub InitializeComponents()
        Me.Text = "Đăng Ký Tài Khoản"
        Me.Size = New Size(420, 380)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MaximizeBox = False
        Me.Font = New Font("Segoe UI", 10)

        ' Username
        Dim lblUsername As New Label With {.Text = "Tên đăng nhập:", .Location = New Point(30, 30), .Size = New Size(120, 25)}
        txtUsername = New TextBox With {.Location = New Point(160, 30), .Size = New Size(200, 25)}

        ' Password
        Dim lblPassword As New Label With {.Text = "Mật khẩu:", .Location = New Point(30, 70), .Size = New Size(120, 25)}
        txtPassword = New TextBox With {.Location = New Point(160, 70), .Size = New Size(200, 25), .PasswordChar = "*"c}

        ' Email
        Dim lblEmail As New Label With {.Text = "Email:", .Location = New Point(30, 110), .Size = New Size(120, 25)}
        txtEmail = New TextBox With {.Location = New Point(160, 110), .Size = New Size(200, 25)}

        ' Role
        Dim lblRole As New Label With {.Text = "Vai trò:", .Location = New Point(30, 150), .Size = New Size(120, 25)}
        cboRole = New ComboBox With {
            .Location = New Point(160, 150),
            .Size = New Size(200, 25),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }

        ' Error label
        lblError = New Label With {
    .Location = New Point(30, 190),
    .Size = New Size(330, 60),
    .ForeColor = Color.Red,
    .AutoSize = False,
    .BorderStyle = BorderStyle.FixedSingle,
    .TextAlign = ContentAlignment.TopLeft}

        ' Buttons
        btnRegister = New Button With {.Text = "Đăng Ký", .Location = New Point(160, 240), .Size = New Size(90, 35)}
        btnBackToLogin = New Button With {.Text = "Quay Lại", .Location = New Point(270, 240), .Size = New Size(90, 35)}

        ' Thêm tất cả controls
        Me.Controls.AddRange({
            lblUsername, txtUsername,
            lblPassword, txtPassword,
            lblEmail, txtEmail,
            lblRole, cboRole,
            lblError,
            btnRegister, btnBackToLogin
        })
    End Sub

    Private Sub LoadRoles()
        Try
            cboRole.Items.Clear()
            Dim roles = _roleService.GetAllRoles()
            For Each role In roles
                cboRole.Items.Add(New With {.Text = role.RoleName, .Value = role.RoleId})
            Next
            If cboRole.Items.Count > 0 Then cboRole.SelectedIndex = 0
        Catch ex As Exception
            lblError.Text = "Lỗi khi tải vai trò: " & ex.Message
        End Try
    End Sub

    Private Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click
        Try
            lblError.Text = ""
            Dim user As New User With {
                .Username = txtUsername.Text.Trim(),
                .PasswordHash = txtPassword.Text.Trim(),
                .Email = txtEmail.Text.Trim(),
                .RoleId = DirectCast(cboRole.SelectedItem, Object).Value
            }

            Dim result = _authService.RegisterUser(user)
            If result.Success Then
                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim loginForm As New LoginForm()
                loginForm.Show()
                Me.Close()
            Else
                lblError.Text = String.Join(vbCrLf, result.Errors.ToArray())
            End If
        Catch ex As Exception
            lblError.Text = "Đã xảy ra lỗi: " & ex.Message
        End Try
    End Sub

    Private Sub btnBackToLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackToLogin.Click
        Dim loginForm As New LoginForm()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub RegisterForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
