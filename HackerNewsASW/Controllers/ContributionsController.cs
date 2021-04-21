using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackerNewsASW.Data;
using HackerNewsASW.Models;
using Microsoft.AspNetCore.Authorization;

namespace HackerNewsASW.Controllers
{
    public class ContributionsController : Controller
    {
        private readonly DatabaseContext _context;

        public ContributionsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Contributions
        public IActionResult Index()
        {
            return NotFound();
        }

        // GET: Contributions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id );
            
            if (contribution == null)
            {
                return NotFound();
            }
            return View(contribution);
        }

        // POST: Contributions/Details/5
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(long? id, [Bind("Content")] Comment comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contribution == null)
            {
                return NotFound();
            }

            comment.Commented = contribution;
            comment.DateCreated = DateTime.Now;
            comment.Author = await _context.Users.FindAsync(GetUserEmail(User));

            if (contribution.Comments is null) contribution.Comments = new HashSet<Comment>();
            contribution.Comments.Add(comment);

            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();

            return View(contribution);
        }

        private bool ContributionExists(long id)
        {
            return _context.Contributions.Any(e => e.Id == id);
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
