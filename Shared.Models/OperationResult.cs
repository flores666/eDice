﻿namespace Shared.Models;

public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
    public IEnumerable<string>? ErrorMessages { get; set; }
    public object? Data { get; set; }

    public static OperationResult Ok(string? message = null) => new() { IsSuccess = true, Data = message };
    public static OperationResult Fail(string error, string? message = null) => new() { IsSuccess = false, Data = message, ErrorMessage = error };
    public static OperationResult Fail(IEnumerable<string> errors, string? message = null) => new() { IsSuccess = false, Data = message, ErrorMessages = errors };
}
