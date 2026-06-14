using BelajarApi.Models;

namespace BelajarApi.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User> CreateAsync(User user);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);

    Task<bool> UpdateAsync(User user);
}