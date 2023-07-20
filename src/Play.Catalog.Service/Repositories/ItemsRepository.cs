using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositorie
{
    public class ItemsRepository
    {
        //Collection name
        private const string collectionName = "items";
        //MongoDB item collection
        private readonly IMongoCollection<Item> dbCollection;
        //Filter builder to query
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        //Constructor
        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            // gets the database
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(collectionName);
        }

        //Returns items that are only readOnly
        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid Id)
        {

            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid Id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, Id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}