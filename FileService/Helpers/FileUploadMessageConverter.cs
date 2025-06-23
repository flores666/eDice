using FileService.Models;
using File = Infrastructure.FileService.Models.File;

namespace FileService.Helpers;

public static class FileUploadMessageConverter
{
    public static FileUploadedMessage FromDbModel(File file)
    {
        return new FileUploadedMessage
        {
            Id = file.Id,
            Extension = file.Extension,
            CreatedAt = file.CreatedAt,
            Name = file.Name,
            Link = file.Link,
            Size = file.Size,
            CreatedBy = file.CreatedBy
        };
    }
}