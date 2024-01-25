using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Sse;

public class StreamSSE(ILogger<StreamSSE> logger)
{
    private static readonly JsonSerializerOptions jsonOption = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };


    [Function(nameof(StreamSSE))]
    public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "stream")] HttpRequest req,
        FunctionContext functionContext, CancellationToken cancellationToken)
    {
        var httpContext = functionContext.GetHttpContext() ?? throw new InvalidOperationException("HttpContext is null");

        httpContext.Response.Headers.ContentType = "text/event-stream";

        foreach (var completion in Sample.Completions)
        {
            await WriteChunkResponseAsync(httpContext, completion, cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }

        logger.LogInformation("Stream SSE completed");
    }


    private static async Task WriteChunkResponseAsync(HttpContext http, ChatCompletions completions, CancellationToken cancellationToken)
    {
        await http.Response.WriteAsync("data: ", cancellationToken: cancellationToken);
        await JsonSerializer.SerializeAsync(http.Response.Body, completions, jsonOption, cancellationToken);
        await http.Response.WriteAsync("\n\n", cancellationToken: cancellationToken);
        await http.Response.Body.FlushAsync(cancellationToken);
    }
}
