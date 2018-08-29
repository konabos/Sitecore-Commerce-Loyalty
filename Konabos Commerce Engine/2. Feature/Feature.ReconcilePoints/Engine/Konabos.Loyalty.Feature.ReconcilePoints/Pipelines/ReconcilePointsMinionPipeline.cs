using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Pipelines
{
    public class ReconcilePointsMinionPipeline : CommercePipeline<ReconcilePointsArgument, Customer>, IReconcilePointsMinionPipeline, IPipeline<ReconcilePointsArgument, Customer, CommercePipelineExecutionContext>
    {
        public ReconcilePointsMinionPipeline(IPipelineConfiguration<IReconcilePointsMinionPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
