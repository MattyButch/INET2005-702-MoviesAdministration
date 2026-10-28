using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;

public class MoviesAdminContext(DbContextOptions<MoviesAdminContext> options) : DbContext(options)
{
    public DbSet<MoviesAdmin.Models.Movie> Movie { get; set; } = default!;
    public DbSet<Genre> Genre { get; set; } = default!;
    public DbSet<AudienceRating> AudienceRating { get; set; } = default!;


}
