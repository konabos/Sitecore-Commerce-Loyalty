using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines;
using Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Arguments;
using Konabos.XConnect.Loyalty.Model.Facets;
using Konabos.XConnect.Loyalty.Model.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Minions
{
    public class ReconcilePointsMinion : Minion
    {
        protected IReconcilePointsMinionPipeline ReconcilePointsMinionPipeline { get; set; }

        public override void Initialize(IServiceProvider serviceProvider,
            ILogger logger,
            MinionPolicy policy,
            CommerceEnvironment environment,
            CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            ReconcilePointsMinionPipeline = serviceProvider.GetService<IReconcilePointsMinionPipeline>();
        }

        public override async Task<MinionRunResultsModel> Run()
        {
            MinionRunResultsModel runResults = new MinionRunResultsModel();

            var customers = (await this.GetListItems<Customer>("Customers", 20, 0));
            if (customers != null && customers.Any())
            {
                foreach (var customer in customers)
                {
                    this.Logger.LogDebug(string.Format("{0}-Reviewing Customer: {1}", (object)this.Name, (object)customer.Id), Array.Empty<object>());

                    var reconcilePointsArgument = new ReconcilePointsArgument(customer.Id);

                    var commerceContext = new CommerceContext(this.Logger, this.MinionContext.TelemetryClient, (IGetLocalizableMessagePipeline)null);
                    commerceContext.Environment = this.Environment;

                    var executionContextOptions = new CommercePipelineExecutionContextOptions(commerceContext, null, null, null, null, null);

                    var order = await this.ReconcilePointsMinionPipeline.Run(reconcilePointsArgument, executionContextOptions);
                }
            }
            return runResults;
        }
    }
}
