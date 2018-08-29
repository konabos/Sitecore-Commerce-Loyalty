using System;
using System.Threading.Tasks;
using Feature.Konabos.Loyalty.Common.Entities;
using Microsoft.Extensions.Logging;
using Plugin.Konabos.Loyalty.Pipelines;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Konabos.Loyalty.Commands
{
    public class GetLoyaltyOrderCommand:CommerceCommand
    {
        private readonly IGetLoyaltyOrderPipeline _getLoyaltyOrderPipeline;

        public GetLoyaltyOrderCommand(IGetLoyaltyOrderPipeline getLoyaltyOrderPipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _getLoyaltyOrderPipeline = getLoyaltyOrderPipeline;
        }

        public virtual async Task<Order> Process(CommerceContext context, string orderId)
        {
            return await GetLoyaltyOrder(context, orderId);
        }

        protected virtual async Task<Order> GetLoyaltyOrder(CommerceContext context, string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return null;
            }

            var options = new CommercePipelineExecutionContextOptions(context);

            var loyaltyOrder = await _getLoyaltyOrderPipeline.Run(orderId, options);
            if (loyaltyOrder == null)
            {
                context.Logger.LogDebug($"Order {orderId} was not found.");
            }

            return loyaltyOrder;
        }
    }
}
