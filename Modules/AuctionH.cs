using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;
using System.Linq;
using KushBot.Resources.Database;

namespace KushBot.Modules
{
    [Group("ah")]
    public class AuctionH : ModuleBase<SocketCommandContext>
    {
        public double Tax = 15;

        [Command("")]
        public async Task help()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithColor(Color.Gold);

            builder.WithTitle("Auction House");

            builder.AddField("Selling an item", $"type ';ah sell itemPrice itemName' (e.g. ;ah sell Ramonators 30). This will put the item in the auction house, you'll get taxed for {Tax}% of the item's actual value");
            builder.AddField("See your Listings", $"Type ';ah list' to see all of your listings");
            builder.AddField("Search for specific item", $"Type ';ah search itemName' (e.g. ;ah search bowler) to look for listing on a specific item");
            builder.AddField("See Random items in AH", $"Type ';ah view' to see 10 randoms items from the ah");
            builder.AddField("Buy an item from AH", $"Type ';ah buy itemName' (e.g. ;ah buy bowler) to buy an item from the AH. this will always buy the cheapest listing");
            builder.AddField("Check Item", "Do ';desc itemName' (e.g. ;desc Ramonators) to see the stats of an item");
            

            await ReplyAsync("", false, builder.Build());
        }

        [Command("sell")]
        public async Task SellItem(int price,[Remainder]string ItemName)
        {
            int count = 0;
            string item = char.ToUpper(ItemName[0]) + ItemName.Substring(1).ToLower();
            if(!Data.Data.GetInventory(Context.User.Id).Contains(item))
            {
                await ReplyAsync($"{Context.User.Mention} You don't have that item Zzzz");
                return;
            }

            if(Data.Data.GetItemValue(item) > Data.Data.GetBalance(Context.User.Id))
            {
                await ReplyAsync($"{Context.User.Mention} too poor to use AH lulwwwww");
                return;
            }

            using(var DbContext = new SqliteDbContext())
            {
                foreach (var nig in DbContext.AH)
                {
                    if(nig.SellerId == Context.User.Id)
                    {
                        count++;
                    }
                }
            }

            if(count >= 24)
            {
                await ReplyAsync($"{Context.User.Mention} You have reached the listings cap of 24.");
                return;
            }

            double value = (Math.Round((double)Data.Data.GetItemValue(item) * (Tax / 100f)));

            await Data.Data.SaveBalance(Context.User.Id, (int)value * -1);

            string[] arr = Data.Data.GetInventory(Context.User.Id).Split(';');
            List<string> inv = arr.ToList();

            inv.Remove(item);

            await Data.Data.AHItem(Context.User.Id, Context.User.Username, item, price);

            await Data.Data.SaveInventory(Context.User.Id, string.Join(';', inv));
            
            if(value <= 0)
            {
                value = 1;
            }

            await ReplyAsync($"{Context.User.Mention} You put your {item} into the AH for {price} baps. You were taxed for {value} baps. Type ';ah list' to check your listings");
        }

        [Command("list")]
        public async Task listing()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{Context.User.Username}'s listings");

            builder.WithColor(Color.Gold);


            using (var DbContext = new SqliteDbContext())
            {
                foreach (Auction auc in DbContext.AH)
                {
                    if(auc.SellerId == Context.User.Id)
                    {
                        builder.AddInlineField($"{auc.Id}. {auc.SellingItem}", $"{auc.ItemPrice} baps");
                    }
                }
            }

