using Ardalis.SharedKernel;
using Microsoft.IdentityModel.Tokens;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Shared.Config.JWT;
using Socially.UserManagment.UseCases.Users.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Socially.UserManagment.Infrastructure.Token;
public class TokenGenerator : ITokenGenerator
{
  private readonly JWTSettings _jwtSettings; // Inject settings for JWT (key, expiry, etc.)
  private readonly IRepository<RefreshToken> _repository;

  public TokenGenerator(JWTSettings jwtSettings, IRepository<RefreshToken> repository)
  {
    _jwtSettings = jwtSettings;
    _repository = repository;
  }

  // Generate the access token (short-lived, typically a JWT)
  public string GenerateAccessToken(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
        {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
        }),
      Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  // Generate the refresh token (long-lived)
  // Generate the refresh token (long-lived) with a family
  public async Task<RefreshToken> GenerateRefreshToken(User user, string? parentToken = null)
  {
    // Generate a new refresh token (e.g., 64-byte secure token)
    var refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    // Generate or reuse a family identifier
    string familyId = parentToken != null
        ? (await GetFamilyFromParentToken(parentToken))  // Fetch the family from the parent token
        : Guid.NewGuid().ToString();  // Create a new family ID if no parent

    // Set the parent token ID if this token is part of a chain
    var parentRefreshToken = parentToken != null
        ? (await _repository.FirstOrDefaultAsync(new GetByTokenSpec(parentToken)))
        : null;
    var parentTokenId = parentRefreshToken?.Id;
    if (parentRefreshToken != null)
    {
      parentRefreshToken.Revoke();
    }
    // Create the refresh token entity
    var refreshToken = new RefreshToken(
        user.Id,
        refreshTokenString,
        DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
        parentTokenId,
        familyId
    );

    // Save the refresh token to the database
    await _repository.AddAsync(refreshToken);

    return refreshToken;
  }


  private async Task<string> GetFamilyFromParentToken(string parentToken)
  {
    var parent = await _repository.FirstOrDefaultAsync(new GetByTokenSpec(parentToken));
    if (parent == null || parent.IsExpired || parent.IsRevoked)
    {
      throw new InvalidOperationException("Invalid or revoked parent token.");
    }
    return parent.Family;
  }
}
