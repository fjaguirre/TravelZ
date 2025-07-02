using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelZ.Core.Requests;
using TravelZ.Core.Interfaces;
using TravelZ.Core.Responses;
using TravelZ.Core.DTOs;

namespace TravelZ.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var tokenString = await _userService.Login(request);
        if (tokenString == null)
            return Unauthorized(new LoginResponse { Message = "Invalid credentials" });

        return Ok(new LoginResponse { Token = tokenString });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userService.GetCurrentUser();
        if (user == null)
            return NotFound(new { Message = "User not found" });

        return Ok(user);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedUser = await _userService.UpdateCurrentUser(userDto);
        if (updatedUser == null)
            return BadRequest(new { Message = "Failed to update user" });

        return Ok(updatedUser);
    }

    [HttpGet("users/{userId}")]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null)
            return NotFound(new { Message = "User not found" });

        return Ok(user);
    }

    [HttpGet("users")]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpPut("users/{userId}")]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedUser = await _userService.UpdateUser(userId, userDto);
        if (updatedUser == null)
            return NotFound(new { Message = "User not found or update failed" });

        return Ok(updatedUser);
    }

}