using System;
using System.Collections.Generic;
using System.Text;

namespace KushBot
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public bool IsBuff { get; set; }
        public int Duration { get; set; }

        public int HpEffect { get; set; }
        public int APEffect { get; set; }
        public int RAPEffect { get; set; }
        public int SPEffect { get; set; }
        public int ArmorEffect { get; set; }
        public int AgilityEffect { get; set; }
        public int LethalityEffect { get; set; }

        public bool TargetSelf { get; set; }

        public StatusEffect(string name, bool isbuff, int duration)
        {
            Name = name;
            IsBuff = isbuff;
            Duration = duration;
            HpEffect = 0;
            APEffect = 0;
            RAPEffect = 0;
            SPEffect = 0;
            ArmorEffect = 0;
            AgilityEffect = 0;
            LethalityEffect = 0;
        }
    }
}
