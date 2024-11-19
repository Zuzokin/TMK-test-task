using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class SteelGradesService : ISteelGradesService
{
    private readonly ISteelGradesRepository _steelGradesRepository;

    public SteelGradesService(ISteelGradesRepository steelGradesRepository)
    {
        _steelGradesRepository = steelGradesRepository;
    }

    public async Task<Guid> CreateSteelGrade(SteelGrade steelGrade)
    {
        // Создание SteelGrade через Create с проверкой на ошибки
        var steelGradeResult = SteelGrade.Create(Guid.NewGuid(), steelGrade.Name);

        if (!steelGradeResult.IsSuccess)
        {
            throw new InvalidOperationException(steelGradeResult.Error);
        }

        return await _steelGradesRepository.Create(steelGradeResult.Value);
    }

    public async Task<Guid> DeleteSteelGrade(Guid id)
    {
        return await _steelGradesRepository.Delete(id);
    }

    public async Task<List<SteelGrade>> GetAllSteelGrades()
    {
        var steelGrades = await _steelGradesRepository.Get();
        return steelGrades ?? new List<SteelGrade>();
    }

    public async Task<SteelGrade> GetSteelGradeById(Guid id)
    {
        return await _steelGradesRepository.GetById(id);
    }

    public async Task<Guid> UpdateSteelGrade(Guid id, string name)
    {
        return await _steelGradesRepository.Update(id, name);
    }
}