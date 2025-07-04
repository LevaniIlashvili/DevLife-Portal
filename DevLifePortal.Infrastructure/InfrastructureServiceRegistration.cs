using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using DevLifePortal.Infrastructure.Mongo;
using DevLifePortal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DevLifeDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection")));

            MongoClassMapping.RegisterMappings();

            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(configuration.GetConnectionString("MongoDB")));

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var db = client.GetDatabase("DevLife");
                return db.GetCollection<DevDatingProfile>("dating_profiles");
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var db = client.GetDatabase("DevLife");
                return db.GetCollection<DevDatingFakeProfile>("fake_profiles");
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var db = client.GetDatabase("DevLife");
                return db.GetCollection<DevDatingSwipeAction>("swipe_actions");
            });

            services.AddTransient<StaticDatingProfileSeeder>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICodeCasinoProfileRepository, CodeCasinoProfileRepository>();
            services.AddScoped<IBugChaseProfileRepository, BugChaseProfileRepository>();
            services.AddScoped<IDevDatingProfileRepository, DevDatingProfileRepository>();
            services.AddScoped<IDevDatingFakeProfileRepository, DevDatingFakeProfileRepository>();
            services.AddScoped<IDevDateSwipeRepository, DevDateSwipeRepository>();

            return services;
        }
    }
}
