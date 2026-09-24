namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; } //Unique ID for each movie, use as primary key
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Length { get; set; } //Length of movie in minutes. Makes the most sense to have it as Int. 
        public string Genre { get; set; } = string.Empty; //Movies will have a single genre (for now)
        public string Rating { get; set;  } = string.Empty; //Movie rating as in "PG-14" or "R", not viewer or critic score 
        public string Director { get; set; } = string.Empty;
    }
}


