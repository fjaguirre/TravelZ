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
        ViewBag.IsProfile = true;
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

    [HttpGet]
    [Route("account/user/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _apiClientService.GetAsync<UserDto>($"/account/users/{id}");
        ViewBag.IsProfile = false;
        if (user != null)
            return View("Profile", user);

        ModelState.AddModelError("", "User not found.");
        return View("Profile", null);
    }

    [HttpGet]
    [Route("account/user/edit/{id?}")]
    public async Task<IActionResult> Edit(string? id)
    {
        string url = string.IsNullOrEmpty(id) ? "/account/me" : $"/account/users/{id}";
        UserDto? user = await _apiClientService.GetAsync<UserDto>(url);
        if (user == null)
            return NotFound();
        ViewBag.EditId = id;
        ViewBag.IsProfile = string.IsNullOrEmpty(id);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("account/user/edit/{id?}")]
    public async Task<IActionResult> Edit(UserDto model, string? id)
    {
        if (!ModelState.IsValid)
            return View(model);

        string url = string.IsNullOrEmpty(id) ? "/account/me" : $"/account/users/{id}";
        UserDto? result = await _apiClientService.PutAsync<UserDto>(url, model);

        if (result != null)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Profile");
            else
                return RedirectToAction("GetUserById", new { id = id });
        }

        ModelState.AddModelError("", "Failed to update user.");
        return View(model);
    }
}
