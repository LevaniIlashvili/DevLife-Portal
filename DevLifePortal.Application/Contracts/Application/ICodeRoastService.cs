using DevLifePortal.Application.DTOs;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface ICodeRoastService
    {
        Task<string> GetProblem(string difficulty);
        Task<string> RoastCode(CodeRoastSolutionDTO solutionDTO);
    }
}
