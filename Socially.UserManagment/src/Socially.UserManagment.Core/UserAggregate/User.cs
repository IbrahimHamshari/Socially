using System.Security.Cryptography;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Socially.UserManagment.Core.UserAggregate.Events;
using Socially.UserManagment.UseCases.Users.ForgetPassword;

namespace Socially.UserManagment.Core.UserAggregate;

public class User : EntityBase<Guid>, IAggregateRoot
{
  public string Username { get; private set; } = string.Empty;
  public string Email { get; private set; } = string.Empty;
  public string PasswordHash { get; private set; } = string.Empty;
  public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? LastLoginAt { get; private set; }
  public bool IsActive { get; private set; } = true;
  public string Bio { get; private set; } = string.Empty;
  public string FirstName { get; private set; } = string.Empty;
  public string LastName { get; private set; } = string.Empty;
  public string? ProfilePictureURL { get; private set; }
  public string? CoverPhotoURL { get; private set; }
  public DateTimeOffset? DateOfBirth { get; private set; }
  public bool Gender { get; private set; } = false;
  public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

  public string? VerificationToken { get; private set; }
  public bool IsEmailVerified { get; private set; } = false;
  public DateTimeOffset? TokenGeneratedAt { get; private set; }

  public string? ResetPasswordToken { get; private set; }

  public DateTimeOffset? ResetTokenGeneratedAt { get; private set; }

  public User(string username, string email, string passwordHash, string firstName, string lastName, bool gender)
  {
    Username = Guard.Against.InvalidUserNameFormat(username, nameof(username));
    Email = Guard.Against.InvalidEmailFormat(email, nameof(email));
    PasswordHash = passwordHash.Length > 24 ? passwordHash : HashPassword(Guard.Against.InvalidPasswordFormat(passwordHash, nameof(passwordHash)));
    FirstName = Guard.Against.InvalidNameFormat(firstName, nameof(firstName));
    LastName = Guard.Against.InvalidNameFormat(lastName, nameof(lastName));
    Gender = gender;
  }

  private User() { }
  // Activate and Deactivate methods
  public void ActivateLogin()
  {
    if (IsActive)
      throw new InvalidOperationException("Account is already active.");
    IsActive = true;
  }

  public void DeactivateAccount()
  {
    if (!IsActive)
      throw new InvalidOperationException("Account is already inactive.");
    IsActive = false;
  }

  // Change Password method
  public void ChangePassword(string currentPassword, string newPassword)
  {
    if (!VerifyPassword(currentPassword))
      throw new UnauthorizedAccessException("Current password is incorrect.");

    newPassword = Guard.Against.InvalidPasswordFormat(newPassword, nameof(newPassword));

    PasswordHash = HashPassword(newPassword);
    UpdatedAt = DateTime.UtcNow;

    var userChangedPasswordEvent = new UserChangedPasswordEvent(this);
    RegisterDomainEvent(userChangedPasswordEvent);
  }

  private static string HashPassword(string password)
  {
    // Generate a 128-bit salt using a secure PRNG
    byte[] salt = new byte[16];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }

