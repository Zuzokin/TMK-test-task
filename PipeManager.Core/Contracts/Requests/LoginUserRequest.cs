using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests
{
    public record LoginUserRequest(
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        string Email,

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 64 characters.")]
        string Password);

}