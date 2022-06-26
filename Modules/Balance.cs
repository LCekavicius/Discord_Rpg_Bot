using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KushBot.Data;

namespace KushBot.Modules
{
    public class Balance : ModuleBase<SocketCommandContext>
    {

        [Command("Balance"), Alias("Bal", "Baps")]
        public async Task PingAsync()
        {

            int baps = Data.Data.GetBalance(Context.User.Id);

            if (baps < 30)
            {
                await ReplyAsync($"{Context.User.Mention} has {baps} Baps, fucking homeless");

            }else if(baps < 200)
            {
                await ReplyAsync($"{Context.User.Mention} has {baps} Baps, what an eyesore");
            }
            else if (baps < 500)
            {
                await ReplyAsync($"{Context.User.Mention} has {baps} Baps, Jewish aborigen");
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} has {baps} Baps, wtf");
            }

        }

        
    }
}
