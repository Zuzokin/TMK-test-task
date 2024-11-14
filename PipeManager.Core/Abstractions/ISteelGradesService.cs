using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions;

public interface ISteelGradesService
{
    Task<Guid> CreateSteelGrade(SteelGrade steelGrade);
    Task<Guid> DeleteSteelGrade(Guid id);
    Task<List<SteelGrade>> GetAllSteelGrades();
    Task<SteelGrade> GetSteelGradeById(Guid id);
    Task<Guid> UpdateSteelGrade(Guid id, string name);
}