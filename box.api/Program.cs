using AspNetCoreRateLimit;
using box.api.Middleware;
using box.api.Presenters;
using box.application;
using box.infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Box",
        Description = "An ASP.NET Core Web API for managing files for multiple projects",
        Contact = new OpenApiContact
        {
            Name = "Samy Mokhtari",
            Url = new Uri("https://github.com/samymokhtari")
        }
    });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "x-api-key", //header with api key
        Type = SecuritySchemeType.ApiKey,
        Description = "The API Key to access the API",
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme, 
                    Id = "ApiKey" 
                },
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// Configure Rate limit
ConfigureRateLimit(builder.Services, builder.Configuration);

// Implement differents layers of Dependency Injections
ConfigurePresenters(builder.Services);
builder.Services.InfrastructurePersistence(builder.Configuration, typeof(Program), builder.Environment.IsDevelopment());
builder.Services.ApplicationPersistance(builder.Configuration);

// Logging
// Use Serilog as the logging provider
builder.Services.AddHttpClient();

/* CORS */
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Here bc no domain to setup https for now
    app.UseHttpsRedirection();
}
app.UsePathBase("/box");
app.UseSwagger();
app.UseSwaggerUI();
app.UseIpRateLimiting();

/* Middleware */
app.UseMiddleware<ApiKeyMiddleware>();
app.UseCors(MyAllowSpecificOrigins);
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
    services.AddScoped<StorageGetPresenter, StorageGetPresenter>();
    services.AddScoped<ProjectPresenter, ProjectPresenter>();
    services.AddScoped<EmptyPresenter, EmptyPresenter>();
}