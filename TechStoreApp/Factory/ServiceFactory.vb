' Factory/ServiceFactory.vb


Public Class ServiceFactory

    ''' <summary>
    ''' Tạo và trả về một đối tượng IAuthService
    ''' </summary>
    ''' <returns>Đối tượng IAuthService</returns>
    Public Shared Function CreateAuthService() As IAuthService
        Dim userRepository As IUserRepository = RepositoryFactory.CreateUserRepository()
        Return New AuthService(userRepository)
    End Function


    ''' <summary>
    ''' Tạo và trả về một đối tượng IRoleService
    ''' </summary>
    ''' <returns>Đối tượng IRoleService</returns>
    Public Shared Function CreateRoleService() As IRoleService
        Dim roleRepository As IRoleRepository = RepositoryFactory.CreateRoleRepository()
        Return New RoleService(roleRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IUserService
    ''' </summary>s
    ''' <returns>Đối tượng IUserService</returns>
    Public Shared Function CreateUserService() As IUserService
        Dim userRepository As IUserRepository = RepositoryFactory.CreateUserRepository()
        Return New UserService(userRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng ICategoryService
    ''' </summary>
    ''' <returns>Đối tượng ICategoryService</returns>
    Public Shared Function CreateCategoryService() As ICategoryService
        Dim categoryRepository As ICategoryRepository = RepositoryFactory.CreateCategoryRepository()
        Return New CategoryService(categoryRepository)
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IProductService
    ''' </summary>
    ''' <returns>Đối tượng IProductService</returns>
    Public Shared Function CreateProductService() As IProductService
        Dim productRepository As IProductRepository = RepositoryFactory.CreateProductRepository()
        Dim categoryRepository As ICategoryRepository = RepositoryFactory.CreateCategoryRepository()
        Return New ProductService(productRepository, CategoryRepository)
    End Function
End Class