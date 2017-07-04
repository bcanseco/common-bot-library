using System.Collections.Generic;
using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    public class WatsonPersonalityResult : TextAnalysisBase
    {
        public List<TraitTree> Personality { get; set; }
        public List<TraitTree> Values { get; set; }
        public List<TraitTree> Needs { get; set; }
        [JsonProperty("consumption_preferences")]
        public List<ConsumptionPreferencesCategory> ConsumptionPreferences { get; set; }

        #region Submodels
        public class TraitTree
        {
            [JsonProperty("trait_id")]
            public string TraitId { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public double Percentile { get; set; }
            [JsonProperty("raw_score")]
            public double RawScore { get; set; }
            public List<TraitTree> Children { get; set; }

            public override string ToString() => Name;
        }

        public class ConsumptionPreferencesCategory
        {
            [JsonProperty("consumption_preference_category_id")]
            public string ConsumptionPreferenceCategoryId { get; set; }
            public string Name { get; set; }
            [JsonProperty("consumption_preferences")]
            public List<ConsumptionPreference> ConsumptionPreferences { get; set; }

            public override string ToString() => Name;
        }

        public class ConsumptionPreference
        {
            [JsonProperty("consumption_preference_id")]
            public string ConsumptionPreferenceId { get; set; }
            public string Name { get; set; }
            public double Score { get; set; }

            public override string ToString() => Name;
        }
        #endregion
    }
}
