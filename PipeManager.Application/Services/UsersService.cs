using PipeManager.Application.Auth;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUsersRepository _usersReposotory;

    public UsersService(IUsersRepository usersRepository ,IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _usersReposotory = usersRepository;
    }
    
    public async Task Register(string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(new Guid(), hashedPassword, email);

        await _usersReposotory.Add(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _usersReposotory.GetByEmail(email);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (result == false)
        {
            throw new Exception("Failed to login");
        }

        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}