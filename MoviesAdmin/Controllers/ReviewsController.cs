
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;


[Authorize]
public class ReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    public ReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }

    // GET: REVIEWS
    public async Task<IActionResult> Index()    
    {
        //Lists reviews created by user
        var reviews = await _context.Review
      .Where(r => r.CreatedBy == User.Identity!.Name)
       .Include(r => r.Movie)
      .ToListAsync();

        return View(reviews);
    }

    // GET: REVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Review
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // GET: REVIEWS/Create
    public IActionResult Create(int movieId)
    {
        ViewBag.MovieId = movieId;
        return View();
    }

    // POST: REVIEWS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,Movie,Description,Rating,IsPublished")] Review review)
    {
        //Makes the post automatically set the "created by" and time of review
        review.CreatedBy = User.Identity?.Name ?? "";
        review.CreatedDate = DateTime.Now;
        //If statement to keep user from making multiple reviews for same movie
        bool reviewExists = await _context.Review.AnyAsync(r =>
        r.MovieId == review.MovieId &&
        r.CreatedBy == review.CreatedBy);

            if (reviewExists)
            {
                ModelState.AddModelError("", "You have already reviewed this movie.");
            }




        review.CreatedDate = DateTime.Now;
        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MovieId = review.MovieId;
        return View(review);
    }

    // GET: REVIEWS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Review.FindAsync(id);
        if (review.IsPublished)
        {
            return RedirectToAction(nameof(Index));
        }
        if (review == null)
        {
            return NotFound();
        }
        return View(review);
    }

    // POST: REVIEWS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,
        [Bind("Id,MovieId,Movie,Description,Rating,IsPublished,CreatedBy,CreatedDate")] Review review)
    {
        if (id != review.Id)
        {
            return NotFound();
        }

        var existingReview = await _context.Review.FindAsync(id);

        if (existingReview != null && existingReview.IsPublished)
        {
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(review.Id))
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

        return View(review);
    }

    // GET: REVIEWS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Review.FirstOrDefaultAsync(m => m.Id == id);

        if (review.IsPublished)
        {
            return RedirectToAction(nameof(Index));
        }


        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // POST: REVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var review = await _context.Review.FindAsync(id);

        if (review != null && review.IsPublished)
        {
            return RedirectToAction(nameof(Index));
        }

        if (review != null)
        {
            _context.Review.Remove(review);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReviewExists(int? id)
    {
        return _context.Review.Any(e => e.Id == id);
    }
}
