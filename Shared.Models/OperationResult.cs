namespace Shared.Models;

public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}
