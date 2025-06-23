using System.Text.Json.Serialization;

namespace FileService.Models;

public class UploadOptions
{
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }
}