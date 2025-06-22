using FileService.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace FileService.Helpers;

public static class ImageProcessor
{
    public static async Task<MemoryStream> ResizeImageAsync(Stream inputStream, string contentType, UploadOptions options)
    {
        if (!contentType.StartsWith("image")) throw new UnsupportedContentTypeException("Provided file is not an image");
    
        var image = await Image.LoadAsync(inputStream);
        var resolution = GetResolution(options.Width, options.Height, image.Size);
        image.Mutate(ctx => ctx.Resize(resolution.Item1, resolution.Item2));

        var outputStream = new MemoryStream();
        var encoder = image.Configuration.ImageFormatsManager.GetEncoder(image.Metadata.DecodedImageFormat);
        await image.SaveAsync(outputStream, encoder);

        outputStream.Position = 0;
        return outputStream;
    }

    private static (int, int) GetResolution(int? width, int? height, Size originalSize)
    {
        if (width == null && height == null) return (originalSize.Width, originalSize.Height);
        
        var aspectRatio = (double)originalSize.Width / originalSize.Height;

        width ??= (int) Math.Round(height!.Value * aspectRatio);
        height ??= (int) Math.Round(width.Value / aspectRatio);

        return (width.GetValueOrDefault(), height.GetValueOrDefault());
    }
}
