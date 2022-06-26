using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KushBot
{
    public class Armor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelReq { get; set; }
        public int ArmorValue { get; set; }
        public string ArmorPlace { get; set; }
        public int AP { get; set; }
        public int RAP { get; set; }
        public int SP { get; set; }
        public int HP { get; set; }

        public Armor(int id, string name,int armorValue, string armorPlace, int aP, int rAP, int sP, int hP)
        {
            Id = id;
            Name = name;
            ArmorValue = armorValue;
            ArmorPlace = armorPlace;
            AP = aP;
            RAP = rAP;
            SP = sP;
            HP = hP;
        }
    }
}
