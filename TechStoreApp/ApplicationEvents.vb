Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' Tạo và hiển thị form bạn muốn chạy đầu tiên
            Dim frm As New FormProducts()
            frm.Show()

            ' Tắt StartupForm mặc định để tránh mở thêm Form1
            ' (Bằng cách đặt ShutdownMode = AfterMainFormCloses rồi đóng luôn)
            Me.MainForm = frm
        End Sub
    End Class
End Namespace
