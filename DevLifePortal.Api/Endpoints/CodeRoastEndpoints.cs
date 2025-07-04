using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DevLifePortal.Api.Endpoints
{
    public static class CodeRoastEndpoints
    {
        public static void MapCodeRoastEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var codeRoastGroup = endpoints.MapGroup("/coderoast");

            codeRoastGroup.MapGet("/problem", async (ICodeRoastService codeRoastService, [FromQuery] string difficulty = "easy") =>
            {
                var problem = await codeRoastService.GetProblem(difficulty);
                return Results.Ok(problem);
            });

            codeRoastGroup.MapPost("/problem", async (ICodeRoastService codeRoastService, [FromBody] CodeRoastSolutionDTO solutionDTO) =>
            {
                var roast = await codeRoastService.RoastCode(solutionDTO.ProblemName, solutionDTO.Solution);
                return Results.Ok(roast);
            });
        }
    }
}
