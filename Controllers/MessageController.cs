using System.Threading.Tasks;
using chatroom.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatroom.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        // GET api/message
        [HttpGet]
        public async Task<IActionResult> GeAll()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new MessageQuery(db);
                var result = await query.AllAsync();
                return new OkObjectResult(result);
            }
        }

        // GET api/message/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new MessageQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                return new NotFoundResult();
                return new OkObjectResult(result);
            }
        }

        // POST api/message
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Message body)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                body.Db = db;
                await body.InsertAsync();
                return new OkObjectResult(body);
            }
        }

        // PUT api/message/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Message body)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new MessageQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                result.text = body.text;
                result.userid = body.userid;
                await result.UpdateAsync();
                return new OkObjectResult(result);
            }
        }

        // DELETE api/message/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new MessageQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                await result.DeleteAsync();
                return new OkResult();
            }
        }

        // DELETE api/message
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new MessageQuery(db);
                await query.DeleteAllAsync();
                return new OkResult();
            }
        }
    }
}