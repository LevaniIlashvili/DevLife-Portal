using DevLifePortal.Application.Contracts.Application;

namespace DevLifePortal.Api.Endpoints
{
    public static class DashboardEndpoints
    {
        public static void MapDashboardEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/dashboard", async (HttpContext httpContext, IUserService userService, IDashboardService dashboardService) =>
            {
                var username = httpContext.User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Results.Unauthorized();
                }

                var dashboardData = dashboardService.GenerateDashboard(username);
                return Results.Ok(dashboardData);
            })
            .RequireAuthorization()
            .WithTags("Dashboard");
        }
    }
}
