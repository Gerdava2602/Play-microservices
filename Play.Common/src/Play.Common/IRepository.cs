using System.Linq.Expressions;

namespace Play.Common
{
    //Refers to the structure of the repository
    //We constraint the type to be an entity
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        //We add a filter to the GetAllAsync method
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Guid Id);
        Task<T> GetAsync(Guid Id, Expression<Func<T, bool>> filter);
        Task RemoveAsync(Guid Id);
        Task UpdateAsync(T entity);
    }
}