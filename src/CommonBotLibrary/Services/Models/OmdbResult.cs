using System.Collections.Generic;
using System.Diagnostics;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Services.Models
{
    public class OmdbSearchResult : MovieBase
    {
        public string ImdbId { get; set; }
        public string Poster { get; set; }
        public OmdbType Type { get; set; }
    }

    public class OmdbDirectResult : OmdbSearchResult
    {
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Metascore { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbVotes { get; set; }
        public string Dvd { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public IList<Rating> Ratings { get; set; }

        [DebuggerDisplay("{Source, nq}: {Value}")]
        public class Rating
        {
            public string Source { get; set; }
            public string Value { get; set; }
        }
    }

    public enum OmdbType
    {
        Movie,
        Series,
        Episode
    }
}
