using AspNet.IdentityJwtRefreshExample.Helpers;
using AspNet.IdentityJwtRefreshExample.Identity;
using AspNet.IdentityJwtRefreshExample.Interfaces;
using AspNet.IdentityJwtRefreshExample.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNet.IdentityJwtRefreshExample.Services
{
    /// <summary>
    /// Класс реализует методы сервиса токена пользователя.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<TokenResponse> RefreshTokenAsync(TokenRequest tokenRequest)
        {           
            ClaimsPrincipal claimsPrincipal = _configuration.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);

            if (claimsPrincipal is null)
            {
                throw new InvalidOperationException("Недопустимый токен доступа или токен обновления.");
            }

            string? userName = claimsPrincipal.Identity?.Name;

            // Получаем пользователя по userName.
            ApplicationUser? user = await _userManager.FindByEmailAsync(userName!);

            if (user is null || user.RefreshToken != tokenRequest.RefreshToken 
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidOperationException($"Недопустимый токен доступа или токен обновления.");
            }

            // Получаем новый токен доступа.
            string newAccessToken = _configuration.GenerateAccessToken(claimsPrincipal.Claims.ToList());

            // Получаем новый токен обновления.
            string newRefreshToken = _configuration.GenerateRefreshToken();

            // Обновляем токен обновления пользователю.
            user.RefreshToken = newRefreshToken;

            // Обновляем время жизни токена обновления.
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_configuration.GetSection(
                "Jwt:RefreshTokenValidityInMinutes").Get<int>());

            await _userManager.UpdateAsync(user);

            TokenResponse result = new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return result;
        }

        public async Task RevokeUserByEmailAsync(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new InvalidOperationException($"Недопустимый email пользователя. UserEmail: {userEmail}.");
            }

            ApplicationUser? user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null)
            {
                throw new InvalidOperationException($"Ошибка получения пользователя. UserEmail: {userEmail}.");
            }
            // Обнуляем токен обновления.
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            await _userManager.UpdateAsync(user);
        }

        public async Task RevokeUsersAllAsync()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();

            // Всем пользователям обнуляем токен обновления.
            foreach (var user in users)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.MinValue;

                await _userManager.UpdateAsync(user);
            }
        }
    }
}
