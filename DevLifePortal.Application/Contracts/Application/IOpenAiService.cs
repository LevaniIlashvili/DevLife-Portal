namespace DevLifePortal.Application.Contracts.Application
{
    public interface IOpenAiService
    {
        Task<string> AskAsync(string userPrompt);
    }
}
