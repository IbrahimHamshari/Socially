using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.UserManagment.Core.RefreshTokenAggregate;

namespace Socially.UserManagment.Infrastructure.Data.Config;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.HasIndex((p) => p.Token)
      .IsUnique();
  }
}
