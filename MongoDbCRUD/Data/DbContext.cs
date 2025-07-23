using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCRUD.Entities;

namespace MongoDbCRUD.Data;

public class DbContext : IDbContext
{
    
    public IMongoDatabase _Database;
    public DbContext(IConfiguration configuration)
    {
        var client = new MongoClient( configuration["DatabaseSettings:connectionStrings"]);
        _Database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

    }

    public IMongoCollection<User> Users => _Database.GetCollection<User>("Users");

    public IMongoCollection<BsonDocument> DynamicUsers => _Database.GetCollection<BsonDocument>("DynamicUsers");
}
