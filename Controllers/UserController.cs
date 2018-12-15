using System.Threading.Tasks;
using chatroom.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatroom.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        // GET api/user
        [HttpGet]
        public async Task<IActionResult> GeAll()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new UserQuery(db);
                var result = await query.AllAsync();
                return new OkObjectResult(result);
            }
        }

        // GET api/user/name/amir
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetOne(string name)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new UserQuery(db);
                var result = await query.FindOneByNameAsync(name);
                if (result == null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new UserQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                return new NotFoundResult();
                return new OkObjectResult(result);
            }
        }

        // POST api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User body)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                body.Db = db;
                await body.InsertAsync();
                return new OkObjectResult(body);
            }
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]User body)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new UserQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                result.name = body.name;
                await result.UpdateAsync();
                return new OkObjectResult(result);
            }
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new UserQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                await result.DeleteAsync();
                return new OkResult();
            }
        }

        // DELETE api/user
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new UserQuery(db);
                await query.DeleteAllAsync();
                return new OkResult();
            }
        }
    }
}