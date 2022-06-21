using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibWeb.Controllers
{
    public class PublisherController : Controller
    {
        private readonly dbgamelibContext _context;

        public PublisherController(dbgamelibContext context)
        {
            _context = context;
        }

        // GET: Publisher
        public async Task<IActionResult> Index()
        {
            var context = _context.Publishers;
            return context != null ? 
                View(await context.ToListAsync()) :
                Problem("Entity set 'dbgamelibContext.Ratings'  is null.");
        }

        // GET: Publisher/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Publishers == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            //List<Game> gameList = new List<Game>();
            //gameList.AddRange(_context.Games.Where(g => g.PublisherId == publisher.Id));

            //ViewBag.Games = gameList;
            //return View(publisher);
            return RedirectToAction("Index", "Game", new {id = publisher.Id, routeType = 2});
        }

        // GET: Publisher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile? image ,[Bind("Id,Name,Info,Media")] Publisher publisher)
        {
            publisher.Media = image != null ? ImageConverter.ToBase64(image) : ImageConverter.ToBase64("wwwroot/images/default.jpg");
            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publisher/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Publishers == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Publisher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, IFormFile? image, [Bind("Id,Name,Info")] Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return NotFound();
            }

            publisher.Media = image != null ? ImageConverter.ToBase64(image) : ImageConverter.ToBase64("wwwroot/images/default.jpg");
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id))
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
            return View(publisher);
        }

        // GET: Publisher/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Publishers == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publisher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Publishers == null)
            {
                return Problem("Entity set 'dbgamelibContext.Publishers'  is null.");
            }
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                //games from this developer
                var pubGames = _context.Games.Where(g => g.PublisherId == publisher.Id).ToList();
                foreach (var game in pubGames)
                {
                    //delete relations from each game
                    _context.RemoveRange(_context.Gamegenrerelations.Where(ggr => ggr.GameId == game.Id));
                }
                await _context.SaveChangesAsync();

                //delete the games
                _context.RemoveRange(pubGames);
                await _context.SaveChangesAsync();
                
                _context.Publishers.Remove(publisher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(uint id)
        {
          return (_context.Publishers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
