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


        
        public async Task<IActionResult> UserSubmissions(string usermail)
        {
           var contributions = await _context.Contributions
                   .Include(u => u.Comments)
                   .Include(u => u.Author)
                   .Where(u => u.Author.Email == usermail)
                   .ToListAsync();

            return View(contributions);
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