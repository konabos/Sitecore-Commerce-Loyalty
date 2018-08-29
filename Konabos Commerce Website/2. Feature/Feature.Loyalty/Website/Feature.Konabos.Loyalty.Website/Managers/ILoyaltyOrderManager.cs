using Feature.Konabos.Loyalty.Website.Managers.Messages;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;

namespace Feature.Konabos.Loyalty.Website.Managers
{
    public interface ILoyaltyOrderManager
    {
        ManagerResponse<GetLoyaltyOrderResult, Sitecore.Commerce.Plugin.Orders.Order> GetLoyaltyOrder(IVisitorContext visitorContext, IStorefrontContext storefrontContext, string orderId);
    }
}
