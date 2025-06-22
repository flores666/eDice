using System.Text.Json;
using FileService.Models;
using FileService.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace FileService;

public static class FileServiceApi
{
    public static IEndpointRouteBuilder MapFileServiceApi(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/upload", UploadFile)
            .DisableAntiforgery()
            .RequireAuthorization()
            .Accepts<IFormFile>("multipart/form-data");
        
        builder.MapDelete("/delete/{id}", DeleteFile)
            .RequireAuthorization();

        return builder;
    }

    private static async Task<IResult> UploadFile(
        [FromForm] IFormFile? file,
        [FromForm(Name = "options")] string? optionsJson,
        IFilesManager filesManager)
    {
        var result = new OperationResult();

        if (file == null)
        {
            result.Message = "Файл обязателен";
            return Results.BadRequest(result);
        }

        UploadOptions? options = null;
        if (!string.IsNullOrWhiteSpace(optionsJson))
        {
            try
            {
                options = JsonSerializer.Deserialize<UploadOptions>(optionsJson);
            }
            catch (JsonException)
            {
                result.Message = "Невалидный JSON в options";
                return Results.BadRequest(result);
            }
        }

        result = await filesManager.UploadAsync(file, options);
        return Results.Json(result);
    }

    private static async Task<IResult> DeleteFile(string id, IFilesManager filesManager)
    {
        if (string.IsNullOrEmpty(id)) return Results.BadRequest();

        var response = await filesManager.DeleteAsync(id);

        return Results.Json(response, statusCode: StatusCodes.Status200OK);
    }
}