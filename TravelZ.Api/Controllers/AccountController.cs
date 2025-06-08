using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelZ.Core.Requests;
using TravelZ.Core.Interfaces;


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
            return Unauthorized(new { Message = "Invalid credentials" });

        return Ok(new { Token = tokenString });
    }

    [HttpGet("users")]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

}