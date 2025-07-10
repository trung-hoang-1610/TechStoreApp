' Common/Utilities/ValidationHelper.vb
Imports System.Collections.Generic

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
End Class