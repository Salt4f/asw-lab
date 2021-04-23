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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        [HttpPost]
        [Authorize]
       public async Task<IActionResult> Profile(string About)
        {
            User author = await _context.Users.FindAsync(GetUserEmail(User));
            author.About=About;
            _context.Update(author);
             await _context.SaveChangesAsync();
            return View(author);
        }


       [Authorize]
       public async Task<IActionResult> Profile()
        {
            User author = await _context.Users.FindAsync(GetUserEmail(User));
            return View(author);
        }

        public async Task<IActionResult> OtherProfile(string usermail)
        {
            User author = await _context.Users.FindAsync(usermail);
            return View(author);
        }

        public async Task<IActionResult> CheckUser(string usermail)
        {
            User author= await _context.Users.FindAsync(GetUserEmail(User));
            string emailact=author.Email;
            if(usermail==emailact) {
                return View("Profile",author);
            }
            else {
                author= await _context.Users.FindAsync(usermail);
                return View("OtherProfile",author);
            }
        }

   [Authorize]
       public async Task<IActionResult> SubmissionsUpvoted()
        {
            string usermail=GetUserEmail(User);
             User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == usermail);
            HashSet<Contribution> upvoted = new HashSet<Contribution>();
        foreach (var c in user.Upvoted) {
        var c2 = await _context.Contributions.Include(c3 => c3.Comments).FirstOrDefaultAsync(c3 => c3.Id == c.Id);
        upvoted.Add(c2);
}

return View(upvoted);
        }

       [Authorize]
        public async Task<IActionResult> CommentsUpvoted()
        {
            string usermail=GetUserEmail(User);
             User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == usermail);
                return View(user.Upvoted);
        }
        

        public IActionResult Login()
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action(nameof(LoginResult))
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        public async Task<IActionResult> LoginResult()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

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
                        UserId = email.Split('@')[0],
                        Email = email,
                        DateCreated = DateTime.Now
                    };

                    await _context.AddAsync(user);
                    await _context.SaveChangesAsync();
                }

                return Redirect("/");
            }

            return RedirectToAction(nameof(Login));
            
        }

         private static string GetUserEmail(System.Security.Claims.ClaimsPrincipal user)
        {
            string email = user.Claims.Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                        .Select(claim => claim.Value).FirstOrDefault();

            if (email != null) return email;
            return "";
        }

        
    }
}
