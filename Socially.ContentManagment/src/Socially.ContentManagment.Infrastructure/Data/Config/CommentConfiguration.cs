using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.ContentManagment.Core.PostAggregate;

namespace Socially.ContentManagment.Infrastructure.Data.Config;
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
  public void Configure(EntityTypeBuilder<Comment> builder)
  {
    builder.Property(c => c.Content).IsRequired().HasMaxLength(DataSchemaConstants.DEFAULT_COMMENT_LENGTH);
  }
}
