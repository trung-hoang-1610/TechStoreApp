' Factory/RepositoryFactory.vb
Imports DAL
Imports DAL.Interfaces

Public Class RepositoryFactory
    ''' <summary>
    ''' Tạo và trả về một đối tượng IRoleRepository
    ''' </summary>
    ''' <returns>Đối tượng IRoleRepository</returns>
    Public Shared Function CreateRoleRepository() As IRoleRepository
        Return New RoleRepository()
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IUserRepository
    ''' </summary>
    ''' <returns>Đối tượng IUserRepository</returns>
    Public Shared Function CreateUserRepository() As IUserRepository
        Return New UserRepository()
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng ICategoryRepository
    ''' </summary>
    ''' <returns>Đối tượng ICategoryRepository</returns>
    Public Shared Function CreateCategoryRepository() As ICategoryRepository
        Return New CategoryRepository()
    End Function

    ''' <summary>
    ''' Tạo và trả về một đối tượng IProductRepository
    ''' </summary>
    ''' <returns>Đối tượng IProductRepository</returns>
    Public Shared Function CreateProductRepository() As IProductRepository
        Return New ProductRepository()
    End Function
End Class