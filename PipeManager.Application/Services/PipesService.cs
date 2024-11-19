using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class PipesService : IPipesService
{
    private readonly IPipesRepository _pipesRepository;

    public PipesService(IPipesRepository pipesRepository)
    {
        _pipesRepository = pipesRepository;
    }

    public async Task<Guid> CreatePipe(Pipe pipe)
    {
        if (!await _pipesRepository.SteelGradeExists(pipe.SteelGradeId))
        {
            throw new InvalidOperationException("The specified SteelGradeId does not exist.");
        }
        
        // Если PackageId указан, проверяем, существует ли указанный PackageId
        if (pipe.PackageId.HasValue && !await _pipesRepository.PackageExists(pipe.PackageId.Value))
        {
            throw new InvalidOperationException("The specified PackageId does not exist.");
        }

        return await _pipesRepository.Create(pipe);
    }

    public async Task<Guid> DeletePipe(Guid id)
    {
        if (await _pipesRepository.IsPipeInPackage(id))
        {
            throw new InvalidOperationException("Cannot delete a pipe that is part of a package.");
        }

        return await _pipesRepository.Delete(id);
    }

    public async Task<List<Pipe>> GetAllPipes()
    {
        var pipes = await _pipesRepository.Get();
        return pipes ?? new List<Pipe>();
    }

    public async Task<Pipe> GetPipeById(Guid id)
    {
        return await _pipesRepository.GetById(id);
    }

    public async Task<Guid> UpdatePipe(
        Guid id,
        string label,
        bool isGood,
        decimal diameter,
        decimal length,
        decimal weight,
        Guid? steelGradeId,
        Guid? packageId)
    {
        if (await _pipesRepository.IsPipeInPackage(id))
        {
            throw new InvalidOperationException("Cannot update a pipe that is part of a package.");
        }

        return await _pipesRepository.Update(id, label, isGood, diameter, length, weight, steelGradeId, packageId);
    }

    public async Task<PipeStatistics> GetStatistics()
    {
        return await _pipesRepository.GetStatistics();
    }

    public async Task<List<Pipe>> FilterPipes(PipeFilter filter)
    {
        return await _pipesRepository.GetFilteredPipes(filter);
    }
}