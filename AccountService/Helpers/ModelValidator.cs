using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.Helpers;

public static class ModelValidator
{
    public static bool TryValidateObject([NotNullWhen(true)] object model, out List<string> errorMessages)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        var isValid = Validator.TryValidateObject(model, context, results, true);
        errorMessages = !isValid ? results.Select(s => s.ErrorMessage!).ToList() : new List<string>();
        
        return isValid;
    }
}