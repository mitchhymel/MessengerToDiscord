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
            if (args.Length != 1)
            {
                throw new Exception("Must supply only one argument that is the path to the directory containing thread files.");
            }

            string path = args[0];
            MessengerHelper helper = new MessengerHelper();
            helper.Initialize(path);

            foreach (Message message in helper.Messages)
            {
                Console.WriteLine(JsonConvert.SerializeObject(message));
            }

            Console.WriteLine("Parsed messages... setting up discord bot");
            DiscordHelper discord = new DiscordHelper();
            await discord.SetUpDiscord();


            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
