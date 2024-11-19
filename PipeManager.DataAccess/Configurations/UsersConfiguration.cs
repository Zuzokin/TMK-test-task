using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipeManager.DataAccess.Entites;

namespace PipeManager.DataAccess.Configurations;

public class UsersConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(sg => sg.Email)
            .HasMaxLength(100)  
            .IsRequired();
        builder.Property(sg => sg.PasswordHash)
            .IsRequired();
    }
}