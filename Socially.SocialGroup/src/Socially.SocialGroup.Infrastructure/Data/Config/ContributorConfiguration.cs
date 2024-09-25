﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.SocialGroup.Core.ContributorAggregate;

namespace Socially.SocialGroup.Infrastructure.Data.Config;
public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
{
  public void Configure(EntityTypeBuilder<Contributor> builder)
  {
    builder.Property(p => p.Name)
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

    builder.OwnsOne(builder => builder.PhoneNumber);

    builder.Property(x => x.Status)
      .HasConversion(
          x => x.Value,
          x => ContributorStatus.FromValue(x));
  }
}