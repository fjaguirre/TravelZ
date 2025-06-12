using TravelZ.Core.Types;

namespace TravelZ.Models;
public class MenuItem
{
    public string Text { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public List<RoleType> Roles { get; set; }
}