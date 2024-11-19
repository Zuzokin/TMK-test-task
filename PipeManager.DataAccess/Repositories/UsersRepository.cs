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
        // _mapper = mapper;
    }


    public async Task Add(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            Email = user.Email
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("user not found");

        return User.Create(
            userEntity.Id,
            userEntity.PasswordHash,
            userEntity.Email);
    }
    
    
}