using AuthorizationService.DataTransferObjects;
using Infrastructure.AuthorizationService.Models;
using Shared.Models;

namespace AuthorizationService.Repository;

public interface IUsersRepository
{
    public Task<UserDto?> GetUserAsync(string login);
    public Task<OperationResult> CreateUserAsync(User model);
}
