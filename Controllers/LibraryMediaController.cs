using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibWeb;

namespace GameLibWeb.Controllers
{
    public class LibraryMediaController : Controller
    {
        private readonly dbgamelibContext _context;

        public LibraryMediaController(dbgamelibContext context)
        {
            _context = context;
        }

        // GET: LibraryMedia
        public async Task<IActionResult> Index()
        {
            var dbgamelibContext = _context.Librarymedia;
            return _context != null ? 
                View(await dbgamelibContext.ToListAsync()) :
                Problem("Entity set 'dbgamelibContext.Librarymedia'  is null.");
        }

        // GET: LibraryMedia/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Librarymedia == null)
            {
                return NotFound();
            }

            var librarymedium = await _context.Librarymedia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (librarymedium == null)
            {
                return NotFound();
            }

            return View(librarymedium);
        }

        // GET: LibraryMedia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryMedia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Media")] Librarymedium librarymedium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(librarymedium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(librarymedium);
        }

        // GET: LibraryMedia/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Librarymedia == null)
            {
                return NotFound();
            }

            var librarymedium = await _context.Librarymedia.FindAsync(id);
            if (librarymedium == null)
            {
                return NotFound();
            }
            return View(librarymedium);
        }

        // POST: LibraryMedia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Media")] Librarymedium librarymedium)
        {
            if (id != librarymedium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(librarymedium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrarymediumExists(librarymedium.Id))
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
            return View(librarymedium);
        }

        // GET: LibraryMedia/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Librarymedia == null)
            {
                return NotFound();
            }

            var librarymedium = await _context.Librarymedia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (librarymedium == null)
            {
                return NotFound();
            }

            return View(librarymedium);
        }

        // POST: LibraryMedia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Librarymedia == null)
            {
                return Problem("Entity set 'dbgamelibContext.Librarymedia'  is null.");
            }
            var librarymedium = await _context.Librarymedia.FindAsync(id);
            if (librarymedium != null)
            {
                _context.Librarymedia.Remove(librarymedium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibrarymediumExists(uint id)
        {
          return (_context.Librarymedia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
