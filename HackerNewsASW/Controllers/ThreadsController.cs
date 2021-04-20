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
    public class ThreadsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<ThreadsController> _logger;

        public ThreadsController(DatabaseContext context, ILogger<ThreadsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public IActionResult UserComments()
        {

            User author = _context.Users
                .Include(u => u.Contributions)
                .Single(u => u.Email == GetUserEmail(User));
            return View(author.Contributions);
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