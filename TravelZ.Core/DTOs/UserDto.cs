using TravelZ.Core.Types;

namespace TravelZ.Core.DTOs
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IList<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
}
