Imports DAL
Imports DAL.Interfaces

''' <summary>
''' Factory class để tạo các đối tượng repository cho việc truy cập dữ liệu.
''' </summary>
Public Class RepositoryFactory
    ''' <summary>
    ''' Tạo và trả về một đối tượng IRoleRepository.
    ''' </summary>
    ''' <returns>Đối tượng IRoleRepository để quản lý vai trò.</returns>
    Public Shared Function CreateRoleRepository() As IRoleRepository
        Return New RoleRepository()
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IUserRepository.
    ''' </summary>
    ''' <returns>Đối tượng IUserRepository để quản lý người dùng.</returns>
    Public Shared Function CreateUserRepository() As IUserRepository
        Return New UserRepository()
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng ICategoryRepository.
    ''' </summary>
    ''' <returns>Đối tượng ICategoryRepository để quản lý danh mục sản phẩm.</returns>
    Public Shared Function CreateCategoryRepository() As ICategoryRepository
        Return New CategoryRepository()
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IProductRepository.
    ''' </summary>
    ''' <returns>Đối tượng IProductRepository để quản lý sản phẩm.</returns>
    Public Shared Function CreateProductRepository() As IProductRepository
        Return New ProductRepository()
    End Function


    ''' <summary>
    ''' Tạo và trả về một đối tượng ISupplierRepository.
    ''' </summary>
    ''' <returns>Đối tượng ISupplierRepository để quản lý nhà cung cấp.</returns>
    Public Shared Function CreateSupplierRepository() As ISupplierRepository
        Return New SupplierRepository()
    End Function

    Public Shared Function CreateStockTransactionRepository() As IStockTransactionRepository
        Return New StockTransactionRepository()
    End Function

End Class