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
    public class GenreRelationController : Controller
    {
        private readonly dbgamelibContext _context;

        public GenreRelationController(dbgamelibContext context)
        {
            _context = context;
        }

        // GET: GenreRelation
        public async Task<IActionResult> Index()
        {
            var dbgamelibContext = _context.Gamegenrerelations.Include(g => g.Game).Include(g => g.Genre);
            return View(await dbgamelibContext.ToListAsync());
        }

        // GET: GenreRelation/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Gamegenrerelations == null)
            {
                return NotFound();
            }

            var gamegenrerelation = await _context.Gamegenrerelations
                .Include(g => g.Game)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamegenrerelation == null)
            {
                return NotFound();
            }

            return View(gamegenrerelation);
        }

        // GET: GenreRelation/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id");
            return View();
        }

        // POST: GenreRelation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameId,GenreId")] Gamegenrerelation gamegenrerelation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gamegenrerelation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id", gamegenrerelation.GameId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", gamegenrerelation.GenreId);
            return View(gamegenrerelation);
        }

        // GET: GenreRelation/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Gamegenrerelations == null)
            {
                return NotFound();
            }

            var gamegenrerelation = await _context.Gamegenrerelations.FindAsync(id);
            if (gamegenrerelation == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id", gamegenrerelation.GameId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", gamegenrerelation.GenreId);
            return View(gamegenrerelation);
        }

        // POST: GenreRelation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,GameId,GenreId")] Gamegenrerelation gamegenrerelation)
        {
            if (id != gamegenrerelation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gamegenrerelation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamegenrerelationExists(gamegenrerelation.Id))
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
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id", gamegenrerelation.GameId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", gamegenrerelation.GenreId);
            return View(gamegenrerelation);
        }

        // GET: GenreRelation/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Gamegenrerelations == null)
            {
                return NotFound();
            }

            var gamegenrerelation = await _context.Gamegenrerelations
                .Include(g => g.Game)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamegenrerelation == null)
            {
                return NotFound();
            }

            return View(gamegenrerelation);
        }

        // POST: GenreRelation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Gamegenrerelations == null)
            {
                return Problem("Entity set 'dbgamelibContext.Gamegenrerelations'  is null.");
            }
            var gamegenrerelation = await _context.Gamegenrerelations.FindAsync(id);
            if (gamegenrerelation != null)
            {
                // because game's index is gamegenrerelations based, if the game doesn't have any relations
                // then it is hidden, but not deleted
                
                // if the relation's game doesn't have any more relations
                if (_context.Gamegenrerelations.Where(ggr => ggr.GameId == gamegenrerelation.GameId).Count() <= 1)
                {
                    //delete it and any other hidden game with no relations
                     _context.RemoveRange(_context.Games.Where(g => g.Id == gamegenrerelation.GameId)
                         .Union(_context.Games.Where(g => g.Gamegenrerelations.Count == 0)));
                }
                _context.Gamegenrerelations.Remove(gamegenrerelation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamegenrerelationExists(uint id)
        {
          return (_context.Gamegenrerelations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
