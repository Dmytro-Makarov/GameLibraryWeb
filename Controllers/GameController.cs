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
    public class GameController : Controller
    {
        private readonly dbgamelibContext _context;

        public GameController(dbgamelibContext context)
        {
            _context = context;
        }

        // GET: Game
        public async Task<IActionResult> Index()
        {
            var dbgamelibContext = _context.Games.Include(g => g.Developer).Include(g => g.LibraryMedia).Include(g => g.Publisher).Include(g => g.Rating);
            return View(await dbgamelibContext.ToListAsync());
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.LibraryMedia)
                .Include(g => g.Publisher)
                .Include(g => g.Rating)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Game/Create
        public IActionResult Create()
        {
            ViewData["Developer.Name"] = new SelectList(_context.Developers, "Name", "DeveloperName");
            //Sends request to get ids for a list
            //ViewData["LibraryMedia.Id"] = new SelectList(_context.Librarymedia, "Id", "Id");
            ViewData["Publisher.Name"] = new SelectList(_context.Publishers, "Name", "PublisherName");
            ViewData["Rating.Age"] = new SelectList(_context.Ratings, "Age", "RatingAge");
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info,PublisherId,DeveloperId,RatingId,LibraryMediaId")] Game game)
        {
            //ViewData["LibraryMedia.Media"]
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Developer.Name"] = new SelectList(_context.Developers, "Name", "DeveloperName");
            //Sends request to get ids for a list
            //ViewData["LibraryMedia.Id"] = new SelectList(_context.Librarymedia, "Id", "Id");
            ViewData["Publisher.Name"] = new SelectList(_context.Publishers, "Name", "PublisherName");
            ViewData["Rating.Age"] = new SelectList(_context.Ratings, "Age", "RatingAge");
            return View(game);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "Id", "Id", game.DeveloperId);
            ViewData["LibraryMediaId"] = new SelectList(_context.Librarymedia, "Id", "Id", game.LibraryMediaId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", game.PublisherId);
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "Id", game.RatingId);
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Name,Info,PublisherId,DeveloperId,RatingId,LibraryMediaId")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "Id", "Id", game.DeveloperId);
            ViewData["LibraryMediaId"] = new SelectList(_context.Librarymedia, "Id", "Id", game.LibraryMediaId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", game.PublisherId);
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "Id", game.RatingId);
            return View(game);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.LibraryMedia)
                .Include(g => g.Publisher)
                .Include(g => g.Rating)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'dbgamelibContext.Games'  is null.");
            }
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(uint id)
        {
          return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
