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
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Content).IsRequired().HasMaxLength(DataSchemaConstants.DEFAULT_CONTENT_LENGTH);
    builder.Property(x => x.Privacy)
      .HasConversion(
      x => x.Value,
      x => Privacy.FromValue(x));
    builder.Property(x => x.UserId).IsRequired();
    builder.Property(x => x.CreatedAt).IsRequired();
    builder.Property(x => x.UpdatedAt).IsRequired();
    builder.HasOne<User>()
      .WithMany()
      .HasForeignKey(p => p.UserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
