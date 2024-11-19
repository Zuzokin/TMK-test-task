using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions
{
    public interface IPackagesRepository
    {
        Task<Guid> Create(Package package);
        Task<Guid> Delete(Guid id);
        Task<List<Package>> Get();
        Task<Package> GetById(Guid id);
        Task<Guid> Update(Guid id, string number, DateTime date);
        Task<bool> HasPipes(Guid packageId);
    }
}