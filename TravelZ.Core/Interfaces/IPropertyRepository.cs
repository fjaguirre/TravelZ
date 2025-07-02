using TravelZ.Core.Models;

namespace TravelZ.Core.Interfaces;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllProperties();
    Task<Property?> GetPropertyById(int id);
    Task<IEnumerable<Property>> GetPropertiesByOwnerId(string ownerId);
}