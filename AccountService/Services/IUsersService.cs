using AccountService.Models;

namespace AccountService.Services;

public interface IUsersService
{
    public Task<UserDto?> GetUserAsync(Guid id);
}