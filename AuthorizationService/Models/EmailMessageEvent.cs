namespace AuthorizationService.Models;

public class EmailMessageEvent
{
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}
