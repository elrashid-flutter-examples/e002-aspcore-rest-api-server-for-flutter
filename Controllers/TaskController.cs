using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using App2.Core.BL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace App2.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly string webRootPath;
        private readonly string contentRootPath;

        public TaskController(IHostingEnvironment hostingEnviroment)
        {
            _hostingEnviroment = hostingEnviroment;
            webRootPath = hostingEnviroment.WebRootPath;
            contentRootPath = hostingEnviroment.ContentRootPath;
        }


        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskOpj>>> GetTasks()
        {
            return await TaskManager.AllAsync(contentRootPath);
        }

        // GET: api/GetTask/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskOpj>> GetTask(string id)
        {
            var obj = await TaskManager.getAsync(contentRootPath, id);
            if (obj == null)
            {
                return NotFound();
            }

            return obj;
        }

        // POST: api/Task
        [HttpPost]
        public async Task<ActionResult<TaskOpj>> PostTask(TaskOpj item)
        {
            var obj = await TaskManager.createAsync(contentRootPath, item);
            return CreatedAtAction(nameof(GetTask), new { id = obj.guid }, obj);
        }
        // PUT: api/Task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(string id, TaskOpj item)
        {
            if (id != item.guid)
            {
                return BadRequest();
            }
            var isUpdated = await TaskManager.updateAsync(contentRootPath, item);
            //TODO: Check isUpdated
            return NoContent();
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var obj = await TaskManager.getAsync(contentRootPath, id);

            if (obj == null)
            {
                return NotFound();
            }

            var isDeleted = TaskManager.delete(contentRootPath, id);
            //TODO: Check isDeleted

            return NoContent();
        }


    }
}
