using Microsoft.EntityFrameworkCore;
using PipeManager.DataAccess.Configurations;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess
{
    public class PipeManagerDbContext : DbContext
    {
        public PipeManagerDbContext(DbContextOptions<PipeManagerDbContext> options)
            : base(options)
        { }

        public DbSet<PipeEntity> Pipes { get; set; }
        public DbSet<SteelGradeEntity> SteelGrades { get; set; }
        public DbSet<PackageEntity> Packages { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Регистрация конфигураций
            modelBuilder.ApplyConfiguration(new PipesConfiguration());
            modelBuilder.ApplyConfiguration(new SteelGradesConfiguration());
            modelBuilder.ApplyConfiguration(new PackagesConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }

    }
}