namespace GeminiCSharpTests.Tests;

public class ChatTest
{
    private GeminiChat _geminiTest;

    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient? _httpClient;
    private HttpResponseMessage _httpResponseMessageTest;

    [SetUp]
    public void Setup()
    {
        _geminiTest = new GeminiChat("chaveTeste");

        _httpResponseMessageTest = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessageTest);
    }

    [Test]
    public async Task MessagesTest()
    {
        var primeiraResposta = new GeminiProResponse("Internacional", "STOP", 0);
        _httpResponseMessageTest.Content = new StringContent(JsonSerializer.Serialize(primeiraResposta));

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        var firstResponse = await _geminiTest.SendMessageAsync("Qual o maior time de futebol do sul do Brasil?", _httpClient);
        Assert.IsNotNull(firstResponse);
        Assert.That(firstResponse, Is.EqualTo("Internacional"));

        var segundaResposta = new GeminiProResponse("Porque ele venceu o Barcelona na final do Mundial de Clubes em 2006", "STOP", 0);
        _httpResponseMessageTest.Content = new StringContent(JsonSerializer.Serialize(segundaResposta));

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        var secondResponse = await _geminiTest.SendMessageAsync("Porque o Internacional é o único clube no Sul do país com o título de campeão do mundo?", _httpClient);
        Assert.IsNotNull(secondResponse);
        Assert.That(secondResponse, Is.EqualTo("Porque ele venceu o Barcelona na final do Mundial de Clubes em 2006"));

        var countContents = _geminiTest.PreviousContents().Count;
        Assert.That(countContents, Is.EqualTo(4));

        _geminiTest.ResetToNewChat();
        countContents = _geminiTest.PreviousContents().Count;
        Assert.That(countContents, Is.EqualTo(0));

        Assert.Pass();
    }
}
