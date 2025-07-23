Imports System.ComponentModel
Imports ClosedXML.Excel

Public Class StockTransactionListForm
    Inherits System.Windows.Forms.Form

    Private ReadOnly _myTransactionService As IStockTransactionService


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

    Private Sub _btnExportExcel_Click(sender As Object, e As EventArgs) Handles _btnExportCsv.Click
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx"
            sfd.FileName = "TransactionStatistics_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".xlsx"

            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim wb As New ClosedXML.Excel.XLWorkbook()

                    ' ====== Tổng quan ======
                    Dim wsSummary = wb.Worksheets.Add("Tổng Quan")
                    wsSummary.Cell("A1").Value = "Báo cáo giao dịch"
                    wsSummary.Range("A1:B1").Merge().Style.Font.Bold = True
                    wsSummary.Cell("A2").Value = "Tổng phiếu nhập"
                    wsSummary.Cell("B2").Value = _lblTotalIn.Text.Replace("Tổng phiếu nhập: ", "")
                    wsSummary.Cell("A3").Value = "Tổng phiếu xuất"
                    wsSummary.Cell("B3").Value = _lblTotalOut.Text.Replace("Tổng phiếu xuất: ", "")
                    wsSummary.Cell("A4").Value = "Tổng giá trị giao dịch"
                    wsSummary.Cell("B4").Value = _lblTotalValue.Text.Replace("Tổng giá trị giao dịch: ", "")
                    wsSummary.Cell("A5").Value = "Tình trạng"
                    wsSummary.Cell("B5").Value = _lblStatusBreakdown.Text.Replace("Tình trạng: ", "")
                    wsSummary.Columns().AdjustToContents()

                    ' ====== Bảng Top sản phẩm ======
                    Dim wsTop = wb.Worksheets.Add("Top sản phẩm")
                    wsTop.Cell("A1").Value = "Mã sản phẩm"
                    wsTop.Cell("B1").Value = "Tên sản phẩm"
                    wsTop.Cell("C1").Value = "Tổng số lượng"
                    wsTop.Cell("D1").Value = "Tổng giá trị"
                    wsTop.Range("A1:D1").Style.Font.Bold = True
                    wsTop.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.LightGray

                    Dim topRow = 2
                    For Each row As DataGridViewRow In _gridStats.Rows
                        If row.IsNewRow Then Continue For
                        wsTop.Cell(topRow, 1).Value = row.Cells("ProductId").Value
                        wsTop.Cell(topRow, 2).Value = row.Cells("ProductName").Value
                        wsTop.Cell(topRow, 3).Value = row.Cells("TotalQuantity").Value
                        wsTop.Cell(topRow, 4).Value = row.Cells("TotalValue").Value
                        topRow += 1
                    Next
                    wsTop.Range("A1:D" & topRow - 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    wsTop.Range("A1:D" & topRow - 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    wsTop.Columns().AdjustToContents()

                    ' ====== Bảng Sản phẩm tồn kho thấp ======
                    Dim wsLow = wb.Worksheets.Add("Tồn kho thấp")
                    wsLow.Cell("A1").Value = "Mã sản phẩm"
                    wsLow.Cell("B1").Value = "Tên sản phẩm"
                    wsLow.Cell("C1").Value = "Tồn kho hiện tại"
                    wsLow.Cell("D1").Value = "Tồn kho tối thiểu"
                    wsLow.Range("A1:D1").Style.Font.Bold = True
                    wsLow.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.LightGray

                    Dim lowRow = 2
                    For Each row As DataGridViewRow In _gridLowStock.Rows
                        If row.IsNewRow Then Continue For
                        wsLow.Cell(lowRow, 1).Value = row.Cells("ProductId").Value
                        wsLow.Cell(lowRow, 2).Value = row.Cells("ProductName").Value
                        wsLow.Cell(lowRow, 3).Value = row.Cells("CurrentStock").Value
                        wsLow.Cell(lowRow, 4).Value = row.Cells("MinimumStock").Value
                        lowRow += 1
                    Next
                    wsLow.Range("A1:D" & lowRow - 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    wsLow.Range("A1:D" & lowRow - 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    wsLow.Columns().AdjustToContents()

                    ' ====== Lưu file Excel ======
                    wb.SaveAs(sfd.FileName)
                    MessageBox.Show("Xuất Excel thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    MessageBox.Show("Lỗi khi xuất Excel: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub _gridLowStock_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles _gridLowStock.CellContentClick

    End Sub

    Private Sub _gridStats_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles _gridStats.CellContentClick

    End Sub
End Class