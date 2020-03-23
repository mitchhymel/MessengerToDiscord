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
        DiscordClient Discord { get; set; }

        public DiscordHelper(string token)
        {
            Discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });
        }

        public async Task<DiscordClient> SetUpDiscord()
        {
            // Connect
            await Discord.ConnectAsync();

            return Discord;
        }


        public async Task WriteMessagesToChannel(ulong channelId, List<Message> messages, string path)
        {
            var channel = await Discord.GetChannelAsync(channelId);

            // Remove all messages that have already been sent
            // check latest discord message and find facebook message with same timestamp
            // remove all messages before it
            var discordMessages = await channel.GetMessagesAsync();

            if (discordMessages.Count > 0)
            {
                var first = discordMessages[0];
                int index = messages.FindIndex((m) => first.Content == m.GetMessageContent());

                if (index > 0)
                {
                    messages.RemoveRange(0, index + 1);
                }
            }

            foreach (Message message in messages)
            {
                var content = message.GetMessageContent();

                var files = message.GetFilesAsStreams(path);

                if (files.Count > 0)
                {
                    DiscordMessage resp = await channel.SendMultipleFilesAsync(files, content);
                }
                else
                {
                    DiscordMessage resp = await channel.SendMessageAsync(content);
                }

                // Sleep for 1000 msec
                System.Threading.Thread.Sleep(1000);
            }
            
        }

        
    }
}
