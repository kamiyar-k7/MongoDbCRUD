using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDbCRUD.Data;
using MongoDbCRUD.Entities;

namespace MongoDbCRUD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{

    #region Ctor

    private readonly IDbContext _context;
    public UsersController(IDbContext Context)
    {
        _context = Context;
    }

    #endregion

    [HttpPost("[action]")]
    public async Task<IActionResult> AddUser(User user)
    {
        await _context.Users.InsertOneAsync(user);
        return Ok();
    }

    [HttpGet("[action]/{Id}")]
    public async Task<ActionResult> GetUserById(string Id)
    {
        var user = await _context.Users.Find(x => x.Id == Id).FirstOrDefaultAsync();

        return Ok(user ?? new User());
    }

    [HttpGet("[action]/{Name}")]
    public async Task<ActionResult<List<User>>> GetUserByName(string Name)
    {
        var user = await _context.Users.Find(x => x.Name == Name).ToListAsync();
        return Ok(user);
    }

    [HttpDelete("[action]/{Id}")]
    public async Task<IActionResult> DeleteUser(string Id)
    {
       
        await _context.Users.DeleteOneAsync(x => x.Id == Id);
        return Ok();
    }

    [HttpPut("[action]/{Id}")]
    public async Task<IActionResult> UpdateUser(string Id, User user)
    {
        var update = Builders<User>.Update
            .Set(x => x.Name, user.Name)
            .Set(x => x.Email, user.Email)
            .Set(x => x.Age, user.Age);
        await _context.Users.UpdateOneAsync(x => x.Id == Id, update);
        return Ok();
    }



}
