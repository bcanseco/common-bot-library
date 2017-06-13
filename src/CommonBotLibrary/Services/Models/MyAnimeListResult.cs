using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Services.Models
{
    public class MyAnimeListResult : AnimeBase, IWebpage, IEquatable<MyAnimeListResult>
    {
        public MyAnimeListResult(XContainer xml)
        {
            Id = (int)xml.Element("id");

            Title = (string)xml.Element("title");
            English = (string)xml.Element("english");
            Synonyms = (string)xml.Element("synonyms");
            Episodes = (int)xml.Element("episodes");
            Score = (double)xml.Element("score");
            Type = (string)xml.Element("type");
            Status = (string)xml.Element("status");
            StartDate = (string)xml.Element("start_date");
            EndDate = (string)xml.Element("end_date");
            Image = (string)xml.Element("image");

            // Remove BBcode and other extraneous characters
            Synopsis = WebUtility.HtmlDecode(Regex
                .Replace((string)xml.Element("synopsis"), @"\[.*?\]|<.*?>", ""));
        }

        /// <summary>
        ///   Unique MyAnimeList anime identifier.
        /// </summary>
        public int Id { get; }
        public string Url => $"https://myanimelist.net/anime/{Id}";

        #region IEquatable members
        public bool Equals(MyAnimeListResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as MyAnimeListResult;
            return other != null && Equals(other);
        }

        public override int GetHashCode() => Id;

        public static bool operator ==(MyAnimeListResult left, MyAnimeListResult right)
            => Equals(left, right);

        public static bool operator !=(MyAnimeListResult left, MyAnimeListResult right)
            => !Equals(left, right);
        #endregion
    }
}
