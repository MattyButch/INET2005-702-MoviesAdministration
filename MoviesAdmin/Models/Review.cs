using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Review

    {
        // primary key
        public int Id { get; set; }

        // foreign key
        public int MovieId { get; set; }

        // navigation property
        [ValidateNever]
        [ScaffoldColumn(false)]
        public Movie Movie { get; set; } = null!;



        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 stars

        //[Required]
        public bool IsPublished { get; set; } = false;

        public string CreatedBy { get; set; } = string.Empty;

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
