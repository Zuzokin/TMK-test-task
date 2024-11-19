namespace PipeManager.Core.Abstractions;

public interface IUsersService
{
    Task Register(string email, string password);

    Task<string> Login(string email, string password);
}