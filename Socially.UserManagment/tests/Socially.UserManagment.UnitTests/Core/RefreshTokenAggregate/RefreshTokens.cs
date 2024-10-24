using FluentAssertions;
using Socially.ContentManagment.Core.RefreshTokenAggregate;
using Xunit;

namespace Socially.ContentManagment.UnitTests.Core.RefreshTokenAggregate;

public class RefreshTokenTests
{
  // Test 1: Ensure that a refresh token is correctly created
  [Fact]
  public void RefreshToken_ShouldInitializeWithCorrectValues()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var token = "sample-token";
    var expiration = DateTimeOffset.UtcNow.AddHours(1);
    var family = "family-token";
    Guid? parentTokenId = null;

    // Act
    var refreshToken = new RefreshToken(userId, token, expiration, parentTokenId, family);

    // Assert
    refreshToken.UserId.Should().Be(userId);
    refreshToken.Token.Should().Be(token);
    refreshToken.Expiration.Should().Be(expiration);
    refreshToken.Family.Should().Be(family);
    refreshToken.ParentTokenId.Should().BeNull();
    refreshToken.IsRevoked.Should().BeFalse();
    refreshToken.RevokedAt.Should().BeNull();
    refreshToken.IsExpired.Should().BeFalse();
  }

  // Test 2: Ensure that a refresh token can be revoked
  [Fact]
  public void Revoke_ShouldSetIsRevokedToTrueAndSetRevokedAt()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var token = "sample-token";
    var expiration = DateTimeOffset.UtcNow.AddHours(1);
    var family = "family-token";
    var refreshToken = new RefreshToken(userId, token, expiration, null, family);

    // Act
    refreshToken.Revoke();

    // Assert
    refreshToken.IsRevoked.Should().BeTrue();
    refreshToken.RevokedAt.Should().NotBeNull();
    refreshToken.RevokedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
  }

  // Test 3: Ensure that the IsExpired property works correctly for non-expired token
  [Fact]
  public void IsExpired_ShouldReturnFalse_WhenTokenIsNotExpired()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var token = "sample-token";
    var expiration = DateTimeOffset.UtcNow.AddHours(1);  // Token expires in 1 hour
    var refreshToken = new RefreshToken(userId, token, expiration, null, "family-token");

    // Act
    var isExpired = refreshToken.IsExpired;

    // Assert
    isExpired.Should().BeFalse();
  }

  // Test 4: Ensure that the IsExpired property works correctly for expired token
  [Fact]
  public void IsExpired_ShouldReturnTrue_WhenTokenIsExpired()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var token = "sample-token";
    var expiration = DateTimeOffset.UtcNow.AddHours(-1);  // Token expired 1 hour ago
    var refreshToken = new RefreshToken(userId, token, expiration, null, "family-token");

    // Act
    var isExpired = refreshToken.IsExpired;

    // Assert
    isExpired.Should().BeTrue();
  }

  // Test 5: Ensure that ParentTokenId is set correctly when provided
  [Fact]
  public void RefreshToken_ShouldSetParentTokenIdWhenProvided()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var token = "sample-token";
    var expiration = DateTimeOffset.UtcNow.AddHours(1);
    var family = "family-token";
    var parentTokenId = Guid.NewGuid();

    // Act
    var refreshToken = new RefreshToken(userId, token, expiration, parentTokenId, family);

    // Assert
    refreshToken.ParentTokenId.Should().Be(parentTokenId);
  }
}
