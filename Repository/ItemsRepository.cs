using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repository
{
    public class ItemsRepository
    {
        private const string collectionName = "Items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            if (mongoClient == null)
            {
                throw new InvalidOperationException("MongoClient is null.");
            }
            var database = mongoClient.GetDatabase("Catalog");
            if (database == null)
            {
                throw new InvalidOperationException("MongoDB database is null.");
            }
            dbCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
          
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(e => e.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            await dbCollection.InsertOneAsync(model);
        }

        public async Task UpdateAsync (Item model)
        {
            if(model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(e => e.Id, model.Id);
            await dbCollection.ReplaceOneAsync(filter, model);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(e => e.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}