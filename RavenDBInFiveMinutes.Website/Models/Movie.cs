using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RavenDBInFiveMinutes.Website.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description{ get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
        
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
    }
}