            builder.AddField("Check Item","Do ';desc itemName' (e.g. ;desc Ramonators) to see the stats of an item");
            builder.AddField("Remove an item", "Do ';ah remove Id' (Id is the number next to the item's name) (e.g. ;ah remove 7) to remove a listing from the AH");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("view")]
        public async Task viewAh()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Gold);

            builder.WithTitle("Auction house");

            int listItems = 12;

            using (var dbContext = new SqliteDbContext())
            {
                if(dbContext.AH.Count() < listItems)
                {
                    listItems = dbContext.AH.Count();

                }

                if(dbContext.AH.Count() > listItems * 2)
                {
                    Random rad = new Random();
                    for (int i = 0; i < listItems; i++)
                    {
                        int ind = rad.Next(0, dbContext.AH.Count());
                        builder.AddInlineField(dbContext.AH.ToArray()[ind].SellingItem, $"{dbContext.AH.ToArray()[ind].SellerName}'s listing for {dbContext.AH.ToArray()[ind].ItemPrice} baps");
                    }
                }
                else
                {
                    for (int i = 0; i < listItems; i++)
                    {
                        builder.AddInlineField(dbContext.AH.ToArray()[i].SellingItem, $"{dbContext.AH.ToArray()[i].SellerName}'s listing for {dbContext.AH.ToArray()[i].ItemPrice} baps");
                    }
                }      

            }

            builder.AddField("Check Item", "Do ';desc itemName' (e.g. ;desc Ramonators) to see the stats of an item");

            builder.AddField("Buy an item", "Do ';ah buy itemName' (e.g. ;ah buy Magic stick) to buy an item from the ah. This will always buy the cheapest listing.");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("Search")]
        public async Task SearchFor([Remainder]string ItemName)
        {
            EmbedBuilder builder = new EmbedBuilder();
            ItemName = char.ToUpper(ItemName[0]) + ItemName.Substring(1).ToLower();

            builder.WithTitle($"Listings for {ItemName}");

            builder.WithColor(Color.Gold);

            List<Auction> auctions = new List<Auction>();

            using (var dbContext = new SqliteDbContext())
            {
                foreach (var item in dbContext.AH)
                {
                    if(item.SellingItem == ItemName)
                    {
                        auctions.Add(item);
                    }
                }
            }
            SortAuctions(auctions);

            foreach (var item in auctions)
            {
                builder.AddInlineField(item.SellingItem, $"{item.SellerName}'s listing for {item.ItemPrice} baps");
            }

            await ReplyAsync("", false, builder.Build());
            
        }

        [Command("Buy")]
        public async Task BuYitem([Remainder]string ItemName)
        {
            if(Data.Data.GetBusyInventorySpace(Context.User.Id) >= 6 + Data.Data.GetBackpackTier(Context.User.Id) * 3)
            {
                await ReplyAsync($"{Context.User.Mention} You don't have enough backpack space bruh.");
                return;
            }

            EmbedBuilder builder = new EmbedBuilder();
            ItemName = char.ToUpper(ItemName[0]) + ItemName.Substring(1).ToLower();
            builder.WithColor(Color.Gold);

            List<Auction> auctions = new List<Auction>();

            using (var dbContext = new SqliteDbContext())
            {
                foreach (var item in dbContext.AH)
                {
                    if (item.SellingItem == ItemName)
                    {
                        auctions.Add(item);
                    }
                }
            }
            SortAuctions(auctions);

            if(Data.Data.GetBalance(Context.User.Id) < auctions[0].ItemPrice)
            {
                await ReplyAsync($"{Context.User.Mention} You are too poor for this luxury");
                return;
            }

            await Data.Data.SaveBalance(Context.User.Id, auctions[0].ItemPrice * -1);
            await Data.Data.AddItemToBackpack(Context.User.Id, ItemName);

            await Data.Data.SaveBalance(auctions[0].SellerId, auctions[0].ItemPrice);

            await Data.Data.RemoveItemFromAH(auctions[0].Id);

            await ReplyAsync($"{Context.User.Mention} You succesfully bought **{ItemName}** for **{auctions[0].ItemPrice}** baps from the AH");

        }


        public void SortAuctions(List<Auction> auc)
        {
            for (int i = 0; i < auc.Count; i++)
            {
                for (int j = 0; j < auc.Count - 1; j++)
                {
                    if(auc[j].ItemPrice > auc[j + 1].ItemPrice)
                    {
                        Auction temp = auc[j];
                        auc[j] = auc[j + 1];
                        auc[j + 1] = temp;
                    }
                }
            }
        }
        [Command("Remove")]
        public async Task RemoveItem(int index)
        {
            bool contains = false;

            using(var DbContext = new SqliteDbContext())
            {
                foreach (var auc in DbContext.AH)
                {
                    if (auc.SellerId == Context.User.Id)
                    {
                        if(auc.Id == index)
                        {
                            contains = true;
                            break;
                        }
                    }
                }
            }

            if (!contains)
            {
                await ReplyAsync($"{Context.User.Mention} That item is not yours, jewish cuck");
                return;
            }

            await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetAHItemById(index));

            await ReplyAsync($"{Context.User.Mention} Your listing for {Data.Data.GetAHItemById(index)} has been removed");
            await Data.Data.RemoveItemFromAH(index);


        }
    }
}
