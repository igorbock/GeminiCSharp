using GeminiCSharp.Interfaces;
using GeminiCSharp.Models;
using GeminiCSharp.Models.Response;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeminiCSharp.Helpers
{
    public class HttpResponseHelper : TypeTHelper<HttpResponseMessage, string>
    {
        public async Task<string> GetResponseFromTypeTAsync(HttpResponseMessage entity)
        {
            entity.EnsureSuccessStatusCode();

            var stringResponse = await entity.Content.ReadAsStringAsync();
            var geminiProResponse = JsonSerializer.Deserialize<GeminiProResponse>(stringResponse);

            var partString = geminiProResponse.Candidates[0].Content.Parts[0];
            var part = JsonSerializer.Deserialize<Part>(partString.ToString());
            return part.Text;
        }
    }
}
