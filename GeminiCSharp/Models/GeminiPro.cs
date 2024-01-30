using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiCSharp.Models
{
    public class GeminiPro
    {
        public GeminiPro() { }

        public GeminiPro(List<Content> contents)
        {
            Contents = contents;
        }

        public GeminiPro(string text, string role)
        {
            Contents = new List<Content>
            {
                new Content
                {
                    Role = role,
                    Parts = new List<object>
                    {
                        new Part { Text = text }
                    }
                }
            };
        }

        [JsonPropertyName("contents")]
        public List<Content> Contents { get; set; }
    }
}
