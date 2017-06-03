using Newtonsoft.Json;

namespace CommonBotLibrary
{
    public partial class Tokens
    {
        public class GoogleCredentials
        {
            [JsonConstructor]
            public GoogleCredentials(string platformKey, string engineId)
                => (PlatformKey, EngineId) = (platformKey, engineId);

            public string PlatformKey { internal get; set; }
            public string EngineId { internal get; set; }
        }

        public class MyAnimeListCredentials
        {
            [JsonConstructor]
            public MyAnimeListCredentials(string username, string password)
                => (Username, Password) = (username, password);

            public string Username { internal get; set; }
            public string Password { internal get; set; }
        }

        public class TwitterCredentials : Tweetinvi.Models.TwitterCredentials
        {
            public new string ConsumerKey
            {
                internal get => base.ConsumerKey;
                set => base.ConsumerKey = value;
            }

            public new string ConsumerSecret
            {
                internal get => base.ConsumerSecret;
                set => base.ConsumerSecret = value;
            }

            public new string AccessToken
            {
                internal get => base.AccessToken;
                set => base.AccessToken = value;
            }

            public new string AccessTokenSecret
            {
                internal get => base.AccessTokenSecret;
                set => base.AccessTokenSecret = value;
            }
        }
    }
}
