using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;
using Discord.Rest;

namespace KushBot.Modules
{
    [Group("help")]
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("")]
        public async Task CallHelp()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Main Help page");

            builder.WithColor(Color.Teal);
            builder.AddField("Money", $"The currency used in {Program.WorldName} is called *baps*, to learn where you can spend it type ';help money'");
            builder.AddField("Combat", $"This bot is mainly combat-oriented, for information about combat do ';help combat'");
            builder.AddField("inventory", $"From time to time you might get some items into your backpack, learn how to deal with them by typing ';help inv'");
            builder.AddField("Professions", $"You can learn/train professions that will help you in various of ways, for more info type ';prof'");


            await ReplyAsync("", false, builder.Build());
        }

        [Command("money")]
        public async Task CallHelpMoney()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Teal);

            builder.WithTitle("Currency help");
            builder.AddField("**Balance**", "Use the command ';inv' to check your balance (the amount of baps you own) along with your items.");
            builder.AddField("**Training**", "You can buy a personal african american to train you by typing ';train'. The blacks used to work for free but that damned zygizmund changed these lands");
            builder.AddField("**Buying items**", "You can buy items from a vendor by typing ';shop'");
            builder.AddField("**Selling items**", "You can sell your items to a vendor by typing ';sell itemName' (e.g. ;sell nigger's flaming sword)");
            builder.AddField("**Using the Auction house**", "You can sell/buy your items to other players in the AH, type ';ah' for more info.");

            await ReplyAsync("", false, builder.Build());

        }


        [Command("inv")]
        public async Task CallHelpInv()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Teal);
            builder.WithTitle("Inventory help");

            builder.AddField("**Inventory**", "Whenever you get an item added to your inventory use the command ';inv' to open it up and read more about the item.");
            builder.AddField("**Getting more inventory space**", "When you first start out, you'll have a total of 9 inventory space. You can increase it by equipping better backpacks.");
            builder.AddField("**Gear**", "You can see the stats of all your equipped items by typing ';gear'");
            builder.AddField("**Equiping**", "You can equip items by typing ';equip itemName' (e.g. ';equip nigger's flaming sword).");
            builder.AddField("**Equiping To offhand**", "To equip an item to an offhand (rogues and warriors need this the most) type ';equip oh itemName' (e.g ;equip oh retarded dagger) " +
                "Do note that **Only onehand and offhand items** can be equipped in the offhand slot.");
            builder.AddField("**Dequiping**", "You can dequip your gear by typing ';dequip itemSlot' (e.g. ;unequip Mainhand or ;unequip helmet)");
            builder.AddField("**ItemSlots**", "The item slots are as follows: **Helmet, Goggles, Necklace, Chest, Mainhand, Offhand, Backpack**");

            await ReplyAsync("", false, builder.Build());

        }

        [Command("combat")]
        public async Task CallHelpCombat()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Teal);

            builder.WithTitle("Combat help");
            builder.AddField("**Status**", "You can see your Ramon's stats aswell as his portrait and what's he doing at that moment by typing ';status'");
            builder.AddField("**Stats explanation**", "**Attack power** - Increases your **melee** damage. **Ranged attack power** - Increases your **ranged** damage. **Spell power** increases your **magic** damage. " +
                "**armor** increases your damage absorption (negates a percentage of damage). **Agility** increases your chance to dodge (evade an enemy's attack), also increases off-hand hit chance for rogues");
            builder.AddField("**Zone-Attacking**", "The simplest form of combat is the zone-attacking, type ';attack' to get the list of zones and their recommended levels. You " +
                "can attack the zone by typing ';attack zoneName' (e.g. ;attack weeb keep)");


            await ReplyAsync("", false, builder.Build());

        }
    }
}
