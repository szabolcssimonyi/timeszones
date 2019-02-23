using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Domain.Seeders
{
    public class ApplicationSeeder : IDomainSeeder
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private const string AdminRoleName = "Admin";
        private const string AdminUserName = "AdminUser";
        private const string DefaultUserName = "DefaultUser";

        public ApplicationSeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await SeedRoles();
            await SeedUsers();
        }

        private async Task SeedRoles()
        {
            if (roleManager.Roles.Any(r =>
                string.Equals(r.Name, AdminRoleName, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole
            {
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper()
            });
        }

        private async Task SeedUsers()
        {
            if (await userManager.FindByNameAsync(AdminUserName) == null)
            {
                var user = new IdentityUser
                {
                    Email = $@"{AdminUserName}@localhost.com",
                    UserName = AdminUserName,
                    NormalizedUserName = AdminUserName.ToUpper(),
                    PhoneNumber = "+36343197785",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString("D"),

                };
                user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, "secretUser01!");
                IdentityResult result = await userManager.CreateAsync(user, "secretUser01!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AdminRoleName);
                    await userManager.AddClaimAsync(user, new Claim("Name", AdminUserName));
                }
            }
            if (await userManager.FindByNameAsync(DefaultUserName) == null)
            {
                var user = new IdentityUser
                {
                    Email = $@"{DefaultUserName}@localhost.com",
                    UserName = DefaultUserName,
                    NormalizedUserName = DefaultUserName.ToUpper(),
                    PhoneNumber = "+36343197785",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };
                user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, "secretUser01!");
                IdentityResult result = await userManager.CreateAsync(user, "secretUser01!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AdminRoleName);
                    await userManager.AddClaimAsync(user, new Claim("Name", DefaultUserName));
                }
            }
        }
    }
}
