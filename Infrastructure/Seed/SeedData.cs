using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class SeedData(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
{
    public async Task<bool> SeedUser()
    {
        var existing = await userManager.FindByNameAsync("admin123");
        if (existing != null) return false;
        
        var user = new User()
        {
            UserName = "admin123",
            Email = "admin@gmail.com",
            Address = "Dushanbe",
            Age = 24,
            FirstName = "Test",
            PhoneNumber = "987654321",
            RegistrationDate = DateTime.UtcNow,
            IsActive = true,
        };

        var result = await userManager.CreateAsync(user, "Qwerty123!");
        if (!result.Succeeded) return false;
        
        await userManager.AddToRoleAsync(user, Roles.Admin);
        return true;
    }
    
    public async Task<bool> SeedRole()
    {
        var newRoles = new List<IdentityRole<int>>()
        {
            new (Roles.Admin),
            new (Roles.Manager),
            new (Roles.User),
            new (Roles.Student),
            new (Roles.Teacher),
        };
        
        var roles = await roleManager.Roles.ToListAsync();
        
        foreach (var role in newRoles)
        {
            if (roles.Any(e => e.Name == role.Name))
                continue;

            await roleManager.CreateAsync(role);
        }

        return true;
    }
}