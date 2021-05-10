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
using Microsoft.Extensions.Logging;

namespace HackerNewsASW.Controllers
{
    public class ContributionsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<ContributionsController> _logger;

        public ContributionsController(DatabaseContext context, ILogger<ContributionsController> logger)
        {
            _context = context;
            _logger = logger;
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

            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user != null) ViewBag.votedList = user.Upvoted;

            return View(Tuple.Create(contribution,await GetComments(contribution)));
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
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contribution == null)
            {
                return NotFound();
            }

            comment.Commented = contribution;
            comment.DateCreated = DateTime.Now;
            comment.Author = await _context.Users.FindAsync(GetUserEmail(User));
            //comment.Reference = contribution.Id;

            if (contribution.Comments is null) contribution.Comments = new HashSet<Comment>();
            contribution.Comments.Add(comment);

            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();

            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user != null) ViewBag.votedList = user.Upvoted;

            return View(Tuple.Create(contribution, await GetComments(contribution)));
        }

        [Authorize]
        public async Task<IActionResult> Upvote(long? id)
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

            var user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user.Upvoted.FirstOrDefault(c => c.Id == contribution.Id) is null)
            {
                contribution.Upvotes++;
                if (user.Upvoted is null) user.Upvoted = new HashSet<Contribution>();
                user.Upvoted.Add(contribution);

                await _context.SaveChangesAsync();
            }

            return Redirect(Request.Query["return"].ToString());
        }










        /* 
         * [Authorize]
       public async Task<IActionResult> SubmissionsUpvoted()
        {
            string usermail=GetUserEmail(User);
             User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == usermail);
                
             return View(user.Contributions);
        }
        */

        [Authorize]
        public async Task<IActionResult> Unvote(long? id)
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

            var user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user.Upvoted.FirstOrDefault(c => c.Id == contribution.Id) != null)
            {
                contribution.Upvotes--;
                user.Upvoted.Remove(contribution);

                await _context.SaveChangesAsync();
            }

            return Redirect(Request.Query["return"].ToString());
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
        private async Task<HashSet<Tupla>> GetComments(Contribution c)
        {
            HashSet<Tupla> comments = new HashSet<Tupla>();

            foreach (var com in c.Comments) {
                var com2 = await _context.Comments.Include(c2 => c2.Author).Include(c2 => c2.Comments).FirstOrDefaultAsync(c2 => c2.Id == com.Id);
                //_logger.LogInformation(com2.Content + " " + com2.Author.UserId);
                Tupla t = new Tupla();
                t.Parent = com2;
                t.Children =  await GetComments(com2);
                comments.Add(t);
            }
            return comments;
        }




        public async Task<IActionResult> UpvoteApi(long id)
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

            var user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user.Upvoted.FirstOrDefault(c => c.Id == contribution.Id) != null)
            {
                contribution.Upvotes--;
                user.Upvoted.Remove(contribution);

                await _context.SaveChangesAsync();
            }

            return Redirect(Request.Query["return"].ToString());

        }

        public async Task<IActionResult> UnVoteApi(long id)
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

            var user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user.Upvoted.FirstOrDefault(c => c.Id == contribution.Id) != null)
            {
                contribution.Upvotes--;
                user.Upvoted.Remove(contribution);

                await _context.SaveChangesAsync();
            }

            return Redirect(Request.Query["return"].ToString());
        }



        [Route("api/[controller]/Contributions/Vote")]
        [HttpPost]
        //[Authorize]
        public async IActionResult VoteApi(long id, string email)
        {
            if (!ContributionExists(id))
            {
                return BadRequest();

            }
            var upvoters = await _context.Contributions.FindAsync(id);

            bool trobat = false;
            foreach (var u in upvoters.Upvoters)
            {
                if(u.Email == email)
                {
                    trobat = true;
                }
            }

            //Si ha votado
            if (trobat)
            {
                return UnVoteApi(id);
            }
            
            return UpvoteApi(id);
        }

          
    }
}
