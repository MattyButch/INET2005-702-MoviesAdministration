using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
