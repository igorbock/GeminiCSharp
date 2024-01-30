using System.Text.Json.Serialization;

namespace GeminiCSharp.Models.Response
{
    public class SafetyRating
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("probability")]
        public string Probability { get; set; }
    }
}
