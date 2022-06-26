using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using KushBot.Resources.Database;
using System.Timers;
using System.IO;
using System.Net;
using Google.Apis.Drive;

namespace KushBot
{
    class Program : ModuleBase<SocketCommandContext>
    {
        static void Main(string[] args)
        => new Program().RunBotAsync().GetAwaiter().GetResult();

        public static DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public static string WorldName = "The land of Ramonia";

        public static List<CreatingCharacter> RamonCreation = new List<CreatingCharacter>();

        public static ulong UserCreating;

        public static List<string> ClassNames = new List<string>();

        public static bool BotTesting = true;

        public static ulong DumpId = 555869540215554049;

        static System.Timers.Timer timer;

        public static List<string> CommonDropTable = new List<string>();
        public static List<string> UncommonDropTable = new List<string>();
        public static List<string> RareDropTable = new List<string>();
        public static List<string> EpicDropTable = new List<string>();

        public static List<StatusEffect> StatusEffects = new List<StatusEffect>();

        public static int TimerK = 1; 

        public struct BattleLog
        {
            public ulong UserId;
            public EmbedBuilder builder;
        };

        public struct FishingArea
        {
            public string Name;
            public string Desc;
            public int BottomLevel;
            public int TopLevel;
        };

        public static List<FishingArea> Areas = new List<FishingArea>();

        public static List<BattleLog> Battlelogs = new List<BattleLog>();

       // public static IMessageChannel dump = _client.GetChannel(555869540215554049) as IMessageChannel;


        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken;
            if (BotTesting)
            {
                botToken = "*************************";
            }
            else
            {
                botToken = "*************************";
            }

            //event subscriptions
            _client.Log += Log;
            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);
        
            await _client.StartAsync();

            SetClassNames();

            Random rad = new Random();

            await _client.SetGameAsync("Fortnite");

            timer = new System.Timers.Timer(1000 * TimerK);

            timer.Elapsed += OnTimedEvent;

            timer.AutoReset = true;

            timer.Enabled = true;

            if(BotTesting)
            {

            }


            Data.Data.SetFishingAreas();
            Data.Data.SetGlobalTable();

