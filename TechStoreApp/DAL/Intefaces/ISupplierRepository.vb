Public Interface ISupplierRepository
    ' Read operations
    Function GetAllSuppliers() As List(Of Supplier)
    Function GetSupplierById(supplierId As Integer) As Supplier


    ' Create operation
    Function AddSupplier(supplier As Supplier) As Integer

    ' Update operation
    Function UpdateSupplier(supplier As Supplier) As Boolean

    ' Delete operation
    Function DeleteSupplier(supplierId As Integer) As Boolean
End Interface
