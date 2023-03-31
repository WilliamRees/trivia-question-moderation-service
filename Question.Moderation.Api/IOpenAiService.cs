namespace Question.Moderation.Api;

public interface IOpenAiService
{
    Task<ChatCompletionsResponse> CreateChatCompletion(
        ChatCompletionsRequest request, 
        CancellationToken cancellationToken = default);
}