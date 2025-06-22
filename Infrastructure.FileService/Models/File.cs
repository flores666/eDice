namespace Infrastructure.FileService.Models;

public partial class File
{
    public string Id { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public long Size { get; set; }

    public string Link { get; set; } = null!;

    public Guid CreatedBy { get; set; }
}
