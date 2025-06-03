using AuthorizationService.DataTransferObjects;
using Infrastructure.AuthorizationService;
using Infrastructure.AuthorizationService.Models;
using Shared.Models;

namespace AuthorizationService.Repository;

public class PostgresUsersRepository : IUsersRepository
{
    private readonly PostgresContext _context;

    public PostgresUsersRepository(PostgresContext context)
    {
        _context = context;
    }
    
    public async Task<UserDto?> GetUserAsync(string login)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult> CreateUserAsync(User model)
    {
        throw new NotImplementedException();
    }
}