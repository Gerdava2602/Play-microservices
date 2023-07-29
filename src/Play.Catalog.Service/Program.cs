using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //Don't removes the Async suffix from the action names
    options.SuppressAsyncSuffixInActionNames = false;
})
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding settings
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

//Adding MongoDB settings
//In here we construct our mongoclient and databsser from the mongodbsettings section
builder.Services.AddSingleton(serviceProvider =>
{
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
    IMongoClient client = new MongoClient(mongoDbSettings?.ConnectionString);
    return client.GetDatabase(serviceSettings?.ServiceName);
});

//Adding the repository
//We register the repository as a singleton using the interface and the implementation
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

//Serialize mongodb Ids as strings
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
//Serialize mongodb DateTimes as strings
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
