using AspNet.IdentityJwtRefreshExample.Identity;
using Microsoft.AspNetCore.Identity;

namespace AspNet.IdentityJwtRefreshExample.Data
{
    /// <summary>
    /// Класс инициализирует таблицу ролей начальными данными.
    /// </summary>
    public class SeedDataIdentityRole
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole<long>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<
                long>>>();
            
            string[] roles = ["User", "Admin"];

            foreach (var role in roles)
            {
                bool isRoleExist = await roleManager.RoleExistsAsync(role);

                if (!isRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole<long>
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }          
        }
    }
}