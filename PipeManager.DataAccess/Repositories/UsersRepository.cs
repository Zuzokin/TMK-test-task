using Microsoft.EntityFrameworkCore;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly PipeManagerDbContext _context;

    public UsersRepository(PipeManagerDbContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            Email = user.Email
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Result<User>> GetByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<User>.Failure("Email cannot be empty.");
        }

        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity == null)
        {
            return Result<User>.Failure("User not found.");
        }

        var userResult = User.Create(
            userEntity.Id,
            userEntity.PasswordHash,
            userEntity.Email);

        if (!userResult.IsSuccess)
        {
            return Result<User>.Failure(userResult.Error);
        }

        return Result<User>.Success(userResult.Value);
    }
}