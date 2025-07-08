using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
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
            })
            .WithTags("Dev Dating");

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
            })
            .WithTags("Dev Dating");

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
            })
            .WithTags("Dev Dating");

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
            })
            .WithTags("Dev Dating");

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
            })
            .WithTags("Dev Dating");

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
            })
            .WithTags("Dev Dating");
        }
    }
}
