using Microsoft.AspNetCore.Mvc;
using Question.Moderation.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IOpenAiService, OpenAiService>();
builder.Services.AddSingleton<IQuestionModerationService, OpenAiQuestionModerationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/moderation/{question}/{answer}", 
    async (string question, string answer, [FromServices] IQuestionModerationService questionModerationService, CancellationToken cancellationToken) =>
    {
        var moderationResult = await questionModerationService.ModerateAsync(question, answer, cancellationToken);

        return Results.Ok(moderationResult);
    })
    .WithName("ModerateQuestion")
    .WithOpenApi();

app.Run();