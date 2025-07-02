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
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roles = new[] { "administrator", "traveler", "owner", "housekeeper" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        var users = new[]
        {
            new { UserName = "admin", Id="06df645c-56d0-4fbd-9fe5-7ab54bc58627", FirstName = "Jhon", LastName = "Doe", PhoneNumber = "5556031234", Password = "Admin123!", Role = "administrator" },
            new { UserName = "traveler", Id="e72d8c71-439f-42f2-9c92-52954d60ffcc", FirstName = "Bruce", LastName = "Wayne", PhoneNumber = "5557349384", Password = "Traveler123!", Role = "traveler" },
            new { UserName = "owner", Id="311ab89b-f917-4241-bfb2-7badf520b2a7", FirstName = "David", LastName = "Brown", PhoneNumber = "5559320909", Password = "Owner123!", Role = "owner" },
            new { UserName = "housekeeper", Id="0792957e-7ab5-481a-a41f-5020328f61a0", FirstName = "Alice", LastName = "Smith", PhoneNumber = "5551234567", Password = "Housekeeper123!", Role = "housekeeper" }
        };

        foreach (var u in users)
        {
            var user = await userManager.FindByEmailAsync($"{u.UserName}@travelz.com");
            if (user == null)
            {
                user = new User { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, UserName = u.UserName, Email = $"{u.UserName}@travelz.com", EmailConfirmed = true, PhoneNumber = u.PhoneNumber };
                await userManager.CreateAsync(user, u.Password);
            }
            if (!await userManager.IsInRoleAsync(user, u.Role))
                await userManager.AddToRoleAsync(user, u.Role);
        }

        var ownerUser = await userManager.FindByEmailAsync("owner@travelz.com");
        if (ownerUser != null)
        {
            var properties = dbContext.Properties.Where(p => p.Id == 1000 || p.Id == 1001).ToList();
            foreach (var property in properties)
            {
                property.OwnerId = ownerUser.Id;
            }
            await dbContext.SaveChangesAsync();
        }
    }
}