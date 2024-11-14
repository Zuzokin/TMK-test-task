using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions
{
    public interface ISteelGradesRepository
    {
        Task<Guid> Create(SteelGrade steelGrade);
        Task<Guid> Delete(Guid id);
        Task<List<SteelGrade>> Get();
        Task<SteelGrade> GetById(Guid id);
        Task<Guid> Update(Guid id, string name);
    }
}