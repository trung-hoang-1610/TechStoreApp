Imports System.Data.Odbc
Imports System.IO
Imports ClosedXML.Excel

Public Class StatisticsForm
    Inherits Form

    Private ReadOnly _productService As IProductService

    Public Sub New()
        InitializeComponent()
        _productService = ServiceFactory.CreateProductService()
        cboTimeRange.SelectedIndex = 0
        LoadStatistics()
    End Sub

    Private Sub LoadStatistics()
        Try
            Dim stats = _productService.GetProductStatistics(cboTimeRange.SelectedItem?.ToString())
            lblTotalProducts.Text = $"Tổng số sản phẩm: {stats.TotalProducts}"
            lblActiveProducts.Text = $"Sản phẩm hoạt động: {stats.ActiveProducts}"
            lblInactiveProducts.Text = $"Sản phẩm ngưng hoạt động: {stats.InactiveProducts}"
            lblLowStockProducts.Text = $"Sản phẩm dưới mức tồn: {stats.LowStockProducts}"
            lblInventoryValue.Text = $"Tổng giá trị tồn kho: {stats.InventoryValue:C}"

            ' Cập nhật DataGridView
            dgvStats.Rows.Clear()
            For Each cat In stats.ProductsByCategory
                dgvStats.Rows.Add(cat.Key, cat.Value)
            Next
        Catch ex As OdbcException
            MessageBox.Show("Lỗi cơ sở dữ liệu: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Lỗi hệ thống: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRefreshStats_Click(sender As Object, e As EventArgs) Handles btnRefreshStats.Click
        LoadStatistics()
    End Sub

    Private Sub btnExportCsv_Click(sender As Object, e As EventArgs) Handles btnExportCsv.Click
        Try
            Dim stats = _productService.GetProductStatistics(cboTimeRange.SelectedItem?.ToString())
            Using saveFileDialog As New SaveFileDialog()
                saveFileDialog.Filter = "Excel Files|*.xlsx"
                saveFileDialog.Title = "Chọn nơi lưu file Excel"
                saveFileDialog.FileName = $"ProductStatistics_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    ExportToExcel(stats, saveFileDialog.FileName)
                    MessageBox.Show($"Xuất file Excel thành công tại: {saveFileDialog.FileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Lỗi khi xuất Excel: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportToExcel(stats As ProductStatistics, filePath As String)
        Using workbook As New XLWorkbook()
            ' Sheet 1: Tổng quan thống kê
            Dim wsOverview = workbook.Worksheets.Add("Tổng quan")
            ' Tiêu đề chung
            wsOverview.Cell(1, 1).Value = "Thống kê sản phẩm"
            wsOverview.Range(1, 1, 1, 2).Merge()
            wsOverview.Cell(1, 1).Style.Font.Bold = True
            wsOverview.Cell(1, 1).Style.Font.FontSize = 16
            wsOverview.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            wsOverview.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 204)
            wsOverview.Cell(1, 1).Style.Font.FontColor = XLColor.White

            ' Tạo bảng
            wsOverview.Cell(3, 1).Value = "Chỉ tiêu"
            wsOverview.Cell(3, 2).Value = "Giá trị"
            wsOverview.Cell(4, 1).Value = "Tổng số sản phẩm"
            wsOverview.Cell(4, 2).Value = stats.TotalProducts
            wsOverview.Cell(5, 1).Value = "Sản phẩm hoạt động"
            wsOverview.Cell(5, 2).Value = stats.ActiveProducts
            wsOverview.Cell(6, 1).Value = "Sản phẩm ngưng hoạt động"
            wsOverview.Cell(6, 2).Value = stats.InactiveProducts
            wsOverview.Cell(7, 1).Value = "Sản phẩm dưới mức tồn"
            wsOverview.Cell(7, 2).Value = stats.LowStockProducts
            wsOverview.Cell(8, 1).Value = "Tổng giá trị tồn kho"
            wsOverview.Cell(8, 2).Value = stats.InventoryValue.ToString("C")

            ' Định dạng bảng
            Dim tableRangeOverview = wsOverview.Range(3, 1, 8, 2)
            tableRangeOverview.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
            tableRangeOverview.Style.Border.InsideBorder = XLBorderStyleValues.Thin
            wsOverview.Range(3, 1, 3, 2).Style.Font.Bold = True
            wsOverview.Range(4, 1, 8, 1).Style.Font.Bold = True
            wsOverview.Range(4, 2, 8, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
            For i As Integer = 4 To 8
                If i Mod 2 = 0 Then
                    wsOverview.Range(i, 1, i, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(240, 240, 240)
                End If
            Next
            wsOverview.Columns().AdjustToContents()

            ' Sheet 2: Thống kê theo danh mục
            Dim wsCategories = workbook.Worksheets.Add("Theo danh mục")
            wsCategories.Cell(1, 1).Value = "Sản phẩm theo danh mục"
            wsCategories.Range(1, 1, 1, 2).Merge()
            wsCategories.Cell(1, 1).Style.Font.Bold = True
            wsCategories.Cell(1, 1).Style.Font.FontSize = 16
            wsCategories.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            wsCategories.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 102, 204)
            wsCategories.Cell(1, 1).Style.Font.FontColor = XLColor.White

            ' Tạo bảng
            wsCategories.Cell(3, 1).Value = "Tên danh mục"
            wsCategories.Cell(3, 2).Value = "Số lượng sản phẩm"
            Dim row = 4
            For Each cat In stats.ProductsByCategory
                wsCategories.Cell(row, 1).Value = cat.Key
                wsCategories.Cell(row, 2).Value = cat.Value
                row += 1
            Next

            ' Định dạng bảng
            Dim tableRangeCategories = wsCategories.Range(3, 1, row - 1, 2)
            tableRangeCategories.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
            tableRangeCategories.Style.Border.InsideBorder = XLBorderStyleValues.Thin
            wsCategories.Range(3, 1, 3, 2).Style.Font.Bold = True
            wsCategories.Range(4, 1, row - 1, 1).Style.Font.Bold = True
            wsCategories.Range(4, 2, row - 1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
            For i As Integer = 4 To row - 1
                If i Mod 2 = 0 Then
                    wsCategories.Range(i, 1, i, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(240, 240, 240)
                End If
            Next
            wsCategories.Columns().AdjustToContents()

            ' Lưu file
            workbook.SaveAs(filePath)
        End Using
    End Sub
End Class