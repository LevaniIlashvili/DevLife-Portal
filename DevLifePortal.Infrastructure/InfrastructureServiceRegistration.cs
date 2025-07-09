using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Application.Services;
using DevLifePortal.Domain.Entities;
using DevLifePortal.Infrastructure.Mongo;
using DevLifePortal.Infrastructure.Repositories;
using DevLifePortal.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using static DevLifePortal.Infrastructure.Mongo.Seeders;

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
                var connectionString = configuration.GetConnectionString("MongoDB");
                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;

                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var db = client.GetDatabase(databaseName);
                return db.GetCollection<DevDatingFakeProfile>("fake_profiles");
            });

            services.AddSingleton(serviceProvider =>
            {
                var connectionString = configuration.GetConnectionString("MongoDB");
                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;

                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var db = client.GetDatabase(databaseName);
                return db.GetCollection<DevDatingSwipeAction>("swipe_actions");
            });

            services.AddSingleton(serviceProvider =>
            {
                var connectionString = configuration.GetConnectionString("MongoDB");
                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;

                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var db = client.GetDatabase("DevLife");
                return db.GetCollection<CodeCasinoChallenge>("code_casino_challenges");
            });

            services.AddTransient<DevDatingFakeProfileSeeder>();
            services.AddTransient<CodeCasinoChallengeSeeder>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICodeCasinoProfileRepository, CodeCasinoProfileRepository>();
            services.AddScoped<IBugChaseProfileRepository, BugChaseProfileRepository>();
            services.AddScoped<IDevDatingProfileRepository, DevDatingProfileRepository>();
            services.AddScoped<IDevDatingFakeProfileRepository, DevDatingFakeProfileRepository>();
            services.AddScoped<IDevDateSwipeRepository, DevDateSwipeRepository>();
            services.AddScoped<ICodeCasinoChallengeRepository, CodeCasinoChallengeRepository>();

            services.AddHttpClient<IGithubService, GithubService>();

            services.AddSingleton<IOpenAiService, OpenAiService>();

            return services;
        }
    }
}
