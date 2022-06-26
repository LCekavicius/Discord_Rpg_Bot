using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;
using Discord.Rest;
using System.Linq;
using System.IO;
using SixLabors.ImageSharp.Processing.Dithering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing.Processors;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using System.Drawing;
using System.Reflection;

namespace KushBot.Modules
{
    public class InventoryManagement : ModuleBase<SocketCommandContext>
    {
        [Command("Inventory"), Alias("Inv")]
        public async Task inven()
        {
            List<int> Weapons = new List<int>();
            List<int> Armor = new List<int>();
            List<int> Misc = new List<int>();

            string inv = Data.Data.GetInventory(Context.User.Id);

            string[] values = inv.Split(';', StringSplitOptions.RemoveEmptyEntries);

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"{Context.User.Username}'s Inventory: {Data.Data.GetBusyInventorySpace(Context.User.Id)}/{6 + 3 * Data.Data.GetBackpackTier(Context.User.Id)} Slots used up");

            int temp = 0;

            foreach (string item in values)
            {
                builder.AddInlineField($"**{item}** {Program.getEmoji(Data.Data.GetItemType(item), Data.Data.GetItemStyle(item))}", $"{Program.ItemDescription(item, Context.User.Id)}");
            }

            builder.AddField("Currency", $"Baps: {Data.Data.GetBalance(Context.User.Id)}");

            builder.WithColor(Discord.Color.Gold);


            await ReplyAsync("", false, builder.Build());
        }

        [Command("equip")]
        public async Task equipp(string place, [Remainder] string Item)
        {
            Item = char.ToUpper(Item[0]) + Item.ToLower().Substring(1);

            if (Data.Data.GetItemLevelReq(Item) > Program.GetLevel(Data.Data.GetExperience(Context.User.Id)))
            {
                await ReplyAsync($"{Context.User.Mention} you can't equip a level {Data.Data.GetItemLevelReq(Item)} item.");
                return;
            }
            bool isOh = false;
            if (place.ToLower() == "offhand" || place.ToLower() == "oh")
            {
                isOh = true;
            }
            else
            {
                await equipp($"{place} {Item}");
                return;
            }

            string inv = Data.Data.GetInventory(Context.User.Id);

            string[] valuesArr = inv.Split(';');
            List<string> values = valuesArr.ToList();


            if (!values.Contains(Item))
            {
                await ReplyAsync($"{Context.User.Mention} You don't have that item, R E T A R D E D LOL!");
                return;
            }

            List<string> allowed = new List<string>();
            allowed.Add("Melee"); allowed.Add("Magic"); allowed.Add("Ranged"); allowed.Add("Armor");
            if (!allowed.Contains(Data.Data.GetItemStyle(Item)))
            {
                await ReplyAsync($"{Context.User.Mention} can't equip that item, not sure if trolling or retarded...");
                return;
            }

            string shit = Data.Data.GetItemEquipPlace(Item);

            if (shit == "Onehand")
            {
                shit = "Offhand";
            }

            if (Data.Data.GetClassName(Context.User.Id) != "Rogue" && Data.Data.GetItemStyle(Item) != "Armor")
            {
                await ReplyAsync($"{Context.User.Mention} only rogues can equip weapons into his offhand");
                return;
            }
            else if (Data.Data.GetClassName(Context.User.Id) != "Warrior" && Data.Data.GetItemType(Item) == "Shield")
            {
                await ReplyAsync($"{Context.User.Mention} only Warriors can equip shields into his offhand");
                return;
            }

            int temp;

            string outputpath = $"D:/KushBot/Kush Bot/KushBotV2/KushBot/Data/Portraits/{Context.User.Id}.png";

            string EPlace = Data.Data.GetItemEquipPlace(Item);


            if ((EPlace == "Onehand" || EPlace == "Offhand") && isOh)
            {
                EPlace = "Offhand";
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} not the brightest fella, aren't you?");
                return;
            }


