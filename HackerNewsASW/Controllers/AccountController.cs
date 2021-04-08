using HackerNewsASW.Data;
using HackerNewsASW.Models;
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

            if (result.Succeeded)
            {
                string email = result.Principal.Identities.First().Claims
                    .Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                    .Select(claim => claim.Value).FirstOrDefault();

                User user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user is null)
                {
                    user = new User
                    {
                        Email = email,
                        DateCreated = DateTime.Now
                    };

                    await _context.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
            }


            return Redirect("/");
        }
    }
}
