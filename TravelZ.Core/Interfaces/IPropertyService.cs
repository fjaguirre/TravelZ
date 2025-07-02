using TravelZ.Core.DTOs;
using TravelZ.Core.Requests;

namespace TravelZ.Core.Interfaces;

public interface IPropertyService
{
    Task<IEnumerable<PropertyDto>> GetAllProperties();

    Task<PropertyDto?> GetPropertyById(int id);

    Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerId(string ownerId);
}