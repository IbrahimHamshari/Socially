using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.UserManagment.Core.UserAggregate;

namespace Socially.UserManagment.Infrastructure.Data.Config;
public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
{
  public void Configure(EntityTypeBuilder<UserConnection> builder)
  {
    builder.HasOne<User>()
      .WithMany()
      .HasForeignKey(uc => uc.FollowerId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne<User>()
      .WithMany()
      .HasForeignKey(uc => uc.FollowedId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(uc => new { uc.FollowerId, uc.FollowedId }).IsUnique();
  }
}
