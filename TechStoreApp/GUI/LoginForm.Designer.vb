' GUI/LoginForm.Designer.vb
Partial Public Class LoginForm

    Private WithEvents btnLogin As Button
    Private WithEvents btnRegister As Button
    Private txtUsername As TextBox
    Private txtPassword As TextBox
    Private lblError As Label

    ''' <summary>
    ''' Khởi tạo các điều khiển giao diện
    ''' </summary>
    Private Sub InitializeComponent()
        Me.Text = "Đăng Nhập"
        Me.Size = New Size(300, 230)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False

        ' Label và TextBox cho Username
        Dim lblUsername As New Label With {
            .Text = "Tên đăng nhập:",
            .Location = New Point(20, 20),
            .Size = New Size(100, 20)
        }
        txtUsername = New TextBox With {
            .Location = New Point(120, 20),
            .Size = New Size(150, 20)
        }

        ' Label và TextBox cho Password
        Dim lblPassword As New Label With {
            .Text = "Mật khẩu:",
            .Location = New Point(20, 50),
            .Size = New Size(100, 20)
        }
        txtPassword = New TextBox With {
            .Location = New Point(120, 50),
            .Size = New Size(150, 20),
            .PasswordChar = "*"c
        }

        ' Label lỗi
        lblError = New Label With {
            .Location = New Point(20, 80),
            .Size = New Size(250, 20),
            .ForeColor = Color.Red
        }

        ' Buttons
        btnLogin = New Button With {
            .Text = "Đăng Nhập",
            .Location = New Point(120, 110),
            .Size = New Size(100, 30)
        }

        btnRegister = New Button With {
            .Text = "Đăng Ký",
            .Location = New Point(120, 150),
            .Size = New Size(100, 30)
        }

        Me.Controls.Add(lblUsername)
        Me.Controls.Add(txtUsername)
        Me.Controls.Add(lblPassword)
        Me.Controls.Add(txtPassword)
        Me.Controls.Add(lblError)
        Me.Controls.Add(btnLogin)
        Me.Controls.Add(btnRegister)

    End Sub

End Class
