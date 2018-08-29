using Feature.Konabos.Loyalty.Common.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace Plugin.Konabos.Loyalty.Pipelines
{
    public interface IGetLoyaltyOrderPipeline:IPipeline<string,Order, CommercePipelineExecutionContext>
    {
    }
}
