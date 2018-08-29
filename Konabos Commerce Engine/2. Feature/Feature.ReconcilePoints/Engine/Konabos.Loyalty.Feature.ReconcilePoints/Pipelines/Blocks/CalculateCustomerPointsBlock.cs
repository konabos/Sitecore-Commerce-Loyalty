using System.Linq;
using System.Threading.Tasks;
using Plugin.Konabos.Loyalty.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Blocks
{
    public class CalculateCustomerPointsBlock : PipelineBlock<Customer, Customer, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IFindEntitiesInListPipeline _findEntitiesInListPipeline;

        public CalculateCustomerPointsBlock(IPersistEntityPipeline persistEntityPipeline, IFindEntitiesInListPipeline findEntitiesInListPipeline)
        {
            _persistEntityPipeline = persistEntityPipeline;
            _findEntitiesInListPipeline = findEntitiesInListPipeline;
        }

        public override async Task<Customer> Run(Customer customer, CommercePipelineExecutionContext context)
        {
            string listName = string.Format("Orders-ByCustomer-{0}", (object)customer.Id);
            FindEntitiesInListArgument result = await _findEntitiesInListPipeline.Run(new FindEntitiesInListArgument(typeof(Order), listName, 0, int.MaxValue), context);

            int customerOrderPoints = 0;
            if (result != null && result.List !=null && result.List.Items.Any())
            {
                foreach (var entity in result.List.Items)
                {
                    var order = (Order)entity;
                    if (order != null)
                    {
                        customerOrderPoints += order.HasComponent<LoyaltyComponent>() ? order.GetComponent<LoyaltyComponent>().PointsEarned : 0;
                    }
                }
                var loyaltyComponent = customer.GetComponent<LoyaltyComponent>();
                loyaltyComponent.PointsEarned = customerOrderPoints;

                var persistEntityArgument = await _persistEntityPipeline.Run(new PersistEntityArgument(customer), context);
            }
            return customer;
        }
    }
}
