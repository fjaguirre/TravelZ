using TravelZ.Core.DTOs;
using TravelZ.Core.Requests;

namespace TravelZ.Core.Interfaces;

public interface IUserService
{
    Task<string?> Login(LoginRequest request);
    Task<UserDto?> GetCurrentUser();
    Task<UserDto?> UpdateCurrentUser(UserDto userDto);
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<UserDto?> GetUserById(string userId);
    Task<UserDto?> UpdateUser(string userId, UserDto userDto);
}