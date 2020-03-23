using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MessengerToDiscord
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting");

            // Given path to directory containing all necessary files for thread
            // e.g. messages/inbox/bobsmith_ybdiekl/
            // should have a bunch of .json files and more folders like /gifs, /photos, /videos, etc.
            if (args.Length != 3)
            {
                throw new Exception("Usage: messengertodiscord.exe [pathToThreadFromMessengerBackup] [discordChannelId] [botToken]");
            }

            string path = args[0];
            ulong channelId = ulong.Parse(args[1]);
            string token = args[2];

            MessengerHelper helper = new MessengerHelper();
            helper.Initialize(path);

            Console.WriteLine("Parsed messages... setting up discord bot");
            DiscordHelper discord = new DiscordHelper(token);
            await discord.SetUpDiscord();

            Console.WriteLine("Writing messages to discord...");
            await discord.WriteMessagesToChannel(channelId, helper.Messages, path);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
