' Common/Utilities/ValidationHelper.vb
Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Public Class ValidationHelper
    ''' <summary>
    ''' Kiểm tra giá trị chuỗi và thêm lỗi vào danh sách nếu không hợp lệ
    ''' </summary>
    ''' <param name="value">Giá trị chuỗi cần kiểm tra</param>
    ''' <param name="fieldName">Tên trường hiển thị trong thông báo lỗi</param>
    ''' <param name="errors">Danh sách lỗi để thêm vào</param>
    ''' <param name="isRequired">Xác định xem trường có bắt buộc hay không</param>
    ''' <param name="maxLength">Độ dài tối đa của chuỗi (0 nếu không giới hạn)</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu danh sách errors là Nothing</exception>
    Public Shared Sub ValidateString(ByVal value As String, ByVal fieldName As String, ByVal errors As List(Of String), Optional ByVal isRequired As Boolean = True, Optional ByVal maxLength As Integer = 0)
        If errors Is Nothing Then
            Throw New ArgumentNullException("errors", "Danh sách lỗi rỗng.")
        End If

        If isRequired AndAlso String.IsNullOrEmpty(value) Then
            errors.Add(fieldName & " không được để trống.")
        ElseIf maxLength > 0 AndAlso Not String.IsNullOrEmpty(value) AndAlso value.Length > maxLength Then
            errors.Add(fieldName & " không được dài quá " & maxLength & " ký tự.")
        End If
    End Sub

    ''' <summary>
    ''' Kiểm tra giá trị số thập phân và thêm lỗi vào danh sách nếu không hợp lệ
    ''' </summary>
    ''' <param name="value">Giá trị số thập phân cần kiểm tra</param>
    ''' <param name="fieldName">Tên trường hiển thị trong thông báo lỗi</param>
    ''' <param name="errors">Danh sách lỗi để thêm vào</param>
    ''' <param name="minValue">Giá trị tối thiểu cho phép</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu danh sách errors là Nothing</exception>
    Public Shared Sub ValidateDecimal(ByVal value As Decimal, ByVal fieldName As String, ByVal errors As List(Of String), Optional ByVal minValue As Decimal = 0)
        If errors Is Nothing Then
            Throw New ArgumentNullException("errors", "Danh sách lỗi không được là Nothing.")
        End If

        If value < minValue Then
            errors.Add(fieldName & " phải lớn hơn hoặc bằng " & minValue & ".")
        End If
    End Sub

    ''' <summary>
    ''' Kiểm tra giá trị số nguyên và thêm lỗi vào danh sách nếu không hợp lệ
    ''' </summary>
    ''' <param name="value">Giá trị số nguyên cần kiểm tra</param>
    ''' <param name="fieldName">Tên trường hiển thị trong thông báo lỗi</param>
    ''' <param name="errors">Danh sách lỗi để thêm vào</param>
    ''' <param name="minValue">Giá trị tối thiểu cho phép</param>
    ''' <exception cref="ArgumentNullException">Ném ra nếu danh sách errors là Nothing</exception>
    Public Shared Sub ValidateInteger(ByVal value As Integer, ByVal fieldName As String, ByVal errors As List(Of String), Optional ByVal minValue As Integer = 0)
        If errors Is Nothing Then
            Throw New ArgumentNullException("errors", "Danh sách lỗi không được là Nothing.")
        End If

        If value < minValue Then
            errors.Add(fieldName & " phải lớn hơn hoặc bằng " & minValue & ".")
        End If
    End Sub

    ''' <summary>
    ''' Kiểm tra tên đăng nhập theo định dạng và yêu cầu bắt buộc
    ''' </summary>
    ''' <param name="username">Tên đăng nhập cần kiểm tra</param>
    ''' <param name="errors">Danh sách lỗi để thêm vào</param>
    ''' <param name="isRequired">Xác định trường này có bắt buộc không</param>
    ''' <param name="maxLength">Độ dài tối đa cho phép</param>
    Public Shared Sub ValidateUsername(ByVal username As String, ByVal errors As List(Of String), Optional ByVal isRequired As Boolean = True, Optional ByVal maxLength As Integer = 50)
        If errors Is Nothing Then
            Throw New ArgumentNullException(NameOf(errors), "Danh sách lỗi không được là Nothing.")
        End If

        If isRequired AndAlso String.IsNullOrEmpty(username) Then
            errors.Add("Tên đăng nhập không được để trống.")
            Return
        End If

        If maxLength > 0 AndAlso Not String.IsNullOrEmpty(username) AndAlso username.Length > maxLength Then
            errors.Add("Tên đăng nhập không được dài quá " & maxLength & " ký tự.")
        End If

        ' Chỉ cho phép chữ cái, số, gạch dưới, không khoảng trắng
        If Not Regex.IsMatch(username, "^[a-zA-Z0-9_]+$") Then
            errors.Add("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới.")
        End If
    End Sub

    ''' <summary>
    ''' Kiểm tra mật khẩu theo yêu cầu bảo mật cơ bản
    ''' </summary>
    ''' <param name="password">Mật khẩu cần kiểm tra</param>
    ''' <param name="errors">Danh sách lỗi để thêm vào</param>
    ''' <param name="isRequired">Xác định trường này có bắt buộc không</param>
    ''' <param name="minLength">Độ dài tối thiểu</param>
    ''' <param name="maxLength">Độ dài tối đa</param>
    Public Shared Sub ValidatePassword(ByVal password As String, ByVal errors As List(Of String), Optional ByVal isRequired As Boolean = True, Optional ByVal minLength As Integer = 6, Optional ByVal maxLength As Integer = 100)
        If errors Is Nothing Then
            Throw New ArgumentNullException(NameOf(errors), "Danh sách lỗi không được là Nothing.")
        End If

        If isRequired AndAlso String.IsNullOrEmpty(password) Then
            errors.Add("Mật khẩu không được để trống.")
            Return
        End If

        If Not String.IsNullOrEmpty(password) Then
            If password.Length < minLength Then
                errors.Add("Mật khẩu phải có ít nhất " & minLength & " ký tự.")
            End If
            If maxLength > 0 AndAlso password.Length > maxLength Then
                errors.Add("Mật khẩu không được dài quá " & maxLength & " ký tự.")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Kiểm tra định dạng email
    ''' </summary>
    ''' <param name="email">Email cần kiểm tra</param>
    ''' <param name="errors">Danh sách lỗi để thêm vào</param>
    ''' <param name="isRequired">Xác định trường này có bắt buộc không</param>
    ''' <param name="maxLength">Độ dài tối đa của email</param>
    Public Shared Sub ValidateEmail(ByVal email As String, ByVal errors As List(Of String), Optional ByVal isRequired As Boolean = True, Optional ByVal maxLength As Integer = 100)
        If errors Is Nothing Then
            Throw New ArgumentNullException(NameOf(errors), "Danh sách lỗi không được là Nothing.")
        End If

        If isRequired AndAlso String.IsNullOrEmpty(email) Then
            errors.Add("Email không được để trống.")
            Return
        End If

        If Not String.IsNullOrEmpty(email) Then
            If maxLength > 0 AndAlso email.Length > maxLength Then
                errors.Add("Email không được dài quá " & maxLength & " ký tự.")
            End If

            Dim emailPattern As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
            If Not Regex.IsMatch(email, emailPattern) Then
                errors.Add("Email không đúng định dạng.")
            End If
        End If
    End Sub
End Class