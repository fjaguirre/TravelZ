using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TravelZ.Core.DTOs;
using TravelZ.Core.Interfaces;
using TravelZ.Core.Models;
using TravelZ.Core.Requests;
using TravelZ.Core.Security;
using TravelZ.Core.Types;

namespace TravelZ.Core.Services;

public class PropertyService : IPropertyService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly IPropertyRepository _propertyRepository;

    public PropertyService(UserManager<User> userManager, IMapper mapper, IConfiguration config, IUserService userService, IPropertyRepository propertyRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _config = config;
        _userService = userService;
        _propertyRepository = propertyRepository;
    }

    public async Task<IEnumerable<PropertyDto>> GetAllProperties()
    {
        var properties = await _propertyRepository.GetAllProperties();
        return await BuildDtoList(properties);
    }

    public async Task<PropertyDto?> GetPropertyById(int id)
    {
        var property = await _propertyRepository.GetPropertyById(id);
        if (property == null)
            return null;

        return await BuildDto(property);
    }

    public async Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerId(string ownerId)
    {
        var properties = await _propertyRepository.GetPropertiesByOwnerId(ownerId);
        return await BuildDtoList(properties);
    }

    private async Task<PropertyDto> BuildDto(Property property)
    {
        var ownerDto = property.OwnerId == null ? null : await _userService.GetUserById(property.OwnerId);
        var dto = _mapper.Map<PropertyDto>(property);
        dto.Owner = ownerDto;
        return dto;
    }

    private async Task<IEnumerable<PropertyDto>> BuildDtoList(IEnumerable<Property> properties)
    {
        var propertyDtos = new List<PropertyDto>();
        foreach (var property in properties)
        {
            var dto = await BuildDto(property);
            propertyDtos.Add(dto);
        }
        return propertyDtos;
    }

}