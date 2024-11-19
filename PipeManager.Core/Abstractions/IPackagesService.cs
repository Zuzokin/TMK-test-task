using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions;

public interface IPackagesService
{
    Task<Guid> CreatePackage(Package package);
    Task<Guid> DeletePackage(Guid id);
    Task<List<Package>> GetAllPackages();
    Task<Package> GetPackageById(Guid id);
    Task<Guid> UpdatePackage(Guid id, string number, DateTime date);
    Task<Result<Package>> AddPipesToPackage(Guid packageId, List<Guid> pipeIds);
    Task<Result<Guid>> RemovePipeFromPackage(Guid packageId, Guid pipeId);
    Task<List<Pipe>> GetPipesInPackage(Guid packageId);
    Task<bool> HasPipes(Guid packageId);
}