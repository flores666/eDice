namespace AccountService.Models;

public class UserDto
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? BannedBefore { get; set; }
    
    public string? Description { get; set; }

    public string? ProfilePicture { get; set; }

    public string? ProfilePicturePreview { get; set; }
}
