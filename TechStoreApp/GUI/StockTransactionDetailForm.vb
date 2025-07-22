Imports System.Data.Common

Public Class StockTransactionDetailForm
    Inherits Form

    Private ReadOnly _transactionService As IStockTransactionService
    Private ReadOnly _transactionId As Integer
    Private WithEvents _btnClose As Button
    Private _lblTransactionCode As Label
    Private _lblTransactionType As Label
    Private _lblCreatedBy As Label
    Private _lblCreatedAt As Label
    Private _lblSupplier As Label
    Private _lblStatus As Label
    Private _lblApprovedBy As Label
    Private _lblApprovedAt As Label
    Private _gridDetails As DataGridView
    Private WithEvents _btnApprove As Button
    Private WithEvents _btnReject As Button

    Public Sub New(ByVal transactionId As Integer)
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        _transactionId = transactionId
        _transactionService = ServiceFactory.CreateStockTransactionService()

        LoadTransactionDetails()
    End Sub

    Private Sub LoadTransactionDetails()
        Try
            Dim transaction = _transactionService.GetTransactionById(_transactionId)
            If transaction Is Nothing Then
                MessageBox.Show("Phiếu không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            If transaction.Status = "Pending" Then
                Dim currentUser = SessionManager.GetCurrentUser()
                If currentUser IsNot Nothing AndAlso currentUser.RoleId = 1 Then
                    _btnApprove.Visible = True
                    _btnReject.Visible = True
                End If
            End If


            _lblTransactionCode.Text = "Mã giao dịch: " & transaction.TransactionCode
            _lblTransactionType.Text = "Loại phiếu: " & transaction.TransactionType
            _lblCreatedBy.Text = "Người tạo: " & transaction.CreatedByName
            _lblCreatedAt.Text = "Ngày tạo: " & transaction.CreatedAt.ToString("dd/MM/yyyy HH:mm")

            _lblSupplier.Text = "Nhà cung cấp: " & If(transaction.SupplierId.HasValue, transaction.SupplierName, "(Không có)")
            _lblStatus.Text = "Trạng thái: " & transaction.Status
            _lblApprovedBy.Text = "Người duyệt: " & If(transaction.ApprovedBy.HasValue AndAlso Not String.IsNullOrEmpty(transaction.ApprovedByName), transaction.ApprovedByName, "(Chưa duyệt)")
            _lblApprovedAt.Text = "Ngày duyệt: " & If(transaction.ApprovedAt.HasValue AndAlso transaction.ApprovedAt.Value > DateTime.MinValue, transaction.ApprovedAt.Value.ToString("dd/MM/yyyy HH:mm"), "(Chưa duyệt)")


            Dim details = _transactionService.GetTransactionDetails(_transactionId)
            _gridDetails.DataSource = details

            If _gridDetails.Columns.Count > 0 Then
                _gridDetails.Columns("DetailId").HeaderText = "Mã chi tiết phiếu"
                _gridDetails.Columns("DetailId").Visible = False
                _gridDetails.Columns("TransactionId").HeaderText = "Mã phiếu"
                _gridDetails.Columns("ProductId").HeaderText = "Mã sản phẩm"
                _gridDetails.Columns("ProductName").HeaderText = "Tên sản phẩm"
                _gridDetails.Columns("Quantity").HeaderText = "Số lượng"
                _gridDetails.Columns("Unit").HeaderText = "Đơn vị"
                _gridDetails.Columns("Note").HeaderText = "Ghi chú"

                ' Thêm các cột khác nếu cần
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Console.WriteLine("Lỗi viewDetail: " & ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub _btnApprove_Click(sender As Object, e As EventArgs) Handles _btnApprove.Click
        Dim confirm = MessageBox.Show("Bạn có chắc muốn duyệt phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm = DialogResult.Yes Then
            Dim user = SessionManager.GetCurrentUser()
            Dim result = _transactionService.ApproveTransaction(_transactionId, user.UserId, True)
            If result.Success Then
                MessageBox.Show("Phiếu đã được duyệt.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If
        End If
    End Sub

    Private Sub _btnReject_Click(sender As Object, e As EventArgs) Handles _btnReject.Click
        Dim confirm = MessageBox.Show("Bạn có chắc muốn từ chối phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm = DialogResult.Yes Then
            Dim user = SessionManager.GetCurrentUser()
            Dim result = _transactionService.ApproveTransaction(_transactionId, user.UserId, False)
            If result.Success Then
                MessageBox.Show("Phiếu đã bị từ chối.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show(String.Join(Environment.NewLine, result.Errors.ToArray()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub _btnClose_Click(sender As Object, e As EventArgs) Handles _btnClose.Click
        Me.Close()
    End Sub

    Private Sub StockTransactionDetailForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
