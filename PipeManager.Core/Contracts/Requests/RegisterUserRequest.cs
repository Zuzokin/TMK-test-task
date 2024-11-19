using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests
{
    public record RegisterUserRequest(
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters.")]
        string Email,

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        string Password
    );
}