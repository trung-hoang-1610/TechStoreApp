' Common/SessionManager.vb
Public Class SessionManager
    Private Shared _currentUser As User

    ''' <summary>
    ''' Lưu thông tin người dùng hiện tại sau khi đăng nhập
    ''' </summary>
    ''' <param name="user">Đối tượng User chứa thông tin người dùng</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu user là Nothing</exception>
    Public Shared Sub SetCurrentUser(ByVal user As User)
        If user Is Nothing Then
            Throw New ArgumentNullException("user", "Đối tượng User không được là Nothing.")
        End If
        _currentUser = user
    End Sub

    ''' <summary>
    ''' Lấy thông tin người dùng hiện tại
    ''' </summary>
    ''' <returns>Đối tượng User hoặc Nothing nếu chưa đăng nhập</returns>
    Public Shared Function GetCurrentUser() As User
        Return _currentUser
    End Function

    ''' <summary>
    ''' Xóa thông tin người dùng hiện tại
    ''' </summary>
    Public Shared Sub ClearSession()
        _currentUser = Nothing
    End Sub
End Class