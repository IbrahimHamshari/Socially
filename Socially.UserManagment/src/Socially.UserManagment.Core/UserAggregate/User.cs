using System.Security.Cryptography;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Socially.UserManagement.Core.UserAggregate;

public class User : EntityBase<Guid>, IAggregateRoot
{
  public string Username { get; private set; }
  public string Email { get; private set; }
  public string PasswordHash { get; private set; }
  public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? LastLoginAt { get; private set; }
  public bool IsActive { get; private set; } = true;
  public string Bio { get; private set; } = string.Empty;
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string? ProfilePictureURL { get; private set; }
  public string? CoverPhotoURL { get; private set; }
  public DateTimeOffset? DateOfBirth { get; private set; }
  public bool Gender { get; private set; }
  public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

  // Constructor
  public User(string username, string email, string password, string firstName, string lastName)
  {
    Username = Guard.Against.InvalidUserNameFormat(username, nameof(username));
    Email = Guard.Against.InvalidEmailFormat(email, nameof(email));
    string pass = Guard.Against.InvalidPasswordFormat(password, nameof(password));
    PasswordHash = HashPassword(pass);
    FirstName = Guard.Against.InvalidNameFormat(firstName, nameof(firstName));
    LastName = Guard.Against.InvalidNameFormat(lastName, nameof(lastName));
  }

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
    if (!VerifyPassword(PasswordHash, currentPassword))
      throw new UnauthorizedAccessException("Current password is incorrect.");

    newPassword = Guard.Against.InvalidPasswordFormat(newPassword, nameof(newPassword));

    PasswordHash = HashPassword(newPassword);
    UpdatedAt = DateTime.UtcNow;


  }

  private string HashPassword(string password)
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
  private bool VerifyPassword(string hashedPasswordWithSalt, string providedPassword)
  {
    // Split the stored value into the salt and the hashed password
    var parts = hashedPasswordWithSalt.Split('.');
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
    if (!IsValidRecoveryToken(recoveryToken))
      throw new ArgumentException("Invalid recovery token.");

    
  }

  private bool IsValidRecoveryToken(string token)
  {
    // Implement token validation logic
    return true; // Placeholder for actual token validation
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
    if ( newDateOfBirth > DateTimeOffset.UtcNow)
      throw new ArgumentException("Date of birth cannot be in the future.");

    DateOfBirth = newDateOfBirth;
    UpdatedAt = DateTimeOffset.UtcNow;
  }

  public void UpdateGender(bool newGender)
  {
    Gender = newGender;
    UpdatedAt = DateTimeOffset.UtcNow;
  }
}
