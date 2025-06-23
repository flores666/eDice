using FileService.Configuration;
using FileService.Helpers;
using FileService.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Infrastructure.FileService;
using Shared.Logging;
using Shared.MessageBus.Kafka.Producer;
using Shared.Models;
using App = Shared.Configuration.App;
using GoogleDriveFile = Google.Apis.Drive.v3.Data.File;
using DbaseFile = Infrastructure.FileService.Models.File;

namespace FileService.Service;

public class GoogleDriveFilesManager : IFilesManager
{
    private readonly IAppLogger<GoogleDriveFilesManager> _logger;
    private readonly PostgresContext _context;
    private readonly IUserContext _userContext;
    private readonly IMessagesProducer<FileUploadedMessage> _messagesProducer;
    private static readonly string[] Scopes = {DriveService.Scope.Drive};
    private readonly DriveService _service;

    public GoogleDriveFilesManager(IAppLogger<GoogleDriveFilesManager> logger, PostgresContext context, IUserContext userContext,
        IMessagesProducer<FileUploadedMessage> messagesProducer)
    {
        _logger = logger;
        _context = context;
        _userContext = userContext;
        _messagesProducer = messagesProducer;

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

    public async Task<OperationResult> UploadAsync(IFormFile? file, UploadOptions? options)
    {
        var result = new OperationResult();
        if (!await FileValidator.IsValidFileAsync(file))
        {
            result.Message = "Похоже что с файлом что-то не так. Еще раз все проверьте";
            return result;
        }

        await using var stream = file.OpenReadStream();
        var streamToUpload = stream;
        
        if (options != null) streamToUpload = await ImageProcessor.ResizeImageAsync(stream, file.ContentType, options);
        
        try
        {
            var mediaUploadRequest = _service.Files.Create(new GoogleDriveFile
            {
                Name = file.FileName,
            }, streamToUpload, file.ContentType);

            mediaUploadRequest.Fields = "id";
            
            var status = await mediaUploadRequest.UploadAsync();
            result.IsSuccess = status != null && status.Status == UploadStatus.Completed && !string.IsNullOrEmpty(mediaUploadRequest.ResponseBody?.Id);
            
            if (result.IsSuccess)
            {
                await _service.Permissions.Create(new Permission
                {
                    Type = "anyone",
                    Role = "reader"
                }, mediaUploadRequest.ResponseBody!.Id).ExecuteAsync();
                
                var dbFileModel = new DbaseFile
                {
                    Name = file.Name,
                    Extension = Path.GetExtension(file.FileName),
                    CreatedAt = DateTime.UtcNow,
                    Size = file.Length,
                    Id = mediaUploadRequest.ResponseBody.Id,
                    Link = $"https://drive.google.com/uc?id={mediaUploadRequest.ResponseBody.Id}",
                    CreatedBy = _userContext.Id
                };
                
                result.Data = dbFileModel;
                await _context.Files.AddAsync(dbFileModel);
                await _context.SaveChangesAsync();
                await _messagesProducer.PublishAsync(MessagesTopics.FileUploaded, FileUploadMessageConverter.FromDbModel(dbFileModel));
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.ToString());
        }

        return result;
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var result = new OperationResult();

        try
        {
            var file = await _context.Files.FindAsync(id);
            if (file != null)
            {
                if (file.CreatedBy != _userContext.Id)
                {
                    result.Message = "Файл вам не принадлежит, удаление невозможно";
                    return result;
                }
                
                await _service.Files.Delete(id).ExecuteAsync();
                result.IsSuccess = true;
             
                _context.Files.Remove(file); 
                await _context.SaveChangesAsync();
            }
            else
            {
                result.Message = "Файл не найден";
                return result;
            }
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