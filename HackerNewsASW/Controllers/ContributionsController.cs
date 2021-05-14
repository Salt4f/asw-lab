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
using Newtonsoft.Json.Linq;

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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contribution == null)
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user != null) ViewBag.votedList = user.Upvoted;

            return View(Tuple.Create(contribution, await GetComments(contribution)));
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

            foreach (var com in c.Comments)
            {
                var com2 = await _context.Comments.Include(c2 => c2.Author).Include(c2 => c2.Comments).FirstOrDefaultAsync(c2 => c2.Id == com.Id);
                //_logger.LogInformation(com2.Content + " " + com2.Author.UserId);
                Tupla t = new Tupla();
                t.Parent = com2;
                t.Children = await GetComments(com2);
                comments.Add(t);
            }
            return comments;
        }



        [Route("api/contributions/{id}/upvote")]
        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> UnvoteAPI(long id)
        {

            var contribution = await _context.Contributions
                .Include(c => c.Comments)
                .Include(c => c.Upvoters)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (contribution == null)
            {
                return NotFound();
            }

            var header = Request.Headers["X-API-KEY"];//.FirstOrDefault();
            if (!header.Any()) return StatusCode(401);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == header.FirstOrDefault());
            if (user is null) return StatusCode(401);

            bool result = contribution.Upvoters.Remove(user);
            if (result) contribution.Upvotes--;

            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();

            return result ? StatusCode(204) : StatusCode(412);

        }

        [Route("api/contributions/{id}/upvote")]
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> UpvoteAPI(long id)
        {

            var contribution = await _context.Contributions
                .Include(c => c.Comments)
                .Include(c => c.Upvoters)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contribution == null)
            {
                return NotFound();
            }

            var header = Request.Headers["X-API-KEY"];//.FirstOrDefault();
            if (!header.Any()) return StatusCode(401);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == header.FirstOrDefault());
            if (user is null) return StatusCode(401);

            bool result = contribution.Upvoters.Contains(user);
            if (!result)
            {
                contribution.Upvotes++;
                if (contribution.Upvoters is null) contribution.Upvoters = new HashSet<User>();
                contribution.Upvoters.Add(user);

                _context.Contributions.Update(contribution);
                await _context.SaveChangesAsync();
            }

            return !result ? Ok() : StatusCode(412);

        }

        [Route("api/contributions/{id}")]
        [HttpPost]
        public async Task<IActionResult> CommentAPI(long id, string comment)
        {

            var contribution = await _context.Contributions
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contribution == null)
            {
                return NotFound();
            }

            var header = Request.Headers["X-API-KEY"];//.FirstOrDefault();
            if (!header.Any()) return StatusCode(401);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == header.FirstOrDefault());
            if (user is null) return StatusCode(401);

            Comment com = new Comment
            {
                Commented = contribution,
                DateCreated = DateTime.Now,
                Content = comment,
                Author = user
            };

            if (contribution.Comments is null) contribution.Comments = new HashSet<Comment>();
            contribution.Comments.Add(com);

            await _context.AddAsync(com);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        [Route("api/contributions/{id}")]
        [HttpGet]
        public async Task<IActionResult> DetailsAPI(long id)
        {

            var contribution = await _context.Contributions
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contribution == null)
            {
                return NotFound();
            }

            //var json = new JArray();

            var item = new JObject();
            item.Add("Id", contribution.Id);
            item.Add("DateCreated", contribution.DateCreated);
            item.Add("Upvotes", contribution.Upvotes);
            item.Add("Title", contribution.getTitle());
            item.Add("Content", contribution.Content);

            var author = new JObject();
            author.Add("UserId", contribution.Author.UserId);
            author.Add("Email", contribution.Author.Email);

            var coms = new JArray();
            await FillJson(coms, contribution);

            item.Add("Comments", coms);

            return Ok(item.ToString());
        }

        private async Task FillJson(JArray json, Contribution c)
        {
            c = await _context.Contributions
                .Include(c => c.Comments)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c2 => c2.Id == c.Id);

            foreach (var c2 in c.Comments)
            {
                var item = new JObject();
                item.Add("Id", c2.Id);
                item.Add("DateCreated", c2.DateCreated);
                item.Add("Upvotes", c2.Upvotes);
                item.Add("Title", c2.getTitle());
                item.Add("Content", c2.Content);

                var author = new JObject();
                author.Add("UserId", c2.Author.UserId);
                author.Add("Email", c2.Author.Email);

                item.Add("Author", author);

                var coms = new JArray();

                await FillJson(coms, c2);
                item.Add("Comments", coms);

                json.Add(item);

            }
        }


    }
}
