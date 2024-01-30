using System.Text.Json.Serialization;

namespace GeminiCSharp.Models
{
    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
