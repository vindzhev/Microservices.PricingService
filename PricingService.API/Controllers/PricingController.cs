namespace PricingService.API.Controllers
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Mvc;
    
    using MicroservicesPOC.Shared.Controllers;
    
    using PricingService.Application.Tariffs.Commands;

    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ApiController
    {
        //TODO refactor products to use Code as Id
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CalculatePriceCommand command) => this.Ok(await this.Mediator.Send(command));
    }
}
