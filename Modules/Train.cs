using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;
using Discord.Rest;
using System.Linq;
using System.Reflection;

namespace KushBot.Modules
{
    public class Train : ModuleBase<SocketCommandContext>
    {

        [Command("Train")]
        public async Task training()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Slabotke's training field");
            builder.WithColor(Color.DarkOrange);

            builder.AddField("Training", "Use the command ';train statName' (e.g. ;train melee) to train that particular skill");
            builder.AddField("Stats and their cost", $"**Melee**: {Data.Data.GetATtackPower(Context.User.Id)} --> {Data.Data.GetATtackPower(Context.User.Id) + 1} Cost: {StyleTraining(Context.User.Id, GetRawAp(Context.User.Id))} baps\n" +
                $"**Ranged**: {Data.Data.GetRangedAttackpower(Context.User.Id)} --> {Data.Data.GetRangedAttackpower(Context.User.Id) + 1} Cost: {StyleTraining(Context.User.Id, GetRawRAP(Context.User.Id))} baps\n" +
                $"**Magic**: {Data.Data.GetSpellPower(Context.User.Id)} --> {Data.Data.GetSpellPower(Context.User.Id) + 1} Cost: {StyleTraining(Context.User.Id, GetRawSp(Context.User.Id))} baps\n" +
                $"**Agility**: {Data.Data.GetAgility(Context.User.Id)} --> {Data.Data.GetAgility(Context.User.Id) + 1} Cost: {AgiTraining(Context.User.Id, GetAgi(Context.User.Id))} baps");

            IMessageChannel dump = Program._client.GetChannel(Program.DumpId) as IMessageChannel;

            RestUserMessage picture = await dump.SendFileAsync($"Pictures/africa.jpg") as RestUserMessage;

            string imgurl = picture.Attachments.First().Url;

            builder.WithImageUrl(imgurl);

            await ReplyAsync("", false, builder.Build());

            await picture.DeleteAsync();
        }


        [Command("Train")]
        public async Task training([Remainder] string stat)
        {

            stat = stat.ToLower();
            if(stat == "agility" || stat == "lethality")
            {
                if (Data.Data.GetBalance(Context.User.Id) < AgiTraining(Context.User.Id, GetAgi(Context.User.Id)))
                {
                    await ReplyAsync($"{Context.User.Mention} cant afford training, poor fuck");
                    return;
                }
            }
            else if(stat == "melee")
            {
                if (Data.Data.GetBalance(Context.User.Id) < StyleTraining(Context.User.Id, GetRawAp(Context.User.Id)))
                {
                    await ReplyAsync($"{Context.User.Mention} cant afford training, poor fuck");
                    return;
                }
            }
            else if (stat == "ranged")
            {
                if (Data.Data.GetBalance(Context.User.Id) < StyleTraining(Context.User.Id, GetRawRAP(Context.User.Id)))
                {
                    await ReplyAsync($"{Context.User.Mention} cant afford training, poor fuck");
                    return;
                }
            }
            else if (stat == "magic")
            {
                if (Data.Data.GetBalance(Context.User.Id) < StyleTraining(Context.User.Id, GetRawSp(Context.User.Id)))
                {
                    await ReplyAsync($"{Context.User.Mention} cant afford training, poor fuck");
                    return;
                }
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} That stat doesnt exist, dyslexia??");
                return;
            }
            MethodInfo mi = GetType().GetMethod(stat);

            mi.Invoke(this, null);

