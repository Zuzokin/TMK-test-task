using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class PackagesService : IPackagesService
{
    private readonly IPackagesRepository _packagesRepository;
    private readonly IPipesRepository _pipesRepository;

    public PackagesService(IPackagesRepository packagesRepository, IPipesRepository pipesRepository)
    {
        _packagesRepository = packagesRepository;
        _pipesRepository = pipesRepository;
    }

    public async Task<Guid> CreatePackage(Package package)
    {
        return await _packagesRepository.Create(package);
    }

    public async Task<Guid> DeletePackage(Guid id)
    {
        // Проверка на существование пакета
        await EnsurePackageExists(id);

        return await _packagesRepository.Delete(id);
    }

    public async Task<List<Package>> GetAllPackages()
    {
        return await _packagesRepository.Get() ?? new List<Package>();
    }

    public async Task<Package> GetPackageById(Guid id)
    {
        return await EnsurePackageExists(id);
    }

    public async Task<Guid> UpdatePackage(Guid id, string number, DateTime date)
    {
        // Проверка на существование пакета
        await EnsurePackageExists(id);

        return await _packagesRepository.Update(id, number, date);
    }

    public async Task<Result<Package>> AddPipesToPackage(Guid packageId, List<Guid> pipeIds)
    {
        var package = await EnsurePackageExists(packageId);

        var notFoundPipes = new List<Guid>();
        var alreadyInPackagePipes = new List<Guid>();

        foreach (var pipeId in pipeIds)
        {
            var pipe = await _pipesRepository.GetById(pipeId);

            if (pipe == null)
            {
                notFoundPipes.Add(pipeId);
                continue;
            }

            if (pipe.PackageId != null)
            {
                alreadyInPackagePipes.Add(pipeId);
                continue;
            }

            // Обновляем пакет ID для трубы
            pipe.PackageId = packageId;
            await _pipesRepository.Update(pipe.Id, pipe.Label, pipe.IsGood, pipe.Diameter, pipe.Length, pipe.Weight, pipe.SteelGradeId, pipe.PackageId);
        }

        if (notFoundPipes.Any() || alreadyInPackagePipes.Any())
        {
            var errorMessages = new List<string>();

            if (notFoundPipes.Any())
            {
                errorMessages.Add($"Pipes not found: {string.Join(", ", notFoundPipes)}.");
            }

            if (alreadyInPackagePipes.Any())
            {
                errorMessages.Add($"Pipes already in another package: {string.Join(", ", alreadyInPackagePipes)}.");
            }

            return Result<Package>.Failure(string.Join(" ", errorMessages));
        }

        return Result<Package>.Success(package);
    }

    public async Task<Result<Guid>> RemovePipeFromPackage(Guid packageId, Guid pipeId)
    {
        var pipe = await _pipesRepository.GetById(pipeId);

        if (pipe == null)
        {
            return Result<Guid>.Failure("Pipe not found.");
        }

        if (pipe.PackageId != packageId)
        {
            return Result<Guid>.Failure("Pipe does not belong to the specified package.");
        }

        pipe.PackageId = null;

        await _pipesRepository.Update(pipe.Id, pipe.Label, pipe.IsGood, pipe.Diameter, pipe.Length, pipe.Weight, pipe.SteelGradeId, pipe.PackageId);

        return Result<Guid>.Success(pipeId);
    }

    public async Task<List<Pipe>> GetPipesInPackage(Guid packageId)
    {
        // Проверяем, существует ли пакет
        var package = await _packagesRepository.GetById(packageId);
        if (package == null)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        // Получаем трубы в пакете
        var pipes = await _pipesRepository.GetPipesInPackage(packageId);
        return pipes;
    }

    public async Task<bool> HasPipes(Guid packageId)
    {
        return await _packagesRepository.HasPipes(packageId);
    }

    private async Task<Package> EnsurePackageExists(Guid packageId)
    {
        var package = await _packagesRepository.GetById(packageId);
        if (package == null)
        {
            throw new KeyNotFoundException($"Package with ID {packageId} not found.");
        }

        return package;
    }
}
