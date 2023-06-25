namespace GptEngineer.API.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/account")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> logger;

    public AccountController(ILogger<AccountController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public ActionResult Login(string returnUrl)
    {
        this.logger.LogDebug("api/v1/account/login?returnUrl={ReturnUrl}", returnUrl);
        var challenge = Challenge(new AuthenticationProperties
        {
            RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
        });
        return challenge;
    }

    [ValidateAntiForgeryToken]
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout() => SignOut(new AuthenticationProperties
        {
            RedirectUri = "/"
        },
        CookieAuthenticationDefaults.AuthenticationScheme,
        OpenIdConnectDefaults.AuthenticationScheme);
}
