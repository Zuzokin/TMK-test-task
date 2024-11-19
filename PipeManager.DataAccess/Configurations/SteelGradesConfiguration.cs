using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Configurations
{
    public class SteelGradesConfiguration : IEntityTypeConfiguration<SteelGradeEntity>
    {
        public void Configure(EntityTypeBuilder<SteelGradeEntity> builder)
        {
// Первичный ключ
            builder.HasKey(x => x.Id);

            // Свойство Name
            builder.Property(sg => sg.Name)
                .HasMaxLength(SteelGrade.MAX_NAME_LENGTH) 
                .IsRequired();

            // Связь с PipeEntity (один ко многим)
            builder.HasMany(sg => sg.Pipes)
                .WithOne(p => p.SteelGrade)
                .HasForeignKey(p => p.SteelGradeId)
                .OnDelete(DeleteBehavior.Restrict); // Отключить каскадное удаление
        }
    }
}