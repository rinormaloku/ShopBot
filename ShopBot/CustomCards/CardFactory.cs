using System;
using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using ShopBot.Models;

namespace ShopBot.CustomCards
{
    public static class CardFactory
    {
        public const string ProductSearch = "ProductSearch";
        public const string ProductRemoval = "ProductRemoval";
        public const string ProductDialogCancellation = "CancelProductDialog";

        public static AdaptiveCard GetProductActionsCard()
        {
            return new AdaptiveCard
            {
                Body = new List<CardElement>
                {
                    new Container
                    {
                        Items = new List<CardElement>
                        {
                            new ColumnSet
                            {
                                Columns = new List<Column>
                                {
                                    new Column
                                    {
                                        Size = ColumnSize.Auto,
                                        Items = new List<CardElement>
                                        {
                                            new Image
                                            {
                                                Url = $"https://robohash.org/{new Random().NextDouble()}?size=150x150",
                                                Size = ImageSize.Medium,
                                                Style = ImageStyle.Person
                                            }
                                        }
                                    },
                                    new Column
                                    {
                                        Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>
                                        {
                                            new TextBlock
                                            {
                                                Text = "Hey there!",
                                                Weight = TextWeight.Bolder
                                            },
                                            new TextBlock
                                            {
                                                Text = "How can we help you?",
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Actions = new List<ActionBase>
                {
                    new ShowCardAction
                    {
                        Title = "Order Products",
                        Card = GetProductsSearchCard()
                    },
                    new SubmitAction
                    {
                        Title = "Cancel",
                        DataJson = $"{{ \"Type\": \"{ProductDialogCancellation}\" }}"
                    }
                }
            };
        }

        private static AdaptiveCard GetProductsSearchCard()
        {
            return new AdaptiveCard
            {
                Body = new List<CardElement>
                {
                    new TextBlock
                    {
                        Text = "Welcome to the Products finder!",
                        Weight = TextWeight.Bolder,
                        Size = TextSize.Large
                    },
                    new TextBlock {Text = "Please enter the Product you are looking for."},
                    new TextInput
                    {
                        Id = "ProductName",
                        Placeholder = "Bike",
                        Style = TextInputStyle.Text
                    },
                    new TextBlock {Text = "How do you want to sort?"},
                    new ChoiceSet
                    {
                        Id = "Sort",
                        Style = ChoiceInputStyle.Compact,
                        Choices = new List<Choice>
                        {
                            new Choice
                            {
                                IsSelected = true,
                                Title = "Cheapest first",
                                Value = "asc"
                            },
                            new Choice
                            {
                                IsSelected = true,
                                Title = "Most expensive first",
                                Value = "desc"
                            }
                        }
                    },
                    new TextBlock {Text = "Limit search to:"},
                    new NumberInput
                    {
                        Id = "Limit",
                        Min = 1,
                        Max = 60
                    }
                },
                Actions = new List<ActionBase>
                {
                    new SubmitAction
                    {
                        Title = "Search",
                        DataJson = $"{{ \"Type\": \"{ProductSearch}\" }}"
                    }
                }
            };
        }

        public static AdaptiveCard GetProductsBasketCard(IList<Product> products)
        {
            var productCards = new List<CardElement>
            {
                new TextBlock
                {
                    Text = $"You have **{products?.Count ?? 0}** products in your Basket."
                }
            };
            productCards.AddRange((products ?? new List<Product>()).Select(TransformToProductCard).ToList<CardElement>());

            return new AdaptiveCard
            {
                Body = new List<CardElement>
                {
                    new Container
                    {
                        Items = productCards
                    }
                }
            };
        }

        private static ColumnSet TransformToProductCard(Product product)
        {
            return new ColumnSet
            {
                Separation = SeparationStyle.Strong,
                Columns = new List<Column>
                {
                    new Column
                    {
                        Size = ColumnSize.Auto,
                        Items = new List<CardElement>
                        {
                            new Image
                            {
                                Url = $"https://robohash.org/bob{product.Name + new Random().Next()}?size=75x75",
                                Size = ImageSize.Small,
                                Style = ImageStyle.Normal
                            }
                        }
                    },
                    new Column
                    {
                        Items = new List<CardElement>
                        {
                            new TextBlock
                            {
                                Text = product.Name
                            },
                            new TextBlock
                            {
                                Text = $"**${product.ListPrice}**",
                                Wrap = true
                            }
                        }
                    }
                }
            };
        }
    }
}