using Microsoft.Extensions.Logging;
using Feature.Konabos.Loyalty.Common.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Konabos.Loyalty.Pipelines
{
    public class GetLoyaltyOrderPipeline : CommercePipeline<string, Order>, IGetLoyaltyOrderPipeline
    {
        public GetLoyaltyOrderPipeline(IPipelineConfiguration<IGetLoyaltyOrderPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
