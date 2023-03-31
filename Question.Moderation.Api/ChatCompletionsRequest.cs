namespace Question.Moderation.Api;

public record ChatCompletionsRequest
{
    public required string Model { get; init; } = string.Empty;

    public required List<Message> Messages { get; init; } = new List<Message>();
    
    public record Message
    {
        public MessageRole Role { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}