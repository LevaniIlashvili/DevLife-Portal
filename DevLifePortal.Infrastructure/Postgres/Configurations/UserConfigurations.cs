using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevLifePortal.Infrastructure.Postgres.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.ToTable(t => t
                .HasCheckConstraint(
                    "CK_User_DateOfBirth_NoFuture",
                    "\"DateOfBirth\" <= CURRENT_DATE"));

            builder.Property(u => u.TechStack)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.ExperienceLevel)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Username).IsUnique();

            builder.HasOne(u => u.CodeCasinoProfile)
                .WithOne(p => p.User)
                .HasForeignKey<CodeCasinoProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.BugChaseProfile)
                .WithOne(p => p.User)
                .HasForeignKey<BugChaseProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.DevDatingProfile)
                .WithOne(p => p.User)
                .HasForeignKey<DevDatingProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
