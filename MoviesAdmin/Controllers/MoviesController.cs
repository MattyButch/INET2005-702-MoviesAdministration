
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using MoviesAdmin.Services;

public class MoviesController : Controller
{
    //Add authorize 
    private readonly MoviesAdminContext _context;
    private readonly AzureStorageService _blobService;

    public MoviesController(MoviesAdminContext context, AzureStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: MOVIES
    [Authorize]
    public async Task<IActionResult> Index(string sortOrder = "date",int page = 1, int? genreId = null)
    {
        const int pageSize = 10;

        var query = _context.Movie.AsQueryable();
        //Genre query
        if (genreId.HasValue)
        {
            query = query.Where(m => m.GenreId == genreId.Value);
        }

        // Sorting
        if (sortOrder == "title")
        {
            query = query.OrderBy(m => m.Title);
        }
        else
        {
            query = query.OrderByDescending(m => m.ReleaseDate);
        }

        // Count total movies
        int totalMovies = await query.CountAsync();

        var movies = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        // ViewBag values for paging/sorting controls
        ViewBag.CurrentSort = sortOrder;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalMovies / (double)pageSize);
        ViewBag.CurrentGenre = genreId;

        ViewBag.Genres = await _context.Genre.OrderBy(g => g.GenreName).ToListAsync();


        return View(movies);
    }

    // GET: MOVIES/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            //Adds reviews to the movie details
            .Include(m => m.Genre)
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    [Authorize]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "GenreName");

        //var ratings = _context.AudienceRating.ToList();
        //ViewBag.AudenceRatingId = new SelectList(ratings, "Id", "Title");

        var ratings = _context.AudienceRating.ToList();

        //Console.WriteLine($"Ratings Count: {ratings.Count}");

        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Title");




        return View();
    }


    /*
     * TO-DO handle image being required
     */

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Length,Rating,AudienceRatingId,GenreId,ReleaseDate")] Movie movie, IFormFile imageFile) //xollects inputs and file input from form
    {
        if (ModelState.IsValid)
        {
            
            //Default rating is three. 
            movie.AudienceRatingId = 3;

            // Generate unique filename
            string extension = Path.GetExtension(imageFile.FileName);
            string guidFile = $"{Guid.NewGuid()}{extension}";

            //Save file in Azure storage
            string imageURL = await _blobService.UploadFileAsync(imageFile, guidFile);

            // Save image Url in Trail record
            movie.ImageUrl = imageURL;

            //Save a record in database
            _context.Add(movie);
            await _context.SaveChangesAsync();

            //Redirect to index
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(
            genres,
            "Id",
            "GenreName",
            movie.GenreId
        );

        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Description,Length,Rating,GenreId,AudienceRatingId,ReleaseDate,ImageUrl")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
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
        return View(movie);
    }

    // GET: MOVIES/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
