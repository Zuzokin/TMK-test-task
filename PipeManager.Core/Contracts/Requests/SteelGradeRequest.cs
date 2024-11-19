using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests
{
    public record SteelGradeRequest(
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        string Name // Название марки стали
    );
}