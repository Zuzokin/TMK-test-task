using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User> GetByEmail(string email);
}