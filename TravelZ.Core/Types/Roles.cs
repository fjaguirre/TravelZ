using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelZ.Core.Types
{
    public enum RoleType : byte
    {
        Unspecified = 0,
        Administrator = 1,
        Owner = 2,
        Traveler = 3,
        Housekeeper = 4
    }

    public class RoleName
    {
        public const string Administrator = "administrator";
        public const string Owner = "owner";
        public const string Traveler = "traveler";
        public const string Housekeeper = "housekeeper";

        private string Name;

        public RoleName(RoleType type)
        {
            switch (type)
            {
                case RoleType.Administrator:
                    Name = Administrator;
                    break;
                case RoleType.Owner:
                    Name = Owner;
                    break;
                case RoleType.Traveler:
                    Name = Traveler;
                    break;
                case RoleType.Housekeeper:
                    Name = Housekeeper;
                    break;
                default:
                    Name = string.Empty;
                    break;
            }
        }

        public static RoleType GetRoleByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return RoleType.Unspecified;

            switch (name)
            {
                case Administrator:
                    return RoleType.Administrator;
                case Owner:
                    return RoleType.Owner;
                case Traveler:
                    return RoleType.Traveler;
                case Housekeeper:
                    return RoleType.Housekeeper;
                default:
                    return RoleType.Unspecified;
            }
        }

        public static implicit operator string(RoleName roleName)
        {
            return roleName.Name;
        }
    }
}
