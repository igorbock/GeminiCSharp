using GeminiCSharp.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GeminiCSharp.Interfaces;
using GeminiCSharp.Helpers;
using System.Collections.Generic;

namespace GeminiCSharp
{
    public class GeminiChat
    {
        private readonly string _apiKey;
        private readonly TypeTHelper<HttpResponseMessage, string> _httpResponse;

        private readonly List<Content> _contents;

        public GeminiChat(string apiKey)
        {
            _apiKey = apiKey;
            _httpResponse = new HttpResponseHelper();
            _contents = new List<Content>();
        }

        public async Task<string> SendMessageAsync(string message, HttpClient httpClient)
        {
            var isKeyless = string.IsNullOrEmpty(_apiKey);
            if (isKeyless)
                throw new ArgumentNullException(nameof(_apiKey));

            var emptyMessage = string.IsNullOrEmpty(message);
            if (emptyMessage)
                throw new ArgumentNullException(nameof(message));

            var newContent = CreateContent(message, "user");
            _contents.Add(newContent);
            var request = new GeminiPro(_contents);

            using (httpClient)
            {
                httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com");

                var contents = request.Contents?.LastOrDefault();
                var part = contents.Parts.Count > 1 ? contents.Parts?[1] : null;
                var isGeminiProVision = part != null;
                var url = isGeminiProVision ? $"v1beta/models/gemini-pro-vision:generateContent?key={_apiKey}" : $"v1beta/models/gemini-pro:generateContent?key={_apiKey}";
                if (isGeminiProVision)
                    request.Contents[0].Role = null;

                var json = JsonSerializer.Serialize(message);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseTypeT = await _httpResponse.GetResponseFromTypeTAsync(response);
                _contents.Add(CreateContent(responseTypeT, "model"));
                return responseTypeT;
            }
        }

        public void ResetToNewChat()
        {
            _contents.Clear();
        }

        public IList<Content> PreviousContents() => _contents;

        public Content CreateContent(string text, string role)
        {
            return new Content
            {
                Role = role,
                Parts = new List<object>
                {
                    new Part{ Text = text }
                }
            };
        }
    }
}
