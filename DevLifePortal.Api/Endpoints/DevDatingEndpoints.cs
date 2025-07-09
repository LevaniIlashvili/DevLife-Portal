using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DevLifePortal.Api.Endpoints
{
    public static class DevDatingEndpoints
    {
        public static void MapDevDatingEndpoints(this IEndpointRouteBuilder app)
        {
            var devDatingGroup = app.MapGroup("/devdating").WithTags("Dev Dating");

            devDatingGroup.MapGet("/profile", async (
                IDevDatingService devDatingService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var profile = await devDatingService.GetProfileAsync(int.Parse(userId));

                return Results.Ok(profile);
            });

            devDatingGroup.MapPost("/profile", async (
                DevDatingAddProfileDTO profile,
                IDevDatingService devDatingService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var addedProfile = await devDatingService.CreateProfileAsync(profile, int.Parse(userId));

                return Results.Created($"/devdating/profile", addedProfile);
            });

            devDatingGroup.MapGet("/potentialmatch", async (
                IDevDatingService devDatingService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var match = await devDatingService.GetPotentialMatch(int.Parse(userId));

                return Results.Ok(match);
            });

            devDatingGroup.MapPost("/swipe", async (
                IDevDatingService devDatingService,
                HttpContext context,
                [FromBody] DevDatingSwipeActionDTO swipeActionDTO) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                await devDatingService.SwipeAsync(swipeActionDTO, int.Parse(userId));

                return Results.Ok();
            });

            devDatingGroup.MapGet("/matches", async (
                IDevDatingService devDatingService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var matches = await devDatingService.GetMatchesAsync(int.Parse(userId));

                return Results.Ok(matches);
            });

            devDatingGroup.MapPost("/textmatch", async (
                IDevDatingService devDatingService,
                HttpContext context,
                [FromBody] DevDatingTextMatchDTO textMatchDTO) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var response = await devDatingService.ChatWithFakeProfileAi(textMatchDTO, int.Parse(userId));

                return Results.Ok(response);
            });
        }
    }
}
