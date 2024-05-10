using Newtonsoft.Json;

namespace CopilotAdherence.Features.Metrics.Common
{
    public class DailyStatistics
    {
        [JsonProperty("day")]
        public DateTime Day { get; set; }

        [JsonProperty("total_suggestions_count")]
        public int TotalSuggestionsCount { get; set; }

        [JsonProperty("total_acceptances_count")]
        public int TotalAcceptancesCount { get; set; }

        [JsonProperty("total_lines_suggested")]
        public int TotalLinesSuggested { get; set; }

        [JsonProperty("total_lines_accepted")]
        public int TotalLinesAccepted { get; set; }

        [JsonProperty("total_active_users")]
        public int TotalActiveUsers { get; set; }

        [JsonProperty("total_chat_acceptances")]
        public int TotalChatAcceptance { get; set; }

        [JsonProperty("total_chat_turns")]
        public int TotalChatTurns { get; set; }

        [JsonProperty("total_active_chat_users")]
        public int TotalActiveChatUsers { get; set; }


        [JsonProperty("breakdown")]
        public List<LanguageBreakdown> Breakdown { get; set; }
    }

    public class LanguageBreakdown
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("editor")]
        public string Editor { get; set; }

        [JsonProperty("suggestions_count")]
        public int SuggestionsCount { get; set; }

        [JsonProperty("acceptances_count")]
        public int AcceptancesCount { get; set; }

        [JsonProperty("lines_suggested")]
        public int LinesSuggested { get; set; }

        [JsonProperty("lines_accepted")]
        public int LinesAccepted { get; set; }

        [JsonProperty("active_users")]
        public int ActiveUsers { get; set; }
    }
}
