using DevLifePortal.Api.Endpoints;
using DevLifePortal.Api.Hubs;
using DevLifePortal.Api.Middlewares;
using DevLifePortal.Application;
using DevLifePortal.Domain.Entities;
using DevLifePortal.Infrastructure;
using DevLifePortal.Infrastructure.Mongo;
using MongoDB.Driver;
using static DevLifePortal.Infrastructure.Mongo.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddAuthentication("DevLifeCookieAuth")
    .AddCookie("DevLifeCookieAuth", options =>
    {
        options.Cookie.Name = "DevLifeAuth";
    });

builder.Services.AddAuthorization();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var datingSeeder = scope.ServiceProvider.GetRequiredService<DevDatingFakeProfileSeeder>();
    await datingSeeder.SeedAsync();

    var codeCasinoSeeder = scope.ServiceProvider.GetRequiredService<CodeCasinoChallengeSeeder>();
    await codeCasinoSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DevLifePortal API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCustomExceptionHandler();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapAuthEndpoints();
app.MapDashboardEndpoints();
app.MapCodeCasinoEndpoints();
app.MapCodeRoastEndpoints();
app.MapExcuseEndpoints();
app.MapDevDatingEndpoints();
app.MapGithubAnalyzerEndpoints();

app.MapHub<BugChaseHub>("/bugchasehub");

app.Run();
