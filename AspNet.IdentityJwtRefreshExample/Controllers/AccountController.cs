using AspNet.IdentityJwtRefreshExample.Interfaces;
using AspNet.IdentityJwtRefreshExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.IdentityJwtRefreshExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AccountController(IAuthService authService,
            ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Метод регистрирует нового пользователя.
        /// </summary>
        /// <param name="request">Входная модель.</param>
        /// <returns>Email пользователя.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            string result = await _authService.RegisterAsync(request);

            return Ok(result);
        }

        /// <summary>
        /// Метод аутентифицирует пользователя.
        /// </summary>
        /// <param name="request">Входная модель.</param>
        /// <returns>Выходная модель.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            TokenResponse result = await _authService.LoginAsync(request);

            return Ok(result);
        }

        /// <summary>
        /// Метод выходит из аккаунта пользователя.
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task LogoutAsync()
        {
            string? userEmail = User.Identity?.Name;

            await _authService.LogoutAsync(userEmail!);
        }

        /// <summary>
        /// Метод получает claims текущего аутентифицированного пользователя.
        /// </summary>
        /// <returns>Claims аутентифицированного пользователя.</returns>
        [Authorize]
        [HttpGet("auth-user")]
        public IActionResult CurrentAuthUser()
        {
           var result = User.Claims.Select(c => new { c.Type, c.Value });

            return Ok(result);
        }

        /// <summary>
        /// Метод обновляет токены пользователя (accessToken и refreshToken).
        /// </summary>
        /// <param name="request">Входная модель.</param>
        /// <returns>Выходная модель.</returns>
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequest request)
        {
            TokenResponse result = await _tokenService.RefreshTokenAsync(request);

            return Ok(result);
        }

        /// <summary>
        /// Метод сбрасывает refreshToken всем пользователям.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("revoke-users")]
        public async Task RevokeUsersAllAsync()
        {
            await _tokenService.RevokeUsersAllAsync();
        }
    }
}
