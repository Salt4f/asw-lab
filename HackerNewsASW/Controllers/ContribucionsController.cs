using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackerNewsASW.Data;
using HackerNewsASW.Models;

namespace HackerNewsASW.Controllers
{
    public class ContribucionsController : Controller
    {
        private readonly DatabaseContext _context;

        public ContribucionsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Contribucions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contribucions.ToListAsync());
        }

        // GET: Contribucions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribucio = await _context.Contribucions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contribucio == null)
            {
                return NotFound();
            }

            return View(contribucio);
        }

        // GET: Contribucions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contribucions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Url,Points,Author,Date,Title,Id")] Contribucio contribucio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contribucio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contribucio);
        }

        // GET: Contribucions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribucio = await _context.Contribucions.FindAsync(id);
            if (contribucio == null)
            {
                return NotFound();
            }
            return View(contribucio);
        }

        // POST: Contribucions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Url,Points,Author,Date,Title,Id")] Contribucio contribucio)
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
        }

        // GET: Contribucions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribucio = await _context.Contribucions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contribucio == null)
            {
                return NotFound();
            }

            return View(contribucio);
        }

        // POST: Contribucions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var contribucio = await _context.Contribucions.FindAsync(id);
            _context.Contribucions.Remove(contribucio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContribucioExists(long id)
        {
            return _context.Contribucions.Any(e => e.Id == id);
        }
    }
}
