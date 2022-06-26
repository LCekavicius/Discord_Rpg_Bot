using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KushBot.Migrations
{
    public partial class Migration : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jews",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<int>(nullable: false),
                    RamonIndex = table.Column<int>(nullable: false),
                    ClassName = table.Column<string>(nullable: false),
                    MaxHealth = table.Column<int>(nullable: false),
                    CurHealth = table.Column<int>(nullable: false),
                    AttackPower = table.Column<int>(nullable: false),
                    RangedAttackPower = table.Column<int>(nullable: false),
                    SpellPower = table.Column<int>(nullable: false),
                    Armor = table.Column<int>(nullable: false),
                    Agility = table.Column<int>(nullable: false),
                    Lethality = table.Column<int>(nullable: false),
                    Experience = table.Column<int>(nullable: false),
                    MainHand = table.Column<int>(nullable: true),
                    OffHand = table.Column<int>(nullable: true),
                    Helmet = table.Column<int>(nullable: true),
                    Chest = table.Column<int>(nullable: true),
                    Necklace = table.Column<int>(nullable: true),
                    Goggles = table.Column<int>(nullable: true),
                    Elemental = table.Column<int>(nullable: true),
                    LastRegen = table.Column<DateTime>(nullable: true),
                    DeathTime = table.Column<DateTime>(nullable: true),
                    Inventory = table.Column<string>(nullable: true),
                    Backpack = table.Column<int>(nullable: true),
                    WorkDate = table.Column<DateTime>(nullable: true),
                    WorkIndex = table.Column<string>(nullable: false),
                    GameState = table.Column<int>(nullable: false),
                    Professions = table.Column<string>(nullable: true),
                    WorkArea = table.Column<string>(nullable: false)



                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jews", x => x.Id);
                });

           
            migrationBuilder.CreateTable
                (
                    name: "Items",
                    columns: table => new
                    {
                        Name = table.Column<string>(nullable: false),
                        LevelReq = table.Column<int>(nullable: false),
                        AttackStyle = table.Column<string>(nullable: true),
                        EquipPlace = table.Column<string>(nullable: true),
                        Type = table.Column<string>(nullable: false),
                        BottomEnd = table.Column<int>(nullable: true),
                        TopEnd = table.Column<int>(nullable: true),
                        ArmorValue = table.Column<int>(nullable: true),
                        HP = table.Column<int>(nullable: true),
                        AP = table.Column<int>(nullable: true),
                        RAP = table.Column<int>(nullable: true),
                        SP = table.Column<int>(nullable: true),
                        AGI = table.Column<int>(nullable: true),
                        Lethality = table.Column<int>(nullable: false),
                        Rarity = table.Column<string>(nullable: true),
                        DropChance = table.Column<double>(nullable: true),
                        Value = table.Column<int>(nullable: false),
                        Global = table.Column<bool>(nullable: false),
                        SetName = table.Column<string>(nullable: true)



                    }
                ); 

             migrationBuilder.CreateTable
                (
                    name: "RegularMobs",
                    columns: table => new
                    {
                        Id = table.Column<int>(nullable: false)
                          .Annotation("Sqlite:Autoincrement", true),
                        Name = table.Column<string>(nullable: false),
                        Level = table.Column<int>(nullable: false),
                        MaxHealth = table.Column<int>(nullable: false),
                        CurHealth = table.Column<int>(nullable: false),
                        BottomEnd = table.Column<int>(nullable: false),
                        TopEnd = table.Column<int>(nullable: false),
                        Armor = table.Column<int>(nullable: false),
                        Agility = table.Column<int>(nullable: false),
                        Lethality = table.Column<int>(nullable: false),
                        BaseExpDrop = table.Column<int>(nullable: false),
                        Drops = table.Column<string>(nullable:true)

                    }
                );


            migrationBuilder.CreateTable
               (
                   name: "Sets",
                   columns: table => new
                   {
                       Name = table.Column<string>(nullable: false),
                       IsFront = table.Column<bool>(nullable: false),

                       AP5 = table.Column<int>(nullable: true),
                       RAP5 = table.Column<int>(nullable: true),
                       SP5 = table.Column<int>(nullable: true),
                       AGI5 = table.Column<int>(nullable: true),
                       Armor5 = table.Column<int>(nullable: true),
                       Lethality = table.Column<int>(nullable: false)

                   }
               );


            migrationBuilder.CreateTable
               (
                   name: "Shops",
                   columns: table => new
                   {
                       BottomLevel = table.Column<int>(nullable: false),
                       TopLevel = table.Column<int>(nullable: false),
                       Refilled = table.Column<DateTime>(nullable: false),
                       AmountOfLimitedItems = table.Column<int>(nullable: false),
                       NoLimitItems = table.Column<string>(nullable: true),
                       LimitedItems = table.Column<string>(nullable: true),


                   }
               );

            migrationBuilder.CreateTable
               (
                   name: "AH",
                   columns: table => new
                   {
                       Id = table.Column<int>(nullable: false),
                       SellerId = table.Column<ulong>(nullable: false),
                       SellerName = table.Column<string>(nullable: false),
                       SellingItem = table.Column<string>(nullable: false),
                       ItemPrice = table.Column<int>(nullable: false)

                   }
               );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jews");

            migrationBuilder.DropTable(
                name: "Items");
            migrationBuilder.DropTable(
               name: "RegularMobs");
        }
    }
}
