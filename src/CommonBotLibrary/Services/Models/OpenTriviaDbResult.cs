using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    /// <summary>
    ///   A model for OpenTriviaDb responses. Question and answer strings are HTML encoded.
    /// </summary>
    public class OpenTriviaDbResult : TriviaBase
    {
        /// <summary> "multiple" for multiple choice or "boolean" for true/false </summary>
        public string Type { get; set; }
        /// <summary> "easy", "medium", or "hard" </summary>
        public string Difficulty { get; set; }
        [JsonProperty("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonProperty("incorrect_answers")]
        public IList<string> IncorrectAnswers { get; set; }
    }

    #region Query models
    public enum OpenTriviaDbCategory
    {
        Any = 0,
        GeneralKnowledge = 9,
        Books = 10,
        Film = 11,
        Music = 12,
        MusicalsTheatres = 13,
        Television = 14,
        VideoGames = 15,
        BoardGames = 16,
        ScienceNature = 17,
        Computers = 18,
        Mathematics = 19,
        Mythology = 20,
        Sports = 21,
        Geography = 22,
        History = 23,
        Politics = 24,
        Art = 25,
        Celebrities = 26,
        Animals = 27,
        Vehicles = 28,
        Comics = 29,
        Gadgets = 30,
        AnimeManga = 31,
        CartoonAnimations = 32
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum OpenTriviaDbDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum OpenTriviaDbType
    {
        Multiple,
        Boolean
    }
    #endregion
}
