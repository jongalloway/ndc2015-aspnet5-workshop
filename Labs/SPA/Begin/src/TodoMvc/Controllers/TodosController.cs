using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TodoMvc.Models;

namespace TodoMvc.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        static List<Todo> sampleData = new Todo[] {
                new Todo { Id = 1, Title = "Item One", Completed = false },
                new Todo { Id = 2, Title = "Item Two", Completed = true }
        }.ToList();

        // GET: api/todos
        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            return sampleData;
        }

        // GET api/todos/5
        [HttpGet("{id}")]
        public Todo Get(int id)
        {
            return sampleData.FirstOrDefault(x => x.Id == id);
        }

        // POST api/todos
        [HttpPost]
        public IActionResult Post([FromBody]Todo value)
        {
            int maxId = sampleData.Max(t => t.Id);
            value.Id = maxId + 1;
            value.Completed = false;
            sampleData.Add(value);

            var result = new ObjectResult(value);
            result.StatusCode = (int)HttpStatusCode.Created;
            return result;
        }

        // PUT api/todos/5
        [HttpPut("{id}")]
        public Todo Put(int id, [FromBody]Todo value)
        {
            var item = sampleData.Find(t => t.Id == id);
            item.Title = value.Title;
            item.Completed = value.Completed;
            return item;
        }

        // DELETE api/todos/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            sampleData.RemoveAll(t => t.Id == id);
            Context.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        //TODO: Routing not working. Should respond to DELETE sent to /api/todos
        [HttpDelete("api/[controller]")]
        public void Archive()
        {
            sampleData.RemoveAll(t => t.Completed == true);
            Context.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        [Route("/api")]
        public void Api()
        {
            Context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}
