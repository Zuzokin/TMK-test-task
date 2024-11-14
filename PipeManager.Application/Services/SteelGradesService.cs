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
        return await _steelGradesRepository.Create(steelGrade);
    }

    public async Task<Guid> DeleteSteelGrade(Guid id)
    {
        return await _steelGradesRepository.Delete(id);
    }

    public async Task<List<SteelGrade>> GetAllSteelGrades()
    {
        return await _steelGradesRepository.Get();
    }

    public async Task<SteelGrade> GetSteelGradeById(Guid id)
    {
        var steelGrade = await _steelGradesRepository.GetById(id);
        //todo getbyid возращает ошибку, а не null, переделать
        if (steelGrade == null)
        {
            throw new KeyNotFoundException("Steel grade not found.");
        }
        return steelGrade;
    }

    public async Task<Guid> UpdateSteelGrade(Guid id, string name)
    {
        return await _steelGradesRepository.Update(id, name);
    }
}