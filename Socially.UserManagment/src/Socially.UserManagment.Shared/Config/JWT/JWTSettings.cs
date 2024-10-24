namespace Socially.ContentManagment.Shared.Config.JWT;

  public class JWTSettings
  {
      public required string Secret { get; set; }          // Secret key used to sign the token
      public required int AccessTokenExpiryMinutes { get; set; }  // Expiry time for the access token
      public required int RefreshTokenExpiryDays { get; set; }    // Expiry time for the refresh token
      public required string Issuer { get; set; }          // Issuer of the token
      public required string Audience { get; set; }        // Audience for the token
  }
