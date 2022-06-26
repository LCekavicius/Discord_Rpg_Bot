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
using KushBot.Resources.Database;

namespace KushBot.Modules
{
    
    public class Professions : ModuleBase<SocketCommandContext>
    {
        [Command("professions"),Alias("profession","profs","prof")]
        public async Task ShowProfs()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Professions");
            builder.WithColor(Color.DarkGreen);

            string profs = Data.Data.GetProfessions(Context.User.Id);

            List<string> professions = profs.Split(';').ToList();

            for (int i = 0; i < professions.Count; i+=2)
            {
                builder.AddField(professions[i],$"{professions[i+1]}/{GetRank(int.Parse(professions[i + 1]))}");
            }
            for (int i = 0; i < professions.Count; i += 2)
            {
                builder.AddField($"{professions[i]}", $"Type ';{professions[i].Substring(0,4)}' for more infomartion. you can stop {char.ToLower(professions[i][0]) + professions[i].Substring(1)} at any time by typing ';stop'");
            }


            builder.AddField("Training", "You can train your professions when you hit your current cap, do this by typing ';prof train'");
            builder.AddField("Learning", "To learn new professions type ';prof learn'");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("prof train")]
        public async Task Trainprofs()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Training professions");
            builder.WithColor(Color.DarkGreen);

            List<string> profs = Data.Data.GetProfessions(Context.User.Id).Split(';').ToList();

            bool upgradable = false;

            for (int i = 0; i < profs.Count; i+=2)
            {
                if(int.Parse(profs[i+1]) % 75 == 0)
                {
                    if(int.Parse(profs[i + 1]) == 300)
                    {
                        continue;
                    }
                    builder.AddField($"{profs[i]}",$"You can train {profs[i].ToLower()} to become a {GetRank(int.Parse(profs[i + 1])+75).Substring(3)} for {Math.Round(Math.Pow(int.Parse(profs[i + 1]), 1.1) * 2,0)} baps");
                    upgradable = true;
                }
            }

            builder.AddField("Train", $"type ';prof train profName' (e.g. ';prof train fishing' to train a specific professions");

