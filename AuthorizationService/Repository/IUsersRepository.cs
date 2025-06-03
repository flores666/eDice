using Infrastructure.AuthorizationService.Models;

namespace AuthorizationService.Repository;

public interface IUsersRepository
{
    public Task<User?> GetUserAsync(string login);
    public Task<bool> CreateUserAsync(User model);
}
