using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repository
{
    public interface IItemsRepository
    {
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task CreateAsync(Item model);
        Task UpdateAsync (Item model);
        Task RemoveAsync(Guid id);
        
    }
}