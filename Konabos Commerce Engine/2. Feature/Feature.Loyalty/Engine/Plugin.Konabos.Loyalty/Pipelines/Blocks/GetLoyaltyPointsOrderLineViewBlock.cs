using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;
using System.Linq;
using Plugin.Konabos.Loyalty.Components;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    public class GetLoyaltyPointsOrderLineViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
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
                entityViewToProcess = arg.ChildViews.FirstOrDefault(p => p.Name == "Lines") as EntityView;
            }
            else
            {
                entityViewToProcess = arg;
            }

            if (entityViewToProcess == null)
            {
                return Task.FromResult(arg);
            }

            foreach (var childView in entityViewToProcess.ChildViews)
            {
                EntityView entityChildViewToProcess = childView as EntityView;
                var lineItemId = entityChildViewToProcess.Properties.FirstOrDefault(p=> p.Name == "ItemId");
                var orderLineItem = order.Lines.FirstOrDefault(o => o.Id == lineItemId.Value);
                if (orderLineItem != null)
                {
                    int pointsEarned = orderLineItem.HasComponent<LoyaltyComponent>() ? orderLineItem.GetComponent<LoyaltyComponent>().PointsEarned : 0;
                    entityChildViewToProcess.Properties.Add(new ViewProperty { Name = "Points Earned", DisplayName = "Points Earned", IsReadOnly = true, RawValue = pointsEarned, Value = pointsEarned.ToString() });
                }
            }

            return Task.FromResult(arg);
        }
    }
}
