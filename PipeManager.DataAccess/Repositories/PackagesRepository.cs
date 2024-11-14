using Microsoft.EntityFrameworkCore;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Repositories
{
    public class PackagesRepository : IPackagesRepository
    {
        private readonly PipeManagerDbContext _context;

        public PackagesRepository(PipeManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Package package)
        {
            var entity = new PackageEntity
            {
                Id = Guid.NewGuid(),
                Number = package.Number,
                Date = package.Date.ToUniversalTime()
            };

            _context.Packages.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Packages
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<List<Package>> Get()
        {
            var entities = await _context.Packages.AsNoTracking().ToListAsync();
            return entities.Select(MapToModel).ToList();
        }

        public async Task<Package> GetById(Guid id)
        {
            var entity = await _context.Packages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException("Package not found");
            }

            return MapToModel(entity);
        }

        public async Task<Guid> Update(Guid id, string number, DateTime date)
        {
            await _context.Packages
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(e => e.Number, number)
                    .SetProperty(e => e.Date, date.ToUniversalTime())
                );

            return id;
        }

        private Package MapToModel(PackageEntity entity)
        {
            // Используем статический метод Create для создания Package
            var packageResult = Package.Create(
                entity.Id,
                entity.Number,
                entity.Date
            );

            if (!packageResult.IsSuccess)
            {
                throw new InvalidOperationException(packageResult.Error);
            }

            return packageResult.Value;
        }
    }
}
