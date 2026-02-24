namespace AspNet.IdentityJwtRefreshExample.Models
{
    /// <summary>
    /// Класс входной модели аутентификации пользователя.
    /// </summary>
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
