using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace KushBot
{
    public abstract class Mob
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int BottomEnd { get; set; }
        public int TopEnd { get; set; }

        public int MaxHealth { get; set; }
        public int CurHealth { get; set; }

        public int Armor { get; set; }

        public int BaseExpDrop { get; set; }

        public int Agility { get; set; }
        public int Lethality { get; set; }

        public string Drops { get; set; }


        protected Mob(int id, string name, int level, int bottomEnd, int topEnd, int maxHealth, int armor, int baseExpDrop, int agility, int lethality, string drops)
        {
            Id = id;
            Name = name;
            Level = level;
            BottomEnd = bottomEnd;
            TopEnd = topEnd;
            MaxHealth = maxHealth;
            CurHealth = maxHealth;
            Armor = armor;
            BaseExpDrop = baseExpDrop;
            Agility = agility;
            Lethality = lethality;
            Drops = drops;
        }
    }
}
