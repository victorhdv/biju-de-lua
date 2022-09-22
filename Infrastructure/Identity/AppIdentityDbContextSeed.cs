using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Lua",
                    Email = "lua@test.com",
                    UserName = "lua@test.com",
                    Address = new Address
                    {
                        FirstName = "Lua",
                        LastName = "Lual",
                        Street = "Praia de Ponta Negra",
                        City = "Natal",
                        State = "RN",
                        CEP = "59000-600",
                    }
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}