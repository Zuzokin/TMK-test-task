using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions;

public interface IPipesService
{
    Task<Guid> CreatePipe(Pipe pipe);
    Task<Guid> DeletePipe(Guid id);
    Task<List<Pipe>> GetAllPipes();
    Task<Pipe> GetPipeById(Guid id);
    Task<Guid> UpdatePipe(Guid id, string label, bool isGood, decimal diameter, decimal length, decimal weight, Guid? steelGradeId, Guid? packageId);
    Task<PipeStatistics> GetStatistics();
}