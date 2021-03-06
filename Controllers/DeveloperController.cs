using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibWeb.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly dbgamelibContext _context;

        public DeveloperController(dbgamelibContext context)
        {
            _context = context;
        }

        // GET: Developer
        public async Task<IActionResult> Index()
        {
            var context = _context.Developers;
            return context != null ? 
                View(await context.ToListAsync()) :
                Problem("Entity set 'dbgamelibContext.Ratings'  is null.");
        }

        // GET: Developer/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Developers == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (developer == null)
            {
                return NotFound();
            }

            //List<Game> gameList = new List<Game>();
            //gameList.AddRange(_context.Games.Where(g => g.PublisherId == developer.Id));

            //ViewBag.Games = gameList;
            
            //return View(developer);
            return RedirectToAction("Index", "Game", new {id = developer.Id, routeType = 1});
        }

        // GET: Developer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Developer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile? image ,[Bind("Id,Name,Info,Media")] Developer developer)
        {
            developer.Media = image != null ? ImageConverter.ToBase64(image) : ImageConverter.ToBase64("wwwroot/images/default.jpg");
            if (ModelState.IsValid)
            {
                _context.Add(developer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(developer);
        }

        // GET: Developer/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Developers == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers.FindAsync(id);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        // POST: Developer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, IFormFile? image, [Bind("Id,Name,Info")] Developer developer)
        {
            if (id != developer.Id)
            {
                return NotFound();
            }

            developer.Media = image != null ? ImageConverter.ToBase64(image) : ImageConverter.ToBase64("wwwroot/images/default.jpg");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(developer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperExists(developer.Id))
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
            return View(developer);
        }

        // GET: Developer/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Developers == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        // POST: Developer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Developers == null)
            {
                return Problem("Entity set 'dbgamelibContext.Developers'  is null.");
            }
            var developer = await _context.Developers.FindAsync(id);
            if (developer != null)
            {
                //games from this developer
                var devGames = _context.Games.Where(g => g.DeveloperId == developer.Id).ToList();
                foreach (var game in devGames)
                {
                    //delete relations from each game
                    _context.RemoveRange(_context.Gamegenrerelations.Where(ggr => ggr.GameId == game.Id));
                }
                await _context.SaveChangesAsync();
                
                //delete the games
                _context.RemoveRange(devGames);
                await _context.SaveChangesAsync();
                
                _context.Developers.Remove(developer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperExists(uint id)
        {
          return (_context.Developers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
