using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibWeb.Controllers
{
    public class GenreController : Controller
    {
        private readonly dbgamelibContext _context;

        public GenreController(dbgamelibContext context)
        {
            _context = context;
        }

        // GET: Genre
        public async Task<IActionResult> Index()
        {
            var context = _context.Genres;
            return context != null ? 
                View(await context.ToListAsync()) :
                Problem("Entity set 'dbgamelibContext.Ratings'  is null.");
        }

        // GET: Genre/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            //List<Game> gameList = new List<Game>();
            //List<Gamegenrerelation> relations = new List<Gamegenrerelation>();
            //relations.AddRange(_context.Gamegenrerelations.Where(ggr => ggr.GenreId == genre.Id));
            
            //foreach (var relation in relations)
            //{
            //    gameList.Add(relation.Game!);
            //}

            //ViewBag.Games = gameList;
            //return View(genre);
            return RedirectToAction("Index", "Game", new {id = genre.Id, routeType = 3});
        }

        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genre/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Name")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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
            return View(genre);
        }

        // GET: Genre/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'dbgamelibContext.Genres'  is null.");
            }
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                //delete all relations with this genre
                _context.RemoveRange(_context.Gamegenrerelations.Where(ggr => ggr.GenreId == genre.Id));
                await _context.SaveChangesAsync();
                //if some games are left with no relations, delete them
                _context.RemoveRange(_context.Games.Where(g => g.Gamegenrerelations.Count == 0));
                await _context.SaveChangesAsync();
                _context.Genres.Remove(genre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(uint id)
        {
          return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
