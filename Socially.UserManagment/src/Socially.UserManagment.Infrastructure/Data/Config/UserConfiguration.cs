using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.UserManagment.Core.UserAggregate;

namespace Socially.UserManagment.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.Property(p => p.FirstName)
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();
    builder.Property(p => p.LastName)
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();
    builder.HasIndex(p => p.Username).IsUnique();
    builder.Property(p => p.Username)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();
    builder.HasIndex(p => p.VerificationToken).IsUnique();
  }
}
