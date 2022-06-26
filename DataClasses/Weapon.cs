using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KushBot
{
    public class Weapon
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelReq { get; set; }
        public string AttackStyle { get; set; }
        public string Type { get; set; }
        public int BottomEnd { get; set; }
        public int TopEnd { get; set; }

        public int AP { get; set; }
        public int RAP { get; set; }
        public int SP { get; set; }

       /* public Weapon(int id, string name, int lvlreq, string attackStyle, string type, int bEnd, int tEnd, int ap, int rap, int sp)
        {
            Id = id;
            Name = name;
            LevelReq = lvlreq;
            AttackStyle = attackStyle;
            Type = type;
            BottomEnd = bEnd;
            TopEnd = tEnd;

            AP = ap;
            RAP = rap;
            SP = sp;

        }*/

        public Weapon(int id, string name, string attackStyle, string type)
        {
            Id = id;
            Name = name;
            LevelReq = 1;
            AttackStyle = attackStyle;
            Type = type;
            BottomEnd = 2;
            TopEnd = 4;

            AP = 0;
            RAP = 0;
            SP = 0;

        }

    }
}
