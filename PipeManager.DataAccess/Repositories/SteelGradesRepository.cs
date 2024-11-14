using Microsoft.EntityFrameworkCore;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Repositories
{
    public class SteelGradesRepository : ISteelGradesRepository
    {
        private readonly PipeManagerDbContext _context;

        public SteelGradesRepository(PipeManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(SteelGrade steelGrade)
        {
            var entity = new SteelGradeEntity
            {
                Id = Guid.NewGuid(),
                Name = steelGrade.Name
            };

            _context.SteelGrades.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.SteelGrades
                .Where(sg => sg.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<List<SteelGrade>> Get()
        {
            var entities = await _context.SteelGrades.AsNoTracking().ToListAsync();
            return entities.Select(MapToModel).ToList();
        }

        public async Task<SteelGrade> GetById(Guid id)
        {
            var entity = await _context.SteelGrades
                .AsNoTracking()
                .FirstOrDefaultAsync(sg => sg.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException("Steel grade not found");
            }

            return MapToModel(entity);
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            await _context.SteelGrades
                .Where(sg => sg.Id == id)
                .ExecuteUpdateAsync(sg => sg
                    .SetProperty(e => e.Name, name)
                );

            return id;
        }

        private SteelGrade MapToModel(SteelGradeEntity entity)
        {
            // Используем статический метод Create для создания SteelGrade
            var steelGradeResult = SteelGrade.Create(
                entity.Id,
                entity.Name
            );

            if (!steelGradeResult.IsSuccess)
            {
                throw new InvalidOperationException(steelGradeResult.Error);
            }

            return steelGradeResult.Value;
        }

    }
}
