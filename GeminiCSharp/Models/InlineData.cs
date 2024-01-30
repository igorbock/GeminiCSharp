using System.Text.Json.Serialization;

namespace GeminiCSharp.Models
{
    public class InlineData
    {
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}
