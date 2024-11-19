using PipeManager.Core.Models;

namespace PipeManager.Application.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}