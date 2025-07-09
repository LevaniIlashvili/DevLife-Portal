using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Domain.Entities;
using DevLifePortal.Domain.Enums;

namespace DevLifePortal.Api.Endpoints
{
    public static class ExcuseEndpoints
    {
        public static void MapExcuseEndpoints(this IEndpointRouteBuilder app)
        {
            var excusesGroup = app.MapGroup("/excuses").WithTags("Excuse Generator");

            excusesGroup.MapGet("/generate", (
                IExcuseGeneratorService excuseGeneratorService,
                string category,
                string type) =>
            {
                if (!Enum.TryParse<ExcuseType>(type, true, out var parsedType))
                {
                    return Results.BadRequest(new { error = "Invalid excuse type" });
                }

                var excuse = excuseGeneratorService.Generate(category, parsedType);
                return Results.Ok(excuse);
            });

            excusesGroup.MapPost("/favorite", async (
                Excuse excuse,
                IExcuseGeneratorService excuseGeneratorService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                await excuseGeneratorService.SaveFavoriteAsync(userId, excuse);
                return Results.Ok();
            });

            excusesGroup.MapGet("/favorites", async (
                IExcuseGeneratorService excuseGeneratorService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var favorites = await excuseGeneratorService.GetFavoritesAsync(userId);
                return Results.Ok(favorites);
            });
        }
    }
}
