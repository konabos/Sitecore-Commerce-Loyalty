using System;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;
using Plugin.Konabos.Loyalty.Components;
using Sitecore.Framework.Conditions;

namespace Plugin.Konabos.Loyalty.Pipelines.Blocks
{
    public class CartLineLoyaltyBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        public override Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The cart argument can not be null");

            var cartLine = context.CommerceContext.GetObjects<CartLineArgument>().First();
            CartLineComponent addedLine = arg.Lines.FirstOrDefault<CartLineComponent>(line => line.Id.Equals(cartLine.Line.Id, StringComparison.OrdinalIgnoreCase));

            var loyalty = addedLine.GetComponent<LoyaltyComponent>();
            loyalty.PointsEarned = (int)addedLine.Totals.SubTotal.Amount;
            return Task.FromResult(arg);
        }
    }
}
