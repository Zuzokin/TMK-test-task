namespace PipeManager.Core.Contracts.Requests;

public record AddPipesRequest
{
    public List<Guid> PipeIds { get; set; }
}