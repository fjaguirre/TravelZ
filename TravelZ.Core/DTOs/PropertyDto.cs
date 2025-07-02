using TravelZ.Core.Types;

namespace TravelZ.Core.DTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string Country { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public int TVs { get; set; }
        public int Pools { get; set; }
        public bool PetFriendly { get; set; }
        public bool Wifi { get; set; }
        public bool Parking { get; set; }
        public bool AirConditioning { get; set; }
        public UserDto? Owner { get; set; }
    }
}