            foreach (string item in values)
            {
                if (Item.ToLower() == item.ToLower())
                {

                    if (EPlace == "Mainhand")
                    {
                        if (Data.Data.GetMainHand(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetMainHand(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (EPlace == "Offhand")
                    {
                        if (Data.Data.GetOffHand(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetOffHand(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Helmet")
                    {
                        if (Data.Data.GetHelmet(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetHelmet(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Necklace")
                    {
                        if (Data.Data.GetNecklace(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetNecklace(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Goggles")
                    {
                        if (Data.Data.GetGoggles(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetGoggles(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Chest")
                    {
                        if (Data.Data.GetChest(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetChest(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }

                    if (Data.Data.ItemSetName(item) != null)
                    {
                        if (Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item)) == 3)
                        {
                            await Program.AddSetBonus(Context.User.Id, Data.Data.ItemSetName(item));
                        }
                    }

                    values.Remove(item);

                    using (Image<Rgba32> img1 = SixLabors.ImageSharp.Image.Load($"Pictures/Ramons/Ramon{Data.Data.GetRamonIndex(Context.User.Id)}.jpg"))
                    using (Image<Rgba32> Helm = SixLabors.ImageSharp.Image.Load($"Pictures/Helmets/{Data.Data.GetHelmet(Context.User.Id).ToLower()}.png"))
                    using (Image<Rgba32> Neck = SixLabors.ImageSharp.Image.Load($"Pictures/Necklaces/{Data.Data.GetNecklace(Context.User.Id).ToLower()}.png"))
                    using (Image<Rgba32> wep = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetMainHand(Context.User.Id)}.png"))
                    using (Image<Rgba32> oh = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetOffHand(Context.User.Id)}.png"))
                    using (Image<Rgba32> Goggles = SixLabors.ImageSharp.Image.Load($"Pictures/Goggles/{Data.Data.GetGoggles(Context.User.Id).ToLower()}.png"))
                    using (Image<Rgba32> Chest = SixLabors.ImageSharp.Image.Load($"Pictures/Chests/{Data.Data.GetChest(Context.User.Id)}.png"))
                    using (Image<Rgba32> Elemental = SixLabors.ImageSharp.Image.Load($"Pictures/Elementals/{Data.Data.GetElemental(Context.User.Id)}.png"))
                    using (Image<Rgba32> outputImage = img1)
                    {
                        oh.Mutate(x => x.RotateFlip(RotateMode.None, FlipMode.Horizontal));

                        outputImage.Mutate(x => x
                            .DrawImage(img1, 1f)
                            .DrawImage(Chest, 1f)
                            .DrawImage(Helm, 1f)
                            .DrawImage(Goggles, 1f)
                            .DrawImage(Neck, 1f)
                            .DrawImage(wep, 1f)
                            .DrawImage(oh, 1f)
                            .DrawImage(Elemental, 1f)
                        );
                        outputImage.Save(outputpath);

                        break;
                    }
                }
            }
            string save = string.Join(';', values);

            await Data.Data.SaveInventory(Context.User.Id, save);

            await ReplyAsync($"{Context.User.Mention} Equipped {Item} into his offhand");

        }


        [Command("equip")]
        public async Task equipp([Remainder] string Item)
        {

            string inv = Data.Data.GetInventory(Context.User.Id);

            string[] valuesArr = inv.Split(';');
            List<string> values = valuesArr.ToList();

            Item = char.ToUpper(Item[0]) + Item.ToLower().Substring(1);

            if (Data.Data.GetItemLevelReq(Item) > Program.GetLevel(Data.Data.GetExperience(Context.User.Id)))
            {
                await ReplyAsync($"{Context.User.Mention} you can't equip a level {Data.Data.GetItemLevelReq(Item)} item.");
                return;
            }

            if (!values.Contains(Item))
            {
                await ReplyAsync($"{Context.User.Mention} You don't have that item, R E T A R D E D LOL!");
                return;
            }

            List<string> allowed = new List<string>();
            allowed.Add("Melee"); allowed.Add("Magic"); allowed.Add("Ranged"); allowed.Add("Armor");
            if (!allowed.Contains(Data.Data.GetItemStyle(Item)))
            {
                await ReplyAsync($"{Context.User.Mention} can't equip that item, not sure if trolling or retarded...");
                return;
            }

            string outputpath = $"D:/KushBot/Kush Bot/KushBotV2/KushBot/Data/Portraits/{Context.User.Id}.png";

            string EPlace = Data.Data.GetItemEquipPlace(Item);


            if (Data.Data.GetItemType(Item) == "Shield" && Data.Data.GetClassName(Context.User.Id) != "Warrior")
            {
                await ReplyAsync($"{Context.User.Mention} only warriors can equip shields");
                return;
            }

            foreach (string item in values)
            {
                if (Item.ToLower() == item.ToLower())
                {

                    if (EPlace == "Mainhand" || EPlace == "Onehand")
                    {
                        if (Data.Data.GetMainHand(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetMainHand(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (EPlace == "Offhand")
                    {
                        if (Data.Data.GetOffHand(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetOffHand(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Helmet")
                    {
                        if (Data.Data.GetHelmet(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetHelmet(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Necklace")
                    {
                        if (Data.Data.GetNecklace(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetNecklace(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Goggles")
                    {
                        if (Data.Data.GetGoggles(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetGoggles(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }
                    if (Data.Data.GetItemEquipPlace(item) == "Chest")
                    {
                        if (Data.Data.GetChest(Context.User.Id) != "Empty")
                        {
                            await ReplyAsync($"{Context.User.Mention} Seems like you already have something equipped there. Try unequipping it first (';unequip {EPlace}')");
                            return;
                        }
                        await Data.Data.SetChest(Context.User.Id, item);
                        await Program.AddItemBonus(Context.User.Id, item);
                    }

                    if (Data.Data.ItemSetName(item) != null)
                    {
                        if (Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item)) == 3)
                        {
                            await Program.AddSetBonus(Context.User.Id, Data.Data.ItemSetName(item));
                        }
                    }

                    values.Remove(item);

                    using (Image<Rgba32> img1 = SixLabors.ImageSharp.Image.Load($"Pictures/Ramons/Ramon{Data.Data.GetRamonIndex(Context.User.Id)}.jpg"))
                    using (Image<Rgba32> Helm = SixLabors.ImageSharp.Image.Load($"Pictures/Helmets/{Data.Data.GetHelmet(Context.User.Id).ToLower()}.png"))
                    using (Image<Rgba32> Neck = SixLabors.ImageSharp.Image.Load($"Pictures/Necklaces/{Data.Data.GetNecklace(Context.User.Id).ToLower()}.png"))
                    using (Image<Rgba32> wep = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetMainHand(Context.User.Id)}.png"))
                    using (Image<Rgba32> oh = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetOffHand(Context.User.Id)}.png"))
                    using (Image<Rgba32> Goggles = SixLabors.ImageSharp.Image.Load($"Pictures/Goggles/{Data.Data.GetGoggles(Context.User.Id).ToLower()}.png"))
                    using (Image<Rgba32> Chest = SixLabors.ImageSharp.Image.Load($"Pictures/Chests/{Data.Data.GetChest(Context.User.Id)}.png"))
                    using (Image<Rgba32> Elemental = SixLabors.ImageSharp.Image.Load($"Pictures/Elementals/{Data.Data.GetElemental(Context.User.Id)}.png"))
                    using (Image<Rgba32> outputImage = img1)
                    {
                        oh.Mutate(x => x.RotateFlip(RotateMode.None, FlipMode.Horizontal));

                        outputImage.Mutate(x => x
                            .DrawImage(img1, 1f)
                            .DrawImage(Chest, 1f)
                            .DrawImage(Helm, 1f)
                            .DrawImage(Goggles, 1f)
                            .DrawImage(Neck, 1f)
                            .DrawImage(wep, 1f)
                            .DrawImage(oh, 1f)
                            .DrawImage(Elemental, 1f)
                        );
                        outputImage.Save(outputpath);

                        break;
                    }
                }
            }
            string save = string.Join(';', values);

            await ReplyAsync($"{Context.User.Mention} Equipped {Item} ");

            await Data.Data.SaveInventory(Context.User.Id, save);
        }


        [Command("unequip"), Alias("Dequip")]
        public async Task dequipp([Remainder] string EquipPlace)
        {

            string inv = Data.Data.GetInventory(Context.User.Id);

            string[] valuesArr = inv.Split(';');
            List<string> values = valuesArr.ToList();

            string outputpath = $"D:/KushBot/Kush Bot/KushBotV2/KushBot/Data/Portraits/{Context.User.Id}.png";

            string item = "Empty";

            List<string> inventory = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();
            if (inventory.Count >= Data.Data.GetBackpackTier(Context.User.Id) * 3 + 6)
            {
                await ReplyAsync($"{Context.User.Mention} Your inventory is full.");
                return;
            }

            int SetCountPre = 0;


            if (EquipPlace.ToLower() == "mainhand")
            {
                item = Data.Data.GetMainHand(Context.User.Id);
                if (item == "Empty")
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have anything equipped there, moron");
                    return;
                }
                await Program.RemoveItemBonus(Context.User.Id, Data.Data.GetMainHand(Context.User.Id));
                SetCountPre = Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item));
                await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetMainHand(Context.User.Id));
                await Data.Data.SetMainHand(Context.User.Id, "Empty");

            }
            else if (EquipPlace.ToLower() == "offhand")
            {
                item = Data.Data.GetOffHand(Context.User.Id);
                if (item == "Empty")
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have anything equipped there, moron");
                    return;
                }
                await Program.RemoveItemBonus(Context.User.Id, Data.Data.GetOffHand(Context.User.Id));
                SetCountPre = Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item));
                await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetOffHand(Context.User.Id));
                await Data.Data.SetOffHand(Context.User.Id, "Empty");
            }
            else if (EquipPlace.ToLower() == "helmet")
            {
                item = Data.Data.GetHelmet(Context.User.Id);
                if (item == "Empty")
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have anything equipped there, moron");
                    return;
                }
                await Program.RemoveItemBonus(Context.User.Id, Data.Data.GetHelmet(Context.User.Id));
                SetCountPre = Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item));
                await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetHelmet(Context.User.Id));
                await Data.Data.SetHelmet(Context.User.Id, "Empty");
            }
            else if (EquipPlace.ToLower() == "necklace")
            {
                item = Data.Data.GetNecklace(Context.User.Id);
                if (item == "Empty")
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have anything equipped there, moron");
                    return;
                }
                await Program.RemoveItemBonus(Context.User.Id, Data.Data.GetNecklace(Context.User.Id));
                SetCountPre = Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item));
                await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetNecklace(Context.User.Id));
                await Data.Data.SetNecklace(Context.User.Id, "Empty");
            }
            else if (EquipPlace.ToLower() == "chest")
            {
                item = Data.Data.GetChest(Context.User.Id);
                if (item == "Empty")
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have anything equipped there, moron");
                    return;
                }
                await Program.RemoveItemBonus(Context.User.Id, Data.Data.GetChest(Context.User.Id));
                SetCountPre = Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item));
                await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetChest(Context.User.Id));
                await Data.Data.SetChest(Context.User.Id, "Empty");
            }
            else if (EquipPlace.ToLower() == "goggles")
            {
                item = Data.Data.GetGoggles(Context.User.Id);
                if (item == "Empty")
                {
                    await ReplyAsync($"{Context.User.Mention} You don't have anything equipped there, moron");
                    return;
                }
                await Program.RemoveItemBonus(Context.User.Id, Data.Data.GetGoggles(Context.User.Id));
                SetCountPre = Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item));
                await Data.Data.AddItemToBackpack(Context.User.Id, Data.Data.GetGoggles(Context.User.Id));
                await Data.Data.SetGoggles(Context.User.Id, "Empty");
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} That's not an eligible item slot, dumbass");
                return;
            }

            if (Data.Data.ItemSetName(item) != null)
            {
                if (SetCountPre == 3)
                {
                    await Program.RemoveSetBonus(Context.User.Id, Data.Data.ItemSetName(item));
                }
            }

            using (Image<Rgba32> img1 = SixLabors.ImageSharp.Image.Load($"Pictures/Ramons/Ramon{Data.Data.GetRamonIndex(Context.User.Id)}.jpg"))
            using (Image<Rgba32> Helm = SixLabors.ImageSharp.Image.Load($"Pictures/Helmets/{Data.Data.GetHelmet(Context.User.Id).ToLower()}.png"))
            using (Image<Rgba32> Neck = SixLabors.ImageSharp.Image.Load($"Pictures/Necklaces/{Data.Data.GetNecklace(Context.User.Id).ToLower()}.png"))
            using (Image<Rgba32> wep = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetMainHand(Context.User.Id)}.png"))
            using (Image<Rgba32> oh = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetOffHand(Context.User.Id)}.png"))
            using (Image<Rgba32> Goggles = SixLabors.ImageSharp.Image.Load($"Pictures/Goggles/{Data.Data.GetGoggles(Context.User.Id).ToLower()}.png"))
            using (Image<Rgba32> Chest = SixLabors.ImageSharp.Image.Load($"Pictures/Chests/{Data.Data.GetChest(Context.User.Id)}.png"))
            using (Image<Rgba32> Elemental = SixLabors.ImageSharp.Image.Load($"Pictures/Elementals/{Data.Data.GetElemental(Context.User.Id)}.png"))
            using (Image<Rgba32> outputImage = img1)
            {
                oh.Mutate(x => x.RotateFlip(RotateMode.None, FlipMode.Horizontal));

                outputImage.Mutate(x => x
                    .DrawImage(img1, 1f)
                    .DrawImage(Chest, 1f)
                    .DrawImage(Helm, 1f)
                    .DrawImage(Goggles, 1f)
                    .DrawImage(Neck, 1f)
                    .DrawImage(wep, 1f)
                    .DrawImage(oh, 1f)
                    .DrawImage(Elemental, 1f)
                );
                outputImage.Save(outputpath);

            }


            await ReplyAsync($"{Context.User.Mention} unequipped {item}");

        }
        [Command("add")]
        public async Task AddItem([Remainder] string item)
        {
            await Data.Data.AddItemToBackpack(Context.User.Id, item);
        }

        /*public string ItemDescription(string item)
        {
            string ret = "";

            string ExtraAP = $"+{Data.Data.GetItemApValue(item)} Attack power\n";
            string ExtraHP = $"+{Data.Data.GetItemHpValue(item)} Health\n";
            string ExtraRAP = $"+{Data.Data.GetItemRapValue(item)} Ranged attack power\n";
            string ExtraSP = $"+{Data.Data.GetItemSpValue(item)} Spell power\n";
            string ExtraArmor = $"+{Data.Data.GetItemArmorValue(item)} Armor\n";
            string ExtraAgi = $"+{Data.Data.GetItemAGIValue(item)} Agility\n";
            string SetBonus = "";

            if (Data.Data.ItemSetName(item) != null)
            {

                if (Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item)) < 3)
                {
                    SetBonus = $"~~**Set Bonus(3)**~~ {Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item))}/3\n";
                }
                else
                {
                    SetBonus = $"**Set Bonus(3)** {Program.SetItemCount(Context.User.Id, Data.Data.ItemSetName(item))}/3\n";
                }

                string SetAp = $"--- +{Data.Data.SetAp5(Data.Data.ItemSetName(item))} Attack power\n";
                string SetRap = $"--- +{Data.Data.SetRap5(Data.Data.ItemSetName(item))} Ranged attack power\n";
                string SetSp = $"--- +{Data.Data.SetSp5(Data.Data.ItemSetName(item))} spell power\n";
                string SetAgi = $"--- +{Data.Data.SetAgi5(Data.Data.ItemSetName(item))} Agility\n";
                string SetArmor = $"--- +{Data.Data.SetArmor5(Data.Data.ItemSetName(item))} Armor\n";

                if (Data.Data.SetAp5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetAp;
                }
                if (Data.Data.SetRap5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetRap;
                }
                if (Data.Data.SetSp5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetSp;
                }
                if (Data.Data.SetAgi5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetAgi;
                }
                if (Data.Data.SetArmor5(Data.Data.ItemSetName(item)) != 0)
                {
                    SetBonus += SetArmor;
                }


            }

            if (item == "Empty")
            {
                return ret;
            }

            if (Data.Data.GetItemStyle(item) != "Armor" && Data.Data.GetItemStyle(item) != "Food")
            {
                if (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) < Data.Data.GetItemLevelReq(item))
                {
                    ret = $"```diff\n-Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemStyle(item)} {Data.Data.GetItemEquipPlace(item)} {Data.Data.GetItemType(item)}.\nRaw Damage Range: {Data.Data.GetItemBottomEnd(item)}-{ Data.Data.GetItemTopEnd(item)}\n```";
                }
                else
                {
                    ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemStyle(item)} {Data.Data.GetItemEquipPlace(item)} {Data.Data.GetItemType(item)}.\nRaw Damage Range: **{Data.Data.GetItemBottomEnd(item)}-{ Data.Data.GetItemTopEnd(item)}**\n";
                }


                string rar = Data.Data.GetItemRarity(item);
                string Rarity = char.ToUpper(rar[0]) + rar.Substring(1);

                ret += $"Baps Value: {Data.Data.GetItemValue(item)} baps\n";
                ret += $"Rarity: {Rarity}\n";


                if (Data.Data.GetItemApValue(item) > 0)
                {
                    ret += ExtraAP;
                }
                if (Data.Data.GetItemRapValue(item) > 0)
                {
                    ret += ExtraRAP;
                }
                if (Data.Data.GetItemSpValue(item) > 0)
                {
                    ret += ExtraSP;
                }
                if (Data.Data.GetItemAGIValue(item) > 0)
                {
                    ret += ExtraAgi;
                }
                if (Data.Data.GetItemArmorValue(item) > 0)
                {
                    ret += ExtraArmor;
                }
                if(Data.Data.GetItemHpValue(item) > 0)
                {
                    ret += ExtraHP;
                }
            }
            else if (Data.Data.GetItemStyle(item) == "Armor")
            {
                if (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) < Data.Data.GetItemLevelReq(item))
                {
                    ret = $"```diff\n-Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)}. Armor Value: {Data.Data.GetItemArmorValue(item)}\n```";
                }
                else
                {
                    ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)}. Armor Value: **{Data.Data.GetItemArmorValue(item)}\n**";
                }
                if (Data.Data.GetItemApValue(item) > 0)
                {
                    ret += ExtraAP;
                }
                if (Data.Data.GetItemRapValue(item) > 0)
                {
                    ret += ExtraRAP;
                }
                if (Data.Data.GetItemSpValue(item) > 0)
                {
                    ret += ExtraSP;
                }
                if (Data.Data.GetItemAGIValue(item) > 0)
                {
                    ret += ExtraAgi;
                }
                if (Data.Data.GetItemHpValue(item) > 0)
                {
                    ret += ExtraHP;
                }
            }
            else if(Data.Data.GetItemStyle(item) == "Food")
            {
                if (Program.GetLevel(Data.Data.GetExperience(Context.User.Id)) < Data.Data.GetItemLevelReq(item))
                {
                    ret = $"```diff\n-Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)} **Healing item**\n```";
                }
                else
                {
                    ret = $"Level {Data.Data.GetItemLevelReq(item)} {Data.Data.GetItemType(item)} **Healing item**\n";
                }

               ret += ExtraHP;


            }
            ret += SetBonus;
            return ret;
        }*/
        [Command("Gear")]
        public async Task OpenGear()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Discord.Color.Gold);


            builder.WithTitle($"{Context.User.Username}'s Equipped gear");
            builder.AddField("**Helmet Slot**", $"-**{Data.Data.GetHelmet(Context.User.Id)}**\n{Program.ItemDescription(Data.Data.GetHelmet(Context.User.Id), Context.User.Id)}");
            builder.AddField("**Goggles Slot**", $"-**{Data.Data.GetGoggles(Context.User.Id)}**\n{Program.ItemDescription(Data.Data.GetGoggles(Context.User.Id), Context.User.Id)}");
            builder.AddField("**Necklace Slot**", $"-**{Data.Data.GetNecklace(Context.User.Id)}**\n{Program.ItemDescription(Data.Data.GetNecklace(Context.User.Id), Context.User.Id)}");
            builder.AddField("**Chest Slot**", $"-**{Data.Data.GetChest(Context.User.Id)}**\n{Program.ItemDescription(Data.Data.GetChest(Context.User.Id), Context.User.Id)}");
            builder.AddField("**Mainhand Slot**", $"-**{Data.Data.GetMainHand(Context.User.Id)}**\n{Program.ItemDescription(Data.Data.GetMainHand(Context.User.Id), Context.User.Id)}");
            builder.AddField("**Offhand Slot**", $"-**{Data.Data.GetOffHand(Context.User.Id)}**\n{Program.ItemDescription(Data.Data.GetOffHand(Context.User.Id), Context.User.Id)}");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("Summon")]
        public async Task SummonEle(string elemental)
        {
            if (Data.Data.GetClassName(Context.User.Id) != "Mage")
            {
                await ReplyAsync($"{Context.User.Mention} only a mage can understand elementals, all other classes are too retarded");
                return;
            }
            if (elemental.ToLower() != "fire" && elemental.ToLower() != "air" && elemental.ToLower() != "water" && elemental.ToLower() != "Earth")
            {
                await ReplyAsync($"{Context.User.Mention} that elemental doesn't exist");
                return;
            }

            await Data.Data.SetElemental(Context.User.Id, elemental);

            string outputpath = $"D:/KushBot/Kush Bot/KushBotV2/KushBot/Data/Portraits/{Context.User.Id}.png";

            using (Image<Rgba32> img1 = SixLabors.ImageSharp.Image.Load($"Pictures/Ramons/Ramon{Data.Data.GetRamonIndex(Context.User.Id)}.jpg"))
            using (Image<Rgba32> Helm = SixLabors.ImageSharp.Image.Load($"Pictures/Helmets/{Data.Data.GetHelmet(Context.User.Id).ToLower()}.png"))
            using (Image<Rgba32> Neck = SixLabors.ImageSharp.Image.Load($"Pictures/Necklaces/{Data.Data.GetNecklace(Context.User.Id).ToLower()}.png"))
            using (Image<Rgba32> wep = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetMainHand(Context.User.Id)}.png"))
            using (Image<Rgba32> oh = SixLabors.ImageSharp.Image.Load($"Pictures/Weapons/{Data.Data.GetOffHand(Context.User.Id)}.png"))
            using (Image<Rgba32> Goggles = SixLabors.ImageSharp.Image.Load($"Pictures/Goggles/{Data.Data.GetGoggles(Context.User.Id).ToLower()}.png"))
            using (Image<Rgba32> Chest = SixLabors.ImageSharp.Image.Load($"Pictures/Chests/{Data.Data.GetChest(Context.User.Id)}.png"))
            using (Image<Rgba32> Elemental = SixLabors.ImageSharp.Image.Load($"Pictures/Elementals/{Data.Data.GetElemental(Context.User.Id)}.png"))
            using (Image<Rgba32> outputImage = img1)
            {
                oh.Mutate(x => x.RotateFlip(RotateMode.None, FlipMode.Horizontal));

                outputImage.Mutate(x => x
                    .DrawImage(img1, 1f)
                    .DrawImage(Chest, 1f)
                    .DrawImage(Helm, 1f)
                    .DrawImage(Goggles, 1f)
                    .DrawImage(Neck, 1f)
                    .DrawImage(wep, 1f)
                    .DrawImage(oh, 1f)
                    .DrawImage(Elemental, 1f)
                );
                outputImage.Save(outputpath);

            }

        }

        [Command("Sell")]
        public async Task SellItem([Remainder]string itemName)
        {
            itemName = char.ToUpper(itemName[0]) + itemName.Substring(1);

            if (!Data.Data.GetInventory(Context.User.Id).Contains(itemName))
            {
                await ReplyAsync($"{Context.User.Mention} You don't have that item");
                return;
            }

            List<string> items = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();

            int baps = Data.Data.GetItemValue(itemName);

            await Data.Data.SaveBalance(Context.User.Id, baps);

            items.Remove(itemName);

            await Data.Data.SaveInventory(Context.User.Id, string.Join(';', items));

            await ReplyAsync($"{Context.User.Mention} sold {itemName} for {baps} baps.");

        }

        [Command("Use")]
        public async Task ConsumeItem([Remainder] string itemName)
        {
            List<string> inv = Data.Data.GetInventory(Context.User.Id).Split(';').ToList();

            string Item = "";

            string line = "";

            foreach (string item in inv)
            {
                if(item.ToLower() == itemName.ToLower())
                {
                    Item = item;
                }
            }

            if(Item == "")
            {
                await ReplyAsync($"{Context.User.Mention} You don't have that item");
                return;
            }

            if(Data.Data.GetItemStyle(Item) == "Food")
            {
                if(Data.Data.GetItemType(Item) == "Potion")
                {
                    double HpHeal = Data.Data.GetItemHpValue(Item);
                    HpHeal = Math.Round((HpHeal / 100) * Data.Data.GetMaxHealth(Context.User.Id), 0);

                    await Data.Data.Regenerate(Context.User.Id,(int)HpHeal);

                    if (Data.Data.GetCurHealth(Context.User.Id) > Data.Data.GetMaxHealth(Context.User.Id))
                    {
                        await Data.Data.SetCurHealth(Context.User.Id, Data.Data.GetMaxHealth(Context.User.Id));
                    }
                    line = $"Ate {Item} like the animal that he is, restored {Data.Data.GetItemHpValue(Item)}% ({HpHeal}) health and he now has {Data.Data.GetCurHealth(Context.User.Id)}/{Data.Data.GetMaxHealth(Context.User.Id)} HP";

                }
                else
                {
                    int HpHeal = Data.Data.GetItemHpValue(Item);
                    while(HpHeal > 0 && Data.Data.GetCurHealth(Context.User.Id) < Data.Data.GetMaxHealth(Context.User.Id))
                    {
                        HpHeal--;
                        await Data.Data.Regenerate(Context.User.Id, 1);
                    }
                    line = $"Ate {Item} like the animal that he is, restored {Data.Data.GetItemHpValue(Item)} health and he now has {Data.Data.GetCurHealth(Context.User.Id)}/{Data.Data.GetMaxHealth(Context.User.Id)} HP";
                }
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} ???????????????");
                return;
            }

            inv.Remove(Item);

            await ReplyAsync($"{Context.User.Mention} {line}");

            await Data.Data.SaveInventory(Context.User.Id, string.Join(';', inv));
        }

        [Command("desc")]
        public async Task checkStats([Remainder] string item)
        {
            item = char.ToUpper(item[0]) + item.Substring(1);

            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField($"{item}", $"{Program.ItemDescription(item, Context.User.Id)}");

            string rarity = Data.Data.GetItemRarity(item);

            switch(rarity)
            {
                case "common":
                    
                    break;
                case "uncommon":
                    builder.WithColor(Discord.Color.Green);
                    break;
                case "rare":
                    builder.WithColor(Discord.Color.Blue);
                    break;
                case "epic":
                    builder.WithColor(Discord.Color.Purple);
                    break;
            }

            await ReplyAsync("", false, builder.Build());
        }

    }

}
