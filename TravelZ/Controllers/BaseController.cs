using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TravelZ.Core.Interfaces;

namespace TravelZ.Controllers;

public class BaseController : Controller
{
    protected readonly ILogger _logger;
    protected readonly IApiClientService _apiClientService;

    public BaseController(ILogger<BaseController> logger, IApiClientService apiClientService)
    {
        _logger = logger;
        _apiClientService = apiClientService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null;

        if (!allowAnonymous)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                context.Result = RedirectToAction("Login", "Account");
                return;
            }
        }

        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (HttpContext.Items.ContainsKey("ApiForbidden"))
        {
            context.Result = RedirectToAction("Forbidden", "Home");
        }
        else if (HttpContext.Items.ContainsKey("ApiUnauthorized"))
        {
            context.Result = RedirectToAction("Login", "Account");
        }

        base.OnActionExecuted(context);
    }
}
