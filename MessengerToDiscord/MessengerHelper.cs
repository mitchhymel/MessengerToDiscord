using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessengerToDiscord
{
    public class MessengerHelper
    {
        public List<Message> Messages { get; set; }

        private string Path { get; set; }

        public MessengerHelper()
        {
            Messages = new List<Message>();
        }

        public void Initialize(string path)
        {
            Path = path;

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.EndsWith(".json"))
                {
                    // parse json file
                    ParseJsonFile(file);
                }
            }
        }

        public void ParseJsonFile(string filePath)
        {
            MessageFile file = JsonConvert.DeserializeObject<MessageFile>(File.ReadAllText(filePath));
            Messages.AddRange(file.Messages);
        }

        public string GetPathToMediaFromUri(string uri)
        {
            string[] uriSplit = uri.Split();
            string mediaType = uriSplit[uriSplit.Length - 2];
            string fileName = uriSplit[uriSplit.Length - 1];
            return $"{Path}/{mediaType}/{fileName}";
        }
    }
}
