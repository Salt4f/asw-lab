using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackerNewsASW.Data;
using HackerNewsASW.Models;
using Newtonsoft.Json.Linq;

namespace HackerNewsASW.Controllers
{
    public class AsksController : Controller
    {
        private readonly DatabaseContext _context;

        public AsksController(DatabaseContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<Contribution>> GetIndexAsk()
        {
            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user != null) ViewBag.votedList = user.Upvoted;

            var ask = _context.Asks
                .Include(c => c.Author)
                .Include(c => c.Comments)
                .OrderByDescending(c => c.Upvotes);
            return ask;
        }

        // GET: Asks
        
        public async Task<IActionResult> Index()
        {
           var ask = await GetIndexAsk();

            return View(ask);
        }

        [Route("api/asks")]
        public async Task<string> AskAPI()
        {
            var contrib = await GetIndexAsk();

            var json = new JArray();

            foreach (var c in contrib)
            {
                var item = new JObject();
                item.Add("Id", c.Id);
                item.Add("DateCreated", c.DateCreated);
                item.Add("Upvotes", c.Upvotes);
                item.Add("Comments", c.Comments.Count);
                item.Add("Title", c.getTitle());
                item.Add("Content", c.Content);

                var author = new JObject();
                author.Add("UserId", c.Author.UserId);
                author.Add("Email", c.Author.Email);

                item.Add("Author", author);

                json.Add(item);
            }

            //return json;
            return json.ToString();
        }
            // GET: Asks/Details/5
            public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ask = await _context.Asks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ask == null)
            {
                return NotFound();
            }

            return View(ask);
        }

        // GET: Asks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Asks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,DateCreated,Upvotes,Content,Id")] Ask ask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ask);
        }

        // GET: Asks/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ask = await _context.Asks.FindAsync(id);
            if (ask == null)
            {
                return NotFound();
            }
            return View(ask);
        }

        // POST: Asks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Title,DateCreated,Upvotes,Content,Id")] Ask ask)
        {
            if (id != ask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AskExists(ask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ask);
        }

        // GET: Asks/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ask = await _context.Asks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ask == null)
            {
                return NotFound();
            }

            return View(ask);
        }

        // POST: Asks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ask = await _context.Asks.FindAsync(id);
            _context.Asks.Remove(ask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AskExists(long id)
        {
            return _context.Asks.Any(e => e.Id == id);
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
