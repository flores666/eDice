namespace FileService.Models;

public class FileUploadedMessage
{
    public string Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; }

    public string Extension { get; set; }

    public long Size { get; set; }

    public string Link { get; set; }

    public Guid CreatedBy { get; set; }
}