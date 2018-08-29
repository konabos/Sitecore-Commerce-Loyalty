using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Arguments
{
    public class ReconcilePointsArgument:PipelineArgument
    {
        public string CustomerId { get; set; }

        public ReconcilePointsArgument(string customerId)
        {
            Condition.Requires<string>(customerId, "customerId").IsNotNullOrEmpty();
            this.CustomerId = customerId;
        }
    }
}
