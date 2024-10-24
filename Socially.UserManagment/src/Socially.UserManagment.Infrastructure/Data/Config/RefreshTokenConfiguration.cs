using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.ContentManagment.Core.RefreshTokenAggregate;

namespace Socially.ContentManagment.Infrastructure.Data.Config;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.HasIndex((p) => p.Token)
      .IsUnique();
  }
}
