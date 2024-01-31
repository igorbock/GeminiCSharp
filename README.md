# How to use GeminiCSharp Lib

- Add NuGet package in project:
```pwsh
dotnet add package GeminiCSharp --version 1.0.4
```
- Use the deployment example below:

```csharp
using System.Configuration;
using GeminiCSharp;

var menu = "Faça sua pergunta para o Bard: \n\n'[X]' - Sair\n'[L]' - Limpar / Nova conversa\n\n";
Console.WriteLine(menu);
var apiKey = ConfigurationManager.AppSettings["API_KEY"];
var geminiChat = new GeminiChat(apiKey);

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
```
---
# Como usar a biblioteca GeminiCSharp

- Adicionar o pacote NuGet no projeto:
```pwsh
dotnet add package GeminiCSharp --version 1.0.4
```
- Use o exemplo de implantação abaixo:

```csharp
using System.Configuration;
using GeminiCSharp;

var menu = "Faça sua pergunta para o Bard: \n\n'[X]' - Sair\n'[L]' - Limpar / Nova conversa\n\n";
Console.WriteLine(menu);
var apiKey = ConfigurationManager.AppSettings["API_KEY"];
var geminiChat = new GeminiChat(apiKey);

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
```
