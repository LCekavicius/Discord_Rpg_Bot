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

namespace KushBot.Modules
{
    public class StartRemastered : ModuleBase<SocketCommandContext>
    {
        IMessageChannel dump = Program._client.GetChannel(Program.DumpId) as IMessageChannel;

        int index;

        [Command("Start")]
        public async Task startt()
        {
            if (Data.Data.JewExists(Context.User.Id))
            {
                await ReplyAsync($"{Context.User.Mention} You have already created your Ramonian, Do ';reroll' if you want to reroll");
                return;
            }

            Program.RamonCreation.Add(new CreatingCharacter(Context.User.Id));

            EmbedBuilder builder = new EmbedBuilder();



            string intro = ($"{Context.User.Mention} This god forsaken land is plaqued with the autism unleashed by the evil warlock Zygizmund. " +
                $"You are one of the few Ramonians chosen to vanquish the evil from this land. You must bring Zygizmund and his followers to the grave.");

            builder.AddField($"Welcome {Context.User.Username} to {Program.WorldName}.", $"{intro}");
            builder.AddField($"Create your character", "To get started, first choose your Ramon.\n type ';select ramonId' (e.g. ';select 5') to select your Ramon or **;next** to check out other ramons");

            RestUserMessage picture = await dump.SendFileAsync($"D:/KushBot/Kush Bot/KushBotV2/KushBot/Pictures/Ramons/Block0.jpg") as RestUserMessage;

            string imgurl = picture.Attachments.First().Url;

            builder.WithImageUrl(imgurl);

            await ReplyAsync("", false, builder.Build());
        }
        [Command("Select")]
        public async Task Selectt(string ramonId)
        {
            foreach (CreatingCharacter item in Program.RamonCreation)
            {
                if (item.UserId == Context.User.Id)
                {
                    if (item.Stage == 2)
                    {
                        if (ramonId.ToLower() != "warrior" && ramonId.ToLower() != "archer" && ramonId.ToLower() != "mage" && ramonId.ToLower() != "rogue")
                        {
                            await ReplyAsync($"{Context.User.Mention} that class doesn't exist, dumbass");
                            return;
                        }
                        ramonId = char.ToUpper(ramonId[0]) + ramonId.ToLower().Substring(1);
                        item.ClassName = ramonId;
                        await Finalize(item);
                        break;
                    }
                    else
                    {
                        if(int.Parse(ramonId) > 35 || int.Parse(ramonId) < 0)
                        {
                            await ReplyAsync($"{Context.User.Mention} Ramon of that ID doesnt exist");
                            return;
                        }
                        item.RamonIndex = int.Parse(ramonId);
                        item.Stage += 1;
                        await RollClasses();
                        return;
                    }
                }
            }
        }

        public async Task RollClasses()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("Now it's time for your class!", $"{Context.User.Mention} You can choose between 4 classes. Those classes are: **Warrior, Archer, Mage** and **Rogue** " +
                $"use the command ';select className' (e.g. ;select Warrior) to select your class. (Case sensitive)");

            await ReplyAsync("", false, builder.Build());
        }

