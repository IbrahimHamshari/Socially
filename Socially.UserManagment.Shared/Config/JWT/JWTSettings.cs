using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.Shared.Config.JWT
{
    public class JWTSettings
    {
        public string Secret { get; set; }          // Secret key used to sign the token
        public int AccessTokenExpiryMinutes { get; set; }  // Expiry time for the access token
        public int RefreshTokenExpiryDays { get; set; }    // Expiry time for the refresh token
        public string Issuer { get; set; }          // Issuer of the token
        public string Audience { get; set; }        // Audience for the token
    }
}
