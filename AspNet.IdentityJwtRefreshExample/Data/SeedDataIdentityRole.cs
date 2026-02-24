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
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<
                ApplicationUser>>();

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

            // Создание администратора по умолчанию.
            string adminEmail = "admin@example.com";

            ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser is null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = "AdminFirstName",
                    LastName = "AdminLastName",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                string? adminPassword = "Admin123!";

                await userManager.CreateAsync(user, adminPassword);

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}