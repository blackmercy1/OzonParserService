using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace OzonParserService.Web.Handlers;

public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";

    private readonly static JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = { new StringEnumConverter() },
        NullValueHandling = NullValueHandling.Ignore
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message is not "" ? exception.Message : UnhandledExceptionMsg);

        var problemDetails = CreateProblemDetails(context, exception);
        var json = ToJson(problemDetails);

        const string contentType = "application/problem+json";
        context.Response.ContentType = contentType;
        await context.Response.WriteAsync(json, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
    {
        var errorCode = context.Response.StatusCode;
        var statusCode = context.Response.StatusCode;
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
        
        if (string.IsNullOrEmpty(reasonPhrase))
            reasonPhrase = UnhandledExceptionMsg;

        var problemDetails = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231",
            Status = statusCode,
            Title = reasonPhrase,
        };

        // Using dictionary for extensions in Newtonsoft
        problemDetails.Extensions[nameof(errorCode)] = errorCode;

        if (!env.IsDevelopment())
            return problemDetails;

        problemDetails.Detail = exception.ToString();
        problemDetails.Extensions["traceId"] = Activity.Current?.Id;
        problemDetails.Extensions["requestId"] = context.TraceIdentifier;
        problemDetails.Extensions["data"] = exception.Data;

        return problemDetails;
    }

    private string ToJson(in ProblemDetails problemDetails)
    {
        try
        {
            return JsonConvert.SerializeObject(problemDetails, SerializerSettings);
        }
        catch (Exception ex)
        {
            const string msg = "An exception has occurred while serializing error to JSON";
            logger.LogError(ex, msg);
        }

        return string.Empty;
    }
}