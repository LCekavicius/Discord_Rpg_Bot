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
    public class Attack : ModuleBase<SocketCommandContext>
    {
        IMessageChannel dump = Program._client.GetChannel(Program.DumpId) as IMessageChannel;

        /*[Command("Attack")]
        public async Task att([Remainder]string place)
        {
            place = place.Replace(" ",string.Empty);
            place = place.ToLower();

            MethodInfo mi = GetType().GetMethod(place);

            try
            {
                mi.Invoke(this, null);
            }
            catch
            {
                await ReplyAsync($"{Context.User.Mention} That zone doesn't exist, are you partially braindead?");
            }


        }*/
        [Command("Attack")]
        public async Task att()
        {
            await ReplyAsync($"{Context.User.Mention} There are different Zones which you can attack for experience and loot. Here are the zones:\n" +
                $"1. Weeb keep (recommended level: none)\n" +
                $"2. Mojang (recommended level: 10)\n" +
                $"3. Plains of Allah (recommended level: 20)\n" +
                $"4. The dark forest (recommended level: 30)\n" +
                $"Use ';attack zoneName' (e.g. ';attack Weeb keep') to attack the zone");
        }

        [Command("Attack")]
        public async Task attt([Remainder] string zone)
        {
            Random Rad = new Random();
            int MobIndMult = MobIndexMultiplier(zone);
            if(MobIndMult == -1)
            {
                await ReplyAsync($"{Context.User.Mention} That zone doesn't exist, are you partially braindead?");
                return;
            }

            List<int> Attackable = new List<int>();

            int Ramonlevel = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            for (int i = 1 * MobIndMult; i <= 6 * MobIndMult; i++)
            {

                if (Rad.NextDouble() < Chance(Data.Data.GetMobLevel(i)) && Rad.NextDouble() < ReverseChance(Data.Data.GetMobLevel(i)))
                {
                    Attackable.Add(i);
                }
            }

            if (Attackable.Count == 0)
            {
                Attackable.Add(6 * MobIndMult);
            }

            int temp = Rad.Next(1, Attackable.Count + 1);

            await Fight(Attackable[temp - 1]);
        }

        public int MobIndexMultiplier(string zoneName)
        {
            string zone = zoneName.ToLower();
            zone = zone.Replace(" ", string.Empty);

            switch (zone)
            {
                case "weebkeep":
                    return 1;
                case "mojang":
                    return 2;
                case "fieldsofallah":
                    return 3;
                case "darkforest":
                    return 4;
            }
            return -1;

        }

        public double Chance(int moblvl)
        {
            double ret;
            int pLvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));
            if(moblvl <= pLvl)
            {
                return 1;
            }
            ret = 1 / Math.Pow((2.25 - pLvl / 100), (moblvl - pLvl + (1 - pLvl * 0.005)));
            return ret;
        }

        public double ReverseChance(int moblvl)
        {
            double ret;
            int pLvl = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            if(pLvl - 4 <= moblvl)
            {
                return 1;
            }

            ret = 1 / ((double)pLvl - (double)moblvl);

            return ret;

        }

        public async Task weebkeep()
        {
            Random Rad = new Random();

            List<int> Attackable = new List<int>();

            int Ramonlevel = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            for(int i = 1; i <= 6;i++)
            {
                if (Rad.NextDouble() < Chance(Data.Data.GetMobLevel(i)))
                {
                    Attackable.Add(i);
                }
            }

            if(Attackable.Count == 0)
            {
                Attackable.Add(1);
            }

            int temp = Rad.Next(1, Attackable.Count +1);


            await Fight(Attackable[temp - 1]);

        }
        public async Task mojang()
        {
            Random Rad = new Random();

            List<int> Attackable = new List<int>();


            int Ramonlevel = Program.GetLevel(Data.Data.GetExperience(Context.User.Id));

            for (int i = 1 + 6; i <= 6 + 6; i++)
            {
                if (Rad.NextDouble() < Chance(Data.Data.GetMobLevel(i)))
                {
                    Attackable.Add(i);
                }
                break;
            }


            if (Attackable.Count == 0)
            {
                Attackable.Add(1 + 6);
            }

            int temp = Rad.Next(1, Attackable.Count + 1);


            await Fight(Attackable[temp - 1]);
        }


        public async Task Fight(int MobId)
        {
            Mob mob = Data.Data.GetMob(MobId);
            EmbedBuilder BattleLog = new EmbedBuilder();

            EmbedBuilder builder = new EmbedBuilder();

            BattleLog.WithColor(Color.DarkGreen);

            builder.WithTitle($"{Context.User.Username} Encountered a level {mob.Level} {mob.Name}");

            RestUserMessage picture = await dump.SendFileAsync($"Pictures/Mobs/{mob.Name.ToLower()}.jpg") as RestUserMessage;
            string imgurl = picture.Attachments.First().Url;


            builder.WithImageUrl(imgurl);

            int BottomEnd = 1;
            int TopEnd = 2;

            int OhBottomEnd = 1;
            int OhTopEnd = 2;

            int RamonDmg = 0;
            int OhRamonDmg = 0;
            int mobDmg = 0;

            int DmgGained = 0;

            Random rad = new Random();

            bool first = true;
            bool MobFirst = mob.Agility > Data.Data.GetAgility(Context.User.Id);

            while (true)
            {
                if (!first || MobFirst)
                {

                    if (rad.NextDouble() < Program.GetDodgeChance(Data.Data.GetAgility(Context.User.Id)) / 100)
                    {
                        BattleLog.AddField($"{mob.Name}'s turn", $"misses");

                    }
                    else
                    {
                        bool crit = false;
                        if (rad.NextDouble() < Program.GetCritChance(mob.Lethality) / 100f)
                        {
                            int bend = (int)Math.Round(mob.BottomEnd * ((double)Program.GetCritDamage(mob.Lethality) / 100f));
                            int tend = (int)Math.Round(mob.TopEnd * ((double)Program.GetCritDamage(mob.Lethality) / 100f));
                            mobDmg = MobTurn(bend, tend, Data.Data.GetArmor(Context.User.Id));
                            crit = true;
                        }
                        else
                        {
                            mobDmg = MobTurn(mob.BottomEnd, mob.TopEnd, Data.Data.GetArmor(Context.User.Id));

                        }

                        await Data.Data.Regenerate(Context.User.Id, -1 * mobDmg);

                        DmgGained += mobDmg;

                        if (crit)
                        {
                            BattleLog.AddField($"{mob.Name}'s turn", $"{mob.Name} dealt a critical hit of {mobDmg} damage. {Context.User.Username} now has {Data.Data.GetCurHealth(Context.User.Id)} HP");
                        }
                        else
                        {
                            BattleLog.AddField($"{mob.Name}'s turn", $"{mob.Name} dealt {mobDmg} damage. {Context.User.Username} now has {Data.Data.GetCurHealth(Context.User.Id)} HP");
                        }


                    }

                    //Mob won management
                    if (Data.Data.GetCurHealth(Context.User.Id) <= 0)
                    {
                        builder.AddInlineField(":heart: HP", $"{mob.CurHealth}/{mob.MaxHealth}");
                        builder.AddInlineField(":crossed_swords: Damage", $"Hit: **{mob.BottomEnd}-{mob.TopEnd}**");
                        builder.AddInlineField(":shield: Armor", $"Arm: **{mob.Armor}** Dmg absorb: {Program.GetDmgAbsorb(mob.Armor)}%");
                        if (Context.Client.GetGuild(337945443252305920) != null || Context.Client.GetGuild(490889121846263808) != null)
                        {
                            builder.AddInlineField($"<a:awooga:519192806854623262> Agility", $"Agi: **{mob.Agility}** Dodge chance: **{Program.GetDodgeChance(mob.Agility)}%**");
                        }
                        else
                        {
                            builder.AddInlineField($":dash:! Agility", $"Agi: {mob.Agility} Dodge chance: {Program.GetDodgeChance(mob.Agility)}%");
                        }

                        builder.AddField($"{Context.User.Username} has fainted!", $"NAYY");
                        builder.WithColor(Color.DarkRed);
                        await Data.Data.SaveDeathTime(Context.User.Id, DateTime.Now);
                        await ReplyAsync("", false, builder.Build());
                        break;
                    }
                    //break
                }

                first = false;

                if (rad.NextDouble() < Program.GetDodgeChance(mob.Agility) / 100)
                {
                    BattleLog.AddField($"{Context.User.Username}'s turn", $"{Context.User.Username} Misses");
                }
                else
                {
                    bool crit = false;
                    
                    if(rad.NextDouble() < Program.GetCritChance(Data.Data.GetLethality(Context.User.Id)) / 100f)
                    {
                        RamonDmg = RamonTurn(ref BottomEnd, ref TopEnd, mob.Armor, true);
                        RamonDmg = (int)Math.Round((double)RamonDmg * (double)(Program.GetCritDamage(Data.Data.GetLethality(Context.User.Id)) / 100f));
                    
                        crit = true;
                    }
                    else
                    {
                        RamonDmg = RamonTurn(ref BottomEnd, ref TopEnd, mob.Armor, true);
                    }

                    mob.CurHealth -= RamonDmg;

                    string ohText = "";

                    if (rad.NextDouble() < (Program.GetOhHitChance(Data.Data.GetAgility(Context.User.Id)) / 100) && Data.Data.GetClassName(Context.User.Id) == "Rogue")
                    {
                        bool ohcrit = false;
                        Console.WriteLine(Program.GetCritChance(Data.Data.GetLethality(Context.User.Id)));
                        if(rad.NextDouble() < Program.GetCritChance(Data.Data.GetLethality(Context.User.Id)) / 100)
                        {
                            OhRamonDmg = RamonTurn(ref OhBottomEnd, ref OhTopEnd, mob.Armor, false);
                            OhRamonDmg = (int)Math.Round((double)OhRamonDmg * (double)(Program.GetCritDamage(Data.Data.GetLethality(Context.User.Id)) / 100f));
                            ohcrit = true;
                        }
                        else
                        {
                            OhRamonDmg = RamonTurn(ref OhBottomEnd, ref OhTopEnd, mob.Armor, false);
                        }
                        mob.CurHealth -= OhRamonDmg;
                        
                        if (ohcrit)
                        {
                            ohText = $"OH Proced and dealt a critical hit of {OhRamonDmg} extra dmg.";
                        }
                        else
                        {
                            ohText = $"OH Proced and dealt {OhRamonDmg} extra dmg.";

                        }
                    }

                        if (crit)
                        {
                            BattleLog.AddField($"{Context.User.Username}'s turn", $"{Context.User.Username} dealt a critical hit of {RamonDmg} damage. {ohText} {mob.Name} now has {mob.CurHealth} HP.");
                        }
                        else
                        {
                            BattleLog.AddField($"{Context.User.Username}'s turn", $"{Context.User.Username} dealt {RamonDmg} damage. {ohText} {mob.Name} now has {mob.CurHealth} HP.");

                        }
                }

                // Player won management
                if (mob.CurHealth <= 0)
                {
                    //int ExpDrop = (int)Math.Round(mob.BaseExpDrop * (1 + (rad.Next(-15, 16) / 100f)));
                    int lvlDiff = Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) - mob.Level;
                    if(lvlDiff < 0)
                    {
                        lvlDiff = 1;
                    }
                    double expDrop = (10 + mob.Level * 2 + mob.MaxHealth / 2) * Math.Pow(0.82, lvlDiff);
                    int ExpDrop = (int)Math.Round(expDrop, 0);
                    //builder
                    builder.AddInlineField(":heart: HP", $"0/{mob.MaxHealth}");
                    builder.AddInlineField(":crossed_swords: Damage", $"Hit: **{mob.BottomEnd}-{mob.TopEnd}**");
                    builder.AddInlineField(":shield: Armor", $"Arm: **{mob.Armor}** Dmg absorb: {Program.GetDmgAbsorb(mob.Armor)}%");
                    if (Context.Client.GetGuild(337945443252305920) != null || Context.Client.GetGuild(490889121846263808) != null)
                    {
                        builder.AddInlineField($"<a:awooga:519192806854623262> Agility", $"Agi: **{mob.Agility}** Dodge chance: **{Program.GetDodgeChance(mob.Agility)}%**");
                    }
                    else
                    {
                        builder.AddInlineField($":dash:! Agility", $"Agi: {mob.Agility} Dodge chance: {Program.GetDodgeChance(mob.Agility)}%");
                    }

                    builder.AddField($"{Context.User.Username} Emerged victorious!", $"{Context.User.Username} took {DmgGained} :broken_heart: damage in that fight\n{Context.User.Username} got {ExpDrop} Experience!");

                    //drops
                    string drops = Drop(mob);
                    int baps = 0;
                    baps += rad.Next(10 + mob.Level, 17 + mob.Level * 2);
                    string dropsText = "";
                    if (drops != "")
                    {
                        string[] values = drops.Split(';');
                        foreach (string item in values)
                        {
                            List<string> inventory = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();
                            if (inventory.Count < Data.Data.GetBackpackTier(Context.User.Id) * 3 + 6)
                            {
                                await Data.Data.AddItemToBackpack(Context.User.Id, item);
                                dropsText += $"-{item}";
                                if (item != values.Last())
                                {
                                    dropsText += "\n";
                                }
                            }
                            else
                            {
                                dropsText += "Your inventory is full!";
                            }

                        }
                    }
                    builder.AddField("Loot received:", $"{dropsText}\n**{baps} baps**");
                    await Data.Data.SaveBalance(Context.User.Id, baps);

                    //exp
                    int ramonXp = Data.Data.GetExperience(Context.User.Id);
                    int ramonlvl = Program.GetLevel(ramonXp);
                    await Data.Data.SaveExperience(Context.User.Id, ExpDrop);
                    if (Program.GetLevel(ramonXp + ExpDrop) > ramonlvl)
                    {
                        builder.AddField($":sparkle: LEVEL UP :sparkle: ", $"{Context.User.Username} is now level {ramonlvl + 1}");
                        await Program.LevelUp(Context.User.Id);
                    }

                    builder.WithColor(Color.Green);
                    await ReplyAsync("", false, builder.Build());
                    break;
                }

            }
            Program.BattleLog bl;
            bl.builder = BattleLog;
            bl.UserId = Context.User.Id;

            foreach (Program.BattleLog item in Program.Battlelogs)
            {
                if(item.UserId == Context.User.Id)
                {
                    Program.Battlelogs.Remove(item);
                    break;
                }
            }

            Program.Battlelogs.Add(bl);
            await picture.DeleteAsync();

        }
        public int RamonTurn(ref int BottomEnd, ref int TopEnd, int mobArmor, bool mh)
        {
            if (Data.Data.GetMainHand(Context.User.Id) != "Empty" && mh)
            {
                BottomEnd = Data.Data.GetItemBottomEnd(Data.Data.GetMainHand(Context.User.Id));
                TopEnd = Data.Data.GetItemTopEnd(Data.Data.GetMainHand(Context.User.Id));
            }
            if(Data.Data.GetOffHand(Context.User.Id) != "Empty" && !mh)
            {
                BottomEnd = Data.Data.GetItemBottomEnd(Data.Data.GetOffHand(Context.User.Id));
                TopEnd = Data.Data.GetItemTopEnd(Data.Data.GetOffHand(Context.User.Id));
            }


            if (Data.Data.GetItemStyle(Data.Data.GetMainHand(Context.User.Id)) == "Melee")
            {
                return Program.DealRabonDamage(BottomEnd, TopEnd, Data.Data.GetATtackPower(Context.User.Id), mobArmor);
            }
            else if (Data.Data.GetItemStyle(Data.Data.GetMainHand(Context.User.Id)) == "Ranged")
            {
                return Program.DealRabonDamage(BottomEnd, TopEnd, Data.Data.GetRangedAttackpower(Context.User.Id), mobArmor);
            }
            else if (Data.Data.GetItemStyle(Data.Data.GetMainHand(Context.User.Id)) == "Magic")
            {
                return Program.DealRabonDamage(BottomEnd, TopEnd, Data.Data.GetSpellPower(Context.User.Id), mobArmor);
            }
            else
            {
                return Program.DealRabonDamage(BottomEnd, TopEnd, Data.Data.GetATtackPower(Context.User.Id), mobArmor);
            }
        }

        public int MobTurn(int bottomEnd,int topEnd, int ramonArmor)
        {
            Random rad = new Random();

            int dmg = rad.Next(bottomEnd, topEnd + 1);
            dmg = dmg * (int)Math.Round((100 - Program.GetDmgAbsorb(ramonArmor)) / 100f);
            return dmg;
        }

        [Command("log")]
        public async Task BattleLog()
        {
            Program.BattleLog log;
            EmbedBuilder builder = new EmbedBuilder();
            foreach (Program.BattleLog item in Program.Battlelogs)
            {
                if(item.UserId == Context.User.Id)
                {
                    builder = item.builder;
                    break;
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

        public string Drop(Mob mob)
        {
            List<string> RetList = new List<string>();

            string[] arr = mob.Drops.Split(';');

            List<string> Comm = new List<string>();
            List<string> Uncom = new List<string>();
            List<string> Rare = new List<string>();
            List<string> Epic = new List<string>();

            foreach (string item in arr)
            {
                if(Data.Data.GetItemRarity(item) == "common")
                {
                    Comm.Add(item);
                }
                else if (Data.Data.GetItemRarity(item) == "uncommon")
                {
                    Uncom.Add(item);
                }
                else if (Data.Data.GetItemRarity(item) == "rare")
                {
                    Rare.Add(item);
                }
                else if (Data.Data.GetItemRarity(item) == "epic")
                {
                    Epic.Add(item);
                }
            }


            int Plus = 2;
            int Division = 10;

            int lvl = mob.Level;

            foreach (string item in Program.CommonDropTable)
            {
                if(Data.Data.GetItemLevelReq(item) <= lvl + Plus + lvl / Division)
                {
                    Comm.Add(item);
                }
            }
            foreach (string item in Program.UncommonDropTable)
            {
                if (Data.Data.GetItemLevelReq(item) <= lvl + Plus + lvl / Division)
                {
                    Uncom.Add(item);
                }
            }
            foreach (string item in Program.RareDropTable)
            {
                if (Data.Data.GetItemLevelReq(item) <= lvl + Plus + lvl / Division)
                {
                    Rare.Add(item);
                }
            }
            foreach (string item in Program.EpicDropTable)
            {
                if (Data.Data.GetItemLevelReq(item) <= lvl + Plus + lvl / Division)
                {
                    Epic.Add(item);
                }
            }

            double chanceKiller = 0;

            Random rad = new Random();

            if(rad.NextDouble() < 0.2)
            {
                while(true)
                {
                    int index = rad.Next(0, Comm.Count);
                    if (rad.NextDouble() * 100 < Data.Data.GetItemDropChance(Comm[index]))
                    {
                        RetList.Add(Comm[index]);
                        chanceKiller += 0.05;
                        Comm.RemoveAt(index);
                        if (rad.NextDouble() > 0.2 - chanceKiller)
                        {                    
                            break;
                        }
                    }
                }
            }

            if (rad.NextDouble() < 0.11)
            {
                while (true)
                {
                    int index = rad.Next(0, Uncom.Count);
                    if (rad.NextDouble() * 100 < Data.Data.GetItemDropChance(Uncom[index]))
                    {
                        RetList.Add(Uncom[index]);
                        chanceKiller += 0.02;
                        Uncom.RemoveAt(index);
                        if (rad.NextDouble() > 0.11 - chanceKiller)
                        {
                            break;
                        }
                    }
                }
            }
            if (rad.NextDouble() < 0.04)
            {
                while (true)
                {
                    int index = rad.Next(0, Rare.Count);
                    if (rad.NextDouble() * 100 < Data.Data.GetItemDropChance(Rare[index]))
                    {
                        RetList.Add(Rare[index]);
                        chanceKiller += 0.02;
                        Rare.RemoveAt(index);
                        if (rad.NextDouble() > 0.04 - chanceKiller)
                        {
                            break;
                        }
                    }
                }
            }
            if (rad.NextDouble() < 0.012)
            {
                while (true)
                {
                    int index = rad.Next(0, Epic.Count);
                    if (rad.NextDouble() * 100 < Data.Data.GetItemDropChance(Epic[index]))
                    {
                        RetList.Add(Epic[index]);
                        chanceKiller += 0.02;
                        if (rad.NextDouble() > 0.012 - chanceKiller)
                        {
                            break;
                        }
                    }
                }
            }



            string ret = string.Join(';', RetList);

            return ret;
        }
    }
}
