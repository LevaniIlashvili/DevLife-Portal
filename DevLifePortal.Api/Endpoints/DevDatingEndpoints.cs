using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DevLifePortal.Api.Endpoints
{
    public static class DevDatingEndpoints
    {
        public static void MapDevDatingEndpoints(this IEndpointRouteBuilder app)
        {
            var devDatingGroup = app.MapGroup("/devdating");

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
                DevDatingProfile profile,
                IDevDatingService devDatingService,
                HttpContext context) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                profile.UserId = int.Parse(userId);

                var addedProfile = await devDatingService.CreateProfileAsync(profile);

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
                [FromBody] DevDatingSwipeAction swipeAction) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                swipeAction.UserId = int.Parse(userId);
                await devDatingService.SwipeAsync(swipeAction);

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
                [FromQuery] Guid fakeProfileId,
                [FromBody] string userText) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var response = await devDatingService.ChatWithFakeProfileAi(int.Parse(userId), fakeProfileId, userText);

                return Results.Ok(response);
            });
        }
    }
}
