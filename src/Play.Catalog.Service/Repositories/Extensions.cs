using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            //Serialize mongodb Ids as strings
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            //Serialize mongodb DateTimes as strings
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            //Adding MongoDB settings
            //In here we construct our mongoclient and databsser from the mongodbsettings section
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration?.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
                IMongoClient client = new MongoClient(mongoDbSettings?.ConnectionString);
                return client.GetDatabase(serviceSettings?.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                //Using the above service we get the database and the collection name from the settings
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}