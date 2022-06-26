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
    public class Status : ModuleBase<SocketCommandContext>
    {
        [Command("status")]
        public async Task Statuss()
        {
            EmbedBuilder builder = new EmbedBuilder();

            int lvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            builder.WithColor(Color.Orange);
            builder.WithTitle($"{Context.User.Username} the Level {lvl} {Data.Data.GetClassName(Context.User.Id)} Ramon");

            int AP = Data.Data.GetATtackPower(Context.User.Id);
            int RAP = Data.Data.GetRangedAttackpower(Context.User.Id);
            int SP = Data.Data.GetSpellPower(Context.User.Id);


            int BottomEndAP = 1;
            int TopEndAP = 2;

            int BottomEndRAP = 1;
            int TopEndRAP = 2;

            int BottomEndSP = 1;
            int TopEndSP = 2;

            string mh = Data.Data.GetMainHand(Context.User.Id);
            string oh = Data.Data.GetOffHand(Context.User.Id);

            if (mh != "Empty" || mh != "")
            {
                if (Data.Data.GetItemStyle(mh) == "Melee")
                {
                    BottomEndAP = Data.Data.GetItemBottomEnd(mh);
                    TopEndAP = Data.Data.GetItemTopEnd(mh);
                }
                else if (Data.Data.GetItemStyle(mh) == "Ranged")
                {
                    BottomEndRAP = Data.Data.GetItemBottomEnd(mh);
                    TopEndRAP = Data.Data.GetItemTopEnd(mh);
                }
                else if (Data.Data.GetItemStyle(mh) == "Magic")
                {
                    BottomEndSP = Data.Data.GetItemBottomEnd(mh);
                    TopEndSP = Data.Data.GetItemTopEnd(mh);
                }
            }


            int CurHp = Data.Data.GetCurHealth(Context.User.Id);
            int MaxHp = Data.Data.GetMaxHealth(Context.User.Id);

            TimeSpan ts = Data.Data.GetLastRegen(Context.User.Id).AddMinutes(30) - DateTime.Now;

            if (CurHp <= 0)
            {
                TimeSpan DT = Data.Data.GetDeathTime(Context.User.Id).AddHours(1) - DateTime.Now;

                builder.AddInlineField(":skull: HP", $"**DEAD** Undies in: {DT.Minutes:D2}:{DT.Seconds:D2}");
            }
            else if(CurHp < (int)Math.Round(MaxHp * 0.2))
            {
                builder.AddInlineField(":broken_heart: HP", $"{CurHp}/{MaxHp} Next Regen: {ts.Minutes:D2}:{ts.Seconds:D2}");
            }
            else if(CurHp == MaxHp)
            {
                builder.AddInlineField(":heart: HP", $"{CurHp}/{MaxHp}");
            }
            else
            {
                builder.AddInlineField(":heart: HP", $"{CurHp}/{MaxHp} Next Regen: {ts.Minutes:D2}:{ts.Seconds:D2}");

            }

            if (oh != "Empty" && Data.Data.GetClassName(Context.User.Id) == "Rogue")
            {
                builder.AddInlineField(":crossed_swords: Melee Dmg", $"Hit: **{Program.BottomEnd(BottomEndAP, AP)}-{Program.TopEnd(TopEndAP, AP)}** Attack power: **{AP}**\n" +
                    $"OH Hit: **{Program.BottomEnd(Data.Data.GetItemBottomEnd(oh), AP)}-{Program.TopEnd(Data.Data.GetItemTopEnd(oh), AP)}**");
            }
            else if(oh == "Empty" && Data.Data.GetClassName(Context.User.Id) == "Rogue")
            {
                builder.AddInlineField(":crossed_swords: Melee Dmg", $"Hit: **{Program.BottomEnd(BottomEndAP, AP)}-{Program.TopEnd(TopEndAP, AP)}** Attack power: **{AP}**\n" +
                    $"OH Hit: **1-2**");
            }
            else
            {
                builder.AddInlineField(":crossed_swords: Melee Dmg", $"Hit: **{Program.BottomEnd(BottomEndAP, AP)}-{Program.TopEnd(TopEndAP, AP)}** Attack power: **{AP}**");
            }

            builder.AddInlineField($":bow_and_arrow: Range Dmg", $"Hit: **{Program.BottomEnd(BottomEndRAP, RAP)}-{Program.TopEnd(TopEndRAP, RAP)}** Rang Att. pow: **{RAP}**");
            builder.AddInlineField($":crystal_ball: Magic Dmg", $"Hit: **{Program.BottomEnd(BottomEndSP, SP)}-{Program.TopEnd(TopEndSP, SP)}** Spell power: **{SP}**");
           /* if (Context.Client.GetGuild(337945443252305920) != null || Context.Client.GetGuild(490889121846263808) != null)
            {
                if(Data.Data.GetClassName(Context.User.Id) == "Rogue")
                {
                    builder.AddInlineField($"<a:awooga:519192806854623262> Agility", $"Agi: **{Data.Data.GetAgility(Context.User.Id)}** Dodge chance: **{Program.GetDodgeChance(Data.Data.GetAgility(Context.User.Id))}%**   " +
                        $"\nOffhand hit chance **{Program.GetOhHitChance(Data.Data.GetAgility(Context.User.Id))}%**");
                }
                else
                {
                    builder.AddInlineField($"<a:awooga:519192806854623262> Agility", $"Agi: **{Data.Data.GetAgility(Context.User.Id)}** Dodge chance: **{Program.GetDodgeChance(Data.Data.GetAgility(Context.User.Id))}%**");
                }
            }*/
           // else
            {
                if (Data.Data.GetClassName(Context.User.Id) == "Rogue")
                {
                    builder.AddInlineField($":dash: Agility", $"Agi: **{Data.Data.GetAgility(Context.User.Id)}** Dodge chance: **{Program.GetDodgeChance(Data.Data.GetAgility(Context.User.Id))}%**   " +
                        $"\nOffhand hit chance **{Program.GetOhHitChance(Data.Data.GetAgility(Context.User.Id))}%**");
                }
                else
                {
                    builder.AddInlineField($":dash: Agility", $"Agi: **{Data.Data.GetAgility(Context.User.Id)}** Dodge chance: **{Program.GetDodgeChance(Data.Data.GetAgility(Context.User.Id))}%**");

                }
            }
            builder.AddInlineField($":boom: Lethality", $"Leth: **{Data.Data.GetLethality(Context.User.Id)}** Crit Chance: **{Program.GetCritChance(Data.Data.GetLethality(Context.User.Id))}%**\nCrit damage: **{Program.GetCritDamage(Data.Data.GetLethality(Context.User.Id))}%**");
            builder.AddInlineField($":shield: Armor", $"Arm: **{Data.Data.GetArmor(Context.User.Id)}** Dmg Absorb: **{Program.GetDmgAbsorb(Data.Data.GetArmor(Context.User.Id))}%**");

            builder.AddInlineField($":green_book: Experience", $"Total Exp: **{CoolFormat(Data.Data.GetExperience(Context.User.Id))}** next lvl in: **{CoolFormat(Program.NextLevelIn(Data.Data.GetExperience(Context.User.Id), lvl))}**");


            if (Data.Data.GetWorkIndex(Context.User.Id) != "neet")
            {
                TimeSpan wts = DateTime.Now - Data.Data.GetWorkDate(Context.User.Id);
                builder.AddField(":hammer_pick: Working", $"Currently is **{Data.Data.GetWorkIndex(Context.User.Id)}**\nTime elapsed: {wts.Hours:D2}:{wts.Minutes:D2}:{wts.Seconds}\ntype ';stop' to stop {Data.Data.GetWorkIndex(Context.User.Id)}");

            }


            IMessageChannel dump = Program._client.GetChannel(Program.DumpId) as IMessageChannel;

            RestUserMessage picture;

            if (Program.BotTesting)
            {
                picture = await dump.SendFileAsync($@"D:\KushBot\Kush Bot\KushBotV2\KushBot\Data\Portraits\{Context.User.Id}.png") as RestUserMessage;
                
            }
            else
            {
                picture = await dump.SendFileAsync($@"Data/Portraits/{Context.User.Id}.png") as RestUserMessage;
            }



            string imgurl = picture.Attachments.First().Url;

            builder.WithImageUrl(imgurl);

            await ReplyAsync("", false, builder.Build());
        }

        public string CoolFormat(int sn)
        {
            string ret = "";
            int inc = 0;

            double n = sn;

            while(n / 1000 >= 1)
            {
                n /= 1000;
            }

            if(sn < 1000)
            {
                ret += $"{n}";
            }
            else if(sn < 100000)
            {
                ret += $"{Math.Round(n,1)}k";
            }
            else if(sn < 1000000)
            {
                ret += $"{Math.Round(n, 0)}k";
            }
            else if(sn < 100000000)
            {
                ret += $"{Math.Round(n, 1)}m";
            }
            else
            {
                ret += $"{Math.Round(n, 0)}m";
            }
            return ret;
        }
    }
}
