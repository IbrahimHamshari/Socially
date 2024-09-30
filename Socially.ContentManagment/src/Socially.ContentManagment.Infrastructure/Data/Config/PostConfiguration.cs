using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.ContentManagment.Core.PostAggregate;

namespace Socially.ContentManagment.Infrastructure.Data.Config;
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
  public void Configure(EntityTypeBuilder<Post> builder)
  {
    builder.Property(x => x.Content).IsRequired().HasMaxLength(DataSchemaConstants.DEFAULT_CONTENT_LENGTH);
    builder.Property(x => x.Privacy)
      .HasConversion(
      x => x.Value,
      x => Privacy.FromValue(x));
  }
}
