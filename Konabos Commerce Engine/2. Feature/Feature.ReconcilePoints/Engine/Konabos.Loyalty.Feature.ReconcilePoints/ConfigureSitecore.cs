// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Commerce.Plugin.Sample
{
    using System.Reflection;
    using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines;
    using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Blocks;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
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


            services.Sitecore().Pipelines(config => config
             .AddPipeline<IReconcilePointsMinionPipeline, ReconcilePointsMinionPipeline>(configure =>
             {
                 configure.Add<RetrieveCustomerBlock>().Add<CalculateCustomerPointsBlock>().Add<UpdateCustomerInxDBBlock>();
             })
            );

            services.RegisterAllCommands(assembly);
        }
    }
}