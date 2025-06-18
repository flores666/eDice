using Shared.Models;

namespace FileService.Service;

public interface IFilesManager
{
    public Task<OperationResult> UploadAsync(IFormFile file);
    public Task<OperationResult> DeleteAsync(string id);
}