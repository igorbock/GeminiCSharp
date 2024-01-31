namespace GeminiCSharpTests.Tests;

public class RequestTest
{
    private HttpClient? _httpClient;
    private HttpResponseMessage? _httpResponseMessageTest;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;

    private GeminiChat? _geminiChatTests;

    private string _textoResposta = "O diâmetro da Terra é de 12.742 quilômetros. A circunferência da Terra é de 40.075 quilômetros. A superfície da Terra é de 510.100.000 quilômetros quadrados. O volume da Terra é de 1.083.210.000.000 quilômetros cúbicos.";

    private TypeTHelper<HttpResponseMessage, string>? _httpReponseHelper;

    [SetUp]
    public void Setup()
    {
        _httpResponseMessageTest = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessageTest);

        _geminiChatTests = new GeminiChat("chaveTeste");

        _httpReponseHelper = new HttpResponseHelper();
    }

    [Test]
    public async Task VerifyTextResponseTest()
    {
        _httpResponseMessageTest!.Content = new StringContent(JsonSerializer.Serialize(new GeminiProResponse(_textoResposta, "STOP", 0)));
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        var response = await _geminiChatTests!.SendMessageAsync("Qual o tamanho do planeta Terra?", _httpClient);

        Assert.IsNotNull(response);
        Assert.That(response, Is.EqualTo(_textoResposta));
        Assert.Pass();
    }

    [Test]
    public async Task VerifyKeyTest()
    {
        _httpResponseMessageTest!.Content = new StringContent(JsonSerializer.Serialize(new GeminiProResponse(_textoResposta, "STOP", 0)));
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        var responseKeyMock = await _geminiChatTests!.SendMessageAsync("Qual o tamanho do planeta Terra?", _httpClient);
        Assert.IsNotNull(responseKeyMock);

        try
        {
            var geminiKeylessMock = new GeminiChat(string.Empty);
            var responseKeylessMock = await geminiKeylessMock.SendMessageAsync("Qual o tamanho do planeta Terra?", _httpClient);

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter '_apiKey')"));
            Assert.Pass();
        }
    }

    [Test]
    public async Task EmptyMessageTest()
    {
        try
        {
            await _geminiChatTests!.SendMessageAsync(string.Empty, _httpClient);

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'message')"));
            Assert.Pass();
        }
    }

    [Test]
    public async Task HttpResponseTest()
    {
        var geminiProResponse = new GeminiProResponse("Resposta de teste.", "STOP", 0);
        var json = JsonSerializer.Serialize(geminiProResponse);

        _httpResponseMessageTest!.Content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        var responseHttpClient = await _httpClient!.PostAsync("https://teste.com.br", new StringContent("teste"));
        var response = await _httpReponseHelper!.GetResponseFromTypeTAsync(responseHttpClient);

        Assert.IsNotNull(response);
        Assert.That(response, Is.EqualTo("Resposta de teste."));
        Assert.Pass();
    }
}