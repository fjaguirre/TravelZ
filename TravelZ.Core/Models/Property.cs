namespace TravelZ.Core.Models;

public class Property
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? OwnerId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int Beds { get; set; } = 1;
    public int Bathrooms { get; set; } = 1;
    public int TVs { get; set; } = 0;
    public int Pools { get; set; } = 0;
    public bool PetFriendly { get; set; } = false;
    public bool Wifi { get; set; } = false;
    public bool Parking { get; set; } = false;
    public bool AirConditioning { get; set; } = false;
    public User? Owner { get; set; }
}