using System.Text.Json;
using System.Text.Json.Serialization;

namespace Question.Moderation.Api;

public class OpenAiService : IOpenAiService
{
    private readonly HttpClient _httpClient;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new ()
    {
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public OpenAiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("authorization", "Bearer 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");
    }
    
    public async Task<ChatCompletionsResponse> CreateChatCompletion(ChatCompletionsRequest request, CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await this._httpClient.PostAsJsonAsync(
            "https://api.openai.com/v1/chat/completions", 
            request, 
            _jsonSerializerOptions, 
            cancellationToken: cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        var httpResponseMessageContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        var response = JsonSerializer.Deserialize<ChatCompletionsResponse>(httpResponseMessageContent, _jsonSerializerOptions);

        return response;
    }
}