        public async Task Finalize(CreatingCharacter item)
        {
            await Data.Data.MakeRowForJew(item.UserId);
            await Data.Data.SaveClassName(Context.User.Id, item.ClassName);
            await Data.Data.SaveRamonId(Context.User.Id, item.RamonIndex);


            if (Data.Data.GetClassName(Context.User.Id).ToLower() == "warrior")
            {
                await Data.Data.SaveMaxHealth(Context.User.Id, 30);
                await Data.Data.SetCurHealth(Context.User.Id, 30);
                await Data.Data.SaveAttackPower(Context.User.Id, 8);
                await Data.Data.SaveRangedAttackpower(Context.User.Id, 4);
                await Data.Data.SaveSpellPower(Context.User.Id, 2);
                await Data.Data.SaveArmor(Context.User.Id, 5);
            }
            else if (Data.Data.GetClassName(Context.User.Id).ToLower() == "mage")
            {
                await Data.Data.SaveMaxHealth(Context.User.Id, 20);
                await Data.Data.SetCurHealth(Context.User.Id, 20);
                await Data.Data.SaveAttackPower(Context.User.Id, 2);
                await Data.Data.SaveRangedAttackpower(Context.User.Id, 5);
                await Data.Data.SaveSpellPower(Context.User.Id, 10);
                await Data.Data.SaveArmor(Context.User.Id, 2);

            }
            else if (Data.Data.GetClassName(Context.User.Id).ToLower() == "rogue")
            {
                await Data.Data.SaveMaxHealth(Context.User.Id, 24);
                await Data.Data.SetCurHealth(Context.User.Id, 24);
                await Data.Data.SaveAttackPower(Context.User.Id, 9);
                await Data.Data.SaveRangedAttackpower(Context.User.Id, 5);
                await Data.Data.SaveSpellPower(Context.User.Id, 4);
            }
            else if (Data.Data.GetClassName(Context.User.Id).ToLower() == "archer")
            {
                await Data.Data.SaveMaxHealth(Context.User.Id, 24);
                await Data.Data.SetCurHealth(Context.User.Id, 24);
                await Data.Data.SaveAttackPower(Context.User.Id, 5);
                await Data.Data.SaveRangedAttackpower(Context.User.Id, 9);
                await Data.Data.SaveSpellPower(Context.User.Id, 4);
            }

            string image1 = $"D:/KushBot/Kush Bot/KushBotV2/KushBot/Pictures/Ramons/Ramon{Program.RamonCreation[index].RamonIndex}.jpg";

            string outputpath = $"D:/KushBot/Kush Bot/KushBotV2/KushBot/Data/Portraits/{Context.User.Id}.png";

            Image<Rgba32> Portrait = SixLabors.ImageSharp.Image.Load($"D:/KushBot/Kush Bot/KushBotV2/KushBot/Pictures/Ramons/Ramon{item.RamonIndex}.jpg");

            Portrait.Save(outputpath);

            Program.RamonCreation.Remove(item);

            await ReplyAsync($"{Context.User.Mention} You have finished building you Ramon! We highly recommended checking out the Ramon lounge by typing ';lounge' to get yourself going. Don't forget to use " +
                   $"';help' for information.");
        }

        [Command("next")]
        public async Task NextRamon()
        {
            int PlayerIndex = 0;

            for(int i = 0; i < Program.RamonCreation.Count; i++)
            {
                if(Program.RamonCreation[i].UserId == Context.User.Id)
                {
                    PlayerIndex = i;
                    break;
                }
            }

            if(Program.RamonCreation[PlayerIndex].RamonBlockId == 3)
            {
                await ReplyAsync($"{Context.User.Mention} no ramons on that side");
                return;
            }

            Program.RamonCreation[PlayerIndex].RamonBlockId += 1;
            
            await ShowMoreRamons(Program.RamonCreation[PlayerIndex].RamonBlockId);
        }

        [Command("prev")]
        public async Task PrevRamon()
        {
            int PlayerIndex = 0;

            for (int i = 0; i < Program.RamonCreation.Count; i++)
            {
                if (Program.RamonCreation[i].UserId == Context.User.Id)
                {
                    PlayerIndex = i;
                    break;
                }
            }

            if (Program.RamonCreation[PlayerIndex].RamonBlockId == 0)
            {
                await ReplyAsync($"{Context.User.Mention} no ramons on that side");
                return;
            }

            Program.RamonCreation[PlayerIndex].RamonBlockId -= 1;

            await ShowMoreRamons(Program.RamonCreation[PlayerIndex].RamonBlockId);
        }

        public async Task ShowMoreRamons(int ind)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField($"{RandomMessage()}", $"{Context.User.Mention} type ';select ramonId' (e.g. ';select 5') to pick your Ramon, **;next** to check the others and **;prev** to go back.");

            RestUserMessage picture = await dump.SendFileAsync($"D:/KushBot/Kush Bot/KushBotV2/KushBot/Pictures/Ramons/Block{ind}.jpg") as RestUserMessage;
            string imgurl = picture.Attachments.First().Url;

            builder.WithImageUrl(imgurl);

            await ReplyAsync("", false, builder.Build());

        }

        public string RandomMessage()
        {
            Random rad = new Random();
            List<string> Messages = new List<string>();
            Messages.Add($"Didn't like that one? how about this?");
            Messages.Add($"Make up your mind already.");
            Messages.Add($"Jesus, you're slow");
            Messages.Add($"Wanna hurry up?");
            Messages.Add($"You're taking so f*cking long...");
            Messages.Add($"Angry... it makes me feel angry!");
            Messages.Add($"Are you mentally challenged?");
            Messages.Add($"How about this one?");
            Messages.Add($"My syndroms may be down, but my hopes are up!");
            Messages.Add($"Please hurry up");
            Messages.Add($"Do it faster, will you?");
            Messages.Add($"Ass");
            Messages.Add($"**npc noises**");




            return Messages[rad.Next(0, Messages.Count)];
        }

    }
}
