using System;
using System.Collections.Generic;
using System.Diagnostics;
using CommonBotLibrary.Converters;
using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    [DebuggerDisplay("{Id, nq}: {Title}")]
    public class SteamResult : IWebpage, IEquatable<SteamResult>
    {
        public uint Id { get; }
        [JsonProperty("tiny_image")]
        public string ImageUrl { get; set; }
        public string Metascore { get; set; }
        public bool StreamingVideo { get; set; }
        public Cost Price { get; set; }
        public Architecture Platforms { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        [JsonConstructor]
        public SteamResult(
            string name,
            uint id,
            string tinyImage,
            string metascore,
            bool streamingVideo,
            Cost price,
            Architecture platforms)
        {
            Title = name;
            Url = $"https://store.steampowered.com/app/{id}";

            Id = id;
            ImageUrl = tinyImage;
            Metascore = string.IsNullOrEmpty(metascore) ? null : metascore;
            StreamingVideo = streamingVideo;
            Price = price;
            Platforms = platforms;
        }

        #region IEquatable members
        public bool Equals(SteamResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as SteamResult;
            return other != null && Equals(other);
        }

        public override int GetHashCode() => (int) Id;

        public static bool operator ==(SteamResult left, SteamResult right)
            => Equals(left, right);

        public static bool operator !=(SteamResult left, SteamResult right)
            => !Equals(left, right);
        #endregion

        #region Submodels
        public class Architecture
        {
            private string _asString;

            public bool Windows { get; set; }
            public bool Mac { get; set; }
            public bool Linux { get; set; }

            public override string ToString()
            {
                if (_asString != null) return _asString;

                var supported = new List<string>();

                if (Windows) supported.Add(nameof(Windows));
                if (Mac) supported.Add(nameof(Mac));
                if (Linux) supported.Add(nameof(Linux));

                return _asString = string.Join(", ", supported);
            }
        }

        [DebuggerDisplay("R: ${Regular, nq}, S: ${Sale, nq}")]
        public class Cost
        {
            [JsonConstructor]
            public Cost(int initial, int final)
            {
                Regular = (float)initial / 100;
                Sale = (float)final / 100;
            }

            public float Regular { get; set; }
            public float Sale { get; set; }

            public bool IsOnSale => Regular > Sale;
        }
        #endregion
    }

    public class SteamSpyData
    {
        public uint AppId { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        [JsonProperty("score_rank")]
        public string ScoreRank { get; set; }
        public uint Owners { get; set; }
        [JsonProperty("owners_variance")]
        public uint OwnersVariance { get; set; }
        [JsonProperty("players_forever")]
        public uint PlayersForever { get; set; }
        [JsonProperty("players_forever_variance")]
        public uint PlayersForeverVariance { get; set; }
        [JsonProperty("players_2weeks")]
        public uint Players2Weeks { get; set; }
        [JsonProperty("players_2weeks_variance")]
        public uint Players2WeeksVariance { get; set; }
        [JsonProperty("average_forever")]
        public uint AverageForever { get; set; }
        [JsonProperty("average_2weeks")]
        public uint Average2Weeks { get; set; }
        [JsonProperty("median_forever")]
        public uint MedianForever { get; set; }
        [JsonProperty("median_2weeks")]
        public uint Median2Weeks { get; set; }
        public uint Ccu { get; set; }
        public string Price { get; set; }
        [JsonConverter(typeof(DictionaryOrEmptyArrayConverter<string, int>))]
        public IDictionary<string, int> Tags { get; set; }
    }
}
