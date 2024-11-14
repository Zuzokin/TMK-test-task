using Microsoft.EntityFrameworkCore;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Repositories;

public class PipesRepository : IPipesRepository
{
    private readonly PipeManagerDbContext _context;

    public PipesRepository(PipeManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Create(Pipe pipe)
    {
        var entity = new PipeEntity
        {
            Id = Guid.NewGuid(),
            Label = pipe.Label,
            IsGood = pipe.IsGood,
            Diameter = pipe.Diameter,
            Length = pipe.Length,
            Weight = pipe.Weight,
            SteelGradeId = pipe.SteelGradeId,
            PackageId = pipe.PackageId
        };

        await _context.Pipes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Pipes
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<List<Pipe>> Get()
    {
        var entities = await _context.Pipes
            .AsNoTracking()
            .Include(p => p.SteelGrade)
            .Include(p => p.Package)
            .ToListAsync();

        return entities.Select(e => MapToModel(e)).ToList();
    }

    public async Task<Pipe> GetById(Guid id)
    {
        var entity = await _context.Pipes
            .AsNoTracking()
            .Include(p => p.SteelGrade)
            .Include(p => p.Package)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            throw new KeyNotFoundException("Pipe not found");
        }

        return MapToModel(entity);
    }

    public async Task<Guid> Update(Guid id, string label, bool isGood, decimal diameter, decimal length, decimal weight, Guid? steelGradeId, Guid? packageId)
    {
        await _context.Pipes
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(pipe => pipe.Label, label)
                .SetProperty(pipe => pipe.IsGood, isGood)
                .SetProperty(pipe => pipe.Diameter, diameter)
                .SetProperty(pipe => pipe.Length, length)
                .SetProperty(pipe => pipe.Weight, weight)
                .SetProperty(pipe => pipe.SteelGradeId, steelGradeId)
                .SetProperty(pipe => pipe.PackageId, packageId)
            );

        return id;
    }

    // Метод для проверки существования SteelGrade
    public async Task<bool> SteelGradeExists(Guid steelGradeId)
    {
        return await _context.SteelGrades.AnyAsync(sg => sg.Id == steelGradeId);
    }

    #region NotImplemented
    
    public async Task<PipeStatistics> GetStatistics()
    {
        var totalCount = await _context.Pipes.CountAsync();
        var goodCount = await _context.Pipes.CountAsync(p => p.IsGood);
        var defectiveCount = totalCount - goodCount;
        var totalWeight = await _context.Pipes.SumAsync(p => p.Weight);

        return new PipeStatistics
        {
            TotalCount = totalCount,
            GoodCount = goodCount,
            DefectiveCount = defectiveCount,
            TotalWeight = totalWeight
        };
    }


    public async Task<List<Pipe>> FilterPipes(bool? isGood = null, Guid? steelGradeId = null, decimal? minDiameter = null,
        decimal? maxDiameter = null, decimal? minWeight = null, decimal? maxWeight = null)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsPipeInPackage(Guid pipeId)
    {
        throw new NotImplementedException();
    }

    public async Task AddPipeToPackage(Guid pipeId, Guid packageId)
    {
        throw new NotImplementedException();
    }

    public async Task RemovePipeFromPackage(Guid pipeId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Pipe>> GetPipesInPackage(Guid packageId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CountPipesByQuality(bool isGood)
    {
        throw new NotImplementedException();
    }
    #endregion
    
    private Pipe MapToModel(PipeEntity entity)
    {
        // Создание объекта Pipe с использованием статического метода Create
        var pipeResult = Pipe.Create(
            entity.Id,
            entity.Label,
            entity.IsGood,
            entity.SteelGradeId,
            entity.Diameter,
            entity.Length,
            entity.Weight
        );

        if (!pipeResult.IsSuccess)
        {
            throw new InvalidOperationException(pipeResult.Error);
        }

        var pipe = pipeResult.Value;
        pipe.PackageId = entity.PackageId;
        
        var steelGradeResult = SteelGrade.Create(entity.SteelGrade.Id, entity.SteelGrade.Name);

        if (!steelGradeResult.IsSuccess)
        {
            throw new InvalidOperationException(steelGradeResult.Error);
        }

        pipe.SteelGrade = steelGradeResult.Value;
        
        var packageResult = Package.Create(entity.Package.Id, entity.Package.Number, entity.Package.Date);

        if (!packageResult.IsSuccess)
        {
            throw new InvalidOperationException(packageResult.Error);
        }

        pipe.Package = packageResult.Value;

        return pipe;
    }


}