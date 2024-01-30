using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiCSharp.Models.Response
{
    public class PromptFeedback
    {
        [JsonPropertyName("safetyRatings")]
        public List<SafetyRating> SafetyRatings { get; set; }
    }
}
