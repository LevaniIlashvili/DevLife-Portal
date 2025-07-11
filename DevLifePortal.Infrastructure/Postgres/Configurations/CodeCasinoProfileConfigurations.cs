using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevLifePortal.Infrastructure.Postgres.Configurations
{
    public class CodeCasinoProfileConfigurations : IEntityTypeConfiguration<CodeCasinoProfile>
    {
        public void Configure(EntityTypeBuilder<CodeCasinoProfile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Points)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithOne(u => u.CodeCasinoProfile)
                .HasForeignKey<CodeCasinoProfile>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId)
                .IsUnique();

            builder.Property(p => p.UserId).IsRequired();
        }
    }
}
