using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCRUD.Entities;

namespace MongoDbCRUD.Data;

public interface IDbContext
{

    IMongoCollection<User> Users { get; }
     IMongoCollection<BsonDocument> DynamicUsers { get; }
}
