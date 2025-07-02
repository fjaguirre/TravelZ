using Microsoft.AspNetCore.Identity;

namespace TravelZ.Core.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}