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

    public async Task<User?> GetUserByLoginAsync(string login) => await _context.Users.FirstOrDefaultAsync(w => w.Email.ToLower() == login.ToLower());

    public async Task<bool> CreateUserAsync(User model)
    {
        _context.Users.Add(model);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        if (user.Id == Guid.Empty) return false;

        var trackedUser = _context.ChangeTracker.Entries<User>().FirstOrDefault(w => w.Entity.Id == user.Id);
        if (trackedUser != null)
        {
            trackedUser.CurrentValues.SetValues(user);
        }
        else
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
        }

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User?> GetUserByRestoreCodeAsync(string code) => await _context.Users
        .Where(w => !string.IsNullOrEmpty(w.ResetCode))
        .FirstOrDefaultAsync(w => w.ResetCode!.ToLower() == code.ToLower());
}