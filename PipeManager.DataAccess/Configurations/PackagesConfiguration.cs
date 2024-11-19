using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipeManager.Core.Models;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Configurations
{
    public class PackagesConfiguration : IEntityTypeConfiguration<PackageEntity>
    {
        public void Configure(EntityTypeBuilder<PackageEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Number)
                .HasMaxLength(Package.MAX_NUMBER_LENGTH)  // Максимальная длина для номера пакета
                .IsRequired();

            builder.Property(p => p.Date)
                .IsRequired();

            builder.HasMany(p => p.Pipes)  // Связь с трубами
                .WithOne(pipe => pipe.Package)
                .HasForeignKey(pipe => pipe.PackageId)
                .OnDelete(DeleteBehavior.SetNull);  // Устанавливаем поведение при удалении пакета
        }
    }
}