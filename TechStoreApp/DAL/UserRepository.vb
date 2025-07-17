' DAL/UserRepository.vb
Imports System.Data.Odbc
Imports System.Security.Cryptography
Imports System.Text

Public Class UserRepository
    Implements IUserRepository


    ''' <summary>
    ''' Lấy tất cả người dùng từ cơ sở dữ liệu
    ''' </summary>
    ''' <returns>Danh sách các đối tượng User, hoặc danh sách rỗng nếu không có người dùng</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetAllUsers() As List(Of User) Implements IUserRepository.GetAllUsers
        Dim users As New List(Of User)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT UserId, Username, PasswordHash, Email, RoleId, CreatedAt FROM Users"
            Using command As New OdbcCommand(query, connection)
                Try
                    If connection.State <> ConnectionState.Open Then
                        connection.Open()
                    End If

                    Using reader As OdbcDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim user As New User
                            user.GetType().GetField("_userId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(user, reader.GetInt32(0))
                            user.Username = reader.GetString(1)
                            user.PasswordHash = reader.GetString(2)
                            user.Email = If(reader.IsDBNull(3), Nothing, reader.GetString(3))
                            user.RoleId = reader.GetInt32(4)
                            user.GetType().GetField("_createdAt", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(user, reader.GetDateTime(5))
                            users.Add(user)
                        End While
                    End Using
                Catch ex As OdbcException
                    Debug.WriteLine($"Lỗi cơ sở dữ liệu trong GetAllUsers: {ex.Message}")
                    Throw
                Catch ex As Exception
                    Debug.WriteLine($"Lỗi hệ thống trong GetAllUsers: {ex.Message}")
                    Throw
                Finally
                    If connection.State = ConnectionState.Open Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Using
        Return users
    End Function
    ''' <summary>
    ''' Lấy người dùng theo mã định danh từ cơ sở dữ liệu
    ''' </summary>
    ''' <param name="id">Mã định danh của người dùng</param>
    ''' <returns>Đối tượng User hoặc Nothing nếu không tìm thấy</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    Public Function GetUserById(ByVal id As Integer) As User Implements IUserRepository.GetUserById
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim query As String = "SELECT UserId, Username, PasswordHash, Email, RoleId, CreatedAt FROM Users WHERE UserId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        Dim user As New User
                        user.GetType().GetField("_userId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(user, reader.GetInt32(0))
                        user.Username = reader.GetString(1)
                        user.PasswordHash = reader.GetString(2)
                        user.Email = If(reader.IsDBNull(3), Nothing, reader.GetString(3))
                        user.RoleId = reader.GetInt32(4)
                        user.GetType().GetField("_createdAt", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(user, reader.GetDateTime(5))
                        Return user
                    End If
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Thêm người dùng mới vào cơ sở dữ liệu
    ''' </summary>
    ''' <param name="user">Đối tượng User cần thêm</param>
    ''' <returns>Mã định danh của người dùng mới</returns>
    ''' <exception cref="OdbcException">Ném ra nếu có lỗi khi thêm vào cơ sở dữ liệu</exception>
    Public Function AddUser(ByVal user As User) As Integer Implements IUserRepository.AddUser
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Dim insertQuery As String = "INSERT INTO Users (Username, PasswordHash, Email, RoleId) VALUES (?, ?, ?, ?)"
            Using command As New OdbcCommand(insertQuery, connection)
                command.Parameters.AddWithValue("", user.Username)
                command.Parameters.AddWithValue("", user.PasswordHash)
                command.Parameters.AddWithValue("", If(String.IsNullOrEmpty(user.Email), DBNull.Value, user.Email))
                command.Parameters.AddWithValue("", user.RoleId)
                command.ExecuteNonQuery()
            End Using

            ' Lấy ID mới thêm
            Using idCommand As New OdbcCommand("SELECT LAST_INSERT_ID()", connection)
                Return Convert.ToInt32(idCommand.ExecuteScalar())
            End Using
        End Using
    End Function


    ''' <summary>
    ''' Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
    ''' </summary>
    ''' <param name="username">Tên đăng nhập</param>
    ''' <param name="password">Mật khẩu chưa mã hóa</param>
    ''' <returns>Đối tượng User nếu xác thực thành công, Nothing nếu thất bại</returns>
    ''' <exception cref="System.Data.Odbc.OdbcException">Ném ra nếu có lỗi khi truy vấn cơ sở dữ liệu</exception>
    ''' <exception cref="ArgumentNullException">Ném ra nếu username hoặc password là Nothing</exception>
    Public Function ValidateUser(ByVal username As String, ByVal password As String) As User Implements IUserRepository.ValidateUser
        If username Is Nothing Then
            Throw New ArgumentNullException("username", "Tên đăng nhập không được là Nothing.")
        End If
        If password Is Nothing Then
            Throw New ArgumentNullException("password", "Mật khẩu không được là Nothing.")
        End If
        Dim query As String = "SELECT UserId, Username, PasswordHash, Email, RoleId, CreatedAt FROM Users WHERE Username = ?"

        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            Try
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If

                Using command As New OdbcCommand(query, connection)
                    command.Parameters.AddWithValue("username", username)

                    Using reader As OdbcDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            Dim storedHash As String = reader.GetString(2)
                            Dim inputHash As String = password

                            If storedHash = inputHash Then
                                Dim user As New User()
                                user.GetType().GetField("_userId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(user, reader.GetInt32(0))
                                user.Username = reader.GetString(1)
                                user.PasswordHash = storedHash
                                user.Email = If(reader.IsDBNull(3), Nothing, reader.GetString(3))
                                user.RoleId = reader.GetInt32(4)
                                user.GetType().GetField("_createdAt", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(user, reader.GetDateTime(5))

                                Return user
                            Else
                                Debug.WriteLine("Mật khẩu không khớp. Stored: " & storedHash & ", Input: " & inputHash)
                            End If
                        Else
                            Debug.WriteLine("Không tìm thấy người dùng với Username: " & username)
                        End If
                    End Using
                End Using
            Catch ex As OdbcException
                Debug.WriteLine("Lỗi cơ sở dữ liệu: " & ex.Message & ", Query: " & query & ", Username: " & username)
                Throw
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try
        End Using

        Return Nothing
    End Function

    Public Function GetUserByUsername(username As String) As User Implements IUserRepository.GetUserByUsername
        Throw New NotImplementedException()
    End Function
End Class