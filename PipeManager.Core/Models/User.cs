namespace PipeManager.Core.Models;

public class User
{
    private User(Guid id, string passwordHash, string email)
    {
        Id = id;
        PasswordHash = passwordHash;
        Email = email;
    }

    public Guid Id { get; set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }

    public User(){}

    public static User Create(Guid id, string passwordHash, string email)
    {
        return new User(id, passwordHash, email);
    }

}