namespace AspNet.IdentityJwtRefreshExample.Models
{
    /// <summary>
    /// Класс входной модели токена пользователя.
    /// </summary>
    public class TokenRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
