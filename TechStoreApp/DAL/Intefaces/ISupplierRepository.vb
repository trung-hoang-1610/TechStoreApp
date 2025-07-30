Imports System.Threading.Tasks
Public Interface ISupplierRepository
    ' Read operations
    Function GetAllSuppliersAsync() As Task(Of List(Of Supplier))
    Function GetSupplierByIdAsync(supplierId As Integer) As Task(Of Supplier)


    ' Create operation
    Function AddSupplierAsync(supplier As Supplier) As Task(Of Integer)

    ' Update operation
    Function UpdateSupplierAsync(supplier As Supplier) As Task(Of Boolean)

    ' Delete operation
    Function DeleteSupplierAsync(supplierId As Integer) As Task(Of Boolean)
End Interface
