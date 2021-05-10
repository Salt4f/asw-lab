﻿using HackerNewsASW.Data;
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
using System.Text;
using Newtonsoft.Json.Linq;

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
            if (author is null) return NotFound();
            return View(author);
       }

        public async Task<IActionResult> OtherProfile(string usermail)
        {
            User author = await _context.Users.FindAsync(usermail);
            if (author is null) return NotFound();
            return View(author);
        }

        

        [Route("api/[controller]/Account")]
        public async Task<string> otherProfileApi(string usermail)
        {
            var Author = await _context.Users.FindAsync(usermail);
            if (Author is null) return "";

            var item = new JObject();
            item.Add("Email", Author.Email);
            item.Add("Date", Author.DateCreated);
            item.Add("About", Author.About);

            

            /*
            var comments = await _context.Comments
            .Include(c => c.Comments)
            .Include(c => c.Commented)
            .Where( c => c.Commented != null) //respon un comentari
            .Where(c => c.Author.Email == usermail)
            .OrderByDescending(c => c.DateCreated)
            .ToListAsync<Comment>();
            
            var submissions = await _context.Contributions
            .Include(c => c.Comments)
            .Where(c => c.GetType() != typeof(Comment)) 
            .Where(c => c.Author.Email == usermail)
            .OrderByDescending(c => c.DateCreated)
            .ToListAsync<Comment>();
            */


            var comments = await _context.Comments
           .Include(c => c.Comments)
           .OrderByDescending(c => c.DateCreated)
           .ToListAsync<Comment>();


            var CommentsJson = new JArray();
            foreach (var c in comments)
            {
                var item2 = new JObject();
                item2.Add("Id", c.Id);
                item2.Add("DateCreated", c.DateCreated);
                item2.Add("Upvotes", c.Upvotes);
                item2.Add("Title", c.getTitle());
                item2.Add("Content", c.Content);
                CommentsJson.Add(item2);
            }

            var news = await _context.News
            .Include(c => c.Author)
            .Include(c => c.Comments)
            .Where(s => s.Author.Email == usermail)
            .ToListAsync<Contribution>();

            var asks = await _context.Asks
            .Include(c => c.Author)
            .Include(c => c.Comments)
            .Where(s => s.Author.Email == usermail)
            .ToListAsync<Contribution>();

            var submissions = news.Union(asks);
            submissions = submissions.OrderByDescending(c => c.DateCreated);

            /*var submissions = await _context.Contributions
            .Include(s => s.Comments)
            //.Where( )
            .Where(s => s.Author.Email == usermail)
            .ToListAsync<Contribution>();*/

            var SubmissionJson = new JArray();
            foreach (var s in submissions)
            {
                var item2 = new JObject();
                item2.Add("Id", s.Id);
                item2.Add("DateCreated", s.DateCreated);
                item2.Add("Upvotes", s.Upvotes);
                item2.Add("Title", s.getTitle());
                item2.Add("Content", s.Content);
                SubmissionJson.Add(item2);
            }

          





            item.Add("Submissions", SubmissionJson);
            item.Add("Comments",CommentsJson);

            

            var json = new JArray();

            json.Add(item);
            

            //return json;
            return json.ToString();


        }


        public async Task<IActionResult> CheckUser(string usermail)
        {
            User author = await _context.Users.FindAsync(GetUserEmail(User));
            if (author is null) return RedirectToAction(nameof(OtherProfile), new { usermail = usermail });
            string emailact = author.Email;
            if(usermail == emailact) {
                return View("Profile",author);
            }
            else {
                author = await _context.Users.FindAsync(usermail);
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
                var c2 = await _context.Contributions.Include(c3 => c3.Comments).Include(c3 => c3.Author).FirstOrDefaultAsync(c3 => c3.Id == c.Id);
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
            HashSet<Contribution> upvoted = new HashSet<Contribution>();
            foreach (var c in user.Upvoted) {
                var c2 = await _context.Contributions.Include(c3 => c3.Comments).Include(c3 => c3.Author).FirstOrDefaultAsync(c3 => c3.Id == c.Id);
                upvoted.Add(c2);
            }
            return View(upvoted);
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
                        DateCreated = DateTime.Now,
                        Token = GenerateToken()
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

        private string GenerateToken()
        {
            var hashfunc = new System.Security.Cryptography.SHA512Managed();
            Random rand = new Random((int) DateTime.Now.Ticks);
            byte[] buffer = new byte[20];
            rand.NextBytes(buffer);
            var hash = hashfunc.ComputeHash(buffer);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            //_logger.LogInformation(sb.ToString());

            return sb.ToString();
        }


    }
}
