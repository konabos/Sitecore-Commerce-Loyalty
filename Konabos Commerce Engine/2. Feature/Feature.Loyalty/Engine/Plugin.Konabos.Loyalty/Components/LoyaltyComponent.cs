using System;
using Sitecore.Commerce.Core;

namespace Plugin.Konabos.Loyalty.Components
{
    public class LoyaltyComponent:Component
    {
        public int PointsEarned { get; set; }
        public int PointsSpent { get; set; }
        public DateTime PointsLastUpdated { get; set; }
    }
}
