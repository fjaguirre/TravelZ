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

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _config = config;
    }

    public async Task<string?> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        return JwtTokenGenerator.Generate(user, roles, _config);
    }

    public async Task<UserDto?> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        if (user == null)
            return null;

        return await BuildDto(user);
    }

    public async Task<UserDto?> UpdateCurrentUser(UserDto userDto)
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        if (user == null)
            return null;

        return await UpdateUserInternal(user, userDto);
    }

    public async Task<UserDto?> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        return await BuildDto(user);
    }

    public async Task<UserDto?> UpdateUser(string userId, UserDto userDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        return await UpdateUserInternal(user, userDto);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = await BuildDto(user);
            userDtos.Add(dto);
        }

        return userDtos;
    }

    private async Task<UserDto?> UpdateUserInternal(User user, UserDto userDto)
    {
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.PhoneNumber = userDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return null;

        return await BuildDto(user);
    }

    private async Task<UserDto> BuildDto(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var dto = _mapper.Map<UserDto>(user);
        dto.Roles = roles.Select(r => new RoleDto { Name = r, Type = RoleName.GetTypeByName(r) }).ToList();
        return dto;
    }
}