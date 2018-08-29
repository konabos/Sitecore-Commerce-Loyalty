using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;
using System.Linq;
using Plugin.Konabos.Loyalty.Components;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    public class GetLoyaltyPointsOrderViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            EntityViewArgument entityViewArgument = context.CommerceContext.GetObject<EntityViewArgument>();
            if (entityViewArgument.ViewName != context.GetPolicy<KnownOrderViewsPolicy>().Summary && entityViewArgument.ViewName != context.GetPolicy<KnownOrderViewsPolicy>().Master)
            {
                // Do nothing if this entityViewArgument is for a different view
                return Task.FromResult(arg);
            }
            if (entityViewArgument.Entity == null)
            {
                // Do nothing if there is no entity loaded
                return Task.FromResult(arg);
            }
            // Only do something if the Entity is an order
            if (!(entityViewArgument.Entity is Order))
            {
                return Task.FromResult(arg);
            }
            var order = entityViewArgument.Entity as Order;
            EntityView entityViewToProcess = null;
            if (entityViewArgument.ViewName == context.GetPolicy<KnownOrderViewsPolicy>().Master)
            {
                entityViewToProcess = arg.ChildViews.FirstOrDefault(p => p.Name == "Summary") as EntityView;
            }
            else
            {
                entityViewToProcess = arg;
            }

            if (entityViewToProcess == null)
            {
                return Task.FromResult(arg);
            }

            int pointsEarned = order.HasComponent<LoyaltyComponent>() ? order.GetComponent<LoyaltyComponent>().PointsEarned : 0;
            int pointsSpent = order.HasComponent<LoyaltyComponent>() ? order.GetComponent<LoyaltyComponent>().PointsSpent : 0;

            entityViewToProcess.Properties.Add(new ViewProperty { Name = "Points Earned", DisplayName = "Points Earned", IsReadOnly = true, RawValue = pointsEarned, Value = pointsEarned.ToString() });
            entityViewToProcess.Properties.Add(new ViewProperty { Name = "Points Spent", DisplayName = "Points Spent", IsReadOnly = true, RawValue = pointsSpent, Value = pointsSpent.ToString() });

            return Task.FromResult(arg);
        }
    }
}
