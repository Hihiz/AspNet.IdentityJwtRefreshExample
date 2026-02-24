using AspNet.IdentityJwtRefreshExample.Helpers;
using AspNet.IdentityJwtRefreshExample.Identity;
using AspNet.IdentityJwtRefreshExample.Interfaces;
using AspNet.IdentityJwtRefreshExample.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNet.IdentityJwtRefreshExample.Services
{
    /// <summary>
    /// Класс реализует сервис аутентификации пользователей.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterAsync(RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.PasswordConfirm)
            {
                throw new InvalidOperationException("Пароли не совпадают.");
            }

            ApplicationUser? findUser = await _userManager.FindByEmailAsync(registerRequest.Email!);

            if (findUser is not null)
            {
                throw new InvalidOperationException(
                    $"Пользователь с Email: {registerRequest.Email} уже зарегистрирован в системе.");
            }

            ApplicationUser user = new ApplicationUser
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            // Создаем пользователя.
            IdentityResult createUser = await _userManager.CreateAsync(user, registerRequest.Password!);

            if (!createUser.Succeeded)
            {
                throw new InvalidOperationException($"Ошибка при создании пользователя {registerRequest.Email}.");
            }

            // Добавляем роль новому пользователю.           
            await _userManager.AddToRoleAsync(user, "User");

            return user.Email!;
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginRequest.Email!);

            if (user is null)
            {
                throw new InvalidOperationException(
                    $"Пользователь с почтой {loginRequest.Email} не существует в системе.");
            }

            // Проверяем пароль пользователя и входим в систему.
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password!);

            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Не удалось выполнить вход. " +
                    "Проверьте корректность учётных данных.");
            }

            // Получаем роли пользователя.
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Добавляем роли пользователя в claims.
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Создаем токен доступа.
            string accessToken = _configuration.GenerateAccessToken(authClaims);

            // Создаем токен обновления для пользователя.
            user.RefreshToken = _configuration.GenerateRefreshToken();

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_configuration.GetSection(
                "Jwt:RefreshTokenValidityInMinutes").Get<int>());

            await _userManager.UpdateAsync(user);

            TokenResponse result = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };

            return result;
        }

        public async Task LogoutAsync(string userEmail)
        {
            await _tokenService.RevokeUserByEmailAsync(userEmail);
        }
    }
}
