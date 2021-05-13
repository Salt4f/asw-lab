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
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HackerNewsASW.Controllers
{
    public class NewsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<NewsController> _logger;

        public NewsController(DatabaseContext context, ILogger<NewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Contribucions
        public async Task<IActionResult> Index()
        {
            return View(await GetIndexInfo());
        }

        [Route("api/contributions")]
        public async Task<string> IndexAPI()
        {
            var contrib = await GetIndexInfo();

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
            return json.ToString();
        }

        private async Task<IEnumerable<News>> GetIndexInfo()
        {
            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user != null) ViewBag.votedList = user.Upvoted;

            return(_context.News
            .Include(c => c.Author)
            .Include(c => c.Comments)
            .OrderByDescending(c => c.Upvotes));
        }

        private async Task<IEnumerable<Contribution>> GetIndexNews()
        {
            User user = await _context.Users
                .Include(u => u.Upvoted)
                .FirstOrDefaultAsync(u => u.Email == GetUserEmail(User));
            if (user != null) ViewBag.votedList = user.Upvoted;

            var news = await _context.News
            .Include(c => c.Author)
            .Include(c => c.Comments)
            .ToListAsync<Contribution>();

            var asks = await _context.Asks
            .Include(c => c.Author)
            .Include(c => c.Comments)
            .ToListAsync<Contribution>();

            var contrib = news.Union(asks);
            contrib = contrib.OrderByDescending(c => c.DateCreated);

            return contrib;

        }

        public async Task<IActionResult> New()
        {
            var contrib = await GetIndexNews();

            return View(contrib);
        }

        [Route("api/contributions/new")]
        public async Task<string> NewAPI()
        {
            var contrib = await GetIndexNews();

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

            /*var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(contrib.FirstOrDefault(), settings);*/
        }

        // GET: Contribucions/Submit
        [Authorize]
        public IActionResult Submit()
        {
            return View();
        }


        public async Task<Tuple<bool, long>> SubmitFunction(SubmitFormModel submit, User author)
        {
            if (submit.Title != null)
            {
                if (author is null) author = await _context.Users.FindAsync(GetUserEmail(User));

                if (submit.Url is null && submit.Text is null) //ASK
                {
                    Ask ask = new Ask
                    {
                        Title = submit.Title,
                        Author = author,
                        Content = "",
                        DateCreated = DateTime.Now
                    };

                    await _context.AddAsync(ask);

                    await _context.SaveChangesAsync();
                    //ask = await _context.Asks.FirstOrDefaultAsync(a => a.Title == submit.Title);
                    return new Tuple<bool, long>(true, ask.Id); //Habría que redireccionar a la inspección de la contribución
                }
                else if (submit.Url is null) //ASK
                {
                    Ask ask = new Ask
                    {
                        Title = submit.Title,
                        Author = author,
                        Content = submit.Text,
                        DateCreated = DateTime.Now
                    };

                    await _context.AddAsync(ask);

                    await _context.SaveChangesAsync();
                    //ask = await _context.Asks.FirstOrDefaultAsync(a => a.Title == submit.Title);
                    return new Tuple<bool, long>(true, ask.Id); //Habría que redireccionar a la inspección de la contribución
                }
                else if (submit.Url.Trim().StartsWith("http"))//URL
                {
                    News url = await _context.News.FirstOrDefaultAsync(n => n.Content == submit.Url);
                    if (url != null)
                    {
                        //Aquí habría que redireccionar a la función que se encargue de visualizar una noticia y sus comentarios
                        return new Tuple<bool, long>(false, url.Id);
                    }

                    if (submit.Text is null) //URL
                    {
                        News news = new News
                        {
                            Title = submit.Title,
                            Author = author,
                            Content = submit.Url,
                            DateCreated = DateTime.Now
                        };

                        await _context.AddAsync(news);

                        await _context.SaveChangesAsync();
                        //news = await _context.News.FirstOrDefaultAsync(a => a.Title == submit.Title);
                        return new Tuple<bool, long>(true, news.Id);  //Habría que redireccionar a la inspección de la contribución
                    }
                    else //URL + COMMENT
                    {
                        News news = new News
                        {
                            Title = submit.Title,
                            Author = author,
                            Content = submit.Url,
                            DateCreated = DateTime.Now,
                            Comments = new HashSet<Comment>()
                        };

                        Comment com = new Comment
                        {
                            Author = author,
                            Content = submit.Text,
                            DateCreated = DateTime.Now,
                            Commented = news
                        };

                        news.Comments.Add(com);

                        await _context.AddAsync(news);
                        await _context.AddAsync(com);

                        await _context.SaveChangesAsync();
                        //news = await _context.News.FirstOrDefaultAsync(a => a.Title == submit.Title);
                        return new Tuple<bool, long> (true, news.Id); //Habría que redireccionar a la inspección de la contribución
                    }
                }
            }

            return new Tuple<bool, long>(false, -1); 
        }
        
        
        // POST: Contribucions/Submit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Submit([Bind("Title, Url, Text")] SubmitFormModel submit)
        {
            var submission = await SubmitFunction(submit, null);
            if (submission.Item2 != -1)
                return RedirectToAction("Details", "Contributions", new { id = submission.Item2 });
            else return View(submit);
        }

        [Route("api/contributions")]
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> SubmitAPI(string title, string url, string text)
        {

            if (title is null) return BadRequest();

            var header = Request.Headers["X-API-KEY"];//.FirstOrDefault();
            if (!header.Any()) return StatusCode(401);

            User author = await _context.Users.FirstOrDefaultAsync(u => u.Token == header.FirstOrDefault());
            if (author is null) return StatusCode(401);

            var submit = new SubmitFormModel()
            {
                Title = title,
                Url = url,
                Text = text
            };
            var submission = await SubmitFunction(submit, author);
            return submission.Item1 ? Created("URI", "Deberíamos poner el objeto (o no)") : StatusCode(412);
        }

        // GET: Contribucions/Edit/5
        /*public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribucio = await _context.Contributions.FindAsync(id);
            if (contribucio == null)
            {
                return NotFound();
            }
            return View(contribucio);
        }*/

        // POST: Contribucions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Edit(long id, [Bind("Url,Points,Author,Date,Title,Id")] Contribution contribucio)
        {
            if (id != contribucio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contribucio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContribucioExists(contribucio.Id))
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
            return View(contribucio);
        }*/

        // GET: Contribucions/Delete/5
        /*public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribucio = await _context.Contributions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contribucio == null)
            {
                return NotFound();
            }

            return View(contribucio);
        }*/

        // POST: Contribucions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        /*public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var contribucio = await _context.Contributions.FindAsync(id);
            _context.Contributions.Remove(contribucio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private static string GetUserID(System.Security.Claims.ClaimsPrincipal user)
        {
            string email = user.Claims.Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                        .Select(claim => claim.Value).FirstOrDefault();

            if (email != null) return email.Split('@')[0];
            return "";
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
