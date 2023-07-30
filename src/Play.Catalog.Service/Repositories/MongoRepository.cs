using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        //MongoDB item collection
        private readonly IMongoCollection<T> dbCollection;
        //Filter builder to query
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        //Constructor
        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);
        }

        //Returns items that are only readOnly
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid Id)
        {

            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, Id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}