using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonBotLibrary
{
    public partial class Tokens
    {
        /// <summary>
        ///   Initializes all static references to third-party API keys.
        /// </summary>
        /// <param name="keysPath">The path to a tokens.json file.</param>
        /// <exception cref="FileNotFoundException">
        ///   Thrown if the file at <paramref name="keysPath"/> cannot be found.
        /// </exception>
        /// <exception cref="JsonReaderException">
        ///   Thrown if the file is found but has a syntax error.
        /// </exception>
        public static Task LoadAsync(string keysPath = "../../tokens.json")
            => LoadAsync<Tokens>(keysPath);

        /// <summary>
        ///   Initializes all static references to third-party API keys
        ///   into the provided type T. Use this if you have a class with 
        ///   additional static API keys that subclasses <see cref="Tokens"/>.
        /// </summary>
        /// <typeparam name="T">The class to deserialize into.</typeparam>
        /// <param name="keysPath">The path to a tokens.json file.</param>
        /// <exception cref="FileNotFoundException">
        ///   Thrown if the file at <paramref name="keysPath"/> cannot be found.
        /// </exception>
        /// <exception cref="JsonReaderException">
        ///   Thrown if the file is found but has a syntax error.
        /// </exception>
        public static Task LoadAsync<T>(string keysPath = "../../tokens.json") where T : Tokens
        {
            JsonConvert.DeserializeObject<T>(File.ReadAllText(keysPath));
            return Task.CompletedTask;
        }

        /// <summary> 
        ///   OpenWeatherMap API key. 
        /// </summary>
        [JsonProperty]
        public static string OpenWeatherMap { internal get; set; }

        /// <summary>
        ///   OMDb API key.
        /// </summary>
        [JsonProperty]
        public static string Omdb { internal get; set; }

        /// <summary>
        ///   Imgur client ID.
        /// </summary>
        [JsonProperty]
        public static string Imgur { internal get; set; }

        /// <summary>
        ///   Yandex Translate API key.
        /// </summary>
        [JsonProperty]
        public static string Yandex { internal get; set; }

        /// <summary> 
        ///   Contains Google Cloud Platform API key and Custom Search Engine ID.
        /// </summary>
        [JsonProperty]
        public static GoogleCredentials Google { get; set; }
        
        /// <summary> 
        ///   Contains Twitter developer credentials. See TweetInvi for docs.
        /// </summary>
        [JsonProperty]
        public static TwitterCredentials Twitter { get; set; }

        /// <summary> 
        ///   Contains MyAnimeList account username and password.
        /// </summary>
        [JsonProperty]
        public static MyAnimeListCredentials MyAnimeList { get; set; }

        /// <summary>
        ///   Contains Github repository and authentication credentials.
        /// </summary>
        [JsonProperty]
        public static GithubCredentials Github { get; set; }
    }
}
