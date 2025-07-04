using DevLifePortal.Api.Endpoints;
using DevLifePortal.Api.Hubs;
using DevLifePortal.Api.Middlewares;
using DevLifePortal.Application;
using DevLifePortal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSignalR();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddAuthentication("DevLifeCookieAuth")
    .AddCookie("DevLifeCookieAuth", options =>
    {
        options.Cookie.Name = "DevLifeAuth";
    });

builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCustomExceptionHandler();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapDashboardEndpoints();
app.MapCodeCasinoEndpoints();
app.MapCodeRoastEndpoints();

app.MapHub<BugChaseHub>("/bugchasehub");

app.Run();
