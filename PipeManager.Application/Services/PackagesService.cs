using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;

namespace PipeManager.Application.Services;

public class PackagesService : IPackagesService
{
    private readonly IPackagesRepository _packagesRepository;

    public PackagesService(IPackagesRepository packagesRepository)
    {
        _packagesRepository = packagesRepository;
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
}