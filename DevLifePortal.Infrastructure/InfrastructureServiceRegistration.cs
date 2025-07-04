using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevLifePortal.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DevLifeDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICodeCasinoProfileRepository, CodeCasinoProfileRepository>();
            services.AddScoped<IBugChaseProfileRepository, BugChaseProfileRepository>();

            return services;
        }
    }
}
