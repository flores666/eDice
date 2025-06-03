using Infrastructure.AuthorizationService;
using Infrastructure.AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Repository;

public class PostgresUsersRepository : IUsersRepository
{
    private readonly PostgresContext _context;

    public PostgresUsersRepository(PostgresContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(w => w.Email.ToLower() == login.ToLower());
    }

    public async Task<bool> CreateUserAsync(User model)
    {
        _context.Users.Add(model);
        return await _context.SaveChangesAsync() > 0;
    }
}
