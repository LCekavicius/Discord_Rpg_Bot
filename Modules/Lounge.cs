using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;
using Discord.Rest;
using System.Linq;

namespace KushBot.Modules
{
    public class Lounge : ModuleBase<SocketCommandContext>
    {
        [Command("Lounge")]
        public async Task ComeToLounge()
        {
            if(Data.Data.GetGameState(Context.User.Id) == 0)
            {
                await ReplyAsync($"Welcome {Context.User.Mention} To the Ramon lounge!. Since this is your first time here we're gonna help your poor-ass out by giving you some baps " +
                    $"and a weapon. Use **';help money'** for information about spending baps. do **';help inv'** for information on how to manage your items. **';help combat'** for combat information. " +
                    $"Use **';status'** to see your Ramon's stats and portrait. And **';help'** to see the help tabs.");
                await Data.Data.SaveBalance(Context.User.Id, 20);
                await Data.Data.SaveGameState(Context.User.Id, 1);
                string Class = Data.Data.GetClassName(Context.User.Id);
                if(Class.ToLower() == "rogue")
                {
                    await Data.Data.AddItemToBackpack(Context.User.Id, "Needle");
                }
                else if (Class.ToLower() == "archer")
                {
                    await Data.Data.AddItemToBackpack(Context.User.Id, "Pliausk");
                }
                else if (Class.ToLower() == "warrior")
                {
                    await Data.Data.AddItemToBackpack(Context.User.Id, "Rock");
                }
                else if (Class.ToLower() == "mage")
                {
                    await Data.Data.AddItemToBackpack(Context.User.Id, "Scuffedwand");
                }
            }
        }

    }
}
