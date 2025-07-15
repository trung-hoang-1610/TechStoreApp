Imports BLL
Imports BLL.Interfaces
Imports DAL.Interfaces
Imports DAL

''' <summary>
''' Factory class để tạo các đối tượng dịch vụ (services) cho tầng logic nghiệp vụ.
''' </summary>
Public Class ServiceFactory
    ''' <summary>
    ''' Tạo và trả về một đối tượng IAuthService.
    ''' </summary>
    ''' <returns>Đối tượng IAuthService để quản lý xác thực người dùng.</returns>
    Public Shared Function CreateAuthService() As IAuthService
        Dim userRepository As IUserRepository = RepositoryFactory.CreateUserRepository()
        Return New AuthService(userRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IRoleService.
    ''' </summary>
    ''' <returns>Đối tượng IRoleService để quản lý vai trò.</returns>
    Public Shared Function CreateRoleService() As IRoleService
        Dim roleRepository As IRoleRepository = RepositoryFactory.CreateRoleRepository()
        Return New RoleService(roleRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IUserService.
    ''' </summary>
    ''' <returns>Đối tượng IUserService để quản lý người dùng.</returns>
    Public Shared Function CreateUserService() As IUserService
        Dim userRepository As IUserRepository = RepositoryFactory.CreateUserRepository()
        Return New UserService(userRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng ICategoryService.
    ''' </summary>
    ''' <returns>Đối tượng ICategoryService để quản lý danh mục sản phẩm.</returns>
    Public Shared Function CreateCategoryService() As ICategoryService
        Dim categoryRepository As ICategoryRepository = RepositoryFactory.CreateCategoryRepository()
        Return New CategoryService(categoryRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IProductService.
    ''' </summary>
    ''' <returns>Đối tượng IProductService để quản lý sản phẩm.</returns>
    Public Shared Function CreateProductService() As IProductService
        Dim productRepository As IProductRepository = RepositoryFactory.CreateProductRepository()
        Dim categoryRepository As ICategoryRepository = RepositoryFactory.CreateCategoryRepository()
        Dim supplierRepository As ISupplierRepository = RepositoryFactory.CreateSupplierRepository()
        Return New ProductService(productRepository, categoryRepository, supplierRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IInventoryTransactionService.
    ''' </summary>
    ''' <returns>Đối tượng IInventoryTransactionService để quản lý giao dịch kho.</returns>
    Public Shared Function CreateInventoryTransactionService() As IInventoryTransactionService
        Dim transactionRepository As IInventoryTransactionRepository = RepositoryFactory.CreateInventoryTransactionRepository()
        Dim productRepository As IProductRepository = RepositoryFactory.CreateProductRepository()
        Dim userRepository As IUserRepository = RepositoryFactory.CreateUserRepository()
        Return New InventoryTransactionService(transactionRepository, productRepository, userRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng ISupplierService.
    ''' </summary>
    ''' <returns>Đối tượng ISupplierService để quản lý nhà cung cấp.</returns>
    Public Shared Function CreateSupplierService() As ISupplierService
        Dim supplierRepository As ISupplierRepository = RepositoryFactory.CreateSupplierRepository()
        Return New SupplierService(supplierRepository)
    End Function
End Class