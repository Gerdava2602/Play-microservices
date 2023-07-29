using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    //Refers to the structure of the repository
    public interface IItemsRepository
    {
        Task CreateAsync(Item entity);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid Id);
        Task RemoveAsync(Guid Id);
        Task UpdateAsync(Item entity);
    }
}