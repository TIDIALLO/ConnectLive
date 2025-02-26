using ConnectLive.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ConnectLive.Application.Middlewares;
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("[ExceptionHandlerMiddleware][START]");

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }

        _logger.LogInformation("[ExceptionHandlerMiddleware][END]");
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogInformation("[ExceptionHandlerMiddleware][EXCEPTION] {exception}", exception);

        var code = HttpStatusCode.InternalServerError;
        string message = exception.InnerException?.Message ?? exception.Message;
        await SendExceptionToAdmin(exception);

        //switch (exception)
        //{
        //    case UnauthorizedAccessException _:
        //        code = HttpStatusCode.Unauthorized;
        //        break;

        //    case KeyNotFoundException _:
        //        code = HttpStatusCode.NotFound;
        //        break;

        //    case NullReferenceException _:
        //        code = HttpStatusCode.Conflict;
        //        message = "Aye aye aye, You need to pay more attention to your code !! :(";
        //        break;  
            
        //    case IbrahimaException _:
        //        code = HttpStatusCode.Created;
        //        break;

        //    case ArgumentNullException _:
        //    case ArgumentException _:
        //        code = HttpStatusCode.Ambiguous;
        //        break;
        //}

        context.Response.ContentType = context.Request?.ContentType ?? "";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(message);
    }

    private Task SendExceptionToAdmin(Exception exception)
    {
        throw new NotImplementedException();
    }
}
