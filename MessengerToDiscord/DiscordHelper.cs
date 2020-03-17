using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessengerToDiscord
{
    public class DiscordHelper
    {
        DiscordClient Discord = new DiscordClient(new DiscordConfiguration
        {
            Token = Secrets.TOKEN,
            TokenType = TokenType.Bot
        });

        public DiscordHelper()
        {

        }

        public async Task<DiscordClient> SetUpDiscord()
        {
            // Connect
            await Discord.ConnectAsync();

            return Discord;
        }


        public async Task WriteMessagesToChannel(ulong channelId, List<Message> messages)
        {

        }

        
    }
}
