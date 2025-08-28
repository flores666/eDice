using Infrastructure.AccountService.Models;

namespace AccountService.Models.Tokens;

public class TokenModel
{
    public string AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
