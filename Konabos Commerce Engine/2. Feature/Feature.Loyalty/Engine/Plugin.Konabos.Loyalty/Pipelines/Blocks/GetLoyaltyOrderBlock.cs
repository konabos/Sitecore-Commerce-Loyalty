using System.Threading.Tasks;
using Feature.Konabos.Loyalty.Common.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    public class GetLoyaltyOrderBlock : PipelineBlock<string, Order, CommercePipelineExecutionContext>
    {
        private readonly GetOrderCommand _getOrderCommand;

        public GetLoyaltyOrderBlock(GetOrderCommand getOrderCommand)
        {
            _getOrderCommand = getOrderCommand;
        }

        public override async Task<Order> Run(string arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The compare collection id can not be null");
            return await _getOrderCommand.Process(context.CommerceContext, arg);
        }
    }
}
