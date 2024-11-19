using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password
    );