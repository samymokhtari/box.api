using AspNetCoreRateLimit;
using box.api.Presenters;
using box.application;
using box.infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Rate limit
ConfigureRateLimit(builder.Services, builder.Configuration);

// Implement differents layers of Dependency Injections
ConfigurePresenters(builder.Services);
builder.Services.InfrastructurePersistence(builder.Configuration, typeof(Program));  ;
builder.Services.ApplicationPersistance(builder.Configuration);

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/// <summary>
/// Configure Rate Limit
/// </summary>
void ConfigureRateLimit(IServiceCollection services, IConfiguration configuration)
{
    // needed to store rate limit counters and ip rules
    services.AddMemoryCache();

    //load general configuration from appsettings.json
    services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

    //load ip rules from appsettings.json
    services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

    // inject counter and rules stores
    services.AddInMemoryRateLimiting();

    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
}

/// <summary>
/// Configure Presenters
/// </summary>
void ConfigurePresenters(IServiceCollection services)
{
    services.AddScoped<StoragePresenter, StoragePresenter>();
    services.AddScoped<ProjectPresenter, ProjectPresenter>();
    services.AddScoped<EmptyPresenter, EmptyPresenter>();
}