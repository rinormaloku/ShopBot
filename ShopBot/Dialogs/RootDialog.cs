using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;
using ShopBot.Dialogs;
using ShopBot.Models;

namespace ShopBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.Contains("products"))
            {
                await context.Forward(new ProductDialog(), ResumeAfterProductDialog, message, CancellationToken.None);
            }
            else if (message.Text.Contains("basket"))
            {
                await context.Forward(new ManageBasketDialog(), ResumeAfterManageBasketDialog, message,
                    CancellationToken.None);
            }
            else if (message.Text.Contains("checkout"))
            {
                await context.Forward(new CheckoutDialog(), ResumeAfterCheckoutDialog, message, CancellationToken.None);
            }
            else
            {
                await context.PostAsync("Welcome, how can we help you?");
                await RootActions(context);
                context.Wait(MessageReceivedAsync);
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
            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterManageBasketDialog(IDialogContext context, IAwaitable<object> result)
        {
            //TODO
        }

        private async Task ResumeAfterCheckoutDialog(IDialogContext context, IAwaitable<object> result)
        {
            //TODO
        }
    }
}