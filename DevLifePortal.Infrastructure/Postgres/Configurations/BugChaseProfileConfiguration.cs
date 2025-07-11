using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevLifePortal.Infrastructure.Postgres.Configurations
{
    public class BugChaseProfileConfigurations : IEntityTypeConfiguration<BugChaseProfile>
    {
        public void Configure(EntityTypeBuilder<BugChaseProfile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.MaxScore)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithOne(u => u.BugChaseProfile)
                .HasForeignKey<BugChaseProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId)
                .IsUnique();

            builder.Property(p => p.UserId).IsRequired();
        }
    }
}