            if (!upgradable)
            {
                await ReplyAsync($"{Context.User.Mention} You don't have any professions to train, you should reach the current caps.");
                return;
            }
            else
            {
                await ReplyAsync("",false, builder.Build());
            }

        }
        [Command("prof train")]
        public async Task Trainprofs(string profession)
        {
            profession = char.ToUpper(profession[0]) + profession.Substring(1).ToLower();

            if(Data.Data.GetProfessionLevel(Context.User.Id, profession) % 75 != 0)
            {
                await ReplyAsync($"{Context.User.Mention} You haven't reached your cap hence you can't train further.");
                return;
            }
            if(Data.Data.GetProfessionLevel(Context.User.Id, profession) == 300)
            {
                await ReplyAsync($"{Context.User.Mention} your {profession} profession level is at max.");
                return;
            }

            int TrainCost = (int)Math.Round(Math.Pow(Data.Data.GetProfessionLevel(Context.User.Id, profession), 1.1) * 2, 0);

            if(Data.Data.GetBalance(Context.User.Id) < TrainCost)
            {
                await ReplyAsync($"{Context.User.Mention} too poor to pay for training");
                return;
            }

            await Data.Data.SaveProfessions(Context.User.Id, profession, Data.Data.GetProfessionLevel(Context.User.Id, profession) + 1);
            await Data.Data.SaveBalance(Context.User.Id, -1 * TrainCost);

            await ReplyAsync($"{Context.User.Mention} trained his {profession.ToLower()} skills for {TrainCost} baps and became a {GetRank(Data.Data.GetProfessionLevel(Context.User.Id, profession)).Substring(3)}");

        }
        [Command("fish")]
        public async Task GoFish()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"Fishing Areas");
            builder.WithColor(Color.DarkGreen);


            foreach (Program.FishingArea item in Program.Areas)
            {
                builder.AddField($"{item.Name}", $"{item.Desc}, fishing level range: {item.BottomLevel}-{item.TopLevel}");
            }
            builder.AddField("Fishing", $"use the command ';fish areaName' (e.g. ;fish Well) to go fishing. you have a chance of receiving a fish every 4 minutes spent fishing");

            await ReplyAsync("", false, builder.Build());
        }
        [Command("fish")]
        public async Task GoFish([Remainder]string area)
        {
            area = char.ToUpper(area[0]) + area.Substring(1).ToLower();

            int level = int.Parse(Data.Data.GetProfessions(Context.User.Id).Split(';')[1]);
            if(level < Program.Areas.Where(x => x.Name == area).Select(x => x.BottomLevel).FirstOrDefault())
            {
                await ReplyAsync($"{Context.User.Mention} You are too pathetic to fish in that zone");
                return;
            }

            if(Data.Data.GetBusyInventorySpace(Context.User.Id) >= 6 + 3 * Data.Data.GetBackpackTier(Context.User.Id))
            {
                await ReplyAsync($"{Context.User.Mention} can't go fishing with a full backpack");
                return;
            }

            await Data.Data.SaveWorkIndex(Context.User.Id, "Fishing");
            await Data.Data.SaveWorkArea(Context.User.Id, area);
            await Data.Data.SaveWorkDate(Context.User.Id, DateTime.Now);
            await ReplyAsync($"{Context.User.Mention} is now Fishing.");
        }

        [Command("cook")]
        public async Task GoCook()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"Cooking");
            builder.WithColor(Color.DarkGreen);

            builder.AddField("Cooking", $"use the command ';cook all' to cook all cookable food in your inventory\nUse ';cook itemName' (e.g. ';cook Raw bass' to cook 1 specific item");

            await ReplyAsync("", false, builder.Build());
        }
        [Command("cook", RunMode = RunMode.Async)]
        public async Task GoCook([Remainder]string input)
        {
            input = char.ToUpper(input[0]) + input.Substring(1).ToLower();

            int level = Data.Data.GetProfessionLevel(Context.User.Id, "Cooking");

            List<string> inv = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();
            List<string> Cookable = new List<string>();
            bool SomeUncooked = false;

            foreach (string item in inv)
            {
                if(Data.Data.GetItemStyle(item) == "Material" && Data.Data.GetItemType(item) == "Fish")
                {
                    if(Data.Data.GetItemLevelReq(item) > level)
                    {
                        SomeUncooked = true;
                    }
                    else
                    {
                        Cookable.Add(item);
                    }
                }
            }

            if (input != "All")
            {
                if (Data.Data.GetItemLevelReq(input) > level)
                {
                    await ReplyAsync($"{Context.User.Mention} Your cooking level is not high enough.");
                    return;
                }
            }

            if (Cookable.Count == 0)
            {
                await ReplyAsync($"{Context.User.Mention} no items to cook");
                return;
            }

            if(input != "All")
            {
                if (!Cookable.Contains(input))
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have that item");
                    return;
                }
            }

            await Data.Data.SaveWorkIndex(Context.User.Id, "Cooking");
            await Data.Data.SaveWorkArea(Context.User.Id, "Firepit");
            await Data.Data.SaveWorkDate(Context.User.Id, DateTime.Now);


            if (input == "All")
            {
                await ReplyAsync($"{Context.User.Mention} is now Cooking. You'll be cooking for {Cookable.Count * 3} minutes.");
                await Task.Delay(3 * 1000 * 60 * Cookable.Count);
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} is now Cooking. You'll be cooking for 3 minutes.");
                await Task.Delay(3 * 1000 * 60);
            }

 
            if (Data.Data.GetWorkIndex(Context.User.Id) == "neet")
            {
                return;
            }

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{Context.User.Username}'s cooking results");

            await Data.Data.SaveWorkArea(Context.User.Id,"None");
            await Data.Data.SaveWorkIndex(Context.User.Id,"neet");

            Random rad = new Random();

            List<string> Items = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();

            int Succeeded = 0;
            int Failed = 0;
            int levelsGained = 0;

            if(input != "All")
            {
                double cookChance = (50 + (level * 2) - (Data.Data.GetItemLevelReq(input) * 2) - (7 * RarityToInt(Data.Data.GetItemRarity(input)))) / 100f;
                if (rad.NextDouble() < cookChance)
                {
                    Items.Remove(input);
                    string cookedItem = char.ToUpper(input.Substring(4)[0]) + input.Substring(5);
                    Items.Add(cookedItem);
                    Succeeded++;
                    if (GainedLevel(Data.Data.GetItemRarity(input), level - Data.Data.GetItemLevelReq(input), level))
                    {
                        level++;
                        levelsGained++;
                    }
                }
                else
                {
                    Items.Remove(input);
                    Failed++;
                }
                await Data.Data.SaveInventory(Context.User.Id, string.Join(';', Items));
            }
            else
            {
                foreach (string item in Cookable)
                {
                    double cookChance = (50 + (level * 2) - (Data.Data.GetItemLevelReq(item) * 2) - (7 * RarityToInt(Data.Data.GetItemRarity(item)))) / 100f;
                    if (rad.NextDouble() < cookChance)
                    {
                        Items.Remove(item);
                        string cookedItem = char.ToUpper(item.Substring(4)[0]) + item.Substring(5);
                        Items.Add(cookedItem);
                        Succeeded++;
                        if(GainedLevel(Data.Data.GetItemRarity(item), level - Data.Data.GetItemLevelReq(item), level))
                        {
                            level++;
                            levelsGained++;
                        }
                    }
                    else
                    {
                        Items.Remove(item);
                        Failed++;
                    }
                    await Data.Data.SaveInventory(Context.User.Id, string.Join(';', Items));
                }
            }


            await Data.Data.SaveProfessions(Context.User.Id, "Cooking", level);

            builder.WithColor(Color.DarkGreen);
            builder.AddField("Results",$"-Succeeded: {Succeeded}\n-Burned: {Failed}");
            builder.AddField("Levels", $"Levels gained: {levelsGained}");
            if (SomeUncooked)
            {
                builder.AddField("Some Fish left uncooked","Your cooking level wasnt high enough for some fish");
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("stop")]
        public async Task Stopprof()
        {
            if(Data.Data.GetWorkIndex(Context.User.Id) == "neet")
            {
                await ReplyAsync($"{Context.User.Mention} you are not working, moron");
                return;
            }
            TimeSpan ts = DateTime.Now - Data.Data.GetWorkDate(Context.User.Id);

            Type thisType = GetType();
            MethodInfo mi = thisType.GetMethod(Data.Data.GetWorkIndex(Context.User.Id).ToLower());
            mi.Invoke(this,null);
        }

        public string GetRank(int level)
        {
            if(level <= 75)
            {
                return "75 Novice";
            }
            else if(level <= 150)
            {
                return "150 Apprentice";
            }
            else if (level <= 225)
            {
                return "225 Artisan";
            }
            else if (level <= 300)
            {
                return "300 Master";
            }
            else
            {
                return "ass";
            }
        }

        public async Task fishing()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Stopped Fishing");
            TimeSpan ts = DateTime.Now - Data.Data.GetWorkDate(Context.User.Id);

            await Data.Data.SaveWorkIndex(Context.User.Id, "neet");


            int length = (int)Math.Round(ts.TotalMinutes,0);

            builder.AddField("Duration", $"Fished for: **{length}** minutes");

            List<string> profs = Data.Data.GetProfessions(Context.User.Id).Split(';').ToList();
            int lvl = 0;
            int ind = 0;
            for(int i = 0; i < profs.Count; i+=2)
            {
                if(profs[i] == "Fishing")
                {
                    lvl = int.Parse(profs[i + 1]);
                    ind = i+1;
                    break;
                }
            }

            List<string> fish = new List<string>();
            using (var DbContext = new SqliteDbContext())
            {
                foreach (Item item in DbContext.Items)
                {
                    if(Data.Data.GetItemType(item.Name) == "Fish" && Data.Data.GetItemEquipPlace(item.Name) == Data.Data.GetWorkArea(Context.User.Id))
                    {
                        if(lvl >= Data.Data.GetItemLevelReq(item.Name))
                        {
                            fish.Add(item.Name);
                        }
                    }
                }
            }

            Random rad = new Random();
            List<string> Caughtfish = new List<string>();

            int levelsGained = 0;

            int AreaLvl = Program.Areas.Where(x => x.Name == Data.Data.GetWorkArea(Context.User.Id)).Select(x => x.BottomLevel).FirstOrDefault();


            while (length >= 4)
            {
                if (Data.Data.GetBusyInventorySpace(Context.User.Id) >= 6 + 3 * Data.Data.GetBackpackTier(Context.User.Id))
                {
                    builder.AddField("Inventory is full", "couldnt get more fish");
                    break;
                }

                if (rad.NextDouble() < ChanceToHitFish(lvl, AreaLvl))
                {
                    string fishOnHook = fish[rad.Next(0, fish.Count)];

                    while (rad.NextDouble() > FishChance(lvl, fishOnHook))
                    {
                        fishOnHook = fish[rad.Next(0, fish.Count)];
                    }
                    Caughtfish.Add(fishOnHook);
                    if (GainedLevel(Data.Data.GetItemRarity(fishOnHook), lvl - Data.Data.GetItemLevelReq(fishOnHook), lvl))
                    {
                        lvl++;
                        levelsGained++;
                    }
                    await Data.Data.AddItemToBackpack(Context.User.Id, fishOnHook);
                }
                length -= 4;
            }

            string PrintFish = "";
            foreach (string fush in Caughtfish)
            {
                PrintFish += $"{fush}\n";

            }
            profs[ind] = lvl.ToString();

            await Data.Data.SaveProfessions(Context.User.Id, string.Join(';', profs));
            await Data.Data.SaveWorkArea(Context.User.Id,"None");
           
            builder.WithColor(Color.DarkGreen);

            if (PrintFish == "")
            {
                builder.AddField($"No fish", "Didn't catch anything apparently");
            }
            else
            {
                builder.AddField("Level", $"Levels gained: {levelsGained}");
                builder.AddField("Caught fish:", $"{PrintFish}");
            }
            await ReplyAsync("",false,builder.Build());
        }

        public async Task cooking()
        {

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Stopped cooking");
            TimeSpan ts = DateTime.Now - Data.Data.GetWorkDate(Context.User.Id);

            await Data.Data.SaveWorkIndex(Context.User.Id, "neet");


            int length = (int)Math.Round(ts.TotalMinutes, 0);

            builder.AddField("Duration", $"cooked for: **{length}** minutes");

            int level = Data.Data.GetProfessionLevel(Context.User.Id, "cooking");

            List<string> inv = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();
            List<string> Cookable = new List<string>();
            bool SomeUncooked = false;

            foreach (string item in inv)
            {
                if (Data.Data.GetItemStyle(item) == "Material" && Data.Data.GetItemType(item) == "Fish")
                {
                    if(Data.Data.GetItemLevelReq(item) > level)
                    {
                        SomeUncooked = true;
                    }
                    else
                    {
                        Cookable.Add(item);
                    }
                }
            }


            Random rad = new Random();

            int Succeeded = 0;
            int Failed = 0;
            int levelsGained = 0;

            while (length >= 3)
            {
                if (Cookable.Count == 0)
                {
                    break;
                }

                string fish = Cookable[0];

                double cookChance = (50 + (level * 2) - (Data.Data.GetItemLevelReq(fish) * 2) - (7 * RarityToInt(Data.Data.GetItemRarity(fish)))) / 100f;

                if (rad.NextDouble() < cookChance)
                {
                    string cookedItem = char.ToUpper(fish.Substring(4)[0]) + fish.Substring(5);
                    inv.Add(cookedItem);
                    Succeeded++;
                    if (GainedLevel(Data.Data.GetItemRarity(fish), level - Data.Data.GetItemLevelReq(fish), level))
                    {
                        level++;
                        levelsGained++;
                    }
                }
                else
                {
                    Failed++;
                }

                Cookable.Remove(fish);
                inv.Remove(fish);

                length -= 3;

            }

            await Data.Data.SaveInventory(Context.User.Id, string.Join(';', inv));

            await Data.Data.SaveProfessions(Context.User.Id, "Cooking", level);

            builder.WithTitle($"{Context.User.Username}'s cooking results");

            await Data.Data.SaveWorkArea(Context.User.Id, "None");
            await Data.Data.SaveWorkIndex(Context.User.Id, "neet");

            builder.WithColor(Color.DarkGreen);
            builder.AddField("Results", $"-Succeeded: {Succeeded}\n-Burned: {Failed}");
            builder.AddField("Levels", $"Levels gained: {levelsGained}");

            if (SomeUncooked)
            {
                builder.AddField("Some Fish left uncooked", "Your cooking level wasnt high enough for some fish");
            }

            await ReplyAsync("", false, builder.Build());

        }

        public int RarityToInt(string rarity)
        {
            if (rarity == "common")
            {
                return 1;
            }
            else if (rarity == "uncommon")
            {
                return 2;
            }
            else if (rarity == "rare")
            {
                return 3;
            }
            else if (rarity == "epic")
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
        public bool GainedLevel(string rarity, int lvlDiff, int curLvl)
        {
            double lvlchance = 0.4 + 0.2 * RarityToInt(rarity);
            Random rad = new Random();

            if(curLvl % 75 == 0)
            {
                return false;
            }

            lvlchance -= lvlDiff / 30;

            if (rad.NextDouble() < lvlchance)
            {
                return true;
            }
            return false;
        }

        public double ChanceToHitFish(int fishingLevel, int AreaBottomLevel)
        {
            double ret = 0;
            ret = ((47 + (double)fishingLevel / 2) - (double)AreaBottomLevel) / 100;
            return ret;
        }
        public double FishChance(int fishingLevel, string fish)
        {
            double ret = 0;
            ret = (45 + fishingLevel - Data.Data.GetItemLevelReq(fish) - 7 * RarityToInt(Data.Data.GetItemRarity(fish)));
            return ret;
        }
    }
}
