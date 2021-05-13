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
using Newtonsoft.Json.Linq;

namespace HackerNewsASW.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SubmissionsController> _logger;

        public SubmissionsController(DatabaseContext context, ILogger<SubmissionsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<Contribution>> getUserSubmissions(string usermail)
        {
            var contributions = await _context.Contributions
                .Include(u => u.Comments)
                .Include(u => u.Author)
                .Where(u => u.Author.Email == usermail)
                .ToListAsync();
            return contributions;
        }

        public async Task<IActionResult> UserSubmissions(string usermail)
        {
            var contributions = await getUserSubmissions(usermail);

            return View(contributions);
        }
        
        [HttpGet]
        [Route("api/users/{usermail}/contributions")]
        public async Task<IActionResult> UserSubmissionsAPI(string usermail)
        {

            if (!await _context.Users.AnyAsync(u => u.Email == usermail)) return NotFound();

            var contributions = await getUserSubmissions(usermail);

            var json = new JArray();

            foreach (var c in contributions)
            {
                var item = new JObject();
                item.Add("Id", c.Id);
                item.Add("DateCreated", c.DateCreated);
                item.Add("Upvotes", c.Upvotes);
                item.Add("Title", c.getTitle());
                item.Add("Content", c.Content);

                
                json.Add(item);
            }
            return Ok(json.ToString());
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