using MediatR;
using Microsoft.AspNetCore.Mvc;
using SomeReallyComplexProject.ServiceOne.Application.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.ServiceOne.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator mediator;

        public ValuesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await mediator.Send(new CreateUserCommand($"UserName-{id}"));
            return Ok("New user ID:" + user.Id);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
