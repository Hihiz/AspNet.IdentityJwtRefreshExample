using Microsoft.AspNetCore.Identity;

namespace AspNet.IdentityJwtRefreshExample.Identity
{
    /// <summary>
    /// Класс пользователя для авторизации.
    /// </summary>
    public class ApplicationUser : IdentityUser<long>
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Токен обновления, для получения нового AccessToken.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Время жизни токена обновления.
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
