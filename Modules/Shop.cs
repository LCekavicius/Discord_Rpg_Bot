using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;
using Discord.Rest;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using Discord;

namespace KushBot.Migrations
{
    public class Shop : ModuleBase<SocketCommandContext>
    {
        public int constModifier = 5;

        [Command("shop")]
        public async Task sohp()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("There are multiple shops from which you can buy shid");
            builder.AddField("Gear", "do ';shop gear' to open up the gear shop where you can buy equipment. There is a limited number of items in this shop, once someone buys something - it's gone. The shop refills every 8 hours");
            builder.AddField("Misc", "do ';shop misc' to open up BS shop where you can buy food, potions, materials, upgrade your backpack. There are unlimited items in this shop");
            builder.WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build());
        }


        [Command("shop Gear")]
        public async Task shop()
        {

            EmbedBuilder builder = new EmbedBuilder();
            int ItemCostModifier = constModifier;

            builder.WithTitle($"Welcome to {randomName()}'s shop");

            int lvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            int index = (int)Math.Truncate((double)lvl / 10);
            index *= 10;

            string ItemString = Data.Data.GetShopItems(index);

            string[] arr;
            arr = ItemString.Split(';');

            foreach (string item in arr)
            {
                if (Data.Data.GetItemRarity(item) == "uncommon")
                {
                    ItemCostModifier++;
                }
                else if (Data.Data.GetItemRarity(item) == "rare")
                {
                    ItemCostModifier+= 2;
                }
                builder.AddInlineField($"{item} {Program.getEmoji(Data.Data.GetItemType(item), Data.Data.GetItemStyle(item))}", $"{Program.ItemDescription(item, Context.User.Id)}\n**Purchase for: {Data.Data.GetItemValue(item) * ItemCostModifier} baps**");
                ItemCostModifier = constModifier;
            }

            TimeSpan ts = Data.Data.GetRefillDate(index).AddHours(8) - DateTime.Now;

            builder.AddField("Refill",$"New stock in: **{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}**");

            builder.AddField("Buying", "To buy an item, type ';buy Gear itemName' (e.g. ;buy gear autochain)"); 
            builder.WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build());
        }


        [Command("shop misc")]
        public async Task shopmisc()
        {
            EmbedBuilder builder = new EmbedBuilder();

            int ItemCostModifier = constModifier;

            builder.WithTitle($"Welcome to {randomName()}'s shop");

            int lvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            int index = (int)Math.Truncate((double)lvl / 10);
            index *= 10;

            string ItemString = Data.Data.GetUnlimitedShopItems(index);

            string[] arr;
            arr = ItemString.Split(';');

            foreach (string item in arr)
            {
                builder.AddInlineField($"{item} {Program.getEmoji(Data.Data.GetItemType(item), Data.Data.GetItemStyle(item))}", $"{Program.ItemDescription(item, Context.User.Id)}\n**Purchase for: {Data.Data.GetItemValue(item) * ItemCostModifier} baps**");
            }
            builder.AddInlineField($"Backpack Upgrade :handbag:", $"Upgrades your backpack to **Tier {Data.Data.GetBackpackTier(Context.User.Id) + 1}**\n +3 inventory slots\n **Purchase for: {Math.Pow(Data.Data.GetBackpackTier(Context.User.Id), 2) * 125}**");

            builder.AddField("Buying", "To buy an item, type ';buy misc itemName' (e.g. ;buy gear bread)");
            builder.WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build());
        }

        public string randomName()
        {
            Random rad = new Random();

            List<string> names = new List<string>();

            names.Add("Zkoomater");
            names.Add("Asswipe");
            names.Add("Laimonini");
            names.Add("Shellman");
            names.Add("Mant");
            names.Add("Rattic");
            names.Add("Fuck");
            names.Add("Baggy");
            names.Add("Peter");
            names.Add("Gregor");
            names.Add("NigNog");
            names.Add("YellowMan");
            names.Add("Zkoomater");

            return names[rad.Next(0, names.Count - 1)];
        }
        [Command("buy gear")]
        public async Task BuyItem([Remainder] string Item)
        {
            int lvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            int ItemCostModifier = constModifier;

            int index = (int)Math.Truncate((double)lvl / 10);
            index *= 10;

            string ItemString = Data.Data.GetShopItems(index);

            string[] arr;
            arr = ItemString.Split(';');

            List<string> Shopitems = arr.ToList();

            string BoughtItem = "";

            for (int i = 0; i < Shopitems.Count; i++)
            {
                if(Shopitems[i].ToLower() == Item.ToLower())
                {
                    BoughtItem = Shopitems[i];
                    Shopitems.Remove(Shopitems[i]);
                    break;
                }
            }

            if (BoughtItem == "")
            {
                await ReplyAsync($"{Context.User.Mention} we don't have that item, moron");
                return;
            }

            if (Data.Data.GetItemRarity(BoughtItem) == "uncommon")
            {
                ItemCostModifier++;
            }
            else if (Data.Data.GetItemRarity(BoughtItem) == "rare")
            {
                ItemCostModifier+=2;
            }
            if (Data.Data.GetItemValue(BoughtItem) * ItemCostModifier > Data.Data.GetBalance(Context.User.Id))
            {
                await ReplyAsync($"{Context.User.Mention} you don't have enough baps, missed out on math classes?");
                return;
            }

            List<string> inventory = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();
            if(inventory.Count >= Data.Data.GetBackpackTier(Context.User.Id) * 3 + 6)
            {
                await ReplyAsync($"{Context.User.Mention} Your inventory is full.");
                return;
            }

            await Data.Data.SaveBalance(Context.User.Id, -1 * Data.Data.GetItemValue(BoughtItem) * ItemCostModifier);
            await Data.Data.AddItemToBackpack(Context.User.Id, BoughtItem);

            string LimitedItems = string.Join(';', Shopitems.ToArray());

            await Data.Data.SaveShopLimitedItems(index, LimitedItems);

            await ReplyAsync($"{Context.User.Mention} Succesfully bought a {BoughtItem} for {Data.Data.GetItemValue(BoughtItem) * ItemCostModifier} baps");

        }

        [Command("buy misc")]
        public async Task BuyItemmisc([Remainder] string Item)
        {
            if(Item.ToLower() == "backpack upgrade")
            {
                int cost = (int)Math.Pow(Data.Data.GetBackpackTier(Context.User.Id), 2) * 125;

                if (Data.Data.GetBalance(Context.User.Id) < cost)
                {
                    await ReplyAsync($"{Context.User.Mention} Not enough baps, depression?");
                    return;
                }
                await Data.Data.SaveBackpackTier(Context.User.Id, Data.Data.GetBackpackTier(Context.User.Id) + 1);
                await ReplyAsync($"{Context.User.Mention} Succesfully upgraded backpack to Tier {Data.Data.GetBackpackTier(Context.User.Id)}");
                await Data.Data.SaveBalance(Context.User.Id, -1 * cost);

                return;
            }

            int ItemCostModifier = constModifier;

            int lvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            int index = (int)Math.Truncate((double)lvl / 10);
            index *= 10;

            string ItemString = Data.Data.GetUnlimitedShopItems(index);

            string[] arr;
            arr = ItemString.Split(';');

            List<string> Shopitems = arr.ToList();

            string BoughtItem = "";

            for (int i = 0; i < Shopitems.Count; i++)
            {
                if (Shopitems[i].ToLower() == Item.ToLower())
                {
                    BoughtItem = Shopitems[i];
                    break;
                }
            }

            if (BoughtItem == "")
            {
                await ReplyAsync($"{Context.User.Mention} we don't have that item, moron");
                return;
            }

            if (Data.Data.GetItemValue(BoughtItem) * ItemCostModifier > Data.Data.GetBalance(Context.User.Id))
            {
                await ReplyAsync($"{Context.User.Mention} you don't have enough baps, missed out on math classes?");
                return;
            }

            await Data.Data.SaveBalance(Context.User.Id, -1 * Data.Data.GetItemValue(BoughtItem) * ItemCostModifier);
            await Data.Data.AddItemToBackpack(Context.User.Id, BoughtItem);

            await ReplyAsync($"{Context.User.Mention} Succesfully bought a {BoughtItem} for {Data.Data.GetItemValue(BoughtItem) * ItemCostModifier} baps");

        }
    }
}
