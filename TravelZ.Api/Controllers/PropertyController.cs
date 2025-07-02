using Microsoft.AspNetCore.Mvc;
using TravelZ.Core.Interfaces;

namespace TravelZ.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpGet("properties")]
    public async Task<IActionResult> GetAllProperties()
    {
        var properties = await _propertyService.GetAllProperties();
        return Ok(properties);
    }

    [HttpGet("properties/{id}")]
    public async Task<IActionResult> GetPropertyById(int id)
    {
        var property = await _propertyService.GetPropertyById(id);
        if (property == null)
            return NotFound(new { Message = "Property not found" });

        return Ok(property);
    }

    [HttpGet("properties/owner/{ownerId}")]
    public async Task<IActionResult> GetPropertiesByOwnerId(string ownerId)
    {
        var properties = await _propertyService.GetPropertiesByOwnerId(ownerId);
        return Ok(properties);
    }

}