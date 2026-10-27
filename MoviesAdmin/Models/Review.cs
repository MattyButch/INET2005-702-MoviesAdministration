using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Review
    {
        public int Id { get; set; }
        [StringLength(5000, MinimumLength = 10)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public Boolean IsPublished { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
