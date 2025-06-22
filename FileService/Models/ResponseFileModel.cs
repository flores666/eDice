namespace FileService.Models;

public class ResponseFileModel
{
    public string Id { get; set; } = null!;
    public long Size { get; set; }
    public string Name { get; set; } = null!;
    public string Extension { get; set; } = null!;
    public string Link { get; set; } = null!;
}
