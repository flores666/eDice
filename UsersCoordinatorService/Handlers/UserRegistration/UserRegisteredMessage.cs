namespace UsersCoordinatorService.Handlers.UserRegistration;

public class UserRegisteredMessage
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ResetCode { get; set; }
}