            await Task.Delay(-1);
            
          
        }

        static async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            TimerK = 60;
            

            Random rad = new Random();

            for (int i = 0; i <= 100; i += 10)
            {
                List<string> Comm = new List<string>();
                List<string> Uncom = new List<string>();
                List<string> Rare = new List<string>();

                if (Data.Data.GetRefillDate(i).AddHours(8) > DateTime.Now)
                {
                    continue;
                }

                string[] Items = new string[Data.Data.GetShopLimit(i)];


                await Data.Data.SaveShopRefillDate(i, DateTime.Now);
                foreach (string item in CommonDropTable)
                {
                    if (Data.Data.GetItemLevelReq(item) < i + 10 && Data.Data.GetItemLevelReq(item) > i)
                    {
                        Comm.Add(item);
                    }
                }
                foreach (string item in Program.UncommonDropTable)
                {
                    if (Data.Data.GetItemLevelReq(item) < i + 10 && Data.Data.GetItemLevelReq(item) > i)
                    {
                        Uncom.Add(item);
                    }
                }
                foreach (string item in Program.RareDropTable)
                {
                    if (Data.Data.GetItemLevelReq(item) < i + 10 && Data.Data.GetItemLevelReq(item) > i)
                    {
                        Rare.Add(item);
                    }
                }

                for (int h = 0; h < Data.Data.GetShopLimit(i); h++)
                {                  
                    double check = rad.NextDouble();

                    if (Rare.Count <= 0 && Uncom.Count <= 0 && Comm.Count <= 0)
                    {
                        return;
                    }

                    if(check < 0.07 && Rare.Count != 0)
                    {
                        Items[h] = Rare[rad.Next(0, Rare.Count)];
                    }
                    else if(check < 0.275 && Uncom.Count != 0)
                    {
                        Items[h] = Uncom[rad.Next(0, Uncom.Count)];
                    }
                    else
                    {
                        Items[h] = Comm[rad.Next(0, Comm.Count)];
                    }
                }

                if(Items.Length != 0)
                {
                    string ret = string.Join(';', Items);
                    await Data.Data.SaveShopLimitedItems(i, ret);
                }

            }

            

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        public static async Task RedeemMessage(string name, string everyone, string desc)
        {
            ulong id = 491605808254156802;

            if (BotTesting)
            {
                id = 494199544582766610;
            }
            var chnl = _client.GetChannel(id) as IMessageChannel;

            if(everyone == "")
            {
                await chnl.SendMessageAsync($"{name} Has redeemed {desc}");
            }
            else
            {
                await chnl.SendMessageAsync($"{everyone}, {name} Has redeemed {desc}");
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {

            var message = arg as SocketUserMessage;
            
            if (message is null || message.Author.IsBot)
            {
                return;
            }
    

            int argPos = 0;

            string Prefix;

            if (BotTesting)
            {
                Prefix = "!";
                
            }
            else
            {
                Prefix = ";";
            }


            if (message.HasStringPrefix(Prefix, ref argPos) || message.HasStringPrefix(";", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {

                if (!Data.Data.JewExists(message.Author.Id))
                {
                    var chnl = _client.GetChannel(message.Channel.Id) as IMessageChannel;

                    bool JewExists = false;
                    string[] permits = { $"{Prefix}start", $"{Prefix}next", $"{Prefix}select", $"{Prefix}prev" };
                    List<string> Permits = permits.ToList();
                    foreach (CreatingCharacter item in RamonCreation)
                    {
                        if (item.UserId == message.Author.Id)
                        {
                            JewExists = true;
                        }
                    }

                    if (!JewExists && message.ToString().ToLower() != $"{Prefix}start")
                    {
                        await chnl.SendMessageAsync($"{message.Author.Mention} You should create your Ramon first. Type ';start'");
                        return;
                    }
                    if (JewExists && !Permits.Contains(message.ToString().ToLower()) && !Permits.Contains(message.ToString().ToLower().Substring(0, 7)))
                    {
                        await chnl.SendMessageAsync($"{message.Author.Mention} You should finish creating your Ramon first.");
                        return;
                    }

                }

                if (Data.Data.GetCurHealth(message.Author.Id) == Data.Data.GetMaxHealth(message.Author.Id))
                {
                    await Data.Data.SaveLastRegen(message.Author.Id, DateTime.Now);
                }

                if (Data.Data.GetCurHealth(message.Author.Id) <= 0 && Data.Data.GetDeathTime(message.Author.Id).AddHours(1) < DateTime.Now)
                {
                    int SetHp = (int)Math.Round(Data.Data.GetMaxHealth(message.Author.Id) / 10f);
                    await Data.Data.SetCurHealth(message.Author.Id, SetHp);
                    await Data.Data.SaveLastRegen(message.Author.Id, DateTime.Now);
                }

                if(Data.Data.GetCurHealth(message.Author.Id) <= 0 && message.ToString().ToLower() != $"{Prefix}status" && message.ToString().ToLower() != $"{Prefix}log" && Data.Data.JewExists(message.Author.Id) && Data.Data.GetClassName(message.Author.Id) != "test")
                {
                    var chnl = _client.GetChannel(message.Channel.Id) as IMessageChannel;
                    await chnl.SendMessageAsync($"{message.Author.Mention} You can't do that when your Ramon is dead. Your ramon Will undie in {(Data.Data.GetDeathTime(message.Author.Id).AddHours(1) - DateTime.Now).Minutes}:{(Data.Data.GetDeathTime(message.Author.Id).AddHours(1) - DateTime.Now).Seconds} ");
                    return;
                }

                if(Data.Data.JewExists(message.Author.Id) && Data.Data.GetWorkIndex(message.Author.Id) != "neet")
                {
                    string[] allowed = { $"{Prefix}status", $"{Prefix}stop", $"{Prefix}log", $"{Prefix}inv", $"{Prefix}ah view", $"{Prefix}ah list", $"{Prefix}ah search", $"{Prefix}desc", $"{Prefix}shop misc", $"{Prefix}shop gear", $"{Prefix}gear", $"{Prefix}prof" };
                    if (!allowed.Contains(message.ToString().ToLower()) && message.ToString().ToLower().Substring(0,10) != $"{Prefix}ah search")
                    {
                        var chnl = _client.GetChannel(message.Channel.Id) as IMessageChannel;
                        await chnl.SendMessageAsync($"{message.Author.Mention} You can't do that while {Data.Data.GetWorkIndex(message.Author.Id)}");
                        return;
                    }
                    
                }

                if (Data.Data.JewExists(message.Author.Id) && Data.Data.GetCurHealth(message.Author.Id) < Data.Data.GetMaxHealth(message.Author.Id))
                {
                    while (Data.Data.GetLastRegen(message.Author.Id).AddMinutes(30) < DateTime.Now)
                    {
                        await Data.Data.Regenerate(message.Author.Id, (int)Math.Round(Data.Data.GetMaxHealth(message.Author.Id) / 10f));
                        await Data.Data.SaveLastRegen(message.Author.Id, Data.Data.GetLastRegen(message.Author.Id).AddMinutes(30));
                        if(Data.Data.GetCurHealth(message.Author.Id) >= Data.Data.GetMaxHealth(message.Author.Id))
                        {
                            await Data.Data.SaveLastRegen(message.Author.Id, DateTime.Now);
                            await Data.Data.SetCurHealth(message.Author.Id, Data.Data.GetMaxHealth(message.Author.Id));
                            break;
                        }
                    }
                }                

                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }

               

            }
        }
        


        public static void SetClassNames()
        {
            ClassNames.Add("Warrior");
            ClassNames.Add("Archer");
            ClassNames.Add("Mage");
            ClassNames.Add("Rogue");
        }


        public static int BottomEnd(int dmg, int AP)
        {
            float dmgf = dmg;

            dmgf += AP / 10;

            //dmgf = dmgf/100 * (100 + (int)Math.Round((float)AP / 2));

            int ret = (int)MathF.Floor(dmgf);

            return ret;

        }

        public static int TopEnd(int dmg, int AP)
        {
            float dmgf = dmg;


            dmgf += AP / 9;
            //dmgf = dmgf / 100 * (100 + AP);

            int ret = (int)MathF.Round(dmgf);

            return ret;
        }

        public static int DealRabonDamage(int bEnd, int tEnd, int ap, int enemyArmor)
        {
            Random rad = new Random();
            
            int dmg = rad.Next(BottomEnd(bEnd, ap), TopEnd(tEnd, ap) + 1);

            dmg = dmg * (int)Math.Round((100 - GetDmgAbsorb(enemyArmor)) / 100f);

            return dmg;
        }

        public static double GetDodgeChance(int Agi)
        {
            double ret = 5;
            
            ret += Math.Sqrt(Agi);

            ret = Math.Round(ret, 2);

            return ret;
        }

        public static double GetCritChance(int Lethality)
        {
            double ret = 3;

            ret += Math.Sqrt(Lethality);

            ret = Math.Round(ret, 2);

            return ret;
        }

        public static int GetCritDamage(int Lethality)
        {
            double ret = 150;

            ret += 5 * Math.Pow(Lethality, (1f / 3f));

            ret = Math.Round(ret, 0);

            return (int)ret;
        }

        public static double GetOhHitChance(int Agi)
        {
            double ret = 10;

            ret += 4 * Math.Sqrt(Agi);

            ret = Math.Round(ret, 2);

            return ret;
        }

        public static int GetLevel(int exp)
        {
            int lvl = 2;
            int nextLvlIn = 83;
            while(exp >= nextLvlIn)
            {
                exp -= nextLvlIn;
                lvl++;
                nextLvlIn = (int)Math.Round((lvl * 3 + 300 * Math.Pow(2, ((lvl / 1.125) - 1) / 7)) / 4);
            }

            lvl--;

            return lvl;
        }

        public static int NextLevelIn(int exp, int lvl)
        {
            int temp = 0;
            int temp2 = 2;

            while (true)
            {
                temp = (int)Math.Round((temp2 * 3 + 300 * Math.Pow(2, ((temp2 / 1.125) - 1) / 7)) / 4);
                if (exp < temp)
                {
                    return temp - exp;
                }
                exp -= temp;
                temp2++;
            }
        }
        public static double GetDmgAbsorb(int armor)
        {
            double temp = 2.5 * Math.Sqrt(armor);

            return Math.Round(temp,2);
        }

        public static async Task AddItemBonus(ulong userId, string item)
        {
            await Data.Data.AddToArmor(userId, Data.Data.GetItemArmorValue(item));
            await Data.Data.AddtoAp(userId, Data.Data.GetItemApValue(item));
            await Data.Data.AddToRap(userId, Data.Data.GetItemRapValue(item));
            await Data.Data.AddToSp(userId, Data.Data.GetItemSpValue(item));
            await Data.Data.AddToAgi(userId, Data.Data.GetItemAGIValue(item));
            await Data.Data.AddToLethality(userId, Data.Data.GetItemLethValue(item));
            await Data.Data.AddToHp(userId, Data.Data.GetItemHpValue(item));
        }

        public static async Task RemoveItemBonus(ulong userId, string item)
        {
            await Data.Data.AddToArmor(userId, -1 * Data.Data.GetItemArmorValue(item));
            await Data.Data.AddtoAp(userId, -1 * Data.Data.GetItemApValue(item));
            await Data.Data.AddToRap(userId, -1 * Data.Data.GetItemRapValue(item));
            await Data.Data.AddToSp(userId, -1 * Data.Data.GetItemSpValue(item));
            await Data.Data.AddToLethality(userId, -1 * Data.Data.GetItemLethValue(item));
            await Data.Data.AddToAgi(userId, -1 * Data.Data.GetItemAGIValue(item));
            await Data.Data.AddToHp(userId, -1 * Data.Data.GetItemHpValue(item));

        }

        public static async Task AddSetBonus(ulong userId, string set)
        {
            await Data.Data.AddToArmor(userId, Data.Data.SetArmor5(set));
            await Data.Data.AddtoAp(userId, Data.Data.SetAp5(set));
            await Data.Data.AddToRap(userId, Data.Data.SetRap5(set));
            await Data.Data.AddToSp(userId, Data.Data.SetSp5(set));
            await Data.Data.AddToAgi(userId, Data.Data.SetAgi5(set));
            await Data.Data.AddToLethality(userId, Data.Data.SetLeth(set));

        }



        public static async Task RemoveSetBonus(ulong userId, string set)
        {
            await Data.Data.AddToArmor(userId, -1 * Data.Data.SetArmor5(set));
            await Data.Data.AddtoAp(userId, -1 * Data.Data.SetAp5(set));
            await Data.Data.AddToRap(userId, -1 * Data.Data.SetRap5(set));
            await Data.Data.AddToSp(userId, -1 * Data.Data.SetSp5(set));
            await Data.Data.AddToAgi(userId, -1 * Data.Data.SetAgi5(set));
            await Data.Data.AddToLethality(userId, -1 * Data.Data.SetLeth(set));

        }
        public static int SetItemCount(ulong userId, string setName)
        {
            if(setName == null)
            {
                return 0;
            }

            int ret = 0;
            if (Data.Data.ItemSetName(Data.Data.GetMainHand(userId)) == setName)
            {
                ret++;
            }
            if (Data.Data.ItemSetName(Data.Data.GetOffHand(userId)) == setName)
            {
                ret++;
            }
            if (Data.Data.ItemSetName(Data.Data.GetHelmet(userId)) == setName)
            {
                ret++;
            }
            if (Data.Data.ItemSetName(Data.Data.GetNecklace(userId)) == setName)
            {
                ret++;
            }
            if (Data.Data.ItemSetName(Data.Data.GetGoggles(userId)) == setName)
            {
                ret++;
            }
            if (Data.Data.ItemSetName(Data.Data.GetChest(userId)) == setName)
            {
                ret++;
            }
            return ret;
        }

        public static async Task LevelUp(ulong userId)
        {

            if (GetLevel(Data.Data.GetExperience(userId)) % 5 == 0)
            {
                await Data.Data.SaveArmor(userId, Data.Data.GetArmor(userId) + 1);
                await Data.Data.SaveAgility(userId, Data.Data.GetAgility(userId) + 1);
            }
            await Data.Data.SaveMaxHealth(userId, Data.Data.GetMaxHealth(userId) + 1);
            await Data.Data.Regenerate(userId, 1);

            string temp = Data.Data.GetClassName(userId);

            if(temp == "Warrior")
            {
                if(GetLevel(Data.Data.GetExperience(userId)) % 4 == 0)
                {
                    await Data.Data.SaveArmor(userId, Data.Data.GetArmor(userId) + 1);
                    await Data.Data.SaveMaxHealth(userId, Data.Data.GetMaxHealth(userId) + 1);
                    await Data.Data.Regenerate(userId, 1);
                    await Data.Data.SaveAttackPower(userId, Data.Data.GetATtackPower(userId) + 1);

                }
                if (GetLevel(Data.Data.GetExperience(userId)) % 3 == 0)
                {
                    await Data.Data.SaveSpellPower(userId, Data.Data.GetSpellPower(userId) + 1);
                    await Data.Data.SaveRangedAttackpower(userId, Data.Data.GetRangedAttackpower(userId) + 1);
                }
                await Data.Data.SaveAttackPower(userId, Data.Data.GetATtackPower(userId) + 1);
            }
            else if (temp == "Rogue")
            {
                if (GetLevel(Data.Data.GetExperience(userId)) % 3 == 0)
                {
                    await Data.Data.SaveSpellPower(userId, Data.Data.GetSpellPower(userId) + 1);
                    await Data.Data.SaveRangedAttackpower(userId, Data.Data.GetRangedAttackpower(userId) + 1);
                    await Data.Data.SaveAttackPower(userId, Data.Data.GetATtackPower(userId) + 1);
                }
                await Data.Data.SaveAttackPower(userId, Data.Data.GetATtackPower(userId) + 1);
            }
            else if (temp == "Mage")
            {
                if (GetLevel(Data.Data.GetExperience(userId)) % 3 == 0)
                {
                    await Data.Data.SaveSpellPower(userId, Data.Data.GetSpellPower(userId) + 1);
                    await Data.Data.SaveRangedAttackpower(userId, Data.Data.GetRangedAttackpower(userId) + 1);
                    await Data.Data.SaveAttackPower(userId, Data.Data.GetATtackPower(userId) + 1);
                }
                await Data.Data.SaveSpellPower(userId, Data.Data.GetSpellPower(userId) + 1);
            }
            else if (temp == "Archer")
            {
                if (GetLevel(Data.Data.GetExperience(userId)) % 3 == 0)
                {
                    await Data.Data.SaveSpellPower(userId, Data.Data.GetSpellPower(userId) + 1);
                    await Data.Data.SaveRangedAttackpower(userId, Data.Data.GetRangedAttackpower(userId) + 1);
                    await Data.Data.SaveAttackPower(userId, Data.Data.GetATtackPower(userId) + 1);
                }
                await Data.Data.SaveRangedAttackpower(userId, Data.Data.GetRangedAttackpower(userId) + 1);
            }
        }

        public static string ItemDescription(string item, ulong Id)
        {
            string ret = "";

            string ExtraAP = $"+{Data.Data.GetItemApValue(item)} Attack power\n";
            string ExtraHP = $"+{Data.Data.GetItemHpValue(item)} Health\n";
            string ExtraRAP = $"+{Data.Data.GetItemRapValue(item)} Ranged attack power\n";
            string ExtraSP = $"+{Data.Data.GetItemSpValue(item)} Spell power\n";
            string ExtraArmor = $"+{Data.Data.GetItemArmorValue(item)} Armor\n";
            string ExtraAgi = $"+{Data.Data.GetItemAGIValue(item)} Agility\n";
            string ExtraLeth = $"+{Data.Data.GetItemLethValue(item)} Lethality\n";

            string SetBonus = "";

            if (Data.Data.ItemSetName(item) != null && Data.Data.ItemSetName(item) != "")
            {

                if (Program.SetItemCount(Id, Data.Data.ItemSetName(item)) < 3)
                {
                    SetBonus = $"~~**Set Bonus(3)**~~ {Program.SetItemCount(Id, Data.Data.ItemSetName(item))}/3\n";
                }
                else
                {
                    SetBonus = $"**Set Bonus(3)** {Program.SetItemCount(Id, Data.Data.ItemSetName(item))}/3\n";
                }

                string SetAp = $"--- +{Data.Data.SetAp5(Data.Data.ItemSetName(item))} Attack power\n";
                string SetRap = $"--- +{Data.Data.SetRap5(Data.Data.ItemSetName(item))} Ranged attack power\n";
                string SetSp = $"--- +{Data.Data.SetSp5(Data.Data.ItemSetName(item))} spell power\n";
                string SetAgi = $"--- +{Data.Data.SetAgi5(Data.Data.ItemSetName(item))} Agility\n";
                string setLeth = $"--- +{Data.Data.SetLeth(Data.Data.ItemSetName(item))} Lethality\n";
                string SetArmor = $"--- +{Data.Data.SetArmor5(Data.Data.ItemSetName(item))} Armor\n";

                if (Data.Data.SetAp5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetAp;
                }
                if (Data.Data.SetRap5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetRap;
                }
                if (Data.Data.SetSp5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetSp;
                }
                if (Data.Data.SetAgi5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetAgi;
                }
                if (Data.Data.SetLeth(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += setLeth;
                }
                if (Data.Data.SetArmor5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetArmor;
                }


            }

            if (item == "Empty")
            {
                return ret;
            }

            if (Data.Data.GetItemStyle(item) != "Armor" && Data.Data.GetItemStyle(item) != "Food" && Data.Data.GetItemStyle(item) != "Material")
            {
                if (Program.GetLevel(Data.Data.GetExperience(Id)) < Data.Data.GetItemLevelReq(item))
                {
                    ret = $"```diff\n-Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemStyle(item)} {Data.Data.GetItemEquipPlace(item)} {Data.Data.GetItemType(item)}.\nRaw Damage Range: {Data.Data.GetItemBottomEnd(item)}-{ Data.Data.GetItemTopEnd(item)}\n```";
                }
                else
                {
                    ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemStyle(item)} {Data.Data.GetItemEquipPlace(item)} {Data.Data.GetItemType(item)}.\nRaw Damage Range: **{Data.Data.GetItemBottomEnd(item)}-{ Data.Data.GetItemTopEnd(item)}**\n";
                }


                string rar = Data.Data.GetItemRarity(item);
                string Rarity = char.ToUpper(rar[0]) + rar.Substring(1);

                ret += $"Baps Value: {Data.Data.GetItemValue(item)} baps\n";
                ret += $"Rarity: {Rarity}\n";


                if (Data.Data.GetItemApValue(item) > 0)
                {
                    ret += ExtraAP;
                }
                if (Data.Data.GetItemRapValue(item) > 0)
                {
                    ret += ExtraRAP;
                }
                if (Data.Data.GetItemSpValue(item) > 0)
                {
                    ret += ExtraSP;
                }
                if (Data.Data.GetItemAGIValue(item) > 0)
                {
                    ret += ExtraAgi;
                }
                if (Data.Data.GetItemLethValue(item) > 0)
                {
                    ret += ExtraLeth;
                }
                if (Data.Data.GetItemArmorValue(item) > 0)
                {
                    ret += ExtraArmor;
                }
                if (Data.Data.GetItemHpValue(item) > 0)
                {
                    ret += ExtraHP;
                }
            }
            else if (Data.Data.GetItemStyle(item) == "Armor")
            {
                if (Program.GetLevel(Data.Data.GetExperience(Id)) < Data.Data.GetItemLevelReq(item))
                {
                    ret = $"```diff\n-Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)}. Armor Value: {Data.Data.GetItemArmorValue(item)}\n```";
                }
                else
                {
                    ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)}. Armor Value: **{Data.Data.GetItemArmorValue(item)}\n**";
                }

                string rar = Data.Data.GetItemRarity(item);
                string Rarity = char.ToUpper(rar[0]) + rar.Substring(1);

                ret += $"Baps Value: {Data.Data.GetItemValue(item)} baps\n";
                ret += $"Rarity: {Rarity}\n";

                if (Data.Data.GetItemApValue(item) > 0)
                {
                    ret += ExtraAP;
                }
                if (Data.Data.GetItemRapValue(item) > 0)
                {
                    ret += ExtraRAP;
                }
                if (Data.Data.GetItemSpValue(item) > 0)
                {
                    ret += ExtraSP;
                }
                if (Data.Data.GetItemAGIValue(item) > 0)
                {
                    ret += ExtraAgi;
                }
                if (Data.Data.GetItemLethValue(item) > 0)
                {
                    ret += ExtraLeth;
                }
                if (Data.Data.GetItemHpValue(item) > 0)
                {
                    ret += ExtraHP;
                }
            }
            else if (Data.Data.GetItemStyle(item) == "Food")
            {
                if (GetLevel(Data.Data.GetExperience(Id)) < Data.Data.GetItemLevelReq(item))
                {
                    ret = $"```diff\n-Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)} **Healing item**\n```";
                }
                else
                {
                    ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)} **Healing item**\n";
                }

                string rar = Data.Data.GetItemRarity(item);
                string Rarity = char.ToUpper(rar[0]) + rar.Substring(1);

                ret += $"Baps Value: {Data.Data.GetItemValue(item)} baps\n";
                ret += $"Rarity: {Rarity}\n";

                if(Data.Data.GetItemType(item) == "Potion")
                {
                    ExtraHP = $"+{Data.Data.GetItemHpValue(item)}**%** ({Math.Round((double)Data.Data.GetMaxHealth(Id) * ((double)Data.Data.GetItemHpValue(item) / 100), 0)}) Health\n";
                }

                ret += ExtraHP;


            }

            else if (Data.Data.GetItemStyle(item) == "Material")
            {
                ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemStyle(item)} {Data.Data.GetItemType(item)}\n";

                string rar = Data.Data.GetItemRarity(item);
                string Rarity = char.ToUpper(rar[0]) + rar.Substring(1);

                ret += $"Baps Value: {Data.Data.GetItemValue(item)} baps\n";
                ret += $"Rarity: {Rarity}\n";

            }
            ret += SetBonus;
            return ret;
        }

        public static string getEmoji(string Type, string Style)
        {
            string ret;

            if (Type == "Dagger")
            {
                return ":dagger:";
            }
            else if (Type == "Potion" && Style == "Food")
            {
                return ":syringe:";
            }
            else if (Type == "Food")
            {
                return ":poultry_leg:";
            }
            else if (Style == "Material" && Type == "Fish")
            {
                return ":fish:";
            }
            else if (Style == "Material")
            {
                return ":tools:";
            }
            else if (Type == "Helmet")
            {
                return ":tophat:";
            }
            else if (Type == "Chest")
            {
                return ":shirt:";
            }
            else if (Type == "Sword")
            {
                return ":knife:";
            }
            else if(Type == "Shield")
            {
                return ":shield:";
            }
            else if (Type == "Bow" || Type == "Gun")
            {
                return ":bow_and_arrow:";
            }
            else if (Type == "Wand" || Type == "Staff")
            {
                return ":sparkles:";
            }
            else if (Type == "Goggles")
            {
                return ":eyeglasses:";
            }
            else if (Type == "Necklace")
            {
                return ":medal:";
            }
            else
            {
                return "";
            }
        }
    }
}
