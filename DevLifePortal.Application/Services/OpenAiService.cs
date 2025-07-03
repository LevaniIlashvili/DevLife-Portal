using DevLifePortal.Application.Contracts.Application;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace DevLifePortal.Application.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly ChatClient _chatClient;

        public OpenAiService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"] ??
                         throw new Exception("OpenAI API Key is missing");
            _chatClient = new ChatClient(model: "gpt-4o", apiKey: apiKey);
        }

        public async Task<string> AskAsync(string userPrompt)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(userPrompt);

            string? responseText = completion.Content.FirstOrDefault()?.Text;
            return responseText ?? "[No response from AI]";
        }
    }
}
