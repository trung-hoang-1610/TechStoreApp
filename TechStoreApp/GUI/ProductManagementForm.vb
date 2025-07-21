Imports System.Data.Odbc
Imports System.Collections.Generic

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
    Private supplierLookup As Dictionary(Of String, Integer)
    Private searchCriteria As ProductSearchCriteriaDTO

    Public Sub New(ByVal userId As Integer)
        InitializeComponent()
        _productService = ServiceFactory.CreateProductService()
        _categoryService = ServiceFactory.CreateCategoryService()
        _supplierService = ServiceFactory.CreateSupplierService()
        _userId = userId
        categoryLookup = New Dictionary(Of String, Integer)
        supplierLookup = New Dictionary(Of String, Integer)
        searchCriteria = New ProductSearchCriteriaDTO With {
            .PageSize = pageSize,
            .PageIndex = currentPage
        }
        ApplyRolePermissions()
        LoadCategories()
        LoadSuppliers()
        LoadSortByOptions()
        LoadStatusOptions()
        LoadProducts()
    End Sub

    Private Sub ApplyRolePermissions()
        Try
            Dim user = SessionManager.GetCurrentUser()
            Dim role = ServiceFactory.CreateRoleService().GetRoleById(user.RoleId)
            Dim roleName = role.RoleName.Trim().ToLower()

            If roleName = "user" Then
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

    Private Sub LoadCategories()
        Try
            cboCategory.Items.Clear()
            cboCategorySort.Items.Clear()
            categoryLookup.Clear()
            Dim categories = _categoryService.GetAllCategories()
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



    Private Sub LoadSuppliers()
        Try
            cboSupplier.Items.Clear()
            supplierLookup.Clear()
            Dim suppliers = _supplierService.GetAllSuppliers()
            For Each sup In suppliers
                cboSupplier.Items.Add(sup.SupplierName)
                supplierLookup.Add(sup.SupplierName, sup.SupplierId)
            Next
            If cboSupplier.Items.Count > 0 Then
                cboSupplier.SelectedIndex = 0
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tải danh mục: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi tải danh mục: " & ex.Message
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

    Private Sub LoadProducts()
        Try
            searchCriteria.PageIndex = currentPage
            Dim products = _productService.SearchProducts(searchCriteria)
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
    End Sub

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
        Dim errors As New List(Of String)

        If String.IsNullOrEmpty(txtProductName.Text) Then
            errors.Add("Tên sản phẩm không được để trống.")
        End If

        Dim categoryName = cboCategory.SelectedItem?.ToString()
        If String.IsNullOrEmpty(categoryName) OrElse Not categoryLookup.ContainsKey(categoryName) Then
            errors.Add("Vui lòng chọn danh mục hợp lệ.")
        End If

        Dim price As Decimal
        If Not Decimal.TryParse(txtPrice.Text, price) OrElse price < 0 Then
            errors.Add("Giá sản phẩm phải là số hợp lệ và không âm.")
        End If

        Dim quantity As Integer
        If Not Integer.TryParse(txtQuantity.Text, quantity) OrElse quantity < 0 Then
            errors.Add("Số lượng phải là số nguyên hợp lệ và không âm.")
        End If

        Dim minStockLevel As Integer
        If Not Integer.TryParse(txtMinStockLevel.Text, minStockLevel) OrElse minStockLevel < 0 Then
            errors.Add("Mức tồn tối thiểu phải là số nguyên hợp lệ và không âm.")
        End If

        If errors.Count > 0 Then
            MessageBox.Show(String.Join(Environment.NewLine, errors.ToArray()), "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not isEditing Then
            ClearInputs()
            SetEditingMode(True, True)
            Return
        End If

        Try
            If Not ValidateInputs() Then
                Return
            End If

            Dim product As New Product With {
                .ProductName = txtProductName.Text.Trim(),
                .Description = txtDescription.Text.Trim(),
                .Unit = txtUnit.Text.Trim(),
                .Price = Decimal.Parse(txtPrice.Text),
                .Quantity = Integer.Parse(txtQuantity.Text),
                .MinStockLevel = Integer.Parse(txtMinStockLevel.Text),
                .CategoryId = categoryLookup(cboCategory.SelectedItem.ToString()),
                .SupplierId = supplierLookup(cboSupplier.SelectedItem.ToString()),
                .CreatedBy = _userId,
                .IsActive = True
            }

            Dim result = _productService.AddProduct(product)
            If result.Success Then
                LoadProducts()
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

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
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

            Dim product As New Product With {
                .ProductName = txtProductName.Text.Trim(),
                .Description = txtDescription.Text.Trim(),
                .Unit = txtUnit.Text.Trim(),
                .Price = Decimal.Parse(txtPrice.Text),
                .Quantity = Integer.Parse(txtQuantity.Text),
                .MinStockLevel = Integer.Parse(txtMinStockLevel.Text),
                .CategoryId = categoryLookup(cboCategory.SelectedItem.ToString()),
                .SupplierId = supplierLookup(cboSupplier.SelectedItem.ToString()),
                .CreatedBy = _userId,
                .IsActive = isActive
            }

            product.GetType().GetField("_productId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, productId)

            If MessageBox.Show("Bạn có chắc chắn muốn cập nhật sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Return
            End If

            Dim result = _productService.UpdateProduct(product)
            If result.Success Then
                LoadProducts()
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

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvProducts.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một sản phẩm để xóa."
                Return
            End If

            Dim productId = Convert.ToInt32(dgvProducts.SelectedRows(0).Cells("ProductId").Value)
            If MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Return
            End If

            Dim result = _productService.DeleteProduct(productId)
            If result Then
                LoadProducts()
                ClearInputs()
                SetEditingMode(False, False)
                MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                lblError.Text = "Xóa sản phẩm thất bại."
                MessageBox.Show("Xóa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi xóa: " & ex.Message
        End Try
    End Sub

    Private Sub dgvProducts_Click(sender As Object, e As EventArgs) Handles dgvProducts.Click
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
            ' Cập nhật searchCriteria.LowStockOnly dựa trên trạng thái checkbox
            searchCriteria.LowStockOnly = chkLowStock.Checked
            Debug.WriteLine($"LowStockOnly: {searchCriteria.LowStockOnly}")

            ' Đặt lại trang về 0 để đảm bảo hiển thị từ đầu
            currentPage = 0
            LoadProducts()
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
            LoadProducts()
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
            LoadProducts()
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi xóa tìm kiếm: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi hệ thống khi xóa tìm kiếm: " & ex.Message
        End Try
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If currentPage > 0 Then
            currentPage -= 1
            LoadProducts()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentPage < totalPages - 1 Then
            currentPage += 1
            LoadProducts()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If isAdding Then
            ClearInputs()
        ElseIf dgvProducts.SelectedRows.Count > 0 Then
            dgvProducts_Click(Nothing, Nothing)
        End If
        SetEditingMode(False, False)
    End Sub

    Private Sub btnViewStats_Click(sender As Object, e As EventArgs) Handles btnViewStats.Click
        Try
            Dim statsForm As New StatisticsForm()
            statsForm.ShowDialog()
        Catch ex As Exception
            lblError.Text = "Lỗi khi mở thống kê: " & ex.Message
        End Try
    End Sub
    Private Sub ProductManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Optional: Initialize additional controls here if needed
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
End Class