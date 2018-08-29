using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.OData;
using Plugin.Konabos.Loyalty.Commands;

namespace Plugin.Konabos.Loyalty.Controllers
{
    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider,
                            CommerceEnvironment globalEnvironment)
                            : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("GetLoyaltyOrder()")]
        public async Task<IActionResult> GetLoyaltyOrder([FromBody] ODataActionParameters value)
        {
            if (!ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (!value.ContainsKey("orderId"))
            {
                return new BadRequestObjectResult(value);
            }

            var orderId = value["orderId"].ToString();
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return new BadRequestObjectResult(value);
            }

            var command = Command<GetLoyaltyOrderCommand>();
            await command.Process(CurrentContext, orderId);
            return Ok("444");

            //var component = await Command<GetLoyaltyOrderCommand>().Process(CurrentContext, orderId);
            //return new ObjectResult(component);
        }
    }
}
