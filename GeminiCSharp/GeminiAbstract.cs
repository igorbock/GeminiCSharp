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
    public abstract class GeminiAbstract
    {
        private readonly string _apiKey;
        private readonly TypeTHelper<HttpResponseMessage, string> _httpResponse;

        public GeminiAbstract(string apiKey)
        {
            _apiKey = apiKey;
            _httpResponse = new HttpResponseHelper();
        }

        public virtual async Task<string> ChatAsync(GeminiPro requestEntity, HttpClient httpClient)
        {
            var isKeyless = string.IsNullOrEmpty(_apiKey);
            if (isKeyless)
                throw new ArgumentNullException(nameof(_apiKey));

            using (httpClient)
            {
                httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com");

                var contents = requestEntity.Contents?.LastOrDefault();
                if (contents == null)
                    throw new ArgumentNullException(nameof(contents));

                var part = contents.Parts.Count > 1 ? contents.Parts?[1] : null;
                var isGeminiProVision = part != null;
                var url = isGeminiProVision ? $"v1beta/models/gemini-pro-vision:generateContent?key={_apiKey}" : $"v1beta/models/gemini-pro:generateContent?key={_apiKey}";
                if (isGeminiProVision)
                    requestEntity.Contents[0].Role = null;

                var json = JsonSerializer.Serialize(requestEntity);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                return await _httpResponse.GetResponseFromTypeTAsync(response);
            }
        }

        public virtual Content CreateContent(string text)
        {
            return new Content
            {
                Role = "user",
                Parts = new List<object>
                {
                    new Part{ Text = text }
                }
            };
        }
    }
}
