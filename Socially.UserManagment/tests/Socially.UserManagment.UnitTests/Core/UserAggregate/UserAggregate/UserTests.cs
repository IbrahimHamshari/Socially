using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Socially.UserManagement.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Events;
using Xunit;

namespace Socially.UserManagment.UnitTests.Core.Users.UserAggregate;
public class UserTests
{
  // Test 1: Valid user creation
  [Fact]
  public void CreateUser_WithValidParameters_ShouldSetPropertiesCorrectly()
  {
    // Arrange
    var username = "validUser123";
    var email = "valid@example.com";
    var password = "Password@123"; // Matches the password regex
    var firstName = "John";
    var lastName = "Doe";
    var gender = true;

    // Act
    var user = new User(username, email, password, firstName, lastName, gender);

    // Assert
    user.Username.Should().Be(username);
    user.Email.Should().Be(email);
    user.VerifyPassword(password).Should().BeTrue();
    user.FirstName.Should().Be(firstName);
    user.LastName.Should().Be(lastName);
    user.Gender.Should().Be(gender);
    user.IsActive.Should().BeTrue();
  }

  // Test 2: Invalid username throws ArgumentException
  [Fact]
  public void CreateUser_WithInvalidUsername_ShouldThrowArgumentException()
  {
    // Arrange
    var invalidUsername = "u"; // Too short for regex
    var email = "valid@example.com";
    var password = "Password123";
    var firstName = "John";
    var lastName = "Doe";
    var gender = true;

    // Act & Assert
    Assert.Throws<ArgumentException>(() => new User(invalidUsername, email, password, firstName, lastName, gender))
        .Message.Should().Contain("Input username was not in required format");
  }

  // Test 3: Invalid email throws ArgumentException
  [Fact]
  public void CreateUser_WithInvalidEmail_ShouldThrowArgumentException()
  {
    // Arrange
    var username = "validUser";
    var invalidEmail = "invalid-email"; // Not matching the email regex
    var password = "Password123";
    var firstName = "John";
    var lastName = "Doe";
    var gender = true;

    // Act & Assert
    Assert.Throws<ArgumentException>(() => new User(username, invalidEmail, password, firstName, lastName, gender))
        .Message.Should().Contain("Input email was not in required format");
  }

  // Test 4: Invalid password throws ArgumentException
  [Fact]
  public void CreateUser_WithInvalidPassword_ShouldThrowArgumentException()
  {
    // Arrange
    var username = "validUser";
    var email = "valid@example.com";
    var invalidPassword = "short"; // Not matching the password regex
    var firstName = "John";
    var lastName = "Doe";
    var gender = true;

    // Act & Assert
    Assert.Throws<ArgumentException>(() => new User(username, email, invalidPassword, firstName, lastName, gender));


  }

  // Test 5: Password change success
  [Fact]
  public void ChangePassword_WithValidCurrentPassword_ShouldUpdatePassword()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "OldPassword@123", "John", "Doe", true);
    var newPassword = "NewPassword@123";

    // Act
    user.ChangePassword("OldPassword@123", newPassword);

    // Assert
    user.VerifyPassword(newPassword).Should().BeTrue();
    user.VerifyPassword("OldPassword@123").Should().BeFalse(); // Ensure old password no longer works
  }

  // Test 6: Password change with invalid current password throws UnauthorizedAccessException
  [Fact]
  public void ChangePassword_WithInvalidCurrentPassword_ShouldThrowUnauthorizedAccessException()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "OldPassword@123", "John", "Doe", true);

    // Act & Assert
    Assert.Throws<UnauthorizedAccessException>(() => user.ChangePassword("WrongPassword", "NewPassword@123"))
        .Message.Should().Contain("Current password is incorrect");
  }

  // Test 7: Activate account
  [Fact]
  public void ActivateLogin_WhenUserIsInactive_ShouldActivateUser()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);
    user.DeactivateAccount(); // Initially deactivate

    // Act
    user.ActivateLogin();

    // Assert
    user.IsActive.Should().BeTrue();
  }

  // Test 8: Activate account when already active throws InvalidOperationException
  [Fact]
  public void ActivateLogin_WhenUserIsAlreadyActive_ShouldThrowInvalidOperationException()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => user.ActivateLogin())
        .Message.Should().Contain("Account is already active");
  }

  // Test 9: Deactivate account
  [Fact]
  public void DeactivateAccount_WhenUserIsActive_ShouldDeactivateUser()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);

    // Act
    user.DeactivateAccount();

    // Assert
    user.IsActive.Should().BeFalse();
  }

  // Test 10: Deactivate account when already inactive throws InvalidOperationException
  [Fact]
  public void DeactivateAccount_WhenUserIsAlreadyInactive_ShouldThrowInvalidOperationException()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);
    user.DeactivateAccount(); // Initially deactivate

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => user.DeactivateAccount())
        .Message.Should().Contain("Account is already inactive");
  }

  // Test 11: Email update success
  [Fact]
  public void UpdateEmail_WithValidEmail_ShouldUpdateEmailProperty()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);
    var newEmail = "new@example.com";

    // Act
    user.UpdateEmail(newEmail);

    // Assert
    user.Email.Should().Be(newEmail);
  }

  // Test 12: Generate email verification token
  [Fact]
  public void GenerateEmailVerificationToken_ShouldSetVerificationTokenAndTime()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);

    // Act
    user.GenerateEmailVerificationToken();

    // Assert
    user.VerificationToken.Should().NotBeNullOrEmpty();
    user.TokenGeneratedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: TimeSpan.FromSeconds(1));
  }

  // Test 13: Verify email success
  [Fact]
  public void VerifyEmail_WithValidToken_ShouldVerifyEmailAndClearToken()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);
    user.GenerateEmailVerificationToken();
    var token = user.VerificationToken;

    // Act
    user.VerifyEmail(token!);

    // Assert
    user.IsEmailVerified.Should().BeTrue();
    user.VerificationToken.Should().BeNull();
  }

  // Test 14: Verify email with invalid token throws UnauthorizedAccessException
  [Fact]
  public void VerifyEmail_WithInvalidToken_ShouldThrowUnauthorizedAccessException()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);
    user.GenerateEmailVerificationToken();

    // Act & Assert
    Assert.Throws<UnauthorizedAccessException>(() => user.VerifyEmail("InvalidToken"))
        .Message.Should().Contain("Invalid or expired token");
  }

  // Test 15: Generate reset password token
  [Fact]
  public void GenerateResetToken_ShouldSetResetPasswordTokenAndTime()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "Password@123", "John", "Doe", true);

    // Act
    user.GenerateResetToken();

    // Assert
    user.ResetPasswordToken.Should().NotBeNullOrEmpty();
    user.ResetTokenGeneratedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: TimeSpan.FromSeconds(1));
  }

  // Test 16: Domain event is raised on password change
  [Fact]
  public void ChangePassword_ShouldRaiseUserChangedPasswordEvent()
  {
    // Arrange
    var user = new User("validUser", "valid@example.com", "OldPassword@123", "John", "Doe", true);
    var newPassword = "NewPassword@123";

    // Act
    user.ChangePassword("OldPassword@123", newPassword);

    // Assert
    user.DomainEvents.Should().ContainSingle(e => e.GetType() == typeof(UserChangedPasswordEvent));
  }

  // Additional tests for recover account and further edge cases can be added similarly.
}


