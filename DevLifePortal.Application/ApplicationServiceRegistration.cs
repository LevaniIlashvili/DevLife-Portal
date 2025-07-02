using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevLifePortal.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
