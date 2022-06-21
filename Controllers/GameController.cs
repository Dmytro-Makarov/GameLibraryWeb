using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(int? id, int? routeType)
        {
            if (id == null)
            {
                switch (routeType)
                {
                    case 1:
                        return RedirectToAction("Index", "Developer");
                    case 2:
                        return RedirectToAction("Index", "Publisher");
                    case 3:
                        return RedirectToAction("Index", "Genre");
                }
            }
            
            IQueryable<Game> db = null! ;
            switch (routeType)
            {
                case 1:
                    db = _context.Games.Where(g => g.DeveloperId == id);
                    break;
                case 2:
                    db = _context.Games.Where(g => g.PublisherId == id);
                    break;
                case 3:
                    var relations = _context.Gamegenrerelations.Where(ggr => ggr.GenreId == id).ToArray();
                    //Define an empty IQueryable
                    db = _context.Games.Where(g => false);
                    foreach (var relation in relations)
                    {
                      db = db.Concat(_context.Games.Where(g => g.Id == relation.GameId));
                    }
                    break;
                default:
                    db = _context.Games;
                    break;
            }

            var dbgamelibContext = db.
                Include(g => g.Developer).
                Include(g => g.Publisher).
                Include(g => g.Rating);
            List<Gamegenrerelation> gamegenrerelations = new List<Gamegenrerelation>();
            gamegenrerelations.AddRange(_context.Gamegenrerelations.Include(ggr => ggr.Genre));
            ViewBag.Relations = gamegenrerelations;
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
            SelectList genreSelectList = new SelectList(_context.Genres, "Id", "Name");
            
            ViewBag.DeveloperName = developerSelectList;
            ViewBag.PublisherName = publisherSelectList;
            ViewBag.GenreName = genreSelectList;
            
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile? image, int[] selectedGenres, 
            [Bind("Id, GameId, GenreId")]Gamegenrerelation relation,
            [Bind("Id")]Developer developer, 
            [Bind("Id")]Publisher publisher,
            [Bind("Id,Age")]Rating? rating,
            [Bind("Id,Name,Info,PublisherId,DeveloperId,Media")] Game game)
        {
            //Convert image to Base64
            game.Media = image != null ? ImageConverter.ToBase64(image) : ImageConverter.ToBase64("wwwroot/images/default.jpg");
            
            game.DeveloperId = developer.Id;
            game.PublisherId = publisher.Id;

            var gameDeveloper = _context.Developers.First(d=> d.Id == game.DeveloperId);
            var gamePublisher = _context.Publishers.First(p=> p.Id == game.PublisherId);
            game.Developer = gameDeveloper;
            game.Publisher = gamePublisher;
            
            //Check if there isn't a rating object with this age
            //if there isn't, add one
            //Warning: Validation is probably skipped, not like i care
            if (!_context.Ratings.Any(r => rating!.Age.Equals(r.Age)))
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                game.RatingId = rating?.Id;
                game.Rating = rating;
            }
            //if there is, find it
            else
            {
                var foundRating = _context.Ratings.First(r=> r.Age == rating!.Age);
                game.RatingId = foundRating.Id;
                game.Rating = foundRating;
            }

            if (!_context.Gamegenrerelations.Any(ggr => ggr.GameId.Equals(game.Id)))
            {
                foreach (var genreId in selectedGenres)
                {
                    //going on a while loop to avoid duplicates
                    while (_context.Gamegenrerelations.Any(ggr => ggr.Id.Equals(relation.Id)))
                    {
                        relation.Id++;
                    }

                    relation.GameId = game.Id;
                    relation.Game = game;
                    
                    relation.GenreId = (uint?) genreId;
                    relation.Genre = _context.Genres.First(gen => gen.Id == relation.GenreId);
                    
                    _context.Add(relation);
                    await _context.SaveChangesAsync();
                    game.Gamegenrerelations?.Add(relation);
                }
            }

            //If we somehow added it to DB via relations
            if (_context.Games.Any(g => g.Id == game.Id))
            {
                return RedirectToAction(nameof(Index));
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            SelectList developerSelectList = new SelectList(_context.Developers, "Id", "Name");
            SelectList publisherSelectList = new SelectList(_context.Publishers, "Id", "Name");
            SelectList genreSelectList = new SelectList(_context.Genres, "Id", "Name");
            
            ViewBag.DeveloperName = developerSelectList;
            ViewBag.PublisherName = publisherSelectList;
            ViewBag.GenreName = genreSelectList;
            
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
            
            SelectList developerSelectList = new SelectList(_context.Developers, "Id", "Name");
            SelectList publisherSelectList = new SelectList(_context.Publishers, "Id", "Name");
            SelectList genreSelectList = new SelectList(_context.Genres, "Id", "Name");
            
            ViewBag.DeveloperName = developerSelectList;
            ViewBag.PublisherName = publisherSelectList;
            ViewBag.GenreName = genreSelectList;
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, IFormFile? image, int[] selectedGenres,
            [Bind("Id, GameId, GenreId")]Gamegenrerelation relation,
            [Bind("Id")]Developer developer, 
            [Bind("Id")]Publisher publisher,
            [Bind("Id,Age")]Rating? rating,
            [Bind("Id,Name,Info,PublisherId,DeveloperId,Media")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            game.Media = image != null ? ImageConverter.ToBase64(image) : ImageConverter.ToBase64("wwwroot/images/default.jpg");
            
            game.DeveloperId = developer.Id;
            game.PublisherId = publisher.Id;

            var gameDeveloper = _context.Developers.First(d=> d.Id == game.DeveloperId);
            var gamePublisher = _context.Publishers.First(p=> p.Id == game.PublisherId);
            game.Developer = gameDeveloper;
            game.Publisher = gamePublisher;
            
            //Check if there isn't a rating object with this age
            //if there isn't, add one
            if (!_context.Ratings.Any(r => rating!.Age.Equals(r.Age)))
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                game.RatingId = rating!.Id;
                game.Rating = rating;
            }
            //if there is, find it
            else
            {
                var foundRating = _context.Ratings.First(r=> r.Age == rating!.Age);
                game.RatingId = foundRating.Id;
                game.Rating = foundRating;
            }
            
            //Check if all relations are the same as selected
            //if not, delete all but selected
            //and add selected, but not added

            if (_context.Games.Any(g => g.Id == game.Id))
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                    if (true)
                    {
                        _context.Gamegenrerelations.RemoveRange(_context.Gamegenrerelations.Where(ggr => ggr.Game!.Id == game.Id));
                        foreach (var genreId in selectedGenres)
                        {
                            while (_context.Gamegenrerelations.Any(ggr => ggr.Id.Equals(relation.Id)))
                            {
                                relation.Id++;
                            }

                            relation.GameId = game.Id;
                            relation.Game = game;
                    
                            relation.GenreId = (uint?) genreId;
                            relation.Genre = _context.Genres.First(gen => gen.Id == relation.GenreId);
                    
                            _context.Add(relation);
                            await _context.SaveChangesAsync();
                            game.Gamegenrerelations?.Add(relation);
                            _context.Update(game);
                            await _context.SaveChangesAsync();
                        }
                    }
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
            SelectList developerSelectList = new SelectList(_context.Developers, "Id", "Name");
            SelectList publisherSelectList = new SelectList(_context.Publishers, "Id", "Name");
            SelectList genreSelectList = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.DeveloperName = developerSelectList;
            ViewBag.PublisherName = publisherSelectList;
            ViewBag.GenreName = genreSelectList;
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
                //delete all relations with this game
                _context.RemoveRange(_context.Gamegenrerelations.Where(ggr => ggr.GameId == game.Id));
                await _context.SaveChangesAsync();
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
