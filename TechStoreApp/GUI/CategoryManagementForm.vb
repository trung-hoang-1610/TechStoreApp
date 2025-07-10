' GUI/CategoryManagementForm.vb
Public Class CategoryManagementForm
    Inherits Form

    Private ReadOnly _categoryService As ICategoryService

    ' Các điều khiển giao diện
    Private WithEvents dgvCategories As DataGridView
    Private WithEvents btnAdd As Button
    Private WithEvents btnUpdate As Button
    Private WithEvents btnDelete As Button
    Private txtCategoryName As TextBox
    Private txtDescription As TextBox
    Private lblError As Label

    ''' <summary>
    ''' Khởi tạo CategoryManagementForm
    ''' </summary>
    Public Sub New()
        _categoryService = ServiceFactory.CreateCategoryService()
        InitializeComponents()
        LoadCategories()
    End Sub

    ''' <summary>
    ''' Khởi tạo các điều khiển giao diện
    ''' </summary>
    Private Sub InitializeComponents()
        Me.Text = "Quản Lý Danh Mục"
        Me.Size = New Size(500, 300)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        ' DataGridView
        dgvCategories = New DataGridView With {.Location = New Point(20, 20), .Size = New Size(450, 150), .ReadOnly = True}
        dgvCategories.Columns.Add("CategoryId", "ID")
        dgvCategories.Columns.Add("CategoryName", "Tên danh mục")
        dgvCategories.Columns.Add("Description", "Mô tả")

        ' Nhập liệu
        Dim lblCategoryName As New Label With {.Text = "Tên danh mục:", .Location = New Point(20, 180), .Size = New Size(100, 20)}
        txtCategoryName = New TextBox With {.Location = New Point(120, 180), .Size = New Size(200, 20)}

        Dim lblDescription As New Label With {.Text = "Mô tả:", .Location = New Point(20, 210), .Size = New Size(100, 20)}
        txtDescription = New TextBox With {.Location = New Point(120, 210), .Size = New Size(200, 20)}

        ' Label lỗi
        lblError = New Label With {.Location = New Point(20, 240), .Size = New Size(450, 20), .ForeColor = Color.Red}

        ' Buttons
        btnAdd = New Button With {.Text = "Thêm", .Location = New Point(350, 180), .Size = New Size(80, 30)}
        btnUpdate = New Button With {.Text = "Cập nhật", .Location = New Point(350, 220), .Size = New Size(80, 30)}
        btnDelete = New Button With {.Text = "Xóa", .Location = New Point(350, 260), .Size = New Size(80, 30)}

        Me.Controls.AddRange({dgvCategories, lblCategoryName, txtCategoryName, lblDescription, txtDescription, lblError, btnAdd, btnUpdate, btnDelete})
    End Sub

    ''' <summary>
    ''' Tải danh sách danh mục vào DataGridView
    ''' </summary>
    Private Sub LoadCategories()
        Try
            dgvCategories.Rows.Clear()
            Dim categories = _categoryService.GetAllCategories()
            For Each category In categories
                dgvCategories.Rows.Add(category.CategoryId, category.CategoryName, category.Description)
            Next
        Catch ex As Exception
            lblError.Text = "Lỗi khi tải danh mục: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Thêm
    ''' </summary>
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Try
            lblError.Text = String.Empty
            Dim category As New Category With {
                .CategoryName = txtCategoryName.Text,
                .Description = txtDescription.Text
            }

            Dim result = _categoryService.AddCategory(category)
            If result.Success Then
                LoadCategories()
                ClearInputs()
            Else
                lblError.Text = String.Join("; ", result.Errors.ToArray())
            End If
        Catch ex As Exception
            lblError.Text = "Lỗi khi thêm danh mục: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Cập nhật
    ''' </summary>
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try
            lblError.Text = String.Empty
            If dgvCategories.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một danh mục để cập nhật."
                Return
            End If

            Dim selectedRow = dgvCategories.SelectedRows(0)
            Dim category As New Category With {
                .CategoryName = txtCategoryName.Text,
                .Description = txtDescription.Text
            }
            category.GetType().GetField("_categoryId", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(category, selectedRow.Cells("CategoryId").Value)

            Dim result = _categoryService.UpdateCategory(category)
            If result.Success Then
                LoadCategories()
                ClearInputs()
            Else
                lblError.Text = String.Join("; ", result.Errors.ToArray())
            End If
        Catch ex As Exception
            lblError.Text = "Lỗi khi cập nhật danh mục: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện nhấn nút Xóa
    ''' </summary>
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Try
            lblError.Text = String.Empty
            If dgvCategories.SelectedRows.Count = 0 Then
                lblError.Text = "Vui lòng chọn một danh mục để xóa."
                Return
            End If

            Dim categoryId = CInt(dgvCategories.SelectedRows(0).Cells("CategoryId").Value)
            Dim success = _categoryService.DeleteCategory(categoryId)
            If success Then
                LoadCategories()
                ClearInputs()
            Else
                lblError.Text = "Không thể xóa danh mục."
            End If
        Catch ex As Exception
            lblError.Text = "Lỗi khi xóa danh mục: " & ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Xóa các trường nhập liệu
    ''' </summary>
    Private Sub ClearInputs()
        txtCategoryName.Text = String.Empty
        txtDescription.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện chọn dòng trong DataGridView
    ''' </summary>
    Private Sub dgvCategories_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvCategories.SelectionChanged
        If dgvCategories.SelectedRows.Count > 0 Then
            Dim category = _categoryService.GetCategoryById(CInt(dgvCategories.SelectedRows(0).Cells("CategoryId").Value))
            If category IsNot Nothing Then
                txtCategoryName.Text = category.CategoryName
                txtDescription.Text = category.Description
            End If
        End If
    End Sub
End Class