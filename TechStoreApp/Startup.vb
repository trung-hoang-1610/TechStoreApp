Module Startup
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Do
            Dim loginForm As New LoginForm()
            If loginForm.ShowDialog() = DialogResult.OK Then
                Dim mainForm As New MainForm()
                Application.Run(mainForm)
                If Not mainForm.IsLogout Then
                    Exit Do ' Thoát ứng dụng nếu không phải đăng xuất
                End If
            Else
                Exit Do ' Thoát nếu không đăng nhập
            End If
        Loop
    End Sub
End Module