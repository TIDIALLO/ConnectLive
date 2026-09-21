using ConnectLive.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ConnectLive.Application.Extensions;
public static class WebHostExtensions
{
    public static IApplicationBuilder UseCustomException(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }

    ////public static WebApplicationBuilder CreateWebHostBuilder<T>(string[] args)
    ////    where T : class
    ////{

    //}
}
