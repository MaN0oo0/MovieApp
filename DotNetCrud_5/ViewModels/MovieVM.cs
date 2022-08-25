using DotNetCrud_5.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetCrud_5.ViewModels
{
    public class MovieVM
    {

        public int id { get; set; }
        [Required, StringLength(250, ErrorMessage = "Max Len 250 Char")]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Range(1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2500, ErrorMessage = "Max Len 2500")]
        public string StoryLine { get; set; }

   
        public byte[] Poster { get; set; }

        [Display(Name ="Catogrey")]
        public byte GenreId { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
