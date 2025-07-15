Public Interface ISupplierRepository
    Function GetAllSuppliers() As List(Of Supplier)
    Function GetSupplierById(supplierId As Integer) As Supplier
End Interface
