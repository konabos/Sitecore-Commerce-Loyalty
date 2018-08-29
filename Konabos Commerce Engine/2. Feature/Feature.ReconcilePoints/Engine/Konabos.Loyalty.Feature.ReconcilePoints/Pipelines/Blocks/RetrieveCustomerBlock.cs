using System.Threading.Tasks;
using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Blocks
{
    [PipelineDisplayName("LoyaltyPoints.block.RetrieveCustomerBlock")]
    public class RetrieveCustomerBlock : PipelineBlock<ReconcilePointsArgument, Customer, CommercePipelineExecutionContext>
    {
        private readonly IFindEntityPipeline _findEntityPipeline;

        public RetrieveCustomerBlock(IFindEntityPipeline findEntityPipeline)
        {
            _findEntityPipeline = findEntityPipeline;
        }

        public override async Task<Customer> Run(ReconcilePointsArgument arg, CommercePipelineExecutionContext context)
        {
            Customer customer = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(Customer), string.Format("{0}", (object)arg.CustomerId),false), context) as Customer;

            if (customer == null)
            {
                CommercePipelineExecutionContext executionContext = context;
                string reason = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error,
                    "CustomerNotFound",
                    new object[1] { arg.CustomerId },
                    string.Format("Customer {0} was not found.", (object)arg.CustomerId));
                executionContext.Abort(reason, (object)context);
                executionContext = (CommercePipelineExecutionContext)null;
                return (Customer)null;
            }

            return customer;
        }
    }
}
