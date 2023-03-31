namespace Question.Moderation.Api;

public interface IQuestionModerationService
{
    Task<ModerationResult> ModerateAsync(string question, string answer, CancellationToken cancellationToken = default);
}