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
        return await _packagesRepository.Delete(id);
    }

    public async Task<List<Package>> GetAllPackages()
    {
        return await _packagesRepository.Get();
    }

    public async Task<Package> GetPackageById(Guid id)
    {
        var package = await _packagesRepository.GetById(id);
        if (package == null)
        {
            throw new KeyNotFoundException("Package not found.");
        }
        return package;
    }

    public async Task<Guid> UpdatePackage(Guid id, string number, DateTime date)
    {
        return await _packagesRepository.Update(id, number, date);
    }
    
    public async Task<Result<Package>> AddPipesToPackage(Guid packageId, List<Guid> pipeIds)
    {
        var package = await _packagesRepository.GetById(packageId);

        if (package == null)
        {
            return Result<Package>.Failure("Package not found.");
        }

        foreach (var pipeId in pipeIds)
        {
            var pipe = await _pipesRepository.GetById(pipeId);
            if (pipe == null)
            {
                return Result<Package>.Failure($"Pipe with ID {pipeId} not found.");
            }

            if (pipe.PackageId != null)
            {
                return Result<Package>.Failure($"Pipe with ID {pipeId} is already in a package.");
            }

            pipe.PackageId = packageId;
            await _pipesRepository.Update(pipe.Id, pipe.Label, pipe.IsGood, pipe.Diameter, pipe.Length, pipe.Weight, pipe.SteelGradeId, pipe.PackageId );
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
        await _pipesRepository.Update(
            pipe.Id,
            pipe.Label,
            pipe.IsGood,
            pipe.Diameter,
            pipe.Length,
            pipe.Weight,
            pipe.SteelGradeId,
            pipe.PackageId);

        return Result<Guid>.Success(pipeId);
    }


}