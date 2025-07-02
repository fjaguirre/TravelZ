using TravelZ.Core.Types;

namespace TravelZ.Core.DTOs
{
    public class RoleDto
    {
        public required RoleType Type { get; set; }
        public required string Name { get; set; }
    }
}