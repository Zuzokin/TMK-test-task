using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Configurations
{
    public class SteelGradesConfiguration : IEntityTypeConfiguration<SteelGradeEntity>
    {
        public void Configure(EntityTypeBuilder<SteelGradeEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(sg => sg.Name)
                //todo add max lentgth to steelGradeClass
                .HasMaxLength(100)  // Максимальная длина для имени марки стали
                .IsRequired();

            builder.HasMany(sg => sg.Pipes)  // Связь с трубами
                .WithOne(p => p.SteelGrade)
                .HasForeignKey(p => p.SteelGradeId);
        }
    }
}