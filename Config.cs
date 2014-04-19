using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TweetShock
{
    public class Config
    {
        public string UserAccessToken = "";
        public string UserAccessSecret = "";
        public string ConsumerKey = "";
        public string ConsumerSecret = "";
        public bool InitializeMessage = true;
        public string InitializeMessageTemplate = "Server is now online! IP: {ip} Port: {port}";

        public void Write(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Config Read(string path)
        {
            if (!File.Exists(path))
            {
                return new Config();
            }
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }
    }
}
