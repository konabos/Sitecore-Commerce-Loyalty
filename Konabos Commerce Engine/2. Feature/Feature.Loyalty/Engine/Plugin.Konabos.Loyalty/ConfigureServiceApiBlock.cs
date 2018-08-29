using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Conditions;
using System.Threading.Tasks;
using Feature.Konabos.Loyalty.Common.Entities;
using Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Konabos.Loyalty
{
    public class ConfigureServiceApiBlock :
                    PipelineBlock<ODataConventionModelBuilder,
                        ODataConventionModelBuilder,
                        CommercePipelineExecutionContext>
    {
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {
            Condition.Requires(modelBuilder).IsNotNull("The argument can not be null");

            //modelBuilder.AddEntityType(typeof(Order));
            //modelBuilder.EntitySet<Order>("GetOrder");

            var getLoyaltyOrder = modelBuilder.Action("GetLoyaltyOrder");
            getLoyaltyOrder.Returns<string>();
            getLoyaltyOrder.Parameter<string>("orderId");
            //getLoyaltyOrder.Returns<LoyaltyOrder>();
            //getLoyaltyOrder.ReturnsFromEntitySet<LoyaltyOrder>("Commands");

            return Task.FromResult(modelBuilder);
        }
    }
}
