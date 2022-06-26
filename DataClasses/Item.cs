using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KushBot
{
    public class Item
    {
        [Key]
        public string Name { get; set; }
        public int LevelReq { get; set; }
        public int ArmorValue { get; set; }
        public string AttackStyle { get; set; }
        public string Type { get; set; }
        public string EquipPlace { get; set; }
        public int BottomEnd { get; set; }
        public int TopEnd { get; set; }

        public int HP { get; set; }
        public int AP { get; set; }
        public int RAP { get; set; }
        public int SP { get; set; }
        public int AGI { get; set; }
        public int Lethality { get; set; }


        public string Rarity { get; set; }
        public double DropChance { get; set; }
        public string SetName { get; set; }

        //public string StatusEffects { get; set; }
        //public double StatusEffectChance { get; set; }

        public int Value { get; set; }
        public bool Global { get; set; }


        public Item(string name, int levelReq, string type, string equipPlace, int aP, int rAP, int sP)
        {
            Name = name;
            LevelReq = levelReq;
            AttackStyle = "misc";
            Type = type;
            EquipPlace = equipPlace;
            BottomEnd = 0;
            TopEnd = 0;
            AP = aP;
            RAP = rAP;
            SP = sP;
        }
    }
}
