﻿using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevLifePortal.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/register", async (IUserService userService, User user) =>
            {
                var createdUser = await userService.RegisterUser(user);
                return Results.Created((string?)null, createdUser);
            })
            .WithTags("User");

            app.MapPost("/login", async (HttpContext context, IUserService userService, [FromBody] string username) =>
            {
                var user = await userService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return Results.Unauthorized();
                }

                context.Session.SetString("UserId", user.Id.ToString());
                context.Session.SetString("Username", user.Username);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var identity = new ClaimsIdentity(claims, "DevLifeCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await context.SignInAsync("DevLifeCookieAuth", principal);

                return Results.Ok();
            })
            .WithTags("User"); ;

            app.MapPost("/logout", async (HttpContext context) =>
            {
                context.Session.Clear();
                await context.SignOutAsync("DevLifeCookieAuth");
                return Results.Ok();
            })
            .WithTags("User"); ;

            app.MapGet("/me", async (HttpContext context, IUserService userService) =>
            {
                var username = context.Session.GetString("Username");
                var user = await userService.GetUserByUsernameAsync(username!);

                return Results.Ok(user);
            })
            .RequireAuthorization()
            .WithTags("User"); ;

            return app;
        }
    }
}