    // Derive a 256-bit subkey (32 bytes) using PBKDF2 with HMACSHA256, 100,000 iterations
    byte[] hash = KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 32
    );

    // Combine salt and hash into a single string for storage
    string saltBase64 = Convert.ToBase64String(salt);
    string hashBase64 = Convert.ToBase64String(hash);
    return $"{saltBase64}.{hashBase64}";
  }

  // Verifies the provided password against the stored hashed password
  public bool VerifyPassword(string providedPassword)
  {
    // Split the stored value into the salt and the hashed password
    var parts = PasswordHash.Split('.');
    if (parts.Length != 2)
    {
      throw new FormatException("Unexpected hashed password format.");
    }

    // Extract the salt and hashed password from the stored value
    byte[] salt = Convert.FromBase64String(parts[0]);
    byte[] storedHash = Convert.FromBase64String(parts[1]);

    // Hash the provided password with the same salt
    byte[] providedHash = KeyDerivation.Pbkdf2(
        password: providedPassword,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 32
    );

    // Compare the hashes in a constant-time manner to prevent timing attacks
    return CryptographicOperations.FixedTimeEquals(providedHash, storedHash);
  }

  // Update Email with format validation
  public void UpdateEmail(string newEmail)
  {
    if (string.IsNullOrWhiteSpace(newEmail))
      throw new ArgumentException("Email cannot be empty.");

    newEmail = Guard.Against.InvalidEmailFormat(newEmail, nameof(newEmail));
    Email = newEmail;
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  // Record Login
  public void RecordLogin()
  {
    LastLoginAt = DateTimeOffset.UtcNow;
  }

  // Recover Account
  public void RecoverAccount(string recoveryToken, string newPassword)
  {
    if (!IsValidRecoveryToken(recoveryToken) || ResetTokenGeneratedAt == null || DateTimeOffset.UtcNow > ResetTokenGeneratedAt.Value.AddHours(3))
      throw new ArgumentException("Invalid recovery token.");

    UpdatePassword(newPassword);

    var recoverAccountEvent = new AccountRecoveredEvent(this);
    ResetPasswordToken = null;
  }

  private bool IsValidRecoveryToken(string token)
  {
    if (this.ResetPasswordToken != token)
    {
      return false;
    }
    return true;
  }

  private void UpdatePassword(string newPassowrd)
  {
    PasswordHash = HashPassword(Guard.Against.InvalidPasswordFormat(newPassowrd, nameof(newPassowrd)));
  }
  // Update methods for each property

  public void UpdateUsername(string newUsername)
  {
    Username = Guard.Against.InvalidFormat(newUsername, nameof(newUsername), "^[a-zA-Z0-9\u0600-\u06FF]{2,16}$");
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateFirstName(string newFirstName)
  {
    FirstName = Guard.Against.InvalidFormat(newFirstName, nameof(newFirstName), "^[a-zA-Z0-9\\u0600-\\u06FF]{2,16}$");
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateLastName(string newLastName)
  {
    LastName = Guard.Against.InvalidFormat(newLastName, nameof(newLastName), "^[a-zA-Z0-9\\u0600-\\u06FF]{2,16}$");
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateBio(string newBio)
  {
    Bio = newBio;
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateProfilePictureURL(string newProfilePictureURL)
  {
    ProfilePictureURL = newProfilePictureURL;
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateCoverPhotoURL(string newCoverPhotoURL)
  {
    CoverPhotoURL = newCoverPhotoURL;
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateDateOfBirth(DateTimeOffset newDateOfBirth)
  {
    if (newDateOfBirth > DateTimeOffset.UtcNow)
      throw new ArgumentException("Date of birth cannot be in the future.");

    DateOfBirth = newDateOfBirth;
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateGender(bool newGender)
  {
    Gender = newGender;
    UpdatedAt = DateTimeOffset.UtcNow;
  }
  public void GenerateEmailVerificationToken()
  {
    VerificationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).Replace("/", "");
    TokenGeneratedAt = DateTimeOffset.UtcNow;
    var userRegisteredEvent = new UserCreatedEvent(this);
    RegisterDomainEvent(userRegisteredEvent);
  }
  public void VerifyEmail(string token)
  {
    if (VerificationToken != token || TokenGeneratedAt == null || DateTimeOffset.UtcNow > TokenGeneratedAt.Value.AddHours(3))
    {
      throw new UnauthorizedAccessException("Invalid or expired token.");
    }

    IsEmailVerified = true;
    VerificationToken = null;
    var userVerifiedEvent = new UserVerifiedEvent(this);
    RegisterDomainEvent(userVerifiedEvent);
  }
  public void GenerateResetToken()
  {
    ResetPasswordToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).Replace("/", "");
    ResetTokenGeneratedAt = DateTimeOffset.UtcNow;

    var userForgotEvent = new PasswordForgotEvent(this);
    RegisterDomainEvent(userForgotEvent);
  }
}
