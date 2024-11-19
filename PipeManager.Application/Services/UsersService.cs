using PipeManager.Application.Auth;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _usersRepository = usersRepository;
    }

    public async Task Register(string email, string password)
    {
        // Проверяем, существует ли пользователь с таким email
        var existingUserResult = await _usersRepository.GetByEmail(email);

        if (existingUserResult.IsSuccess)
        {
            throw new InvalidOperationException($"User with email '{email}' already exists.");
        }

        // Хешируем пароль
        var hashedPassword = _passwordHasher.Generate(password);

        // Создаём нового пользователя
        var userResult = User.Create(Guid.NewGuid(), hashedPassword, email);

        if (!userResult.IsSuccess)
        {
            throw new InvalidOperationException(userResult.Error);
        }

        // Добавляем пользователя в репозиторий
        await _usersRepository.Add(userResult.Value);
    }

    public async Task<string> Login(string email, string password)
    {
        // Ищем пользователя по email
        var userResult = await _usersRepository.GetByEmail(email);

        if (!userResult.IsSuccess)
        {
            throw new InvalidOperationException(userResult.Error);
        }

        var user = userResult.Value;

        // Проверяем пароль
        if (!_passwordHasher.Verify(password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        // Генерируем JWT токен
        return _jwtProvider.GenerateToken(user);
    }
}
