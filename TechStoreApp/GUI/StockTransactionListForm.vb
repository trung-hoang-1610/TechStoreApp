Public Class StockTransactionListForm
    Inherits System.Windows.Forms.Form

    Private ReadOnly _myTransactionService As IStockTransactionService

    Private WithEvents _tabControl As TabControl
    Private WithEvents _gridIn As DataGridView
    Private WithEvents _gridOut As DataGridView
    Private WithEvents _btnCreateIn As Button
    Private WithEvents _btnCreateOut As Button
    Private WithEvents _btnViewDetails As Button
    Private WithEvents _btnApprove As Button
    Private WithEvents _txtSearch As TextBox
    Private WithEvents _cmbStatus As ComboBox

    Public Sub New()
        _myTransactionService = ServiceFactory.CreateStockTransactionService
        ' Gọi InitializeComponent trước để khởi tạo giao diện
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        ' Cấu hình giao diện
        ConfigureControls()

        ' Tải dữ liệu
        LoadTransactions()
        LoadStatistics()



    End Sub

    Private Sub ConfigureControls()
        ' Thêm các mục vào ComboBox và thiết lập SelectedIndex
        _cmbStatus.Items.Add("All")
        _cmbStatus.Items.Add("Pending")
        _cmbStatus.Items.Add("Approved")
        _cmbStatus.Items.Add("Rejected")
        If _cmbStatus.Items.Count > 0 Then
            _cmbStatus.SelectedIndex = 0
        End If


        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser IsNot Nothing AndAlso currentUser.RoleId = 1 Then
            _btnApprove.Visible = True
        Else
            _btnApprove.Visible = False
        End If
    End Sub

    Private Sub LoadTransactions()


        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser Is Nothing Then
            MessageBox.Show("Người dùng chưa đăng nhập. Vui lòng đăng nhập để tiếp tục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        Dim userId As Integer? = Nothing
        If currentUser.RoleId <> 1 Then
            userId = currentUser.UserId
        End If

        Dim criteria As New SearchCriteriaDTO
        If _txtSearch IsNot Nothing AndAlso Not String.IsNullOrEmpty(_txtSearch.Text) Then
            criteria.TransactionCode = _txtSearch.Text.Trim()
        Else
            criteria.TransactionCode = Nothing
        End If

        If _cmbStatus IsNot Nothing AndAlso _cmbStatus.SelectedIndex > 0 AndAlso _cmbStatus.SelectedItem IsNot Nothing Then
            criteria.Status = _cmbStatus.SelectedItem.ToString()
        Else
            criteria.Status = Nothing
        End If

        Try

            Dim inData = _myTransactionService.SearchTransactions("IN", userId, criteria)
            _gridIn.DataSource = inData
            Dim outData = _myTransactionService.SearchTransactions("OUT", userId, criteria)
            _gridOut.DataSource = outData
        Catch ex As Exception
            MessageBox.Show("Lỗi khi tải danh sách phiếu: " & ex.Message & vbCrLf & ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub _btnCreateIn_Click(sender As Object, e As EventArgs) Handles _btnCreateIn.Click
        Using selectSupplierForm As New SelectSupplierForm()
            If selectSupplierForm.ShowDialog() = DialogResult.OK Then
                LoadTransactions()  ' Tải lại danh sách sau khi tạo phiếu
            End If
        End Using
    End Sub

    Private Sub _btnCreateOut_Click(sender As Object, e As EventArgs) Handles _btnCreateOut.Click
        Using createForm As New StockTransactionCreateForm("OUT")
            If createForm.ShowDialog() = DialogResult.OK Then
                LoadTransactions()
            End If
        End Using
    End Sub

    Private Sub _gridIn_SelectionChanged(sender As Object, e As EventArgs) Handles _gridIn.SelectionChanged, _gridOut.SelectionChanged
        HandleSelectionChanged(_gridIn)

    End Sub
    Private Sub _gridOut_SelectionChanged(sender As Object, e As EventArgs) Handles _gridOut.SelectionChanged
        HandleSelectionChanged(_gridOut)
    End Sub
    Private Sub HandleSelectionChanged(grid As DataGridView)
        Dim isRowSelected = grid.SelectedRows.Count > 0
        _btnViewDetails.Enabled = isRowSelected

        Dim currentUser = SessionManager.GetCurrentUser()
        _btnApprove.Enabled = isRowSelected AndAlso (currentUser IsNot Nothing AndAlso currentUser.RoleId = 1)
    End Sub

    Private Sub _gridIn_DoubleClick(sender As Object, e As EventArgs) Handles _gridIn.DoubleClick
        ShowTransactionDetail(_gridIn)
    End Sub

    Private Sub _gridOut_DoubleClick(sender As Object, e As EventArgs) Handles _gridOut.DoubleClick
        ShowTransactionDetail(_gridOut)
    End Sub

    Private Sub ShowTransactionDetail(grid As DataGridView)
        If grid.SelectedRows.Count > 0 Then
            Dim transactionId As Integer

            If grid Is _gridIn Then
                transactionId = CInt(grid.SelectedRows(0).Cells("TransactionId").Value)
            ElseIf grid Is _gridOut Then
                transactionId = CInt(grid.SelectedRows(0).Cells("TransactionIdOut").Value)
            Else
                Exit Sub
            End If

            Using detailForm As New StockTransactionDetailForm(transactionId)
                If detailForm.ShowDialog() = DialogResult.OK Then
                    LoadTransactions()
                End If
            End Using
        End If
    End Sub

    Private Sub _btnViewDetails_Click(sender As Object, e As EventArgs) Handles _btnViewDetails.Click
        Dim selectedTab = _tabControl.SelectedTab

        If selectedTab IsNot Nothing Then
            If selectedTab.Text = "Phiếu nhập" Then
                ShowTransactionDetail(_gridIn)
            ElseIf selectedTab.Text = "Phiếu xuất" Then
                ShowTransactionDetail(_gridOut)
            End If
        End If
    End Sub


    Private Sub _btnApprove_Click(sender As Object, e As EventArgs) Handles _btnApprove.Click
        Dim selectedGrid As DataGridView = If(_gridIn.SelectedRows.Count > 0, _gridIn, _gridOut)
        If selectedGrid.SelectedRows.Count > 0 Then
            Dim transactionId As Integer
            If selectedGrid Is _gridIn Then
                transactionId = CInt(selectedGrid.SelectedRows(0).Cells("TransactionId").Value)
            Else
                transactionId = CInt(selectedGrid.SelectedRows(0).Cells("TransactionIdOut").Value)
            End If
            Dim dialogResult = MessageBox.Show("Duyệt phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dialogResult = DialogResult.Yes Then
                Dim currentUser = SessionManager.GetCurrentUser()
                If currentUser Is Nothing Then
                    MessageBox.Show("Người dùng chưa đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim result = _myTransactionService.ApproveTransaction(transactionId, currentUser.UserId, True)
                If result.Success Then
                    MessageBox.Show("Duyệt phiếu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadTransactions()
                Else
                    MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub _txtSearch_TextChanged(sender As Object, e As EventArgs) Handles _txtSearch.TextChanged, _cmbStatus.SelectedIndexChanged
        LoadTransactions()
    End Sub


    Private Sub LoadStatistics()
        Try
            Dim criteria As New SearchCriteriaDTO
            If _txtSearch IsNot Nothing AndAlso Not String.IsNullOrEmpty(_txtSearch.Text) Then
                criteria.TransactionCode = _txtSearch.Text.Trim()
            Else
                criteria.TransactionCode = Nothing
            End If

            If _cmbStatus IsNot Nothing AndAlso _cmbStatus.SelectedIndex > 0 AndAlso _cmbStatus.SelectedItem IsNot Nothing Then
                criteria.Status = _cmbStatus.SelectedItem.ToString()
            Else
                criteria.Status = Nothing
            End If

            Dim statsData As TransactionStatisticsDTO = _myTransactionService.GetTransactionStatistics(criteria)
            _lblTotalIn.Text = "Tổng phiếu nhập: " & statsData.TotalInTransactions.ToString()
            _lblTotalOut.Text = "Tổng phiếu xuất: " & statsData.TotalOutTransactions.ToString()
            _lblTotalValue.Text = "Tổng giá trị giao dịch: " & statsData.TotalTransactionValue.ToString("C")

            ' Tính và hiển thị tỷ lệ trạng thái
            Dim total As Integer = statsData.StatusBreakdown.Values.Sum()
            Dim statusText As String = "Tình trạng: "
            If total > 0 Then
                Dim statusList As New List(Of String)
                For Each kvp As KeyValuePair(Of String, Integer) In statsData.StatusBreakdown
                    Dim percentage As Double = (kvp.Value * 100.0) / total
                    statusList.Add(kvp.Key & ": " & String.Format("{0:F1}%", percentage))
                Next
                statusText &= String.Join(", ", statusList.ToArray())
            Else
                statusText &= "Chưa có dữ liệu"
            End If
            _lblStatusBreakdown.Text = statusText

            _gridStats.DataSource = statsData.TopProducts
            _gridLowStock.DataSource = statsData.LowStockProducts
        Catch ex As Exception
            MessageBox.Show("Lỗi khi tải thống kê: " & ex.Message & vbCrLf & ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub _btnExportCsv_Click(sender As Object, e As EventArgs) Handles _btnExportCsv.Click
        Using sfd As New SaveFileDialog()
            sfd.Filter = "CSV Files (*.csv)|*.csv"
            sfd.FileName = "TransactionStatistics_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Using writer As New System.IO.StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                        ' Ghi số liệu tổng quan
                        writer.WriteLine("Tổng phiếu nhập,Tổng phiếu xuất,Tổng giá trị giao dịch")
                        writer.WriteLine($"{_lblTotalIn.Text.Replace("Tổng phiếu nhập: ", "")},{_lblTotalOut.Text.Replace("Tổng phiếu xuất: ", "")},{_lblTotalValue.Text.Replace("Tổng giá trị giao dịch: ", "")}")
                        writer.WriteLine("Tình trạng," & _lblStatusBreakdown.Text.Replace("Tình trạng: ", ""))
                        writer.WriteLine()

                        ' Ghi top sản phẩm giao dịch
                        writer.WriteLine("Top sản phẩm giao dịch")
                        writer.WriteLine("Mã sản phẩm,Tên sản phẩm,Tổng số lượng,Tổng giá trị")
                        For Each row As DataGridViewRow In _gridStats.Rows
                            If row.IsNewRow Then Continue For
                            writer.WriteLine($"{row.Cells("ProductId").Value},{row.Cells("ProductName").Value},{row.Cells("TotalQuantity").Value},{row.Cells("TotalValue").Value}")
                        Next
                        writer.WriteLine()

                        ' Ghi sản phẩm dưới mức tồn kho
                        writer.WriteLine("Sản phẩm dưới mức tồn kho")
                        writer.WriteLine("Mã sản phẩm,Tên sản phẩm,Tồn kho hiện tại,Tồn kho tối thiểu")
                        For Each row As DataGridViewRow In _gridLowStock.Rows
                            If row.IsNewRow Then Continue For
                            writer.WriteLine($"{row.Cells("ProductId").Value},{row.Cells("ProductName").Value},{row.Cells("CurrentStock").Value},{row.Cells("MinimumStock").Value}")
                        Next
                    End Using
                    MessageBox.Show("Xuất CSV thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Lỗi khi xuất CSV: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub
    Private Sub StockTransactionListForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class