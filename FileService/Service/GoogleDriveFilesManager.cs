using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Shared.Logging;
using Shared.Models;
using App = Shared.Configuration.App;
using File = Google.Apis.Drive.v3.Data.File;

namespace FileService.Service;

public class GoogleDriveFilesManager : IFilesManager
{
    private readonly IAppLogger<GoogleDriveFilesManager> _logger;
    private static readonly string[] Scopes = {DriveService.Scope.Drive};
    private readonly DriveService _service;

    public GoogleDriveFilesManager(IAppLogger<GoogleDriveFilesManager> logger)
    {
        _logger = logger;
        
        var credential = GoogleCredential.FromJsonParameters(new JsonCredentialParameters
        {
            PrivateKeyId = Environment.GetEnvironmentVariable("GOOGLE_DRIVE__KEY_ID"),
            PrivateKey = Environment.GetEnvironmentVariable("GOOGLE_DRIVE__KEY")?.Replace("\\n", "\n"),
            ProjectId = Environment.GetEnvironmentVariable("GOOGLE_DRIVE__PROJECT_ID"),
            ClientId = Environment.GetEnvironmentVariable("GOOGLE_DRIVE__CLIENT_ID"),
            ClientEmail = Environment.GetEnvironmentVariable("GOOGLE_DRIVE__CLIENT_EMAIL"),
            Type = "service_account"
        }).CreateScoped(Scopes);

        _service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = App.Name
        });
    }

    public async Task<OperationResult> UploadAsync(IFormFile file)
    {
        var result = new OperationResult();

        await using var stream = file.OpenReadStream();
        
        try
        {
            var request = _service.Files.Create(new File
            {
                Name = file.FileName,
            }, stream, file.ContentType);

            request.Fields = "id";
            
            var status = await request.UploadAsync();
            result.IsSuccess = status != null && status.Status == UploadStatus.Completed && !string.IsNullOrEmpty(request.ResponseBody?.Id);
            
            if (result.IsSuccess)
            {
                await _service.Permissions.Create(new Permission
                {
                    Type = "anyone",
                    Role = "reader"
                }, request.ResponseBody!.Id).ExecuteAsync();
                
                result.Data = new ResponseFileModel
                {
                    Id = request.ResponseBody.Id,
                    Name = file.Name,
                    Extension = Path.GetExtension(file.FileName),
                    Link = $"https://drive.google.com/uc?id={request.ResponseBody.Id}",
                    Size = file.Length
                };
            }
        }
        catch (Exception e)
        {
            stream.Close();
            _logger.LogWarning(e.ToString());
        }

        return result;
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var result = new OperationResult();

        try
        {
            await _service.Files.Delete(id).ExecuteAsync();
            result.IsSuccess = true;
        }
        catch (Google.GoogleApiException e)
        {
            result.IsSuccess = false;
            _logger.LogWarning("Google API error -> {E}", e);
        }
        catch (Exception e)
        {
            result.IsSuccess = false;
            _logger.LogWarning("General error -> {E}", e);
        }

        return result;
    }
}