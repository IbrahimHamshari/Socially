using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.UserManagment.Core.RefreshTokenAggregate;

namespace Socially.UserManagment.Infrastructure.Data.Config;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.HasIndex((p)=>p.Token)
      .IsUnique();

  }
}
