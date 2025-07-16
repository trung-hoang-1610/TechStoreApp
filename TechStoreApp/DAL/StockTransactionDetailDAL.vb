Imports System.Data.Odbc
Imports System.Collections.Generic

''' <summary>
''' Lớp DAL cho các thao tác liên quan đến chi tiết phiếu nhập/xuất kho.
''' </summary>
Public Class StockTransactionDetailDAL
    Implements IStockTransactionDetailDAL


    ''' <summary>
    ''' Tạo danh sách chi tiết cho một phiếu nhập/xuất.
    ''' </summary>
    Public Function CreateTransactionDetails(ByVal details As List(Of StockTransactionDetail)) As Boolean Implements IStockTransactionDetailDAL.CreateTransactionDetails
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "INSERT INTO StockTransactionDetails (TransactionId, ProductId, Quantity, Note) VALUES (?, ?, ?, ?)"
            Using command As New OdbcCommand(query, connection)
                For Each detail As StockTransactionDetail In details
                    command.Parameters.Clear()
                    command.Parameters.AddWithValue("?", detail.TransactionId)
                    command.Parameters.AddWithValue("?", detail.ProductId)
                    command.Parameters.AddWithValue("?", detail.Quantity)
                    command.Parameters.AddWithValue("?", If(String.IsNullOrEmpty(detail.Note), DBNull.Value, detail.Note))
                    command.ExecuteNonQuery()
                Next
                Return True
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
    End Function

    ''' <summary>
    ''' Lấy danh sách chi tiết của một phiếu.
    ''' </summary>
    Public Function GetTransactionDetails(ByVal transactionId As Integer) As List(Of StockTransactionDetail) Implements IStockTransactionDetailDAL.GetTransactionDetails
        Dim details As New List(Of StockTransactionDetail)
        Using connection As OdbcConnection = ConnectionHelper.GetConnection()
            connection.Open()
            Dim query As String = "SELECT d.*, p.ProductName, p.Unit " &
                                 "FROM StockTransactionDetails d " &
                                 "LEFT JOIN Products p ON d.ProductId = p.ProductId " &
                                 "WHERE d.TransactionId = ?"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("?", transactionId)
                Using reader As OdbcDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim detail As New StockTransactionDetail()
                        detail.SetDetailId(reader.GetInt32(reader.GetOrdinal("DetailId")))
                        detail.TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId"))
                        detail.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"))
                        detail.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                        detail.Note = If(reader.IsDBNull(reader.GetOrdinal("Note")), Nothing, reader.GetString(reader.GetOrdinal("Note")))
                        details.Add(detail)
                    End While
                End Using
            End Using
            ConnectionHelper.CloseConnection(connection)

        End Using
        Return details
    End Function
End Class