# Common Bot Library [![NuGet](https://img.shields.io/nuget/v/CommonBotLibrary.svg?maxAge=2592000)](https://www.nuget.org/packages/CommonBotLibrary) [![Build status](https://ci.appveyor.com/api/projects/status/mcli9fa0refvotfm?svg=true)](https://ci.appveyor.com/project/bcanseco/common-bot-library)

> Tired of rewriting the same code for all your bots? *The Common Bot Library* (CBL) is for you.

CBL is an asynchronous .NET library of API wrappers designed to help with typical bot commands. Things like weather, definitions, movie information, etc. are all readily available for your projects.

```c#
ICalculatorService calc = new NCalcService();
var result = await calc.EvaluateAsync("1 + (3 / (23 - 5) * 6)");
Console.WriteLine(result); // 2
```
**You can check out the full list of services [here](#services).**

## Quick Start
If you're using Visual Studio, you can install this library by following [these instructions](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui).  
The package name on NuGet is `CommonBotLibrary`. **Note:** this library targets [.NET Standard 1.6](https://docs.microsoft.com/en-us/dotnet/standard/library#net-platforms-support).

### Authentication
You have two choices for services that require API keys or other tokens.
1. Construct the service by passing in the necessary keys as arguments.
2. Load in tokens from a file once, then forget about 'em.

```c#
/* Method 1 */
var service = new GoogleService("platformKey", "engineId");
var results = await service.SearchAsync("dogs");

/* Method 2 */
await Tokens.LoadAsync("mytokens.json");
var service = new GoogleService(); // notice we don't pass any keys in!
var results = await service.SearchAsync("dogs");
```

In the second example, we keep a JSON file with our API keys and import them before creating our services. This process only needs to be done once unless your tokens file changes.  

#### Extensibility
What if your project has extra services that need API keys?
For example, take this token file:
```json
{
  "OpenWeatherMap": "Im_a_key_that_CBL_recognizes",
  "MyCustomDatabaseString": "Im_a_custom_key"
}
```
You can take advantage of the CBL token system by subclassing the [Tokens](src/CommonBotLibrary/Tokens.cs) file.
```c#
public class MyCustomTokens : Tokens
{
    [JsonProperty]
    public string MyCustomDatabaseString { get; set; }
}
```
Then, instead of calling `LoadAsync("mytokens.json")` normally, you would use your custom token class as a typeparam:
```c#
await Tokens.LoadAsync<MyCustomTokens>("mytokens.json");
var databaseConnection = MyCustomTokens.MyCustomDatabaseString;
```
Now you can use all the CBL services plus your own custom services without having to worry about API keys.

### Services
* **Google Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.GoogleService.html)**
   * Searches https://google.com for webpages
   * Requires a [Cloud Platform API key](https://support.google.com/cloud/answer/6158862?hl=en) and a [Custom Search Engine ID](https://support.google.com/customsearch/answer/2649143?hl=en).
   * To search all of Google, set your engine to ["search the entire web"](https://support.google.com/customsearch/answer/2631040?hl=en) but don't add any emphasized sites.
* **MyAnimeList Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.MyAnimeListService.html)**
   * Searches https://myanimelist.net for anime.
   * Requires a [MAL account](https://myanimelist.net/register.php) username and password.
* **NCalc Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.NCalcService.html)**
   * Evaluates mathematical expressions.
* **Omdb Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.OmdbService.html)**
   * Searches https://omdbapi.com for movies.
   * Requires an [API key](http://www.omdbapi.com/apikey.aspx).
* **OpenWeatherMap Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.OpenWeatherMapService.html)**
   * Searches https://openweathermap.org for weather data.
   * Requires an [API key](https://openweathermap.org/appid).
* **Random Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.RandomService.html)**
   * Flips coins and rolls dice.
   * Good for when you have users input their desired die side numbers (e.g. `d20`). Everything is validated.
* **Reddit Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.RedditService.html)**
   * Gets posts from https://reddit.com.
* **Steam Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.SteamService.html)**
   * Searches https://store.steampowered.com for games.
* **Twitter Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.TwitterService.html)**
   * Searches https://twitter.com for tweets.
   * Requires four keys that can be registered [here](https://apps.twitter.com/).
* **UrbanDictionary Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.UrbanDictionaryService.html)**
   * Searches https://urbandictionary.com for definitions.
* **Yahoo Finance Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.YahooFinanceService.html)**
   * Searches https://finance.yahoo.com/ for stock information.
* **YouTube Service [[API docs]](https://bcanseco.github.io/common-bot-library/api/CommonBotLibrary.Services.YouTubeService.html)**
   * Searches https://youtube.com for videos.
   * Requires a [Google Cloud Platform API key](https://support.google.com/cloud/answer/6158862?hl=en).

More services are constantly added. If you prefer to keep things tidy, there are [interfaces](src/CommonBotLibrary/Interfaces) available.

## Contributing
This project is actively maintained! Please feel free to open an issue or submit a pull request if you have a suggestion, an idea, run into an issue, or have any other questions. Everything is responded to within 24 hours.

## License

[MIT](LICENSE)
