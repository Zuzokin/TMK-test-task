using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests
{
    public record AddPipesRequest(
        [Required(ErrorMessage = "PipeIds are required.")]
        [MinLength(1, ErrorMessage = "At least one PipeId must be provided.")]
        List<Guid> PipeIds // Список идентификаторов труб
    );
}