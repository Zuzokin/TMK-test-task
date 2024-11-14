using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Configurations
{
    public class PipesConfiguration : IEntityTypeConfiguration<PipeEntity>
    {
        public void Configure(EntityTypeBuilder<PipeEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Label)
                .HasMaxLength(Pipe.MAX_NUMBER_LENGTH)  // Максимальная длина для Label (можно изменить в зависимости от требований)
                .IsRequired();

            builder.Property(p => p.IsGood)
                .IsRequired();

            builder.Property(p => p.Diameter)
                .HasPrecision(18, 2)  // Устанавливаем точность для диаметра (например, до 2 знаков после запятой)
                .IsRequired();

            builder.Property(p => p.Length)
                .HasPrecision(18, 2)  // Устанавливаем точность для длины
                .IsRequired();

            builder.Property(p => p.Weight)
                .HasPrecision(18, 2)  // Устанавливаем точность для веса
                .IsRequired();

            builder.HasOne(p => p.SteelGrade)  // Связь с маркой стали
                .WithMany(sg => sg.Pipes)
                .HasForeignKey(p => p.SteelGradeId)
                .IsRequired();

            builder.HasOne(p => p.Package)  // Связь с пакетом
                .WithMany(pkg => pkg.Pipes)
                .HasForeignKey(p => p.PackageId)
                .OnDelete(DeleteBehavior.SetNull);  // Устанавливаем поведение при удалении пакета (например, пакет может быть удален, но труба не теряет связь)
        }
    }
}