using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AspNet.IdentityJwtRefreshExample.Helpers
{
    /// <summary>
    /// Класс помщник для работы с токенами.
    /// </summary>
    public static class TokenHelper
    {
        /// <summary>
        /// Метод генерирует токен доступа.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="claims">Сведения пользователя.</param>
        /// <returns>Токен доступа.</returns>
        public static string GenerateAccessToken(this IConfiguration configuration, IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
            var tokenValidityInMinutes = configuration.GetSection("Jwt:TokenValidityInMinutes").Get<int>();
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: claims,
                signingCredentials: signinCredentials
                );

            string? tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        /// <summary>
        /// Метод генерирует токен обновления.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <returns>Токен обновления.</returns>
        public static string GenerateRefreshToken(this IConfiguration configuration)
        {
            var randomNumber = new byte[64];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Метод получает информацию о пользователе из просроченного токена доступа.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns>Сведения пользователя.</returns>
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(this IConfiguration configuration,
            string? accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Невалидный токен");
            }

            return principal;
        }
    }
}
