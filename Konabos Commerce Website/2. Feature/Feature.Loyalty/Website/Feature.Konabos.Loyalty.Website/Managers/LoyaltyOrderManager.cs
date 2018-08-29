using System;
using Feature.Konabos.Loyalty.Website.Managers.Messages;
using Sitecore.Commerce.Engine.Connect;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using Sitecore.Commerce.ServiceProxy;
using Sitecore.Diagnostics;
using CommerceOps.Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Orders;

namespace Feature.Konabos.Loyalty.Website.Managers
{
    public class LoyaltyOrderManager : ILoyaltyOrderManager
    {
        public ManagerResponse<GetLoyaltyOrderResult, Sitecore.Commerce.Plugin.Orders.Order> GetLoyaltyOrder(IVisitorContext visitorContext, IStorefrontContext storefrontContext, string orderId)
        {
            var getLoyaltyOrderResult = GetLoyaltyOrderFromEngine(storefrontContext.CurrentStorefront.ShopName, visitorContext.CustomerId, orderId);
            return new ManagerResponse<GetLoyaltyOrderResult, Sitecore.Commerce.Plugin.Orders.Order>(getLoyaltyOrderResult, getLoyaltyOrderResult.LoyaltyOrder);
        }

        protected GetLoyaltyOrderResult GetLoyaltyOrderFromEngine(string shopName, string customerId, string orderId)
        {
            var result = new GetLoyaltyOrderResult();
            try
            {
                var container = EngineConnectUtility.GetShopsContainer(shopName: shopName, customerId: customerId);
                Sitecore.Commerce.Plugin.Orders.Order order = Proxy.GetValue<Sitecore.Commerce.Plugin.Orders.Order>(container.Orders.ByKey(orderId).Expand("Lines($expand=CartLineComponents),Components"));

                if (order != null)
                {
                    result.LoyaltyOrder  = order;
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to get order:'{orderId}' shopName:'{shopName}', customerId'{customerId}'", ex, this);
            }
            return result;
        }
    }
}