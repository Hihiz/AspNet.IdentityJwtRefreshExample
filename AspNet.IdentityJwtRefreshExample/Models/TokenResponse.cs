namespace AspNet.IdentityJwtRefreshExample.Models
{
    /// <summary>
    /// Класс выходной модели токенов пользователя.
    /// </summary>
    public class TokenResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
