using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class PipesService : IPipesService
{
    private readonly IPipesRepository _pipesesRepository;

    public PipesService(IPipesRepository pipesesRepository)
    {
        _pipesesRepository = pipesesRepository;
    }

    public async Task<Guid> CreatePipe(Pipe pipe)
    {
        // Проверка на существование SteelGrade с указанным SteelGradeId
        var steelGradeExists = await _pipesesRepository.SteelGradeExists(pipe.SteelGradeId);
        if (!steelGradeExists)
        {
            throw new InvalidOperationException("The specified SteelGradeId does not exist.");
        }
        return await _pipesesRepository.Create(pipe);
    }

    public async Task<Guid> DeletePipe(Guid id)
    {
        // Проверка, что труба не находится в пакете перед удалением
        bool isInPackage = await _pipesesRepository.IsPipeInPackage(id);
        if (isInPackage)
        {
            throw new InvalidOperationException("Cannot delete a pipe that is part of a package.");
        }

        return await _pipesesRepository.Delete(id);
    }

    public async Task<List<Pipe>> GetAllPipes()
    {
        return await _pipesesRepository.Get();
    }

    public async Task<Pipe> GetPipeById(Guid id)
    {
        var pipe = await _pipesesRepository.GetById(id);
        if (pipe == null)
        {
            throw new KeyNotFoundException("Pipe not found.");
        }
        return pipe;
    }

    public async Task<Guid> UpdatePipe(Guid id, string label, bool isGood, decimal diameter, decimal length, decimal weight, Guid? steelGradeId, Guid? packageId)
    {
        // Проверка, что труба не находится в пакете перед редактированием
        bool isInPackage = await _pipesesRepository.IsPipeInPackage(id);
        if (isInPackage)
        {
            throw new InvalidOperationException("Cannot update a pipe that is part of a package.");
        }

        return await _pipesesRepository.Update(id, label, isGood, diameter, length, weight, steelGradeId, packageId);
    }

    public async Task<PipeStatistics> GetStatistics()
    {
        return await _pipesesRepository.GetStatistics();
    }
}
