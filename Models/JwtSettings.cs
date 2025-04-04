using Microsoft.Extensions.Options;

namespace StockWatchAPI.Models
{
    public class JwtSettings
    {
        public string? SecretKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpirationMinutes { get; set; }
    }
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            // Ensure JwtSettings is not null
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings), "JwtSettings are not configured.");
        }

        public string GenerateToken()
        {
            // Safely access the Issuer
            string validIssuer = _jwtSettings.Issuer ?? "defaultIssuer";  // Provide a fallback if null

            // Use validIssuer to generate token...
            return validIssuer;
        }
    }
}
