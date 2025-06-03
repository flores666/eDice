using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Helpers;

public static class ModelValidator
{
    public static bool TryValidateObject(object model, out List<string?> errorMessages)
    {
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true))
        {
            errorMessages = validationResults.Select(s => s.ErrorMessage).ToList();
        }
        else errorMessages = new List<string?>();

        return true;
    }
}
