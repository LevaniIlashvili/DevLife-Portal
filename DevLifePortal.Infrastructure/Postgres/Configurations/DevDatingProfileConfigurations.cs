using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevLifePortal.Infrastructure.Postgres.Configurations
{
    public class DevDatingProfileConfigurations : IEntityTypeConfiguration<DevDatingProfile>
    {
        public void Configure(EntityTypeBuilder<DevDatingProfile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.IsMale)
                .IsRequired();

            builder.Property(p => p.PrefersMale)
                .IsRequired();

            builder.Property(p => p.Bio)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.HasIndex(p => p.UserId)
                .IsUnique();

            builder.HasOne(p => p.User)
                .WithOne(u => u.DevDatingProfile)
                .HasForeignKey<DevDatingProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
