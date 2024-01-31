using System.Configuration;
using GeminiCSharp;

var menu = "Faça sua pergunta para o Bard: \n\n'[X]' - Sair\n'[L]' - Limpar / Nova conversa\n\n";
Console.WriteLine(menu);
var nugetKey = ConfigurationManager.AppSettings["NUGET_KEY"];
var geminiChat = new GeminiChat(nugetKey);

var text = string.Empty;
while (text != "[X]")
{
    using var httpClient = new HttpClient();

    Console.Write("[Você]: ");
    text = Console.ReadLine();
    if (text == "[X]") break;
    if (text == "[L]")
    {
        geminiChat.ResetToNewChat();
        Console.Clear();
        Console.WriteLine(menu);
        continue;
    }

    var response = await geminiChat.SendMessageAsync(text, httpClient);
    Console.WriteLine($"[Bard]: {response}");
}