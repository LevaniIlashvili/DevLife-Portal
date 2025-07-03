namespace DevLifePortal.Application.Contracts.Application
{
    public interface ICodeRoastService
    {
        Task<string> GetProblem(string difficulty);
        Task<string> RoastCode(string problem, string solution);
    }
}
