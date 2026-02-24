using AspNet.IdentityJwtRefreshExample.Models;

namespace AspNet.IdentityJwtRefreshExample.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса токенов пользователя.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Метод обновляет токены пользователя (accessToken и refreshToken).
        /// </summary>
        /// <param name="tokenRequest">Входная модель.</param>
        /// <returns>Выходная модель.</returns>
        Task<TokenResponse> RefreshTokenAsync(TokenRequest tokenRequest);

        /// <summary>
        /// Метод сбрасывает refreshToken пользователя по Email.
        /// </summary>
        /// <param name="userEmail">Email пользователя.</param>
        Task RevokeUserByEmailAsync(string userEmail);

        /// <summary>
        /// Метод сбрасывает refreshToken всем пользователям.
        /// </summary>
        Task RevokeUsersAllAsync();
    }
}
