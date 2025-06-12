using Infrastructure.AuthorizationService.Models;

namespace AuthorizationService.Models.Tokens;

public class TokenModel
{
    public string AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
