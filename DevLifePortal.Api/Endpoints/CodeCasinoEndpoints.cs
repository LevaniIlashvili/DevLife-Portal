using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DevLifePortal.Api.Endpoints
{
    public static class CodeCasinoEndpoints
    {
        public static void MapCodeCasinoEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var codeCasinoGroup = endpoints.MapGroup("/codecasino");

            codeCasinoGroup.MapGet("/profile", async (ICodeCasinoService codeCasinoService, HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var profile = await codeCasinoService.GetProfile(int.Parse(userId));

                return Results.Ok(profile);
            })
            .WithTags("Code Casino");

            codeCasinoGroup.MapGet("/challenge", async (ICodeCasinoService codeCasinoService, HttpContext context) =>
            {
                var username = context.Session.GetString("Username");

                if (string.IsNullOrEmpty(username))
                {
                    return Results.Unauthorized();
                }

                var codeSnippets = await codeCasinoService.GetSnippets(username);
                return Results.Ok(codeSnippets);
            })
            .WithTags("Code Casino");

            codeCasinoGroup.MapPost("/challenge", async (
                ICodeCasinoService codeCasinoService, 
                HttpContext context, 
                [FromBody] CodeCasinoAnswerChallengeDTO answerChallengeDTO) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                await codeCasinoService.AnswerChallenge(int.Parse(userId), answerChallengeDTO);

                return Results.Ok();
            })
            .WithTags("Code Casino");
        }
    }
}
