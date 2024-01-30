using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiCSharp.Models.Response
{
    public class GeminiProResponse
    {
        public GeminiProResponse() { }

        public GeminiProResponse(string text, string reason, int index)
        {
            Candidates = new List<Candidate>
            {
                new Candidate
                {
                    FinishReason = reason,
                    Index = index,
                    Content = new Content
                    {
                        Parts = new List<object>
                        {
                            new Part { Text = text }
                        },
                        Role = "model"
                    }
                }
            };
        }

        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }

        [JsonPropertyName("promptFeedback")]
        public PromptFeedback PromptFeedback { get; set; }
    }
}
