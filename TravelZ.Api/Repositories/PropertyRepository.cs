using Microsoft.EntityFrameworkCore;
using TravelZ.Core.Interfaces;
using TravelZ.Core.Models;
using TravelZ.Api.Data;

namespace TravelZ.Api.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PropertyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Property>> GetAllProperties()
    {
        return await _dbContext.Properties
            .Include(p => p.Owner)
            .ToListAsync();
    }

    public async Task<Property?> GetPropertyById(int id)
    {
        return await _dbContext.Properties
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerId(string ownerId)
    {
        return await _dbContext.Properties
            .Include(p => p.Owner)
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }
}