using System;
using System.Collections.Generic;
using System.Text;

namespace KushBot
{
    public class CreatingCharacter
    {
        public ulong UserId { get; set; }
        public int RamonIndex { get; set; }
        public int RamonBlockId { get; set; }
        public string ClassName { get; set; }
        public int Stage { get; set; }
        public int index { get; set; }
        

        public CreatingCharacter(ulong userId)
        {
            UserId = userId;
            Stage = 1;
        }
    }
}
