using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KushBot
{
    public class Set
    {
        [Key]
        public string Name { get; set; }
        public bool IsFront { get; set; }

        public int Armor5 { get; set; }
        public int Ap5 { get; set; }
        public int Rap5 { get; set; }
        public int Sp5 { get; set; }
        public int Agi5 { get; set; }
        public int Lethality { get; set; }

    }
}
