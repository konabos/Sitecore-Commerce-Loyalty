// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Plugin.Konabos.Loyalty
{
    using System.Reflection;
    using global::Plugin.Konabos.Loyalty.Pipelines.Blocks;
    using Microsoft.Extensions.DependencyInjection;
    using Plugin.Konabos.Loyalty.Pipelines;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Carts;
    using Sitecore.Commerce.Plugin.Customers;
    using Sitecore.Commerce.Plugin.Orders;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Pipelines(config => config
               .ConfigurePipeline<IAddCartLinePipeline>(configure => configure.Add<CartLineLoyaltyBlock>().Before<PersistCartBlock>())
               .ConfigurePipeline<IUpdateCartLinePipeline>(configure => configure.Add<CartLineLoyaltyBlock>().Before<PersistCartBlock>())
               .ConfigurePipeline<IOrderPlacedPipeline>(configure => configure.Add<OrderPlacedAssignPointsBlock>().After<OrderPlacedAssignConfirmationIdBlock>())

                .ConfigurePipeline<IGetEntityViewPipeline>(configure => configure.Add<GetLoyaltyPointsOrderViewBlock>())
                .ConfigurePipeline<IGetEntityViewPipeline>(configure => configure.Add<GetLoyaltyPointsOrderLineViewBlock>())
                .ConfigurePipeline<IGetEntityViewPipeline>(c => 
                    {
                        c.Replace<GetCustomerDetailsViewBlock, GetCustomDetailsViewBlock>();
                    })
                .ConfigurePipeline<IUpdateCustomerDetailsPipeline>(c =>
                    {
                        c.Replace<UpdateCustomerDetailsBlock, UpdateCustomDetailsBlock>();
                    })
               );
        }
    }
}