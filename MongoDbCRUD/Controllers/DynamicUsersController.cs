using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCRUD.Data;
using System.Text.Json;

namespace MongoDbCRUD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DynamicUsersController : ControllerBase
{

    #region Ctor

    private readonly IDbContext _context;
    public DynamicUsersController(IDbContext context)
    {
        _context = context;
    }

    #endregion

    [HttpPost("[action]")]
    public async Task<ActionResult<dynamic>> AddUser( dynamic user)
    {
        try
        {
            var json = user.GetRawText();
            var document = BsonDocument.Parse(json);

            await _context.DynamicUsers.InsertOneAsync(document);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<dynamic>> GetDynamicUserById(string id)
    {
        try
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);


            var document = await _context.DynamicUsers.Find(filter).FirstOrDefaultAsync();

            if (document == null)
                return NotFound("User Didnt Found!");

            var json = JsonSerializer.Deserialize<object>(document.ToJson());


            return Ok(json);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<dynamic>> GetUserByField(string field, string value)
    {
        try
        {
           

            dynamic doc;
            #region Id Search
            if (ObjectId.TryParse(value, out var objectId))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

                doc = await _context.DynamicUsers.Find(filter).FirstOrDefaultAsync();

                if (doc == null)
                    return NotFound(new { message = $"User not found where {field} = {value}" });

                var idresult = BsonTypeMapper.MapToDotNetValue(doc);

                return Ok(idresult);

            }
            #endregion

            else
            {
                object parsedvalue;
                if (bool.TryParse(value, out var boolVal))
                    parsedvalue = boolVal;
                else if (int.TryParse(value, out var intVal))
                    parsedvalue = intVal;
                else if (long.TryParse(value, out var longVal))
                    parsedvalue = longVal;
                else if (double.TryParse(value, out var doubleVal))
                    parsedvalue = doubleVal;
                else
                    parsedvalue = value;

                var filter = Builders<BsonDocument>.Filter.Eq(field, parsedvalue);
               var docs = await _context.DynamicUsers.Find(filter).ToListAsync();

                if (docs.Count == 0)
                    return NotFound(new { message = $"No users found where {field} = {value}" });

 

                var result = docs.Select(doc =>
                {
                    var dict = doc.ToDictionary();
                    if (doc.Contains("_id"))
                        dict["_id"] = doc["_id"].AsObjectId.ToString();

                    return dict;
                });

                return Ok(result);
            }

           

       

        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateDynamicUser([FromBody] JsonElement user, [FromQuery] string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest(new { message = "Invalid ID format" });
            }

            var bsonDoc = BsonDocument.Parse(user.ToString());

            bsonDoc["_id"] = objectId;

            var result = await _context.DynamicUsers.ReplaceOneAsync( u => u["_id"] == objectId, bsonDoc);

            if (result.MatchedCount == 0)
            {
                return NotFound(new { message = $"No user found with id = {id}" });
            }

            return Ok(new { message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
