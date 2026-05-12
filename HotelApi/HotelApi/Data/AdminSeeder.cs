using HotelApi.Models;
using HotelApi.Services;

namespace HotelApi.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Admins.Any()) return;

            context.Admins.AddRange(
     new Admin
     {
         Username = "Nouh",
         PasswordHash = PasswordHasher.Hash("181196"),
         FullName = "Nouh",
         Role = "SuperAdmin"
     },
     new Admin
     {
         Username = "sehla",
         PasswordHash = PasswordHasher.Hash("1042004"),
         FullName = "Sehla",
         Role = "Admin"
     },
     new Admin
     {
         Username = "esra",
         PasswordHash = PasswordHasher.Hash("1042004"),
         FullName = "Esra",
         Role = "Admin"
     },
     new Admin
     {
         Username = "shap",
         PasswordHash = PasswordHasher.Hash("1042004"),
         FullName = "Shap",
         Role = "Admin"
     }
 );

            await context.SaveChangesAsync();
        }
    }
}