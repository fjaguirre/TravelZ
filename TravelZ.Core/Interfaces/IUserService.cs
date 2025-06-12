using TravelZ.Core.DTOs;
using TravelZ.Core.Requests;

namespace TravelZ.Core.Interfaces;

public interface IUserService
{
    Task<string?> Login(LoginRequest request);
    Task<UserDto?> GetCurrentUser();
    Task<IEnumerable<UserDto>> GetAllUsers();
}