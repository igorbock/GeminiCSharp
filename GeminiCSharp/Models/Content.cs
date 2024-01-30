using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiCSharp.Models
{
    public class Content
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("parts")]
        public List<object> Parts { get; set; }
    }
}