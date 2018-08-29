using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Pipelines
{
    public interface IReconcilePointsMinionPipeline : IPipeline<ReconcilePointsArgument, Customer, CommercePipelineExecutionContext>
    {
    }
}
