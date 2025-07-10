' GUI/ProductManagementForm.vb
Imports System.Data.Odbc

Public Class ProductManagementForm
    Inherits Form

    Private ReadOnly _productService As IProductService
    Private ReadOnly _categoryService As ICategoryService
    Private ReadOnly _userId As Integer

    Private currentPage As Integer = 0
    Private pageSize As Integer = 3
    Private totalPages As Integer = 0
    Private categoryLookup As Dictionary(Of String, Integer)

    ''' <summary>
    ''' Khởi tạo ProductManagementForm với ID người dùng
    ''' </summary>
    ''' <param name="userId">Mã định danh của người dùng</param>
    Public Sub New(ByVal userId As Integer)
        InitializeComponent()
        _productService = ServiceFactory.CreateProductService()
        _categoryService = ServiceFactory.CreateCategoryService()
        _userId = userId
        categoryLookup = New Dictionary(Of String, Integer)
        ApplyRolePermissions()
        LoadCategories()
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

                txtProductName.Enabled = False
                txtDescription.Enabled = False
                txtPrice.Enabled = False
                txtQuantity.Enabled = False
                cboCategory.Enabled = False
            End If
        Catch ex As Exception
            lblError.Text = "Không thể xác định quyền: " & ex.Message
        End Try
    End Sub


    ''' <summary>
    ''' Tải danh sách danh mục vào ComboBox
    ''' </summary>
    Private Sub LoadCategories()
        Try
            cboCategory.Items.Clear()
            categoryLookup.Clear()
            Dim categories = _categoryService.GetAllCategories()
            For Each cat In categories
                cboCategory.Items.Add(cat.CategoryName)
                categoryLookup.Add(cat.CategoryName, cat.CategoryId)
            Next
            If cboCategory.Items.Count > 0 Then
                cboCategory.SelectedIndex = 0
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tải danh mục: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Tải danh sách sản phẩm vào DataGridView
    ''' </summary>
    Private Sub LoadProducts()
        Try
            Dim totalCount = _productService.GetTotalProductCount()
            totalPages = If(pageSize > 0, CInt(Math.Ceiling(totalCount / pageSize)), 1)

            Dim products = _productService.GetProductsByPage(currentPage, pageSize)
            dgvProducts.Rows.Clear()
            For Each p In products
                dgvProducts.Rows.Add(p.ProductId, p.ProductName, p.Description, p.Price, p.Quantity, p.CategoryName)
            Next

            lblPage.Text = $"Trang {currentPage + 1}/{totalPages}"
            btnPrev.Enabled = (currentPage > 0)
            btnNext.Enabled = (currentPage < totalPages - 1)
        Catch ex As OdbcException
            lblError.Text = "Lỗi khi tải sản phẩm: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xóa các trường nhập liệu
    ''' </summary>
    Private Sub ClearInputs()
        txtProductName.Text = String.Empty
        txtDescription.Text = String.Empty
        txtPrice.Text = String.Empty
        txtQuantity.Text = String.Empty
        If cboCategory.Items.Count > 0 Then
            cboCategory.SelectedIndex = 0
        Else
            cboCategory.SelectedIndex = -1
        End If
        lblError.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Thêm
    ''' </summary>
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            lblError.Text = String.Empty
            Dim categoryName = cboCategory.SelectedItem?.ToString()
            If String.IsNullOrEmpty(categoryName) OrElse Not categoryLookup.ContainsKey(categoryName) Then
                lblError.Text = "Vui lòng chọn danh mục hợp lệ."
                Return
            End If

            Dim price As Decimal
            If Not Decimal.TryParse(txtPrice.Text, price) Then
                lblError.Text = "Giá sản phẩm phải là số hợp lệ."
                Return
            End If

            Dim quantity As Integer
            If Not Integer.TryParse(txtQuantity.Text, quantity) Then
                lblError.Text = "Số lượng phải là số nguyên hợp lệ."
                Return
            End If

            Dim product As New Product With {
                .ProductName = txtProductName.Text.Trim(),
                .Description = txtDescription.Text.Trim(),
                .Price = price,
                .Quantity = quantity,
                .CategoryId = categoryLookup(categoryName),
                .CreatedBy = _userId
            }

            Dim result = _productService.AddProduct(product)
            If result.Success Then
                LoadProducts()
                ClearInputs()
            Else
                lblError.Text = "Lỗi: " & String.Join("; ", result.Errors.ToArray())
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As FormatException
            lblError.Text = "Dữ liệu nhập không đúng định dạng."
        Catch ex As Exception
            lblError.Text = "Lỗi khi thêm: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Cập nhật
    ''' </summary>
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            lblError.Text = String.Empty
            If dgvProducts.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một sản phẩm để cập nhật."
                Return
            End If

            Dim row = dgvProducts.SelectedRows(0)
            Dim productId = Convert.ToInt32(row.Cells("ProductId").Value)
            Dim categoryName = cboCategory.SelectedItem?.ToString()
            If String.IsNullOrEmpty(categoryName) OrElse Not categoryLookup.ContainsKey(categoryName) Then
                lblError.Text = "Vui lòng chọn danh mục hợp lệ."
                Return
            End If

            Dim price As Decimal
            If Not Decimal.TryParse(txtPrice.Text, price) Then
                lblError.Text = "Giá sản phẩm phải là số hợp lệ."
                Return
            End If

            Dim quantity As Integer
            If Not Integer.TryParse(txtQuantity.Text, quantity) Then
                lblError.Text = "Số lượng phải là số nguyên hợp lệ."
                Return
            End If

            Dim product As New Product With {
                .ProductName = txtProductName.Text.Trim(),
                .Description = txtDescription.Text.Trim(),
                .Price = price,
                .Quantity = quantity,
                .CategoryId = categoryLookup(categoryName),
                .CreatedBy = _userId
            }
            product.GetType().GetField("_productId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(product, productId)

            Dim result = _productService.UpdateProduct(product)
            If result.Success Then
                LoadProducts()
                ClearInputs()
            Else
                lblError.Text = "Lỗi: " & String.Join("; ", result.Errors.ToArray())
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As FormatException
            lblError.Text = "Dữ liệu nhập không đúng định dạng."
        Catch ex As Exception
            lblError.Text = "Lỗi khi cập nhật: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Xóa
    ''' </summary>
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            lblError.Text = String.Empty
            If dgvProducts.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một sản phẩm để xóa."
                Return
            End If

            Dim productId = Convert.ToInt32(dgvProducts.SelectedRows(0).Cells("ProductId").Value)
            If _productService.DeleteProduct(productId) Then
                LoadProducts()
                ClearInputs()
            Else
                lblError.Text = "Xóa sản phẩm thất bại."
            End If
        Catch ex As OdbcException
            lblError.Text = "Lỗi cơ sở dữ liệu: " & ex.Message
        Catch ex As Exception
            lblError.Text = "Lỗi khi xóa: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện chọn dòng trong DataGridView
    ''' </summary>
    Private Sub dgvProducts_Click(sender As Object, e As EventArgs) Handles dgvProducts.Click
        Try
            If dgvProducts.SelectedRows.Count > 0 Then
                Dim row = dgvProducts.SelectedRows(0)
                txtProductName.Text = If(row.Cells("ProductName").Value?.ToString(), String.Empty)
                txtDescription.Text = If(row.Cells("Description").Value?.ToString(), String.Empty)
                txtPrice.Text = If(row.Cells("Price").Value?.ToString(), String.Empty)
                txtQuantity.Text = If(row.Cells("Quantity").Value?.ToString(), String.Empty)
                Dim catName = If(row.Cells("CategoryName").Value?.ToString(), String.Empty)
                cboCategory.SelectedIndex = If(cboCategory.Items.Contains(catName), cboCategory.Items.IndexOf(catName), -1)
            End If
        Catch ex As Exception
            lblError.Text = "Lỗi khi chọn sản phẩm: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Trang trước
    ''' </summary>
    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If currentPage > 0 Then
            currentPage -= 1
            LoadProducts()
        End If
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Trang sau
    ''' </summary>
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentPage < totalPages - 1 Then
            currentPage += 1
            LoadProducts()
        End If
    End Sub

    Private Sub ProductManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class