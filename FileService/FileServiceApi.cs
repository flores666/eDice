using FileService.Service;

namespace FileService;

public static class FileServiceApi
{
    public static IEndpointRouteBuilder MapFileServiceApi(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/upload", UploadFile).DisableAntiforgery().RequireAuthorization();
        builder.MapDelete("/delete/{id}", DeleteFile).RequireAuthorization();
        
        return builder;
    }

    private static async Task<IResult> UploadFile(IFormFile? file, IFilesManager filesManager)
    {
        if (file == null) return Results.BadRequest();

        var response = await filesManager.UploadAsync(file);
        
        return Results.Json(response, statusCode: StatusCodes.Status200OK);
    }
    
    private static async Task<IResult> DeleteFile(string id, IFilesManager filesManager)
    {
        if (string.IsNullOrEmpty(id)) return Results.BadRequest();

        var response = await filesManager.DeleteAsync(id);
        
        return Results.Json(response, statusCode: StatusCodes.Status200OK);
    }
}