namespace Shared.Models;

public class OperationResult
{
    private string? _message;
    public bool IsSuccess { get; set; }
    
    public string Message
    {
        get => _message ?? (IsSuccess ? "Все прошло успешно" : "Что-то пошло не так"); 
        set => _message = value; 
    } 
    
    public object? Data { get; set; }

    public static OperationResult Ok(string? message = null) => new() { IsSuccess = true, Message = message };
    public static OperationResult Fail(string error) => new() { IsSuccess = false, Message = error };
}
