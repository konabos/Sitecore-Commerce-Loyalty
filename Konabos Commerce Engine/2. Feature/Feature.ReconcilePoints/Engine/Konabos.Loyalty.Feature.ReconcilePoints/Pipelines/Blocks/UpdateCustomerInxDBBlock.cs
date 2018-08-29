using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Konabos.XConnect.Loyalty.Model.Facets;
using Konabos.XConnect.Loyalty.Model.Models;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using Plugin.Konabos.Loyalty.Components;
using Sitecore.Framework.Pipelines;
using Microsoft.Extensions.Logging;

namespace Konabos.Loyalty.Feature.ReconcilePoints.Pipelines.Blocks
{
    public class UpdateCustomerInxDBBlock : PipelineBlock<Customer, Customer, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IFindEntitiesInListPipeline _findEntitiesInListPipeline;

        public UpdateCustomerInxDBBlock(IPersistEntityPipeline persistEntityPipeline, IFindEntitiesInListPipeline findEntitiesInListPipeline)
        {
            _persistEntityPipeline = persistEntityPipeline;
            _findEntitiesInListPipeline = findEntitiesInListPipeline;
        }

        public override async Task<Customer> Run(Customer customer, CommercePipelineExecutionContext context)
        {
            CertificateWebRequestHandlerModifierOptions options =
                CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=BC9B7186102910E8F34EE8D9F38138203F7555BA");

            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            var collectionClient = new CollectionWebApiClient(new Uri("https://cateringdemo.xconnect.dev.local/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://cateringdemo.xconnect.dev.local/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://cateringdemo.xconnect.dev.local/configuration"), clientModifiers, new[] { certificateModifier });

            var cfg = new XConnectClientConfiguration(
                new XdbRuntimeModel(LoyaltyModel.Model), collectionClient, searchClient, configurationClient);

            try
            {
                await cfg.InitializeAsync();
            }
            catch (XdbModelConflictException ce)
            {
                context.Logger.LogError(string.Format("{0}-Error in UpdateCustomerInxDBBlock connecting to xDB : {1}", (object)this.Name, (object)ce.Message), Array.Empty<object>());
            }
            using (var client = new XConnectClient(cfg))
            {
                try
                {
                    IdentifiedContactReference reference = new IdentifiedContactReference("CommerceUser", string.Concat(customer.Domain, "\\", customer.Email));
                    Contact existingContact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey, LoyaltyPointsFacet.DefaultFacetKey, LoyaltyCommerceFacet.DefaultFacetKey }));

                    if (existingContact != null)
                    {
                        //Add an identifier for the contact with the Commerce Customer Id
                        string identifierSource = "LoyaltyCustomerId";
                        var loyaltyCustomerIdentifier = existingContact.Identifiers.Where(i => i.Source == identifierSource).FirstOrDefault();
                        if (loyaltyCustomerIdentifier == null)
                        {
                            client.AddContactIdentifier(existingContact, new ContactIdentifier(identifierSource, customer.Id.ToString(), ContactIdentifierType.Known));
                            client.Submit();
                        }

                        //Add or Update Loyalty Points for the customer
                        LoyaltyPointsFacet loyaltyPointFacet = existingContact.GetFacet<LoyaltyPointsFacet>(LoyaltyPointsFacet.DefaultFacetKey);
                        if (loyaltyPointFacet == null)
                            loyaltyPointFacet = new LoyaltyPointsFacet();

                        loyaltyPointFacet.PointsEarned = customer.GetComponent<LoyaltyComponent>().PointsEarned;
                        loyaltyPointFacet.PointsSpent = customer.GetComponent<LoyaltyComponent>().PointsSpent;

                        client.SetFacet<LoyaltyPointsFacet>(existingContact, LoyaltyPointsFacet.DefaultFacetKey, loyaltyPointFacet);
                        client.Submit();

                        //Add or Update the Commerce Customer ID
                        LoyaltyCommerceFacet loyaltyCommerceFacet = existingContact.GetFacet<LoyaltyCommerceFacet>(LoyaltyCommerceFacet.DefaultFacetKey);
                        if (loyaltyCommerceFacet == null)
                            loyaltyCommerceFacet = new LoyaltyCommerceFacet();

                        loyaltyCommerceFacet.CommerceCustomerId = customer.Id.ToString();
                        client.SetFacet<LoyaltyCommerceFacet>(existingContact, LoyaltyCommerceFacet.DefaultFacetKey, loyaltyCommerceFacet);
                        client.Submit();
                    }
                    
                }
                catch (XdbExecutionException ex)
                {
                    context.Logger.LogError(string.Format("{0}-Error in UpdateCustomerInxDBBlock updating customer {2} to xDB : {1}", (object)this.Name, (object)ex.Message, customer.Id.ToString()), Array.Empty<object>());
                }
            }
            return customer;
        }
    }
}
