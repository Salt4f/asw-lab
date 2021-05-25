using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackerNewsASW.Data;
using HackerNewsASW.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HackerNewsASW.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<ThreadsController> _logger;

        public ThreadsController(DatabaseContext context, ILogger<ThreadsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<IActionResult> UserComments(string usermail)
        {
            User author = await _context.Users
                .Include(u => u.Contributions)
                .FirstOrDefaultAsync(u => u.Email == usermail);

            if (author is null) return NotFound();

            var contrib = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .Include(c => c.Commented)
                .Where(c => c.Author.Email == author.Email)
                .ToListAsync();

            return View(contrib);
        }

        [Authorize]
        public async Task<IActionResult> UserThreads()
        {
            User author = await _context.Users
                .Include(u => u.Contributions)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));

            var contrib = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .Include(c => c.Commented)
                .Where(c => c.Author.Email == author.Email)
                .ToListAsync();

            return View("UserComments", contrib);
        }

        private static string GetUserEmail(System.Security.Claims.ClaimsPrincipal user)
        {
            string email = user.Claims.Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                        .Select(claim => claim.Value).FirstOrDefault();

            if (email != null) return email;
            return "";
        }

        private async Task<IEnumerable<Comment>> GetThreadsNews(string usermail)
        {
            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == usermail);
            if (user != null) ViewBag.votedList = user.Upvoted;

            var comments = await _context.Comments
            .Include(c => c.Author)
            .Include(c => c.Comments)
            .Where(c => c.Author == user)
            .OrderByDescending(c => c.DateCreated)
            .ToListAsync<Comment>();

            return comments;

        }

        [HttpGet]
        [Route("api/users/{usermail}/threads")]
        public async Task<IActionResult> ThreadAPI(string usermail)
        {
            User user = await _context.Users.FindAsync(usermail);

            if (user is null) return NotFound(); //404
            
            var header = Request.Headers["X-API-KEY"];//.FirstOrDefault();
            if (!header.Any() || header.FirstOrDefault() != user.Token) return StatusCode(401);


            var comments = await GetThreadsNews(usermail);


            var json = new JArray();

            foreach (var c in comments)
            {
                var item = new JObject();
                item.Add("Id", c.Id);
                item.Add("DateCreated", c.DateCreated);
                item.Add("Upvotes", c.Upvotes);
                item.Add("Title", c.getTitle());
                item.Add("Content", c.Content);

                var author = new JObject();
                author.Add("UserId", c.Author.UserId);
                author.Add("Email", c.Author.Email);

                if (user != null) 
                {
                    item.Add("UpvotedByUser", user.Upvoted.Contains(c));
                }

                var coms = new JArray();
                await FillJson(coms, c);

                item.Add("Comments", coms);

                json.Add(item);
            }

            return Ok(json.ToString());
        }

        private async Task FillJson(JArray json, Contribution c)
        {
            c = await _context.Contributions
                .Include(c => c.Comments)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c2 => c2.Id == c.Id);

            foreach (var c2 in c.Comments)
            {
                var c3 = await _context.Contributions
                .Include(c => c.Comments)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == c2.Id);

                var item = new JObject();
                item.Add("Id", c3.Id);
                item.Add("DateCreated", c3.DateCreated);
                item.Add("Upvotes", c3.Upvotes);
                item.Add("Title", c3.getTitle());
                item.Add("Content", c3.Content);

                var author = new JObject();
                author.Add("UserId", c3.Author.UserId);
                author.Add("Email", c3.Author.Email);

                item.Add("Author", author);

                if (user != null) 
                {
                    item.Add("UpvotedByUser", user.Upvoted.Contains(c));
                }

                var coms = new JArray();
                await FillJson(coms, c3);

                item.Add("Comments", coms);

                json.Add(item);

            }
        }
    }
}
