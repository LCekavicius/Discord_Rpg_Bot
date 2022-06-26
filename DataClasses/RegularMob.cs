using System;
using System.Collections.Generic;
using System.Text;

namespace KushBot
{
    public class RegularMob : Mob
    {
        public RegularMob(int id, string name, int level, int bottomEnd, int topEnd, int maxHealth, int armor, int baseExpDrop, int agility, int lethality, string drops) : base(id, name,level,bottomEnd,topEnd,maxHealth,armor,baseExpDrop, agility,lethality , drops)
        {

        }
    }
}
