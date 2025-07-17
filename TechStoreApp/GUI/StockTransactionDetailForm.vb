
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

    Public Sub New(ByVal transactionId As Integer)
        InitializeComponent()
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

            _lblTransactionCode.Text = $"Mã giao dịch: {transaction.TransactionCode}"
            _lblTransactionType.Text = $"Loại phiếu: {transaction.TransactionType}"
            _lblCreatedBy.Text = $"Người tạo: {transaction.CreatedByName}"
            _lblCreatedAt.Text = $"Ngày tạo: {transaction.CreatedAt}"
            _lblSupplier.Text = $"Nhà cung cấp: {transaction.SupplierName}"
            _lblStatus.Text = $"Trạng thái: {transaction.Status}"
            _lblApprovedBy.Text = $"Người duyệt: {transaction.ApprovedByName}"
            _lblApprovedAt.Text = $"Ngày duyệt: {If(transaction.ApprovedAt, "Chưa duyệt")}"

            Dim details = _transactionService.GetTransactionDetails(_transactionId)
            _gridDetails.DataSource = details
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub _btnClose_Click(sender As Object, e As EventArgs) Handles _btnClose.Click
        Me.Close()
    End Sub
End Class
