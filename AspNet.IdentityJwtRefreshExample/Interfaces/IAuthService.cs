using AspNet.IdentityJwtRefreshExample.Models;

namespace AspNet.IdentityJwtRefreshExample.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса аутентификации пользователей.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Метод регистрирует нового пользователя.
        /// </summary>
        /// <param name="registerRequest">Входная модель.</param>
        /// <returns>Email пользователя.</returns>
        Task<string> RegisterAsync(RegisterRequest registerRequest);

        /// <summary>
        /// Метод аутентифицирует пользователя.
        /// </summary>
        /// <param name="loginRequest">Входная модель.</param>
        /// <returns>Выходная модель.</returns>
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);

        /// <summary>
        /// Метод выходит из аккаунта пользователя.
        /// </summary>
        /// <param name="userEmail">Email пользователя.</param>
        Task LogoutAsync(string userEmail);
    }
}
