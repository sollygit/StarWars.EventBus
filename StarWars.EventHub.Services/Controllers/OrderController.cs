using Microsoft.AspNetCore.Mvc;
using StarWars.EventHub.Models;
using StarWars.EventHub.Services.Services;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IEventBusService<OrderRequest> service;

        public OrderController(IEventBusService<OrderRequest> service)
        {
            this.service = service;
        }

        [HttpPost("Process")]
        public async Task<IActionResult> Process([FromBody] OrderRequest request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            await service.Process(request);

            return Ok();
        }
    }
}
