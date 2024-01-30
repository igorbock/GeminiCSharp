using GeminiCSharp.Helpers;
using GeminiCSharp.Interfaces;
using Moq.Protected;
using System.Text;
using System.Text.Json;

namespace GeminiCSharpTests;

public class RequestTest
{
    private Mock<GeminiAbstract>? _geminiTestsMock;
    private Mock<GeminiAbstract>? _geminiTestsKeylessMock;
    private GeminiPro? _geminiProRequest;

    private HttpClient _httpClient;
    private HttpResponseMessage? _httpResponseMessageTest;

    private GeminiTests? _geminiTestsKeyless;
    private GeminiTests? _geminiTests;

    private string _textoResposta = "O diâmetro da Terra é de 12.742 quilômetros. A circunferência da Terra é de 40.075 quilômetros. A superfície da Terra é de 510.100.000 quilômetros quadrados. O volume da Terra é de 1.083.210.000.000 quilômetros cúbicos.";

    private TypeTHelper<HttpResponseMessage, string>? _httpReponseHelper;

    [SetUp]
    public void Setup()
    {
        _httpResponseMessageTest = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessageTest);
        _httpClient = new HttpClient(handler.Object);

        _geminiProRequest = new("Qual o tamanho do planeta Terra?", "user");

        _geminiTestsMock = new Mock<GeminiAbstract>("chaveTeste");
        _geminiTestsMock!.Setup(a => a.ChatAsync(_geminiProRequest, _httpClient)).Returns(Task.FromResult(_textoResposta));

        _geminiTestsKeylessMock = new Mock<GeminiAbstract>(string.Empty);
        _geminiTestsKeylessMock!.Setup(a => a.ChatAsync(_geminiProRequest, _httpClient)).Throws(new ArgumentNullException("_apiKey"));

        _geminiTestsKeyless = new GeminiTests(string.Empty);
        _geminiTests = new GeminiTests("chaveTeste");

        _httpReponseHelper = new HttpResponseHelper();
    }

    [Test]
    public async Task VerifyTextResponseTestMock()
    {
        var responseMock = await _geminiTestsMock!.Object.ChatAsync(_geminiProRequest, _httpClient);

        Assert.IsNotNull(responseMock);
        Assert.That(responseMock, Is.EqualTo(_textoResposta));
        Assert.Pass();
    }

    [Test]
    public async Task VerifyKeyTestMock()
    {
        var responseKeyMock = await _geminiTestsMock!.Object.ChatAsync(_geminiProRequest, _httpClient);
        Assert.IsNotNull(responseKeyMock);

        try
        {
            var geminiKeylessMock = _geminiTestsKeylessMock!.Object;
            var responseKeylessMock = await geminiKeylessMock.ChatAsync(_geminiProRequest, _httpClient);

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter '_apiKey')"));
            Assert.Pass();
        }
    }

    [Test]
    public async Task VerifyKeyTest()
    {
        try
        {
            var responseKeyless = await _geminiTestsKeyless!.ChatAsync(new GeminiPro(), _httpClient);

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter '_apiKey')"));
            Assert.Pass();
        }
    }

    [Test]
    public async Task EmptyContentTest()
    {
        try
        {
            var request = new GeminiPro();
            await _geminiTests!.ChatAsync(request, _httpClient);

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'contents')"));
            Assert.Pass();
        }
    }

    [Test]
    public async Task HttpResponseTest()
    {
        var geminiProResponse = new GeminiProResponse("Resposta de teste.", "STOP", 0);
        var json = JsonSerializer.Serialize(geminiProResponse);

        _httpResponseMessageTest!.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var responseHttpClient = await _httpClient.PostAsync("https://teste.com.br", new StringContent("teste"));
        var response = await _httpReponseHelper!.GetResponseFromTypeTAsync(responseHttpClient);

        Assert.IsNotNull(response);
        Assert.That(response, Is.EqualTo("Resposta de teste."));
        Assert.Pass();
    }
}