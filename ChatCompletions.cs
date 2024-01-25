
namespace Sse;

// https://platform.openai.com/docs/guides/text-generation/chat-completions-response-format
public class ChatCompletions
{
    public List<ChatChoice> Choices { get; set; } = [];
    public long Created { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    public required string Id { get; set; }
    public CompletionUsage Usage { get; set; } = new();
}

public class ChatChoice
{
    public string? FinishReason { get; set; } = null;
    public required int Index { get; set; }
    public required ChatMessage Message { get; set; }
    public float? Logprobs { get; set; } = null;
}

public class ChatMessage
{
    public required string Content { get; set; } = string.Empty;
    public required string Role { get; set; }
}

public class CompletionUsage
{
    public int CompletionTokens { get; set; }
    public int PromptTokens { get; set; }
    public int TotalTokens { get; set; }
}

public static class Sample
{
    public static readonly List<ChatCompletions> Completions = [
        new() {
            Id = $"0",
            Choices = [ new() { Index = 0, Message = new() { Role = "assistant", Content = $"Hello there, I am the Developer Platform AI." } } ]
        },
        new() {
            Id = $"1",
            Choices = [ new() { Index = 1, Message = new() { Role = "assistant", Content = $"What can I help you with today?" } } ]
        },
        new() {
            Id = $"2",
            Choices = [ new() { Index = 2, Message = new() { Role = "assistant", Content = $"Thinking..." } } ]
        },
        new() {
            Id = $"3",
            Choices = [ new() { Index = 3, Message = new() { Role = "assistant", Content = $"Done" } } ]
        },
    ];
}
