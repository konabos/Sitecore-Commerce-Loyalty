using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.Entities.Orders;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Feature.Konabos.Loyalty.Website.Managers;
using Sitecore.Commerce;
using Sitecore.Commerce.XA.Foundation.Connect;
using System.Linq;

namespace Feature.Konabos.Loyalty.Website.Models
{
    public class OrderHeaderRenderingModel : Sitecore.Commerce.XA.Feature.Account.Models.OrderHeaderRenderingModel
    {
        public string PointsEarned { get; set; }
        public string PointsEarnedTooltip { get; set; }

        private readonly IModelProvider _modelProvider;
        private readonly ILoyaltyOrderManager _loyaltyOrderManager;
        private readonly IVisitorContext _visitorContext;
        private readonly IStorefrontContext _storefrontContext;

        public OrderHeaderRenderingModel(IStorefrontContext storefrontContext, IModelProvider modelProvider, ILoyaltyOrderManager loyaltyOrderManager, IVisitorContext visitorContext) : base(storefrontContext)
        {
            Assert.ArgumentNotNull(modelProvider, nameof(modelProvider));
            _storefrontContext = storefrontContext;
            _modelProvider = modelProvider;
            _loyaltyOrderManager = loyaltyOrderManager;
            _visitorContext = visitorContext;
        }

        public override void Initialize(Order order)
        {
            base.Initialize(order);
            var result = _loyaltyOrderManager.GetLoyaltyOrder(_visitorContext, _storefrontContext, order.OrderID);
            if (result.ServiceProviderResult.Success)
            {
                var loyaltyComponent = result.Result.Components.OfType<Plugin.Konabos.Loyalty.Components.LoyaltyComponent>().FirstOrDefault();
                this.PointsEarned = loyaltyComponent != null ? loyaltyComponent.Points.ToString() : "0";
            }
            else
                this.PointsEarned = "0";
        }
    }
}