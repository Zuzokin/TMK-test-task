using Microsoft.EntityFrameworkCore;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Repositories;

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
        var rowsAffected = await _context.Packages
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        return id;
    }

    public async Task<List<Package>> Get()
    {
        var entities = await _context.Packages.AsNoTracking().ToListAsync();

        return entities
            .Select(entity =>
            {
                var packageResult = Package.Create(entity.Id, entity.Number, entity.Date);
                if (!packageResult.IsSuccess)
                {
                    throw new InvalidOperationException(packageResult.Error);
                }

                return packageResult.Value;
            })
            .ToList();
    }

    public async Task<Package> GetById(Guid id)
    {
        var entity = await _context.Packages
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        var packageResult = Package.Create(entity.Id, entity.Number, entity.Date);

        if (!packageResult.IsSuccess)
        {
            throw new InvalidOperationException(packageResult.Error);
        }

        return packageResult.Value;
    }

    public async Task<Guid> Update(Guid id, string number, DateTime date)
    {
        var rowsAffected = await _context.Packages
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(e => e.Number, number)
                .SetProperty(e => e.Date, date.ToUniversalTime()));

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        return id;
    }

    public async Task<bool> HasPipes(Guid packageId)
    {
        return await _context.Pipes.AnyAsync(p => p.PackageId == packageId);
    }

}
