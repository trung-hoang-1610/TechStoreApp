' GUI/MainForm.vb
Imports System.Windows.Forms
Imports Common
Imports Model

Public Class MainForm
    Inherits Form

    Private ReadOnly _userId As Integer

    ''' <summary>
    ''' Khởi tạo MainForm với ID người dùng đã đăng nhập
    ''' </summary>
    ''' <param name="userId">Mã định danh của người dùng</param>
    Public Sub New(ByVal userId As Integer)
        _userId = userId
        InitializeComponents()
    End Sub

    ''' <summary>
    ''' Khởi tạo các điều khiển giao diện
    ''' </summary>
    Private Sub InitializeComponents()
        Me.Text = "Hệ Thống Quản Lý Sản Phẩm"
        Me.Size = New Size(400, 300)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        ' MenuStrip
        Dim menuStrip As New MenuStrip
        Dim menuProduct As New ToolStripMenuItem("Quản lý sản phẩm")
        Dim menuCategory As New ToolStripMenuItem("Quản lý danh mục")
        menuStrip.Items.AddRange({menuProduct, menuCategory})

        ' Sự kiện click cho menu
        AddHandler menuProduct.Click, AddressOf MenuProduct_Click
        AddHandler menuCategory.Click, AddressOf MenuCategory_Click

        ' Label hiển thị thông tin người dùng
        Dim currentUser = SessionManager.GetCurrentUser()
        Dim lblUser As New Label With {
            .Text = "Người dùng: " & If(currentUser?.Username, "Không xác định"),
            .Location = New Point(20, 40),
            .Size = New Size(200, 20)
        }

        Me.Controls.AddRange({menuStrip, lblUser})
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện click menu Quản lý sản phẩm
    ''' </summary>
    Private Sub MenuProduct_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim productForm As New ProductManagementForm(_userId)
        productForm.ShowDialog()
    End Sub

    ''' <summary>
    ''' Xử lý sự kiện click menu Quản lý danh mục
    ''' </summary>
    Private Sub MenuCategory_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim categoryForm As New CategoryManagementForm()
        categoryForm.ShowDialog()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class