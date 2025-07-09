using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevLifePortal.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var authGroup = app.MapGroup("/auth").WithTags("Auth");

            authGroup.MapPost("/register", async (IUserService userService, RegisterUserDTO user) =>
            {
                var createdUser = await userService.RegisterUser(user);
                return Results.Created((string?)null, createdUser);
            });

            authGroup.MapPost("/login", async (HttpContext context, IUserService userService, [FromBody] string username) =>
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
            });

            authGroup.MapPost("/logout", async (HttpContext context) =>
            {
                context.Session.Clear();
                await context.SignOutAsync("DevLifeCookieAuth");
                return Results.Ok();
            });

            authGroup.MapGet("/me", async (HttpContext context, IUserService userService) =>
            {
                var userId = context.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var user = await userService.GetUserByIdAsync(int.Parse(userId));

                return Results.Ok(user);
            });

            return app;
        }
    }
}
