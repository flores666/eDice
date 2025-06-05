using System.ComponentModel.DataAnnotations;
using MailService.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Logging;

namespace MailService;

[ApiController]
[Route("api/[controller]")]
public class SmtpController(IAppLogger<SmtpController> logger, ISmtpService smtpService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IResult> Send(
        [FromBody] EmailRequest request)    
    {
        var validationContext = new ValidationContext(request);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        if (!isValid)
        {
            logger.LogWarning("Validation failed.");
            return Results.BadRequest("Поля To, Subject и Body обязательны и должны быть корректными.");
        }

        try
        {
            
            await smtpService.SendEmailAsync(request.To, request.Subject, request.Body);
            
            logger.LogInformation("Send email success.");
            return Results.Ok(new { message = "Письмо успешно отправлено через Gmail SMTP" });
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, $"Error sending email: {ex.Message}");
            return Results.StatusCode(500);
        }
    }
}