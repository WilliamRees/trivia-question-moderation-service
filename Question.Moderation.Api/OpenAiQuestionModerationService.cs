using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Question.Moderation.Api;

public class OpenAiQuestionModerationService : IQuestionModerationService
{
    private readonly IOpenAiService _openAiService;

    public OpenAiQuestionModerationService(IOpenAiService openAiService)
    {
        this._openAiService = openAiService;
    }
    
    public async Task<ModerationResult> ModerateAsync(
        string question, 
        string answer, 
        CancellationToken cancellationToken = default)
    {
        var prompt = PreparePrompt(question, answer);

        var request = new ChatCompletionsRequest
        {
            Model = "gpt-4",
            Messages = new List<ChatCompletionsRequest.Message>
            {
                new ()
                {
                    Role = MessageRole.User,
                    Content = prompt
                }
            }
        };

        var response = await this._openAiService.CreateChatCompletion(request, cancellationToken);
        
        var result = JsonSerializer.Deserialize<ModerationResult>(
            response.Choices.First().Message.Content, 
            new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        if (result is null)
        {
            throw new Exception($"Unable to deserialize response to {nameof(ModerationResult)}");
        }
        
        return result;
    }

    private static string PreparePrompt(string question, string answer)
    {
        var promptStringBuilder = new StringBuilder(
            @"Respond as if you're an API that moderates trivia questions and is responsible for approving or rejecting the question. Since you're acting as an API you can only respond with valid JSON text. You should reject a question for any of the following reasons:
1. Spelling mistakes
2. Profanity
3. The answer is incorrect
4. Its a question about the novels or movies from the Harry Potter series

If you reject the question please provide a detailed explanation why in a professional and polite tone.

Your response must be valid JSON! The JSON schema is:
{
    ""isApproved"": true,
    ""rejectionReason"": """"
}

Please moderate this questions:

");

        promptStringBuilder.Append($"question: {question}");
        promptStringBuilder.Append(Environment.NewLine);
        promptStringBuilder.Append($"answer: {answer}");

        var prompt = promptStringBuilder.ToString();

        return prompt;
    }
}