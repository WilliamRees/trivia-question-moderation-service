namespace Question.Moderation.Api;

public class ModerationResult
{
    public bool IsApproved { get; set; }

    public string RejectionReason { get; set; } = string.Empty;
}