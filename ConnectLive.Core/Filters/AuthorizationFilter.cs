using System.Text;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectLive.Core.Api.Filters;

public class AuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var services = httpContext.RequestServices;

        var environment = services.GetRequiredService<IWebHostEnvironment>();
        if (environment.IsDevelopment())
        {
            return true;
        }

        var configuration = services.GetRequiredService<IConfiguration>();
        var expectedUser = configuration["HangfireDashboard:Username"];
        var expectedPassword = configuration["HangfireDashboard:Password"];

        if (string.IsNullOrEmpty(expectedUser) || string.IsNullOrEmpty(expectedPassword))
        {
            // No credentials configured outside Development: fail closed rather than expose the dashboard.
            return false;
        }

        var authHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire\"";
            return false;
        }

        try
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader["Basic ".Length..].Trim()));
            var parts = decoded.Split(':', 2);
            return parts.Length == 2 && parts[0] == expectedUser && parts[1] == expectedPassword;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
