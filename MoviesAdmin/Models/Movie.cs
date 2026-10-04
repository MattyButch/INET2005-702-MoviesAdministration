using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; } //Unique ID for each movie, use as primary key

        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string Title { get; set; } = string.Empty;
        [StringLength(1000, MinimumLength = 1)]
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range (0, 60000)]
        public int Length { get; set; } //Length of movie in minutes. Makes the most sense to have it as Int. 
        [Required]
        public string Genre { get; set; } = string.Empty; //Movies will have a single genre (for now)
        [Required]
        public string Rating { get; set; } = string.Empty; //Movie rating as in "PG-14" or "R", not viewer or critic score 

        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now; 
    }
}


//using System.ComponentModel.DataAnnotations;