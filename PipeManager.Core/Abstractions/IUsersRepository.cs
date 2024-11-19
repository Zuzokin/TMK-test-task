using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions;

public interface IUsersRepository
{
    Task Add(User user);
    Task<Result<User>> GetByEmail(string email);
}