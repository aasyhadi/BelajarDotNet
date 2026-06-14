using BelajarApi.Data;
using BelajarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelajarApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _db.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return true;
    }
}