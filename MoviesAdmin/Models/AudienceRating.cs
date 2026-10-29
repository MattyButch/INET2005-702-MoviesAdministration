using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        [ValidateNever]
        public List<Movie> Movies { get; set; } = new();
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