            return;
        
        }
        public async Task agility()
        {
            await Data.Data.SaveAgility(Context.User.Id, Data.Data.GetAgility(Context.User.Id) + 1);
            await Data.Data.SaveBalance(Context.User.Id,-1 * AgiTraining(Context.User.Id, GetAgi(Context.User.Id)));
            await ReplyAsync($"{Context.User.Mention} 'Thanks for feeding our families' - say the african children.");

        }
        public async Task lethality()
        {
            await Data.Data.SaveAgility(Context.User.Id, Data.Data.GetLethality(Context.User.Id) + 1);
            await Data.Data.SaveBalance(Context.User.Id, -1 * AgiTraining(Context.User.Id, GetAgi(Context.User.Id)));
            await ReplyAsync($"{Context.User.Mention} 'Thanks for feeding our families' - say the african children.");

        }
        public async Task melee()
        {
            await Data.Data.SaveAttackPower(Context.User.Id, Data.Data.GetATtackPower(Context.User.Id) + 1);
            await Data.Data.SaveBalance(Context.User.Id, -1 * StyleTraining(Context.User.Id, GetRawAp(Context.User.Id)));
            await ReplyAsync($"{Context.User.Mention} 'Thanks for feeding our families' - say the african children.");
        }
        public async Task ranged()
        {
            await Data.Data.SaveRangedAttackpower(Context.User.Id, Data.Data.GetRangedAttackpower(Context.User.Id) + 1);
            await Data.Data.SaveBalance(Context.User.Id, -1 * StyleTraining(Context.User.Id, GetRawRAP(Context.User.Id)));
            await ReplyAsync($"{Context.User.Mention} 'Thanks for feeding our families' - say the african children.");
        }
        public async Task magic()
        {
            await Data.Data.SaveSpellPower(Context.User.Id, Data.Data.GetSpellPower(Context.User.Id) + 1);
            await Data.Data.SaveBalance(Context.User.Id, -1 * StyleTraining(Context.User.Id, GetRawSp(Context.User.Id)));
            await ReplyAsync($"{Context.User.Mention} 'Thanks for feeding our families' - say the african children.");
        }

        public int AgiTraining(ulong userId, int rawStat)
        {
            int ret = 50;
            double temp = 8 * Math.Pow(rawStat, 1.5);
            ret += (int)Math.Round(temp, 0);

            return ret;
        }


        public int StyleTraining(ulong userId, int rawStat)
        {
            int ret = 50;
            double temp = 6 * Math.Pow(rawStat, 1.2);
            ret += (int)Math.Round(temp, 0);

            return ret;
        }

        public int GetRawAp(ulong userId)
        {
            string mh = Data.Data.GetMainHand(userId);
            string Helm = Data.Data.GetHelmet(userId);
            string Chest = Data.Data.GetChest(userId);
            string Neck = Data.Data.GetNecklace(userId);
            string Goggles = Data.Data.GetGoggles(userId);
            string oh = Data.Data.GetOffHand(userId);

            int ret = Data.Data.GetATtackPower(userId);
            ret -= Data.Data.GetItemApValue(mh);
            ret -= Data.Data.GetItemApValue(Helm);
            ret -= Data.Data.GetItemApValue(Chest);
            ret -= Data.Data.GetItemApValue(Neck);
            ret -= Data.Data.GetItemApValue(Goggles);
            ret -= Data.Data.GetItemApValue(oh);

           
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(mh)) >= 3)
            {
                ret -= Data.Data.SetAp5(Data.Data.ItemSetName(mh));
            }
            else if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetChest(userId))) >= 3)
            {
                ret -= Data.Data.SetAp5(Data.Data.ItemSetName(Data.Data.GetChest(userId)));
            }
            else if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetNecklace(userId))) >= 3)
            {
                ret -= Data.Data.SetAp5(Data.Data.ItemSetName(Data.Data.GetNecklace(userId)));
            }
            else if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetHelmet(userId))) >= 3)
            {
                ret -= Data.Data.SetAp5(Data.Data.ItemSetName(Data.Data.GetHelmet(userId)));
            }

            int deny = 0;
            if (Data.Data.GetClassName(userId) == "Warrior")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3)) + (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) - 1);
            }
            else if (Data.Data.GetClassName(userId) == "Rogue")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3)) + (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) - 1);
            }
            else if (Data.Data.GetClassName(userId) == "Mage")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            else if (Data.Data.GetClassName(userId) == "Archer")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }

            ret -= deny;
            return ret;

        }

        public int GetRawRAP(ulong userId)
        {
            string mh = Data.Data.GetMainHand(userId);
            string Helm = Data.Data.GetHelmet(userId);
            string Chest = Data.Data.GetChest(userId);
            string Neck = Data.Data.GetNecklace(userId);
            string Goggles = Data.Data.GetGoggles(userId);
            string oh = Data.Data.GetOffHand(userId);

            int ret = Data.Data.GetRangedAttackpower(userId);
            ret -= Data.Data.GetItemRapValue(mh);
            ret -= Data.Data.GetItemRapValue(Helm);
            ret -= Data.Data.GetItemRapValue(Chest);
            ret -= Data.Data.GetItemRapValue(Neck);
            ret -= Data.Data.GetItemRapValue(Goggles);
            ret -= Data.Data.GetItemRapValue(oh);

            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetMainHand(userId))) >= 3)
            {
                ret -= Data.Data.SetRap5(Data.Data.ItemSetName(Data.Data.GetMainHand(userId)));
            }
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetChest(userId))) >= 3)
            {
                ret -= Data.Data.SetRap5(Data.Data.ItemSetName(Data.Data.GetChest(userId)));
            }
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetNecklace(userId))) >= 3)
            {
                ret -= Data.Data.SetRap5(Data.Data.ItemSetName(Data.Data.GetNecklace(userId)));
            }
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetHelmet(userId))) >= 3)
            {
                ret -= Data.Data.SetRap5(Data.Data.ItemSetName(Data.Data.GetHelmet(userId)));
            }

            int deny = 0;
            if (Data.Data.GetClassName(userId) == "Warrior")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            else if (Data.Data.GetClassName(userId) == "Rogue")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            else if (Data.Data.GetClassName(userId) == "Mage")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            else if (Data.Data.GetClassName(userId) == "Archer")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3)) + (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) - 1);
            }
            ret -= deny;
            return ret;
        }

        public int GetRawSp(ulong userId)
        {
            string mh = Data.Data.GetMainHand(userId);
            string Helm = Data.Data.GetHelmet(userId);
            string Chest = Data.Data.GetChest(userId);
            string Neck = Data.Data.GetNecklace(userId);
            string Goggles = Data.Data.GetGoggles(userId);
            string oh = Data.Data.GetOffHand(userId);

            int ret = Data.Data.GetSpellPower(userId);
            ret -= Data.Data.GetItemSpValue(mh);
            ret -= Data.Data.GetItemSpValue(Helm);
            ret -= Data.Data.GetItemSpValue(Chest);
            ret -= Data.Data.GetItemSpValue(Neck);
            ret -= Data.Data.GetItemSpValue(Goggles);
            ret -= Data.Data.GetItemSpValue(oh);

            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetMainHand(userId))) >= 3)
            {
                ret -= Data.Data.SetSp5(Data.Data.ItemSetName(Data.Data.GetMainHand(userId)));
            }
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetChest(userId))) >= 3)
            {
                ret -= Data.Data.SetSp5(Data.Data.ItemSetName(Data.Data.GetChest(userId)));
            }
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetNecklace(userId))) >= 3)
            {
                ret -= Data.Data.SetSp5(Data.Data.ItemSetName(Data.Data.GetNecklace(userId)));
            }
            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetHelmet(userId))) >= 3)
            {
                ret -= Data.Data.SetSp5(Data.Data.ItemSetName(Data.Data.GetHelmet(userId)));
            }

            int deny = 0;
            if (Data.Data.GetClassName(userId) == "Warrior")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            else if (Data.Data.GetClassName(userId) == "Rogue")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            else if (Data.Data.GetClassName(userId) == "Mage")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3)) + (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) - 1);
            }
            else if (Data.Data.GetClassName(userId) == "Archer")
            {
                deny = (int)(Math.Truncate((double)Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) / 3));
            }
            ret -= deny;

            return ret;
        }

        public int GetAgi(ulong userId)
        {
            string mh = Data.Data.GetMainHand(userId);
            string Helm = Data.Data.GetHelmet(userId);
            string Chest = Data.Data.GetChest(userId);
            string Neck = Data.Data.GetNecklace(userId);
            string Goggles = Data.Data.GetGoggles(userId);
            //string oh = Data.Data.get(userId);

            int ret = Data.Data.GetAgility(userId);
            ret -= Data.Data.GetItemAGIValue(mh);
            ret -= Data.Data.GetItemAGIValue(Helm);
            ret -= Data.Data.GetItemAGIValue(Chest);
            ret -= Data.Data.GetItemAGIValue(Neck);
            ret -= Data.Data.GetItemAGIValue(Goggles);
            //ret -= Data.Data.GetItemAGIValue(oh);

            int deny = (int)Math.Truncate((double)ret / 5);
            
            ret -= deny;


            if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetMainHand(userId))) >= 3)
            {
                ret -= Data.Data.SetAgi5(Data.Data.ItemSetName(Data.Data.GetMainHand(userId)));
            }
            else if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetChest(userId))) >= 3)
            {
                ret -= Data.Data.SetAgi5(Data.Data.ItemSetName(Data.Data.GetChest(userId)));
            }
            else if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetNecklace(userId))) >= 3)
            {
                ret -= Data.Data.SetAgi5(Data.Data.ItemSetName(Data.Data.GetNecklace(userId)));
            }
            else if (Program.SetItemCount(userId, Data.Data.ItemSetName(Data.Data.GetHelmet(userId))) >= 3)
            {
                ret -= Data.Data.SetAgi5(Data.Data.ItemSetName(Data.Data.GetHelmet(userId)));
            }

            return ret;
        }
    }
}
