using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KushBot
{
    public class SUser
    {
        [Key]
        public ulong Id { get; set; }
        public int Balance { get; set; }

        public int RamonIndex { get; set; }
        public string ClassName { get; set; }

        public int MaxHealth { get; set; }
        public int CurHealth { get; set; }
        public int AttackPower { get; set; }
        public int RangedAttackPower { get; set; }
        public int SpellPower { get; set; }
        public int Armor { get; set; }
        public int Lethality { get; set; }


        public int Experience { get; set; }


        public string MainHand { get; set; }
        public string OffHand { get; set; }
        public string Helmet { get; set; }
        public string Chest { get; set; }
        public string Necklace { get; set; }
        public string Goggles { get; set; }
        public string Elemental { get; set; }

        public DateTime LastRegen { get; set; }
        public DateTime DeathTime { get; set; }

        public string Inventory { get; set; }
        public int BackPack { get; set; }

        public int Agility { get; set; }

        public DateTime WorkDate { get; set; }
        public string WorkIndex { get; set; }
        public string Professions { get; set; }
        public string WorkArea { get; set; }

        public int GameState { get; set; }


        public SUser(ulong id, int balance)
        {
            Id = id;
            Balance = balance;
            RamonIndex = 1;
            ClassName = "test";

            AttackPower = 1;
            SpellPower = 1;
            MaxHealth = 10;
            CurHealth = 10;
            RangedAttackPower = 1;
            Experience = 0;
            Armor = 3;

            BackPack = 1;

            Inventory = "";
            Lethality = 0;

            LastRegen = DateTime.Now;
            DeathTime = DateTime.Now.AddHours(-1);

            MainHand = "Empty";
            OffHand = "Empty";
            Helmet = "Empty";
            Chest = "Empty";
            Necklace = "Empty";
            Goggles = "Empty";
            Elemental = "Empty";

            Agility = 0;

            WorkIndex = "neet";
            WorkArea = "none";
            Professions = "Fishing;1;Cooking;1";
           
            GameState = 0;
            

        }

        public static bool operator > (SUser lhs, SUser rhs)
        {
            if (lhs.Balance > rhs.Balance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator < (SUser lhs, SUser rhs)
        {
            if (lhs.Balance < rhs.Balance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
