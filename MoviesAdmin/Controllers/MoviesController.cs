
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
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Movie
            .OrderByDescending(m => m.ReleaseDate)
            .ToListAsync());
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
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    [Authorize]
    public IActionResult Create()
    {
        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "GenreName");

        //var ratings = _context.AudienceRating.ToList();
        //ViewBag.AudenceRatingId = new SelectList(ratings, "Id", "Title");

        var ratings = _context.AudienceRating.ToList();

        Console.WriteLine($"Ratings Count: {ratings.Count}");

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
    [Authorize]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Length,Rating,AudienceRatingId,GenreId,ReleaseDate")] Movie movie, IFormFile imageFile) //xollects inputs and file input from form
    {
        if (ModelState.IsValid)
        {


            //Save file in Azure storage
            string imageURL = await _blobService.UploadFileAsync(imageFile, imageFile.FileName);

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
    [Authorize]
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
        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Description,Length,Rating,AudienceRatingId,ReleaseDate,ImageUrl")] Movie movie)
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
    [Authorize]
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
    [Authorize]
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
