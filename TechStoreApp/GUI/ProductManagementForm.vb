Imports System.Data.Odbc
Imports System.Threading.Tasks
Imports System.ComponentModel

Public Class ProductManagementForm
    Inherits Form

    Private ReadOnly _productService As IProductService
    Private ReadOnly _categoryService As ICategoryService
    Private ReadOnly _supplierService As ISupplierService
    Private ReadOnly _userId As Integer
    Private isEditing As Boolean = False
    Private isAdding As Boolean = False
    Private currentPage As Integer = 0
    Private pageSize As Integer = 7
    Private totalPages As Integer = 0
    Private categoryLookup As Dictionary(Of String, Integer)
    Dim supplierLookup As New Dictionary(Of Integer, String)
    Private searchCriteria As ProductSearchCriteriaDTO
    Private WithEvents _backgroundWorkerProducts As BackgroundWorker
    Private WithEvents _backgroundWorkerCategories As BackgroundWorker
    Private WithEvents _backgroundWorkerSuppliers As BackgroundWorker
    Private WithEvents _backgroundWorkerRolePermissions As BackgroundWorker

    Public Sub New(ByVal userId As Integer)
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        _productService = ServiceFactory.CreateProductService()
        _categoryService = ServiceFactory.CreateCategoryService()
        _supplierService = ServiceFactory.CreateSupplierService()
        _userId = userId
        categoryLookup = New Dictionary(Of String, Integer)
        supplierLookup = New Dictionary(Of Integer, String)
        searchCriteria = New ProductSearchCriteriaDTO With {
            .PageSize = pageSize,
            .PageIndex = currentPage
        }

    End Sub



    Private Async Sub ApplyRolePermissionsAsync()
        Try
            Dim user = SessionManager.GetCurrentUser()
            Dim role = Await ServiceFactory.CreateRoleService().GetRoleById(user.RoleId)
            Dim roleName = role.RoleName.Trim().ToLower()

            If roleName <> "admin" Then
                btnAdd.Visible = False
                btnUpdate.Visible = False
                btnDelete.Visible = False
                btnCancel.Visible = False
                txtProductName.Enabled = False
                txtDescription.Enabled = False
                txtPrice.Enabled = False
                txtQuantity.Enabled = False
                txtUnit.Enabled = False
                txtMinStockLevel.Enabled = False
                cboCategory.Enabled = False
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Không thể xác định quyền: " & ex.Message
        End Try
    End Sub



    Private Async Sub LoadCategoriesAsync()
        Try
            Dim categories = Await _categoryService.GetAllCategoriesAsync()
            cboCategory.Items.Clear()
            cboCategorySort.Items.Clear()
            categoryLookup.Clear()
            cboCategorySort.Items.Add("Tất cả")
            categoryLookup.Add("Tất cả", Nothing)
            For Each cat In categories
                cboCategory.Items.Add(cat.CategoryName)
                cboCategorySort.Items.Add(cat.CategoryName)
                categoryLookup.Add(cat.CategoryName, cat.CategoryId)
            Next
            If cboCategory.Items.Count > 0 Then
                cboCategory.SelectedIndex = 0
                cboCategorySort.SelectedIndex = 0
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tải danh mục: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi tải danh mục: " & ex.Message
        End Try
    End Sub



    Private Async Sub LoadSuppliersAsync()
        Try
            Dim suppliers = Await _supplierService.GetAllSuppliers()
            cboSupplier.Items.Clear()
            supplierLookup.Clear()
            For Each sup In suppliers
                cboSupplier.Items.Add(sup.SupplierName)
                supplierLookup.Add(sup.SupplierId, sup.SupplierName)
            Next
            If cboSupplier.Items.Count > 0 Then
                cboSupplier.SelectedIndex = 0
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tải danh mục: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi tải nhà cung cấp: " & ex.Message
        End Try
    End Sub



    Private Sub LoadSortByOptions()
        cboSortBy.Items.Clear()
        cboSortBy.Items.AddRange({"Tất cả", "Tên (A-Z)", "Giá (Thấp đến Cao)", "Giá (Cao đến Thấp)"})
        cboSortBy.SelectedIndex = 0
    End Sub

    Private Sub LoadStatusOptions()
        cboStatus.Items.Clear()
        cboStatus.Items.AddRange({"Tất cả", "Đang hoạt động", "Ngưng hoạt động"})
        cboStatus.SelectedIndex = 0
    End Sub

    Private Async Function LoadProductsAsync() As Task
        Try
            searchCriteria.PageIndex = currentPage
            Dim products = Await _productService.SearchProductsAsync(searchCriteria)
            totalPages = If(searchCriteria.PageSize > 0, CInt(Math.Ceiling(searchCriteria.TotalCount / searchCriteria.PageSize)), 1)
            dgvProducts.Rows.Clear()
            For Each p In products
                dgvProducts.Rows.Add(p.ProductId, p.ProductName, p.Description, p.Unit, p.Price, p.Quantity, p.MinStockLevel, p.CategoryName, p.SupplierName, p.CreatedByName, p.CreatedAt, If(p.IsActive, "Active", "InActive"))
            Next

            lblPage.Text = $"Trang {currentPage + 1}/{totalPages}"
            btnPrev.Enabled = (currentPage > 0)
            btnNext.Enabled = (currentPage < totalPages - 1)
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tải sản phẩm: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi tải sản phẩm: " & ex.Message
        End Try
    End Function



    Private Sub ClearInputs()
        txtProductName.Text = String.Empty
        txtDescription.Text = String.Empty
        txtPrice.Text = String.Empty
        txtQuantity.Text = String.Empty
        txtUnit.Text = String.Empty
        txtMinStockLevel.Text = String.Empty
        If cboCategory.Items.Count > 0 Then
            cboCategory.SelectedIndex = 0
        Else
            cboCategory.SelectedIndex = -1
        End If
        If cboIsActive.Items.Count > 0 Then
            cboIsActive.SelectedIndex = 0 ' Mặc định là Hoạt động
        Else
            cboIsActive.SelectedIndex = -1
        End If
        lblError.Text = String.Empty
    End Sub

    Private Function ValidateInputs() As Boolean
        ' Xóa lỗi cũ
        ErrorProvider1.Clear()
        ErrorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink
        Dim isValid As Boolean = True

        ' Tên sản phẩm
        If String.IsNullOrEmpty(txtProductName.Text) Then
            ErrorProvider1.SetError(txtProductName, "Tên sản phẩm không được để trống.")
            isValid = False
        End If

        ' Danh mục
        Dim categoryName = cboCategory.SelectedItem?.ToString()
        If String.IsNullOrEmpty(categoryName) OrElse Not categoryLookup.ContainsKey(categoryName) Then
            ErrorProvider1.SetError(cboCategory, "Vui lòng chọn danh mục hợp lệ.")
            isValid = False
        End If

        ' Giá sản phẩm
        Dim price As Decimal
        If Not Decimal.TryParse(txtPrice.Text, price) OrElse price < 0 Then
            ErrorProvider1.SetError(txtPrice, "Giá sản phẩm phải là số hợp lệ và không âm.")
            isValid = False
        End If

        ' Số lượng
        Dim quantity As Integer
        If Not Integer.TryParse(txtQuantity.Text, quantity) OrElse quantity < 0 Then
            ErrorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên hợp lệ và không âm.")
            isValid = False
        End If

        ' Mức tồn tối thiểu
        Dim minStockLevel As Integer
        If Not Integer.TryParse(txtMinStockLevel.Text, minStockLevel) OrElse minStockLevel < 0 Then
            ErrorProvider1.SetError(txtMinStockLevel, "Mức tồn tối thiểu phải là số nguyên hợp lệ và không âm.")
            isValid = False
        End If

        Return isValid
    End Function


    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ErrorProvider1.Clear()

        If Not isEditing Then
            ClearInputs()
            SetEditingMode(True, True)
            Return
        End If

        Try
            If Not ValidateInputs() Then
                Return
            End If

            Dim selectedSupplierName As String = cboSupplier.SelectedItem.ToString()
            Dim selectedSupplierId As Integer = supplierLookup.First(Function(pair) pair.Value = selectedSupplierName).Key

            Dim product As New Product With {
                .ProductName = txtProductName.Text.Trim(),
                .Description = txtDescription.Text.Trim(),
                .Unit = txtUnit.Text.Trim(),
                .Price = Decimal.Parse(txtPrice.Text),
                .Quantity = Integer.Parse(txtQuantity.Text),
                .MinStockLevel = Integer.Parse(txtMinStockLevel.Text),
                .CategoryId = categoryLookup(cboCategory.SelectedItem.ToString()),
                .SupplierId = selectedSupplierId,
                .CreatedBy = _userId,
                .IsActive = True
            }

            Dim result = Await _productService.AddProductAsync(product)
            If result.Success Then
                LoadProductsAsync()
                ClearInputs()
                SetEditingMode(False, False)
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblError.Text = "Lỗi: " & String.Join("; ", result.Errors.ToArray())
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As FormatException
            lblError.Text = "Dữ liệu nhập không đúng định dạng."
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi thêm: " & ex.Message
        End Try
    End Sub

    Private Async Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If Not isEditing Then
            If dgvProducts.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một sản phẩm để cập nhật."
                Return
            End If
            SetEditingMode(True, False)
            Return
        End If

        Try
            If Not ValidateInputs() Then
                Return
            End If

            Dim row = dgvProducts.SelectedRows(0)
            Dim productId = Convert.ToInt32(row.Cells("ProductId").Value)
            Dim isActive As Boolean = (cboIsActive.SelectedItem.ToString() = "Hoạt động")

            Dim selectedSupplierName As String = cboSupplier.SelectedItem.ToString()
            Dim selectedSupplierId As Integer = supplierLookup.First(Function(pair) pair.Value = selectedSupplierName).Key
            Dim product As New Product With {
                .ProductName = txtProductName.Text.Trim(),
                .Description = txtDescription.Text.Trim(),
                .Unit = txtUnit.Text.Trim(),
                .Price = Decimal.Parse(txtPrice.Text),
                .Quantity = Integer.Parse(txtQuantity.Text),
                .MinStockLevel = Integer.Parse(txtMinStockLevel.Text),
                .CategoryId = categoryLookup(cboCategory.SelectedItem.ToString()),
                .SupplierId = selectedSupplierId,
                .CreatedBy = _userId,
                .IsActive = isActive
            }

            product.GetType().GetField("_productId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, productId)
            Dim oldName As String = row.Cells("ProductName").Value.ToString()
            Dim oldDescription As String = row.Cells("Description").Value.ToString()
            Dim oldUnit As String = row.Cells("Unit").Value.ToString()
            Dim oldPrice As Decimal = Convert.ToDecimal(row.Cells("Price").Value)
            Dim oldQuantity As Integer = Convert.ToInt32(row.Cells("Quantity").Value)
            Dim oldMinStock As Integer = Convert.ToInt32(row.Cells("MinStockLevel").Value)
            Dim oldCategoryName As String = row.Cells("CategoryName").Value.ToString()
            Dim oldSupplierName As String = row.Cells("SupplierName").Value.ToString()
            Dim isActiveText As String = row.Cells("IsActive").Value.ToString().Trim().ToLower()

            Dim oldIsActive As Boolean = (isActiveText = "active")

            Dim oldCategoryId As Integer = categoryLookup(oldCategoryName)

            ' Ánh xạ SupplierName → Id (tìm key từ value)
            Dim oldSupplierId As Integer = supplierLookup.First(Function(pair) pair.Value = oldSupplierName).Key
            ' So sánh toàn bộ các trường
            If product.ProductName = oldName AndAlso
               product.Description = oldDescription AndAlso
               product.Unit = oldUnit AndAlso
               product.Price = oldPrice AndAlso
               product.Quantity = oldQuantity AndAlso
               product.MinStockLevel = oldMinStock AndAlso
               product.CategoryId = oldCategoryId AndAlso
               product.SupplierId = oldSupplierId AndAlso
               product.IsActive = oldIsActive Then

                MessageBox.Show("Thông tin không có thay đổi nào. Không cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If MessageBox.Show("Bạn có chắc chắn muốn cập nhật sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Return
            End If

            Dim result = Await _productService.UpdateProductAsync(product)
            If result.Success Then
                LoadProductsAsync()
                ClearInputs()
                SetEditingMode(False, False)
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblError.Text = "Lỗi: " & String.Join("; ", result.Errors.ToArray())
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As FormatException
            lblError.Text = "Dữ liệu nhập không đúng định dạng."
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi cập nhật: " & ex.Message
        End Try
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvProducts.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một sản phẩm để xóa."
                Return
            End If

            Dim productId = Convert.ToInt32(dgvProducts.SelectedRows(0).Cells("ProductId").Value)
            If MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Return
            End If

            Dim result = Await _productService.DeleteProductAsync(productId)
            If result Then
                LoadProductsAsync()
                ClearInputs()
                SetEditingMode(False, False)
                MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblError.Text = "Xóa sản phẩm thất bại."
                MessageBox.Show("Xóa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As OdbcException
            If ex.Message.ToLower().Contains("constraint") OrElse ex.Message.ToLower().Contains("foreign key") Then
                MessageBox.Show("Không thể xóa sản phẩm  vì đang được liên kết với đơn hàng hoặc dữ liệu khác.", "Thông báo")
            Else
                MessageBox.Show("Đã xảy ra lỗi kết nối cơ sở dữ liệu.", "Lỗi hệ thống")
            End If
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi xóa: " & ex.Message
        End Try
    End Sub

    Private Sub dgvProducts_Click(sender As Object, e As EventArgs) Handles dgvProducts.Click
        ErrorProvider1.Clear()

        Try
            If dgvProducts.SelectedRows.Count > 0 Then
                Dim row = dgvProducts.SelectedRows(0)
                txtProductName.Text = If(row.Cells("ProductName").Value IsNot Nothing, row.Cells("ProductName").Value.ToString(), String.Empty)
                txtDescription.Text = If(row.Cells("Description").Value IsNot Nothing, row.Cells("Description").Value.ToString(), String.Empty)
                txtUnit.Text = If(row.Cells("Unit").Value IsNot Nothing, row.Cells("Unit").Value.ToString(), String.Empty)
                txtPrice.Text = If(row.Cells("Price").Value IsNot Nothing, row.Cells("Price").Value.ToString(), String.Empty)
                txtQuantity.Text = If(row.Cells("Quantity").Value IsNot Nothing, row.Cells("Quantity").Value.ToString(), String.Empty)
                txtMinStockLevel.Text = If(row.Cells("MinStockLevel").Value IsNot Nothing, row.Cells("MinStockLevel").Value.ToString(), String.Empty)
                Dim catName = If(row.Cells("CategoryName").Value IsNot Nothing, row.Cells("CategoryName").Value.ToString(), String.Empty)
                cboCategory.SelectedIndex = If(cboCategory.Items.Contains(catName), cboCategory.Items.IndexOf(catName), -1)
                Dim supName = If(row.Cells("SupplierName").Value IsNot Nothing, row.Cells("SupplierName").Value.ToString(), String.Empty)
                cboSupplier.SelectedIndex = If(cboSupplier.Items.Contains(supName), cboSupplier.Items.IndexOf(supName), -1)
                Dim status = If(row.Cells("IsActive").Value IsNot Nothing, row.Cells("IsActive").Value.ToString(), "Active")
                cboIsActive.SelectedIndex = If(status = "Active", 0, 1)
            End If
        Catch ex As Exception
            lblError.Text = "Lỗi khi chọn sản phẩm: " & ex.Message
        End Try
    End Sub

    Private Sub chkLowStock_CheckedChanged(sender As Object, e As EventArgs) Handles chkLowStock.CheckedChanged
        Try
            searchCriteria.LowStockOnly = chkLowStock.Checked
            Debug.WriteLine($"LowStockOnly: {searchCriteria.LowStockOnly}")
            currentPage = 0
            LoadProductsAsync()
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi lọc sản phẩm tồn kho thấp: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi lọc sản phẩm tồn kho thấp: " & ex.Message
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            Dim nameValue = If(String.IsNullOrEmpty(txtSearchName.Text.Trim()), Nothing, txtSearchName.Text.Trim())
            Dim categoryIdValue = If(cboCategorySort.SelectedIndex >= 0 AndAlso cboCategorySort.SelectedItem IsNot Nothing, categoryLookup(cboCategorySort.SelectedItem.ToString()), Nothing)
            Dim isActiveValue As Boolean? = Nothing

            Select Case cboStatus.SelectedIndex
                Case 0 ' Tất cả
                    isActiveValue = Nothing
                Case 1 ' Đang hoạt động
                    isActiveValue = True
                Case 2 ' Ngưng hoạt động
                    isActiveValue = False
            End Select

            Dim sortByValue As String = Nothing ' Default to no sorting
            If cboSortBy IsNot Nothing AndAlso cboSortBy.SelectedIndex >= 0 AndAlso cboSortBy.SelectedItem IsNot Nothing Then
                Select Case cboSortBy.SelectedItem.ToString()
                    Case "Tất cả"
                        sortByValue = Nothing ' No sorting
                    Case "Tên (A-Z)"
                        sortByValue = "Name"
                    Case "Giá (Thấp đến Cao)"
                        sortByValue = "PriceAsc"
                    Case "Giá (Cao đến Thấp)"
                        sortByValue = "PriceDesc"
                End Select
            End If

            searchCriteria = New ProductSearchCriteriaDTO With {
                .Name = nameValue,
                .CategoryId = categoryIdValue,
                .IsActive = isActiveValue,
                .PageIndex = 0,
                .PageSize = pageSize,
                .SortBy = sortByValue,
                .LowStockOnly = If(chkLowStock IsNot Nothing, chkLowStock.Checked, False)
            }

            currentPage = 0
            LoadProductsAsync()
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tìm kiếm sản phẩm: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi tìm kiếm: " & ex.Message
        End Try
    End Sub

    Private Sub btnClearSearch_Click(sender As Object, e As EventArgs) Handles btnClearSearch.Click
        Try
            txtSearchName.Text = String.Empty
            cboStatus.SelectedIndex = 0
            cboSortBy.SelectedIndex = 0
            If cboCategorySort.Items.Count > 0 Then
                cboCategorySort.SelectedIndex = 0
            Else
                cboCategorySort.SelectedIndex = -1
            End If
            chkLowStock.Checked = False
            searchCriteria = New ProductSearchCriteriaDTO With {
                .PageSize = pageSize,
                .PageIndex = 0
            }

            currentPage = 0
            LoadProductsAsync()
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi xóa tìm kiếm: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi xóa tìm kiếm: " & ex.Message
        End Try
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If currentPage > 0 Then
            currentPage -= 1
            LoadProductsAsync()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentPage < totalPages - 1 Then
            currentPage += 1
            LoadProductsAsync()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ErrorProvider1.Clear()

        If isAdding Then
            ClearInputs()
        ElseIf dgvProducts.SelectedRows.Count > 0 Then
            dgvProducts_Click(Nothing, Nothing)
        End If
        SetEditingMode(False, False)
    End Sub

    Private Sub btnViewStats_Click(sender As Object, e As EventArgs) Handles btnViewStats.Click
        Try
            Dim statsForm As New ProductStatisticsForm()
            statsForm.ShowDialog()
        Catch ex As Exception
            lblError.Text = "Lỗi khi mở thống kê: " & ex.Message
        End Try
    End Sub

    Private Async Sub ProductManagementForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ApplyRolePermissionsAsync()
        LoadCategoriesAsync()
        LoadSuppliersAsync()
        Await LoadProductsAsync()
        LoadSortByOptions()
        LoadStatusOptions()
    End Sub

    Private Sub ProductManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtProductName.Enabled = False
        txtDescription.Enabled = False
        txtUnit.Enabled = False
        txtPrice.Enabled = False
        txtQuantity.Enabled = False
        txtMinStockLevel.Enabled = False
        cboCategory.Enabled = False
        cboIsActive.Enabled = False
        cboSupplier.Enabled = False

    End Sub

    Private Sub SetEditingMode(editing As Boolean, adding As Boolean)
        isEditing = editing
        isAdding = adding

        txtProductName.Enabled = editing
        txtDescription.Enabled = editing
        txtUnit.Enabled = editing
        txtPrice.Enabled = editing
        txtQuantity.Enabled = editing
        txtMinStockLevel.Enabled = editing
        cboCategory.Enabled = editing
        cboIsActive.Enabled = editing
        cboSupplier.Enabled = editing

        btnAdd.Text = If(adding, "Lưu", "Thêm")
        btnUpdate.Text = If(Not adding AndAlso editing, "Lưu", "Cập nhật")
        btnAdd.Enabled = Not editing OrElse adding
        btnUpdate.Enabled = Not editing OrElse Not adding
        btnDelete.Enabled = Not editing
        btnPrev.Enabled = Not editing
        btnNext.Enabled = Not editing
        btnCancel.Enabled = editing
        btnSearch.Enabled = Not editing
        btnClearSearch.Enabled = Not editing
        dgvProducts.Enabled = Not editing

        btnCancel.Visible = editing
    End Sub

    Private Sub dgvProducts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProducts.CellContentClick
    End Sub

    Private Sub ProductManagementForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing





    End Sub
End Class