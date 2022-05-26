using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibWeb;
using Microsoft.Data.SqlClient.Server;

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
            var dbgamelibContext = _context.Games.Include(g => g.Developer).Include(g => g.Publisher).Include(g => g.Rating);
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
            SelectList developerSelectList = new SelectList(_context.Developers, "Id", "Name");
            SelectList publisherSelectList = new SelectList(_context.Publishers, "Id", "Name");
            SelectList ratingSelectList = new SelectList(_context.Ratings, "Age", "RatingAge");
            ViewBag.DeveloperName = developerSelectList;
            ViewBag.PublisherName = publisherSelectList;
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile image, 
            [Bind("Id, GameId, GenreId")]Gamegenrerelation relation,
            [Bind("Name")]Developer developer, 
            [Bind("Name")]Publisher publisher,
            [Bind("Id,Age")]Rating rating,
            [Bind("Id,Name,Info,PublisherId,DeveloperId,Media")] Game game)
        {
            //Convert image to Base64
            game.Media = ImageConverter.ToBase64(image);
            //idk why the foreign key is in name don't mention it
            game.DeveloperId = uint.Parse(developer.Name);
            game.PublisherId = uint.Parse(publisher.Name);

            var gameDeveloper = _context.Developers.First(d=> d.Id == game.DeveloperId);
            game.Developer = gameDeveloper;
            var gamePublisher = _context.Publishers.First(p=> p.Id == game.PublisherId);
            game.Publisher = gamePublisher;

            /*
            if (!_context.Gamegenrerelations.Any(ggr => ggr.GameId.Equals(game.Id)))
            {
                relation.GameId = game.Id;
                relation.GenreId = 1;
                
                relation.Game = game;
                relation.Genre = _context.Genres.First(gen => gen.Id == relation.GenreId);
                _context.Add(relation);
                await _context.SaveChangesAsync();
            }
            */
            //Check if there isn't a rating object with this age
            //if there isn't, add one
            if (!_context.Ratings.Any(r => rating.Age.Equals(r.Age)))
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                game.RatingId = rating.Id;
            }
            //if there is, find it
            else
            {
                var ratingId = _context.Ratings.First(r=> r.Age == rating.Age).Id;
                game.RatingId = ratingId;
            }
            //TODO: I guess this shithead lacks genres, simultaneously create and add genre relation
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            SelectList developerSelectList = new SelectList(_context.Developers, "Id", "Name");
            SelectList publisherSelectList = new SelectList(_context.Publishers, "Id", "Name");
            SelectList ratingSelectList = new SelectList(_context.Ratings, "Age", "RatingAge");
            ViewBag.DeveloperName = developerSelectList;
            ViewBag.PublisherName = publisherSelectList;
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
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", game.PublisherId);
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "Id", game.RatingId);
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Name,Info,PublisherId,DeveloperId,RatingId,Media")] Game game)
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
