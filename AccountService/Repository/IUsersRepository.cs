using Infrastructure.AccountService.Models;

namespace AccountService.Repository;

public interface IUsersRepository
{
    public Task<User?> GetUserByLoginAsync(string login);
    public Task<User?> GetUserByIdAsync(Guid userId);
    public Task<bool> CreateUserAsync(User model);
    public Task<bool> UpdateUserAsync(User user);
    public Task<User?> GetUserByRestoreCodeAsync(string code);
}
