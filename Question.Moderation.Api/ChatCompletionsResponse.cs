namespace Question.Moderation.Api;

public record ChatCompletionsResponse
{
    public List<ChoiceResponse> Choices { get; set; }

    public record ChoiceResponse
    {
        public MessageResponse Message { get; set; }
    }

    public record MessageResponse
    {
        public MessageRole Role { get; set; }

        public string Content { get; set; } = string.Empty;

    }
}