using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelZ.Core.Requests;
using TravelZ.Core.Interfaces;
using TravelZ.Core.Responses;


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

    [HttpGet("users")]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

}