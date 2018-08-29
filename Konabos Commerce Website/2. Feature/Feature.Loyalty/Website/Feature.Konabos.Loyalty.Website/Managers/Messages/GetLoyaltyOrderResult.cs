using Sitecore.Commerce.Services;

namespace Feature.Konabos.Loyalty.Website.Managers.Messages
{
    public class GetLoyaltyOrderResult : ServiceProviderResult
    {
        public Sitecore.Commerce.Plugin.Orders.Order LoyaltyOrder { get; set; }
    }
}