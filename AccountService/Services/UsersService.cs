using AccountService.Helpers;
using AccountService.Models;
using Infrastructure.AccountService;

namespace AccountService.Services;

public class UsersService : IUsersService
{
    private readonly PostgresContext _context;

    public UsersService(PostgresContext context)
    {
        _context = context;
    }
    
    public async Task<UserDto?> GetUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;
        
        return UsersMapper.MapToDto(user);
    }
}
