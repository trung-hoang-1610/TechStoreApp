Imports System.Text

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
    Private lblUsernameError As Label
    Private lblPasswordError As Label
    Private lblEmailError As Label
    Private lblRoleError As Label

    Public Sub New()
        _authService = ServiceFactory.CreateAuthService()
        _roleService = ServiceFactory.CreateRoleService()
        Me.StartPosition = FormStartPosition.CenterScreen
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
        lblUsernameError = New Label With {
            .Location = New Point(160, 55),
            .Size = New Size(200, 20),
            .ForeColor = Color.Red,
            .AutoSize = False,
            .TextAlign = ContentAlignment.TopLeft
        }

        ' Password
        Dim lblPassword As New Label With {.Text = "Mật khẩu:", .Location = New Point(30, 85), .Size = New Size(120, 25)}
        txtPassword = New TextBox With {.Location = New Point(160, 85), .Size = New Size(200, 25), .PasswordChar = "*"c}
        lblPasswordError = New Label With {
            .Location = New Point(160, 110),
            .Size = New Size(200, 20),
            .ForeColor = Color.Red,
            .AutoSize = False,
            .TextAlign = ContentAlignment.TopLeft
        }

        ' Email
        Dim lblEmail As New Label With {.Text = "Email:", .Location = New Point(30, 140), .Size = New Size(120, 25)}
        txtEmail = New TextBox With {.Location = New Point(160, 140), .Size = New Size(200, 25)}
        lblEmailError = New Label With {
            .Location = New Point(160, 165),
            .Size = New Size(200, 20),
            .ForeColor = Color.Red,
            .AutoSize = False,
            .TextAlign = ContentAlignment.TopLeft
        }

        ' Role
        Dim lblRole As New Label With {.Text = "Vai trò:", .Location = New Point(30, 195), .Size = New Size(120, 25)}
        cboRole = New ComboBox With {
            .Location = New Point(160, 195),
            .Size = New Size(200, 25),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        lblRoleError = New Label With {
            .Location = New Point(160, 220),
            .Size = New Size(200, 20),
            .ForeColor = Color.Red,
            .AutoSize = False,
            .TextAlign = ContentAlignment.TopLeft
        }

        ' Buttons
        btnRegister = New Button With {.Text = "Đăng Ký", .Location = New Point(160, 260), .Size = New Size(90, 35)}
        btnBackToLogin = New Button With {.Text = "Quay Lại", .Location = New Point(270, 260), .Size = New Size(90, 35)}

        ' Thêm tất cả controls
        Me.Controls.AddRange({
            lblUsername, txtUsername, lblUsernameError,
            lblPassword, txtPassword, lblPasswordError,
            lblEmail, txtEmail, lblEmailError,
            lblRole, cboRole, lblRoleError,
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
            cboRole.DisplayMember = "Text" ' Hiển thị chỉ RoleName
            If cboRole.Items.Count > 0 Then cboRole.SelectedIndex = 0
        Catch ex As Exception
            lblRoleError.Text = "Lỗi khi tải vai trò: " & ex.Message
        End Try
    End Sub

    Private Function IsNullOrWhiteSpace(ByVal value As String) As Boolean
        If value Is Nothing Then Return True
        If value.Length = 0 Then Return True
        For Each c As Char In value
            If Not Char.IsWhiteSpace(c) Then Return False
        Next
        Return True
    End Function

    Private Function ValidateFields(ByVal user As User, ByVal fieldErrorMap As Dictionary(Of String, Label)) As Boolean
        Dim hasError As Boolean = False
        If IsNullOrWhiteSpace(user.Username) Then
            fieldErrorMap("Username").Text = "Tên đăng nhập không được để trống"
            hasError = True
        End If
        If IsNullOrWhiteSpace(user.PasswordHash) Then
            fieldErrorMap("Password").Text = "Mật khẩu không được để trống"
            hasError = True
        ElseIf user.PasswordHash.Length <= 6 Then
            fieldErrorMap("Password").Text = "Mật khẩu phải dài hơn 6 ký tự"
            hasError = True
        End If
        If IsNullOrWhiteSpace(user.Email) Then
            fieldErrorMap("Email").Text = "Email không được để trống"
            hasError = True
        End If
        If user.RoleId = Nothing Then
            fieldErrorMap("Role").Text = "Vui lòng chọn vai trò"
            hasError = True
        End If
        Return Not hasError
    End Function

    Private Sub DisplayServiceErrors(ByVal errors As IEnumerable(Of String), ByVal fieldErrorMap As Dictionary(Of String, Label))
        Dim unmatchedErrors As New StringBuilder
        For Each _error In errors
            Dim errorLower As String = _error.ToLowerInvariant()
            If errorLower.Contains("username") Then
                fieldErrorMap("Username").Text = _error
            ElseIf errorLower.Contains("password") Then
                fieldErrorMap("Password").Text = _error
            ElseIf errorLower.Contains("email") Then
                fieldErrorMap("Email").Text = _error
            ElseIf errorLower.Contains("role") Then
                fieldErrorMap("Role").Text = _error
            Else
                unmatchedErrors.AppendLine(_error)
            End If
        Next
        If unmatchedErrors.Length > 0 Then
            fieldErrorMap("Username").Text = unmatchedErrors.ToString().Trim()
        End If
    End Sub

    Private Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click
        Try
            ' Xóa các lỗi trước đó
            Dim fieldErrorMap As New Dictionary(Of String, Label) From {
                {"Username", lblUsernameError},
                {"Password", lblPasswordError},
                {"Email", lblEmailError},
                {"Role", lblRoleError}
            }
            For Each lbl In fieldErrorMap.Values
                lbl.Text = ""
            Next

            Dim user As New User With {
                .Username = txtUsername.Text.Trim(),
                .PasswordHash = txtPassword.Text.Trim(),
                .Email = txtEmail.Text.Trim(),
                .RoleId = If(cboRole.SelectedItem IsNot Nothing, DirectCast(cboRole.SelectedItem, Object).Value, Nothing)
            }

            If ValidateFields(user, fieldErrorMap) Then
                Dim result = _authService.RegisterUser(user)
                If result.Success Then
                    MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Dim loginForm As New LoginForm()
                    loginForm.Show()
                    Me.Close()
                Else
                    DisplayServiceErrors(result.Errors, fieldErrorMap)
                End If
            End If
        Catch ex As Exception
            lblUsernameError.Text = "Đã xảy ra lỗi: " & ex.Message
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