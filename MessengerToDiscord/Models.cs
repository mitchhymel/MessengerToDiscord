using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace MessengerToDiscord
{

    [DataContract]
    public class MessageFile
    {
        [DataMember(Name = "participants")]
        public List<Participant> Participants;

        [DataMember(Name = "messages")]
        public List<Message> Messages;
    }

    [DataContract]
    public class Participant
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class Media
    {
        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [JsonConverter(typeof(MyDateTimeConverter))]
        [DataMember(Name = "timestamp_ms")]
        public DateTime TimeStamp { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }
    }

    [DataContract]
    public class Share
    {
        [DataMember(Name = "link")]
        public string Link { get; set; }
    }


    public class MyDateTimeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = (long)reader.Value;
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var d = (DateTime)value;
            writer.WriteValue(d.ToLongDateString());
        }
    }



    [DataContract]
    public class Message
    {
        [DataMember(Name = "sender_name")]
        public string SenderName { get; set; }

        [JsonConverter(typeof(MyDateTimeConverter))]
        [DataMember(Name = "timestamp_ms")]
        public DateTime TimeStamp { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "share")]
        public Share Share { get; set; }

        [DataMember(Name = "audio_files")]
        public List<Media> AudioFiles { get; set; } = new List<Media>();

        [DataMember(Name = "files")]
        public List<Media> Files { get; set; } = new List<Media>();

        [DataMember(Name = "photos")]
        public List<Media> Photos { get; set; } = new List<Media>();

        [DataMember(Name = "gifs")]
        public List<Media> Gifs { get; set; } = new List<Media>();


        public Dictionary<string, Stream> GetFilesAsStreams(string path)
        {
            var result = new Dictionary<string, Stream>();

            foreach (Media media in AudioFiles)
            {
                AddMediaToDictionary(result, path, media);
            }

            foreach (Media media in Gifs)
            {
                AddMediaToDictionary(result, path, media);
            }

            foreach (Media media in Photos)
            {
                AddMediaToDictionary(result, path, media);
            }

            foreach (Media media in Files)
            {
                AddMediaToDictionary(result, path, media);
            }

            return result;
        }

        public string GetMessageContent()
        {
            return $"[{TimeStamp.ToLongDateString()} {TimeStamp.ToLongTimeString()}][{SenderName}]: {Content}";
        }

        private void AddMediaToDictionary(Dictionary<string, Stream> dict, string path, Media media)
        {
            string filePath = MessengerHelper.GetPathToMediaFromUri(path, media.Uri);
            FileStream stream = File.OpenRead(filePath);
            string[] pathSplit = filePath.Split("\\");
            string name = pathSplit[pathSplit.Length - 1];
            dict.Add(name, stream);
        }

    }

}

