using FileService.Models;
using Shared.Models;

namespace FileService.Service;

public interface IFilesManager
{
    public Task<OperationResult> UploadAsync(IFormFile? file, UploadOptions? options);
    public Task<OperationResult> DeleteAsync(string id);
}