namespace AuthorizationService.Models.Tokens;

public class TokenResultModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}