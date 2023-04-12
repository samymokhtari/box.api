using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace box.api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verify presence of the API Key
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeySectionName, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            // Get env API KEY configured
            string apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName) ?? Environment.GetEnvironmentVariable(AuthConstants.ApiKeySectionName) ?? string.Empty;

            if (string.IsNullOrEmpty(apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("API Key not configured");
                return;
            }

            // Compare
            if (!apiKey.Equals(extractedApiKey.ToString()))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            // Next
            await _next(context);
        }
    }
}
