using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
