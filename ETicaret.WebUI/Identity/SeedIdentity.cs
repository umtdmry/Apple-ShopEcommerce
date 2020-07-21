using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Identity
{
    //Static olarak proje her çalıştığında eğer böyle bir kullanıcı yoksa oluşturulcak Admin verisi.
    public static class SeedIdentity
    {
        //Değerleri application.json dosyasından almaktadır.
        //Admin kullanıcıyı oluşturmaktadır.
        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var username = configuration["Data:AdminUser:username"];
            var email= configuration["Data:AdminUser:email"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];

            if (await userManager.FindByNameAsync(username)==null)
            {

                await roleManager.CreateAsync(new IdentityRole(role));

                var user = new ApplicationUser()
                {
                    UserName = username,
                    Email = email,
                    FullName = "Admin User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }

        }
    }
}
