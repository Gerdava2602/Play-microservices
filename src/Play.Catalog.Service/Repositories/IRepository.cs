using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    //Refers to the structure of the repository
    //We constraint the type to be an entity
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid Id);
        Task RemoveAsync(Guid Id);
        Task UpdateAsync(T entity);
    }
}