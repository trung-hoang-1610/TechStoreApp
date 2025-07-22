Imports System.ComponentModel

Public Class StockTransactionListForm
    Inherits System.Windows.Forms.Form

    Private ReadOnly _myTransactionService As IStockTransactionService
    Private WithEvents _backgroundWorkerLoad As BackgroundWorker
    Private WithEvents _backgroundWorkerStats As BackgroundWorker

    Private WithEvents _tabControl As TabControl
    Private WithEvents _gridIn As DataGridView
    Private WithEvents _gridOut As DataGridView
    Private WithEvents _btnCreateIn As Button
    Private WithEvents _btnCreateOut As Button
    Private WithEvents _btnViewDetails As Button
    Private WithEvents _btnApprove As Button
    Private WithEvents _txtSearch As TextBox
    Private WithEvents _cmbStatus As ComboBox

    Friend WithEvents _btnPrevPageIn As Button
    Friend WithEvents _btnNextPageIn As Button
    Friend WithEvents _lblPagingStatusIn As Label

    Friend WithEvents _btnPrevPageOut As Button
    Friend WithEvents _btnNextPageOut As Button
    Friend WithEvents _lblPagingStatusOut As Label

    Private ReadOnly _criteriaIn As StockTransationSearchCriterialDTO
    Private ReadOnly _criteriaOut As StockTransationSearchCriterialDTO

    Public Sub New()
        _myTransactionService = ServiceFactory.CreateStockTransactionService
        InitializeComponent()
        InitializeBackgroundWorkers()
        _gridIn.AutoGenerateColumns = False
        _gridOut.AutoGenerateColumns = False
        Me.StartPosition = FormStartPosition.CenterScreen
        _criteriaIn = New StockTransationSearchCriterialDTO With {.PageIndex = 1, .PageSize = 10}
        _criteriaOut = New StockTransationSearchCriterialDTO With {.PageIndex = 1, .PageSize = 10}
        ConfigureControls()
        StartLoadTransactions()
        StartLoadStatistics()
    End Sub

    Private Sub InitializeBackgroundWorkers()
        _backgroundWorkerLoad = New BackgroundWorker()
        _backgroundWorkerLoad.WorkerSupportsCancellation = True
        _backgroundWorkerLoad.WorkerReportsProgress = False

        _backgroundWorkerStats = New BackgroundWorker()
        _backgroundWorkerStats.WorkerSupportsCancellation = True
        _backgroundWorkerStats.WorkerReportsProgress = False
    End Sub

    Private Sub ConfigureControls()
        _cmbStatus.Items.AddRange({"All", "Pending", "Approved", "Rejected"})
        If _cmbStatus.Items.Count > 0 Then
            _cmbStatus.SelectedIndex = 0
        End If

        Dim currentUser = SessionManager.GetCurrentUser()
        _btnApprove.Visible = (currentUser IsNot Nothing AndAlso currentUser.RoleId = 1)
    End Sub

    Private Sub StartLoadTransactions()
        If _backgroundWorkerLoad.IsBusy Then
            _backgroundWorkerLoad.CancelAsync()
            ' Wait for cancellation to complete
            While _backgroundWorkerLoad.IsBusy
                Application.DoEvents()
            End While
        End If

        Dim currentUser = SessionManager.GetCurrentUser()
        If currentUser Is Nothing Then
            MessageBox.Show("Người dùng chưa đăng nhập. Vui lòng đăng nhập để tiếp tục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        _criteriaIn.TransactionCode = If(String.IsNullOrEmpty(_txtSearch?.Text), Nothing, _txtSearch.Text.Trim())
        _criteriaIn.Status = If(_cmbStatus?.SelectedIndex > 0 AndAlso _cmbStatus.SelectedItem IsNot Nothing, _cmbStatus.SelectedItem.ToString(), Nothing)


        _criteriaOut.TransactionCode = If(String.IsNullOrEmpty(_txtSearch?.Text), Nothing, _txtSearch.Text.Trim())
        _criteriaOut.Status = If(_cmbStatus?.SelectedIndex > 0 AndAlso _cmbStatus.SelectedItem IsNot Nothing, _cmbStatus.SelectedItem.ToString(), Nothing)

        Dim args As New LoadTransactionArgs With {
            .UserId = If(currentUser.RoleId <> 1, currentUser.UserId, Nothing),
            .CriteriaIn = _criteriaIn,
            .CriteriaOut = _criteriaOut
}

        _backgroundWorkerLoad.RunWorkerAsync(args)
    End Sub

    Private Sub _backgroundWorkerLoad_DoWork(sender As Object, e As DoWorkEventArgs) Handles _backgroundWorkerLoad.DoWork
        If _backgroundWorkerLoad.CancellationPending Then
            e.Cancel = True
            Return
        End If

        Dim args = DirectCast(e.Argument, LoadTransactionArgs)
        Dim inData = _myTransactionService.SearchTransactions("IN", args.UserId, args.CriteriaIn)
        Dim outData = _myTransactionService.SearchTransactions("OUT", args.UserId, args.CriteriaOut)
        e.Result = New TransactionData With {.InData = inData, .OutData = outData}
    End Sub

    Private Sub _backgroundWorkerLoad_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _backgroundWorkerLoad.RunWorkerCompleted
        If e.Cancelled Then Return

        If e.Error IsNot Nothing Then
            MessageBox.Show("Lỗi khi tải danh sách phiếu: " & e.Error.Message & vbCrLf & e.Error.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result = DirectCast(e.Result, TransactionData)
        _gridIn.DataSource = result.InData
        _gridOut.DataSource = result.OutData
        _lblPagingStatusIn.Text = $"Trang {_criteriaIn.PageIndex} / {Math.Ceiling(_criteriaIn.TotalCount / _criteriaIn.PageSize)}"
        _lblPagingStatusOut.Text = $"Trang {_criteriaOut.PageIndex} / {Math.Ceiling(_criteriaOut.TotalCount / _criteriaOut.PageSize)}"


    End Sub

    Private Sub StartLoadStatistics()
        If _backgroundWorkerStats.IsBusy Then
            _backgroundWorkerStats.CancelAsync()
            While _backgroundWorkerStats.IsBusy
                Application.DoEvents()
            End While
        End If

        Dim criteria As New StockTransationSearchCriterialDTO With {
            .TransactionCode = If(String.IsNullOrEmpty(_txtSearch?.Text), Nothing, _txtSearch.Text.Trim()),
            .Status = If(_cmbStatus?.SelectedIndex > 0 AndAlso _cmbStatus.SelectedItem IsNot Nothing, _cmbStatus.SelectedItem.ToString(), Nothing)
        }

        _backgroundWorkerStats.RunWorkerAsync(criteria)
    End Sub

    Private Sub _backgroundWorkerStats_DoWork(sender As Object, e As DoWorkEventArgs) Handles _backgroundWorkerStats.DoWork
        If _backgroundWorkerStats.CancellationPending Then
            e.Cancel = True
            Return
        End If

        Dim criteria = DirectCast(e.Argument, StockTransationSearchCriterialDTO)
        e.Result = _myTransactionService.GetTransactionStatistics(criteria)
    End Sub

    Private Sub _backgroundWorkerStats_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _backgroundWorkerStats.RunWorkerCompleted
        If e.Cancelled Then Return

        If e.Error IsNot Nothing Then
            MessageBox.Show("Lỗi khi tải thống kê: " & e.Error.Message & vbCrLf & e.Error.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim statsData = DirectCast(e.Result, TransactionStatisticsDTO)
        _lblTotalIn.Text = "Tổng phiếu nhập: " & statsData.TotalInTransactions.ToString()
        _lblTotalOut.Text = "Tổng phiếu xuất: " & statsData.TotalOutTransactions.ToString()
        _lblTotalValue.Text = "Tổng giá trị giao dịch: " & statsData.TotalTransactionValue.ToString("C")

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
    End Sub

    Private Sub _btnCreateIn_Click(sender As Object, e As EventArgs) Handles _btnCreateIn.Click
        Using selectSupplierForm As New SelectSupplierForm()
            If selectSupplierForm.ShowDialog() = DialogResult.OK Then
                StartLoadTransactions()
            End If
        End Using
    End Sub

    Private Sub _btnCreateOut_Click(sender As Object, e As EventArgs) Handles _btnCreateOut.Click
        Using createForm As New StockTransactionCreateForm("OUT")
            If createForm.ShowDialog() = DialogResult.OK Then
                StartLoadTransactions()
            End If
        End Using
    End Sub

    Private Sub _gridIn_SelectionChanged(sender As Object, e As EventArgs) Handles _gridIn.SelectionChanged, _gridOut.SelectionChanged
        HandleSelectionChanged(If(sender Is _gridIn, _gridIn, _gridOut))
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
            Dim transactionId As Integer = CInt(grid.SelectedRows(0).Cells(If(grid Is _gridIn, "TransactionId", "TransactionIdOut")).Value)
            Using detailForm As New StockTransactionDetailForm(transactionId)
                If detailForm.ShowDialog() = DialogResult.OK Then
                    StartLoadTransactions()
                End If
            End Using
        End If
    End Sub

    Private Sub _btnViewDetails_Click(sender As Object, e As EventArgs) Handles _btnViewDetails.Click
        Dim selectedTab = _tabControl.SelectedTab
        If selectedTab?.Text = "Phiếu nhập" Then
            ShowTransactionDetail(_gridIn)
        ElseIf selectedTab?.Text = "Phiếu xuất" Then
            ShowTransactionDetail(_gridOut)
        End If
    End Sub

    Private Sub _btnApprove_Click(sender As Object, e As EventArgs) Handles _btnApprove.Click
        Dim selectedGrid As DataGridView = If(_gridIn.SelectedRows.Count > 0, _gridIn, _gridOut)
        If selectedGrid.SelectedRows.Count > 0 Then
            Dim transactionId As Integer = CInt(selectedGrid.SelectedRows(0).Cells(If(selectedGrid Is _gridIn, "TransactionId", "TransactionIdOut")).Value)
            If MessageBox.Show("Duyệt phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim currentUser = SessionManager.GetCurrentUser()
                If currentUser Is Nothing Then
                    MessageBox.Show("Người dùng chưa đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim result = _myTransactionService.ApproveTransaction(transactionId, currentUser.UserId, True)
                If result.Success Then
                    MessageBox.Show("Duyệt phiếu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    StartLoadTransactions()
                Else
                    MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub _txtSearch_TextChanged(sender As Object, e As EventArgs) Handles _txtSearch.TextChanged, _cmbStatus.SelectedIndexChanged
        StartLoadTransactions()
        StartLoadStatistics()
    End Sub

    Private Sub _btnExportCsv_Click(sender As Object, e As EventArgs) Handles _btnExportCsv.Click
        Using sfd As New SaveFileDialog()
            sfd.Filter = "CSV Files (*.csv)|*.csv"
            sfd.FileName = "TransactionStatistics_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Using writer As New System.IO.StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                        writer.WriteLine("Tổng phiếu nhập,Tổng phiếu xuất,Tổng giá trị giao dịch")
                        writer.WriteLine($"{_lblTotalIn.Text.Replace("Tổng phiếu nhập: ", "")},{_lblTotalOut.Text.Replace("Tổng phiếu xuất: ", "")},{_lblTotalValue.Text.Replace("Tổng giá trị giao dịch: ", "")}")
                        writer.WriteLine("Tình trạng," & _lblStatusBreakdown.Text.Replace("Tình trạng: ", ""))
                        writer.WriteLine()

                        writer.WriteLine("Top sản phẩm giao dịch")
                        writer.WriteLine("Mã sản phẩm,Tên sản phẩm,Tổng số lượng,Tổng giá trị")
                        For Each row As DataGridViewRow In _gridStats.Rows
                            If row.IsNewRow Then Continue For
                            writer.WriteLine($"{row.Cells("ProductId").Value},{row.Cells("ProductName").Value},{row.Cells("TotalQuantity").Value},{row.Cells("TotalValue").Value}")
                        Next
                        writer.WriteLine()

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
        ConfigureGridColumns(_gridIn, {
            New String() {"TransactionId", "Mã phiếu", "80", "TransactionId"},
            New String() {"TransactionCode", "Mã giao dịch", "120", "TransactionCode"},
            New String() {"CreatedByName", "Người tạo", "120", "CreatedByName"},
            New String() {"CreatedAt", "Ngày tạo", "100", "CreatedAt", "dd/MM/yyyy"},
            New String() {"SupplierName", "Nhà cung cấp", "150", "SupplierName"},
            New String() {"Status", "Trạng thái", "100", "Status"},
            New String() {"ApprovedByName", "Người duyệt", "120", "ApprovedByName"},
            New String() {"ApprovedAt", "Ngày duyệt", "100", "ApprovedAtString", "dd/MM/yyyy"}
        })

        ConfigureGridColumns(_gridOut, {
            New String() {"TransactionIdOut", "Mã phiếu", "80", "TransactionId"},
            New String() {"TransactionCodeOut", "Mã giao dịch", "120", "TransactionCode"},
            New String() {"CreatedByNameOut", "Người tạo", "120", "CreatedByName"},
            New String() {"CreatedAtOut", "Ngày tạo", "100", "CreatedAt", "dd/MM/yyyy"},
            New String() {"StatusOut", "Trạng thái", "100", "Status"},
            New String() {"ApprovedByNameOut", "Người duyệt", "120", "ApprovedByName"},
            New String() {"ApprovedAtOut", "Ngày duyệt", "100", "ApprovedAtString", "dd/MM/yyyy"}
        })
    End Sub

    Private Sub ConfigureGridColumns(grid As DataGridView, columns As String()())
        For Each column In columns
            Dim col As New DataGridViewTextBoxColumn With {
                .Name = column(0),
                .HeaderText = column(1),
                .Width = CInt(column(2)),
                .DataPropertyName = column(3)
            }
            If column.Length > 4 Then
                col.DefaultCellStyle.Format = column(4)
            End If
            grid.Columns.Add(col)
        Next
    End Sub

    Private Sub StockTransactionListForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _backgroundWorkerLoad.IsBusy Then
            _backgroundWorkerLoad.CancelAsync()
            While _backgroundWorkerLoad.IsBusy
                Application.DoEvents()
            End While
        End If

        If _backgroundWorkerStats.IsBusy Then
            _backgroundWorkerStats.CancelAsync()
            While _backgroundWorkerStats.IsBusy
                Application.DoEvents()
            End While
        End If
    End Sub
    ' Phân trang: Trang trước
    Private Sub _btnPrevPageIn_Click(sender As Object, e As EventArgs) Handles _btnPrevPageIn.Click
        If _criteriaIn.PageIndex > 1 Then
            _criteriaIn.PageIndex -= 1
            StartLoadTransactions()
        End If
    End Sub

    ' Phân trang: Trang sau
    Private Sub _btnNextPageIn_Click(sender As Object, e As EventArgs) Handles _btnNextPageIn.Click
        Dim totalPage = Math.Ceiling(_criteriaIn.TotalCount / _criteriaIn.PageSize)
        If _criteriaIn.PageIndex < totalPage Then
            _criteriaIn.PageIndex += 1
            StartLoadTransactions()
        End If
    End Sub



    ' Phân trang: Trang trước
    Private Sub _btnPrevPageOut_Click(sender As Object, e As EventArgs) Handles _btnPrevPageOut.Click
        If _criteriaOut.PageIndex > 1 Then
            _criteriaOut.PageIndex -= 1
            StartLoadTransactions()
        End If
    End Sub

    ' Phân trang: Trang sau
    Private Sub _btnNextPageOut_Click(sender As Object, e As EventArgs) Handles _btnNextPageOut.Click
        Dim totalPage = Math.Ceiling(_criteriaOut.TotalCount / _criteriaOut.PageSize)
        If _criteriaOut.PageIndex < totalPage Then
            _criteriaOut.PageIndex += 1
            StartLoadTransactions()
        End If
    End Sub

    Private Class LoadTransactionArgs
        Public UserId As Integer?
        Public CriteriaIn As StockTransationSearchCriterialDTO
        Public CriteriaOut As StockTransationSearchCriterialDTO
    End Class

    Private Class TransactionData
        Public InData As Object
        Public OutData As Object
    End Class
End Class