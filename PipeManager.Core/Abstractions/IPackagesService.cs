using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions;

public interface IPackagesService
{
    Task<Guid> CreatePackage(Package package);
    Task<Guid> DeletePackage(Guid id);
    Task<List<Package>> GetAllPackages();
    Task<Package> GetPackageById(Guid id);
    Task<Guid> UpdatePackage(Guid id, string number, DateTime date);
}