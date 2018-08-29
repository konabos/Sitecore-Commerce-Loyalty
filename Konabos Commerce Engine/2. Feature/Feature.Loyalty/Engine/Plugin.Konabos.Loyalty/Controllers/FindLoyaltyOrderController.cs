using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Feature.Konabos.Loyalty.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Plugin.Konabos.Loyalty.Commands;
using Sitecore.Commerce.Core;

namespace Plugin.Konabos.Loyalty.Controllers
{
    [EnableQuery]
    [Route("api/FindLoyaltyOrder")]
    public class FindLoyaltyOrderController : CommerceController
    {
        public FindLoyaltyOrderController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
             : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpGet]
        [Route("(Id={id})")]
        [EnableQuery]
        public async Task<IActionResult> Get(string id)
        {
            if (!this.ModelState.IsValid || string.IsNullOrEmpty(id))
                return (IActionResult)this.NotFound();

            var result = await this.Command<GetLoyaltyOrderCommand>().Process(this.CurrentContext, id);
            return result != null ? (IActionResult)new ObjectResult((object)result) : (IActionResult)this.NotFound();
        }

    }
}
