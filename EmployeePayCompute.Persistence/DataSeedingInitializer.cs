using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EmployeePayCompute.Persistence
{
    public static class DataSeedingInitializer
    {
        public static async Task UserAndRoleSeedAsync(UserManager<IdentityUser> userManager, 
                                                RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Manager", "Staff" };
            foreach(var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if(!roleExist)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //Create Admin User.

            if(userManager.FindByEmailAsync("admin@as3globalsolution.com").Result==null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                if(identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            //Create Manager User.

            if (userManager.FindByEmailAsync("manager@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            //Create Staff User.

            if (userManager.FindByEmailAsync("staff@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "staff@gmail.com",
                    Email = "staff@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Staff").Wait();
                }
            }

            //Create NO Role User.

            if (userManager.FindByEmailAsync("abhisheksrivastava5502@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "abhisheksrivastava5502@gmail.com",
                    Email = "abhisheksrivastava5502@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Password1").Result;
                //No Role assigned to Mr. Abhishek
            }
        }
    }
}
