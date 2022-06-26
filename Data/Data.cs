using KushBot.Resources.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Reflection;

namespace KushBot.Data
{
    public static class Data
    {
        public static async Task MakeRowForJew(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                    Console.WriteLine(DbContext.Jews.Count());

                    DbContext.Jews.Add(new SUser(UserId, 0));

                    await DbContext.SaveChangesAsync();
                }          
            }
        }      
        
        public static bool JewExists(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                    return false;
                }
                return true;
            }
        }

        //balance
        public static int GetBalance(ulong UserId)
        {
            using(var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Balance).FirstOrDefault();
            }
        }

        public static int GetExperience(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Experience).FirstOrDefault();
            }
        }

        public static int GetRamonIndex(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.RamonIndex).FirstOrDefault();
            }
        }
        
        public static string GetClassName(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return null;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.ClassName).FirstOrDefault();
            }
        }

        public static string GetProfessions(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return null;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Professions).FirstOrDefault();
            }
        }

        public static string GetInventory(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return null;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Inventory).FirstOrDefault();
            }
        }

        public static int GetMaxHealth(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.MaxHealth).FirstOrDefault();
            }
        }

        public static int GetCurHealth(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.CurHealth).FirstOrDefault();
            }
        }

        public static int GetATtackPower(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.AttackPower).FirstOrDefault();
            }
        }

        public static int GetRangedAttackpower(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.RangedAttackPower).FirstOrDefault();
            }
        }

        public static int GetSpellPower(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.SpellPower).FirstOrDefault();
            }
        }

        public static int GetArmor(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Armor).FirstOrDefault();
            }
        }

        public static string GetWorkIndex(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.WorkIndex).FirstOrDefault();
            }
        }

        public static string GetWorkArea(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.WorkArea).FirstOrDefault();
            }
        }

        public static int GetAgility(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Agility).FirstOrDefault();
            }
        }

        public static int GetGameState(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.GameState).FirstOrDefault();
            }
        }

        public static int GetLethality(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Lethality).FirstOrDefault();
            }
        }

        public static int GetBackpackTier(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.BackPack).FirstOrDefault();
            }
        }

        public static int GetBusyInventorySpace(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return 0;

                
                return (DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Inventory).FirstOrDefault()).Split(';',StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        public static async Task AddItemToBackpack(ulong UserId, string item)
        {
            using (var DbContext = new SqliteDbContext())
            {

                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();

                string temp = DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Inventory).FirstOrDefault();

                List<string> Save = new List<string>();

                string[] values = temp.Split(';', StringSplitOptions.RemoveEmptyEntries);

                Save = values.ToList();

                Save.Add(item);

                Current.Inventory = string.Join(';', Save);

                DbContext.Jews.Update(Current);

                await DbContext.SaveChangesAsync();
            }
        }

        public static int GetProfessionLevel(ulong UserId,string profession)
        {
            using (var DbContext = new SqliteDbContext())
            {
                string prof = DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Professions).FirstOrDefault();
                List<string> Profs = prof.Split(';').ToList();

                for (int i = 0; i < Profs.Count; i+=2)
                {
                    if(Profs[i].ToLower() == profession.ToLower())
                    {
                        return int.Parse(Profs[i + 1]);
                    }
                }
                return -1;

            }
        }

        //Saving
        /// <summary>
        /// Saving Starts here
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="rageDuration"></param>
        /// <returns></returns>
        public static async Task SaveBalance(ulong UserId, int Amount)
        {
            using(var DbContext = new SqliteDbContext())
            {
                if(DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Balance += Amount;
                    
                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveProfessions(ulong UserId,string profession, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                List<string> profs = Current.Professions.Split(';').ToList();

                for (int i = 0; i < profs.Count; i+=2)
                {
                    if(profs[i].ToLower() == profession.ToLower())
                    {
                        profs[i+1] = Amount.ToString();
                    }
                }
                Current.Professions = string.Join(';', profs);

                    DbContext.Jews.Update(Current);

                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveBackpackTier(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.BackPack = Amount;

                    DbContext.Jews.Update(Current);
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveExperience(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Experience += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveRamonId(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.RamonIndex = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveClassName(ulong UserId, string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.ClassName = name;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveMaxHealth(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.MaxHealth = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task Regenerate(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.CurHealth += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }
        public static async Task SetCurHealth(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.CurHealth = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveAttackPower(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.AttackPower = Amount;
                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveAgility(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Agility = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveGameState(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.GameState = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveRangedAttackpower(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.RangedAttackPower = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveSpellPower(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.SpellPower = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveArmor(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Armor = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddToArmor(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Armor += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddtoAp(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.AttackPower += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddToHp(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.MaxHealth += Amount;
                    Current.CurHealth += Amount;
                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddToRap(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.RangedAttackPower += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddToSp(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.SpellPower += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddToAgi(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Agility += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task AddToLethality(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Lethality += Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveInventory(ulong UserId, string Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Inventory = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveProfessions(ulong UserId, string Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Professions = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static DateTime GetLastRegen(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return DateTime.Now;

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.LastRegen).FirstOrDefault();
            }
        }

        public static async Task SaveLastRegen(ulong UserId, DateTime Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.LastRegen = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static DateTime GetDeathTime(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return DateTime.Now.AddHours(1);

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.DeathTime).FirstOrDefault();
            }
        }

        public static DateTime GetWorkDate(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return DateTime.Now.AddHours(1);

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.WorkDate).FirstOrDefault();
            }
        }

        public static async Task SaveDeathTime(ulong UserId, DateTime Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.DeathTime = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveWorkDate(ulong UserId, DateTime Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.WorkDate = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveWorkIndex(ulong UserId, string Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.WorkIndex = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SaveWorkArea(ulong UserId, string Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.WorkArea = Amount;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }
        public static string GetMainHand(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.MainHand).FirstOrDefault();
            }
        }
        public static string GetOffHand(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.OffHand).FirstOrDefault();
            }
        }

        public static string GetHelmet(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Helmet).FirstOrDefault();
            }

        }
        public static string GetNecklace(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Necklace).FirstOrDefault();
            }

        }

        public static string GetGoggles(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Goggles).FirstOrDefault();
            }
        }

        public static string GetChest(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Chest).FirstOrDefault();
            }

        }

        public static string GetElemental(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                    return "";

                return DbContext.Jews.Where(x => x.Id == UserId).Select(x => x.Elemental).FirstOrDefault();
            }

        }


        public static async Task SetMainHand(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.MainHand = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetOffHand(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.OffHand = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetHelmet(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Helmet = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetNecklace(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Necklace = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetGoggles(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Goggles = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetChest(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Chest = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task SetElemental(ulong UserId, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Jews.Where(x => x.Id == UserId).Count() < 1)
                {
                    //no row for user, create one
                }
                else
                {
                    SUser Current = DbContext.Jews.Where(x => x.Id == UserId).FirstOrDefault();
                    Current.Elemental = value;

                    DbContext.Jews.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static int GetMobDamage(int id)
        {
            Random rad = new Random();
            int bEnd = 0;
            int tEnd = 0;

            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.RegularMobs.Where(x => x.Id == id).Count() < 1)
                    return 0;

                bEnd = DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.BottomEnd).FirstOrDefault();
                tEnd = DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.TopEnd).FirstOrDefault();

                return rad.Next(bEnd, tEnd + 1);
            }
        }
        public static Mob GetMob(int id)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.RegularMobs.Where(x => x.Id == id).Count() < 1)
                    return null;

                //return DbContext.Weapons.Where(x => x.Id == id).Select(x => x.AttackStyle).FirstOrDefault();
                Mob mob = new RegularMob
                (
                    id,
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Level).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.BottomEnd).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.TopEnd).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.MaxHealth).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Armor).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.BaseExpDrop).FirstOrDefault(), 
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Agility).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Lethality).FirstOrDefault(),
                    DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Drops).FirstOrDefault()

                );

                return mob;
                
            }
        }

        public static int GetMobLevel(int id)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.RegularMobs.Where(x => x.Id == id).Count() < 1)
                    return 0;

                return DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Level).FirstOrDefault();
            }
        }

        public static int GetMobAgility(int id)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.RegularMobs.Where(x => x.Id == id).Count() < 1)
                    return 0;

                return DbContext.RegularMobs.Where(x => x.Id == id).Select(x => x.Agility).FirstOrDefault();
            }
        }

        #region Items

        public static string GetItemStyle(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return "error";

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.AttackStyle).FirstOrDefault();
            }
        }

        public static double GetItemDropChance(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 1;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.DropChance).FirstOrDefault();
            }
        }

        public static string GetItemRarity(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return "error";

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.Rarity).FirstOrDefault();
            }
        }

        public static int GetItemLevelReq(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.LevelReq).FirstOrDefault();
            }
        }

        public static string GetItemEquipPlace(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return "error";

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.EquipPlace).FirstOrDefault();
            }
        }

        public static int GetItemArmorValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.ArmorValue).FirstOrDefault();
            }
        }

        public static int GetItemAGIValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.AGI).FirstOrDefault();
            }
        }

        public static int GetItemLethValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.Lethality).FirstOrDefault();
            }
        }
        public static int GetItemApValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.AP).FirstOrDefault();
            }
        }

        public static int GetItemHpValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.HP).FirstOrDefault();
            }
        }

        public static int GetItemRapValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.RAP).FirstOrDefault();
            }
        }

        public static int GetItemSpValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.SP).FirstOrDefault();
            }
        }

        public static int GetItemValue(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                double slotK;

                Item item = DbContext.Items.Where(x => x.Name == name).FirstOrDefault();

                if(item.AttackStyle == "Material" || item.AttackStyle == "Food")
                {
                    return GetItemValue(name, true);
                }

                switch (item.EquipPlace)
                {
                    case "Chest":
                        slotK = 1.4;
                        break;
                    case "Goggles":
                        slotK = 1.2;
                        break;
                    case "Helmet":
                        slotK = 1.25;
                        break;
                    case "Necklace":
                        slotK = 1.3;
                        break;
                    default:
                        slotK = 1.6;
                        break;
                }

                double slotR;

                switch (item.Rarity)
                {
                    case "common":
                        slotR = 1;
                        break;
                    case "uncommon":
                        slotR = 1.5;
                        break;
                    case "rare":
                        slotR = 2;
                        break;
                    case "epic":
                        slotR = 2.5;
                        break;
                    default:
                        slotR = 3.5;
                        break;
                }

                int StatSum = item.HP + item.AP + item.RAP + item.SP + item.AGI + (2 * item.Lethality);

                double value = (5 + 2 * item.LevelReq * slotK + StatSum) * slotR;

                List<string> starters = new List<string>();
                starters.Add("Rock");
                starters.Add("Pliausk");
                starters.Add("Needle");
                starters.Add("Scuffedwand");

                if (starters.Contains(item.Name))
                {
                    value = 1;
                }

                value = Math.Round(value, 0);

                return (int)value;
            }
        }

        public static int GetItemValue(string name, bool yes)
        {
            using (var DbContext = new SqliteDbContext())
            {
                return DbContext.Items.Where(x => x.Name == name).Select(x => x.Value).FirstOrDefault();
            }
        }

        public static int GetItemBottomEnd(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                if (DbContext.Items.Where(x => x.Name == name).Select(x => x.AttackStyle).FirstOrDefault() == "Armor")
                {
                    Console.WriteLine("Armor doesnt have dmg");
                    return 0;
                }

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.BottomEnd).FirstOrDefault();
            }
        }


        public static int GetItemTopEnd(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return 0;

                if(DbContext.Items.Where(x => x.Name == name).Select(x => x.AttackStyle).FirstOrDefault() == "Armor")
                {
                    Console.WriteLine("Armor doesnt have dmg");
                    return 0;
                }

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.TopEnd).FirstOrDefault();
            }
        }


        public static string GetItemType(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return "error";

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.Type).FirstOrDefault();
            }
        }

        public static string ItemSetName(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Items.Where(x => x.Name == name).Count() < 1)
                    return "0";

                return DbContext.Items.Where(x => x.Name == name).Select(x => x.SetName).FirstOrDefault();
            }
        }

        #endregion

        public static int SetAp5(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Sets.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Sets.Where(x => x.Name == name).Select(x => x.Ap5).FirstOrDefault();
            }
        }
        public static int SetRap5(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Sets.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Sets.Where(x => x.Name == name).Select(x => x.Rap5).FirstOrDefault();
            }
        }
        public static int SetSp5(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Sets.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Sets.Where(x => x.Name == name).Select(x => x.Sp5).FirstOrDefault();
            }
        }
        public static int SetAgi5(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Sets.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Sets.Where(x => x.Name == name).Select(x => x.Agi5).FirstOrDefault();
            }
        }

        public static int SetLeth(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Sets.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Sets.Where(x => x.Name == name).Select(x => x.Lethality).FirstOrDefault();
            }
        }

        public static int SetArmor5(string name)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Sets.Where(x => x.Name == name).Count() < 1)
                    return 0;

                return DbContext.Sets.Where(x => x.Name == name).Select(x => x.Armor5).FirstOrDefault();
            }
        }      
        
        public static void SetGlobalTable()
        {
            using (var DbContext = new SqliteDbContext())
            {
                foreach (Item item in DbContext.Items)
                {
                     if(item.DropChance > 0 && item.Global)
                     {
                         if(item.Rarity == "common")
                         {
                             Program.CommonDropTable.Add(item.Name);
                         }
                         else if (item.Rarity == "uncommon")
                         {
                             Program.UncommonDropTable.Add(item.Name);
                         }
                         else if (item.Rarity == "rare")
                         {
                             Program.RareDropTable.Add(item.Name);
                         }
                         else if (item.Rarity == "epic")
                         {
                             Program.EpicDropTable.Add(item.Name);
                         }
                     }
                }
            }
        }

        public static string GetShopItems(int index)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Shops.Where(x => x.BottomLevel == index).Count() < 1)
                    return "";

                return DbContext.Shops.Where(x => x.BottomLevel == index).Select(x => x.LimitedItems).FirstOrDefault();
            }
        }

        public static int GetShopLimit(int index)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Shops.Where(x => x.BottomLevel == index).Count() < 1)
                    return 0;

                return DbContext.Shops.Where(x => x.BottomLevel == index).Select(x => x.AmountOfLimitedItems).FirstOrDefault();
            }
        }

        public static async Task SaveShopLimitedItems(int bottomLevel, string value)
        {
            using (var DbContext = new SqliteDbContext())
            {
                    Shop Current = DbContext.Shops.Where(x => x.BottomLevel == bottomLevel).FirstOrDefault();
                    Current.LimitedItems = value;

                    DbContext.Shops.Update(Current);

                await DbContext.SaveChangesAsync();
            }
        }

        public static string GetUnlimitedShopItems(int index)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Shops.Where(x => x.BottomLevel == index).Count() < 1)
                    return "";

                return DbContext.Shops.Where(x => x.BottomLevel == index).Select(x => x.NoLimitItems).FirstOrDefault();
            }
        }

        public static DateTime GetRefillDate(int index)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Shops.Where(x => x.BottomLevel == index).Count() < 1)
                    return DateTime.Now;

                return DbContext.Shops.Where(x => x.BottomLevel == index).Select(x => x.Refilled).FirstOrDefault();
            }
        }

        public static async Task SaveShopRefillDate(int index, DateTime Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                    Shop Current = DbContext.Shops.Where(x => x.BottomLevel == index).FirstOrDefault();
                    Current.Refilled = Amount;

                    DbContext.Shops.Update(Current);
                await DbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// AUCTION HOUSe
        /// </summary>
        /// 

        public static async Task AHItem(ulong UserId, string SellerName, string ItemName, int Price)
        {
            using (var DbContext = new SqliteDbContext())
            {
                int id = GetEmptyAhId();
                DbContext.AH.Add(new Auction(id,UserId, SellerName, ItemName, Price));
                   


                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task RemoveItemFromAH(int Id)
        {
            using (var DbContext = new SqliteDbContext())
            {
                DbContext.AH.Remove(DbContext.AH.Where(x => x.Id == Id).FirstOrDefault());

                await DbContext.SaveChangesAsync();
            }
        }

        public static string GetAHItemById(int id)
        {
            using(var DbContext = new SqliteDbContext())
            {
                return DbContext.AH.Where(x => x.Id == id).FirstOrDefault().SellingItem;
            }
        }

        public static int GetEmptyAhId()
        {
            int temp = 1;
            using(var Dbcontext = new SqliteDbContext())
            {
                foreach (var auc in Dbcontext.AH)
                {
                    foreach (var item in Dbcontext.AH)
                    {
                        if(temp == item.Id)
                        {
                            temp++;
                        }
                    }
                }
            }
            return temp;
        }

        public static void SetFishingAreas()
        {
            Program.FishingArea area = new Program.FishingArea();
            area.Name = "Well";
            area.Desc = "Fish in the local well";
            area.BottomLevel = 1;
            area.TopLevel = 10;

            Program.Areas.Add(area);
            area.Name = "Lake";
            area.Desc = "Fish in the local lake";
            area.BottomLevel = 10;
            area.TopLevel = 25;
            Program.Areas.Add(area);

        }

    }
}
