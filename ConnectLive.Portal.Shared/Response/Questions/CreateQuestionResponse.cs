namespace ConnectLive.Portal.Shared.Response.Questions;
public class CreateQuestionResponse
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Choice1 { get; set; } = string.Empty;
    public string Choice2 { get; set; } = string.Empty;
    public string Choice3 { get; set; } = string.Empty;
    public string Choice4 { get; set; } = string.Empty;
}
