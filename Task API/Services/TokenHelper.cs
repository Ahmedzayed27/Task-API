using System.IdentityModel.Tokens.Jwt;

namespace TaskManagment.Services
{
    public class TokenHelper
    {

        public static bool IsTokenExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Check if the token has expired
                var exp = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                if (exp == null)
                    return false;

                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;
                return expirationTime < DateTime.UtcNow;
            }
            catch (Exception)
            {
                return true;  
            }
        }
    }
}
