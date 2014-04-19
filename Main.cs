using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Newtonsoft.Json;
using Tweetinvi;
using Tweetinvi.Core;


namespace TweetShock
{
    [ApiVersion(1, 15)]
    public class TweetShock : TerrariaPlugin
    {
        string path = Path.Combine(TShock.SavePath, "TweetShock.json");
        Config Config = new Config();
        public override string Name
        {
            get
            {
                return "TweetShock";
            }
        }
        public override string Author
        {
            get
            {
                return "Loganizer";
            }
        }
        public override string Description
        {
            get
            {
                return "Lets you post tweets in-game.";
            }
        }
        public override Version Version
        {
            get
            {
                return new Version("1.0");
            }
        }
        public TweetShock(Main game)
            : base(game)
        {
            Order = 1;
        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInitialize);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.GamePostInitialize.Deregister(this, OnPostInitialize);
            }
            base.Dispose(disposing);           
        }
        public void OnInitialize(EventArgs args)
        {
            if (!File.Exists(path))
            {
                Config.Write(path);
            }
            Config = Config.Read(path);
            TwitterCredentials.SetCredentials("UserAccessToken", "UserAccessSecret", "ConsumerKey", "ConsumerSecret");
            //add commands here
            Commands.ChatCommands.Add(new Command("tweet.tweet", SendTweet, "tweet"));
        }
        public void OnPostInitialize(EventArgs e)
        {

        }
        void SendTweet(CommandArgs e)
        {
            string msg = string.Join(" ", e.Parameters);
            Tweet.PublishTweet(msg);
            
        }
        bool SendTweet(string msg, out string error)
        {
            Tweet.PublishTweet(msg);
            var exID = ExceptionHandler.GetLastException().StatusCode;
            if (exID != 200)
            {
                if (exID == 401)
                {
                    
                    Log.ConsoleError("Tweet failed to post! Reason: your authentication keys are incorrect or missing. Please correct/add keys to the TweetShock.json file!");
                    Log.Error("Tweet failed to post! Reason: your authentication keys are incorrect or missing. Please correct/add keys to the TweetShock.json file!");
                    error = "Your authentication keys are incorrect or missing. Please correct/add keys to the TweetShock.json file!";
                    return false;
                }
                var exMessage = ExceptionHandler.GetLastException().TwitterDescription;
                Log.ConsoleError("Tweet failed to post! Reason: " + exMessage);
                Log.Error("Tweet failed to post! Reason: " + exMessage);
                error = exMessage;
                return false;
            }
            else
            {
                error = "";
                return true;
            }
        }
    }
}
