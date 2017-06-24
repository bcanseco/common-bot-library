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
        EntertainmentBooks = 10,
        EntertainmentFilm = 11,
        EntertainmentMusic = 12,
        EntertainmentMusicalsTheatres = 13,
        EntertainmentTelevision = 14,
        EntertainmentVideoGames = 15,
        EntertainmentBoardGames = 16,
        ScienceNature = 17,
        ScienceComputers = 18,
        ScienceMathematics = 19,
        Mythology = 20,
        Sports = 21,
        Geography = 22,
        History = 23,
        Politics = 24,
        Art = 25,
        Celebrities = 26,
        Animals = 27,
        Vehicles = 28,
        EntertainmentComics = 29,
        ScienceGadgets = 30,
        EntertainmentJapaneseAnimeManga = 31,
        EntertainmentCartoonAnimations = 32
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
