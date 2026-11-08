using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; } //Unique ID for each movie, use as primary key

        //naviagation property

        //navigation property
        public List<Review> Reviews { get; set; } = new List<Review>();

        //
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [ValidateNever]
        public Genre Genre { get; set; } = null!;

        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string Title { get; set; } = string.Empty;
        [StringLength(1000, MinimumLength = 1)]
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range (0, 60000)]
        [Display(Name = "Length (minutes)")]
        public int Length { get; set; } //Length of movie in minutes.


        //TO DO REMOVE 
        [Required]
        [Display(Name = "Movie Rating")]
        public string Rating { get; set; } = string.Empty; //Movie rating as in "PG-14" or "R", not viewer or critic score 
        [Required]


        //Audience Rating
        [Display(Name = "Audience Rating")]
        public int AudienceRatingId { get; set; }

        [ValidateNever]
        public AudienceRating AudienceRating { get; set; } = null!;
        //Url to image stored in Azure storage
        public string ImageUrl { get; set; } = string.Empty;
        [Display(Name = "Release Date")]

        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now; 
    }
}


//using System.ComponentModel.DataAnnotations;