using HackerNewsASW.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsASW.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(DatabaseContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Login()
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action(nameof(LoginResult))
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> LoginResult()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            foreach (var id in result.Principal.Identities)
            {
                foreach (var claim in id.Claims) 
                {
                    _logger.LogInformation(claim.Type + " " + claim.Value);
                }
            }

            return Redirect("/");
        }
    }
}
