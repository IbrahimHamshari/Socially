using Xunit;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.RefreshTokenAggregate.Specifications;
using Socially.UserManagment.UseCases.Users.Interfaces;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Socially.UserManagment.UseCases.Users.Refresh;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.UnitTests.UseCases.Users;

public class RefreshCommandHandlerTests
{
  private readonly IRepository<RefreshToken> _repository;
  private readonly ITokenGenerator _tokenGenerator;
  private readonly ILogger<RefreshCommandHandler> _logger;
  private readonly RefreshCommandHandler _handler;

  public RefreshCommandHandlerTests()
  {
    _repository = Substitute.For<IRepository<RefreshToken>>();
    _tokenGenerator = Substitute.For<ITokenGenerator>();
    _logger = Substitute.For<ILogger<RefreshCommandHandler>>();
    _handler = new RefreshCommandHandler(_repository, _tokenGenerator, _logger);
  }

  // Test 1: Ensure valid refresh token generates new tokens
  [Fact]
  public async Task Handle_WithValidRefreshToken_ShouldGenerateNewTokens()
  {
    // Arrange
    var refreshToken = new RefreshToken(Guid.NewGuid(), "validToken", DateTimeOffset.UtcNow.AddDays(1), null, "family");
    var command = new RefreshCommand("validToken");

    _repository.FirstOrDefaultAsync(Arg.Any<GetByTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns(refreshToken);

    _tokenGenerator.GenerateAccessToken(refreshToken.UserId).Returns("newAccessToken");
    _tokenGenerator.GenerateRefreshToken(refreshToken.UserId, refreshToken.Token)
        .Returns(Task.FromResult(new RefreshToken(refreshToken.UserId, "newRefreshToken", DateTimeOffset.UtcNow.AddDays(1), null, refreshToken.Family)));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Contain(new[] { "newAccessToken", "newRefreshToken" });

    // Verify that tokens were generated
    _tokenGenerator.Received(1).GenerateAccessToken(refreshToken.UserId);
    await _tokenGenerator.Received(1).GenerateRefreshToken(refreshToken.UserId, refreshToken.Token);
  }

  // Test 2: Ensure expired refresh token returns unauthorized
  [Fact]
  public async Task Handle_WithExpiredRefreshToken_ShouldReturnUnauthorized()
  {
    // Arrange
    var refreshToken = new RefreshToken(Guid.NewGuid(), "expiredToken", DateTimeOffset.UtcNow.AddDays(-1), null, "family");
    var command = new RefreshCommand("expiredToken");

    _repository.FirstOrDefaultAsync(Arg.Any<GetByTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns(refreshToken);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.Unauthorized);

    // Verify that no tokens were generated
    _tokenGenerator.DidNotReceive().GenerateAccessToken(Arg.Any<Guid>());
    await _tokenGenerator.DidNotReceive().GenerateRefreshToken(Arg.Any<Guid>(), Arg.Any<string>());
  }
  // Test 3: Ensure revoked refresh token logs a critical message and returns unauthorized
  [Fact]
  public async Task Handle_WithRevokedRefreshToken_ShouldLogCriticalAndReturnUnauthorized()
  {
    // Arrange
    var refreshToken = new RefreshToken(Guid.NewGuid(), "revokedToken", DateTimeOffset.UtcNow.AddDays(1), null, "family");
    refreshToken.Revoke();
    var command = new RefreshCommand("revokedToken");

    _repository.FirstOrDefaultAsync(Arg.Any<GetByTokenSpec>(), Arg.Any<CancellationToken>())
        .Returns(refreshToken);

    var refreshTokenList = new List<RefreshToken> { refreshToken };
    _repository.ListAsync(Arg.Any<GetByFamilySpec>(), Arg.Any<CancellationToken>())
        .Returns(Task.FromResult(refreshTokenList));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Status.Should().Be(ResultStatus.Unauthorized);

    // Verify that no tokens were generated
    _tokenGenerator.DidNotReceive().GenerateAccessToken(Arg.Any<Guid>());
    await _tokenGenerator.DidNotReceive().GenerateRefreshToken(Arg.Any<Guid>(), Arg.Any<string>());

    // Verify that a critical log was created with the correct message and arguments
    _logger.Received(1).Log(
        LogLevel.Critical,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString()!.Contains($"Someone Tried to use an Old Refresh Token {refreshToken.Id} for the user of {refreshToken.UserId}")),
        null,
        Arg.Any<Func<object, Exception?, string>>());
  }

}
