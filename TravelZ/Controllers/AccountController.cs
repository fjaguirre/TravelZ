using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using TravelZ.Core.Requests;
using TravelZ.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using TravelZ.Core.Interfaces;
using TravelZ.Core.Responses;

namespace TravelZ.Controllers;

public class AccountController : BaseController
{
    public AccountController(ILogger<AccountController> logger, IApiClientService apiClientService) : base(logger, apiClientService)
    {
    }

    [HttpGet]
    [Route("account/login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        var token = HttpContext.Session.GetString("JWToken");
        if (!string.IsNullOrEmpty(token))
            return RedirectToAction("Profile", "Account");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _apiClientService.PostAsync<LoginResponse>("/account/login", model);
        if (response != null && !string.IsNullOrEmpty(response.Token))
        {
            _logger.LogInformation("Login successful. Token received.");
            HttpContext.Session.SetString("JWToken", response.Token);

            var user = await _apiClientService.GetAsync<UserDto>("/account/me");
            if (user != null)
            {
                HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(user));
            }

            return RedirectToAction("Index", "Home");
        }
        _logger.LogWarning("Login attempt failed: " + (response?.Message ?? "Unknown error"));
        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("account/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JWToken");
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [Route("account/profile")]
    public async Task<IActionResult> Profile()
    {
        var user = await _apiClientService.GetAsync<UserDto>("/account/me");
        if (user != null)
            return View(user);

        ModelState.AddModelError("", "Failed to retrieve user profile.");
        return View(null);
    }

    [HttpGet]
    [Route("account/users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _apiClientService.GetAsync<List<UserDto>>("/account/users");
        if (users != null)
            return View(users);

        ModelState.AddModelError("", "Failed to retrieve users.");
        return View(new List<UserDto>());

    }
}
