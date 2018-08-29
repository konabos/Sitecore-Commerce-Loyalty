using Plugin.Konabos.Loyalty.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    [PipelineDisplayName("Orders.block.OrderPlacedAssignPoints")]
    public class OrderPlacedAssignPointsBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        public OrderPlacedAssignPointsBlock(GenerateUniqueCodeCommand generateUniqueCodeCommand)
          : base((string)null)
        {
        }

        public override Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires<Order>(arg).IsNotNull<Order>("The order can not be null");

            int orderPoints = 0;
            foreach (var line in arg.Lines)
            {
                var lineLoyalty = line.GetComponent<LoyaltyComponent>();
                orderPoints += lineLoyalty.PointsEarned;
            }

            var loyalty = arg.GetComponent<LoyaltyComponent>();
            loyalty.PointsEarned = orderPoints;

            return Task.FromResult<Order>(arg);
        }
    }
}
