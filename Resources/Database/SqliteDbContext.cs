using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KushBot.Resources.Database
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<SUser> Jews { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<RegularMob> RegularMobs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Auction> AH { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            string DbLocation = Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.0", @"Data/");

            //Options.UseSqlite($@"Data Source= Data/Database.sqlite");
            if (Program.BotTesting)
            {
                Options.UseSqlite($@"Data Source= D:\KushBot\Kush Bot\KushBotV2\KushBot\Data\Database.sqlite");
            }
            else
            {
                Options.UseSqlite($@"Data Source= Data/Database.sqlite");
            }

            //{DbLocation}
        }

    }
}
