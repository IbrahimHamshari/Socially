using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.RefreshTokenAggregate;

namespace Socially.UserManagment.UseCases.Users.Interfaces;
public interface ITokenGenerator
{
  string GenerateAccessToken(Guid userId);
  Task<RefreshToken> GenerateRefreshToken(Guid userId, string? parentToken = null);
}
