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
    }
}