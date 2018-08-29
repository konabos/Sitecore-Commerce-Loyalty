using System.Threading.Tasks;
using Plugin.Konabos.Loyalty.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    public class UpdateCustomDetailsBlock : Sitecore.Commerce.Plugin.Customers.UpdateCustomerDetailsBlock
    {
        public UpdateCustomDetailsBlock(IFindEntityPipeline findEntityPipeline) : base(findEntityPipeline)
        {
        }

        public override async Task<Customer> Run(Customer arg, CommercePipelineExecutionContext context)
        {
            var customer = await base.Run(arg, context);

            if (arg.HasComponent<LoyaltyComponent>())
            {
                var customDetails = arg.GetComponent<LoyaltyComponent>();
                customer.SetComponent(customDetails);
            }

            return customer;
        }
    }
}
