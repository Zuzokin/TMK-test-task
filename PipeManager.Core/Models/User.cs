namespace PipeManager.Core.Models;

public class User
{
    private User(Guid id, string passwordHash, string email)
    {
        Id = id;
        PasswordHash = passwordHash;
        Email = email;
    }

    public Guid Id { get; private set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }

    public User() { }

    public static Result<User> Create(Guid id, string passwordHash, string email)
    {
        if (id == Guid.Empty)
        {
            return Result<User>.Failure("Id cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<User>.Failure("Email cannot be empty.");
        }

        if (email.Length > 254)
        {
            return Result<User>.Failure("Email length cannot exceed 254 characters.");
        }

        if (!IsValidEmail(email))
        {
            return Result<User>.Failure("Email is not in a valid format.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result<User>.Failure("Password hash cannot be empty.");
        }

        var user = new User(id, passwordHash, email);
        return Result<User>.Success(user);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}