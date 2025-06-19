namespace FileService;

public class FileValidator
{
    private static readonly HashSet<string> AllowedImageMimeTypes = new()
    {
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp",
        "image/bmp",
        "image/tiff"
    };
    
    private static readonly HashSet<string> AllowedTextMimeTypes = new()
    {
        "text/plain", "application/json", "application/xml",
        "text/csv", "text/xml",
    };

    public static async Task<bool> IsValidFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var contentType = file.ContentType.ToLowerInvariant();

        switch (contentType)
        {
            case var _ when AllowedImageMimeTypes.Contains(contentType):
                if (!AllowedImageMimeTypes.Contains(contentType))
                    return false;
                return await HasValidImageSignatureAsync(file);

            case var _ when AllowedTextMimeTypes.Contains(contentType):
                if (!AllowedTextMimeTypes.Contains(contentType))
                    return false;
                return await HasValidTextSignatureAsync(file);

            default:
                return false;
        }
    }

    /// <summary>
    /// Проверка реального типа изображений по известной сигнатуре
    /// </summary>
    private static async Task<bool> HasValidImageSignatureAsync(IFormFile file)
    {
        var buffer = new byte[12];

        await using (var stream = file.OpenReadStream())
        {
            await stream.ReadAsync(buffer, 0, buffer.Length);
        }

        // JPEG
        if (buffer.Take(3).SequenceEqual(new byte[] { 0xFF, 0xD8, 0xFF })) return true;

        // PNG
        if (buffer.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })) return true;

        // GIF
        if (buffer.Take(3).SequenceEqual(new byte[] { 0x47, 0x49, 0x46 })) return true;

        // BMP
        if (buffer.Take(2).SequenceEqual(new byte[] { 0x42, 0x4D })) return true;

        // TIFF — две возможные сигнатуры
        if (buffer.Take(4).SequenceEqual(new byte[] { 0x49, 0x49, 0x2A, 0x00 })) return true; // Little endian
        if (buffer.Take(4).SequenceEqual(new byte[] { 0x4D, 0x4D, 0x00, 0x2A })) return true; // Big endian

        // WEBP — начинается с RIFF....WEBP
        if (buffer.Length >= 12 &&
            buffer[0] == 0x52 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x46 && // "RIFF"
            buffer[8] == 0x57 && buffer[9] == 0x45 && buffer[10] == 0x42 && buffer[11] == 0x50)  // "WEBP"
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Проверка реального типа текстового файла по известной сигнатуре
    /// </summary>
    private static async Task<bool> HasValidTextSignatureAsync(IFormFile file)
    {
        // Прочитаем первые 512 байт (можно меньше)
        var buffer = new byte[512];

        await using var stream = file.OpenReadStream();
        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        if (bytesRead == 0) return false;

        // Проверка на наличие "непечатных" символов (бинарных)
        // Оставляем UTF-8 BOM (0xEF, 0xBB, 0xBF)
        for (var i = 0; i < bytesRead; i++)
        {
            var b = buffer[i];

            // ASCII: допускаем от 0x09 (tab), 0x0A (LF), 0x0D (CR), до 0x7E (~)
            if (b < 0x09 || (b > 0x0D && b < 0x20) || b > 0x7E)
            {
                // Допускаем UTF-8 BOM
                if (i < 3 && buffer.Length >= 3 &&
                    buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                {
                    continue;
                }

                return false; // подозрение на бинарный файл
            }
        }

        return true;
    }
}