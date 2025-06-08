using Microsoft.AspNetCore.Identity;
using TravelZ.Core.Models;

namespace TravelZ.Api.Data;

public static class DatabaseSeeder
{
    public static async Task Seed(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { "administrator", "traveler", "owner", "housekeeper" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        var users = new[]
        {
            new { FirstName = "Jhon", LastName = "Doe", PhoneNumber = "5556031234", UserName = "admin", Password = "Admin123!", Role = "administrator" },
            new { FirstName = "Bruce", LastName = "Wayne", PhoneNumber = "5557349384", UserName = "traveler", Password = "Traveler123!", Role = "traveler" },
            new { FirstName = "David", LastName = "Brown", PhoneNumber = "5559320909", UserName = "owner", Password = "Owner123!", Role = "owner" },
            new { FirstName = "Alice", LastName = "Smith", PhoneNumber = "5551234567", UserName = "housekeeper", Password = "Housekeeper123!", Role = "housekeeper" }
        };

        foreach (var u in users)
        {
            var user = await userManager.FindByEmailAsync($"{u.UserName}@travelz.com");
            if (user == null)
            {
                user = new User { FirstName = u.FirstName, LastName = u.LastName, UserName = u.UserName, Email = $"{u.UserName}@travelz.com", EmailConfirmed = true, PhoneNumber = u.PhoneNumber };
                await userManager.CreateAsync(user, u.Password);
            }
            if (!await userManager.IsInRoleAsync(user, u.Role))
                await userManager.AddToRoleAsync(user, u.Role);
        }
    }
}