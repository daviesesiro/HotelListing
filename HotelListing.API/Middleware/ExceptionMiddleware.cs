using System.Net;
using HotelListing.API.Exceptions;
using Newtonsoft.Json;

namespace HotelListing.API.Middleware;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while processing the request: {Path}", ctx.Request.Path);
            await HandleExceptionAsync(ctx, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext ctx, Exception ex)
    {
        ctx.Response.ContentType = "application/json";
        var statusCode = HttpStatusCode.InternalServerError;
        var errorDetails = new ErrorDetails
        {
            ErrorMessage = ex.Message,
            ErrorType = "Failure"
        };

        switch (ex)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorDetails.ErrorType = "NotFound";
                break;
        }

        var response = JsonConvert.SerializeObject(errorDetails);
        ctx.Response.StatusCode = (int)statusCode;

        await ctx.Response.WriteAsync(response);
    }


    public class ErrorDetails
    {
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}