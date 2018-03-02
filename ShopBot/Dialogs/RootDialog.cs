using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using ShopBot.Models;

namespace ShopBot.Dialogs
{
    [LuisModel("00a372e0-f261-4947-90b3-5fd0329f8164", "ef653df99ee04a58a9f451c8ed519a37")]
    [Serializable]
    public class RootDialog : LuisDialog<object>    
    {
        private const string EntityProducts = "Products";
        private const string EntityBasket = "Basket";
        private const string EntityCheckout = "Checkout";

        [LuisIntent("")]
        [LuisIntent("None")]
        [LuisIntent("Help")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Welcome, how can we help you?");
            await RootActions(context);

            context.Wait(MessageReceived);
        }
        
        [LuisIntent("SelectDialog")]
        public async Task SelectDialog(IDialogContext context, IAwaitable<IMessageActivity> messageActivity, LuisResult result)
        {
            var message = await messageActivity;
            
            if (result.TryFindEntity(EntityProducts, out var _))
            {
                await context.Forward(new ProductDialog(), ResumeAfterProductDialog, message, CancellationToken.None);
            }
            else if (result.TryFindEntity(EntityBasket, out var _))
            {
                await context.Forward(new ManageBasketDialog(), ResumeAfterManageBasketDialog, message,
                    CancellationToken.None);
            }
            else if (result.TryFindEntity(EntityCheckout, out var _))
            {
                await context.Forward(new CheckoutDialog(), ResumeAfterCheckoutDialog, message, CancellationToken.None);
            }
        }

        private static async Task RootActions(IDialogContext context)
        {
            await context.PostAsync(
                "Looking for products? Managing your Basket? Or want to checkout?");
        }

        private async Task ResumeAfterProductDialog(IDialogContext context, IAwaitable<MessageBag<Product>> result)
        {
            var message = await result;
            switch (message.Type)
            {
                case MessageType.ProductOrder:
                    await context.PostAsync($"The user ordered the product \"{message.Content}\"");
                    break;
                case MessageType.ProductRemoval:
                    await context.PostAsync($"The user removed the product {message.Content}");
                    break;
                case MessageType.ProductDialogCancelled:
                    await context.PostAsync("Okay where are we headed next");
                    break;
            }

            await RootActions(context);
            context.Wait(MessageReceived);
        }

        private async Task ResumeAfterManageBasketDialog(IDialogContext context, IAwaitable<object> result)
        {
            await RootActions(context);
        }

        private async Task ResumeAfterCheckoutDialog(IDialogContext context, IAwaitable<object> result)
        {
            //TODO
        }
    }
}