using System.ComponentModel.DataAnnotations;
using MailService.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Logging;
using Shared.Models;

namespace MailService;

[ApiController]
[Route("api/[controller]")]
public class SmtpController(IAppLogger<SmtpController> logger, ISmtpService smtpService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<ActionResult<OperationResult>> Send(
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
            var errors = validationResults
                .Select(error => error.ErrorMessage ?? "Неверное значение поля")
                .ToList();
            
            logger.LogWarning("Validation failed. Errors: {Errors}", errors);
            
            var result = OperationResult.Fail(errors, "Поля To, Subject и Body обязательны и должны быть корректными.");
            return BadRequest(result);
        }

        try
        {
            
            await smtpService.SendEmailAsync(request.To, request.Subject, request.Body);
            
            logger.LogInformation("Письмо успешно отправлено через Gmail SMTP");
            var result = OperationResult.Ok("Письмо успешно отправлено через Gmail SMTP");
            return Ok(result);
        }
        catch (Exception ex)
        {
            var result = OperationResult.Fail(new[] { ex.Message }, "Произошла внутренняя ошибка при отправке письма.");
            logger.LogCritical(ex, $"Error sending email: {ex.Message}");
            return StatusCode(500, result);
        }
    }
}