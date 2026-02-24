using AspNet.IdentityJwtRefreshExample.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNet.IdentityJwtRefreshExample.Data
{
    /// <summary>
    /// Класс контекста Ef Core.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) { }
    }
}
