using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Konabos.Loyalty.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Customers;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    public class GetCustomDetailsViewBlock : Sitecore.Commerce.Plugin.Customers.GetCustomerDetailsViewBlock
    {
        public GetCustomDetailsViewBlock(IGetLocalizedCustomerStatusPipeline getLocalizedCustomerStatusPipeline) : base(getLocalizedCustomerStatusPipeline)
        {
        }

        protected override async Task PopulateDetails(EntityView view, Customer customer, bool isAddAction, bool isEditAction, CommercePipelineExecutionContext context)
        {
            await base.PopulateDetails(view, customer, isAddAction, isEditAction, context);

            if (customer == null)
            {
                return;
            }

            var details = customer.GetComponent<LoyaltyComponent>();

            view.Properties.Add(new ViewProperty
            {
                Name = nameof(LoyaltyComponent.PointsEarned),
                IsRequired = false,
                RawValue = details?.PointsEarned,
                IsReadOnly = true
            });
            view.Properties.Add(new ViewProperty
            {
                Name = nameof(LoyaltyComponent.PointsSpent),
                IsRequired = false,
                RawValue = details?.PointsSpent,
                IsReadOnly = true
            });
        }
    }
}
